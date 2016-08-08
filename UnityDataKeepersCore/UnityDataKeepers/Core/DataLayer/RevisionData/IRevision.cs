using System;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public interface IRevision
    {
        DateTime CreationDate { get; }
        IRevisionSignature Signature { get; }
        IRevisionDataCollector DataCollector { get; }
    }
}