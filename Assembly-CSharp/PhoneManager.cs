using System;
using Date_Everything.Scripts.Ink;
using Team17.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000121 RID: 289
public class PhoneManager : Singleton<PhoneManager>
{
	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060009BA RID: 2490 RVA: 0x00037CCF File Offset: 0x00035ECF
	// (set) Token: 0x060009BB RID: 2491 RVA: 0x00037CD7 File Offset: 0x00035ED7
	public bool phoneOpened { get; private set; }

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x00037CE0 File Offset: 0x00035EE0
	public bool SubMenuOpen
	{
		get
		{
			return this.subMenuOpen;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x00037CE8 File Offset: 0x00035EE8
	public PhoneManager.OrientationState Orientation
	{
		get
		{
			return this._orientation;
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x00037CF0 File Offset: 0x00035EF0
	public override void AwakeSingleton()
	{
		this._uiUtilities = Object.FindObjectOfType<UIUtilities>();
		if (this.mainPhoneScreen != null && this.mainPhoneScreen.GetComponent<GridNavigationUpdater>() == null)
		{
			this.mainPhoneScreen.AddComponent<GridNavigationUpdater>();
		}
		this.pauseScreen = this.phoneMenu.gameObject.GetComponent<PauseScreen>();
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00037D4B File Offset: 0x00035F4B
	private void OnDisable()
	{
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00037D50 File Offset: 0x00035F50
	private void UpdateApps(bool IsGlassesEquipped)
	{
		this.phoneButtonThiscord.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonWrkspace.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonSettings.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonSaveLoad.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonTitleScreen.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonResume.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonCredits.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonRoomers.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonDateADex.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonSpecs.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonSkylar.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonPhoenicia.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonReggie.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonBackground.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonMusic.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.phoneButtonArt.GetComponent<PhoneAppManager>().UpdateApp(IsGlassesEquipped);
		this.ChangeGameObjectEnabledState(this.phoneButtonBackground, Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION);
		this.ChangeGameObjectEnabledState(this.phoneButtonMusic, Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION);
		this.ChangeGameObjectEnabledState(this.phoneButtonArt, Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION);
		if (Singleton<Dateviators>.Instance != null)
		{
			this.ChangeGameObjectEnabledState(this.phoneButtonCredits, IsGlassesEquipped);
			this.ChangeGameObjectEnabledState(this.phoneButtonRoomers, IsGlassesEquipped && Singleton<Save>.Instance.GetDateStatus("maggie_mglass") > RelationshipStatus.Unmet);
			this.ChangeGameObjectEnabledState(this.phoneButtonDateADex, IsGlassesEquipped && Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") > RelationshipStatus.Unmet);
			this.ChangeGameObjectEnabledState(this.phoneButtonSpecs, IsGlassesEquipped && Singleton<Save>.Instance.GetDateStatus("dorian_door") > RelationshipStatus.Unmet);
			this.ChangeGameObjectEnabledState(this.phoneButtonSkylar, IsGlassesEquipped && Singleton<Save>.Instance.GetTutorialFinished());
			this.ChangeGameObjectEnabledState(this.phoneButtonPhoenicia, IsGlassesEquipped && Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") > RelationshipStatus.Unmet);
			this.ChangeGameObjectEnabledState(this.phoneButtonReggie, IsGlassesEquipped && Singleton<Save>.Instance.GetDateStatus("reggie_rejection") != RelationshipStatus.Unmet && !Singleton<Save>.Instance.GetTutorialThresholdState(PhoneManager.REGGIE_APP_UNINSTALLED));
		}
		if (DateADex.Instance.CheckEntriesForNotifications())
		{
			this.ChangeGameObjectEnabledState(this.DateADexAppAlertIcon, true);
		}
		else
		{
			this.ChangeGameObjectEnabledState(this.DateADexAppAlertIcon, false);
		}
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
		{
			this.ChangeGameObjectEnabledState(this.phoneButtonWrkspace, true);
			this.ChangeGameObjectEnabledState(this.phoneButtonSaveLoad, true);
		}
		else
		{
			this.ChangeGameObjectEnabledState(this.phoneButtonWrkspace, false);
			this.ChangeGameObjectEnabledState(this.phoneButtonSaveLoad, false);
		}
		this.phoneButtonSaveLoad.GetComponent<Button>().interactable = Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD);
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0003803C File Offset: 0x0003623C
	public void ToggleSelectedItem()
	{
		this.phoneButtonThiscord.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonWrkspace.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonSettings.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonSaveLoad.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonTitleScreen.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonCredits.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonRoomers.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonDateADex.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonSpecs.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonSkylar.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonPhoenicia.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonReggie.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonBackground.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonMusic.GetComponent<Animator>().SetBool("selected", false);
		this.phoneButtonArt.GetComponent<Animator>().SetBool("selected", false);
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00038194 File Offset: 0x00036394
	private void OnEnable()
	{
		if (Singleton<Dateviators>.Instance != null)
		{
			if (Singleton<Dateviators>.Instance.Equipped)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_menu_dateviators_open, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_open, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00038208 File Offset: 0x00036408
	public void UpdatePhoneUI()
	{
		this.ChangeGameObjectEnabledState(this.mainPhoneScreen, true);
		this.ChangeGameObjectEnabledState(this.phoneBackground, true);
		if (Singleton<DayNightCycle>.Instance != null)
		{
			this.dayNumber.text = Singleton<DayNightCycle>.Instance.GetShortDayOfWeek().ToUpperInvariant() ?? "";
		}
		if (Singleton<Dateviators>.Instance != null)
		{
			int currentCharges = Singleton<Dateviators>.Instance.GetCurrentCharges();
			for (int i = 0; i < this.phoneBatterySprites.Length; i++)
			{
				this.ChangeGameObjectEnabledState(this.phoneBatterySprites[i], currentCharges == i);
			}
			this.UpdateApps(Singleton<Dateviators>.Instance.IsEquippingOrEquipped);
		}
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x000382AC File Offset: 0x000364AC
	public void OpenPhoneAppAsync(GameObject subMenu)
	{
		if (this.IsPhoneMenuOpened())
		{
			this.OpenPhoneAppInternal(subMenu);
			return;
		}
		this.OpenPhoneAsync(delegate
		{
			this.OpenPhoneAppInternal(subMenu);
		});
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x000382F4 File Offset: 0x000364F4
	public void OpenPhoneAppAsyncForced(GameObject subMenu)
	{
		this.OpenPhoneAppInternal(subMenu);
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x000382FD File Offset: 0x000364FD
	private void OpenPhoneAppInternal(GameObject subMenu)
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating() && this.Orientation == PhoneManager.OrientationState.Portrait)
		{
			this.ChangeGameObjectEnabledState(this.mainPhoneScreen, false);
			this.curPhoneMenu = subMenu;
			this.ChangeGameObjectEnabledState(subMenu, true);
			this.subMenuOpen = true;
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0003833A File Offset: 0x0003653A
	public void CloseWrkspce()
	{
		if (Singleton<ChatMaster>.Instance.canopyButton.gameObject.activeInHierarchy)
		{
			Singleton<TutorialController>.Instance.ForceCloseComputer2();
			return;
		}
		this.ReturnToMainPhoneScreenRotate();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00038363 File Offset: 0x00036563
	private bool BlockPhoneAppIfDemo()
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			Singleton<Popup>.Instance.CreatePopup("Date Everything", "The phone apps are locked away for this silly little demo. Go date things!", true);
			return true;
		}
		return false;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00038384 File Offset: 0x00036584
	public void OpenPhoneAppCredits(GameObject subMenu)
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating())
		{
			this.OpenPhoneAppHorizontalAsync(subMenu, delegate
			{
				this.PlayCredits();
			});
		}
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x000383AC File Offset: 0x000365AC
	public void OpenPhoneAppHorizontalThiscord()
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating())
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_landscape_transition, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			this.phoneButtonThiscord.GetComponent<Button>().onClick.Invoke();
		}
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00038404 File Offset: 0x00036604
	public void OpenPhoneAppHorizontalWrkspace()
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating())
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_landscape_transition, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			this.phoneButtonWrkspace.GetComponent<Button>().onClick.Invoke();
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0003845C File Offset: 0x0003665C
	[Obsolete("Use OpenPhoneAppHorizontalAsync instead. This is kept around as prefabs use this in button callbacks.", true)]
	public void OpenPhoneAppHorizontal(GameObject subMenu)
	{
		this.OpenPhoneAppHorizontalAsync(subMenu, null);
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x00038468 File Offset: 0x00036668
	public void OpenPhoneAppHorizontalAsync(GameObject subMenu, Action onOpened = null)
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating())
		{
			if (this.IsPhoneMenuOpened())
			{
				this.OpenPhoneAllHorizontalInternal(subMenu, onOpened);
				return;
			}
			this.OpenPhoneAsync(delegate
			{
				this.OpenPhoneAllHorizontalInternal(subMenu, onOpened);
			});
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x000384D0 File Offset: 0x000366D0
	private void OpenPhoneAllHorizontalInternal(GameObject subMenu, Action onOpened = null)
	{
		this.curPhoneMenu = subMenu;
		this.SetOrientation(PhoneManager.OrientationState.RotatingToLandscape);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_landscape_transition, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		this.phoneAnimating = true;
		this.pauseScreen.SetAnimTrigger("RotatePhone", delegate
		{
			this.phoneAnimating = false;
			this.SetSubmenuActive();
			this.OnRotatePhoneCompleted(PhoneManager.OrientationState.Landscape);
			Action onOpened2 = onOpened;
			if (onOpened2 == null)
			{
				return;
			}
			onOpened2();
		});
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00038548 File Offset: 0x00036748
	public void PlayCredits()
	{
		Singleton<AudioManager>.Instance.PlayTrack("epilogue_short", AUDIO_TYPE.MUSIC, true, true, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		this.pauseScreen.uiUtilities.SetMusicFrequency(1f);
		this.pauseScreen.uiUtilities.SetMusicResonance(1f);
		this.creditsScreen.GetComponent<CreditsScreen>().StartCredits();
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x000385B4 File Offset: 0x000367B4
	public void BackCreditsForSassy()
	{
		this.creditsScreen.GetComponent<CreditsScreen>().StopCredits();
		DayNightCycle.PlayDayMusic(0f);
		this.pauseScreen.uiUtilities.SetMusicFrequency(1f);
		this.pauseScreen.uiUtilities.SetMusicResonance(1f);
		PlayerPauser.Unpause();
		CursorLocker.Lock();
		this.pauseScreen.sidewaysImage.gameObject.SetActive(false);
		this.forceCloseRequested = true;
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0003862C File Offset: 0x0003682C
	public void BackCredits()
	{
		this.creditsScreen.GetComponent<CreditsScreen>().StopCredits();
		this.pauseScreen.uiUtilities.SetMusicFrequency(800f);
		this.pauseScreen.uiUtilities.SetMusicResonance(1f);
		DayNightCycle.PlayDayMusic(1f);
		this.ReturnToMainPhoneScreenRotate();
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00038683 File Offset: 0x00036883
	private void SetSubmenuActive()
	{
		this.ChangeGameObjectEnabledState(this.curPhoneMenu, true);
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00038694 File Offset: 0x00036894
	private void SetRoomers()
	{
		this.ChangeGameObjectEnabledState(this.roomersMainWidget, true);
		this.ChangeGameObjectEnabledState(this.curPhoneMenu, true);
		Roomers.Instance.SetupData();
		Roomers.Instance.GoToActiveEntries();
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_roomer_menu_open, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x000386F8 File Offset: 0x000368F8
	private void SetDateADex()
	{
		this.ChangeGameObjectEnabledState(this.dateADexMainWidget, true);
		this.ChangeGameObjectEnabledState(this.curPhoneMenu, true);
		DateADex.Instance.SetupMenus();
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_date_a_dex_menu_open, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00038750 File Offset: 0x00036950
	public void OpenPhoneAppDateADex(GameObject subMenu)
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating())
		{
			this.ChangeGameObjectEnabledState(this.mainPhoneScreen, false);
			this.curPhoneMenu = subMenu;
			this.SetOrientation(PhoneManager.OrientationState.RotatingToLandscape);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_landscape_transition, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			DateADex.Instance.UpdateDateADexEntries();
			DateADex.Instance.ResetSort();
			DateADex.Instance.UpdateListSummaryData(true);
			this.phoneAnimating = true;
			this.pauseScreen.SetAnimTrigger("RotatePhone", delegate
			{
				this.phoneAnimating = false;
				this.SetDateADex();
				this.OnRotatePhoneCompleted(PhoneManager.OrientationState.Landscape);
			});
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x000387F8 File Offset: 0x000369F8
	public void OpenPhoneAppRoomers(GameObject subMenu)
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating())
		{
			this.ChangeGameObjectEnabledState(this.mainPhoneScreen, false);
			this.curPhoneMenu = subMenu;
			this.SetOrientation(PhoneManager.OrientationState.RotatingToLandscape);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_landscape_transition, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			this.phoneAnimating = true;
			this.pauseScreen.SetAnimTrigger("RotatePhone", delegate
			{
				this.phoneAnimating = false;
				this.SetRoomers();
				this.OnRotatePhoneCompleted(PhoneManager.OrientationState.Landscape);
			});
		}
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0003887C File Offset: 0x00036A7C
	public void ReturnToMainPhoneScreen()
	{
		if (!this.phoneOpened)
		{
			return;
		}
		if (this.IsPhoneAnimating())
		{
			return;
		}
		this.subMenuOpen = false;
		if (this.IsPhoneAnimatingOrientation())
		{
			return;
		}
		if (!this.BlockPhoneAppIfDemo())
		{
			this.phoneOpened = true;
			if (this._orientation == PhoneManager.OrientationState.Landscape)
			{
				this.ReturnToMainPhoneScreenRotate();
				return;
			}
			this.ChangeGameObjectEnabledState(this.curPhoneMenu, false);
			this.curPhoneMenu = this.mainPhoneScreen;
			this.UpdatePhoneUI();
		}
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000388E9 File Offset: 0x00036AE9
	public void ReturnToMainPhoneScreenRotateSpecs()
	{
		if (this.IsPhoneAnimating())
		{
			return;
		}
		this.ChangeGameObjectEnabledState(Singleton<SpecStatMain>.Instance.transform.GetChild(0).gameObject, false);
		this.ReturnToMainPhoneScreenRotate();
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00038918 File Offset: 0x00036B18
	public bool CanCloseCommApp()
	{
		bool flag = true;
		if (this.curPhoneMenu != null && this.curPhoneMenu.tag == "WorkspaceComputer")
		{
			Component child = this.curPhoneMenu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetChild(0);
			Transform child2 = this.curPhoneMenu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetChild(3);
			if (child.gameObject.activeInHierarchy)
			{
				flag = Singleton<ComputerManager>.Instance.ReadyToExit();
				if (!flag)
				{
					Singleton<Popup>.Instance.CreatePopup("VALDIVIAN", "You still haven't read through all your WRKSPCE messages!", true);
				}
				else
				{
					Singleton<ChatMaster>.Instance.ActiveChatWorkspace = null;
				}
			}
			else if (child2.gameObject.activeInHierarchy)
			{
				flag = Singleton<ComputerManager>.Instance.ThiscordReadyToExit();
				if (!flag)
				{
					Singleton<Popup>.Instance.CreatePopup("THISCORD", "You still haven't read all your messages!", true);
				}
				else
				{
					Singleton<ChatMaster>.Instance.ActiveChatThiscord = null;
				}
			}
		}
		return flag;
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x00038A29 File Offset: 0x00036C29
	private void ClosePhoneMainMenuIfNotRequired()
	{
		if (TalkingUI.Instance != null && TalkingUI.Instance.open)
		{
			this.ClosePhoneAsync(null, false);
		}
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x00038A4C File Offset: 0x00036C4C
	public void ReturnToMainPhoneScreenRotate()
	{
		if (!this.phoneOpened)
		{
			return;
		}
		if (this._orientation == PhoneManager.OrientationState.Portrait)
		{
			this.ClosePhoneMainMenuIfNotRequired();
			return;
		}
		if (this.IsPhoneAnimating())
		{
			T17Debug.LogError("[PHONE] ReturnToMainPhoneScreenRotate - currently animating.");
			return;
		}
		if (!this.CanCloseCommApp())
		{
			T17Debug.LogError("[PHONE] ReturnToMainPhoneScreenRotate - not allowed to return to main phone screen yet (potentially in forced communication app)");
			return;
		}
		if (this.curPhoneMenu == this.mainPhoneScreen)
		{
			return;
		}
		this.subMenuOpen = false;
		if (!this.phoneOpened)
		{
			PlayerPauser.Unpause();
			CursorLocker.Lock();
		}
		this.ChangeGameObjectEnabledState(this.curPhoneMenu, false);
		this.curPhoneMenu = this.mainPhoneScreen;
		if (base.gameObject.activeInHierarchy)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_portrait_transition, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		this.SetOrientation(PhoneManager.OrientationState.RotatingToPortrait);
		this.phoneAnimating = true;
		this.pauseScreen.SetAnimTrigger("RotatePhoneBack", delegate
		{
			this.phoneAnimating = false;
			this.UpdatePhoneUI();
			this.OnRotatePhoneCompleted(PhoneManager.OrientationState.Portrait);
			this.ClosePhoneMainMenuIfNotRequired();
		});
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x00038B3C File Offset: 0x00036D3C
	public void OpenPhoneAsync(Action onOpened = null)
	{
		if (this.forceCloseRequested)
		{
			return;
		}
		if (this.openRequested)
		{
			this.openRequestedEvent = (Action)Delegate.Combine(this.openRequestedEvent, onOpened);
			return;
		}
		if (this.IsPhoneMenuOpened())
		{
			if (onOpened != null)
			{
				onOpened();
			}
			return;
		}
		this.openRequested = true;
		CanvasUIManager.SwitchMenu("Phone Menu");
		PlayerPauser.Pause();
		CursorLocker.Unlock();
		this.pauseScreen.uiUtilities.SetMusicFrequency(800f);
		this.pauseScreen.uiUtilities.SetMusicResonance(1f);
		this.pauseScreen.sidewaysImage.gameObject.SetActive(false);
		this.openRequestedEvent = onOpened;
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00038BE6 File Offset: 0x00036DE6
	private void ProcessOpenPhoneRequest()
	{
		if (!this.openRequested)
		{
			return;
		}
		if (this.TryOpenPhone())
		{
			this.OnOpenedPhone();
		}
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00038C00 File Offset: 0x00036E00
	private bool TryOpenPhone()
	{
		if (Singleton<PhoneManager>.Instance.IsPhoneMenuOpened())
		{
			return true;
		}
		if (!this.pauseScreen.gameObject.activeSelf)
		{
			this.pauseScreen.gameObject.SetActive(true);
		}
		this.SetInitialFocusedApp();
		if (Singleton<PhoneManager>.Instance.IsPhoneAnimating())
		{
			return false;
		}
		this.UpdatePhoneUI();
		bool wasEquippingOrEquipped = Singleton<Dateviators>.Instance.IsEquippingOrEquipped;
		this.PhoneBackgroundSwitcher.LoadBackgroundOnPhoneOpen();
		this.phoneAnimating = true;
		this.pauseScreen.SetAnimTrigger("OpenPhone", delegate
		{
			if (!this.toggling)
			{
				bool isEquippingOrEquipped = Singleton<Dateviators>.Instance.IsEquippingOrEquipped;
				if (isEquippingOrEquipped != wasEquippingOrEquipped)
				{
					this.UpdateApps(isEquippingOrEquipped);
				}
			}
			this.phoneOpened = true;
			this.phoneAnimating = false;
			Singleton<PhoneManager>.Instance.ResetPhoneAnimating("OpenPhone");
			this.PhoneBackgroundSwitcher.LoadBackgroundOnPhoneOpen();
		});
		return false;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00038CA4 File Offset: 0x00036EA4
	private void OnOpenedPhone()
	{
		this.SetInitialFocusedApp();
		this.openRequested = false;
		Action action = this.openRequestedEvent;
		if (action != null)
		{
			action();
		}
		this.openRequestedEvent = null;
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00038CCC File Offset: 0x00036ECC
	private void SetInitialFocusedApp()
	{
		GameObject gameObject = ((this.InitialFocusedApp == null) ? this.phoneButtonThiscord : this.InitialFocusedApp);
		if (gameObject == null || !gameObject.activeInHierarchy)
		{
			gameObject = this.mainMenu;
		}
		if (ControllerMenuUI.GetCurrentSelectedControl() != gameObject)
		{
			ControllerMenuUI.SetCurrentlySelected(gameObject, ControllerMenuUI.Direction.Down, false, false);
			if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != this.phoneButtonThiscord && Singleton<ControllerMenuUI>.Instance != null)
			{
				Singleton<ControllerMenuUI>.Instance.HighlightAButton(true);
			}
		}
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00038D58 File Offset: 0x00036F58
	public void ClosePhoneAsync(SimpleAnimController.AnimFinishedEvent closedEvent = null, bool silentClose = false)
	{
		if (this.forceCloseRequested)
		{
			this.forcedCloseEvent = (SimpleAnimController.AnimFinishedEvent)Delegate.Combine(this.forcedCloseEvent, closedEvent);
			return;
		}
		if (!this.IsPhoneMenuOpened())
		{
			if (closedEvent != null)
			{
				closedEvent();
			}
			return;
		}
		if (!Singleton<PhoneManager>.Instance.CanCloseCommApp())
		{
			return;
		}
		this.forceCloseRequested = true;
		if (silentClose)
		{
			this.pauseScreen.doNotPlayCloseSound = true;
		}
		this.pauseScreen.uiUtilities.SetMusicFrequency(1f);
		this.pauseScreen.uiUtilities.SetMusicResonance(1f);
		if (!this.loadingSave)
		{
			PlayerPauser.Unpause();
		}
		CursorLocker.Lock();
		this.pauseScreen.sidewaysImage.gameObject.SetActive(false);
		this.forcedCloseEvent = closedEvent;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x00038E14 File Offset: 0x00037014
	private void OnClosed()
	{
		if (this.forceCloseRequested)
		{
			Singleton<CanvasUIManager>.Instance.PhoneClosed(this.pauseScreen);
			this.pauseScreen.gameObject.SetActive(false);
			this.forceCloseRequested = false;
			SimpleAnimController.AnimFinishedEvent animFinishedEvent = this.forcedCloseEvent;
			if (animFinishedEvent != null)
			{
				animFinishedEvent();
			}
			this.forcedCloseEvent = null;
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x00038E69 File Offset: 0x00037069
	private void ProcessClosePhoneRequest()
	{
		if (!this.forceCloseRequested)
		{
			return;
		}
		if (this.TryClosePhone())
		{
			this.OnClosed();
			return;
		}
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x00038E84 File Offset: 0x00037084
	private bool TryClosePhone()
	{
		if (!this.phoneOpened)
		{
			return true;
		}
		if (!this.TryGoToPhoneMainMenu())
		{
			return false;
		}
		if (this.phoneAnimating)
		{
			return false;
		}
		this.phoneAnimating = true;
		this.pauseScreen.SetAnimTrigger("ClosePhone", delegate
		{
			this.phoneOpened = false;
			this.phoneAnimating = false;
			Singleton<PhoneManager>.Instance.ResetPhoneAnimating("ClosePhone");
		});
		return false;
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00038ED3 File Offset: 0x000370D3
	private bool TryGoToPhoneMainMenu()
	{
		if (this._orientation == PhoneManager.OrientationState.Portrait)
		{
			return true;
		}
		if (this._orientation == PhoneManager.OrientationState.RotatingToLandscape || this._orientation == PhoneManager.OrientationState.RotatingToPortrait)
		{
			return false;
		}
		if (this.phoneAnimating)
		{
			return false;
		}
		this.ReturnToMainPhoneScreenRotate();
		return false;
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00038F04 File Offset: 0x00037104
	public void GoToMainMenu()
	{
		if (!this.BlockPhoneAppIfDemo() && !this.IsPhoneAnimating())
		{
			UnityEvent unityEvent = new UnityEvent();
			unityEvent.AddListener(new UnityAction(this.ConfirmGoToMainMenu));
			UnityEvent unityEvent2 = new UnityEvent();
			Singleton<Popup>.Instance.CreatePopup("Sure?", "Are you sure you want to go back to the main menu?", unityEvent, unityEvent2, true);
		}
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00038F58 File Offset: 0x00037158
	public void GoToDemoMenu()
	{
		Singleton<AudioManager>.Instance.StopAll(0.5f);
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			this.ChangeGameObjectEnabledState(this.mainMenu, true);
			this.ChangeGameObjectEnabledState(this.demoCamera, true);
		}
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			Object.Destroy(rootGameObjects[i]);
		}
		Singleton<InkStoryProvider>.Instance.ResetStory();
		this._uiUtilities.UnloadSceneAsync(SceneConsts.kGameScene);
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			this._uiUtilities.LoadSceneAsyncSingle(SceneConsts.kDemoMenu, false);
		}
		else
		{
			this._uiUtilities.LoadSceneAsyncSingle(SceneConsts.kMainMenu, false);
		}
		PlayerPauser.Unpause();
		CursorLocker.Unlock();
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x00039008 File Offset: 0x00037208
	public void ConfirmGoToMainMenu()
	{
		if (BetterPlayerControl.Instance)
		{
			BetterPlayerControl.Instance.ChangePlayerState(BetterPlayerControl.PlayerState.CantControl);
			BetterPlayerControl.Instance.StopBeamSounds();
		}
		UnityEvent returningToMainMenu = this.ReturningToMainMenu;
		if (returningToMainMenu != null)
		{
			returningToMainMenu.Invoke();
		}
		this.loadingSave = true;
		HudCanvasManager.Instance.ForceSlideOut();
		this.GoToMainMenuStopAllSound();
		this.ClosePhoneAsync(new SimpleAnimController.AnimFinishedEvent(this.GoToMainMenuAnimFinished), false);
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00039071 File Offset: 0x00037271
	private void GoToMainMenuAnimFinished()
	{
		this.ConfirmGoToMainMenuImpl();
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00039079 File Offset: 0x00037279
	private void GoToMainMenuStopAllSound()
	{
		Singleton<AudioManager>.Instance.StopAll(0.75f);
		this.pauseScreen.uiUtilities.SetMusicFrequency(1f);
		this.pauseScreen.uiUtilities.SetMusicResonance(1f);
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x000390B4 File Offset: 0x000372B4
	private void ConfirmGoToMainMenuImpl()
	{
		this.ChangeGameObjectEnabledState(this.mainPhoneScreen, false);
		this.ChangeGameObjectEnabledState(this.phoneMenu, false);
		PlayerPauser.Unpause();
		CursorLocker.Unlock();
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			this._uiUtilities.LoadSceneAsyncSingle(SceneConsts.kDemoMenu, false);
			return;
		}
		this._uiUtilities.LoadSceneAsyncSingle(SceneConsts.kMainMenu, false);
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0003910F File Offset: 0x0003730F
	public bool HasNewMessageAlert()
	{
		return this.thiscordPhoneAlert.gameObject.activeSelf;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00039124 File Offset: 0x00037324
	public void SetNewMessageAlert(bool isWrkspace = false)
	{
		int num = Singleton<ComputerManager>.Instance.UnreadWrkspceMessages();
		int num2 = Singleton<ComputerManager>.Instance.UnreadThiscordMessages();
		if (Singleton<Dateviators>.Instance != null)
		{
			Singleton<Dateviators>.Instance.UpdatePhoneAlertDisplay(num + num2);
		}
		this.ChangeGameObjectEnabledState(this.wrkspacePhoneAlert, num > 0);
		TextMeshProUGUI componentInChildren = this.wrkspacePhoneAlert.GetComponentInChildren<TextMeshProUGUI>();
		if (componentInChildren != null)
		{
			componentInChildren.SetText(num.ToString(), true);
		}
		this.ChangeGameObjectEnabledState(this.thiscordPhoneAlert, num2 > 0);
		TextMeshProUGUI componentInChildren2 = this.thiscordPhoneAlert.GetComponentInChildren<TextMeshProUGUI>();
		if (componentInChildren2 == null)
		{
			return;
		}
		componentInChildren2.SetText(num2.ToString(), true);
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x000391BC File Offset: 0x000373BC
	public void SetNewMessageAlertSkylar()
	{
		this.ChangeGameObjectEnabledState(this.skylarPhoneAlert, true);
		TextMeshProUGUI componentInChildren = this.skylarPhoneAlert.GetComponentInChildren<TextMeshProUGUI>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.SetText("1", true);
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x000391E6 File Offset: 0x000373E6
	public void StartChatWithDateable(string dateable_name)
	{
		if (!this.BlockPhoneAppIfDemo())
		{
			this.dateableName = dateable_name;
			Singleton<PhoneManager>.Instance.ClosePhoneAsync(new SimpleAnimController.AnimFinishedEvent(this.StartChatWithSelectedDateable), true);
		}
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0003920E File Offset: 0x0003740E
	public void StartChatWithSelectedDateable()
	{
		this.StartChatWithSelectedDateable(null);
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00039218 File Offset: 0x00037418
	public void StartChatWithSelectedDateable(string dateable)
	{
		if (dateable != null)
		{
			this.dateableName = dateable;
		}
		GameController.SelectObjResult selectObjResult;
		if (this.dateableName == "phoenicia_phone")
		{
			selectObjResult = Singleton<GameController>.Instance.SelectObj(this.phoneButtonPhoenicia.GetComponent<InteractableObj>(), true, null, false, false, false);
		}
		else if (this.dateableName == "skylar_specs")
		{
			selectObjResult = Singleton<GameController>.Instance.SelectObj(this.phoneButtonSkylar.GetComponent<InteractableObj>(), true, null, false, false, false);
		}
		else if (this.dateableName == "reggie_rejection")
		{
			selectObjResult = Singleton<GameController>.Instance.SelectObj(this.phoneButtonReggie.GetComponent<InteractableObj>(), true, null, false, false, false);
		}
		else if (this.dateableName == "willi_work")
		{
			selectObjResult = Singleton<GameController>.Instance.SelectObj(this.phoneButtonWrkspace.GetComponent<InteractableObj>(), true, null, false, false, false);
		}
		else if (this.dateableName == "sassy_chap")
		{
			selectObjResult = Singleton<GameController>.Instance.SelectObj(this.phoneButtonCredits.GetComponent<InteractableObj>(), true, null, false, false, false);
		}
		else
		{
			if (!(this.dateableName == "narrator_date"))
			{
				return;
			}
			selectObjResult = Singleton<GameController>.Instance.SelectObj(this.phoneButtonTitleScreen.GetComponent<InteractableObj>(), true, null, false, false, false);
		}
		if (selectObjResult == GameController.SelectObjResult.FAILED)
		{
			Singleton<CanvasUIManager>.Instance.BackWithReturn();
			PlayerPauser.Unpause();
		}
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00039368 File Offset: 0x00037568
	public void EndChatWithDateable()
	{
		this.ChangeGameObjectEnabledState(this.phoneBackground, true);
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00039377 File Offset: 0x00037577
	public bool IsPhoneMenuOpened()
	{
		return this.phoneOpened && !this.forceCloseRequested;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0003938C File Offset: 0x0003758C
	public bool IsPhoneAnimatingOrientation()
	{
		return this._orientation == PhoneManager.OrientationState.RotatingToLandscape || this._orientation == PhoneManager.OrientationState.RotatingToPortrait;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x000393A4 File Offset: 0x000375A4
	public void ToggleGlasses()
	{
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			this.toggling = true;
			Singleton<Dateviators>.Instance.Equip();
			this.GlassesToggleIsEquipped = true;
			this.PhoneBackgroundSwitcher.SetBackgroundImageColor(this.bgGlassesColor, 1f, delegate
			{
				this.toggling = false;
			});
			this.ChangeGameObjectEnabledState(this.phoneScreenMask, true);
			this.phoneScreenMaskAnimation.TriggerAnimationAtIndex(0);
		}
		else if (Singleton<Dateviators>.Instance.Equipped)
		{
			BetterPlayerControl.Instance.StopBeamSounds();
			Singleton<Dateviators>.Instance.Dequip();
			this.toggling = true;
			this.GlassesToggleIsEquipped = false;
			this.PhoneBackgroundSwitcher.SetBackgroundImageColor(Color.white, 1f, delegate
			{
				this.toggling = false;
			});
		}
		this.phoneMainScreenAnimator.SetTrigger("ToggleGlasses1");
		base.Invoke("ToggleGlasses2", 1.15f);
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00039481 File Offset: 0x00037681
	public void ToggleGlasses2()
	{
		this.phoneScreenMaskAnimation.TriggerAnimationAtIndex(1);
		this.UpdateApps(this.GlassesToggleIsEquipped);
		this.phoneMainScreenAnimator.SetTrigger("ToggleGlasses2");
		base.Invoke("disableMask", 0.96f);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x000394BB File Offset: 0x000376BB
	private void disableMask()
	{
		this.ChangeGameObjectEnabledState(this.phoneScreenMask, false);
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x000394CA File Offset: 0x000376CA
	public bool IsPhoneAppOpened()
	{
		return this.curPhoneMenu != null && this.curPhoneMenu != this.mainPhoneScreen && this.curPhoneMenu.gameObject.activeInHierarchy;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00039502 File Offset: 0x00037702
	public GameObject GetCurrentApp()
	{
		return this.curPhoneMenu;
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0003950A File Offset: 0x0003770A
	private void OnRotatePhoneCompleted(PhoneManager.OrientationState newOrientation)
	{
		if (newOrientation == PhoneManager.OrientationState.Portrait)
		{
			this.SetOrientation(PhoneManager.OrientationState.Portrait);
			return;
		}
		if (newOrientation == PhoneManager.OrientationState.Landscape)
		{
			this.SetOrientation(PhoneManager.OrientationState.Landscape);
		}
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00039522 File Offset: 0x00037722
	public void ResetOutline()
	{
		this.phoneBorderImage.material = null;
		this.StreamPFX.Stop();
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0003953C File Offset: 0x0003773C
	public void UpdateOutline(float normalizedAmount)
	{
		if (normalizedAmount == 0f)
		{
			this.phoneBorderImage.material = null;
			this.StreamPFX.Stop();
			return;
		}
		if (this.StreamPFX.isStopped)
		{
			this.StreamPFX.Play();
		}
		if (this.phoneBorderImage.material != this.phoneBorderSheenMaterial)
		{
			this.phoneBorderImage.material = this.phoneBorderSheenMaterial;
		}
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x000395AA File Offset: 0x000377AA
	public bool IsCreditsScreenOn()
	{
		return this.creditsScreen != null && this.creditsScreen.activeInHierarchy;
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x000395CA File Offset: 0x000377CA
	public void ForceInactivatePhone()
	{
		this.pauseScreen.gameObject.SetActive(false);
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x000395DD File Offset: 0x000377DD
	public bool IsPhoneAnimating()
	{
		return this.phoneAnimating;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x000395E5 File Offset: 0x000377E5
	public void ResetPhoneAnimating(string _trigger)
	{
		this.pauseScreen.ResetAnimTrigger(_trigger);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x000395F3 File Offset: 0x000377F3
	public void ChangeDateADexPing()
	{
		if (DateADex.Instance.CheckEntriesForNotifications())
		{
			this.ChangeGameObjectEnabledState(this.DateADexAppAlertIcon, true);
			return;
		}
		this.ChangeGameObjectEnabledState(this.DateADexAppAlertIcon, false);
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0003961C File Offset: 0x0003781C
	private void ChangeGameObjectEnabledState(GameObject obj, bool enableState = true)
	{
		if (obj != null && obj.activeSelf != enableState)
		{
			obj.SetActive(enableState);
		}
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00039637 File Offset: 0x00037837
	public void Update()
	{
		this.ProcessClosePhoneRequest();
		this.ProcessOpenPhoneRequest();
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00039645 File Offset: 0x00037845
	public void SetOrientation(PhoneManager.OrientationState rotatingToPortrait)
	{
		this._orientation = rotatingToPortrait;
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x0003964E File Offset: 0x0003784E
	public float GetPhoneAnimationDelay()
	{
		return this.phoneAnimationDelay;
	}

	// Token: 0x040008F8 RID: 2296
	public static string REGGIE_APP_UNINSTALLED = "ReggieAppUninstalled";

	// Token: 0x040008F9 RID: 2297
	public static string CROUCH_CONTROL_ENABLED = "CrouchControl";

	// Token: 0x040008FA RID: 2298
	[SerializeField]
	private GameObject phoneButtonThiscord;

	// Token: 0x040008FB RID: 2299
	[SerializeField]
	private GameObject phoneButtonWrkspace;

	// Token: 0x040008FC RID: 2300
	[SerializeField]
	private GameObject phoneButtonSettings;

	// Token: 0x040008FD RID: 2301
	[SerializeField]
	private GameObject phoneButtonSaveLoad;

	// Token: 0x040008FE RID: 2302
	[SerializeField]
	private GameObject phoneButtonTitleScreen;

	// Token: 0x040008FF RID: 2303
	[SerializeField]
	private GameObject phoneButtonResume;

	// Token: 0x04000900 RID: 2304
	[SerializeField]
	private GameObject phoneButtonCredits;

	// Token: 0x04000901 RID: 2305
	[SerializeField]
	private GameObject creditsScreen;

	// Token: 0x04000902 RID: 2306
	[SerializeField]
	private GameObject phoneButtonRoomers;

	// Token: 0x04000903 RID: 2307
	[SerializeField]
	private GameObject phoneButtonDateADex;

	// Token: 0x04000904 RID: 2308
	[SerializeField]
	private GameObject phoneButtonSpecs;

	// Token: 0x04000905 RID: 2309
	[SerializeField]
	private GameObject phoneButtonSkylar;

	// Token: 0x04000906 RID: 2310
	[SerializeField]
	private GameObject phoneButtonPhoenicia;

	// Token: 0x04000907 RID: 2311
	[SerializeField]
	private GameObject phoneButtonReggie;

	// Token: 0x04000908 RID: 2312
	[SerializeField]
	private GameObject phoneButtonBackground;

	// Token: 0x04000909 RID: 2313
	[SerializeField]
	private GameObject phoneButtonMusic;

	// Token: 0x0400090A RID: 2314
	[SerializeField]
	private GameObject phoneButtonArt;

	// Token: 0x0400090B RID: 2315
	[SerializeField]
	private Color bgGlassesColor;

	// Token: 0x0400090C RID: 2316
	[SerializeField]
	private GameObject dateADexMainWidget;

	// Token: 0x0400090D RID: 2317
	[SerializeField]
	private GameObject roomersMainWidget;

	// Token: 0x0400090E RID: 2318
	[SerializeField]
	private GameObject mainPhoneScreen;

	// Token: 0x0400090F RID: 2319
	[SerializeField]
	private GameObject curPhoneMenu;

	// Token: 0x04000910 RID: 2320
	[SerializeField]
	private GameObject thiscordPhoneAlert;

	// Token: 0x04000911 RID: 2321
	[SerializeField]
	private GameObject wrkspacePhoneAlert;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private GameObject skylarPhoneAlert;

	// Token: 0x04000913 RID: 2323
	[SerializeField]
	private TextMeshProUGUI dayNumber;

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private GameObject[] phoneBatterySprites;

	// Token: 0x04000915 RID: 2325
	[SerializeField]
	private GameObject phoneMenu;

	// Token: 0x04000916 RID: 2326
	[SerializeField]
	private GameObject phoneBackground;

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	public GameObject mainMenu;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	public GameObject demoCamera;

	// Token: 0x04000919 RID: 2329
	[SerializeField]
	private float phoneAnimationDelay = 0.6f;

	// Token: 0x0400091A RID: 2330
	[SerializeField]
	private GameObject phoneScreenMask;

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private DoCodeAnimation phoneScreenMaskAnimation;

	// Token: 0x0400091C RID: 2332
	[SerializeField]
	private Image phoneBorderImage;

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	private Material phoneBorderSheenMaterial;

	// Token: 0x0400091E RID: 2334
	[SerializeField]
	private GameObject DateADexAppAlertIcon;

	// Token: 0x0400091F RID: 2335
	[SerializeField]
	private ParticleSystem StreamPFX;

	// Token: 0x04000920 RID: 2336
	[SerializeField]
	private GameObject InitialFocusedApp;

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	private PhoneBackgroundSwitcher PhoneBackgroundSwitcher;

	// Token: 0x04000922 RID: 2338
	private UIUtilities _uiUtilities;

	// Token: 0x04000923 RID: 2339
	public GameObject openedSubMenu;

	// Token: 0x04000924 RID: 2340
	public Animator phoneAnimator;

	// Token: 0x04000925 RID: 2341
	public Animator phoneMainScreenAnimator;

	// Token: 0x04000926 RID: 2342
	public bool toggling;

	// Token: 0x04000928 RID: 2344
	private bool phoneAnimating;

	// Token: 0x04000929 RID: 2345
	private bool subMenuOpen;

	// Token: 0x0400092A RID: 2346
	private SimpleAnimController.AnimFinishedEvent forcedCloseEvent;

	// Token: 0x0400092B RID: 2347
	public bool forceCloseRequested;

	// Token: 0x0400092C RID: 2348
	private Action openRequestedEvent;

	// Token: 0x0400092D RID: 2349
	public bool openRequested;

	// Token: 0x0400092E RID: 2350
	public UnityEvent ReturningToMainMenu = new UnityEvent();

	// Token: 0x0400092F RID: 2351
	public bool BlockPhoneOpening;

	// Token: 0x04000930 RID: 2352
	private PhoneManager.OrientationState _orientation;

	// Token: 0x04000931 RID: 2353
	private PauseScreen pauseScreen;

	// Token: 0x04000932 RID: 2354
	private bool loadingSave;

	// Token: 0x04000933 RID: 2355
	private string dateableName;

	// Token: 0x04000934 RID: 2356
	private bool GlassesToggleIsEquipped;

	// Token: 0x02000327 RID: 807
	public enum OrientationState
	{
		// Token: 0x04001282 RID: 4738
		Portrait,
		// Token: 0x04001283 RID: 4739
		RotatingToLandscape,
		// Token: 0x04001284 RID: 4740
		Landscape,
		// Token: 0x04001285 RID: 4741
		RotatingToPortrait
	}
}
