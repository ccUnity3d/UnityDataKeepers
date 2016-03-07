using System;
using System.Collections.Generic;
using DataKeepers.DataBase;
using SQLite;
using UnityEngine;
using UnityEditor;
using UnityEngineInternal;

namespace DataKeepers.Manager
{
    public class DataKeepersManager : EditorWindow
    {
        private const string LoadUrlFormat =
            "http://script.google.com/macros/s/{0}/exec?keeperId={1}";

        private DataKeepersManagerEditorData _editorData = new DataKeepersManagerEditorData();

        private string _backendScriptId;
        private string _backendMainKeeperId;

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
            var versions = _editorData.GetVersions();
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
            string GetKeeperUrl = string.Format(LoadUrlFormat, _backendScriptId, _backendMainKeeperId);
            Debug.Log("Start loading keeper from " + GetKeeperUrl);
            var www = new WWW(GetKeeperUrl);
            ContinuationManager.Add(() => www.isDone, () =>
            {
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(string.Format("Failed to load main keepers from URL [{0}].\nError: {1}", GetKeeperUrl, www.error));
                }
                else
                {
                    try
                    {
                        var version = new KeeperVersion(www.text);
                        _editorData.SaveKeeperVersion(version);
                        Debug.Log(string.Format("Keeper was loaded and saved!\nLoaded data:\n{0}",www.text));
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
    }
}