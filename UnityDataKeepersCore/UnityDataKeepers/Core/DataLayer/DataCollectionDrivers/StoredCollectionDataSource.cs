namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public struct StoredCollectionDataSource
    {
        public readonly bool IsReadonly;
        public readonly string FilePath;
        public readonly bool CreateSourceIfNoExist;

        public StoredCollectionDataSource(string filePath, bool isReadonly)
        {
            FilePath = filePath;
            IsReadonly = isReadonly;
            CreateSourceIfNoExist = true;
        }

        public StoredCollectionDataSource(string filePath, bool isReadonly, bool createSourceIfNoExist)
        {
            IsReadonly = isReadonly;
            FilePath = filePath;
            CreateSourceIfNoExist = createSourceIfNoExist;
        }
    }
}