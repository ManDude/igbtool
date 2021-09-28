using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace igbgui
{
    public class IgbType
    {
        public string Name { get; set; }
        public bool Bool { get; set; }
    }

    public class IgbShaderType
    {
        public string Name { get; set; }
    }

    public class IgbFieldInfo
    {
        public IgbType Type { get; set; }
        public short UnkOff { get; set; }
        public short Size { get; set; }
    }

    public class IgbField
    {
        public IgbObject Parent { get; set; }
        public int Index { get; }
        public int Offset
        {
            get
            {
                int o = 0;
                for (int i = 0; i < Parent.Struct.Fields.Count; ++i)
                {
                    if (i == Index) break;
                    var field = Parent.Struct.Fields[i];
                    if (field.Type.Name == "igStringMetaField")
                    {
                        o += BitConverter.ToInt32(Parent.Data, o);
                    }
                    o += field.Size;
                    o = BitUtils.Align(o, 4);
                }
                return o;
            }
        }

        public int ReadInt(byte[] data) => BitConverter.ToInt32(data, Offset);
        public string ReadString(byte[] data) => Encoding.UTF8.GetString(data, Offset+4, BitConverter.ToInt32(data, Offset)).TrimNull();
        public float ReadFloat(byte[] data) => BitConverter.ToSingle(data, Offset);
        public Vector3 ReadVec3f(byte[] data)
        {
            var ofs = Offset;
            return new Vector3(BitConverter.ToSingle(data, ofs + 0), BitConverter.ToSingle(data, ofs + 4), BitConverter.ToSingle(data, ofs + 8));
        }
        public Vector4 ReadVec4f(byte[] data)
        {
            var ofs = Offset;
            return new Vector4(BitConverter.ToSingle(data, ofs + 0), BitConverter.ToSingle(data, ofs + 4), BitConverter.ToSingle(data, ofs + 8), BitConverter.ToSingle(data, ofs + 12));
        }
        public void Write(byte[] data, int val) => BitUtils.WriteBytes(data, Offset, val);
        public void Write(byte[] data, Vector3 val)
        {
            var ofs = Offset;
            BitUtils.WriteBytes(data, ofs+0, val.X);
            BitUtils.WriteBytes(data, ofs+4, val.Y);
            BitUtils.WriteBytes(data, ofs+8, val.Z);
        }
        public void Write(byte[] data, Vector4 val)
        {
            var ofs = Offset;
            BitUtils.WriteBytes(data, ofs + 0, val.X);
            BitUtils.WriteBytes(data, ofs + 4, val.Y);
            BitUtils.WriteBytes(data, ofs + 8, val.Z);
            BitUtils.WriteBytes(data, ofs + 12, val.W);
        }
        public void Write(byte[] data, string val)
        {
            // this fucking sucks
            var len = Encoding.UTF8.GetByteCount(val);
            var oldlen = ReadInt(data);
            var alen = BitUtils.Align(len, 4);
            var aoldlen = BitUtils.Align(oldlen, 4);
            var ofs = Offset;
            if (alen < aoldlen)
            {
                // new string is shorter
                Array.Copy(data, ofs + aoldlen, data, ofs + alen, data.Length - (ofs + aoldlen));
                Array.Resize(ref data, data.Length - (aoldlen - alen));
            }
            else if (alen > aoldlen)
            {
                // new string is longer
                Array.Resize(ref data, data.Length - (aoldlen - alen));
                Array.Copy(data, ofs + aoldlen, data, ofs + alen, data.Length - (ofs + aoldlen));
            }
            BitUtils.WriteBytes(data, ofs, len);
            BitUtils.WriteBytes(data, ofs+4, val);
        }

        public IgbField() { }
        public IgbField(IgbObject parent, int index)
        {
            Parent = parent;
            Index = index;

            var src_field = Parent.Struct.Fields[index];
            if (src_field.Type.Name != GetType().Name)
            {
                throw new Exception(string.Format("field type mismatch on {2}: {0} vs. {1}", src_field.Type.Name, GetType().Name, Parent.GetType().Name));
            }
        }
    }

    public class IgbStruct
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Master { get; set; }
        public IgbStruct Parent { get; set; }
        public int UnkSize { get; set; }
        public List<IgbFieldInfo> Fields { get; } = new();
    }

    public class IgbRefType
    {
    }

    public class IgbObject : IgbRefType
    {
        public IgbStruct Struct { get; }
        public byte[] Data { get; set; }

        public IgbObject(IgbStruct s)
        {
            Struct = s;
        }
    }

    public class IgbMemory : IgbRefType
    {
        public IgbType Type { get; set; }
        public byte Unk1 { get; set; }
        public int Unk2 { get; set; }
        public byte[] Data { get; set; }
    }
}
