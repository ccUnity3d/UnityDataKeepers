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

        public static IStoredCollectionDriver<T> CreateCsvDataDriver<T>(string filePath, bool isReadonly, bool autoLoad = true)
            where T : class, IDataItem, new()
        {
            var driver = new CsvDataCollectionDriver<T>();
            driver.SetDataSource(new StoredCollectionDataSource(filePath, isReadonly));
            if (autoLoad)
                driver.Load();
            return driver;
        }
    }
}
