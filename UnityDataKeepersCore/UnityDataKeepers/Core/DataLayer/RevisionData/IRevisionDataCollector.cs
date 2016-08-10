using System;
using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.DataCollections;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public interface IRevisionDataCollector
    {
        Guid Hash { get; }
        IEnumerable<IDataCollection> Collections { get; }
    }
}