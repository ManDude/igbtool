using igbgui.Fields;
using igbgui.Structs;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class RestartPointDataList : igDataList<RestartPointDataMetaField>, IigList<RestartPointData>
    {
        public RestartPointDataList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<RestartPointData> GetList()
        {
            var list = new List<RestartPointData>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new RestartPointData At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
