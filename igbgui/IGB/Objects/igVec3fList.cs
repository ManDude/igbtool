using igbgui.Types;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public class igVec3fList : igDataList<igVec3fMetaField>, IigList<Vector3>
    {
        public igVec3fList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<Vector3> GetList()
        {
            var list = new List<Vector3>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new Vector3 At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
