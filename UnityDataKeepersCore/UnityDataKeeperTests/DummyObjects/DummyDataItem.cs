using System;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeeperTests.DummyObjects
{
    public class DummyDataItem : IDataItem
    {
        public Guid Hash { get; set; }
        public string DummyProperty;

        public int CompareTo(object obj)
        {
            return string.Compare(Hash.ToString(),
                ((DummyDataItem) obj).Hash.ToString(),
                StringComparison.InvariantCulture);
        }
    }
}