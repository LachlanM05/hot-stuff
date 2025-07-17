using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Rewired;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000DE RID: 222
public class TutorialController : Singleton<TutorialController>, IReloadHandler
{
	// Token: 0x06000724 RID: 1828 RVA: 0x00027F94 File Offset: 0x00026194
	public void Start()
	{
		this.playerControl = Object.FindObjectOfType<BetterPlayerControl>();
		this.triggerZones = Object.FindObjectsOfType<TutorialTriggerZone>(true);
		this.SignpostAnimator = this.tutorialSignpost.GetComponent<DoCodeAnimation>();
		this.SubtitleText = this.Subtitles.GetComponentInChildren<TextMeshProUGUI>();
		this.car1Animator = this.car1GameObject.GetComponent<Animator>();
		this.car2Animator = this.car2GameObject.GetComponent<Animator>();
		this.car2GameObject.SetActive(false);
		base.Invoke("StartCar2", 6f);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00028018 File Offset: 0x00026218
	private void StartCar2()
	{
		this.car2GameObject.SetActive(true);
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00028026 File Offset: 0x00026226
	public void OnEnable()
	{
		Save.onAwakening += this.SomethingAwoken;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00028039 File Offset: 0x00026239
	public void OnDisable()
	{
		Save.onAwakening -= this.SomethingAwoken;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x0002804C File Offset: 0x0002624C
	public override void AwakeSingleton()
	{
		this.player = ReInput.players.GetPlayer(0);
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0002805F File Offset: 0x0002625F
	public void SomethingAwoken()
	{
		this.pendingTutorialUpdate = true;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00028068 File Offset: 0x00026268
	public void Update()
	{
		if (this.pendingTutorialUpdate)
		{
			this.pendingTutorialUpdate = false;
			this.SetTutorialText(null, true);
		}
		this.UpdateSignpostState();
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00028088 File Offset: 0x00026288
	public void SetTutorialText(string customMessage = null, bool hideSignpost = false)
	{
		Save.SetDeluxeEditionVariables();
		this.tutorialSignpost.SetActive(true);
		if (customMessage != null && customMessage != "")
		{
			this.tutorialSignpostTMP.text = customMessage;
			return;
		}
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS) && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
		{
			TutorialTriggerZone[] array = this.triggerZones;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(false);
			}
			this.TurnOnOfficeSwitch();
			this.tutorialSignpostTMP.text = "Start your new job at your computer";
			this.ShowTutorialSignpost(true);
			MovingDateable.MoveDateable("MovingParticleMonitor", "particleOn", false);
			return;
		}
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK) && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD))
		{
			if (Singleton<PhoneManager>.Instance.HasNewMessageAlert())
			{
				this.tutorialSignpostTMP.text = "Check the message on your phone.";
			}
			else
			{
				this.tutorialSignpostTMP.text = "Leave the office and reflect on your life choices.";
			}
			this.ShowTutorialSignpost(false);
			MovingDateable.MoveDateable("MovingParticleMonitor", "default", false);
			return;
		}
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("skylar_specs") == RelationshipStatus.Unmet)
		{
			this.tutorialSignpostTMP.text = "Check the delivery at the front door.";
			if (!hideSignpost)
			{
				this.ShowTutorialSignpost(false);
				return;
			}
		}
		else if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("dorian_door") == RelationshipStatus.Unmet && !Singleton<Dateviators>.Instance.Equipped)
		{
			string actionText = ControlsUI.GetActionText(ReInput.players.GetPlayer(0), 52);
			this.tutorialSignpostTMP.text = "Press " + actionText + " to toggle the Dateviators.";
			if (!hideSignpost)
			{
				this.ShowTutorialSignpost(false);
				return;
			}
		}
		else
		{
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("dorian_door") == RelationshipStatus.Unmet && Singleton<Dateviators>.Instance.Equipped)
			{
				string actionText2 = ControlsUI.GetActionText(ReInput.players.GetPlayer(0), 13);
				this.tutorialSignpostTMP.text = "Awaken a door by holding " + actionText2 + ".";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
				}
				MovingDateable.MoveDateable("MovingParticleDoor", "particleOn", false);
				return;
			}
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") == RelationshipStatus.Unmet)
			{
				this.tutorialSignpostTMP.text = "Awaken your phone.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
				}
				MovingDateable.MoveDateable("MovingParticleDoor", "default", false);
				MovingDateable.MoveDateable("MovingParticlePhoneOff", "particleOn", false);
				MovingDateable.MoveDateable("MovingParticlePhoneOn", "particleOn", false);
				return;
			}
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("maggie_mglass") == RelationshipStatus.Unmet)
			{
				this.tutorialSignpostTMP.text = "Locate the magnifying glass to Awaken it.";
				this.TurnOnLightSwitches();
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
				}
				MovingDateable.MoveDateable("MovingParticlePhoneOff", "default", false);
				MovingDateable.MoveDateable("MovingParticlePhoneOn", "default", false);
				MovingDateable.MoveDateable("MovingParticleMagnifyingGlass", "particleOn", false);
				return;
			}
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("betty_bed") == RelationshipStatus.Unmet)
			{
				this.TurnOnLightSwitches();
				this.tutorialSignpostTMP.text = "Follow the clue in Roomers to use your day's final charge.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
				}
				MovingDateable.MoveDateable("MovingParticleMagnifyingGlass", "default", false);
				MovingDateable.MoveDateable("MovingParticleBed", "particleOn", false);
				return;
			}
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO))
			{
				this.tutorialSignpostTMP.text = "Charge the Dateviators by going to sleep.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
				}
				MovingDateable.MoveDateable("MovingParticleBed", "default", false);
				return;
			}
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && !(bool)Singleton<InkController>.Instance.story.variablesState["skylar_where"])
			{
				this.tutorialSignpostTMP.text = "Continue to awaken dateable objects.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && Singleton<Save>.Instance.AvailableTotalMetDatables() < 10)
			{
				this.tutorialSignpostTMP.text = "Speak with Maggie for more Roomers.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && Singleton<Save>.Instance.GetRoomersFound().Count > 5)
			{
				this.tutorialSignpostTMP.text = "Continue to Awaken Dateable Objects.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && Singleton<Save>.Instance.AvailableTotalMetDatables() >= 48 && Singleton<Save>.Instance.AvailableTotalRealizedDatables() == 0 && Singleton<InkController>.Instance.story.variablesState["realize_skylar_asap"].ToString() == "on")
			{
				this.tutorialSignpostTMP.text = "Talk to Skylar Specs!";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && Singleton<Save>.Instance.AvailableTotalMetDatables() >= 48 && ((!Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalRealizedDatables() == 99) || (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalRealizedDatables() == 101)))
			{
				this.tutorialSignpostTMP.text = "Realize Skylar Specs herself.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if ((Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && Singleton<Save>.Instance.GetDateStatus("reggie") == RelationshipStatus.Unmet && !Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalLoveEndings() == 99) || (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalLoveEndings() == 101))
			{
				this.tutorialSignpostTMP.text = "Talk to Skylar Specs.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if ((Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && Singleton<Save>.Instance.GetDateStatus("reggie") == RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatusRealized("dorian") != RelationshipStatus.Realized && !Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalFriendEndings() == 99) || (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalFriendEndings() == 101))
			{
				this.tutorialSignpostTMP.text = "Talk to Dorian.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if ((Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && !Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalHateEndings() == 100) || (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalHateEndings() == 102))
			{
				this.tutorialSignpostTMP.text = "Leave your home to return the dateviators.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if ((Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && !Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalRealizedDatables() == 100) || (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalRealizedDatables() == 102))
			{
				this.tutorialSignpostTMP.text = "Leave your home to see your effects on the world...";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && Singleton<Save>.Instance.AvailableTotalMetDatables() >= 48 && Singleton<InkController>.Instance.story.variablesState["realize_skylar_asap"].ToString() == "complete")
			{
				this.tutorialSignpostTMP.text = "Realize Dateable Objects.";
				if (!hideSignpost)
				{
					this.ShowTutorialSignpost(false);
					return;
				}
			}
			else
			{
				this.tutorialSignpost.SetActive(false);
				this.tutorialSignpostTMP.text = "";
				this.HideTutorialSignpost();
			}
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00028848 File Offset: 0x00026A48
	public bool CanGoToBed()
	{
		return Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("skylar_specs") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("dorian_door") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("maggie_mglass") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("betty_bed") > RelationshipStatus.Unmet;
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x000288C0 File Offset: 0x00026AC0
	public bool CanInteract(InteractableObj obj)
	{
		if (obj == null)
		{
			return true;
		}
		if (Singleton<Save>.Instance.GetFullTutorialFinished())
		{
			return true;
		}
		if (Singleton<Save>.Instance.GetDateStatus("skylar_specs") == RelationshipStatus.Unmet && obj.InternalName() == "skylar")
		{
			return true;
		}
		if (Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") == RelationshipStatus.Unmet && obj.InternalName() == "phoenicia")
		{
			return true;
		}
		if (Singleton<Save>.Instance.GetDateStatus("skylar_specs") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("dorian_door") == RelationshipStatus.Unmet && obj.InternalName() != "dorian" && Singleton<Dateviators>.Instance.Equipped)
		{
			Singleton<Popup>.Instance.CreatePopup("", "Sorry, but today is special. There's a method to my madness - I promise!", true);
			return false;
		}
		if (Singleton<Save>.Instance.GetDateStatus("dorian_door") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") == RelationshipStatus.Unmet && obj.InternalName() != "phoenicia" && Singleton<Dateviators>.Instance.Equipped)
		{
			Singleton<Popup>.Instance.CreatePopup("", "Sorry, but today is special. There's a method to my madness - I promise!", true);
			return false;
		}
		if (Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("maggie_mglass") == RelationshipStatus.Unmet && obj.InternalName() != "maggie" && Singleton<Dateviators>.Instance.Equipped)
		{
			Singleton<Popup>.Instance.CreatePopup("", "Sorry, but today is special. There's a method to my madness - I promise!", true);
			return false;
		}
		if (Singleton<Save>.Instance.GetDateStatus("maggie_mglass") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatus("betty_bed") == RelationshipStatus.Unmet && obj.InternalName() != "betty" && Singleton<Dateviators>.Instance.Equipped)
		{
			Singleton<Popup>.Instance.CreatePopup("", "Sorry, but today is special. There's a method to my madness - I promise!", true);
			return false;
		}
		return true;
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00028A90 File Offset: 0x00026C90
	public void TutorialFinishedWork()
	{
		if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
		{
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<Dateviators>.Instance.StartAddPhoneAnimation();
			Singleton<ComputerManager>.Instance.ManageCanopyButton();
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_notification, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			Singleton<AudioManager>.Instance.StopTrack("wrkspce_music", 3f);
			this.SetTutorialText(null, false);
			base.Invoke("NewPhoneMessageAnimation", 2f);
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x00028B31 File Offset: 0x00026D31
	public void NewPhoneMessageAnimation()
	{
		Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00028B40 File Offset: 0x00026D40
	public void ForceCloseComputer()
	{
		UnityEvent unityEvent = new UnityEvent();
		unityEvent.AddListener(new UnityAction(this.ForceCloseComputer2));
		UnityEvent unityEvent2 = new UnityEvent();
		unityEvent2.AddListener(new UnityAction(this.EnableCloseButton));
		Singleton<Popup>.Instance.CreatePopup("VALDIVIAN", "Would you like to leave your computer? You won't be able to return later.", unityEvent, unityEvent2, true);
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x00028B94 File Offset: 0x00026D94
	private void EnableCloseButton()
	{
		Singleton<ChatMaster>.Instance.EnableCloseButton();
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00028BA0 File Offset: 0x00026DA0
	public void ForceCloseComputer2()
	{
		Singleton<ComputerManager>.Instance.ExitOutOfCanopyMenu();
		Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
		base.Invoke("TutorialFinishedWork", 2.5f);
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x00028BC7 File Offset: 0x00026DC7
	public void ForceCloseThiscord()
	{
		Singleton<ComputerManager>.Instance.ExitOutOfCanopyMenu();
		Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
		Singleton<ChatMaster>.Instance.ActiveChatThiscord = null;
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00028BE9 File Offset: 0x00026DE9
	private void ForceOpenPhoneAppThiscord()
	{
		Singleton<PhoneManager>.Instance.OpenPhoneAppHorizontalThiscord();
		base.Invoke("ForceOpenPhoneAppThiscord2", 1f);
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00028C05 File Offset: 0x00026E05
	private void ForceOpenPhoneAppThiscord2()
	{
		if (Singleton<ChatMaster>.Instance.ThiscordChats.Count > 0)
		{
			Singleton<ChatMaster>.Instance.SwitchToChat(Singleton<ChatMaster>.Instance.ThiscordChats[0], ChatType.Thiscord, false);
		}
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00028C35 File Offset: 0x00026E35
	public void TutorialState4AfterSkylarIsAwakened()
	{
		this.SetTutorialText(null, false);
		if (Services.GameSettings.GetInt(Save.SettingKeySkipTutorial, 0) == 0)
		{
			base.Invoke("ShowTutorialPopup1", 2f);
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00028C64 File Offset: 0x00026E64
	private void ShowTutorialPopup1()
	{
		UnityEvent unityEvent = new UnityEvent();
		unityEvent.AddListener(new UnityAction(this.ShowTutorialPopup2));
		string actionText = ControlsUI.GetActionText(this.player, 52);
		Singleton<Popup>.Instance.CreatePopup("You got the dateviators!", "Press " + actionText + " to switch between glasses on and off modes.", unityEvent, false);
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00028CB8 File Offset: 0x00026EB8
	private void ShowTutorialPopup2()
	{
		string actionText = ControlsUI.GetActionText(this.player, 13);
		Singleton<Popup>.Instance.CreatePopup("You got the dateviators!", "Hold down " + actionText + " when wearing the glasses while aimed at an object to speak to it!", false);
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00028CF3 File Offset: 0x00026EF3
	public void ShowTutorialPopup3AfterAwakeningBetty()
	{
		Singleton<Popup>.Instance.CreatePopup("", "Switch back to glasses off mode when you want to interact with the world around you!", false);
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00028D0C File Offset: 0x00026F0C
	public void SkipIntroBlackScreen()
	{
		this.blackScreen.color = new Color(0f, 0f, 0f, 0f);
		this.blackScreen.gameObject.SetActive(true);
		this.blackScreen.DOFade(1f, 1.5f);
		base.Invoke("EndSkipIntroBlackScreen", 1.5f);
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00028D74 File Offset: 0x00026F74
	public void EndSkipIntroBlackScreen()
	{
		Singleton<TutorialController>.Instance.ShowLogo();
		Singleton<TutorialController>.Instance.EndInitialAnimation();
		base.Invoke("EndSkipIntroBlackScreen2", 1f);
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00028D9A File Offset: 0x00026F9A
	public void EndSkipIntroBlackScreen2()
	{
		this.blackScreen.DOFade(0f, 1f);
		this.blackScreen.gameObject.SetActive(true);
		base.Invoke("EndSkipIntroBlackScreen3", 1f);
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00028DD3 File Offset: 0x00026FD3
	public void EndSkipIntroBlackScreen3()
	{
		this.blackScreen.gameObject.SetActive(false);
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00028DE8 File Offset: 0x00026FE8
	public void ShowLogo()
	{
		this.OpeningAnimator.enabled = false;
		this.logo.gameObject.SetActive(true);
		this.logo.DOFade(1f, 3f).OnComplete(delegate
		{
			this.logo.DOFade(0f, 2f).OnComplete(delegate
			{
				this.logo.gameObject.SetActive(false);
			});
		});
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00028E3C File Offset: 0x0002703C
	private void StartInitialAnimation()
	{
		Singleton<ScreenFader>.Instance.InstantBlack();
		CinematicBars.Show(-1f);
		Singleton<DayNightCycle>.Instance.ForceLightingScenario(6);
		Singleton<AudioManager>._instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0f);
		Singleton<AudioManager>.Instance.SfxAmbientGroup.audioMixer.GetFloat("ambient_volume", out this.storedAmbientVolume);
		Singleton<AudioManager>.Instance.SfxAmbientGroup.audioMixer.DOSetFloat("ambient_volume", -80f, 0f);
		Singleton<AudioManager>.Instance.SfxAmbientGroup.audioMixer.GetFloat("ambient_volume", out this.storedFoleyVolume);
		Singleton<AudioManager>.Instance.SfxAmbientGroup.audioMixer.DOSetFloat("foley_volume", -80f, 0f);
		this.inCinematic = true;
		MovingDateable.MoveDateable("MovingOfficeDoor", "locked", false);
		MovingDateable.MoveDateable("MovingOfficeDoorCloset", "locked", false);
		this.currentCamera = Singleton<CameraSpaces>.Instance.GetCameraByRoom("opening_v2").DialogueCamera;
		this.currentCamera.Priority = 200;
		this.car1GameObject.SetActive(false);
		this.car2GameObject.SetActive(false);
		this.ambientSpaces = Object.FindObjectsOfType<AmbientSpace>().ToList<AmbientSpace>();
		this.skipper.enabled = true;
		this.skipper.Hide();
		foreach (AmbientSpace ambientSpace in this.ambientSpaces)
		{
			ambientSpace.enabled = false;
			ambientSpace.ResetImmediate();
		}
		this.OpeningAnimator.enabled = true;
		this.OpeningAnimator.SetTrigger("PlayOpeningAnimation");
		base.Invoke("EndInitialAnimationGivePlayerControl", 29f);
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00029008 File Offset: 0x00027208
	public void EndInitialAnimationGivePlayerControl()
	{
		PlayerPauser.Unpause();
		this.inCinematic = false;
		base.Invoke("StartInitialAnimation_Step5c", 5f);
		base.Invoke("ReleasePlayer_Step5a", 8f);
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00029036 File Offset: 0x00027236
	public void SetLightScenario(int lightSetting)
	{
		Singleton<DayNightCycle>.Instance.ForceLightingScenario(lightSetting);
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00029044 File Offset: 0x00027244
	public void EndInitialAnimation()
	{
		if (this.initialEnded)
		{
			return;
		}
		base.CancelInvoke();
		Singleton<AudioManager>.Instance.SfxAmbientGroup.audioMixer.DOSetFloat("ambient_volume", this.storedAmbientVolume, 5f);
		Singleton<AudioManager>.Instance.SfxAmbientGroup.audioMixer.DOSetFloat("foley_volume", this.storedFoleyVolume, 1f);
		this.officeSpace.ResetFromZero();
		this.SetLightScenario(0);
		this.currentCamera.Priority = -1;
		if (this.skipper.skipped)
		{
			Singleton<AudioManager>.Instance.StopTrack("opening_cinematic_audio", 2f);
		}
		this.EndInitialAnimationGivePlayerControl();
		this.ReleasePlayer_Step5a();
		this.skipper.enabled = false;
		this.skipper.Hide();
		this.initialEnded = true;
		GameObject dateable = MovingDateable.GetDateable("MovingPlayerPosition", "initial");
		dateable.transform.position = new Vector3(dateable.transform.position.x, dateable.transform.position.y - 0.6f, dateable.transform.position.z);
		Singleton<MorningRoutine>.Instance.setplayerpos(dateable, true);
		this.car1GameObject.SetActive(true);
		this.car2GameObject.SetActive(true);
		foreach (AmbientSpace ambientSpace in this.ambientSpaces)
		{
			ambientSpace.enabled = true;
			ambientSpace.ResetImmediate();
		}
		this.OpenAllCurtains();
		Singleton<ScreenFader>.Instance.InstantFadeIn();
		CinematicBars.Hide(14f);
		this.Subtitles.SetActive(false);
		Singleton<ChatMaster>.Instance.CanopyEmptyMessage.SetActive(true);
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00029210 File Offset: 0x00027410
	private void OpenOfficeCurtains()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("OfficeCurtains");
		for (int i = 0; i < array.Length; i++)
		{
			GenericInteractable component = array[i].GetComponent<GenericInteractable>();
			if (!component.interactedWithState)
			{
				component.Interact(false, false);
				component.ToggleInteractedWith(BetterPlayerControl.Instance.transform.position, false);
			}
		}
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00029268 File Offset: 0x00027468
	private void OpenAllCurtains()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("OfficeCurtains");
		for (int i = 0; i < array.Length; i++)
		{
			GenericInteractable component = array[i].GetComponent<GenericInteractable>();
			if (!component.interactedWithState)
			{
				component.Interact(false, false);
				component.ToggleInteractedWith(BetterPlayerControl.Instance.transform.position, false);
			}
		}
		array = GameObject.FindGameObjectsWithTag("HouseCurtains");
		for (int i = 0; i < array.Length; i++)
		{
			GenericInteractable component2 = array[i].GetComponent<GenericInteractable>();
			if (!component2.interactedWithState)
			{
				component2.Interact(false, false);
				component2.ToggleInteractedWith(BetterPlayerControl.Instance.transform.position, false);
			}
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00029305 File Offset: 0x00027505
	private void ReleasePlayer_Step5a()
	{
		Singleton<Dateviators>.Instance.StartHud();
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00029314 File Offset: 0x00027514
	private void StartInitialAnimation_Step5c()
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_wkspace_notification, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.computer.gameObject, false, SFX_SUBGROUP.UI, false);
		Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS);
		this.Subtitles.SetActive(false);
		this.SetTutorialText(null, false);
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x00029374 File Offset: 0x00027574
	public void DeliverGiftBox0()
	{
		if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD))
		{
			foreach (TutorialTriggerZone tutorialTriggerZone in this.triggerZones)
			{
				if (tutorialTriggerZone.eventType == TutorialTriggerZone.EventType.TriggerDrone)
				{
					tutorialTriggerZone.gameObject.SetActive(true);
				}
			}
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreenRotate();
			this.SetTutorialText("", false);
		}
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x000293E0 File Offset: 0x000275E0
	public void DeliverGiftBox()
	{
		this.OpenOfficeCurtains();
		this.SetTutorialText("", false);
		foreach (TutorialTriggerZone tutorialTriggerZone in this.triggerZones)
		{
			if (tutorialTriggerZone.eventType == TutorialTriggerZone.EventType.TriggerDrone)
			{
				tutorialTriggerZone.gameObject.SetActive(false);
			}
		}
		Singleton<PhoneManager>.Instance.ClosePhoneAsync(delegate
		{
			CinematicBars.Show(-1f);
			base.Invoke("DeliverGiftBox2", 0.1f);
		}, true);
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00029443 File Offset: 0x00027643
	private void DeliverGiftBox2()
	{
		Singleton<PhoneManager>.Instance.ClosePhoneAsync(delegate
		{
			CanvasUIManager.SwitchMenu("HUD");
			Roomers.Instance.LoadInitialDataOnGameLoad();
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD);
			this.droneAnimator.gameObject.SetActive(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.mysterious_box_drone_fly_deliver_sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, SFX_SUBGROUP.FOLEY, false);
			this.DeliverGiftBox3();
		}, true);
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0002945C File Offset: 0x0002765C
	private void DeliverGiftBox3()
	{
		this.SetTutorialText(null, false);
		this.currentCamera = this.droneCamera;
		this.currentCamera.Priority = 200;
		this.droneAnimator.ResetTrigger("GoAway");
		this.droneAnimator.SetTrigger("DeliverBox");
		PlayerPauser.Pause();
		base.Invoke("CrashSound", 20.4f);
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x000294C4 File Offset: 0x000276C4
	private void CrashSound()
	{
		this.currentCamera.Priority = -1;
		this.currentCamera = Singleton<CameraSpaces>.Instance.GetCameraByRoom("opening_shake").DialogueCamera;
		this.currentCamera.Priority = 200;
		this.currentCamera.Priority = -1;
		PlayerPauser.Unpause();
		base.Invoke("UnpauseAndGoToGift", 0.5f);
		base.Invoke("LandSound", 1f);
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00029538 File Offset: 0x00027738
	public void DroneHoveringTutorialFix()
	{
		this.droneAnimator.gameObject.SetActive(true);
		this.droneAnimator.enabled = true;
		this.droneAnimator.SetTrigger("Hover");
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x00029568 File Offset: 0x00027768
	public void GotCloseToGift()
	{
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.mysterious_box_drone_loop.name, 1f);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.mysterious_box_drone_leave_sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, SFX_SUBGROUP.FOLEY, false);
		this.droneAnimator.gameObject.SetActive(true);
		this.droneAnimator.enabled = true;
		this.droneAnimator.SetTrigger("GoAway");
		Singleton<CanvasUIManager>.Instance.Hide();
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x000295F0 File Offset: 0x000277F0
	private void HideDrone()
	{
		this.droneAnimator.gameObject.SetActive(false);
		this.droneAnimator.GetComponent<TutorialDrone>().DisappearDrone();
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00029613 File Offset: 0x00027813
	private void LandSound()
	{
		CinematicBars.Hide(-1f);
		Singleton<CanvasUIManager>.Instance.Show();
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0002962C File Offset: 0x0002782C
	private void UnpauseAndGoToGift()
	{
		MovingDateable.MoveDateable("MovingOfficeDoor", "unlocked", false);
		MovingDateable.MoveDateable("MovingOfficeDoorCloset", "unlocked", false);
		MovingDateable.MoveDateable("MovingFrontDoor", "broken", false);
		this.giftBox.SetActive(true);
		PlayerPauser.Unpause();
		this.SetTutorialText(null, false);
		Singleton<AudioManager>.Instance.PlayTrack("mysteriousbox_music", AUDIO_TYPE.MUSIC, true, false, 5f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.mysterious_box_drone_loop, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.droneAnimator.gameObject, true, SFX_SUBGROUP.FOLEY, false);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x000296D8 File Offset: 0x000278D8
	public void HideCars()
	{
		if (this.car1Animator != null)
		{
			this.car1Animator.SetBool("carDisabled", true);
			this.car1Animator.speed = 2f;
			this.car1Animator.gameObject.SetActive(false);
		}
		if (this.car2Animator != null)
		{
			this.car2Animator.SetBool("carDisabled", true);
			this.car2Animator.speed = 2f;
			this.car2Animator.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00029768 File Offset: 0x00027968
	public void CarsTimeCheck()
	{
		if (Singleton<DayNightCycle>.Instance.CurrentDayPhase == DayPhase.NIGHT || Singleton<DayNightCycle>.Instance.CurrentDayPhase == DayPhase.MIDNIGHT)
		{
			if (this.car1Animator != null)
			{
				this.car1Animator.SetBool("nighttime", true);
			}
			if (this.car2Animator != null)
			{
				this.car2Animator.SetBool("nighttime", true);
				return;
			}
		}
		else
		{
			if (this.car1Animator != null)
			{
				this.car1Animator.SetBool("nighttime", false);
			}
			if (this.car2Animator != null)
			{
				this.car2Animator.SetBool("nighttime", false);
			}
		}
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0002980C File Offset: 0x00027A0C
	public void LoadState()
	{
		if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			MovingDateable.MoveDateable("MovingMikey", "shown", false);
		}
		else
		{
			MovingDateable.MoveDateable("MovingMikey", "notShown", false);
		}
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			GameObject dateable = MovingDateable.GetDateable("MovingPlayerPosition", "demo");
			BetterPlayerControl.Instance.transform.position = dateable.transform.position;
			BetterPlayerControl.Instance.transform.rotation = dateable.transform.rotation;
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS);
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK);
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD);
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO);
			this.Subtitles.SetActive(false);
			MovingDateable.MoveDateable("Computer", "after", false);
			MovingDateable.MoveDateable("MovingOfficeDoor", "unlocked", false);
			MovingDateable.MoveDateable("MovingOfficeDoorCloset", "unlocked", false);
			if (this.giftBox != null)
			{
				this.giftBox.SetActive(false);
			}
			Singleton<Save>.Instance.SetTutorialFinished(true);
			Singleton<Save>.Instance.SetDateStatus("skylar_specs", RelationshipStatus.Single);
			Singleton<Save>.Instance.SetDateStatus("dorian_door", RelationshipStatus.Single);
			Singleton<Save>.Instance.SetDateStatus("phoenicia_phone", RelationshipStatus.Single);
			Singleton<Save>.Instance.SetDateStatus("maggie_mglass", RelationshipStatus.Single);
			Singleton<Save>.Instance.SetDateStatus("betty_bed", RelationshipStatus.Single);
			Singleton<Dateviators>.Instance.StartHud();
			MovingDateable.MoveDateableEnableAllKeys("MovingPlants");
			MovingDateable.MoveDateable("MuromachiArt", "piano", false);
			MovingDateable.MoveDateable("TurnerArt", "bottle", false);
			MovingDateable.MoveDateable("BanksyArt", "bathroom", false);
			MovingDateable.MoveDateable("HouseArt", "bedroom", false);
			Singleton<InkController>.Instance.TrySetVariable("tutorial", "done");
		}
		else
		{
			this.finishedTutorial = Singleton<Save>.Instance.GetTutorialFinished();
			if (this.giftBox && this.giftBox.gameObject)
			{
				this.giftBox.gameObject.SetActive(false);
			}
			if (!this.finishedTutorial)
			{
				if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD))
				{
					PlayerPauser.Pause();
					this.StartInitialAnimation();
					return;
				}
				if (Singleton<Save>.Instance.GetDateStatus("skylar_specs") == RelationshipStatus.Unmet)
				{
					this.UnpauseAndGoToGift();
				}
			}
			else
			{
				Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS);
				Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK);
				Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD);
				Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO);
				this.Subtitles.SetActive(false);
				MovingDateable.MoveDateable("MovingOfficeDoor", "unlocked", false);
				MovingDateable.MoveDateable("MovingOfficeDoorCloset", "unlocked", false);
				if (this.giftBox != null)
				{
					this.giftBox.SetActive(false);
				}
			}
			Singleton<ComputerManager>.Instance.ManageCanopyButton();
		}
		this.SetTutorialText(null, false);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x00029B07 File Offset: 0x00027D07
	public void SaveState()
	{
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00029B09 File Offset: 0x00027D09
	public int Priority()
	{
		return 900;
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00029B10 File Offset: 0x00027D10
	private void HideTutorialSignpost()
	{
		if (this.SignpostAnimator.IsAnimating || !this.SignpostAnimator.isActiveAndEnabled)
		{
			this._PendingSignpostState = TutorialController.SingpostStateEnum.OffScreen;
			return;
		}
		if (this._SignpostState == TutorialController.SingpostStateEnum.OnScreen)
		{
			this.SignpostAnimator.TriggerAnimationAtIndex(0);
			this._SignpostState = TutorialController.SingpostStateEnum.OffScreen;
			this._TimeToHideSignpost = float.MaxValue;
		}
		this._PendingSignpostState = TutorialController.SingpostStateEnum.None;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00029B70 File Offset: 0x00027D70
	private void ShowTutorialSignpost(bool forever = false)
	{
		if (this.SignpostAnimator.IsAnimating || !this.SignpostAnimator.isActiveAndEnabled)
		{
			this._PendingSignpostState = (forever ? TutorialController.SingpostStateEnum.OnScreenForver : TutorialController.SingpostStateEnum.OnScreen);
			return;
		}
		if (this._SignpostState == TutorialController.SingpostStateEnum.OffScreen)
		{
			Vector3 position = this.tutorialSignpost.transform.position;
			position.x = 0f;
			this.tutorialSignpost.transform.position = position;
			this.SignpostAnimator.TriggerAnimationAtIndex(1);
			this._SignpostState = TutorialController.SingpostStateEnum.OnScreen;
		}
		this._TimeToHideSignpost = (forever ? float.MaxValue : (Time.realtimeSinceStartup + 5f));
		this._PendingSignpostState = TutorialController.SingpostStateEnum.None;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00029C14 File Offset: 0x00027E14
	private void UpdateSignpostState()
	{
		if (this._PendingSignpostState != TutorialController.SingpostStateEnum.None && this.SignpostAnimator.isActiveAndEnabled && !this.SignpostAnimator.IsAnimating)
		{
			switch (this._PendingSignpostState)
			{
			case TutorialController.SingpostStateEnum.OnScreen:
				this.ShowTutorialSignpost(false);
				break;
			case TutorialController.SingpostStateEnum.OffScreen:
				this.HideTutorialSignpost();
				break;
			case TutorialController.SingpostStateEnum.OnScreenForver:
				this.ShowTutorialSignpost(true);
				break;
			}
		}
		if (this._SignpostState == TutorialController.SingpostStateEnum.OnScreen && Time.realtimeSinceStartup > this._TimeToHideSignpost && this.SignpostAnimator.isActiveAndEnabled)
		{
			this.HideTutorialSignpost();
		}
		if (Singleton<PhoneManager>.Instance.IsPhoneMenuOpened())
		{
			if (this._SignpostState != TutorialController.SingpostStateEnum.OnScreen)
			{
				this.ShowTutorialSignpost(true);
			}
			this._PhoneOpen = true;
			return;
		}
		if (this._PhoneOpen)
		{
			this._PhoneOpen = false;
			this.HideTutorialSignpost();
		}
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00029CDA File Offset: 0x00027EDA
	private void SetSubtitleText(string text)
	{
		this.SubtitleText.SetText(text, true);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00029CE9 File Offset: 0x00027EE9
	public void OnIntroCameraCut(int cameraNumber)
	{
		if (cameraNumber < TutorialController.SubtitleTextLines.Length && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS))
		{
			this.SetSubtitleText(TutorialController.SubtitleTextLines[cameraNumber]);
			return;
		}
		this.Subtitles.SetActive(false);
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00029D20 File Offset: 0x00027F20
	private void TurnOnLightSwitches()
	{
		this.lights.ForEach(delegate(Lights_Inter light)
		{
			light.Interact(true, false);
		});
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00029D4C File Offset: 0x00027F4C
	private void TurnOnOfficeSwitch()
	{
		this.officeLights.ForEach(delegate(Lights_Inter light)
		{
			light.Interact(true, false);
		});
	}

	// Token: 0x04000688 RID: 1672
	public static string TUTORIAL_STATE_0_ANIMATIONS = "TutorialInitialAnimations";

	// Token: 0x04000689 RID: 1673
	public static string TUTORIAL_STATE_1_WENT_TO_WORK = "TutorialWentToWork";

	// Token: 0x0400068A RID: 1674
	public static string TUTORIAL_STATE_2_SAW_THISCORD = "TutorialDay1SawThiscord";

	// Token: 0x0400068B RID: 1675
	public static string TUTORIAL_STATE_3_WOKE_UP_DAY_TWO = "TutorialWokeUpDay2";

	// Token: 0x0400068C RID: 1676
	public static string TUTORIAL_STATE_3_TALKED_TO_SKYLAR_DAY_TWO = "TutorialTalkedToSkylarDay2";

	// Token: 0x0400068D RID: 1677
	public static string TUTORIAL_STATE_1000_DATEVIATORS_GONE = "TutorialDateviatorsGone";

	// Token: 0x0400068E RID: 1678
	public static string STATE_DRANK_COFFEE = "TutorialDrankCoffee";

	// Token: 0x0400068F RID: 1679
	private static string[] SubtitleTextLines = new string[] { "", "Congratulations!", "Your degree in customer service has rewarded you with a remote job at Valdivian.", "...one of the foremost megacorporations of our times.", "Though, your house may be large, it is empty.", "And its halls long for visitors.", "The sticky notes on your bulletin board remind you of your ultimate aims...", "...but for now...", "It's time to get to work." };

	// Token: 0x04000690 RID: 1680
	public BetterPlayerControl playerControl;

	// Token: 0x04000691 RID: 1681
	private Player player;

	// Token: 0x04000692 RID: 1682
	[SerializeField]
	private GameObject giftBox;

	// Token: 0x04000693 RID: 1683
	[SerializeField]
	private GameObject tutorialSignpost;

	// Token: 0x04000694 RID: 1684
	[SerializeField]
	private TextMeshPro_T17 tutorialSignpostTMP;

	// Token: 0x04000695 RID: 1685
	[SerializeField]
	private GameObject hud;

	// Token: 0x04000696 RID: 1686
	[SerializeField]
	private Image blackScreen;

	// Token: 0x04000697 RID: 1687
	[SerializeField]
	private Animator droneAnimator;

	// Token: 0x04000698 RID: 1688
	[SerializeField]
	private CinemachineVirtualCamera droneCamera;

	// Token: 0x04000699 RID: 1689
	[SerializeField]
	private GameObject frontDoor;

	// Token: 0x0400069A RID: 1690
	[SerializeField]
	private GameObject computer;

	// Token: 0x0400069B RID: 1691
	public Animator OpeningAnimator;

	// Token: 0x0400069C RID: 1692
	[SerializeField]
	private AmbientSpace officeSpace;

	// Token: 0x0400069D RID: 1693
	[SerializeField]
	private Image logo;

	// Token: 0x0400069E RID: 1694
	[SerializeField]
	private CutsceneSkipper skipper;

	// Token: 0x0400069F RID: 1695
	private List<AmbientSpace> ambientSpaces = new List<AmbientSpace>();

	// Token: 0x040006A0 RID: 1696
	[SerializeField]
	private List<Lights_Inter> lights = new List<Lights_Inter>();

	// Token: 0x040006A1 RID: 1697
	[SerializeField]
	private List<Lights_Inter> officeLights = new List<Lights_Inter>();

	// Token: 0x040006A2 RID: 1698
	private Color transparentColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040006A3 RID: 1699
	private Color solidColor = Color.black;

	// Token: 0x040006A4 RID: 1700
	[SerializeField]
	public GameObject car1GameObject;

	// Token: 0x040006A5 RID: 1701
	[SerializeField]
	public GameObject car2GameObject;

	// Token: 0x040006A6 RID: 1702
	private Animator car1Animator;

	// Token: 0x040006A7 RID: 1703
	private Animator car2Animator;

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	private GameObject Subtitles;

	// Token: 0x040006A9 RID: 1705
	private TextMeshProUGUI SubtitleText;

	// Token: 0x040006AA RID: 1706
	private bool finishedTutorial;

	// Token: 0x040006AB RID: 1707
	private CinemachineVirtualCamera currentCamera;

	// Token: 0x040006AC RID: 1708
	private TutorialTriggerZone[] triggerZones;

	// Token: 0x040006AD RID: 1709
	private DoCodeAnimation SignpostAnimator;

	// Token: 0x040006AE RID: 1710
	private bool pendingTutorialUpdate;

	// Token: 0x040006AF RID: 1711
	private bool initialEnded;

	// Token: 0x040006B0 RID: 1712
	private const float SignpostVisibleTime = 5f;

	// Token: 0x040006B1 RID: 1713
	private const int SIGNPOST_ANIM_HIDE_INDEX = 0;

	// Token: 0x040006B2 RID: 1714
	private const int SIGNPOST_ANIM_SHOW_INDEX = 1;

	// Token: 0x040006B3 RID: 1715
	public bool inCinematic;

	// Token: 0x040006B4 RID: 1716
	private TutorialController.SingpostStateEnum _SignpostState = TutorialController.SingpostStateEnum.OnScreen;

	// Token: 0x040006B5 RID: 1717
	private TutorialController.SingpostStateEnum _PendingSignpostState = TutorialController.SingpostStateEnum.OnScreen;

	// Token: 0x040006B6 RID: 1718
	private float _TimeToHideSignpost = float.MaxValue;

	// Token: 0x040006B7 RID: 1719
	private bool _PhoneOpen;

	// Token: 0x040006B8 RID: 1720
	private float storedAmbientVolume;

	// Token: 0x040006B9 RID: 1721
	private float storedFoleyVolume;

	// Token: 0x020002FC RID: 764
	private enum SingpostStateEnum
	{
		// Token: 0x040011EC RID: 4588
		None,
		// Token: 0x040011ED RID: 4589
		OnScreen,
		// Token: 0x040011EE RID: 4590
		OffScreen,
		// Token: 0x040011EF RID: 4591
		OnScreenForver
	}
}
