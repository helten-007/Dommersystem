using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers
{
    public class ListSet<T> : IDbSet<T> where T : class, new()
    {
        private List<T> _list = new List<T>();
        private ObservableCollection<T> _oList;

        public ListSet()
        {
            _oList = new ObservableCollection<T>(_list);
        }

        public T Add(T entity)
        {
            Local.Add(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            return entity;
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return null;
        }

        public T Create()
        {
            return new T();
        }

        public T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<T> Local
        {
            get { return _oList; }
        }

        public T Remove(T entity)
        {
            _list.Remove(entity);
            return entity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Local.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Local.GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return Local.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return Local.AsQueryable().Provider; }
        }
    }
}
