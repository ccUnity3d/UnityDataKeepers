using System;
using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers.CsvFiles;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers.Drivers
{
    internal class CsvDataCollectionDriver<TItem> : 
        SessionDataCollectionDriver<TItem>,
        IStoredCollectionDriver<TItem>
        where TItem : class, IDataItem, new()
    {
        private StoredCollectionDataSource? _dataSource = null;

        public override bool IsReadOnly
        {
            get
            {
                return _dataSource.HasValue
                    ? _dataSource.Value.IsReadonly
                    : base.IsReadOnly;
            }
        }

        public void SetDataSource(StoredCollectionDataSource source)
        {
            _dataSource = source;
        }

        public bool Load()
        {
            if (!_dataSource.HasValue)
                return false;

            return Add(CsvFile.Read<TItem>(_dataSource.Value.FilePath)) > 0;
        }

        public bool Save()
        {
            try
            {
                using (var csvFile = new CsvFile<TItem>("clients.csv"))
                {
                    foreach (var item in GetAll())
                    {
                        csvFile.Append(item);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void Dispose()
        {
            _dataSource = null;
            base.Dispose();
        }
    }
}