using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.Model;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers
{
    internal class CsvDataCollectionDriver<TItem> :
        IDataCollectionDriver<TItem>
        where TItem : class, IDataItem
    {
        public CsvDataCollectionDriver(string filePath, bool isReadonly)
        {
            throw new System.NotImplementedException();
        }

        public bool isReadOnly { get; private set; }
        public TItem GetByHash(Hash128 hash)
        {
            throw new System.NotImplementedException();
        }

        public bool Add(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public int Add(IEnumerable<TItem> items)
        {
            throw new System.NotImplementedException();
        }

        public int Remove(IEnumerable<TItem> items)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TItem> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public int Count()
        {
            throw new System.NotImplementedException();
        }
    }
}