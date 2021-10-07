using igbgui.Fields;
using igbgui.Structs;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class CrateDataList : igDataList<CrateDataMetaField>, IigList<CrateData>
    {
        public CrateDataList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<CrateData> GetList()
        {
            var list = new List<CrateData>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new CrateData At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
