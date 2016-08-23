using System;
using System.IO;
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

        public bool SetAndVerifyDataSource(StoredCollectionDataSource source)
        {
            if (string.IsNullOrEmpty(source.FilePath))
                return false;
            var exist = File.Exists(source.FilePath);
            if (!exist && !source.CreateSourceIfNoExist)
                return false;
            if (!exist && source.CreateSourceIfNoExist)
            {
                bool canCreate;
                try
                {
                    using (File.Create(source.FilePath)) { }
                    File.Delete(source.FilePath);
                    canCreate = true;
                }
                catch
                {
                    canCreate = false;
                }
                if (!canCreate) return false;
            }

            _dataSource = source;
            return true;
        }

        public bool Load()
        {
            if (!_dataSource.HasValue)
                return false;
            if (!File.Exists(_dataSource.Value.FilePath))
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