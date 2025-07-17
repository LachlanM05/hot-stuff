using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class Tutorial : MonoBehaviour, IReloadHandler
{
	// Token: 0x0600071F RID: 1823 RVA: 0x00027F51 File Offset: 0x00026151
	public void LoadState()
	{
		if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS))
		{
			this.TutorialInit();
			return;
		}
		this.controller.HideVideoPlayer();
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00027F76 File Offset: 0x00026176
	public int Priority()
	{
		return 10000;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00027F7D File Offset: 0x0002617D
	public void SaveState()
	{
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00027F7F File Offset: 0x0002617F
	private void TutorialInit()
	{
		Singleton<MorningRoutine>.Instance.StartMorningRoutine(0);
	}

	// Token: 0x04000687 RID: 1671
	[SerializeField]
	private CutsceneController controller;
}
