using igbgui.Types;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public class igVec3fList : igDataList, IigList<Vector3>
    {
        public igVec3fList(IgbStruct s) : base(s)
        {
        }

        public new List<Vector3> GetList()
        {
            var list = new List<Vector3>();
            for (int i = 0; i < DataList.Value.Data.Length / 12; ++i)
            {
                list.Add(At(i));
            }
            return list;
        }
        public new Vector3 At(int index)
        {
            float x = BitConverter.ToSingle(DataList.Value.Data, index * 12 + 0);
            float y = BitConverter.ToSingle(DataList.Value.Data, index * 12 + 4);
            float z = BitConverter.ToSingle(DataList.Value.Data, index * 12 + 8);
            return new Vector3(x, y, z);
        }
    }
}
