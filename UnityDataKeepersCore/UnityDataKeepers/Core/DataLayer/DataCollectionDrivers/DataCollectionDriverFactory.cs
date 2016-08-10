using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public static class DataCollectionDriverFactory
    {
        public static IDataCollectionDriver<T> CreateSessionDataDriver<T>()
            where T : class, IDataItem
        {
            return new SessionDataCollectionDriver<T>();
        }

//        public static IDataCollectionDriver<T> CreateSessionDataDriver<T>()
//            where T : class, IDataItem
//        {
//            return new SessionDataCollectionDriver<T>();
//        }
    }
}
