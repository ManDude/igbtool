using igbgui.Structs;

namespace igbgui.Fields
{
    public class CrateDataMetaField : IgbStructField<CrateData>
    {
        public CrateDataMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadCrateData) { }
        public CrateDataMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadCrateData) { }

        public new static int Size => 32;
    }
}
