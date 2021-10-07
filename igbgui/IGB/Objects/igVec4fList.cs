using igbgui.Fields;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class igVec4fList : igDataList<igVec4fMetaField>, IigList<Vector4>
    {
        public igVec4fList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<Vector4> GetList()
        {
            var list = new List<Vector4>();
            if (DataList.Value != null)
            {
                foreach (var v in DataList.Value.Data)
                {
                    list.Add(v.Value);
                }
            }
            return list;
        }
        public new Vector4 At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
