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

        public static IStoredCollectionDriver<T> CreateCsvDataDriver<T>(StoredCollectionDataSource source, bool autoLoad = true)
            where T : class, IDataItem, new()
        {
            var driver = new CsvDataCollectionDriver<T>();
            if (!driver.SetAndVerifyDataSource(source))
                return null;
            if (autoLoad)
                driver.Load();
            return driver;
        }
    }
}
