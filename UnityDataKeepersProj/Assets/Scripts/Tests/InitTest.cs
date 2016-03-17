using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InitTest : MonoBehaviour
{
//    public Text TestLog;
//
//    private int _t = 0;
//    private int _num = 0;
//
//    void Awake()
//    {
//        CustomKeeper.Instance.GetById("test1");
//        CustomKeeper.Instance.Add(new CustomKeeperItem() {Id = Random.value.ToString(), SomeData = "data"});
//        Debug.Log(CustomKeeper.Instance.Count());
//
//        StartCoroutine(TestProc());
//    }
//
//    IEnumerator TestProc()
//    {
//        while (true)
//        {
//            EffectsKeeper.Instance.FindAll(e => e.Id == "depot_get" && e.ResultType == "Add");
//            _num++;
//            yield return null;
//        }
//    }
//
//    void Update()
//    {
//        if ((int) (Time.time) > _t)
//        {
//            _t = (int) (Time.time);
//            Debug.Log(_num);
//            _num = 0;
//        }
//    }
//
//    public void Restart()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }
}