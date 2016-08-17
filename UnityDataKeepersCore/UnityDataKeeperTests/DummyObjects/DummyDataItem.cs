using System;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeeperTests.DummyObjects
{
    public class DummyDataItem : IDataItem
    {
        public Guid Guid { get; set; }
        public string DummyProperty;

        public int CompareTo(object obj)
        {
            return string.Compare(Guid.ToString(),
                ((DummyDataItem) obj).Guid.ToString(),
                StringComparison.InvariantCulture);
        }
    }
}