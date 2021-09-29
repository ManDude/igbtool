using igbgui.Fields;
using igbgui.Structs;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class CrystalDataList : igDataList<CrystalDataMetaField>, IigList<CrystalData>
    {
        public CrystalDataList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<CrystalData> GetList()
        {
            var list = new List<CrystalData>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new CrystalData At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
