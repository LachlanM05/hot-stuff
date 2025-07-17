using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class TutorialDrone : MonoBehaviour, IReloadHandler
{
	// Token: 0x06000763 RID: 1891 RVA: 0x00029F5D File Offset: 0x0002815D
	private void Start()
	{
		this.DisappearDrone();
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00029F65 File Offset: 0x00028165
	public void DisappearDrone()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x00029F73 File Offset: 0x00028173
	public int Priority()
	{
		return 500;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00029F7A File Offset: 0x0002817A
	public void LoadState()
	{
		if (Singleton<Save>.Instance.GetTutorialFinished())
		{
			this.DisappearDrone();
		}
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00029F8E File Offset: 0x0002818E
	public void SaveState()
	{
	}
}
