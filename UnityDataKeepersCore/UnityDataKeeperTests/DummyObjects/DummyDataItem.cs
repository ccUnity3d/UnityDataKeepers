using System;
using UnityDataKeepersCore.Core.DataLayer.Model;
using UnityEngine;

namespace UnityDataKeeperTests.DummyObjects
{
    public class DummyDataItem : IDataItem
    {
        public Hash128 Hash { get; private set; }
        public string DummyProperty;

        public int CompareTo(object obj)
        {
            return string.Compare(Hash.ToString(),
                ((DummyDataItem) obj).Hash.ToString(),
                StringComparison.InvariantCulture);
        }
    }
}