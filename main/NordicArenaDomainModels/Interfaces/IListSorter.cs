using System.Collections.Generic;

namespace NordicArenaDomainModels.Interfaces
{
    public interface IListSorter
    {
        void Sort<T>(IList<T> list);
    }
}
