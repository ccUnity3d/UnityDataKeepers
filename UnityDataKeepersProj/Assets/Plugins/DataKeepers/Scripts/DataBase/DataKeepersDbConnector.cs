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

        public SQLiteConnection ConnectToDefaultStorage()
        {
            return ConnectTo(DataKeepersPaths.DataBasePath);
        }

        public SQLiteConnection GetConnection()
        {
            return _dbc;
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

        public SQLiteConnection ConnectTo(string dataBasePath)
        {
            if (!DataKeepersPaths.Exists)
            {
                CreateEmpty();
            }
            _dbc = new SQLiteConnection(dataBasePath);
            return _dbc;
        }
    }
}