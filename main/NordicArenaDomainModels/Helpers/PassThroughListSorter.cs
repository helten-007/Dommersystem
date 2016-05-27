using System.Collections.Generic;
using NordicArenaDomainModels.Interfaces;

namespace NordicArenaDomainModels.Helpers
{
    public class PassThroughListSorter : IListSorter
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        public void Sort<T>(IList<T> list)
        {
        }
    }
}
