using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public interface IDataCollectionSignature : IComparable
    {
        Hash128 Hash { get; }
        IEnumerable<RevisionAttribute> Attributes { get; }
    }
}