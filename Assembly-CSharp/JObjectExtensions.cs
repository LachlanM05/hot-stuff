using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Token: 0x020000AC RID: 172
public static class JObjectExtensions
{
	// Token: 0x0600057F RID: 1407 RVA: 0x0001FC37 File Offset: 0x0001DE37
	public static JToken FromVector3(Vector3 vector3)
	{
		return JToken.FromObject(vector3, new JsonSerializer
		{
			Converters = { JObjectExtensions.Vector3Converter }
		});
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001FC5C File Offset: 0x0001DE5C
	public static bool GetBool(this JObject jObject, string key, string fallbackKey = null, bool defaultValue = false)
	{
		bool flag = defaultValue;
		JToken jtoken;
		if (jObject.TryGetValue(key, out jtoken))
		{
			flag = jtoken.ToObject<bool>();
		}
		else if (!string.IsNullOrEmpty(fallbackKey) && jObject.TryGetValue(fallbackKey, out jtoken))
		{
			flag = jtoken.ToObject<bool>();
		}
		return flag;
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0001FC9C File Offset: 0x0001DE9C
	public static string GetString(this JObject jObject, string key, string fallbackKey = null, string defaultValue = "")
	{
		string text = defaultValue;
		JToken jtoken;
		if (jObject.TryGetValue(key, out jtoken))
		{
			text = jtoken.ToString();
		}
		else if (!string.IsNullOrEmpty(fallbackKey) && jObject.TryGetValue(fallbackKey, out jtoken))
		{
			text = jtoken.ToString();
		}
		return text;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001FCDC File Offset: 0x0001DEDC
	public static Vector3 GetVector3(this JObject jObject, string key, string fallbackKey = null, Vector3 defaultValue = default(Vector3), JsonSerializer serializer = null)
	{
		JToken jtoken;
		if (jObject.TryGetValue(key, out jtoken))
		{
			Vector3? vector = JObjectExtensions.DeserializeVector3(jtoken, serializer);
			if (vector == null)
			{
				return defaultValue;
			}
			return vector.GetValueOrDefault();
		}
		else
		{
			if (string.IsNullOrEmpty(fallbackKey) || !jObject.TryGetValue(fallbackKey, out jtoken))
			{
				return defaultValue;
			}
			Vector3? vector = JObjectExtensions.DeserializeVector3(jtoken, serializer);
			if (vector == null)
			{
				return defaultValue;
			}
			return vector.GetValueOrDefault();
		}
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0001FD40 File Offset: 0x0001DF40
	private static Vector3? DeserializeVector3(JToken token, JsonSerializer serializer)
	{
		Vector3? vector;
		using (JsonReader jsonReader = token.CreateReader())
		{
			vector = (Vector3?)JObjectExtensions.Vector3Converter.ReadJson(jsonReader, typeof(Vector3), default(Vector3), serializer);
		}
		return vector;
	}

	// Token: 0x04000549 RID: 1353
	private static readonly Vector3Converter Vector3Converter = new Vector3Converter();
}
