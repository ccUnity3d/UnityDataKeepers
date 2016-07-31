using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver
{
    internal interface IDataItem
    {
        Hash128 Hash { get; }
    }
}