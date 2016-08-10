using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers
{
    internal class SessionDataCollectionDriver<TItem> : 
        IDataCollectionDriver<TItem>
        where TItem : class, IDataItem
    {
        private readonly List<TItem> _collection = new List<TItem>(); 

        public TItem GetByHash(Hash128 hash)
        {
            return _collection.FirstOrDefault(i => i.Hash.Equals(hash));
        }

        public bool Add(TItem item)
        {
            if (item == null) 
                return false;
            if (_collection.Contains(item))
                return false;
            _collection.Add(item);
            return true;
        }

        public bool Remove(TItem item)
        {
            return _collection.Remove(item);
        }

        public int Add(IEnumerable<TItem> items)
        {
            if (items == null) return 0;
            var toAdd = items.Where(i => i != null && !_collection.Contains(i)).ToList();
            _collection.AddRange(toAdd);
            return toAdd.Count();
        }

        public int Remove(IEnumerable<TItem> items)
        {
            if (items == null) return 0;
            return
                _collection.RemoveAll(
                    i =>
                        items.Any(
                            item => item != null && item.Hash.Equals(i.Hash)));
        }

        public bool Update(TItem item)
        {
            var index = _collection.BinarySearch(item);
            if (index < 0) return false;
            _collection[index] = item;
            return true;
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
