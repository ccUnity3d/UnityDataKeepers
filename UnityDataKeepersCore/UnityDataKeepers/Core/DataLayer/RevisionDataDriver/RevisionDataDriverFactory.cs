using System.Collections.Generic;
using UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver.Drivers;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver
{
    public static class RevisionDataDriverFactory
    {
        public static IRevisionDataDriver<DummyDataItem> GetDummy()
        {
            return new DummyDataDriver<DummyDataItem>();
        }

        public static IRevisionDataDriver<T> CreateSessionDataDriver<T>()
            where T : class, IDataItem, IComparer<T>
        {
            return new SessionDataDriver<T>();
        }
    }
}
