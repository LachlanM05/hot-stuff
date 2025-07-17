using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public abstract class SubInteractable : Interactable
{
	// Token: 0x06000572 RID: 1394 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
	public override bool CheckCanUse()
	{
		return base.CheckCanUse();
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0001F9C8 File Offset: 0x0001DBC8
	public override void ToggleInteractedWith(Vector3 positionWhenInteracting, bool loading = false)
	{
		base.ToggleInteractedWith(positionWhenInteracting, false);
	}
}
