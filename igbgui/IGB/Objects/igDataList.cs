using igbgui.Types;
using System;
using System.Collections.Generic;

namespace igbgui.Structs
{
    public class igDataList : igObject, IigList<object>
    {
        public igIntMetaField Length;
        public igIntMetaField AllocatedLength;
        public igMemoryRefMetaField DataList;
        public igDataList(IgbStruct s) : base(s)
        {
            Length = new(this, 0);
            AllocatedLength = new(this, 1);
            DataList = new(this, 2);
        }

        public List<object> GetList()
        {
            throw new NotImplementedException("Cannot access the data of an igDataList");
        }
        public object At(int index)
        {
            throw new NotImplementedException("Cannot access the data of an igDataList");
        }
    }
}
