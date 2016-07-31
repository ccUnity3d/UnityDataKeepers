using UnityEngine;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver
{
    public class DummyDataItem : IDataItem
    {
        public Hash128 Hash { get; private set; }
    }
}