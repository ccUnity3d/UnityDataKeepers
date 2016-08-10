using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public static class DataCollectionDriverFactory
    {
        public static IDataCollectionDriver<T> CreateSessionDataDriver<T>()
            where T : class, IDataItem
        {
            return new SessionDataCollectionDriver<T>();
        }

        public static IDataCollectionDriver<T> CreateCsvDataDriver<T>(string filePath, bool isReadonly)
            where T : class, IDataItem
        {
            return new CsvDataCollectionDriver<T>(filePath, isReadonly);
        }
    }
}
