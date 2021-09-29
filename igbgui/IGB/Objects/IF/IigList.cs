using System.Collections.Generic;

namespace igbgui.Objects
{
    public interface IigList<T>
    {
        public List<T> GetList();
        public T At(int index);
    }
}
