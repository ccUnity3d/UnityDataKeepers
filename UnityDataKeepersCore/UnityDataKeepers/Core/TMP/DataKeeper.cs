namespace UnityDataKeepersCore.Core.Model
{
    public abstract class DataKeeper<TItem> : DataProvider<TItem>, IDataKeeper<TItem> where TItem:IDataKeeperItem
    {
        private DataKeeper() { }

        public Events.AddKeeperItemEvent OnItemAdd
        {
            get { throw new System.NotImplementedException(); }
        }

        public Events.DeleteKeeperItemEvent OnItemDelete
        {
            get { throw new System.NotImplementedException(); }
        }

        public Events.UpdateKeeperItemEvent OnItemUpdate
        {
            get { throw new System.NotImplementedException(); }
        }

        public KeeperSavePoliticType SavePoliticType
        {
            get { throw new System.NotImplementedException(); }
        }

        public TItem GetBuyId(string id)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TItem> GetAllById(string id)
        {
            throw new System.NotImplementedException();
        }

        public TItem Find(System.Predicate<TItem> predicate)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TItem> FindAll(System.Predicate<TItem> predicate)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TItem> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public bool Add(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public int Add(System.Collections.Generic.IEnumerable<TItem> items)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public int Remove(System.Collections.Generic.IEnumerable<TItem> items)
        {
            throw new System.NotImplementedException();
        }

        public int Remove(System.Predicate<TItem> predicate)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public int Count()
        {
            throw new System.NotImplementedException();
        }

        public bool Update(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public bool AddOrUpdate(TItem item)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(TItem other)
        {
            throw new System.NotImplementedException();
        }
    }
}
