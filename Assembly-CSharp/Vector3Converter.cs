using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class Vector3Converter : JsonConverter<Vector3>
{
	// Token: 0x06000CC9 RID: 3273 RVA: 0x0004A420 File Offset: 0x00048620
	public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
	{
		JObject.FromObject(new Dictionary<string, float>
		{
			{ "x", value.x },
			{ "y", value.y },
			{ "z", value.z }
		}).WriteTo(writer, Array.Empty<JsonConverter>());
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0004A478 File Offset: 0x00048678
	public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JObject jobject = JObject.Load(reader);
		JToken jtoken = jobject["x"];
		float num = ((jtoken != null) ? jtoken.Value<float>() : 0f);
		JToken jtoken2 = jobject["y"];
		float num2 = ((jtoken2 != null) ? jtoken2.Value<float>() : 0f);
		JToken jtoken3 = jobject["z"];
		float num3 = ((jtoken3 != null) ? jtoken3.Value<float>() : 0f);
		return new Vector3(num, num2, num3);
	}
}
