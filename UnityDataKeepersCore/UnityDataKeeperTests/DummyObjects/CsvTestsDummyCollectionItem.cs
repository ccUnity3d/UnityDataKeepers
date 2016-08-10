using System;
using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers;
using UnityDataKeepersCore.Core.DataLayer.Model;
using UnityEngine;

namespace UnityDataKeeperTests.DummyObjects
{
    class CsvTestsDummyCollectionItem
    {
        public class DummyDataItem : IDataItem
        {
            public Hash128 Hash { get; private set; }
            public string DummyProperty;

            public int CompareTo(object obj)
            {
                return string.Compare(Hash.ToString(),
                    ((UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.DummyDataItem)obj).Hash.ToString(),
                    StringComparison.InvariantCulture);
            }
        }
    }
}
