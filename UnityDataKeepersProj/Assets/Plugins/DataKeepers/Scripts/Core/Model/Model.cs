using System;
using System.Collections;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class Model : CharpSingleton<Model>
{
    private bool _isInited = false;

    public bool IsInited
    {
        get { return _isInited; }
        private set
        {
            if (!value) return;
            _isInited = value;
            OnInited.Invoke();
        }
    }

    public UnityEvent OnInited = new UnityEvent();

    public static string KeeperFilePath
    {
        get { return Application.streamingAssetsPath + "/Keepers/keepers.json"; }
    }

    public void InitKeepers()
    {
        LoadKeeperFormFile(KeeperFilePath);
    }

    public void LoadKeeperFormFile(string path)
    {
        GameObject.FindObjectOfType<MonoBehaviour>().StartCoroutine(LoadKeeperCoroutine(path));
    }

    private IEnumerator LoadKeeperCoroutine(string filePath)
    {
        if (PlayerPrefs.HasKey(filePath))
        {
            ParseLoaded(PlayerPrefs.GetString(filePath));
        }
        else if (filePath.Contains("://"))
        {
            var www = new WWW(filePath);
            yield return www;
            var json = www.text;
            if (string.IsNullOrEmpty(www.error))
                ParseLoaded(json);
        }
        else
        {
            if (File.Exists(filePath))
            {
                var result = File.ReadAllText(filePath);
                ParseLoaded(result);
            }
        }
        IsInited = true;
    }

    private void ParseLoaded(string json)
    {
        JsonReader reader = new JsonTextReader(new StringReader(json));
        ReadKeepers(reader);
    }

    private void ReadKeepers(JsonReader reader)
    {
        var log = "Loading keepers...";
        var loaded = 0;
        var currentKeeperTypeName = "";
        var objectLevel = 0;
        var serializer = new JsonSerializer();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    objectLevel++;
                    if (objectLevel > 1)
                    {
                        var keeperType =Type.GetType(currentKeeperTypeName);
                        var itemType = keeperType.BaseType.GetGenericArguments()[1];
                        var item = serializer.Deserialize(reader,itemType);
                        PushObjectToKeeper(keeperType,itemType,item);
                        loaded++;
                        objectLevel--;
                    }
                    break;

                case JsonToken.EndObject:
                    objectLevel--;
                    if (objectLevel == 0)
                    {
                        var t = Type.GetType(currentKeeperTypeName);
                        var p = GetPropertyInBase(t,"Instance");
                        var m = t.GetMethod("Count");
                        var count = m.Invoke(p.GetValue(null, null), null);
                        log += string.Format("loaded objects: {0}, real {1} ", loaded, count);
                        loaded = 0;
                        // model finished!
                    }
                    break;

                case JsonToken.PropertyName:
                    if (objectLevel == 1)
                    {
                        if ((string) reader.Value == "Type")
                        {
                            reader.Read();
                            currentKeeperTypeName = (string) reader.Value;
                            log += string.Format("\r\nKeeper: [{0}] ", currentKeeperTypeName);
                        }
                    }
                    break;
            }
        }
        Debug.Log(log);
    }

    private void PushObjectToKeeper(Type keeperType, Type itemType, object item) {
        var inst = GetPropertyInBase(keeperType,"Instance");
        var method = keeperType.GetMethod("Add",new []{itemType});
        method.Invoke(inst.GetValue(null,null),new []{item});
    }

    private PropertyInfo GetPropertyInBase(Type type, string pName) {
        return type.BaseType==null?null:type.BaseType.GetProperty(pName)??GetPropertyInBase(type.BaseType, pName);
    }
}