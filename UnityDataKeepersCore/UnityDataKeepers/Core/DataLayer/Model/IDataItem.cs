﻿using System;

namespace UnityDataKeepersCore.Core.DataLayer.Model
{
    public interface IDataItem : IComparable
    {
        Guid Guid { get; set; }
    }
}