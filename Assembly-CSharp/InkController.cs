using System;
using System.Collections.Generic;
using System.Linq;
using CowberryStudios.ProjectAI;
using Date_Everything.Scripts.Ink;
using Ink;
using Ink.Runtime;
using Team17.Common;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class InkController : Singleton<InkController>
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060002FC RID: 764 RVA: 0x00012920 File Offset: 0x00010B20
	public StageManager stageManager
	{
		get
		{
			StageManager stageManager = this.stageManagerRef;
			return this.stageManagerRef;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060002FD RID: 765 RVA: 0x0001292F File Offset: 0x00010B2F
	public bool AllowQuickResponseChoices
	{
		get
		{
			return this.story.currentChoices.Count >= 1 && this.story.currentChoices.Count <= 4;
		}
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0001295C File Offset: 0x00010B5C
	public void HideChatBackgroundImage()
	{
		this.stageManagerRef.HideBackgroundImage();
		this.stageManagerRef.HideForegroundImage();
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00012974 File Offset: 0x00010B74
	public void SetStageManagerRef(in StageManager stageRef)
	{
		this.stageManagerRef = stageRef;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0001297E File Offset: 0x00010B7E
	public void ResetStageManagerRef()
	{
		this.stageManagerRef = null;
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000301 RID: 769 RVA: 0x00012987 File Offset: 0x00010B87
	public DialogueTagManager dialogueManager
	{
		get
		{
			DialogueTagManager dialogueTagManager = this.dialogueManagerRef;
			return this.dialogueManagerRef;
		}
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00012996 File Offset: 0x00010B96
	public void SetDialogueTagManagerRef(in DialogueTagManager dialogueManagerRef)
	{
		this.dialogueManagerRef = dialogueManagerRef;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x000129A0 File Offset: 0x00010BA0
	public void ResetDialogueManagerRef()
	{
		this.dialogueManagerRef = null;
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000304 RID: 772 RVA: 0x000129A9 File Offset: 0x00010BA9
	public InkBinding binding
	{
		get
		{
			InkBinding binding = this._binding;
			return this._binding;
		}
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000129B8 File Offset: 0x00010BB8
	public void TrySetVariable(string variableName, string stateToSet)
	{
		this.story.variablesState[variableName] = stateToSet;
	}

	// Token: 0x06000306 RID: 774 RVA: 0x000129CC File Offset: 0x00010BCC
	public string GetVariable(string variableName)
	{
		if (this.story.variablesState.Contains(variableName))
		{
			return this.story.variablesState[variableName].ToString();
		}
		return null;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x000129FC File Offset: 0x00010BFC
	public bool CompareVariable(string variableName, string comparedResult)
	{
		string text = this.GetVariable(variableName);
		if (text == null)
		{
			T17Debug.LogError("[INK CONTROLLER] Variable request " + variableName + " was invalid");
			return false;
		}
		text = text.ToLowerInvariant();
		return text == comparedResult;
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000308 RID: 776 RVA: 0x00012A3E File Offset: 0x00010C3E
	public Story story
	{
		get
		{
			return Singleton<InkStoryProvider>.Instance.Story;
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00012A4A File Offset: 0x00010C4A
	public string GetStoryAppPrefix(string stitch)
	{
		if (stitch == null)
		{
			return null;
		}
		if (stitch.StartsWith("canopy_work."))
		{
			return stitch;
		}
		if (stitch.Contains("_"))
		{
			return stitch.Substring(0, stitch.LastIndexOf("_"));
		}
		return stitch;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00012A81 File Offset: 0x00010C81
	public void SetSaveToLoad(string saveState)
	{
		this.SaveStateToLoad = saveState;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00012A8A File Offset: 0x00010C8A
	public InkBinding FinishedLoadingIn()
	{
		this._binding = new InkBinding();
		this.SetStateSave(this.SaveStateToLoad);
		return this._binding;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00012AA9 File Offset: 0x00010CA9
	public void Start()
	{
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00012AAC File Offset: 0x00010CAC
	public override void AwakeSingleton()
	{
		this._binding = new InkBinding();
		this.goHome = false;
		this.goCritPath = false;
		this.previousChoices = new Stack<PrevChoice>();
		base.gameObject.AddComponent<KnotJumpTool>();
		base.gameObject.AddComponent<InkVariableTool>();
		base.gameObject.AddComponent<MessyModelsTool>();
		base.gameObject.AddComponent<DateableEndingTool>();
		this.text = "";
		this.path = "";
		this.tags = new List<ParsedTag>();
		this.choices = new List<Choice>();
		if (this.LocalisationData != null)
		{
			Singleton<Localisation>.Instance.LoadCSV(this.LocalisationData.text);
		}
		if (!this.story.HasExternalFunction("openLocket"))
		{
			this.story.BindExternalFunction("openLocket", delegate
			{
				this.locketObj.SetActive(true);
			}, false);
		}
		global::UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00012B91 File Offset: 0x00010D91
	private void ResetData()
	{
		this.text = "";
		this.path = "";
		this.tags = new List<ParsedTag>();
		this.choices = new List<Choice>();
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00012BBF File Offset: 0x00010DBF
	private void StoryOnError(string message, ErrorType type)
	{
		if (type != ErrorType.Warning)
		{
			T17Debug.LogError(message);
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00012BCB File Offset: 0x00010DCB
	public void JumpToKnot(string path)
	{
		this.story.ChoosePathString(path, true, Array.Empty<object>());
		this.ResetData();
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00012BE8 File Offset: 0x00010DE8
	public void JumpToKnot(string path, bool _resetCallStack)
	{
		this.PreDialogueSaveState = this.story.state.ToJson();
		this.inkKnotLoaded = path;
		this.story.ChoosePathString(path, _resetCallStack, Array.Empty<object>());
		if (Debug.isDebugBuild)
		{
			this.TrySetVariable("inDebugBuild", "true");
			return;
		}
		this.TrySetVariable("inDebugBuild", "false");
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00012C4C File Offset: 0x00010E4C
	public string GetInkKnotLoaded()
	{
		return this.inkKnotLoaded;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00012C54 File Offset: 0x00010E54
	public void LoadPassageFromHome(string path)
	{
		this.previousChoices.Clear();
		this.JumpToKnot(path, true);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00012C69 File Offset: 0x00010E69
	public void LoadPassage(string path)
	{
		this.previousChoices.Clear();
		this.JumpToKnot(path, true);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00012C7E File Offset: 0x00010E7E
	public void LoadPassageKeepCallStack(string path)
	{
		this._binding = new InkBinding();
		this.previousChoices.Clear();
		this.JumpToKnot(path, false);
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00012C9E File Offset: 0x00010E9E
	public void LoadPassage(InteractableObj obj)
	{
		this.LoadPassage(obj.KnotName());
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00012CAC File Offset: 0x00010EAC
	public void ResetGoHome()
	{
		this.ResetStageManagerRef();
		this.ResetDialogueManagerRef();
		this.goHome = false;
		this.goCritPath = false;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00012CC8 File Offset: 0x00010EC8
	public void UpdateDataDirty()
	{
		this.path = this.story.state.currentPathString;
		this.text = this.GetClearCurrentStoryText();
		this.choices = this.story.currentChoices;
		this.UpdateTags();
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00012D04 File Offset: 0x00010F04
	public bool ShouldForceGoHomeAfterCritPath()
	{
		if (this.goHome && this.goCritPath)
		{
			this.goHome = false;
			this.goCritPath = false;
			return true;
		}
		return false;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00012D28 File Offset: 0x00010F28
	public void Continue()
	{
		if (!this.story.canContinue)
		{
			return;
		}
		this.path = this.story.state.currentPathString;
		bool flag = this.debug;
		for (bool flag2 = true; flag2 || this.StartOfConversationCheck(); flag2 = false)
		{
			this.ContinueStory();
			bool flag3 = this.debug;
			if (!this.goCritPath && this.story.currentTags.Count == 1 && this.story.currentTags.Contains("home"))
			{
				this.CheckCriticalPath();
			}
			else
			{
				if (this.story.state.currentPathString == "home" || (this.story.currentTags.Count == 1 && this.story.currentTags.Contains("home")))
				{
					this.goHome = true;
					this.goCritPath = false;
					return;
				}
				if (this.story.currentTags.Contains("home"))
				{
					this.goHome = true;
					this.goCritPath = false;
				}
			}
			this.text = this.GetClearCurrentStoryText();
			bool flag4 = this.debug;
			this.UpdateChoices();
			this.UpdateTags();
		}
		if (!this.HasText() && this.choices.Count == 0 && this.tags.Count == 0 && this.story.canContinue)
		{
			bool flag5 = false;
			for (int i = 0; i < this.k_WhiteSpaceErrorRetries; i++)
			{
				if (this.TryContinue())
				{
					flag5 = true;
					break;
				}
			}
			if (!flag5)
			{
				this.goHome = true;
				bool flag6 = this.debug;
				return;
			}
			this.UpdateDataDirty();
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00012EC4 File Offset: 0x000110C4
	public string ContinueStory()
	{
		string text = this.story.Continue();
		while (text.Trim() == "" && this.story.canContinue)
		{
			this.UpdateTags();
			text = this.story.Continue();
		}
		return text;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00012F12 File Offset: 0x00011112
	private void UpdateChoices()
	{
		this.choices = this.story.currentChoices;
		bool flag = this.debug;
		if (this.choices.Count > 0)
		{
			this.outOfChoice = false;
			return;
		}
		this.outOfChoice = true;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00012F4C File Offset: 0x0001114C
	private bool StartOfConversationCheck()
	{
		if (this.text.StartsWith("false"))
		{
			return true;
		}
		if (this.text.Trim(new char[] { '\n', ' ', '\r' }) == string.Empty && this.CanContinue() && this.story.currentChoices.Count == 0)
		{
			if (this.story.currentTags.Count == 0)
			{
				return true;
			}
			using (List<string>.Enumerator enumerator = this.story.currentTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Contains("main_text"))
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0001301C File Offset: 0x0001121C
	private int GetHateNumber()
	{
		return Singleton<Save>.Instance.AvailableTotalHateEndings();
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00013028 File Offset: 0x00011228
	private int GetAwakenedNumber()
	{
		return Singleton<Save>.Instance.AvailableTotalMetDatables();
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00013034 File Offset: 0x00011234
	private int GetRealizedNumber()
	{
		return Singleton<Save>.Instance.AvailableTotalRealizedDatables();
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00013040 File Offset: 0x00011240
	private void ShowTutorialPopup3AfterAwakeningBetty()
	{
		Singleton<TutorialController>.Instance.ShowTutorialPopup3AfterAwakeningBetty();
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0001304C File Offset: 0x0001124C
	public void CheckCriticalPath()
	{
		if (Singleton<GameController>.Instance.endgameTriggered || Locket.IsLocketEnabled())
		{
			return;
		}
		DateADex.Instance.UnlockCollectableRelease();
		this.goHome = true;
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetDateStatus("betty_bed") != RelationshipStatus.Unmet && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO))
		{
			base.Invoke("ShowTutorialPopup3AfterAwakeningBetty", 6f);
		}
		if (Singleton<InkController>.Instance.story.variablesState["realize_skylar_asap"].ToString() == "on" || this.GetRealizedNumber() == 50)
		{
			Singleton<PhoneManager>.Instance.SetNewMessageAlertSkylar();
		}
		this.JumpToKnot("critical_path_sorter", false);
		if (Singleton<SpecStatMain>.Instance.GetStatLevel("smarts", true) >= 110 && Singleton<SpecStatMain>.Instance.GetStatLevel("poise", true) >= 110 && Singleton<SpecStatMain>.Instance.GetStatLevel("empathy", true) >= 110 && Singleton<SpecStatMain>.Instance.GetStatLevel("charm", true) >= 110)
		{
			Singleton<SpecStatMain>.Instance.GetStatLevel("sass", true);
		}
		if (this.GetHateNumber() >= 56 && this.CompareVariable("hate_gates", "0"))
		{
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.tom_hate", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetHateNumber() >= 68 && this.CompareVariable("hate_gates", "1"))
		{
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.val_hate", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetHateNumber() >= 80 && this.CompareVariable("hate_gates", "2"))
		{
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_hate", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetHateNumber() >= 92 && this.CompareVariable("hate_gates", "3"))
		{
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_hate", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (((!Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalHateEndings() == 100) || (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.AvailableTotalHateEndings() == 102)) && this.CompareVariable("hate_gates", "4"))
		{
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.most_hate", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 8 && this.CompareVariable("critpath", "0"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_3", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 12 && this.CompareVariable("critpath", "1"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_4", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 13 && this.CompareVariable("critpath", "2"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_2", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 14 && this.CompareVariable("critpath", "3"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_5", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 15 && this.CompareVariable("critpath", "4"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_3", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 21 && this.CompareVariable("critpath", "5"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_4", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 22 && this.CompareVariable("critpath", "6"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_6", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 24 && this.CompareVariable("critpath", "7"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_4", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 30 && this.CompareVariable("critpath", "8"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_5", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 31 && this.CompareVariable("critpath", "9"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.tom_3", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 34 && this.CompareVariable("critpath", "10"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_7", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 36 && this.CompareVariable("critpath", "11"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_6", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 37 && this.CompareVariable("critpath", "12"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.val_1", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 39 && this.CompareVariable("critpath", "13"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_5", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 42 && this.CompareVariable("critpath", "14"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_8", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 43 && this.CompareVariable("critpath", "15"))
		{
			Singleton<Save>.Instance.SetInteractableState("DateADexRecipe", true);
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_7", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 46 && this.CompareVariable("critpath", "16"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_9", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetAwakenedNumber() >= 48 && this.CompareVariable("critpath", "17"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_8", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (Singleton<InkController>.Instance.story.variablesState["realize_skylar_asap"].ToString() == "asked" && this.CompareVariable("critpath", "18"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_9", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 1 && this.CompareVariable("critpath", "19"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_6", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 4 && this.CompareVariable("critpath", "20"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.tom_4", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 5 && this.CompareVariable("critpath", "21"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_10", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 10 && this.CompareVariable("critpath", "22"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_7", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 11 && this.CompareVariable("critpath", "23"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_11", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 15 && this.CompareVariable("critpath", "24"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_10", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 17 && this.CompareVariable("critpath", "25"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_12", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 20 && this.CompareVariable("critpath", "26"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.val_2", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 23 && this.CompareVariable("critpath", "27"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_13", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 25 && this.CompareVariable("critpath", "28"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_11", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 29 && this.CompareVariable("critpath", "29"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_14", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 30 && this.CompareVariable("critpath", "30"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_8", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 35 && this.CompareVariable("critpath", "31"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_15", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 37 && this.CompareVariable("critpath", "32"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_12", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 40 && this.CompareVariable("critpath", "33"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.tom_5", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 42 && this.CompareVariable("critpath", "34"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_16", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 44 && this.CompareVariable("critpath", "35"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_9", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 49 && this.CompareVariable("critpath", "36"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_17", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 50 && this.CompareVariable("critpath", "37"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_13", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 53 && this.CompareVariable("critpath", "38"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.val_3", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 55 && this.CompareVariable("critpath", "39"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_18", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 60 && this.CompareVariable("critpath", "40"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_10", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 61 && this.CompareVariable("critpath", "41"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_19", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 65 && this.CompareVariable("critpath", "42"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_14", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 70 && this.CompareVariable("critpath", "43"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_11", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 75 && this.CompareVariable("critpath", "44"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_20", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 80 && this.CompareVariable("critpath", "45"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.val_4", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 83 && this.CompareVariable("critpath", "46"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_15", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 85 && this.CompareVariable("critpath", "47"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.sam_12", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 90 && this.CompareVariable("critpath", "48"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_21", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 93 && this.CompareVariable("critpath", "49"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_22", ChatType.Wrkspce, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 94 && this.CompareVariable("critpath", "50"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			Singleton<ChatMaster>.Instance.StartChat("thiscord_phone.tfh_16", ChatType.Thiscord, false, true);
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (this.GetRealizedNumber() >= 98 && this.CompareVariable("critpath", "51"))
		{
			this.TrySetVariable("critpath", (int.Parse(this.GetVariable("critpath")) + 1).ToString());
			return;
		}
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00014E71 File Offset: 0x00013071
	public bool IsInChoice()
	{
		return !this.outOfChoice;
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00014E7C File Offset: 0x0001307C
	private bool TryContinue()
	{
		if (!this.story.canContinue)
		{
			return true;
		}
		this.path = this.story.state.currentPathString;
		this.ContinueStory();
		if (!this.goCritPath && this.story.currentTags.Count == 1 && this.story.currentTags.Contains("home"))
		{
			this.goCritPath = true;
			this.JumpToKnot("critical_path_sorter", false);
		}
		else if (this.story.state.currentPathString == "home" || (this.story.currentTags.Count == 1 && this.story.currentTags.Contains("home")))
		{
			this.goHome = true;
			this.goCritPath = false;
			return true;
		}
		this.text = this.GetClearCurrentStoryText();
		if (this.HasText())
		{
			return true;
		}
		this.choices = this.story.currentChoices;
		return this.choices.Count != 0;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00014F89 File Offset: 0x00013189
	private string GetClearCurrentStoryText()
	{
		if (this.story.currentText.TrimStart().StartsWith("true"))
		{
			return this.story.currentText.Trim().Substring(4);
		}
		return this.story.currentText;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00014FC9 File Offset: 0x000131C9
	public bool ShouldGoHome()
	{
		return this.goHome;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00014FD1 File Offset: 0x000131D1
	public void SetGoHome(bool goHome)
	{
		this.goHome = goHome;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00014FDA File Offset: 0x000131DA
	public bool CanContinue()
	{
		return this.story.canContinue;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00014FE7 File Offset: 0x000131E7
	public string GetState()
	{
		return this.story.state.ToJson();
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00014FF9 File Offset: 0x000131F9
	public string GetPreDialogueState()
	{
		return this.PreDialogueSaveState;
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00015001 File Offset: 0x00013201
	private PrevChoice GetStatePrevChoice(bool lastChoiceContinue)
	{
		return new PrevChoice(lastChoiceContinue, this.GetState());
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001500F File Offset: 0x0001320F
	public void SavePrevChoice(bool lastChoiceContinue)
	{
		this.previousChoices.Push(this.GetStatePrevChoice(lastChoiceContinue));
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00015023 File Offset: 0x00013223
	public bool GetLastChoice(out PrevChoice lastChoice)
	{
		return this.previousChoices.TryPop(out lastChoice);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00015031 File Offset: 0x00013231
	public bool GetLastChoiceSave(out PrevChoice lastChoice)
	{
		return this.previousChoices.TryPeek(out lastChoice);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0001503F File Offset: 0x0001323F
	public int PreviousChoiceCount()
	{
		return this.previousChoices.Count;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0001504C File Offset: 0x0001324C
	public void ChooseChoiceIndex(int index)
	{
		this.outOfChoice = true;
		this.story.ChooseChoiceIndex(index);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00015061 File Offset: 0x00013261
	public void SetState(string data)
	{
		this.story.state.LoadJson(data);
		this.UpdateDataDirty();
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0001507A File Offset: 0x0001327A
	public void SetStateSave(string data)
	{
		this.UpdateTags();
		this.SetState(data);
		this.GetTags();
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00015091 File Offset: 0x00013291
	public void SetState(PrevChoice pc)
	{
		this.SetState(pc.prevState);
	}

	// Token: 0x06000334 RID: 820 RVA: 0x000150A0 File Offset: 0x000132A0
	protected List<ParsedTag> UpdateTags()
	{
		foreach (string text in this.story.currentTags)
		{
			this.tags.Add(new ParsedTag(text));
		}
		return this.tags;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00015108 File Offset: 0x00013308
	public string GetText()
	{
		return this.text.Trim();
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00015115 File Offset: 0x00013315
	public bool HasText()
	{
		return !string.IsNullOrWhiteSpace(this.text) || !string.IsNullOrEmpty(this.text);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00015134 File Offset: 0x00013334
	public List<Choice> GetChoices()
	{
		return this.choices;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0001513C File Offset: 0x0001333C
	public List<ParsedTag> GetTags()
	{
		List<ParsedTag> list = this.tags;
		this.tags = new List<ParsedTag>();
		return list;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x0001514F File Offset: 0x0001334F
	public string GetLastKnownPath()
	{
		return this.path;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00015158 File Offset: 0x00013358
	public List<string> GetStitchesAtLocation(string knot)
	{
		List<string> output = new List<string>();
		this.story.KnotContainerWithName(knot).namedContent.Keys.ToList<string>().ForEach(delegate(string stitch)
		{
			output.Add(knot + "." + stitch);
		});
		return output;
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000151B4 File Offset: 0x000133B4
	public void HideBackgroundFrontgroundImages()
	{
		this.stageManagerRef.HideForegroundImage();
		this.stageManagerRef.HideBackgroundImage();
	}

	// Token: 0x04000332 RID: 818
	public bool debug;

	// Token: 0x04000333 RID: 819
	private StageManager stageManagerRef;

	// Token: 0x04000334 RID: 820
	private DialogueTagManager dialogueManagerRef;

	// Token: 0x04000335 RID: 821
	private InkBinding _binding;

	// Token: 0x04000336 RID: 822
	[SerializeField]
	private TextAsset inkJSONAsset;

	// Token: 0x04000337 RID: 823
	public TextAsset LocalisationData;

	// Token: 0x04000338 RID: 824
	protected string text;

	// Token: 0x04000339 RID: 825
	protected List<Choice> choices;

	// Token: 0x0400033A RID: 826
	protected List<ParsedTag> tags;

	// Token: 0x0400033B RID: 827
	protected Stack<PrevChoice> previousChoices;

	// Token: 0x0400033C RID: 828
	private bool outOfChoice;

	// Token: 0x0400033D RID: 829
	protected bool goCritPath;

	// Token: 0x0400033E RID: 830
	protected bool goHome;

	// Token: 0x0400033F RID: 831
	public bool forceGoHome;

	// Token: 0x04000340 RID: 832
	protected string path;

	// Token: 0x04000341 RID: 833
	protected string SaveStateToLoad;

	// Token: 0x04000342 RID: 834
	protected string PreDialogueSaveState;

	// Token: 0x04000343 RID: 835
	protected string inkKnotLoaded;

	// Token: 0x04000344 RID: 836
	protected int k_WhiteSpaceErrorRetries = 50;

	// Token: 0x04000345 RID: 837
	public GameObject locketObj;
}
