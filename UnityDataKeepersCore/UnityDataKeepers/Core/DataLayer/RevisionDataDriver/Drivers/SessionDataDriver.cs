using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver.Drivers
{
    class SessionDataDriver<TItem> : 
        IRevisionDataDriver<TItem>
        where TItem : class, IDataItem, IComparer<TItem>
    {
        private readonly List<TItem> _collection = new List<TItem>(); 

        public TItem GetByHash(Hash128 hash)
        {
            return _collection.FirstOrDefault(i => i.Hash.Equals(hash));
        }

        public bool Add(TItem item)
        {
            _collection.Add(item);
            return true;
        }

        public bool Remove(TItem item)
        {
            return _collection.Remove(item);
        }

        public int Add(IEnumerable<TItem> items)
        {
            var toAdd = items.SkipWhile(i => _collection.Contains(i)).ToArray();
            _collection.AddRange(toAdd);
            return toAdd.Count();
        }

        public int Remove(IEnumerable<TItem> items)
        {
            return _collection.RemoveAll(i=>items.Any(item=>item.Hash.Equals(i.Hash)));
        }

        public bool Update(TItem item)
        {
            var index = _collection.BinarySearch(item);
            if (index < 0) return false;
            _collection[index] = item;
        }

        public IEnumerable<TItem> GetAll()
        {
            return _collection;
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public int Count()
        {
            return _collection.Count;
        }
    }
}
