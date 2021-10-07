using igbgui.Structs;

namespace igbgui.Fields
{
    public class RestartPointDataMetaField : IgbStructField<RestartPointData>
    {
        public RestartPointDataMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadRestartPointData) { }
        public RestartPointDataMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadRestartPointData) { }

        public new static int Size => 32;
    }
}
