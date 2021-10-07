using System;
using System.Collections.Generic;
using System.Reflection;

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

    public abstract class IgbField
    {
        public IgbEntity Parent { get; set; }
        public int Index { get; }

        public static int Size => throw new Exception();

        protected int GetOffset(byte[] data)
        {
            if (Parent is IgbObject obj)
            {
                int o = 0;
                for (int i = 0; i < obj.Struct.Fields.Count; ++i)
                {
                    if (i == Index) break;
                    var field = obj.Struct.Fields[i];
                    if (field.Type.Name == "igStringMetaField")
                    {
                        o += BitConverter.ToInt32(data, o);
                    }
                    o += field.Size;
                    o = BitUtils.Align(o, 4);
                }
                return o;
            }
            throw new Exception();
        }

        public IgbField(IgbEntity parent, int index)
        {
            Parent = parent;
            Index = index;

            if (Parent is IgbObject obj)
            {
                var this_t = GetType();
                var src_type = obj.Struct.Fields[index].Type.Name;
                var new_type = this_t.IsGenericType ? this_t.Name.Remove(this_t.Name.IndexOf('`')) : this_t.Name;
                if (src_type != new_type)
                {
                    throw new Exception(string.Format("field type mismatch on {2}: {0} vs. {1}", src_type, new_type, obj.GetType().Name));
                }
            }
        }
    }

    public abstract class IgbClassField<T> : IgbField where T : class
    {
        public T Value { get; set; }

        public IgbClassField(IgbObject parent, IgbObjectRef info, int index, Func<byte[], int, T> read) : base(parent, index)
        {
            if (read != null)
                Value = read(info.Data, GetOffset(info.Data));
        }

        public IgbClassField(IgbEntity parent, IgbMemoryRef info, int offset, Func<byte[], int, T> read) : base(parent, -1)
        {
            if (read != null)
                Value = read(info.Data, offset);
        }
    }

    public abstract class IgbStructField<T> : IgbField where T : struct
    {
        public T Value { get; set; }

        public IgbStructField(IgbObject parent, IgbObjectRef info, int index, Func<byte[], int, T> read) : base(parent, index)
        {
            if (read != null)
                Value = read(info.Data, GetOffset(info.Data));
        }

        public IgbStructField(IgbEntity parent, IgbMemoryRef info, int offset, Func<byte[], int, T> read) : base(parent, -1)
        {
            if (read != null)
                Value = read(info.Data, offset);
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
        public int ID { get; set; }

        public IgbRefType(int id)
        {
            ID = id;
        }
    }

    public class IgbObjectRef : IgbRefType
    {
        public IgbStruct Struct { get; set; }
        public byte[] Data { get; set; }

        public IgbObjectRef(int id) : base(id) { }
    }

    public class IgbMemoryRef : IgbRefType
    {
        public IgbType Type { get; set; }
        public byte Unk1 { get; set; }
        public int Unk2 { get; set; }
        public byte[] Data { get; set; }

        public IgbMemoryRef(int id) : base(id) { }
    }

    public class IgbEntity
    {
        public int ID { get; }
        public IGB IGB { get; }

        protected IgbEntity(IGB igb, int id)
        {
            IGB = igb;
            ID = id;
        }
    }

    public class IgbObject : IgbEntity
    {
        public IgbStruct Struct { get; }

        public IgbObject(IGB igb, IgbObjectRef info) : base(igb, info.ID)
        {
            Struct = info.Struct;
        }
    }

    public class IgbMemory<T> : IgbEntity where T : IgbField
    {
        public byte Unk1 { get; }
        public int Unk2 { get; }

        public List<T> Data { get; set; }

        public IgbMemory(IGB igb, IgbMemoryRef info) : base(igb, info.ID)
        {
            Unk1 = info.Unk1;
            Unk2 = info.Unk2;
            Data = new();

            var test = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            int elt_sz = (int)typeof(T).GetProperty("Size", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);

            int ofs = 0;
            for (int i = 0; i < info.Data.Length / elt_sz; ++i)
            {
                T thing = Activator.CreateInstance(typeof(T), this, info, ofs) as T;
                Data.Add(thing);
                ofs += elt_sz;
            }
        }
    }
}
