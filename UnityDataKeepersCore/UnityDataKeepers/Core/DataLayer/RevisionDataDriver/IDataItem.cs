using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver
{
    public interface IDataItem
    {
        Hash128 Hash { get; }
    }
}