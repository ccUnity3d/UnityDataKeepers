using System.Collections.Generic;
using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace DataKeepers
{
    [Serializable]
    public class Keeper<T, I> : SerializableCharpSingleton<T>, IEnumerable<I> where T : class where I : KeeperItem
    {
        private class KeeperInnerItem
        {
            public readonly List<I> Items;

            public KeeperInnerItem(I item)
            {
                Items = new List<I> {item};
            }
        }

        private class SerializedData
        {
            public string Type;
            public Dictionary<string, KeeperInnerItem> Items;
        }

        private readonly Dictionary<string, KeeperInnerItem> _items = new Dictionary<string, KeeperInnerItem>(); 

        public KeeperItemEvent OnAddItem = new KeeperItemEvent();
        public KeeperItemEvent OnDeleteItem = new KeeperItemEvent();

        public virtual string ConfigPath
        {
            get { return string.Format("{0}/Keepers/{1}.json", Application.streamingAssetsPath, GetType().Name); }
        }

        protected virtual bool EnableSelfLoading
        {
            get { return false; }
        }

        protected virtual bool EnableStaticLoading
        {
            get { return true; }
        }

        protected override void OnInit()
        {
            base.OnInit();
            if (EnableSelfLoading)
            {
                if (EnableStaticLoading)
                {
                    KeepersLoader.Instance.LoadKeeperFormFile(ConfigPath);
                }
                else
                {
                    SelfLoad();
                }
            }
        }

        public virtual void SelfSave()
        {
            string saveResult = JsonConvert.SerializeObject(new SerializedData { Type = GetType().Name, Items = _items });
            PlayerPrefs.SetString(ConfigPath, saveResult);
            PlayerPrefs.Save();
        }

        protected virtual void SelfLoad()
        {
            string json = PlayerPrefs.GetString(ConfigPath, "");
            if (!string.IsNullOrEmpty(json))
            {
                var serializedData = JsonConvert.DeserializeObject(json, typeof(SerializedData)) as SerializedData;
                _items.Clear();

                if (serializedData == null) return;
                foreach (var kvp in serializedData.Items)
                {
                    //for some reason deserialization adds null element
                    kvp.Value.Items.RemoveAt(0);
                    _items[kvp.Key] = kvp.Value;
                }
            }
	
        }

        public I GetById(string id)
        {
            KeeperInnerItem result;
            if (!_items.TryGetValue(id, out result)) return null;
            return result.Items.Count > 0 ? result.Items[0] : null;
        }

        public List<I> GetAllById(string id)
        {
            KeeperInnerItem result;
            return _items.TryGetValue(id, out result) ? result.Items : new List<I>();
        }

        public List<I> FindAll(Func<I, bool> predicate)
        {
            List<I> result = new List<I>();

            foreach (var item in this)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public virtual bool Add(I item)
        {
            if (!Validate(item)) return false;
            KeeperInnerItem currentInnerItem;
            _items.TryGetValue(item.Id, out currentInnerItem);
            if (currentInnerItem  != null)
            {
                currentInnerItem.Items.Add(item);
            }
            else
            {
                _items.Add(item.Id, new KeeperInnerItem(item));
            }

            try
            {
                OnAddItem.Invoke(item);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return true;
        }

        public virtual int Add(IEnumerable<I> items)
        {
            if (items == null) return 0;
            int c = 0;
            foreach (var i in items)
                if (Add(i)) c++;
            return c;
        }

        public virtual bool Remove(I i)
        {
            if (!_items.ContainsKey(i.Id)) return false;
            if (!_items[i.Id].Items.Contains(i)) return true;
            _items[i.Id].Items.Remove(i);
            try
            {
                OnDeleteItem.Invoke(i);
            }
            catch(Exception e)
            {
                Debug.Log("OnDeleteItem exception " + e);
            }
            return true;
        }

        public virtual int Remove(IEnumerable<I> items)
        {
            var c = 0;
            foreach (var i in items)
                if (Remove(i)) c++;
            return c;
        }

        protected virtual bool Validate(I obj)
        {
            return true;
        }

        public int Count()
        {
            var count = 0;
            for (var i = GetEnumerator(); i.MoveNext();)
                count++;
            return count;
        }

        public virtual void Clear()
        {
            _items.Clear();
        }

        public IEnumerator<I> GetEnumerator()
        {
            foreach (var keeperInnerItem in _items)
            {
                foreach (var item in keeperInnerItem.Value.Items)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class KeeperItemEvent : UnityEvent<KeeperItem>
    {
    
    }
}