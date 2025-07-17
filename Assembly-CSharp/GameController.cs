using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using T17.Services;
using Team17.Common;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

// Token: 0x02000055 RID: 85
public class GameController : Singleton<GameController>
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000226 RID: 550 RVA: 0x0000C8B2 File Offset: 0x0000AAB2
	// (set) Token: 0x06000227 RID: 551 RVA: 0x0000C8BA File Offset: 0x0000AABA
	public VIEW_STATE viewState
	{
		get
		{
			return this._viewState;
		}
		set
		{
			VIEW_STATE viewState = this._viewState;
			this._viewState = value;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000228 RID: 552 RVA: 0x0000C8CA File Offset: 0x0000AACA
	public Dictionary<string, AudioClip> musicFiles
	{
		get
		{
			if (this._musicFiles == null)
			{
				this._musicFiles = new Dictionary<string, AudioClip>();
			}
			return this._musicFiles;
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
	public void Start()
	{
		if (this.DialogueExit == null)
		{
			this.DialogueExit = new UnityEvent();
		}
		PlayerPauser.ClearAll();
		CursorLocker.Reset();
		this.tempDatable = new GameObject
		{
			transform = 
			{
				parent = base.transform
			}
		}.AddComponent<InteractableObj>();
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000C935 File Offset: 0x0000AB35
	public override void AwakeSingleton()
	{
		this.viewState = VIEW_STATE.HOUSE;
		this.player = ReInput.players.GetPlayer(0);
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000C94F File Offset: 0x0000AB4F
	public List<InteractableObj> GetAllCharacters()
	{
		if (this.interactables == null || this.interactables.Count == 0)
		{
			this.interactables = Object.FindObjectsOfType<InteractableObj>().ToList<InteractableObj>();
		}
		return this.interactables;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000C97C File Offset: 0x0000AB7C
	public void ParseMainTags(List<string> tags)
	{
		foreach (string text in tags)
		{
			string[] array = text.Split(' ', StringSplitOptions.None);
			if (array.Length == 3 && array[0] == "define_music")
			{
				if (this.musicFiles.ContainsKey(array[1]))
				{
					break;
				}
				AudioClip audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>("Music/" + array[2].Trim(), false);
				if (audioClip != null)
				{
					this.musicFiles.Add(array[1], audioClip);
				}
			}
		}
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000CA28 File Offset: 0x0000AC28
	public void SetCurrentDatableByName(string name)
	{
		InteractableObj interactableObj = this.GetAllCharacters().FirstOrDefault((InteractableObj x) => x.KnotName() == name);
		if (interactableObj != null)
		{
			this.currentDatable = interactableObj;
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000CA6C File Offset: 0x0000AC6C
	public void ForceDialogueOnLoad(string previousDateable)
	{
		PlayerPauser.Pause(this);
		if (this.tempDatable == null)
		{
			GameObject gameObject = new GameObject();
			this.tempDatable = gameObject.AddComponent<InteractableObj>();
		}
		this.tempDatable.inkFileName = previousDateable;
		this.currentDatable = this.tempDatable;
		this.wasLoadedFromSave = true;
		this.StartStateTransition(VIEW_STATE.TALKING, false);
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000CAC6 File Offset: 0x0000ACC6
	public void MorningMusic()
	{
		base.Invoke("MorningMusic_Invoked", 3f);
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000CAD8 File Offset: 0x0000ACD8
	private void MorningMusic_Invoked()
	{
		if (Singleton<TutorialController>.Instance != null && Singleton<TutorialController>.Instance.inCinematic)
		{
			return;
		}
		if (TalkingUI.Instance != null && TalkingUI.Instance.open)
		{
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack("Overworld_" + Singleton<DayNightCycle>.Instance.GetTime().ToString(), AUDIO_TYPE.MUSIC, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000CB5A File Offset: 0x0000AD5A
	public void StartSongSubtitle()
	{
		this.talkingUI.ContinuePressed();
		base.Invoke("ContinueChatOnSong", 3f);
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000CB77 File Offset: 0x0000AD77
	public void ContinueChatOnSong()
	{
		if (Singleton<InkController>.Instance.CanContinue())
		{
			this.talkingUI.ContinuePressed();
			base.Invoke("ContinueChatOnSong", 3f);
		}
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
	public void ForceDialogue(string node, GameController.DelegateAfterChatEndsEvents delegateMethod = null, bool stopTime = false, bool consumeCharge = false)
	{
		if (this.viewState == VIEW_STATE.TALKING && delegateMethod != null)
		{
			delegateMethod();
			return;
		}
		this.stopTime = stopTime;
		this.methodToCallWhenChatEnds = delegateMethod;
		PlayerPauser.Pause(this);
		this.tempDatable.inkFileName = node;
		this.currentDatable = this.tempDatable;
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_awakened, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.STINGER, false);
		if (Singleton<Save>.Instance.MeetDatableIfUnmet(node) || consumeCharge)
		{
			Singleton<Dateviators>.Instance.ConsumeCharge();
		}
		this.StartStateTransition(VIEW_STATE.TALKING, false);
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000CC34 File Offset: 0x0000AE34
	public static InteractableObj FindInteractableObjInScene(string dateable_name)
	{
		return Object.FindObjectsOfType<InteractableObj>().FirstOrDefault((InteractableObj i) => i.inkFileName == dateable_name);
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000CC64 File Offset: 0x0000AE64
	public InteractableObj GetCurrentLookingAt()
	{
		return this.currentDatable;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000CC6C File Offset: 0x0000AE6C
	private bool CheckLucinda(InteractableObj iObj)
	{
		if (!Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION || !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) || Singleton<Save>.Instance.GetDateableTalkedTo("lucinda") == Singleton<DayNightCycle>.Instance.GetDaysSinceStart())
		{
			return false;
		}
		string text = iObj.InternalName();
		if (text == "daemon" || text == "dorian" || text == "doug" || text == "nightmare" || text == "phoenicia" || text == "reggie" || text == "skylar" || text == "sassy" || text == "tbc" || text == "willi" || text == "zoey" || text == "rebel")
		{
			return false;
		}
		string variable = Singleton<InkController>.Instance.GetVariable("lucinda_final");
		if (iObj.InternalName() == "lucinda" && variable != "lust")
		{
			return false;
		}
		string variable2 = Singleton<InkController>.Instance.GetVariable("lucinda_frequency");
		int num;
		if (!(variable2 == "low"))
		{
			if (!(variable2 == "med"))
			{
				if (!(variable2 == "high"))
				{
					num = 0;
				}
				else
				{
					num = 25;
				}
			}
			else
			{
				num = 7;
			}
		}
		else
		{
			num = 5;
		}
		return Random.Range(1, 100) <= num;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
	private bool CheckDorianReggieEdgeCase()
	{
		return ((!Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalFriendEndings() == 99) || (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalFriendEndings() == 101)) && Singleton<Save>.Instance.GetDateStatusRealized("dorian") == RelationshipStatus.Realized && Singleton<Save>.Instance.GetDateStatus("reggie") == RelationshipStatus.Unmet;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000CE50 File Offset: 0x0000B050
	private bool CheckRebel()
	{
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			return false;
		}
		if (this.currentDatable.InternalName() == "rebel")
		{
			return false;
		}
		GameObject dateable = MovingDateable.GetDateable("MovingRubberDuck", "gone");
		if (dateable != null && !dateable.activeInHierarchy)
		{
			return false;
		}
		string text = this.RebelInteractableObj.InternalName();
		if (Singleton<Save>.Instance.GetDateStatus(text) == RelationshipStatus.Unmet || Singleton<Save>.Instance.GetDateStatus(text) == RelationshipStatus.Realized || Singleton<Save>.Instance.GetDateableTalkedTo("rebel") == Singleton<DayNightCycle>.Instance.GetDaysSinceStart())
		{
			return false;
		}
		string variable = Singleton<InkController>.Instance.GetVariable("rebel_popin");
		int num;
		if (!(variable == "low"))
		{
			if (!(variable == "high"))
			{
				num = 0;
			}
			else
			{
				num = 70;
			}
		}
		else
		{
			num = 40;
		}
		return Random.Range(1, 100) <= num;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000CF31 File Offset: 0x0000B131
	private void StartRebel()
	{
		if (this.SelectObj(this.RebelInteractableObj, true, null, true, false, false) != GameController.SelectObjResult.CHAT_STARTED)
		{
			this.AutoSave();
		}
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000CF4D File Offset: 0x0000B14D
	private void StartDorianReggie()
	{
		if (this.SelectObj(this.DorianReggieInteractableObj, true, null, true, false, false) != GameController.SelectObjResult.CHAT_STARTED)
		{
			this.AutoSave();
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000CF6C File Offset: 0x0000B16C
	public bool CanSelectObj(InteractableObj iObj, bool forceWithoutGlasses = false, bool stopTime = false, bool ignoreGlassesAwakening = false, bool isFrontDoorInteraction = false)
	{
		string text = iObj.InternalName();
		if (text != "" && (Singleton<Dateviators>.Instance.Equipped || forceWithoutGlasses) && !ignoreGlassesAwakening)
		{
			string text2 = text;
			if (text2 == "cf")
			{
				text2 = "celia";
			}
			if (iObj.inkFileName.StartsWith("dorian_door.dorian_complete_reggie_needed"))
			{
				text2 = "reggie";
			}
			if (Singleton<Save>.Instance.GetDateStatusRealized(text2) != RelationshipStatus.Realized)
			{
				if (Singleton<Save>.Instance.GetDateableTalkedTo(text) == Singleton<DayNightCycle>.Instance.GetDaysSinceStart())
				{
					return false;
				}
				if (iObj.inkFileName != "" && (this.IsChargeRequiredForDateable(text) && !Singleton<Dateviators>.Instance.CanConsumeCharge() && !stopTime))
				{
					return false;
				}
			}
			else if (!isFrontDoorInteraction)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000D034 File Offset: 0x0000B234
	public GameController.SelectObjResult SelectObj(InteractableObj iObj, bool forceWithoutGlasses = false, GameController.DelegateAfterChatEndsEvents delegateMethod = null, bool stopTime = false, bool ignoreGlassesAwakening = false, bool isFrontDoorInteraction = false)
	{
		GameController.SelectObjResult selectObjResult = GameController.SelectObjResult.FAILED;
		if (this.CheckLucinda(iObj))
		{
			this.currentDatable = this.LucindaInteractableObj;
		}
		else
		{
			this.currentDatable = iObj;
		}
		this.forceWithoutGlasses = forceWithoutGlasses;
		this.methodToCallWhenChatEnds = delegateMethod;
		this.stopTime = stopTime;
		string text = this.currentDatable.InternalName();
		BetterPlayerControl.Instance.StopBeamSounds();
		if (text != "" && (Singleton<Dateviators>.Instance.Equipped || forceWithoutGlasses) && !ignoreGlassesAwakening)
		{
			string text2 = text;
			if (text2 == "cf")
			{
				text2 = "celia";
			}
			if (this.currentDatable.inkFileName.StartsWith("dorian_door.dorian_complete_reggie_needed"))
			{
				text2 = "reggie";
			}
			if (Singleton<Save>.Instance.GetDateStatusRealized(text2) != RelationshipStatus.Realized)
			{
				if (Singleton<Save>.Instance.GetDateableTalkedTo(text) == Singleton<DayNightCycle>.Instance.GetDaysSinceStart())
				{
					Singleton<Popup>.Instance.CreatePopup("Oops", "You already talked to this object today. Try again tomorrow!", true);
					return GameController.SelectObjResult.FAILED;
				}
				if (this.currentDatable.inkFileName != "")
				{
					bool flag = this.IsChargeRequiredForDateable(text);
					if (!flag || Singleton<Dateviators>.Instance.CanConsumeCharge() || stopTime)
					{
						Singleton<Save>.Instance.SetDateableTalkedTo(text);
						if (!stopTime && flag)
						{
							Singleton<Dateviators>.Instance.ConsumeCharge();
						}
						this.currentDatable.StartDialogue();
						this.StartStateTransition(VIEW_STATE.TALKING, false);
						return GameController.SelectObjResult.CHAT_STARTED;
					}
					Singleton<Popup>.Instance.CreatePopup("Oops", "You need to use a charge to awaken this object, but the Dateviators are out of battery. Sleep to recharge the battery!", false);
					return GameController.SelectObjResult.FAILED;
				}
			}
			else if (!isFrontDoorInteraction)
			{
				return GameController.SelectObjResult.FAILED;
			}
		}
		else if (this.currentDatable.AlternateInteractions.Count != 0)
		{
			foreach (Interactable interactable in this.currentDatable.AlternateInteractions)
			{
				if (!(interactable == null) && interactable.CheckCanUse())
				{
					selectObjResult = GameController.SelectObjResult.ALT_INTERACTION;
					interactable.Interact();
					interactable.ToggleInteractedWith(BetterPlayerControl.Instance.transform.position, false);
				}
			}
		}
		return selectObjResult;
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000D238 File Offset: 0x0000B438
	private bool IsChargeRequiredForDateable(string internalCharacterName)
	{
		return !(internalCharacterName == "zoey") || this.DoesRequireChargeToInteractWithZoey();
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000D24F File Offset: 0x0000B44F
	private bool DoesRequireChargeToInteractWithZoey()
	{
		return GhostController.IsSeanceComplete();
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000D25B File Offset: 0x0000B45B
	public void SetDelegateMethodWhenChatEnds(GameController.DelegateAfterChatEndsEvents delegateMethod)
	{
		this.methodToCallWhenChatEnds = delegateMethod;
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000D264 File Offset: 0x0000B464
	public bool ForceSelectObj(InteractableObj iObj, GameController.DelegateAfterChatEndsEvents delegateMethod = null)
	{
		this.currentDatable = iObj;
		this.forceWithoutGlasses = true;
		this.methodToCallWhenChatEnds = delegateMethod;
		this.stopTime = true;
		iObj.StartDialogue();
		this.StartStateTransition(VIEW_STATE.TALKING, false);
		return true;
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000D294 File Offset: 0x0000B494
	public void ReturnToHouse(InteractableObj iObj)
	{
		Singleton<Analytics>.Instance.AddReturnHome();
		if (this.currentDatable == null)
		{
			this.currentDatable = iObj;
		}
		if (this.currentDatable != iObj && iObj != null)
		{
			this.currentDatable = iObj;
		}
		if (Singleton<InkController>.Instance.dialogueManager == null)
		{
			T17Debug.LogError("Dialogue Manager is null.");
		}
		else
		{
			Singleton<InkController>.Instance.dialogueManager.SpotlightOff(null);
		}
		this.StartStateTransition(VIEW_STATE.HOUSE, false);
		if (this.methodToCallWhenChatEnds != null)
		{
			this.methodToCallWhenChatEnds();
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000D321 File Offset: 0x0000B521
	public InteractableObj GetCurrentActiveDatable()
	{
		if (this.currentDatable == null)
		{
			return null;
		}
		return this.currentDatable;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000D33C File Offset: 0x0000B53C
	public void StartStateTransition(VIEW_STATE to, bool stayPaused = false)
	{
		VIEW_STATE viewState = this.viewState;
		if (viewState == VIEW_STATE.HOUSE && to == VIEW_STATE.TALKING)
		{
			float num;
			this.AudioMixer.GetFloat("ambient_volume", out num);
			this.AudioMixer.SetFloat("ambient_volume", num - 12f);
			float num2;
			this.AudioMixer.GetFloat("foley_volume", out num2);
			this.AudioMixer.SetFloat("foley_volume", num2 - 80f);
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.25f);
			PlayerPauser.Pause(this);
			InteractableObj[] array = Object.FindObjectsOfType<InteractableObj>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetInkFileNamePrefix() == this.currentDatable.GetInkFileNamePrefix())
				{
					Singleton<Save>.Instance.SetDateableTalkedTo(this.currentDatable.internalCharacterName);
				}
			}
			this.endTransition = true;
			Singleton<Dateviators>.Instance.DisableReticle();
			Singleton<RecipeAnim>.Instance.stopanim();
			this.DialogueEnter.Invoke();
			Singleton<MessageLogManager>.Instance.Additem(MessageType.Event, "==Started Talking To " + this.currentDatable.InternalName() + "==", null);
			Singleton<InkController>.Instance.LoadPassageKeepCallStack(this.currentDatable.KnotName());
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.SPECIAL, 0.5f);
			return;
		}
		if (viewState == VIEW_STATE.TALKING && to == VIEW_STATE.HOUSE)
		{
			this.shouldGoToHouse = true;
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000D4A5 File Offset: 0x0000B6A5
	public void SetDoNotPostProcessOnReturn()
	{
		this.postProcessOnReturn = false;
		this.HUDanimator.SetBool("DateviatorsOff", true);
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
	private void GoToHouseTriggers()
	{
		this.endTransition = true;
		float num;
		this.AudioMixer.GetFloat("ambient_volume", out num);
		this.AudioMixer.SetFloat("ambient_volume", num + 12f);
		float num2;
		this.AudioMixer.GetFloat("foley_volume", out num2);
		this.AudioMixer.SetFloat("foley_volume", num2 + 80f);
		Singleton<MessageLogManager>.Instance.Additem(MessageType.Event, "==Stopped Talking To " + this.currentDatable.InternalName() + "==", null);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_transition_house, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		BetterPlayerControl.Instance.ShrinkPlayer();
		Singleton<BossBattleHealthBar>.Instance.DisableBars();
		Singleton<InkController>.Instance.CheckCriticalPath();
		Singleton<InkController>.Instance.story.ChoosePathString("home", true, Array.Empty<object>());
		Singleton<InkController>.Instance.HideChatBackgroundImage();
		if (this.currentDatable != null && this.currentDatable.inkFileName == "zoey_ghost")
		{
			GhostController ghostController = Object.FindFirstObjectByType<GhostController>();
			if (ghostController != null)
			{
				ghostController.DisableAfterConversarion();
			}
		}
		Singleton<CollectableAnim>.Instance.stopanim();
		this.talkingUI.ToggleOpen(false);
		CursorLocker.ForceLock();
		PlayerPauser.ForceUnpause();
		if (!this.stopTime)
		{
			if (!this.endgameTriggered && !Locket.IsLocketEnabled())
			{
				Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.SPECIAL, 0.4f);
				Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.2f);
				Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 1f);
			}
			Singleton<DayNightCycle>.Instance.IncrementTime();
		}
		else if (this.stopTime && this.currentDatable.inkFileName == "rebel_duck")
		{
			DayNightCycle.PlayDayMusic(0f);
		}
		this.stopTime = false;
		if (Singleton<Dateviators>.Instance.Equipped)
		{
			if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1000_DATEVIATORS_GONE))
			{
				Singleton<Dateviators>.Instance.Dequip();
			}
			if (this.postProcessOnReturn)
			{
				Singleton<Dateviators>.Instance.ForcePostProcessVolumeTo1();
			}
			else
			{
				Singleton<Dateviators>.Instance.Dequip();
			}
		}
		Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.PLAYTHROUGH);
		DateADex.Instance.UnlockCollectableRelease();
		Singleton<Save>.Instance.RevertTemporaryAwakening();
		this.shouldGoToHouse = false;
		this.postProcessOnReturn = true;
		if (this.shouldTriggerKeithOpeningDoor)
		{
			this.shouldTriggerKeithOpeningDoor = false;
			AtticDoorUnlocker atticDoorUnlocker = Object.FindObjectOfType<AtticDoorUnlocker>(true);
			if (atticDoorUnlocker != null)
			{
				atticDoorUnlocker.gameObject.SetActive(true);
				MovingDateable.MoveDateable("MovingPlants", "attic", true);
				atticDoorUnlocker.StartSequence();
				return;
			}
		}
		else
		{
			if (this.CheckDorianReggieEdgeCase())
			{
				base.Invoke("StartDorianReggie", 0.3f);
				return;
			}
			if (this.CheckRebel())
			{
				base.Invoke("StartRebel", 0.3f);
				return;
			}
			base.Invoke("AutoSave", 0.3f);
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000D78C File Offset: 0x0000B98C
	private void AutoSave()
	{
		base.StopCoroutine(this.WaitForAutoSave());
		base.StartCoroutine(this.WaitForAutoSave());
		Save.AutoSaveGame();
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000D7AC File Offset: 0x0000B9AC
	private void PlayDayMusicDelayed()
	{
		DayNightCycle.PlayDayMusic(0f);
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
	private void EndStateTransition(VIEW_STATE to)
	{
		VIEW_STATE viewState = this.viewState;
		if (viewState == VIEW_STATE.HOUSE && to == VIEW_STATE.TALKING)
		{
			PlayerPauser.Pause(this);
			CursorLocker.Unlock();
			Singleton<TutorialController>.Instance.car1GameObject.SetActive(false);
			Singleton<TutorialController>.Instance.car2GameObject.SetActive(false);
			this.talkingUI.ToggleOpen(true);
			base.StartCoroutine(this.DelayCharacterShow());
		}
		else if (viewState == VIEW_STATE.TALKING && to == VIEW_STATE.HOUSE)
		{
			PlayerPauser.Unpause();
		}
		this.viewState = to;
		if (this.viewState == VIEW_STATE.HOUSE)
		{
			PlayerPauser.Unpause();
			if (!this.endgameTriggered)
			{
				UnityEvent<List<string>> dialogueEndings = this.DialogueEndings;
				if (dialogueEndings != null)
				{
					dialogueEndings.Invoke(new List<string>(this.characterEndings));
				}
				this.characterEndings = new List<string>();
			}
			if (Singleton<InkController>.Instance.story.canContinue)
			{
				Singleton<InkController>.Instance.ContinueStory();
			}
			Singleton<InkController>.Instance.ResetGoHome();
			if (!BetterPlayerControl.Instance.isGameEndingOn)
			{
				Singleton<TutorialController>.Instance.car1GameObject.SetActive(true);
				Singleton<TutorialController>.Instance.car2GameObject.SetActive(true);
			}
			if (this.endgameTriggered)
			{
				this.endgameTriggered = false;
				return;
			}
			base.StartCoroutine(this.WaitForAutoSave());
		}
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000D8DB File Offset: 0x0000BADB
	private IEnumerator WaitForAutoSave()
	{
		Singleton<Dateviators>.Instance.ResetTriggers();
		Singleton<Dateviators>.Instance.ShowReticle();
		yield return new WaitUntil(() => SaveIndicatorController.Instance.IsInProgressIndicatorActive);
		yield return new WaitUntil(() => !SaveIndicatorController.Instance.IsInProgressIndicatorActive);
		this.DialogueExit.Invoke();
		yield return new WaitUntil(() => !Singleton<AudioManager>._instance.CurrentTracks.Find((AudioManager.MusicChild music) => music != null && music.Type == AUDIO_TYPE.MUSIC));
		DayNightCycle.PlayDayMusic(0f);
		yield break;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000D8EC File Offset: 0x0000BAEC
	private void CheckTransition()
	{
		if (Singleton<GameController>.Instance.viewState == VIEW_STATE.HOUSE)
		{
			if (this.endTransition)
			{
				this.endTransition = false;
				this.EndStateTransition(VIEW_STATE.TALKING);
				return;
			}
		}
		else if (Singleton<GameController>.Instance.viewState == VIEW_STATE.TALKING && this.endTransition)
		{
			this.endTransition = false;
			this.EndStateTransition(VIEW_STATE.HOUSE);
		}
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000D940 File Offset: 0x0000BB40
	private void ChangeControllerMap(string mapName)
	{
		this.player.controllers.maps.SetAllMapsEnabled(false);
		this.player.controllers.maps.SetMapsEnabled(true, "Default");
		this.player.controllers.maps.SetMapsEnabled(true, mapName);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000D998 File Offset: 0x0000BB98
	public void LateUpdate()
	{
		if (this.GetCurrentActiveDatable() != null)
		{
			this.CheckTransition();
		}
		if (this.shouldGoToHouse && Singleton<SpecStatMain>.Instance && ResultSplashScreen.Instance && AwakenSplashScreen.Instance)
		{
			if (this.shouldGoToHouse && !ResultSplashScreen.Instance.isOpen && !Singleton<SpecStatMain>.Instance.visible && !AwakenSplashScreen.Instance.isOpen && !Locket.IsLocketEnabled())
			{
				this.GoToHouseTriggers();
				return;
			}
		}
		else if (this.shouldGoToHouse)
		{
			this.GoToHouseTriggers();
		}
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000DA2D File Offset: 0x0000BC2D
	public void Sleep()
	{
		this.UpdateHUDForTimeChange.Invoke();
		this.UpdateHUD.Invoke();
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000DA45 File Offset: 0x0000BC45
	public void TriggerEndGame()
	{
		this.endgameTriggered = true;
		if (Singleton<Dateviators>.Instance.Equipped)
		{
			Singleton<Dateviators>.Instance.DequipOverride();
		}
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0000DA64 File Offset: 0x0000BC64
	public IEnumerator DelayCharacterShow()
	{
		Singleton<ExamineController>.Instance.HideExamine();
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_transition_character, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		yield return new WaitForSeconds(0.2f);
		this.talkingUI.InitializeCurrentDialogue(this.wasLoadedFromSave);
		this.wasLoadedFromSave = false;
		yield return null;
		yield break;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000DA73 File Offset: 0x0000BC73
	public void ShowEmote(string characterName, EmoteReaction emoteReaction, EmoteHeight height)
	{
		this.talkingUI.ShowEmote(characterName, emoteReaction, height);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000DA83 File Offset: 0x0000BC83
	public void HideEmotes(string characterName)
	{
		this.talkingUI.HideEmotes(characterName);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000DA91 File Offset: 0x0000BC91
	public void HideEmote(string characterName, EmoteReaction emote)
	{
		this.talkingUI.HideEmote(characterName, emote);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
	public void ForceShowHUD()
	{
		this.HUD.gameObject.SetActive(true);
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000DAB3 File Offset: 0x0000BCB3
	public void ForceHideHUD()
	{
		this.HUD.gameObject.SetActive(false);
	}

	// Token: 0x04000308 RID: 776
	private VIEW_STATE _viewState = VIEW_STATE.HOUSE;

	// Token: 0x04000309 RID: 777
	public string homeMusic;

	// Token: 0x0400030A RID: 778
	private InteractableObj currentDatable;

	// Token: 0x0400030B RID: 779
	private InteractableObj tempDatable;

	// Token: 0x0400030C RID: 780
	public TalkingUI talkingUI;

	// Token: 0x0400030D RID: 781
	public Canvas HUD;

	// Token: 0x0400030E RID: 782
	public Animator HUDanimator;

	// Token: 0x0400030F RID: 783
	public InteractableObj LucindaInteractableObj;

	// Token: 0x04000310 RID: 784
	public InteractableObj RebelInteractableObj;

	// Token: 0x04000311 RID: 785
	public InteractableObj DorianReggieInteractableObj;

	// Token: 0x04000312 RID: 786
	[HideInInspector]
	public UnityEvent UpdateHUD;

	// Token: 0x04000313 RID: 787
	[HideInInspector]
	public UnityEvent UpdateHUDForTimeChange;

	// Token: 0x04000314 RID: 788
	public UnityEvent DialogueEnter;

	// Token: 0x04000315 RID: 789
	public UnityEvent DialogueExit;

	// Token: 0x04000316 RID: 790
	public UnityEvent<List<string>> DialogueEndings = new UnityEvent<List<string>>();

	// Token: 0x04000317 RID: 791
	public string lastFlowName;

	// Token: 0x04000318 RID: 792
	public List<string> characterEndings = new List<string>();

	// Token: 0x04000319 RID: 793
	public bool isUnskippable;

	// Token: 0x0400031A RID: 794
	public bool isSubtitle;

	// Token: 0x0400031B RID: 795
	public bool shouldGoToHouse;

	// Token: 0x0400031C RID: 796
	public bool shouldTriggerKeithOpeningDoor;

	// Token: 0x0400031D RID: 797
	protected Dictionary<string, AudioClip> _musicFiles;

	// Token: 0x0400031E RID: 798
	public bool wasLoadedFromSave;

	// Token: 0x0400031F RID: 799
	public string saveDataString = "";

	// Token: 0x04000320 RID: 800
	private Player player;

	// Token: 0x04000321 RID: 801
	private bool forceWithoutGlasses;

	// Token: 0x04000322 RID: 802
	public bool debugMagical;

	// Token: 0x04000323 RID: 803
	private GameController.DelegateAfterChatEndsEvents methodToCallWhenChatEnds;

	// Token: 0x04000324 RID: 804
	private bool stopTime;

	// Token: 0x04000325 RID: 805
	public bool endgameTriggered;

	// Token: 0x04000326 RID: 806
	[SerializeField]
	private AudioMixer AudioMixer;

	// Token: 0x04000327 RID: 807
	private List<InteractableObj> interactables;

	// Token: 0x04000328 RID: 808
	private bool endTransition;

	// Token: 0x04000329 RID: 809
	private bool postProcessOnReturn = true;

	// Token: 0x0400032A RID: 810
	public bool intro;

	// Token: 0x020002B3 RID: 691
	public enum SelectObjResult
	{
		// Token: 0x040010C1 RID: 4289
		FAILED,
		// Token: 0x040010C2 RID: 4290
		CHAT_STARTED,
		// Token: 0x040010C3 RID: 4291
		ALT_INTERACTION
	}

	// Token: 0x020002B4 RID: 692
	// (Invoke) Token: 0x06001548 RID: 5448
	public delegate void DelegateAfterChatEndsEvents();
}
