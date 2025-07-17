using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public abstract class CustomizationChoice : ScriptableObject
{
	// Token: 0x060006A6 RID: 1702
	public abstract void Set(GameObject target);

	// Token: 0x040005DD RID: 1501
	public string Name;

	// Token: 0x040005DE RID: 1502
	public Sprite Thumbnail;

	// Token: 0x040005DF RID: 1503
	public BodyPart TargetBodyPart;

	// Token: 0x040005E0 RID: 1504
	public bool IsSet;

	// Token: 0x040005E1 RID: 1505
	public bool IsDefault = true;
}
