using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public interface IRevisionDataCollector
    {
        Hash128 Hash { get; }
        bool PutDataTo<TItem>(IRevisionDataDriver<TItem> driver)
            where TItem : class, IDataItem, IComparer<TItem>;
    }
}