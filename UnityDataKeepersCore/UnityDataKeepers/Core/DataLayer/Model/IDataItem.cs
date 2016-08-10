using System;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.Model
{
    public interface IDataItem : IComparable
    {
        Hash128 Hash { get; }
    }
}