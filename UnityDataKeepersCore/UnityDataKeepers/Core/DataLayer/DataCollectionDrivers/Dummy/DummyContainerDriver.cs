using System.Collections.Generic;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public class DummyContainerDriver<TItem> : IDataCollectionDriver<TItem> 
        where TItem : class, IDataItem
    {
        public TItem GetByHash(Hash128 hash)
        {
            return null;
        }

        public bool Add(TItem item)
        {
            return false;
        }

        public bool Remove(TItem item)
        {
            return false;
        }

        public int Add(IEnumerable<TItem> items)
        {
            return 0;
        }

        public int Remove(IEnumerable<TItem> items)
        {
            return 0;
        }

        public bool Update(TItem item)
        {
            return false;
        }

        public IEnumerable<TItem> GetAll()
        {
            return null;
        }

        public void Clear()
        {
        }

        public int Count()
        {
            return 0;
        }
    }
}