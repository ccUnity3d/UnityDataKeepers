using System.Collections.Generic;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public interface IDataCollectionDriver<TItem>
        where TItem : class, IDataItem
    {
        bool isReadOnly { get; }
        TItem GetByHash(Hash128 hash);
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
