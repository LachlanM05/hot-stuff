using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000163 RID: 355
public class EmoteAnim : Singleton<EmoteAnim>
{
	// Token: 0x04000BA2 RID: 2978
	public const string k_AnimRegular = "UnlockCollectable";

	// Token: 0x04000BA3 RID: 2979
	public const string k_AnimHold = "UnlockCollectableHold";

	// Token: 0x04000BA4 RID: 2980
	public const string k_AnimRelease = "UnlockCollectableRelease";

	// Token: 0x04000BA5 RID: 2981
	[FormerlySerializedAs("Anger")]
	public GameObject Angry;

	// Token: 0x04000BA6 RID: 2982
	public GameObject Joy;

	// Token: 0x04000BA7 RID: 2983
	public GameObject Love;

	// Token: 0x04000BA8 RID: 2984
	public GameObject Love1;

	// Token: 0x04000BA9 RID: 2985
	public GameObject Love2;

	// Token: 0x04000BAA RID: 2986
	public GameObject Love3;

	// Token: 0x04000BAB RID: 2987
	public GameObject Disgust;

	// Token: 0x04000BAC RID: 2988
	public GameObject Sweat;

	// Token: 0x04000BAD RID: 2989
	public GameObject Good;

	// Token: 0x04000BAE RID: 2990
	public GameObject Bad;

	// Token: 0x04000BAF RID: 2991
	public GameObject Resolve;

	// Token: 0x04000BB0 RID: 2992
	public GameObject Violent;

	// Token: 0x04000BB1 RID: 2993
	public GameObject Sing;

	// Token: 0x04000BB2 RID: 2994
	public GameObject Sleep;
}
