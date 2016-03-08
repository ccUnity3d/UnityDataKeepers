using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DataKeepers.DataBase;
using Newtonsoft.Json;
using SQLite;
using UnityEngine;
using UnityEditor;
using UnityEngineInternal;
using Assets.Plugins.DataKeepers.Editor;

namespace DataKeepers.Manager
{
    public class DataKeepersManager : EditorWindow
    {
        private const string LoadUrlFormat =
            "http://script.google.com/macros/s/{0}/exec?keeperId={1}";

        private DataKeepersManagerEditorData _editorData = new DataKeepersManagerEditorData();

        private string _backendScriptId;
        private string _backendMainKeeperId;
        private Vector2 _versionsPos;
        private int _connectionTry = 0;

        void OnGUI()
        {
            if (!_editorData.IsLoaded)
                _editorData.Load();

            EditorGUILayout.BeginVertical();

            ShowBackendSettings();
            ShowVersions();

            EditorGUILayout.EndVertical();
        }

        private void ShowVersions()
        {
            try
            {
                _versionsPos = EditorGUILayout.BeginScrollView(_versionsPos);
                var versions = _editorData.GetVersions();
                foreach (var version in versions)
                {
                    ShowVersion(version);
                }
                EditorGUILayout.EndScrollView();
                _connectionTry = 0;
            }
            catch
            {
                EditorGUILayout.EndScrollView();
                _editorData.Load();
                if (_connectionTry < 3)
                {
                    Repaint();
                    _connectionTry++;
                }
            }
        }

        private void ShowVersion(KeeperVersion version)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(string.Format("Id: {0}", version.Id));
            EditorGUILayout.LabelField(string.Format("LoadingDateTime: {0}", version.LoadingDateTime.ToString(CultureInfo.InvariantCulture)));
            EditorGUILayout.LabelField(string.Format("Text length: {0}", version.KeeperJson.Length));
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Generate sources\n(and set as actual)", GUILayout.Height(60))) GenerateSources(version);
            if (GUILayout.Button("Set as actual", GUILayout.Height(60))) SetAsActual(version);
            if (GUILayout.Button("Remove", GUILayout.Height(60))) RemoveVersion(version);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void SetAsActual(KeeperVersion version)
        {
            var json = version.KeeperJson;
            JsonReader reader = new JsonTextReader(new StringReader(json));
            List<Dictionary<string, string>> keepers;
            List<Dictionary<string, string>> items;
            GetItemsSignatures(reader, out keepers, out items);
            RewriteActualDataSignatures(keepers, items);

            //Debug.Log("Keepers: "+JsonConvert.SerializeObject(keepers));
            //Debug.Log("Items signatures: " + JsonConvert.SerializeObject(items));

            Debug.Log("All keepers sucessfully generated!");
        }
        private void GenerateSources(KeeperVersion version)
        {
            throw new NotImplementedException();
        }

        private void RewriteActualDataSignatures(List<Dictionary<string, string>> keepers, List<Dictionary<string, string>> items)
        {
            DataKeepersDbConnector current = new DataKeepersDbConnector();
            var con = current.ConnectToDefaultStorage();
            con.CreateTable<KeeperSignature>();
            con.DeleteAll<KeeperSignature>();
            // generate sql
            foreach (var keeper in keepers)
            {
                var kSignature = new KeeperSignature
                {
                    KeeperName = keeper["Type"],
                    ItemType = keeper["Items"]
                };
                con.Insert(kSignature);
            }
            foreach (var item in items)
            {
                con.CreateCommand("DROP TABLE " + item["Type"]).ExecuteNonQuery();
                // TODO: create query to create item's table and enter items

            }
            con.Close();
        }

        private void GetItemsSignatures(JsonReader reader, out List<Dictionary<string, string>> keepersSignatures,
            out List<Dictionary<string, string>> itemsSignatures)
        {
            keepersSignatures = new List<Dictionary<string, string>>();
            itemsSignatures = new List<Dictionary<string, string>>();
            var objectLevel = 0;
            var itemSignature = new Dictionary<string, string>();
            var keeperSignature = new Dictionary<string, string>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        objectLevel++;
                        break;

                    case JsonToken.EndObject:
                        objectLevel--;
                        if (objectLevel == 0)
                        {
                            keeperSignature["Items"] = itemSignature["Type"];
                            itemsSignatures.Add(itemSignature);
                            keepersSignatures.Add(keeperSignature);
                            keeperSignature = new Dictionary<string, string>();
                            itemSignature = new Dictionary<string, string>();
                        }
                        break;

                    case JsonToken.PropertyName:
                        if (objectLevel == 1)
                        {
                            if ((string)reader.Value == "Items")
                            {
                                keeperSignature.Add("Items", "Array");
                                break;
                            }
                            string name = (string) reader.Value;
                            reader.Read();
                            keeperSignature.Add(name, name == "Type" ? (string) reader.Value : reader.ValueType.FullName);
                        }
                        break;

                    case JsonToken.StartArray:
                        if (objectLevel > 0)
                            itemSignature = ReadItemSignature(reader);
                        break;
                }
            }
        }

        private static Dictionary<string, string> ReadItemSignature(JsonReader reader)
        {
            var res = new Dictionary<string, string>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        var name = reader.Value as string;
                        reader.Read();
                        res.Add(name, name == "Type" ? (string)reader.Value : reader.ValueType.FullName);
                        break;

                    case JsonToken.EndObject:
                        return res;
                }
            }
            return res;
        }

        private void RemoveVersion(KeeperVersion version)
        {
            if (EditorUtility.DisplayDialog("Attention",
                string.Format("Are you sure than you want to remove version {0} created at {1} from base?", version.Id,
                    version.LoadingDateTime), "Yes", "No"))
            {
                _editorData.Remove(version);
                Repaint();
            }
        }

        private void ShowBackendSettings()
        {
            GUILayout.Label("Backend settings");
            if (PlayerPrefs.HasKey("_backendScriptId"))
                _backendScriptId = PlayerPrefs.GetString("_backendScriptId");
            var newScriptId = EditorGUILayout.TextField("Google backend Id",_backendScriptId);
            if (_backendScriptId != newScriptId)
            {
                PlayerPrefs.SetString("_backendScriptId", newScriptId);
                _backendScriptId = newScriptId;
            }
            if (PlayerPrefs.HasKey("_backendMainKeeperId"))
                _backendMainKeeperId = PlayerPrefs.GetString("_backendMainKeeperId");
            var newBackendMainKeeperId = EditorGUILayout.TextField("Main keeper id",_backendMainKeeperId);
            if (_backendMainKeeperId != newBackendMainKeeperId)
            {
                PlayerPrefs.SetString("_backendMainKeeperId",newBackendMainKeeperId);
                _backendMainKeeperId = newBackendMainKeeperId;
            }
            if (GUILayout.Button("Load last keepes"))
            {
                LoadKeepersAndSave();
            }
            EditorGUILayout.Space();
        }

        private void LoadKeepersAndSave()
        {
            var getKeeperUrl = string.Format(LoadUrlFormat, _backendScriptId, _backendMainKeeperId);
            Debug.Log("Start loading keeper from " + getKeeperUrl);
            var www = new WWW(getKeeperUrl);
            ContinuationManager.Add(() => www.isDone, () =>
            {
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(string.Format("Failed to load main keepers from URL [{0}].\nError: {1}", getKeeperUrl, www.error));
                }
                else
                {
                    try
                    {
                        var version = new KeeperVersion(www.text);
                        _editorData.SaveKeeperVersion(version);
                        Debug.Log(string.Format("Keeper was loaded and saved!\nLoaded data:\n{0}",www.text));
                        Repaint();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(string.Format("Keeper was loaded, but not saved.\nSaving error:\n{0}", e.Message));
                    }
                }
            });
        }


        [MenuItem("Window/Data keepers manager")]
        public static void ShowWindow()
        {
            GetWindow(typeof(DataKeepersManager));
        }
    }

    public class KeeperVersion
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public DateTime LoadingDateTime { get; set; }
        public string KeeperJson { get; set; }

        public KeeperVersion(string keeperJson)
        {
            KeeperJson = keeperJson;
            LoadingDateTime = DateTime.Now;
        }

        public KeeperVersion()
        {

        }
    }

    internal class DataKeepersManagerEditorData
    {
        private SQLiteConnection _dbEditor;
        public bool IsLoaded { get; private set; }

        public DataKeepersManagerEditorData()
        {
            IsLoaded = false;
        }

        public void Load()
        {
            var connector = new DataKeepersDbConnector();
            _dbEditor = connector.ConnectTo(Application.dataPath + "/../Library/DataKeeper.db3");
            _dbEditor.CreateTable<KeeperVersion>();
            IsLoaded = true;
        }

        public void SaveKeeperVersion(KeeperVersion version)
        {
            _dbEditor.Insert(version);
        }

        public List<KeeperVersion> GetVersions()
        {
            return _dbEditor.Query<KeeperVersion>("SELECT * FROM KeeperVersion");
        }

        public void Remove(KeeperVersion version)
        {
            _dbEditor.Table<KeeperVersion>().Delete(v => v.Id == version.Id);
        }
    }
}