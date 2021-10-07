using igbgui.Fields;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class igObjectList<T> : igDataList<igObjectRefMetaField<T>>, IigList<T> where T : igObject
    {
        public igObjectList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<T> GetList()
        {
            var list = new List<T>();
            if (DataList.Value != null)
            {
                foreach (var v in DataList.Value.Data)
                {
                    list.Add(v.Value);
                }
            }
            return list;
        }
        public new T At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
