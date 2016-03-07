using JsonDotNet.Extras.CustomConverters;
using Newtonsoft.Json;
using UnityEngine;

public class SerializableObject
{
    private static readonly JsonConverter[] Converters = {
        new Vector2Converter(),
        new Vector3Converter(),
        new Vector4Converter()
    };

    [SerializeField] public string Id = "";

    public string Justify()
    {
        return JsonConvert.SerializeObject(this, Converters);
    }

    public override string ToString()
    {
        return Justify();
    }
}
