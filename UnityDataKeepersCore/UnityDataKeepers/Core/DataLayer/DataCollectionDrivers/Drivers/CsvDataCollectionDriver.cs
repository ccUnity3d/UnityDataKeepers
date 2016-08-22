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

        public override bool IsNotStorable
        {
            get
            {
                return _dataSource.HasValue
                    ? _dataSource.Value.IsReadonly
                    : base.IsNotStorable;
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

            Add(CsvFile.Read<TItem>(_dataSource.Value.FilePath));
            return true;
        }

        public bool Save()
        {
            try
            {
                if (_dataSource.HasValue)
                {
                    using (var csvFile = new CsvFile<TItem>(_dataSource.Value.FilePath))
                    {
                        foreach (var item in GetAll())
                        {
                            csvFile.Append(item);
                        }
                    }
                }
                else
                {
                    return false;
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