using igbgui.Types;
using System;
using System.Collections.Generic;

namespace igbgui.Structs
{
    public class igDataList<T> : igObject, IigList<object> where T : IgbField
    {
        public igIntMetaField Length;
        public igIntMetaField AllocatedLength;
        public igMemoryRefMetaField<T> DataList;
        public igDataList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Length = new(this, info, 0);
            AllocatedLength = new(this, info, 1);
            DataList = new(this, info, 2);
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
