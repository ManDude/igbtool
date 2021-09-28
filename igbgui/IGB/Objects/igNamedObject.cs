using igbgui.Types;

namespace igbgui.Structs
{
    public class igNamedObject : igObject
    {
        public igStringMetaField Name;
        public igNamedObject(IgbStruct s) : base(s)
        {
            Name = new(this, 0);
        }
    }
}
