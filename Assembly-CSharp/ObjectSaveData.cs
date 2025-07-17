using System;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x0200006E RID: 110
[JsonConverter(typeof(ObjectSaveDataConverter))]
public class ObjectSaveData
{
	// Token: 0x060003BC RID: 956 RVA: 0x00017A2C File Offset: 0x00015C2C
	public ObjectSaveData(string gameObjectName)
	{
		this.gameObjectName = gameObjectName;
		this.activeSelf = true;
		this.activatedAnimation = false;
		this.isClean = false;
		this.hasNormalInteracted = false;
		this.positionWhenInteracted = new Vector3(0f, 0f, 0f);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00017A7C File Offset: 0x00015C7C
	[JsonConstructor]
	public ObjectSaveData(string gameObjectName, bool activeSelf, bool activatedAnimation, bool isClean, bool hasNormalInteracted, Vector3 positionWhenInteracted)
	{
		this.gameObjectName = gameObjectName;
		this.activeSelf = activeSelf;
		this.activatedAnimation = activatedAnimation;
		this.isClean = isClean;
		this.hasNormalInteracted = hasNormalInteracted;
		this.positionWhenInteracted = positionWhenInteracted;
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00017AB1 File Offset: 0x00015CB1
	public void UpdateSaveData(string gameObjectName, bool activeSelf)
	{
		this.UpdateSaveData(gameObjectName, activeSelf, this.activatedAnimation, this.isClean, this.hasNormalInteracted, this.positionWhenInteracted);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00017AD3 File Offset: 0x00015CD3
	public void UpdateSaveData(string gameObjectName, bool activeSelf, bool activatedAnimation, bool isClean, bool hasNormalInteracted, Vector3 positionWhenInteracted)
	{
		this.gameObjectName = gameObjectName;
		this.activeSelf = activeSelf;
		this.activatedAnimation = activatedAnimation;
		this.isClean = isClean;
		this.hasNormalInteracted = hasNormalInteracted;
		this.positionWhenInteracted = positionWhenInteracted;
	}

	// Token: 0x040003B1 RID: 945
	public string gameObjectName;

	// Token: 0x040003B2 RID: 946
	public bool activeSelf;

	// Token: 0x040003B3 RID: 947
	public bool activatedAnimation;

	// Token: 0x040003B4 RID: 948
	public bool isClean;

	// Token: 0x040003B5 RID: 949
	public bool hasNormalInteracted;

	// Token: 0x040003B6 RID: 950
	public Vector3 positionWhenInteracted;
}
