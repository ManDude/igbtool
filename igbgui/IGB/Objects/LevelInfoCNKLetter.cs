using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoCNKLetter : igObject
    {
        public igObjectRefMetaField<CNKLetterDataList> CNKLetterList;

        public LevelInfoCNKLetter(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            CNKLetterList = new(this, info, 0);
        }
    }
}
