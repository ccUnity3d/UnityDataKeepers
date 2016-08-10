using System;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public static class RevisionFactory
    {
        public static IRevision CreateRevision()
        {
            return new Revision();
        }
    }

    public class Revision : IRevision
    {
        public DateTime CreationDate { get; private set; }
        public string RevisionRole { get; private set; }
        public IRevisionDataCollector DataCollector { get; private set; }
    }
}
