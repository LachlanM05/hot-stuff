using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class Teleporter_AnimEvents : MonoBehaviour
{
	// Token: 0x0600052E RID: 1326 RVA: 0x0001EAA7 File Offset: 0x0001CCA7
	public void CamOn()
	{
		this.interactable.CamLogic(100);
		if (this.otherInteractable != null)
		{
			this.otherInteractable.CamLogic(-1);
		}
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0001EAD0 File Offset: 0x0001CCD0
	public void CamOff()
	{
		if (this.otherInteractable != null)
		{
			this.otherInteractable.CamLogic(-1);
		}
		this.interactable.CamLogic(-1);
	}

	// Token: 0x04000519 RID: 1305
	[SerializeField]
	private GenericInteractable otherInteractable;

	// Token: 0x0400051A RID: 1306
	[SerializeField]
	private GenericInteractable interactable;
}
