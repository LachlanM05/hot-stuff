using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class HideableInteractable : MonoBehaviour, IReloadHandler
{
	// Token: 0x060004C2 RID: 1218 RVA: 0x0001CD74 File Offset: 0x0001AF74
	public void Start()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0001CD82 File Offset: 0x0001AF82
	public void LoadState()
	{
		if (Singleton<Save>.Instance.GetInteractableState(base.gameObject.tag))
		{
			base.gameObject.SetActive(true);
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0001CDB4 File Offset: 0x0001AFB4
	public void SaveState()
	{
		Singleton<Save>.Instance.SetInteractableState(base.gameObject.tag, base.gameObject.activeInHierarchy);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0001CDD6 File Offset: 0x0001AFD6
	public int Priority()
	{
		return 1000;
	}
}
