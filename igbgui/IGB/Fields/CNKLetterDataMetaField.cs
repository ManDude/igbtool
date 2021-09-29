using igbgui.Structs;

namespace igbgui.Fields
{
    public class CNKLetterDataMetaField : IgbStructField<CNKLetterData>
    {
        public CNKLetterDataMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadCNKLetterData) { }
        public CNKLetterDataMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadCNKLetterData) { }

        public new static int Size => 32;
    }
}
