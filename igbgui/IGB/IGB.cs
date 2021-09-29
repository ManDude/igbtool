using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using igbgui.Objects;

namespace igbgui
{
    public sealed class IGB
    {
        private static Dictionary<string, Type> igbFieldTypes = new();
        private static Dictionary<string, Type> igbObjectTypes = new();
        static IGB()
        {
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == "igbgui.Fields"
                    select t;
            q.ToList().ForEach(t => igbFieldTypes.Add(t.IsGenericType ? t.Name.Remove(t.Name.IndexOf('`')) : t.Name, t));
            q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == "igbgui.Objects"
                    select t;
            q.ToList().ForEach(t => igbObjectTypes.Add(t.IsGenericType ? t.Name.Remove(t.Name.IndexOf('`')) : t.Name, t));
        }

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
                throw new Exception("invalid magic number");
            uint version = BitConverter.ToUInt32(data, seek+4);
            seek += 8;

            // TYPES
            var type_name_lengths = new int[counts[4]];
            var type_bools = new bool[counts[4]];
            for (int i = 0; i < counts[4]; ++i)
            {
                type_name_lengths[i] = BitConverter.ToInt32(data, seek + 0);
                var booltemp = BitConverter.ToUInt32(data, seek + 4);
                if (booltemp != 0 && booltemp != 1)
                    throw new Exception(string.Format("type bool is invalid value {0}", booltemp));
                type_bools[i] = BitConverter.ToBoolean(data, seek + 4);
                var unkval = BitConverter.ToUInt32(data, seek + 8);
                if (unkval != 0)
                    throw new Exception(string.Format("type unknown value is invalid value {0}", unkval));
                seek += 12;
            }
            var types = new IgbType[counts[4]];
            for (int i = 0; i < types.Length; ++i)
            {
                var name = Encoding.UTF8.GetString(data, seek, type_name_lengths[i]).TrimNull();
                types[i] = new IgbType
                {
                    Name = name,
                    Bool = type_bools[i]
                };
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
                IgbStruct s = new();
                s.ID = i;
                s.Name = name;
                s.Master = BitConverter.ToBoolean(data, structdefsk + 4);
                var reserved = BitConverter.ToInt32(data, structdefsk + 8);
                if (reserved != 0)
                {
                    throw new Exception(string.Format("struct unknown value is invalid value {0}", reserved));
                }
                s.UnkSize = BitConverter.ToInt32(data, structdefsk + 20);
                var parent = BitConverter.ToInt32(data, structdefsk + 16);
                s.Parent = parent == -1 ? null : structs[parent];
                var fieldc = BitConverter.ToInt32(data, structdefsk + 12);
                for (int ii = 0; ii < fieldc; ++ii)
                {
                    s.Fields.Add(new IgbFieldInfo
                    {
                        Type = types[BitConverter.ToInt16(data, seek + 0)],
                        UnkOff = BitConverter.ToInt16(data, seek + 2),
                        Size = BitConverter.ToInt16(data, seek + 4)
                    });
                    seek += 6;
                }
                structs[i] = s;

                structdefsk += 24;
            }

            // REFS
            var refs = new IgbRefType[counts[0]];
            var objs = new List<IgbObjectRef>();
            var mems = new List<IgbMemoryRef>();
            for (int i = 0; i < refs.Length; ++i)
            {
                var type = BitConverter.ToInt32(data, seek+0);
                var size = BitConverter.ToInt32(data, seek+4);
                var unk = BitConverter.ToInt32(data, seek+8);
                if (unk != 0)
                {
                    throw new Exception(string.Format("ref unknown value is invalid value {0}", unk));
                }
                if (type == 3)
                {
                    var obj = new IgbObjectRef(i)
                    {
                        Struct = structs[BitConverter.ToInt32(data, seek + 12)]
                    };
                    refs[i] = obj;
                    objs.Add(obj);
                }
                else if (type == 4)
                {
                    var mem = new IgbMemoryRef(i)
                    {
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
                    throw new Exception(string.Format("unknown ref type {0}", type));
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
                    throw new Exception(string.Format("object {0} struct mismatch", i));
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

            return new IGB(version, types, shader_types, shaderval, structs, refs, topobj);
        }

        public IGB(uint version, IEnumerable<IgbType> types, IEnumerable<IgbShaderType> shadertypes, int shaderval, IEnumerable<IgbStruct> structs, IEnumerable<IgbRefType> refs, int top)
        {
            ShaderVal = shaderval;
            Version = version;
            Types = new(types);
            ShaderTypes = new(shadertypes);
            Structs = new(structs);
            Refs = new(refs);
            CurID = Refs.Count;

            Top = GetRef<igInfoList>(top);
        }

        private int CurID { get; set; }
        public uint Version { get; set; }
        public List<IgbType> Types { get; set; }
        public List<IgbShaderType> ShaderTypes { get; set; }
        public int ShaderVal { get; set; }
        public List<IgbStruct> Structs { get; set; }
        public HashSet<IgbRefType> Refs { get; set; }
        public List<IgbObject> Objects { get; } = new();
        public List<IgbMemory<IgbField>> Memorys { get; } = new();
        public IgbObject Top { get; set; }

        public T GetRef<T>(int idx) where T : IgbEntity
        {
            if (idx == -1) return null;
            IgbRefType res = null;
            foreach (var r in Refs)
            {
                if (r.ID == idx)
                {
                    res = r;
                    break;
                }
            }
            if (res is IgbObjectRef objref && igbObjectTypes.ContainsKey(objref.Struct.Name) && typeof(T).IsSubclassOf(typeof(IgbObject)))
            {
                var type = igbObjectTypes[objref.Struct.Name];
                if (type.Name == typeof(T).Name || type.IsSubclassOf(typeof(T)))
                {
                    Refs.Remove(res);
                    T obj;
                    if (type.IsGenericType)
                    {
                        obj = Activator.CreateInstance(type.MakeGenericType(typeof(T).GetGenericArguments()), this, objref) as T;
                    }
                    else
                    {
                        obj = Activator.CreateInstance(type, this, objref) as T;
                    }
                    Objects.Add(obj as IgbObject);
                    return obj;
                }
                else
                {
                    throw new Exception();
                }
            }
            else if (res is IgbMemoryRef memref && igbFieldTypes.ContainsKey(memref.Type.Name))
            {
                var type = igbFieldTypes[memref.Type.Name];
                var memtype = typeof(T).GetGenericArguments()[0];
                if (type.Name == memtype.Name)
                {
                    return Activator.CreateInstance(typeof(T), this, memref) as T;
                }
                else
                {
                    throw new Exception();
                }
            }
            return null;
            throw new Exception();
        }
    }
}
