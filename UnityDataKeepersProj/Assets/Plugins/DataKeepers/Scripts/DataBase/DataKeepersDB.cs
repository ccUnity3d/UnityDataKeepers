using System.IO;
using Mono.Data.Sqlite;
using SQLite;

namespace DataKeepers.DataBase
{
    public class DataKeepersDb
    {
        private SQLiteConnection _dbc;

        private static DataKeepersDb _instance;
        public static DataKeepersDb Instance { get { return _instance ?? (_instance = new DataKeepersDb()); } }
        public bool Connected { get { return _dbc != null; }}

        public SQLiteConnection Connect()
        {
            if (!DataKeepersPaths.Exists)
            {
                CreateEmpty();
            }
            _dbc = new SQLiteConnection(DataKeepersPaths.DataBasePath);
            return _dbc;
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
    }
}