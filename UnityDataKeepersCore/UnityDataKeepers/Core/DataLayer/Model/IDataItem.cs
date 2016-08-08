using System;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public interface IDataItem : IComparable
    {
        Hash128 Hash { get; }
    }
}