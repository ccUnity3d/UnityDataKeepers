using DataKeepers.DataBase;
using SQLite;
using UnityEngine;

public class InitTest : MonoBehaviour
{
    [ContextMenu("Connection test")]
    public void Test()
    {
        Debug.Log(string.Format("Path of db: {0}",DataKeepersPaths.DataBasePath));
        Debug.Log(string.Format("Is db exists? {0}", DataKeepersPaths.Exists));

        Debug.Log("Connection...");
        var conn = DataKeepersDb.Instance.Connect();
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
        if (!DataKeepersDb.Instance.Connected)
        {
            DataKeepersDb.Instance.Connect();
        }
        DataKeepersDb.Instance.GetConnection().CreateTable<TestData>();
        DataKeepersDb.Instance.GetConnection().Insert(new TestData());
    }
}