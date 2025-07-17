using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class ObjectSaveDataConverter : JsonConverter<ObjectSaveData>
{
	// Token: 0x060005C4 RID: 1476 RVA: 0x00020DA4 File Offset: 0x0001EFA4
	public override ObjectSaveData ReadJson(JsonReader reader, Type objectType, ObjectSaveData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JObject jobject = JObject.Load(reader);
		string @string = jobject.GetString("gn", "gameObjectName", "");
		bool @bool = jobject.GetBool("as", "activeSelf", false);
		bool bool2 = jobject.GetBool("aa", "activatedAnimation", false);
		bool bool3 = jobject.GetBool("ic", "isClean", false);
		bool bool4 = jobject.GetBool("hni", "hasNormalInteracted", false);
		Vector3 vector = jobject.GetVector3("pwi", "positionWhenInteracted", ObjectSaveDataConverter.DefaultVector3Value, serializer);
		return new ObjectSaveData(@string, @bool, bool2, bool3, bool4, vector);
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00020E3C File Offset: 0x0001F03C
	public override void WriteJson(JsonWriter writer, ObjectSaveData value, JsonSerializer serializer)
	{
		JObject jobject = new JObject();
		jobject["gn"] = value.gameObjectName;
		JObject jobject2 = jobject;
		if (value.activeSelf)
		{
			jobject2["as"] = value.activeSelf;
		}
		if (value.activatedAnimation)
		{
			jobject2["aa"] = value.activatedAnimation;
		}
		if (value.isClean)
		{
			jobject2["ic"] = value.isClean;
		}
		if (value.hasNormalInteracted)
		{
			jobject2["hni"] = value.hasNormalInteracted;
		}
		if (value.positionWhenInteracted != ObjectSaveDataConverter.DefaultVector3Value)
		{
			jobject2["pwi"] = JObjectExtensions.FromVector3(value.positionWhenInteracted);
		}
		jobject2.WriteTo(writer, Array.Empty<JsonConverter>());
	}

	// Token: 0x0400057A RID: 1402
	private const bool DefaultBoolValue = false;

	// Token: 0x0400057B RID: 1403
	private static readonly Vector3 DefaultVector3Value;

	// Token: 0x0400057C RID: 1404
	private const string GameObjectNameFullName = "gameObjectName";

	// Token: 0x0400057D RID: 1405
	private const string GameObjectNameShortName = "gn";

	// Token: 0x0400057E RID: 1406
	private const string ActiveSelfFullName = "activeSelf";

	// Token: 0x0400057F RID: 1407
	private const string ActiveSelfShortName = "as";

	// Token: 0x04000580 RID: 1408
	private const string ActivatedAnimationFullName = "activatedAnimation";

	// Token: 0x04000581 RID: 1409
	private const string ActivatedAnimationShortName = "aa";

	// Token: 0x04000582 RID: 1410
	private const string IsCleanFullName = "isClean";

	// Token: 0x04000583 RID: 1411
	private const string IsCleanShortName = "ic";

	// Token: 0x04000584 RID: 1412
	private const string HasNormalInteractedFullName = "hasNormalInteracted";

	// Token: 0x04000585 RID: 1413
	private const string HasNormalInteractedShortName = "hni";

	// Token: 0x04000586 RID: 1414
	private const string PositionWhenInteractedFullName = "positionWhenInteracted";

	// Token: 0x04000587 RID: 1415
	private const string PositionWhenInteractedShortName = "pwi";
}
