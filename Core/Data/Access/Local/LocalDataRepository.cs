using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Access.Local
{
    public class LocalDataRepository<T1> : IDataRepository<T1> where T1:DataObject
    {
        private class DataCollection<T2> : KeyedCollection<string, T2> where T2:T1
        {
            public DataCollection()
            {
                // do nothing
            }

            protected override string GetKeyForItem(T2 t)
            {
                return t.Id;
            }
        }

        private DataCollection<T1> data;

        public LocalDataRepository()
        {
            data = new DataCollection<T1>();
        }

        public IEnumerable<T1> GetAll()
        {
            List<T1> copies = new List<T1>(data.Count());
            foreach (T1 t in data)
            {
                copies.Add((T1)t.Clone());
            }
            return copies;
        }

        public T1 Get(string id)
        {
            try
            {
                T1 t = data[id];
                return (T1)t.Clone();
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        public bool Add(T1 t)
        {
            if (t != null)
            {
                data.Remove(t.Id);
                data.Add((T1)t.Clone());
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(string id)
        {
            return data.Remove(id);
        }
    }
}
