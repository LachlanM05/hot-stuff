using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000049 RID: 73
public class DemoMenuButton : MonoBehaviour
{
	// Token: 0x060001DE RID: 478 RVA: 0x0000B98B File Offset: 0x00009B8B
	public void Start()
	{
		this.demoCamera.SetActive(true);
		Singleton<PhoneManager>.Instance.ClosePhoneAsync(null, false);
		CursorLocker.Unlock();
		this.canvasUIManager.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000B9BB File Offset: 0x00009BBB
	public void OnDisable()
	{
		this.demoStarted = false;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000B9C4 File Offset: 0x00009BC4
	public void Update()
	{
		if (this.demoScreen.activeInHierarchy && !this.demoCamera.activeInHierarchy)
		{
			this.demoCamera.SetActive(true);
		}
		if (this.canvasUIManager.GetComponent<Canvas>().renderMode != RenderMode.ScreenSpaceCamera)
		{
			this.canvasUIManager.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000BA1C File Offset: 0x00009C1C
	public void ConfirmPlayDemo()
	{
		Singleton<Save>.Instance.NewGame();
		Singleton<Save>.Instance.SetPlayerName("PLAYER");
		Singleton<Save>.Instance.SetPlayerPronouns(0);
		Singleton<Save>.Instance.SetPlayerTown("DEMO");
		base.StartCoroutine(this.AnimateToNewGame());
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000BA6C File Offset: 0x00009C6C
	public void PlayDemo()
	{
		if (!this.demoStarted)
		{
			this.demoStarted = true;
			UnityEvent unityEvent = new UnityEvent();
			unityEvent.AddListener(new UnityAction(this.ConfirmPlayDemo));
			Singleton<Popup>.Instance.CreatePopup("Date Everything", "We hope you enjoy our demo, please be aware that the game is still in active development, may contain bugs, unfinished content, and is not a true reflection of the final product!", unityEvent, false);
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000BAB6 File Offset: 0x00009CB6
	private IEnumerator AnimateToNewGame()
	{
		yield return new WaitForEndOfFrame();
		Singleton<Save>.Instance.SetInkVariables();
		yield return new WaitForSeconds(1f);
		this.uiUtilities.ShowLoadingScreen(true);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.start_game, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		yield return new WaitForSeconds(0.5f);
		Singleton<ChatMaster>.Instance.ClearChatHistory();
		Singleton<Save>.Instance.CallGameLoad();
		this.uiUtilities.FadeOutTrack("main_menu");
		this.uiUtilities.LoadSceneAsyncSingle(SceneConsts.kGameScene, false);
		Singleton<PhoneManager>.Instance.mainMenu = this.demoScreen;
		Singleton<PhoneManager>.Instance.demoCamera = this.demoCamera;
		this.demoScreen.SetActive(false);
		yield break;
	}

	// Token: 0x040002CA RID: 714
	public UIUtilities uiUtilities;

	// Token: 0x040002CB RID: 715
	public CanvasUIManager canvasUIManager;

	// Token: 0x040002CC RID: 716
	public GameObject demoScreen;

	// Token: 0x040002CD RID: 717
	public GameObject demoCamera;

	// Token: 0x040002CE RID: 718
	private bool demoStarted;
}
