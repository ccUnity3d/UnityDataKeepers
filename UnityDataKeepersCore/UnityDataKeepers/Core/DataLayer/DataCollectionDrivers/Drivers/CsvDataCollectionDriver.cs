using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

            try
            {
                using (var file = new StreamReader(File.OpenRead(_dataSource.Value.FilePath)))
                {
                    var header = file.ReadLine() ?? "";
                    var headerAttributes = header.Split(',').OrderBy(i => i);
                    var itemAttributes = typeof(TItem).GetFields(BindingFlags.Instance | BindingFlags.Public)
                        .Select(i => i.Name)
                        .Union(typeof(TItem).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Select(i => i.Name)).Where(i => !i.Equals("Guid")).OrderBy(i => i);
                    if (!headerAttributes.SequenceEqual(itemAttributes)) return false;
                    //                    attributes = Enumerable.Except()
                }
            }
            catch
            {
                return false;
            }

            try
            {
                var data = CsvFile.Read<TItem>(_dataSource.Value.FilePath).ToArray();
                Add(data);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Save()
        {
            try
            {
                if (_dataSource.HasValue && !IsNotStorable)
                {
                    using (var file = new StreamWriter(File.Open(_dataSource.Value.FilePath,FileMode.Create)))
                    {
                        const string separator = ",";
                        const string eol = "\n";
                        var fields = typeof(TItem).GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
                        var properties = typeof (TItem).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(i => !i.Name.Equals("Guid")).ToArray();
                        var headers = string.Join(separator,
                            fields.Select(i => i.Name).Union(properties.Select(i => i.Name)).ToArray());
                        file.Write(headers + eol);

                        var getters = fields.Select(field => (Func<TItem, string>) (i => field.GetValue(i).ToString()))
                            .Union(
                                properties.Select(prop => (Func<TItem, string>) (i => prop.GetValue(i, null).ToString())));
                        var all = GetAll();
                        var datas = all.Select((i, index) => string.Join(separator, getters.Select(g => g(i)).ToArray()));
                        foreach (var data in datas)
                        {
                            file.Write(data);
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