using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite; 
using SQLite;
using UnityEngine;

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
            //            ConnectTo(DataKeepersPaths.DataBasePath);

            var DatabaseName = DataKeepersPaths.DataBasePathInStreamingAssets;

#if UNITY_EDITOR

            var dir = Path.GetDirectoryName(DatabaseName);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
            // check if file exists in Application.persistentDataPath
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);
       
            if (!File.Exists(filepath))
            {
                var dir = Path.GetDirectoryName(filepath);
                if (dir != null && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
                try
                {
                    Debug.Log("Start loading keepers from '" + "jar:file://" + Application.dataPath + "!/assets/" + DatabaseName + "'");
                    var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
                    while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                    // then save to Application.persistentDataPath
                    Debug.Log("Successfully loaded! Start writing to " + "'" + filepath + "'");
                    File.WriteAllBytes(filepath, loadDb.bytes);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error when reading keepers: "+e.Message);
                }
#elif UNITY_IOS
                var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
           
#elif UNITY_WINRT
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#endif
           
                Debug.Log("Database written");
            }
       
            var dbPath = filepath;
#endif
//            Debug.Log("Final PATH: " + dbPath);
            _dbc = new SQLiteConnection(dbPath);
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

        public List<T> GetQuery<T>(Predicate<T> selectIf) where T : new()
        {
            return GetQuery<T>().FindAll(selectIf);
        }

        public List<T> GetQuery<T>() where T : new()
        {
            return _dbc == null ? new List<T>() : Query<T>(string.Format("SELECT * FROM {0}", typeof(T).Name));
        }

        /// <summary>
        /// Updates all of the columns of a table using the specified object
        /// except for its primary key.
        /// The object is required to have a primary key.
        /// </summary>
        /// <param name="obj">
        /// The object to update. It must have a primary key designated using the PrimaryKeyAttribute.
        /// </param>
        /// <returns>
        /// The number of rows updated.
        /// </returns>
        public int Update<T>(T obj)
        {
            return _dbc == null ? 0 : _dbc.Update(obj);
        }

        public void Remove<T>(T item)
        {
            _dbc.Delete(item);
        }

        public int GetCount<T>() where T : new()
        {
            return _dbc.Table<T>().Count();
        }

        public void DeleteAll<T>()
        {
            _dbc.DeleteAll<T>();
        }

        public bool TableExists<T>()
        {
            return GetTablesInfos().Exists(t => t.Name == typeof (T).Name);
        }
    }

    public class TableInfo
    {
        public string Name { get; set; }
    }
}