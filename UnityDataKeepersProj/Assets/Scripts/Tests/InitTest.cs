using DataKeepers;
using UnityEngine;

public class InitTest : MonoBehaviour
{
    [ContextMenu("Connection test")]
    public void Test()
    {
        Debug.Log(string.Format("Path of db: {0}",DataKeepersPaths.DataBasePath));
        Debug.Log(string.Format("Is db exists? {0}", DataKeepersPaths.Exists));

        Debug.Log("Connection...");
        var conn = DataKeepersDB.Instance.Connect();
        Debug.Log("Connected? "+(conn!=null));
    }
}