using System;
using System.Collections;
using Rewired;
using T17.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011D RID: 285
public class PauseScreen : MenuComponent
{
	// Token: 0x0600099F RID: 2463 RVA: 0x000376C9 File Offset: 0x000358C9
	protected override void Awake()
	{
		base.Awake();
		if (this.anim == null)
		{
			this.anim = base.GetComponent<Animator>();
		}
		this.GetAnimController();
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x000376F2 File Offset: 0x000358F2
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.anim.keepAnimatorStateOnDisable = true;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00037711 File Offset: 0x00035911
	private bool IsClosePhoneInputButtonDown()
	{
		return this.player.GetButtonDown(5) || (Singleton<PhoneManager>.Instance.IsPhoneMenuOpened() && !Singleton<PhoneManager>.Instance.IsPhoneAppOpened() && this.player.GetButtonDown(29));
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00037750 File Offset: 0x00035950
	private void Update()
	{
		if (!this.Dex.activeInHierarchy)
		{
			this.ChangeGameObjectEnabledState(this.Dex, true);
		}
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
			return;
		}
		if (this.IsClosePhoneInputButtonDown() && !Singleton<Popup>.Instance.IsPopupOpen() && !UIDialogManager.Instance.AreAnyDialogsActive() && Singleton<CanvasUIManager>.Instance.BackWithReturn() && !Singleton<PhoneManager>.Instance.IsPhoneAnimatingOrientation())
		{
			PlayerPauser.Unpause();
		}
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x000377D0 File Offset: 0x000359D0
	public SimpleAnimController GetAnimController()
	{
		if (this.animController == null && (this.animController = base.GetComponent<SimpleAnimController>()) == null)
		{
			this.animController = base.gameObject.AddComponent<SimpleAnimController>();
			this.animController.notifyJustBeforeEnd = true;
		}
		return this.animController;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00037825 File Offset: 0x00035A25
	public void SetClosedPositionForPhone()
	{
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00037827 File Offset: 0x00035A27
	public void CheckAwakenInPhone(InputActionEventData data)
	{
		this.CheckAwakenInPhone(false);
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00037834 File Offset: 0x00035A34
	public bool CheckAwakenInPhone(bool testOnly = false)
	{
		if (Singleton<GameController>.Instance == null || !Singleton<CanvasUIManager>.Instance.isInGame)
		{
			return false;
		}
		bool flag = false;
		if (!BetterPlayerControl.Instance.isGameEndingOn)
		{
			if (Singleton<Save>.Instance.GetDateableTalkedTo("sassy_chap") != Singleton<DayNightCycle>.Instance.GetDaysSinceStart() && this.CreditsScreen.activeInHierarchy && Singleton<Dateviators>.Instance.CanConsumeCharge() && Singleton<Dateviators>.Instance.Equipped && Singleton<Save>.Instance.GetFullTutorialFinished() && Singleton<Save>.Instance.GetDateStatusRealized("sassy_chap") != RelationshipStatus.Realized)
			{
				flag = true;
				if (!testOnly)
				{
					base.StartCoroutine(this.StartChatWithChap());
				}
			}
			else if (Singleton<Save>.Instance.GetDateableTalkedTo("willi") != Singleton<DayNightCycle>.Instance.GetDaysSinceStart() && Singleton<ChatMaster>.Instance.IsWrkspceOpened() && Singleton<Dateviators>.Instance.CanConsumeCharge() && Singleton<Dateviators>.Instance.Equipped && Singleton<Save>.Instance.GetFullTutorialFinished() && Singleton<Save>.Instance.GetDateStatusRealized("willi_work") != RelationshipStatus.Realized)
			{
				flag = true;
				if (!testOnly)
				{
					base.StartCoroutine(this.StartChatWithWilli());
				}
			}
			else if (Singleton<Popup>.Instance.IsPopupOpen() && Singleton<Dateviators>.Instance.CanConsumeCharge() && Singleton<Dateviators>.Instance.Equipped && Singleton<Save>.Instance.GetFullTutorialFinished())
			{
				flag = true;
				if (!testOnly)
				{
					Singleton<Popup>.Instance.StartChatWithDateable();
				}
			}
			else if (Singleton<PhoneManager>.Instance.IsPhoneMenuOpened() && !Singleton<PhoneManager>.Instance.IsPhoneAppOpened() && Singleton<Dateviators>.Instance.CanConsumeCharge() && Singleton<Dateviators>.Instance.Equipped && Singleton<Save>.Instance.GetDateStatus("dorian_door") != RelationshipStatus.Unmet)
			{
				if (Singleton<Save>.Instance.GetFullTutorialFinished())
				{
					if (Singleton<Save>.Instance.GetDateStatusRealized("phoenicia_phone") != RelationshipStatus.Realized)
					{
						flag = true;
					}
				}
				else if (Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") == RelationshipStatus.Unmet)
				{
					flag = true;
				}
				if (flag && !testOnly)
				{
					Singleton<PhoneManager>.Instance.StartChatWithDateable("phoenicia_phone");
				}
			}
		}
		return flag;
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x00037A2F File Offset: 0x00035C2F
	private IEnumerator StartChatWithChap()
	{
		Singleton<PhoneManager>.Instance.BackCredits();
		yield return new WaitForSeconds(1.15f);
		Singleton<PhoneManager>.Instance.StartChatWithDateable("sassy_chap");
		yield break;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00037A37 File Offset: 0x00035C37
	private IEnumerator StartChatWithWilli()
	{
		Singleton<ComputerManager>.Instance.ExitOutOfCanopyMenu();
		yield return new WaitForSeconds(1.15f);
		Singleton<PhoneManager>.Instance.StartChatWithDateable("willi_work");
		yield break;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00037A40 File Offset: 0x00035C40
	public void OnCloseStart()
	{
		if (Singleton<Dateviators>.Instance != null)
		{
			if (Singleton<Dateviators>.Instance.Equipped)
			{
				if (!this.doNotPlayCloseSound)
				{
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_menu_dateviators_close, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				}
			}
			else if (!this.doNotPlayCloseSound)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_close, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
		}
		this.doNotPlayCloseSound = false;
		CursorLocker.Lock();
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00037AD0 File Offset: 0x00035CD0
	public void SetAnimTrigger(string _trigger, SimpleAnimController.AnimFinishedEvent onCompletedTriggerEvent = null)
	{
		this.IsPhoneAnimating();
		this.animController.RegisterForAnimFinished(onCompletedTriggerEvent);
		this.animController.SetAnimTrigger(_trigger);
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00037AF1 File Offset: 0x00035CF1
	public void ResetAnimTrigger(string _trigger)
	{
		this.animController.ResetAnimTrigger(_trigger);
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00037AFF File Offset: 0x00035CFF
	public bool IsPhoneAnimating()
	{
		return this.animController.IsAnimating();
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00037B0C File Offset: 0x00035D0C
	private void ChangeGameObjectEnabledState(GameObject obj, bool enableState = true)
	{
		if (obj != null && obj.activeSelf != enableState)
		{
			obj.SetActive(enableState);
		}
	}

	// Token: 0x040008E1 RID: 2273
	public UIUtilities uiUtilities;

	// Token: 0x040008E2 RID: 2274
	public DexTitleFollow title;

	// Token: 0x040008E3 RID: 2275
	public Image portrait;

	// Token: 0x040008E4 RID: 2276
	public Image portraitShadow;

	// Token: 0x040008E5 RID: 2277
	public Image sidewaysImage;

	// Token: 0x040008E6 RID: 2278
	public Image phoneBackgroundSideways;

	// Token: 0x040008E7 RID: 2279
	public GameObject Dex;

	// Token: 0x040008E8 RID: 2280
	public GameObject PhoneBackground;

	// Token: 0x040008E9 RID: 2281
	public GameObject CreditsScreen;

	// Token: 0x040008EA RID: 2282
	public GameObject WorkspaceScreen;

	// Token: 0x040008EB RID: 2283
	public Animator anim;

	// Token: 0x040008EC RID: 2284
	private Player player;

	// Token: 0x040008ED RID: 2285
	private const float phoneAnimationDelay = 1.15f;

	// Token: 0x040008EE RID: 2286
	public bool doNotPlayCloseSound;

	// Token: 0x040008EF RID: 2287
	public SimpleAnimController animController;
}
