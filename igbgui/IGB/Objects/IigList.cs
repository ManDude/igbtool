using System.Collections.Generic;

namespace igbgui.Structs
{
    public interface IigList<T>
    {
        public List<T> GetList();
        public T At(int index);
    }
}
