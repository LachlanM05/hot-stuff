using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class Teleporter : Interactable, IReloadHandler
{
	// Token: 0x06000518 RID: 1304 RVA: 0x0001E6C3 File Offset: 0x0001C8C3
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0001E6CA File Offset: 0x0001C8CA
	private void Start()
	{
		this.inAnimation = false;
		bool flag = this.isCrawlspace;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0001E6DC File Offset: 0x0001C8DC
	public void LoadInSet()
	{
		if (this.loadedIn)
		{
			return;
		}
		if (this.teleportedIn)
		{
			Animator animator = this.crawlspaceInteractable.animator;
			this.crawlspaceInteractable.animator = this.crawlspaceInteractable.alternateAnimator;
			this.crawlspaceInteractable.alternateAnimator = animator;
			this.crawlspaceInteractable.VirtualCam = this.crawlspaceInteractable.animator.GetComponentInChildren<CinemachineVirtualCamera>();
			Singleton<DayNightCycle>.Instance.StopMusicInstant();
		}
		this.loadedIn = true;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0001E754 File Offset: 0x0001C954
	public void Update()
	{
		if (this.pendingLoad)
		{
			this.loadedIn = false;
			this.pendingLoad = false;
			this.LoadInSet();
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0001E774 File Offset: 0x0001C974
	public void ResetCams()
	{
		Animator animator = this.crawlspaceInteractable.animator;
		Component alternateAnimator = this.crawlspaceInteractable.alternateAnimator;
		animator.GetComponentInChildren<CinemachineVirtualCamera>().Priority = -1;
		alternateAnimator.GetComponentInChildren<CinemachineVirtualCamera>().Priority = -1;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0001E7B0 File Offset: 0x0001C9B0
	public override void Interact()
	{
		if (this.LocationDown != null && this.LocationUp != null && !this.inAnimation)
		{
			this.inAnimation = true;
			PlayerPauser.Pause();
			CursorLocker.Lock();
			this.ResolveMusic();
			this.TakeControlFromPlayer();
			this.PlayAnimation();
			this.teleportedIn = !this.teleportedIn;
			if (this.teleportedIn)
			{
				Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.UNDERWORLD);
				base.Invoke("TeleportMidEvent", 8f);
				return;
			}
			base.Invoke("TeleportMidEvent", 8f);
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001E849 File Offset: 0x0001CA49
	public void TeleportMidEvent()
	{
		if (!this.teleportedIn)
		{
			Singleton<MorningRoutine>.Instance.setplayerpos(this.LocationUp, false);
		}
		else
		{
			Singleton<MorningRoutine>.Instance.setplayerpos(this.LocationDown, true);
		}
		base.Invoke("TeleportEndAnimation", 1f);
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001E887 File Offset: 0x0001CA87
	public void TeleportEndAnimation()
	{
		this.inAnimation = false;
		PlayerPauser.Unpause();
		if (!this.teleportedIn)
		{
			this.interactedWithState = false;
			this.cameraGoingUp.Priority = -1;
			return;
		}
		this.interactedWithState = true;
		this.cameraGoingDown.Priority = -1;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001E8C4 File Offset: 0x0001CAC4
	public void TakeControlFromPlayer()
	{
		BetterPlayerControl.Instance.ChangePlayerState(BetterPlayerControl.PlayerState.CantControl);
		Singleton<CanvasUIManager>.Instance.Hide();
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001E8DC File Offset: 0x0001CADC
	public void EndTeleport()
	{
		if (this.isCrawlspace)
		{
			Animator animator = this.crawlspaceInteractable.animator;
			this.crawlspaceInteractable.animator = this.crawlspaceInteractable.alternateAnimator;
			this.crawlspaceInteractable.alternateAnimator = animator;
			this.crawlspaceInteractable.CamLogic(-1);
			this.crawlspaceInteractable.VirtualCam = this.crawlspaceInteractable.animator.GetComponentInChildren<CinemachineVirtualCamera>();
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001E946 File Offset: 0x0001CB46
	public void MovePlayerToCamera()
	{
		if (this.teleportedIn)
		{
			Singleton<MorningRoutine>.Instance.setplayerpos(this.cameraGoingUp.gameObject, true);
			return;
		}
		Singleton<MorningRoutine>.Instance.setplayerpos(this.cameraGoingDown.gameObject, true);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0001E980 File Offset: 0x0001CB80
	public void PlayAnimation()
	{
		if (!this.teleportedIn)
		{
			this.animationGoingDown.SetTrigger("standardAnimStart");
			this.cameraGoingDown.Priority = 100;
			return;
		}
		this.animationGoingUp.SetTrigger("standardAnimStart");
		this.cameraGoingUp.Priority = 100;
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0001E9D0 File Offset: 0x0001CBD0
	public void MovePlayerToCameraPositionDown(GameObject cameraDown)
	{
		if (!this.loadedIn && !this.teleportedIn)
		{
			Singleton<MorningRoutine>.Instance.setplayerpos(cameraDown, true);
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001E9EE File Offset: 0x0001CBEE
	public void MovePlayerToCameraPositionUp(GameObject cameraUp)
	{
		if (this.loadedIn || this.teleportedIn)
		{
			Singleton<MorningRoutine>.Instance.setplayerpos(cameraUp, true);
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001EA0C File Offset: 0x0001CC0C
	public void ResolveMusic()
	{
		if (this.teleportedIn)
		{
			base.Invoke("FadeMusicIn", 2f);
			return;
		}
		this.FadeMusicOut();
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001EA2D File Offset: 0x0001CC2D
	public void FadeMusicOut()
	{
		Singleton<DayNightCycle>.Instance.StopMusic();
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001EA39 File Offset: 0x0001CC39
	public void FadeMusicIn()
	{
		DayNightCycle.PlayDayMusic(9f);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001EA45 File Offset: 0x0001CC45
	public int Priority()
	{
		return 1200;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001EA4C File Offset: 0x0001CC4C
	public void LoadState()
	{
		this.teleportedIn = Singleton<Save>.Instance.GetTutorialThresholdState(Teleporter.CRAWLSPACE_STATE);
		this.pendingLoad = true;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0001EA6A File Offset: 0x0001CC6A
	public void SaveState()
	{
		if (this.teleportedIn)
		{
			Singleton<Save>.Instance.SetTutorialThresholdState(Teleporter.CRAWLSPACE_STATE);
			return;
		}
		Singleton<Save>.Instance.RemoveTutorialThresholdState(Teleporter.CRAWLSPACE_STATE);
	}

	// Token: 0x04000509 RID: 1289
	public GameObject LocationDown;

	// Token: 0x0400050A RID: 1290
	public GameObject LocationUp;

	// Token: 0x0400050B RID: 1291
	public float WaitInSeconds;

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	public bool teleportedIn;

	// Token: 0x0400050D RID: 1293
	[Space(10f)]
	[Header("Crawlspace-Specific")]
	[SerializeField]
	private bool isCrawlspace;

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	private GenericInteractable crawlspaceInteractable;

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	private Vector3 teleportInRotation;

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	private Vector3 teleportOutRotation;

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	private CinemachineVirtualCamera cameraGoingDown;

	// Token: 0x04000512 RID: 1298
	[SerializeField]
	private CinemachineVirtualCamera cameraGoingUp;

	// Token: 0x04000513 RID: 1299
	[SerializeField]
	private Animator animationGoingDown;

	// Token: 0x04000514 RID: 1300
	[SerializeField]
	private Animator animationGoingUp;

	// Token: 0x04000515 RID: 1301
	public static string CRAWLSPACE_STATE = "SavedInCrawlspace";

	// Token: 0x04000516 RID: 1302
	private bool loadedIn;

	// Token: 0x04000517 RID: 1303
	private bool inAnimation;

	// Token: 0x04000518 RID: 1304
	private bool pendingLoad;
}
