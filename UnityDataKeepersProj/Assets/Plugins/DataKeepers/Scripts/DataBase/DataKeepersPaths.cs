using System;
using System.IO;
using UnityEngine;

namespace DataKeepers.DataBase
{
    public static class DataKeepersPaths
    {
        public static bool Exists
        {
            get { return File.Exists(DataBasePath); }
        }

        public static string DataBasePath
        {
            get
            {
                return String.Format("{0}/{1}", DataKeepersDir, DefaultDataBasePath);
            }
        }

        private static string DataKeepersDir
        {
            get { return Path.Combine(Application.streamingAssetsPath, "DataKeepers"); }
        }

        private static object DefaultDataBasePath
        {
            get { return "DataKeepers.db3"; }
        }
    }
}