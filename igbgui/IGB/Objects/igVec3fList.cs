using igbgui.Fields;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class igVec3fList : igDataList<igVec3fMetaField>, IigList<Vector3>
    {
        public igVec3fList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<Vector3> GetList()
        {
            var list = new List<Vector3>();
            if (DataList.Value != null)
            {
                foreach (var v in DataList.Value.Data)
                {
                    list.Add(v.Value);
                }
            }
            return list;
        }
        public new Vector3 At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
