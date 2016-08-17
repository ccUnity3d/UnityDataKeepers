using System;
using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.RevisionData;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollections
{
    public interface IDataCollectionSignature : IComparable
    {
        Guid Guid { get; }
        IEnumerable<RevisionAttribute> Attributes { get; }
    }
}