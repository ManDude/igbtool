using igbgui.Types;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public class igVec4fList : igDataList<igVec4fMetaField>, IigList<Vector4>
    {
        public igVec4fList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<Vector4> GetList()
        {
            var list = new List<Vector4>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new Vector4 At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
