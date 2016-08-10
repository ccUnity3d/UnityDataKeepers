using System;
using UnityDataKeepersCore.Core.DataLayer.DataCollectionDrivers;
using UnityDataKeepersCore.Core.DataLayer.Model;

namespace UnityDataKeepersCore.Core.DataLayer.DataCollections
{
    public interface IDataCollection
    {
        IDataCollectionSignature Signature { get; }
        Type CollectionDriverType { get; }
        IDataCollectionDriver<T> GetDriver<T>() where T : class, IDataItem;
    }
}
