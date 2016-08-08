using System;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public interface IRevision
    {
        DateTime CreationDate { get; }
        string RevisionRole { get; }
        IRevisionDataCollector DataCollector { get; }
    }
}