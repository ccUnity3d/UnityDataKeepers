using System.Collections.Generic;
using System;
using System.Collections;
using DataKeepers.DataBase;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace DataKeepers
{
    [Serializable]
    public class Keeper<TKeeper, TItem> : SerializableCharpSingleton<TKeeper>, IEnumerable<TItem> where TKeeper : class where TItem : KeeperItem, new()
    {
        private static DataKeepersDbConnector _dataConnector = null;

        protected Keeper()
        {
            if (_dataConnector != null) return;
            _dataConnector = new DataKeepersDbConnector();
            _dataConnector.ConnectToDefaultStorage();
        }

        public KeeperItemEvent OnAddItem = new KeeperItemEvent();
        public KeeperItemEvent OnDeleteItem = new KeeperItemEvent();

        public TItem GetById(string id)
        {
            var result = _dataConnector.GetQuery<TItem>(i=>i.Id == id);
            return result.Count > 0 ? result[0] : null;
        }

        public List<TItem> GetAllById(string id)
        {
            return _dataConnector.GetQuery<TItem>(i=>i.Id == id);
        }

        public List<TItem> FindAll(Predicate<TItem> predicate)
        {
            return _dataConnector.GetQuery(predicate);
        }

        public virtual bool Add(TItem item)
        {
            if (!Validate(item)) return false;
            try
            {
                _dataConnector.Insert(item);
            }
            catch (Exception e)
            {
                Debug.Log("Can't insert object " + item.Justify() + " because error: " + e.Message);
                return false;
            }
            return true;
        }

        public virtual int Add(IEnumerable<TItem> items)
        {
            if (items == null) return 0;
            int c = 0;
            foreach (var i in items)
                if (Add(i)) c++;
            return c;
        }

        public virtual bool Remove(TItem i)
        {
            try
            {
                _dataConnector.Remove(i);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual int Remove(IEnumerable<TItem> items)
        {
            var c = 0;
            foreach (var i in items)
                if (Remove(i)) c++;
            return c;
        }

        protected virtual bool Validate(TItem obj)
        {
            return true;
        }

        public int Count()
        {
            try
            {
                return _dataConnector.GetCount<TItem>();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public virtual void Clear()
        {
            try
            {
                _dataConnector.DeleteAll<TItem>();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return FindAll(i => true).GetEnumerator();
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