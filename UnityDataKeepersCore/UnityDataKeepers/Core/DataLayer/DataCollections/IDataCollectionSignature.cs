using System;
using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.RevisionData;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollections
{
    public interface IDataCollectionSignature : IComparable
    {
        Hash128 Hash { get; }
        IEnumerable<RevisionAttribute> Attributes { get; }
    }
}