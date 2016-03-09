using System;
using DataKeepers.DataBase;
using SQLite;
using UnityEngine;
using Random = UnityEngine.Random;

public class InitTest : MonoBehaviour
{
    [ContextMenu("Connection test")]
    public void Test()
    {
        Debug.Log(string.Format("Path of db: {0}",DataKeepersPaths.DataBasePath));
        Debug.Log(string.Format("Is db exists? {0}", DataKeepersPaths.Exists));

        Debug.Log("Connection...");
        var conn = new DataKeepersDbConnector();
        conn.ConnectToDefaultStorage();
        Debug.Log("Connected? "+(conn!=null));
    }

    public class TestData
    {
        public TestData()
        {
            Id = "Id" + Random.value;
            NumData = (int) (Random.value*100);
            FloatData = Random.value;
        }

        [PrimaryKey] public string Id { get; set; }
        public int NumData { get; set; }
        public float FloatData { get; set; }
    }

    [ContextMenu("Create table test")]
    public void CreateTableTest()
    {
        var conn = new DataKeepersDbConnector();
        conn.ConnectToDefaultStorage();
        conn.CreateTable<TestData>();
        conn.Insert(new TestData());
    }
}