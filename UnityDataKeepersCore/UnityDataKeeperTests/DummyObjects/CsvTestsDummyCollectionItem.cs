using System;
using UnityDataKeepersCore.Core.DataLayer.Model;
using Random = System.Random;

namespace UnityDataKeeperTests.DummyObjects
{
    class CsvTestsDummyCollectionItem : IDataItem
    {
        private static readonly Random Random = new Random();

        public enum CsvTestEnum
        {
            Field1,
            Field2,
            OtherField
        }

        public Guid Hash { get; set; }
        public string StringProperty;
        public int IntProperty;
        public float FloatProperty;
        public CsvTestEnum EnumField;
        public DateTime DateTimeField;
        public TimeSpan TimeSpanField;

        public CsvTestsDummyCollectionItem()
        {
            Hash = Guid.NewGuid();
        }

        public int CompareTo(object obj)
        {
            return string.Compare(Hash.ToString(),
                ((CsvTestsDummyCollectionItem)obj).Hash.ToString(),
                StringComparison.InvariantCulture);
        }
    }
}
