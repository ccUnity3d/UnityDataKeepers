using System.Text;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers
{
    public struct StoredCollectionDataSource
    {
        public readonly bool IsReadonly;
        public readonly string FilePath;
        public readonly Encoding FileEncoding;

        public StoredCollectionDataSource(string filePath, bool isReadonly)
        {
            FilePath = filePath;
            IsReadonly = isReadonly;
            FileEncoding = Encoding.Unicode;
        }

        public StoredCollectionDataSource(string filePath, bool isReadonly, Encoding fileEncoding)
        {
            FilePath = filePath;
            IsReadonly = isReadonly;
            FileEncoding = fileEncoding;
        }
    }
}