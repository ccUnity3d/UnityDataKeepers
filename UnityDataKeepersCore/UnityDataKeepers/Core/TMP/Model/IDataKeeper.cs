using System;
using System.Collections.Generic;
using UnityDataKeepersCore.Helpers.Events;

namespace UnityDataKeepersCore.Core.TMP.Model
{
    public enum KeeperSavePoliticType
    {
        Sessional,
        Manual
    }

    public interface IDataKeeper<TItem> : IEquatable<TItem> where TItem : IDataKeeperItem
    {
        AddKeeperItemEvent OnItemAdd { get; }
        DeleteKeeperItemEvent OnItemDelete { get; }
        UpdateKeeperItemEvent OnItemUpdate { get; }
        KeeperSavePoliticType SavePoliticType { get; }

        TItem GetBuyId(string id);

        IEnumerable<TItem> GetAllById(string id);

        TItem Find(Predicate<TItem> predicate);

        IEnumerable<TItem> FindAll(Predicate<TItem> predicate);

        IEnumerable<TItem> GetAll();

        bool Add(TItem item);

        int Add(IEnumerable<TItem> items);

        bool Remove(TItem item);

        int Remove(IEnumerable<TItem> items);

        int Remove(Predicate<TItem> predicate);

        void Clear();

        int Count();

        bool Update(TItem item);

        bool AddOrUpdate(TItem item);
    }
}
