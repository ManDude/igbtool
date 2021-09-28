using igbgui.Types;
using System;
using System.Collections.Generic;

namespace igbgui.Structs
{
    public class igInfoList : igObjectList, IigList<LevelInfo>
    {
        public igInfoList(IgbStruct s) : base(s)
        {
        }

        public new List<LevelInfo> GetList()
        {
            var list = new List<LevelInfo>();
            for (int i = 0; i < DataList.Value.Data.Length / 4; ++i)
            {
                list.Add(At(i));
            }
            return list;
        }
        public new LevelInfo At(int index)
        {
            return IGB.GetRef<LevelInfo>(BitConverter.ToInt32(DataList.Value.Data, index * 4));
        }
    }
}
