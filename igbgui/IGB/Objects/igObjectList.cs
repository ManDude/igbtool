using igbgui.Types;
using System;
using System.Collections.Generic;

namespace igbgui.Structs
{
    public class igObjectList : igDataList, IigList<igObject>
    {
        public igObjectList(IgbStruct s) : base(s)
        {
        }

        public new List<igObject> GetList()
        {
            var list = new List<igObject>();
            for (int i = 0; i < DataList.Value.Data.Length / 4; ++i)
            {
                list.Add(At(i));
            }
            return list;
        }
        public new igObject At(int index)
        {
            return IGB.GetRef<igObject>(BitConverter.ToInt32(DataList.Value.Data, index * 4));
        }
    }
}
