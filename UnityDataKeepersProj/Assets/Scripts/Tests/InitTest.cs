using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitTest : MonoBehaviour
{
    public Text TestLog;

    void Awake()
    {
        var id = "text1";
        var loc = LocalizationKeeper.Instance.GetById(id);
        TestLog.text+=string.Format("localization for id {0}: {1}", id, loc == null ? "null" : loc.Justify());
        TestLog.text += string.Format("resource for id {0}: {1}", "wood", ResourcesKeeper.Instance.GetById("wood"));
        TestLog.text += string.Format("resource for id {0}: {1}", "wood2", ResourcesKeeper.Instance.GetById("wood2"));

        var res = ResourcesKeeper.Instance.GetById("wood");
        res.DebugDescription += ((int) (Random.value*10)).ToString();
        ResourcesKeeper.Instance.Update(res);
        TestLog.text += string.Format("resource for id {0}: {1}", "wood", ResourcesKeeper.Instance.GetById("wood"));
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}