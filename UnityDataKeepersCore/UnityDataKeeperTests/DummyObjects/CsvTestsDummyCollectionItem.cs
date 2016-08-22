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
            Field3,
            Field4,
            Field5,
            Field6,
            Field7,
            Field8,
            Field9,
            Field10,
            Field11,
            Field12,
            Field13,
            Field14,
            Field15,
            Field16,
            Field17,
            Field18,
            Field19,
            Field20,
            OtherField
        }

        public Guid Guid { get; set; }
        public string StringProperty;
        public int IntProperty;
        public float FloatProperty;
        public CsvTestEnum EnumField;
        public DateTime DateTimeField;
        public TimeSpan TimeSpanField;

        public CsvTestsDummyCollectionItem()
        {
            Guid = Guid.NewGuid();
        }

        public int CompareTo(object obj)
        {
            return string.Compare(Guid.ToString(),
                ((CsvTestsDummyCollectionItem)obj).Guid.ToString(),
                StringComparison.InvariantCulture);
        }
    }
}
