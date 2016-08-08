using System;

namespace UnityDataKeepersCore.Core.DataLayer.RevisionData
{
    public struct RevisionAttribute
    {
        public bool IsPrimary;
        public string AttributeName;
        public Type AttributeType;
    }
}