using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mono.Data.Sqlite;
using SQLite;

namespace DataKeepers.DataBase
{
    public class DataKeepersDbConnector
    {
        private SQLiteConnection _dbc;
        public bool Connected { get { return _dbc != null; }}

        ~DataKeepersDbConnector()
        {
            if (Connected)
            {
                _dbc.Close();
            }
        }

        public void ConnectToDefaultStorage()
        {
            ConnectTo(DataKeepersPaths.DataBasePath);
        }

        private void CreateEmpty()
        {
            var dir = Path.GetDirectoryName(DataKeepersPaths.DataBasePath);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            SqliteConnection.CreateFile(DataKeepersPaths.DataBasePath);
        }

        public void ConnectTo(string dataBasePath)
        {
            if (!DataKeepersPaths.Exists)
            {
                CreateEmpty();
            }
            _dbc = new SQLiteConnection(dataBasePath);
        }

        public List<TableInfo> GetTablesInfos()
        {
            if (_dbc == null) return new List<TableInfo>();
            var infos = _dbc.Query<TableInfo>("SELECT name as Name FROM sqlite_master WHERE type='table'");
            return infos;
        }

        public bool DropTableIfExists(string tableName)
        {
            if (_dbc == null) return false;
            return Query(string.Format("DROP TABLE IF EXISTS {0}", tableName));
        }

        public void CreateTable<T>()
        {
            if (_dbc != null)
                _dbc.CreateTable<T>();
        }

        public void Close()
        {
            if (_dbc != null)
            {
                _dbc.Close();
                _dbc = null;
            }
        }

        public void Insert(object val)
        {
            if (_dbc != null)
            {
                _dbc.Insert(val);
            }
        }

        public bool Query(string query)
        {
            if (_dbc == null) return false;
            var com = _dbc.CreateCommand(query);
            com.ExecuteNonQuery();
            return true;
        }

        public List<T> Query<T>(string query) where T : new()
        {
            if (_dbc == null) return new List<T>();
            return _dbc.Query<T>(query);
        }

        public TableQuery<T> Table<T>() where T : new()
        {
            return _dbc == null ? null : _dbc.Table<T>();
        }
    }

    public class TableInfo
    {
        public string Name { get; set; }
    }
}