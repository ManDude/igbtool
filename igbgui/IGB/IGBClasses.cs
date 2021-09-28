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

        protected bool ReadBool() => BitConverter.ToBoolean(Parent.Data, Offset);
        protected int ReadInt() => BitConverter.ToInt32(Parent.Data, Offset);
        protected string ReadString() => Encoding.UTF8.GetString(Parent.Data, Offset+4, BitConverter.ToInt32(Parent.Data, Offset)).TrimNull();
        protected float ReadFloat() => BitConverter.ToSingle(Parent.Data, Offset);
        protected IgbMemory ReadMemRef() => Parent.IGB.GetRef<IgbMemory>(BitConverter.ToInt32(Parent.Data, Offset));
        protected IgbObject ReadObjRef() => Parent.IGB.GetRef<IgbObject>(BitConverter.ToInt32(Parent.Data, Offset));
        protected Vector3 ReadVec3f()
        {
            var ofs = Offset;
            return new Vector3(BitConverter.ToSingle(Parent.Data, ofs + 0), BitConverter.ToSingle(Parent.Data, ofs + 4), BitConverter.ToSingle(Parent.Data, ofs + 8));
        }
        protected Vector4 ReadVec4f()
        {
            var ofs = Offset;
            return new Vector4(BitConverter.ToSingle(Parent.Data, ofs + 0), BitConverter.ToSingle(Parent.Data, ofs + 4), BitConverter.ToSingle(Parent.Data, ofs + 8), BitConverter.ToSingle(Parent.Data, ofs + 12));
        }
        protected void Write(bool val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(byte val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(sbyte val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(short val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(ushort val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(int val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(uint val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(long val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(ulong val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(float val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(double val) => BitUtils.WriteBytes(Parent.Data, Offset, val);
        protected void Write(IgbRefType val) => BitUtils.WriteBytes(Parent.Data, Offset, Parent.IGB.Refs.IndexOf(val));
        protected void Write(Vector3 val)
        {
            var ofs = Offset;
            BitUtils.WriteBytes(Parent.Data, ofs+0, val.X);
            BitUtils.WriteBytes(Parent.Data, ofs+4, val.Y);
            BitUtils.WriteBytes(Parent.Data, ofs+8, val.Z);
        }
        protected void Write(Vector4 val)
        {
            var ofs = Offset;
            BitUtils.WriteBytes(Parent.Data, ofs + 0, val.X);
            BitUtils.WriteBytes(Parent.Data, ofs + 4, val.Y);
            BitUtils.WriteBytes(Parent.Data, ofs + 8, val.Z);
            BitUtils.WriteBytes(Parent.Data, ofs + 12, val.W);
        }
        protected void Write(string val)
        {
            // this fucking sucks
            var ofs = Offset;
            var len = Encoding.UTF8.GetByteCount(val);
            var oldlen = ReadInt();
            var alen = BitUtils.Align(len, 4);
            var aoldlen = BitUtils.Align(oldlen, 4);
            if (alen < aoldlen)
            {
                // new string is shorter
                Array.Copy(Parent.Data, ofs + aoldlen, Parent.Data, ofs + alen, Parent.Data.Length - (ofs + aoldlen));
                Parent.Resize(Parent.Data.Length - (aoldlen - alen));
            }
            else if (alen > aoldlen)
            {
                // new string is longer
                Parent.Resize(Parent.Data.Length - (aoldlen - alen));
                Array.Copy(Parent.Data, ofs + aoldlen, Parent.Data, ofs + alen, Parent.Data.Length - (ofs + aoldlen));
            }
            BitUtils.WriteBytes(Parent.Data, ofs, len);
            BitUtils.WriteBytes(Parent.Data, ofs+4, val);
        }

        public IgbField() { }
        public IgbField(IgbObject parent, int index)
        {
            Parent = parent;
            Index = index;

            var this_t = GetType();
            var src_type = Parent.Struct.Fields[index].Type.Name;
            var new_type = this_t.IsGenericType ? this_t.Name.Remove(this_t.Name.IndexOf('`')) : this_t.Name;
            if (src_type != new_type)
            {
                throw new Exception(string.Format("field type mismatch on {2}: {0} vs. {1}", src_type, new_type, Parent.GetType().Name));
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

        public IGB IGB { get; set; }

        public void Resize(int sz)
        {
            var data = Data;
            Array.Resize(ref data, sz);
        }

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
