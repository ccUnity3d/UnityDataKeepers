using System;
using System.Collections.Generic;
using System.Linq;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers
{
    internal class SessionDataCollectionDriver<TItem> : 
        IDataCollectionDriver<TItem>
        where TItem : class, IDataItem
    {
        private static readonly System.Random Rnd = new System.Random();

        private readonly List<TItem> _collection = new List<TItem>();
        private readonly HashSet<string> _hashes = new HashSet<string>(); 

        public bool isReadOnly
        {
            get { return false; }
        }

        public TItem GetByHash(Guid hash)
        {
            return _collection.FirstOrDefault(i => i.Hash.Equals(hash));
        }

        public bool Add(TItem item)
        {
            if (item == null) 
                return false;
            if (_collection.Contains(item))
                return false;
            while (_hashes.Contains(item.Hash.ToString()))
            {
                item.Hash = Guid.NewGuid();
            }
            _hashes.Add(item.Hash.ToString());

            _collection.Add(item);
            return true;
        }
        public bool Remove(TItem item)
        {
            _hashes.Remove(item.Hash.ToString());
            return _collection.Remove(item);
        }

        public int Add(IEnumerable<TItem> items)
        {
            if (items == null) return 0;
            var toAdd = items.Where(i => i != null && _hashes.Contains(i.Hash.ToString())).ToList();
            
            foreach (var t in toAdd)
            {
                while (_hashes.Contains(t.Hash.ToString()))
                {
                    t.Hash = Guid.NewGuid();
                }
                _hashes.Add(t.Hash.ToString());
            }

            _collection.AddRange(toAdd);
            return toAdd.Count();
        }

        public int Remove(IEnumerable<TItem> items)
        {
            if (items == null) return 0;
            var toRemove = items.Where(i => i != null && _hashes.Contains(i.Hash.ToString())).ToList();
            foreach (var item in toRemove)
            {
                _hashes.Remove(item.Hash.ToString());
            }
            return
                _collection.RemoveAll(
                    i =>
                        toRemove.Any(
                            item => item.Hash.Equals(i.Hash)));
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
