namespace UnityDataKeepersCore.Core.DataLayer.RevisionDataDriver
{
    public static class RevisionDataDriverFactory
    {
        public static IRevisionDataDriver<IDataItem> GetDummy()
        {
            return (IRevisionDataDriver<IDataItem>) new DummyDataDriver<DummyDataItem>();
        }
    }
}
