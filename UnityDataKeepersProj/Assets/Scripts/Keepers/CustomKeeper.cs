using UnityEngine;
using System.Collections;
using DataKeepers;

public class CustomKeeper : Keeper<CustomKeeper, CustomKeeperItem>
{
}

public class CustomKeeperItem : KeeperItem
{
    public string SomeData { get; set; }
}