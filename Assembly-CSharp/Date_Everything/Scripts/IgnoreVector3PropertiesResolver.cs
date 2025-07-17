using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Date_Everything.Scripts
{
	// Token: 0x02000269 RID: 617
	public class IgnoreVector3PropertiesResolver : DefaultContractResolver
	{
		// Token: 0x0600140A RID: 5130 RVA: 0x00060808 File Offset: 0x0005EA08
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
			if (jsonProperty.DeclaringType == typeof(Vector3) && (jsonProperty.PropertyName == "magnitude" || jsonProperty.PropertyName == "sqrMagnitude"))
			{
				jsonProperty.ShouldSerialize = (object _) => false;
			}
			return jsonProperty;
		}
	}
}
