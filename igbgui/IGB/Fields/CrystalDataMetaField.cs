using igbgui.Structs;

namespace igbgui.Fields
{
    public class CrystalDataMetaField : IgbStructField<CrystalData>
    {
        public CrystalDataMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadCrystalData) { }
        public CrystalDataMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadCrystalData) { }

        public new static int Size => 32;
    }
}
