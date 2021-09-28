using System;
using System.Collections.Generic;
using System.Text;

namespace igbgui
{
    public sealed class IGB
    {

        public static IGB Load(byte[] data)
        {
            int seek = 0;

            // HEADER
            var sizes = new int[5];
            var counts = new int[5];
            for (int i = 0; i < 5; ++i)
            {
                sizes[i] = BitConverter.ToInt32(data, seek+0);
                counts[i] = BitConverter.ToInt32(data, seek+4);
                seek += 8;
            }
            uint magic = BitConverter.ToUInt32(data, seek);
            if (magic != 0xFADA)
                throw new ArgumentException("invalid magic number");
            uint version = BitConverter.ToUInt32(data, seek+4);
            seek += 8;

            // TYPES
            var types = new IgbType[counts[4]];
            var type_name_lengths = new int[types.Length];
            for (int i = 0; i < types.Length; ++i)
            {
                types[i] = new();
                type_name_lengths[i] = BitConverter.ToInt32(data, seek + 0);
                var booltemp = BitConverter.ToUInt32(data, seek + 4);
                if (booltemp != 0 && booltemp != 1)
                    throw new ArgumentException(string.Format("type bool is invalid value {0}", booltemp));
                types[i].Bool = BitConverter.ToBoolean(data, seek + 4);
                var unkval = BitConverter.ToUInt32(data, seek + 8);
                if (unkval != 0)
                    throw new ArgumentException(string.Format("type unknown value is invalid value {0}", unkval));
                seek += 12;
            }
            for (int i = 0; i < types.Length; ++i)
            {
                types[i].Name = Encoding.UTF8.GetString(data, seek, type_name_lengths[i]).TrimNull();
                seek += type_name_lengths[i];
            }

            // SHADER TYPES
            var shadersecsize = BitConverter.ToInt32(data, seek + 0);
            var shaderval = BitConverter.ToInt32(data, seek + 4);
            var shaderamt = BitConverter.ToInt32(data, seek + 8);
            seek += 12;
            int shaderlensk = seek;
            seek += 4 * shaderamt;

            var shader_types = new IgbShaderType[shaderamt];
            for (int i = 0; i < shader_types.Length; ++i)
            {
                int len = BitConverter.ToInt32(data, shaderlensk + 4 * i);
                shader_types[i] = new();
                shader_types[i].Name = Encoding.UTF8.GetString(data, seek, len).TrimNull();
                seek += len;
            }

            // STRUCTS
            int structdefsk = seek;
            seek += 24 * counts[1];
            var structs = new IgbStruct[counts[1]];
            for (int i = 0; i < structs.Length; ++i)
            {
                var len = BitConverter.ToInt32(data, structdefsk + 0);
                var name = Encoding.UTF8.GetString(data, seek, len).TrimNull();
                seek += len;
                structs[i] = new();
                structs[i].ID = i;
                structs[i].Name = name;
                structs[i].Master = BitConverter.ToBoolean(data, structdefsk + 4);
                var reserved = BitConverter.ToInt32(data, structdefsk + 8);
                if (reserved != 0)
                {
                    throw new ArgumentException(string.Format("struct unknown value is invalid value {0}", reserved));
                }
                structs[i].UnkSize = BitConverter.ToInt32(data, structdefsk + 20);
                var parent = BitConverter.ToInt32(data, structdefsk + 16);
                structs[i].Parent = parent == -1 ? null : structs[parent];
                var fieldc = BitConverter.ToInt32(data, structdefsk + 12);
                for (int ii = 0; ii < fieldc; ++ii)
                {
                    var field = new IgbField
                    {
                        Type = types[BitConverter.ToInt16(data, seek + 0)],
                        UnkOff = BitConverter.ToInt16(data, seek + 2),
                        Size = BitConverter.ToInt16(data, seek + 4)
                    };
                    structs[i].Fields.Add(field);
                    seek += 6;
                }

                structdefsk += 24;
            }

            // REFS
            var refs = new IgbRefType[counts[0]];
            var objs = new List<IgbObject>();
            var mems = new List<IgbMemory>();
            for (int i = 0; i < refs.Length; ++i)
            {
                var type = BitConverter.ToInt32(data, seek+0);
                var size = BitConverter.ToInt32(data, seek+4);
                var unk = BitConverter.ToInt32(data, seek+8);
                if (unk != 0)
                {
                    throw new ArgumentException(string.Format("ref unknown value is invalid value {0}", unk));
                }
                if (type == 3)
                {
                    var obj = new IgbObject
                    {
                        RefID = i,
                        Struct = structs[BitConverter.ToInt32(data, seek + 12)]
                    };
                    refs[i] = obj;
                    objs.Add(obj);
                }
                else if (type == 4)
                {
                    var mem = new IgbMemory
                    {
                        RefID = i,
                        Type = types[BitConverter.ToInt32(data, seek + 16)],
                        Data = new byte[BitConverter.ToInt32(data, seek + 12)],
                        Unk1 = data[seek + 20],
                        Unk2 = BitConverter.ToInt32(data, seek + 24)
                    };
                    refs[i] = mem;
                    mems.Add(mem);
                }
                else
                {
                    throw new ArgumentException(string.Format("unknown ref type {0}", type));
                }
                seek += size;
            }

            int topobj = -1;
            if (version == 0x80000005)
            {
                topobj = BitConverter.ToInt32(data, seek);
                seek += 4;
            }

            // OBJECTS
            var objsk = seek;
            for (int i = 0; i < counts[2]; ++i)
            {
                var obj = objs[i];
                if (obj.Struct != structs[BitConverter.ToInt32(data, seek)])
                {
                    throw new ArgumentException(string.Format("object {0} struct mismatch", i));
                }
                var sz = BitConverter.ToInt32(data, seek + 4);
                obj.Data = new byte[sz - 8];
                Array.Copy(data, seek + 8, obj.Data, 0, obj.Data.Length);
                seek += sz;
            }

            // MEMORY
            var memsk = seek;
            for (int i = 0; i < counts[3]; ++i)
            {
                var mem = mems[i];
                Array.Copy(data, seek, mem.Data, 0, mem.Data.Length);
                seek = memsk + BitUtils.Align((seek-memsk) + mem.Data.Length, 4);
            }

            if (version == 0x80000004)
            {
                topobj = BitConverter.ToInt32(data, seek);
                seek += 4;
            }

            return new IGB(version, types, shader_types, shaderval, structs, refs, objs, mems, topobj);
        }

        public IGB(uint version, IEnumerable<IgbType> types, IEnumerable<IgbShaderType> shadertypes, int shaderval, IEnumerable<IgbStruct> structs, IEnumerable<IgbRefType> refs, IEnumerable<IgbObject> objs, IEnumerable<IgbMemory> mems, int top)
        {
            Version = version;
            Types = new(types);
            ShaderTypes = new(shadertypes);
            Structs = new(structs);
            Refs = new(refs);
            Objects = new(objs);
            Memorys = new(mems);

            ShaderVal = shaderval;
            Top = GetRef<IgbObject>(top);
        }

        public uint Version { get; set; }
        public List<IgbType> Types { get; set; }
        public List<IgbShaderType> ShaderTypes { get; set; }
        public int ShaderVal { get; set; }
        public List<IgbStruct> Structs { get; set; }
        public List<IgbRefType> Refs { get; set; }
        public List<IgbObject> Objects { get; set; }
        public List<IgbMemory> Memorys { get; set; }
        public IgbObject Top { get; set; }

        public T GetRef<T>(int idx) where T : IgbRefType
        {
            if (idx == -1) return null;
            return Refs[idx] as T;
        }
    }
}
