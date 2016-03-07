using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace JsonDotNet.Extras.CustomConverters
{
	public class Vector4Converter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
            Vector4 v = (Vector4)value;
            var o = new JObject();
            o.AddFirst(new JProperty("w", v.w));
            o.AddFirst(new JProperty("z", v.z));
            o.AddFirst(new JProperty("y", v.y));
            o.AddFirst(new JProperty("x", v.x));
            o.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector4);
		}
	}
}
