using System;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class JerryDrawer : MonoBehaviour
{
	// Token: 0x06000553 RID: 1363 RVA: 0x0001F384 File Offset: 0x0001D584
	public void CheckForEnding()
	{
		if (Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Friend || Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Love)
		{
			this.genInt.blockMagical = false;
			return;
		}
		this.genInt.blockMagical = true;
	}

	// Token: 0x04000536 RID: 1334
	[SerializeField]
	private GenericInteractable genInt;

	// Token: 0x04000537 RID: 1335
	[SerializeField]
	private InteractableObj interactableObj;
}
