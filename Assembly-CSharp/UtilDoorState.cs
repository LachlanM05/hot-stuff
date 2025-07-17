using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class UtilDoorState : MonoBehaviour, IReloadHandler
{
	// Token: 0x06000546 RID: 1350 RVA: 0x0001F1A8 File Offset: 0x0001D3A8
	public void ToggleOpened()
	{
		this.opened = !this.opened;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0001F1B9 File Offset: 0x0001D3B9
	public void CloseDoorIfNeeded()
	{
		if (this.opened)
		{
			this.opened = false;
			base.gameObject.GetComponent<Animator>().SetTrigger("standardAnimStart");
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0001F1DF File Offset: 0x0001D3DF
	public void LoadState()
	{
		if (Singleton<Save>.Instance.GetTutorialThresholdState(base.tag))
		{
			this.opened = true;
			return;
		}
		this.opened = false;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0001F202 File Offset: 0x0001D402
	public void SaveState()
	{
		if (this.opened)
		{
			Singleton<Save>.Instance.SetTutorialThresholdState(base.tag);
			return;
		}
		Singleton<Save>.Instance.RemoveTutorialThresholdState(base.tag);
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0001F22D File Offset: 0x0001D42D
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x0400052E RID: 1326
	public string doorTag;

	// Token: 0x0400052F RID: 1327
	public bool opened;
}
