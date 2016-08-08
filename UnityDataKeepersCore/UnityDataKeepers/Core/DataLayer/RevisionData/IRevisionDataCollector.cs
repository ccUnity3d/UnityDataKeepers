using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.DataCollections;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public interface IRevisionDataCollector
    {
        Hash128 Hash { get; }
        IEnumerable<IDataCollection> Collections { get; }
    }
}