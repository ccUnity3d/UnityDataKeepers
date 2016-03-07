using System;
using JsonDotNet.Extras.CustomConverters;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class SerializableCharpSingleton<T> : CharpSingleton<T> where T : class
{
    protected SerializableCharpSingleton():base() {}

    private static readonly JsonConverter[] Converters = {
        new Vector2Converter(),
        new Vector3Converter(),
        new Vector4Converter()
    };

    [SerializeField]
    public string Id = "";

    public string Justify()
    {
        return JsonConvert.SerializeObject(this, Converters);
    }
}