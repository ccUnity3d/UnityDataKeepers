using UnityEngine;

namespace UnityDataKeepersCore.Core.Model
{
    public interface IDataKeeperItem
    {
        Hash128 Hash { get; }
    }
}
