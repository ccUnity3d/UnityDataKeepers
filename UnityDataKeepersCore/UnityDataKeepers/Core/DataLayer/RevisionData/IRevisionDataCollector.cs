using System;
using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.DataCollections;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public interface IRevisionDataCollector
    {
        Guid Guid { get; }
        IEnumerable<IDataCollection> Collections { get; }
    }
}