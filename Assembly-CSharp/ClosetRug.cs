using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000077 RID: 119
public class ClosetRug : MonoBehaviour
{
	// Token: 0x06000405 RID: 1029 RVA: 0x000192C0 File Offset: 0x000174C0
	private void Start()
	{
		this.AnimInteraction.InteractEnded.AddListener(new UnityAction(this.RugMoved));
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x000192DE File Offset: 0x000174DE
	private void RugMoved()
	{
		if (this.MyCollider)
		{
			this.MyCollider.enabled = false;
		}
	}

	// Token: 0x04000405 RID: 1029
	public GenericInteractable AnimInteraction;

	// Token: 0x04000406 RID: 1030
	public Collider MyCollider;
}
