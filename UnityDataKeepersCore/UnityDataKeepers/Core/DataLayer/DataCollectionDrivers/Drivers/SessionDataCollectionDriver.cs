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
                return x.Guid.CompareTo(y.Guid);
            }
        }

        private List<TItem> _collection = new List<TItem>();

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual TItem GetByGuid(Guid guid)
        {
            return _collection.FirstOrDefault(i => i.Guid.Equals(guid));
        }

        public virtual bool Add(TItem item)
        {
            return Add(item, true);
        }

        private bool Add(TItem item, bool needSortCollection)
        {
            if (item == null)
                return false;
            item.Guid = Guid.NewGuid();
            if (_collection.Any(i => i.Guid.Equals(item.Guid)))
                return false;
            _collection.Add(item);
            if (needSortCollection)
                _collection.Sort((a, b) => a.Guid.CompareTo(b.Guid));
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
            //                        i.Guid = Guid.NewGuid();
            //                        return i;
            //                    }).ToList();
            //            _collection.AddRange(toAdd);
            //            return toAdd.Count();

            var added = items.Select(i=>Add(i,false)).Count();
            if (added>0)
                _collection.Sort((a, b) => a.Guid.CompareTo(b.Guid));
            return added;
        }

        public virtual int Remove(IEnumerable<TItem> items)
        {
            if (items == null) return 0;
            var toRemove = items.Where(i => i != null && _collection.Contains(i)).ToList();
            return
                _collection.RemoveAll(
                    i =>
                        toRemove.Any(
                            item => item.Guid.Equals(i.Guid)));
        }

        public virtual bool Update(TItem item)
        {
            if (item == null) return false;
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
