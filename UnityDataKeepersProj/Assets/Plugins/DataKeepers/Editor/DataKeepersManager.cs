using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using DataKeepers.DataBase;
using Newtonsoft.Json;
using SQLite;
using UnityEngine;
using UnityEditor;
using Assets.Plugins.DataKeepers.Editor;

namespace DataKeepers.Manager
{
    public class DataKeepersManager : EditorWindow
    {
        private const string LoadUrlFormat =
            "http://script.google.com/macros/s/{0}/exec?keeperId={1}";

        private static string GeneratedFilesDir
        {
            get { return Application.streamingAssetsPath + "/../Scripts/Generated"; }
        }

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
            catch (SQLiteException e)
            {
                Debug.LogError("SQL Error when drawing versions list: " + e.Message);
                EditorGUILayout.EndScrollView();
                _editorData.Load();
                if (_connectionTry < 3)
                {
                    Repaint();
                    _connectionTry++;
                }
            }
            catch
            {
                // ignored
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
            List<Dictionary<string, string>> keepers;
            List<Dictionary<string, string>> items;
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                GetItemsSignatures(reader, out keepers, out items);
            }
            RewriteActualDataSignatures(keepers, items);
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                PushActualData(reader);
            }
        }

        private void GenerateSources(KeeperVersion version)
        {
            try
            {
                var json = version.KeeperJson;
                List<Dictionary<string, string>> keepers;
                List<Dictionary<string, string>> items;
                using (var reader = new JsonTextReader(new StringReader(json)))
                {
                    GetItemsSignatures(reader, out keepers, out items);
                }
                RewriteActualDataSignatures(keepers, items);
                using (var reader = new JsonTextReader(new StringReader(json)))
                {
                    ReadKeepersSignatureAndGenerate(reader);
                }
                using (var reader = new JsonTextReader(new StringReader(json)))
                {
                    PushActualData(reader);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error when creating sources: "+e.Message);
            }
        }

        private static void ReadKeepersSignatureAndGenerate(JsonReader reader)
        {
            ClearGeneratedDirectory();

            int objectLevel = 0;
            Dictionary<string, string> itemSignature = new Dictionary<string, string>();
            Dictionary<string, string> keeperSignature = new Dictionary<string, string>();
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
                            CreateClasses(keeperSignature, itemSignature);
                            keeperSignature.Clear();
                            itemSignature.Clear();
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
                            string name = (string)reader.Value;
                            reader.Read();
                            var typeName = name == "Type" ? (string) reader.Value : reader.ValueType.FullName;
                            keeperSignature.Add(name, typeName);
                        }
                        break;

                    case JsonToken.StartArray:
                        if (objectLevel > 0)
                            itemSignature = ReadItemSignature(reader);
                        break;
                }
            }
        }

        private static void ClearGeneratedDirectory()
        {
            foreach (var path in Directory.GetFiles(GeneratedFilesDir))
            {
                File.Delete(path);
            }
        }

        private static void CreateClasses(Dictionary<string, string> keeperSignature, Dictionary<string, string> itemSignature)
        {
            CodeCompileUnit item = GenerateItem(itemSignature, typeof(KeeperItem));
            GenerateCSharpCode(item, itemSignature["Type"] + ".cs");

            //keeperSignature["Items"] = itemSignature["Type"] + "[]";
            if (keeperSignature.ContainsKey("Items")) keeperSignature.Remove("Items");
            CodeCompileUnit keeper = GenerateKeeper(keeperSignature,
                "Keeper<" + keeperSignature["Type"] + "," + itemSignature["Type"] + ">");
            if (keeper != null)
                GenerateCSharpCode(keeper, keeperSignature["Type"] + ".cs");
        }
        private static CodeCompileUnit GenerateItem(Dictionary<string, string> keeperSignature, Type baseType)
        {
            CodeCompileUnit res = new CodeCompileUnit();
            CodeTypeDeclaration targetClass;
            CodeNamespace samples = new CodeNamespace();

            // header
            samples.Imports.Add(new CodeNamespaceImport("UnityEngine"));
            samples.Imports.Add(new CodeNamespaceImport("System"));

            // class
            targetClass = new CodeTypeDeclaration(keeperSignature["Type"]);
            targetClass.IsClass = true;
            targetClass.IsPartial = true;
            targetClass.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;
            var attr = new CodeAttributeDeclarationCollection();
            attr.Add(new CodeAttributeDeclaration("Serializable"));
            targetClass.CustomAttributes = attr;
            targetClass.BaseTypes.Add(new CodeTypeReference(baseType));

            // fields
            foreach (var member in keeperSignature)
            {
                if (member.Key == "Type")
                    continue;

                MemberInfo[] mi = baseType.GetMember(member.Key);
                if (mi.Length > 0) continue;

                CodeMemberField field = new CodeMemberField();
                field.Attributes = MemberAttributes.Public;
                field.Name = member.Key;
                try
                {
                    field.Type = new CodeTypeReference(Type.GetType(member.Value));
                }
                catch (Exception)
                {
                    field.Type = new CodeTypeReference(member.Value);
                }
                field.CustomAttributes.Add(new CodeAttributeDeclaration("SerializeField"));
                targetClass.Members.Add(field);
            }

            samples.Types.Add(targetClass);
            res.Namespaces.Add(samples);
            return res;
        }

        private static CodeCompileUnit GenerateKeeper(Dictionary<string, string> keeperSignature, string baseType)
        {
            CodeCompileUnit res = new CodeCompileUnit();
            CodeTypeDeclaration targetClass;
            CodeNamespace samples = new CodeNamespace();

            // header
            samples.Imports.Add(new CodeNamespaceImport("UnityEngine"));
            samples.Imports.Add(new CodeNamespaceImport("System"));
            samples.Imports.Add(new CodeNamespaceImport("DataKeepers"));

            // class
            targetClass = new CodeTypeDeclaration(keeperSignature["Type"])
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };
            var attr = new CodeAttributeDeclarationCollection();
            targetClass.CustomAttributes = attr;
            targetClass.BaseTypes.Add(new CodeTypeReference(baseType));

            // fields
            foreach (var member in keeperSignature)
            {
                if (member.Key == "Type"
#if RELEASE
                || member.Key == "DebugDescription"
#endif
)
                    continue;

                CodeMemberField field = new CodeMemberField
                {
                    Attributes = MemberAttributes.Public,
                    Name = member.Key
                };
                try
                {
                    field.Type = new CodeTypeReference(Type.GetType(member.Value));
                }
                catch (Exception)
                {
                    field.Type = new CodeTypeReference(member.Value);
                }
                targetClass.Members.Add(field);
            }

            samples.Types.Add(targetClass);
            res.Namespaces.Add(samples);
            return res;
        }

        public static void GenerateCSharpCode(CodeCompileUnit targetUnit, string fileName)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            if (!Directory.Exists(GeneratedFilesDir))
                Directory.CreateDirectory(GeneratedFilesDir);
            using (StreamWriter sourceWriter = new StreamWriter(GeneratedFilesDir + "/" + fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    targetUnit, sourceWriter, options);
            }
        }

        private void PushActualData(JsonReader reader)
        {
            try
            {
                var current = new DataKeepersDbConnector();
                current.ConnectToDefaultStorage();

                var reading = false;
                var objectLevel = 0;

                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.StartObject:
                            objectLevel++;
                            if (reading && objectLevel > 1)
                            {
                                objectLevel--;
                                var itemSignature = ReadItemValues(reader);
                                var query = CreateIntestItemQuery(itemSignature);
                                current.Query(query);
                            }
                            break;

                        case JsonToken.EndObject:
                            objectLevel--;
                            break;

                        case JsonToken.StartArray:
                            if (objectLevel > 0)
                                reading = true;
                            break;

                        case JsonToken.EndArray:
                            if (objectLevel == 1)
                                reading = false;
                            break;
                    }
                }

                current.Close();
                Debug.Log("Current data pushed successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError("Error when pushing data do db: " + e.Message);
            }
        }

        private string CreateIntestItemQuery(Dictionary<string, object> item)
        {
            var names = item.Where(pair => pair.Key != "Type").Select(pair => pair.Key).ToArray();
            var values =
                item.Where(pair => pair.Key != "Type")
                    .Select(pair => string.Format("'{0}'", pair.Value))
                    .ToArray();
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", item["Type"], string.Join(",", names),
                string.Join(",", values));
        }

        private void RewriteActualDataSignatures(List<Dictionary<string, string>> keepers, List<Dictionary<string, string>> items)
        {
            try
            {
                DataKeepersDbConnector current = new DataKeepersDbConnector();
                current.ConnectToDefaultStorage();
                current.DropTableIfExists(typeof (KeeperSignature).Name);
                current.CreateTable<KeeperSignature>();
                // generate sql
                foreach (var keeper in keepers)
                {
                    var kSignature = new KeeperSignature
                    {
                        KeeperName = keeper["Type"],
                        ItemType = keeper["Items"]
                    };
                    current.Insert(kSignature);
                }

                foreach (var item in items)
                {
                    current.DropTableIfExists(item["Type"]);
                    var query = GenerateCreateTableQueryFromSignature(item);
                    if (!current.Query(query))
                        Debug.Log("Error when executing creating of table " + item["Type"]);
                }
                current.Close();
                Debug.Log("Current signatures rewrited successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError("Signature rewriting error: "+e.Message);
            }
        }

        private string GenerateCreateTableQueryFromSignature(Dictionary<string, string> item)
        {
            var attrs = new string[item.Count-1];
            var pos = 0;
            foreach (var data in item)
            {
                if (data.Key == "Type") continue;
                var type = data.Value;
                if (type == typeof(bool).FullName) type = "BOOLEAN";
                else if (type == typeof(string).FullName) type = "VARCHAR";
                else if (type == typeof(int).FullName) type = "INT";
                else if (type == typeof(long).FullName) type = "BIGINT";
                else if (type == typeof(float).FullName) type = "FLOAT";
                else type = "VARCHAR";
                attrs[pos++] = string.Format("{0} {1}", data.Key, type);
            }
            var q = string.Format("CREATE TABLE {0} ({1});", item["Type"], string.Join(",", attrs));
            return q;
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
                            var name = (string) reader.Value;
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
                        var typeName = name == "Type" ? (string)reader.Value : reader.ValueType.FullName;
                        if (reader.ValueType == typeof (double)) typeName = typeof (float).FullName;
                        res.Add(name, typeName);
                        break;

                    case JsonToken.EndObject:
                        return res;
                }
            }
            return res;
        }

        private static Dictionary<string, object> ReadItemValues(JsonReader reader)
        {
            var res = new Dictionary<string, object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        var name = reader.Value as string;
                        reader.Read();
                        res.Add(name, reader.Value);
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
        private DataKeepersDbConnector _dbEditor;
        public bool IsLoaded { get; private set; }

        public DataKeepersManagerEditorData()
        {
            IsLoaded = false;
        }

        public void Load()
        {
            _dbEditor = new DataKeepersDbConnector();
            _dbEditor.ConnectTo(Application.dataPath + "/../Library/DataKeeper.db3");
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