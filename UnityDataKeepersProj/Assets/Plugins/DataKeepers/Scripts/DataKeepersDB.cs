using System.Data;
using System.IO;
using Mono.Data.Sqlite;

// ReSharper disable once InconsistentNaming
namespace DataKeepers
{
    public class DataKeepersDB
    {
        private string _constr = "";
        private IDbConnection _dbc;
        private IDbCommand _dbcm;

        private static DataKeepersDB _instance;
        public static DataKeepersDB Instance { get { return _instance ?? (_instance = new DataKeepersDB()); } }

        public IDbConnection Connect()
        {
            if (!DataKeepersPaths.Exists)
            {
                CreateEmpty();
            }
            _constr = string.Format("URI=file:{0}", DataKeepersPaths.DataBasePath);
            _dbc = new SqliteConnection(_constr);
            _dbc.Open();
            _dbcm = _dbc.CreateCommand();
            return _dbc;
        }

        public IDbCommand GetCommand()
        {
            return _dbcm;
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