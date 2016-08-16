using System;
using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public interface IDataCollectionDriver<TItem> : IDisposable
        where TItem : class, IDataItem
    {
        bool IsReadOnly { get; }
        TItem GetByHash(Guid hash);
        bool Add(TItem item);
        bool Remove(TItem item);
        int Add(IEnumerable<TItem> items);
        int Remove(IEnumerable<TItem> items);
        bool Update(TItem item);
        IEnumerable<TItem> GetAll();
        void Clear();
        int Count();
    }
}
