using JsonDotNet.Extras.CustomConverters;
using Newtonsoft.Json;
using SQLite;
using UnityEngine;

public class SerializableObject
{
    private static readonly JsonConverter[] Converters = {
        new Vector2Converter(),
        new Vector3Converter(),
        new Vector4Converter()
    };

    [PrimaryKey,SerializeField] public string Id { get; set; }

    public string Justify()
    {
        return JsonConvert.SerializeObject(this, Converters);
    }

    public override string ToString()
    {
        return Justify();
    }
}
