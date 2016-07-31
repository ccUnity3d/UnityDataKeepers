using System.Collections.Generic;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver
{
    internal interface IRevisionDataDriver<T> where T : class, IDataItem
    {
        T GetByHash(Hash128 hash);
        bool Add(T item);
        bool Remove(T item);
        int Add(IEnumerable<T> items);
        int Remove(IEnumerable<T> items);
        bool Update(T item);
        IEnumerable<T> GetAll();
        void Clear();
        int Count();
    }
}
