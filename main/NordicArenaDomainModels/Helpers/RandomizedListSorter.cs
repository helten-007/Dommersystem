using System.Collections.Generic;
using NordicArenaDomainModels.Interfaces;
using NordicArenaDomainModels.Lang;

namespace NordicArenaDomainModels.Helpers
{
    public class RandomizedListSorter : IListSorter
    {
        public void Sort<T>(IList<T> list)
        {
            LangUtilities.Shuffle(list);
        }
    }
}
