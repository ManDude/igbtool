using igbgui.Fields;

namespace igbgui.Objects
{
    public class igNamedObject : igObject
    {
        public igStringMetaField Name;
        public igNamedObject(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Name = new(this, info, 0);
        }
    }
}
