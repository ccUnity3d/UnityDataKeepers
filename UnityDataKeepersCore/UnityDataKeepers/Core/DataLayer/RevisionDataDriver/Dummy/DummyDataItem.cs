using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver
{
    public class DummyDataItem : IDataItem,
        IComparer<DummyDataItem>
    {
        public Hash128 Hash { get; private set; }
        public string DummyProperty;
        public int Compare(DummyDataItem x, DummyDataItem y)
        {
            return string.Compare(x.Hash.ToString(),
                y.Hash.ToString(),
                StringComparison.InvariantCulture);
        }
    }
}