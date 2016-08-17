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
        private class ItemsComparer : IComparer<TItem>
        {
            public int Compare(TItem x, TItem y)
            {
                return x.Hash.CompareTo(y.Hash);
            }
        }

        private List<TItem> _collection = new List<TItem>();

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual TItem GetByHash(Guid hash)
        {
            return _collection.FirstOrDefault(i => i.Hash.Equals(hash));
        }

        public virtual bool Add(TItem item)
        {
            if (item == null)
                return false;
            item.Hash = Guid.NewGuid();
            if (_collection.Any(i => i.Hash.Equals(item.Hash)))
                return false;
            _collection.Add(item);
            return true;
        }

        public virtual bool Remove(TItem item)
        {
            if (item == null) return false;
            return _collection.Remove(item);
        }

        public virtual int Add(IEnumerable<TItem> items)
        {
            if (items == null)
                return 0;

            //            var toAdd =
            //                items.Where(i => i != null)
            //                    .Where(i => !_collection.Any(c => c.Equals(i)))
            //                    .Select(i =>
            //                    {
            //                        i.Hash = Guid.NewGuid();
            //                        return i;
            //                    }).ToList();
            //            _collection.AddRange(toAdd);
            //            return toAdd.Count();

            return items.Select(Add).Count(i => i);
        }

        public virtual int Remove(IEnumerable<TItem> items)
        {
            if (items == null) return 0;
            var toRemove = items.Where(i => i != null && _collection.Contains(i)).ToList();
            return
                _collection.RemoveAll(
                    i =>
                        toRemove.Any(
                            item => item.Hash.Equals(i.Hash)));
        }

        public virtual bool Update(TItem item)
        {
            var index = _collection.BinarySearch(item, new ItemsComparer());
            if (index < 0) return false;
            _collection[index] = item;
            return true;
        }

        public virtual IEnumerable<TItem> GetAll()
        {
            return _collection;
        }

        public virtual void Clear()
        {
            _collection.Clear();
        }

        public virtual int Count()
        {
            return _collection.Count;
        }

        public virtual void Dispose()
        {
            _collection = null;
        }
    }
}
