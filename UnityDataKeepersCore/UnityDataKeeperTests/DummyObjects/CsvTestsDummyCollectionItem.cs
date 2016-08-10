using System;
using UnityDataKeepersCore.Core.DataLayer.Model;
using UnityEngine;
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

        public Hash128 Hash { get; private set; }
        public string StringProperty;
        public int IntProperty;
        public float FloatProperty;
        public CsvTestEnum EnumField;
        public DateTime DateTimeField;
        public TimeSpan TimeSpanField;

        public CsvTestsDummyCollectionItem()
        {
            Hash = new Hash128(RndUint(), RndUint(), RndUint(), RndUint());
        }

        private static uint RndUint()
        {
            var thirtyBits = (uint) Random.Next(1 << 30);
            var twoBits = (uint) Random.Next(1 << 2);
            return (thirtyBits << 2) | twoBits;
        }

        public int CompareTo(object obj)
        {
            return string.Compare(Hash.ToString(),
                ((CsvTestsDummyCollectionItem)obj).Hash.ToString(),
                StringComparison.InvariantCulture);
        }
    }
}
