using System;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers.CsvFiles
{
    public class CsvIgnorePropertyAttribute : Attribute
    {
        public override string ToString()
        {
            return "Ignore Property";
        }
    }
}