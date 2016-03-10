using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InitTest : MonoBehaviour
{
    public Text TestLog;

    void Awake()
    {
//        try
//        {
//            var id = "text1";
//            var loc = LocalizationKeeper.Instance.GetById(id);
//            TestLog.text += string.Format("\nlocalization for id {0}: {1}", id, loc == null ? "null" : loc.Justify());
//            TestLog.text += string.Format("\nresource for id {0}: {1}", "wood", ResourcesKeeper.Instance.GetById("wood"));
//            TestLog.text += string.Format("\nresource for id {0}: {1}", "wood2", ResourcesKeeper.Instance.GetById("wood2"));
//
//            var res = ResourcesKeeper.Instance.GetById("wood");
//            res.DebugDescription += ((int)(Random.value * 10)).ToString();
//            ResourcesKeeper.Instance.Update(res);
//            TestLog.text += string.Format("\nresource for id {0}: {1}", "wood", ResourcesKeeper.Instance.GetById("wood"));
//
//        }
//        catch (Exception e)
//        {
//            TestLog.text += "\nERROR: " + e.Message;
//        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}