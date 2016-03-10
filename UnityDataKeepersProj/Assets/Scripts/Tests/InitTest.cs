using UnityEngine;

public class InitTest : MonoBehaviour
{
    void Awake()
    {
        var id = "text1";
        var loc = LocalizationKeeper.Instance.GetById(id);
        Debug.LogFormat("localization for id {0}: {1}", id, loc == null ? "null" : loc.Justify());
        Debug.LogFormat("resource for id {0}: {1}", "wood", ResourcesKeeper.Instance.GetById("wood"));
        Debug.LogFormat("resource for id {0}: {1}", "wood2", ResourcesKeeper.Instance.GetById("wood2"));
    }
}