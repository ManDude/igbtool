using System.Collections.Generic;

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

    public class IgbField
    {
        public IgbType Type { get; set; }
        public short UnkOff { get; set; }
        public short Size { get; set; }
    }

    public class IgbStruct
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Master { get; set; }
        public IgbStruct Parent { get; set; }
        public int UnkSize { get; set; }
        public List<IgbField> Fields { get; } = new();
    }

    public class IgbRefType
    {
        public int RefID { get; set; }
    }

    public class IgbObject : IgbRefType
    {
        public IgbStruct Struct { get; set; }
        public byte[] Data { get; set; }
    }

    public class IgbMemory : IgbRefType
    {
        public IgbType Type { get; set; }
        public byte Unk1 { get; set; }
        public int Unk2 { get; set; }
        public byte[] Data { get; set; }
    }
}
