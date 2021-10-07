namespace igbgui.Objects
{
    public class PhantomOBBList : igObjectList<PhantomOBB>, IigList<PhantomOBB>
    {
        public PhantomOBBList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }
    }
}
