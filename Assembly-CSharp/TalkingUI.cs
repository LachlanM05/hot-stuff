using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cinemachine;
using CowberryStudios.ProjectAI;
using DG.Tweening;
using Ink.Runtime;
using JetBrains.Annotations;
using Rewired;
using T17.Services;
using Team17.Common;
using Team17.Scripts.Services.Input;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x0200013F RID: 319
public class TalkingUI : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000B8C RID: 2956 RVA: 0x000425E1 File Offset: 0x000407E1
	public StageManager StageManager
	{
		get
		{
			return this._stageManager;
		}
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x000425E9 File Offset: 0x000407E9
	private void Awake()
	{
		TalkingUI.Instance = this;
		this.player = ReInput.players.GetPlayer(0);
		this.currentSpeakers = new Dictionary<string, SpeakingCharacter>(StringComparer.OrdinalIgnoreCase);
		this.CheckAndCreateManagers();
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00042618 File Offset: 0x00040818
	public void CheckAndCreateManagers()
	{
		if (this._stageManager == null)
		{
			this._stageManager = new StageManager(this.speakerPortraits, this.speakerPositions, this.bgImg, this.foregroundImage);
		}
		if (this.dialogueTagManager == null)
		{
			this.dialogueTagManager = new DialogueTagManager(this._stageManager);
		}
		if (Singleton<InkController>.Instance.dialogueManager == null)
		{
			Singleton<InkController>.Instance.SetDialogueTagManagerRef(in this.dialogueTagManager);
		}
		if (this._stageManager != null)
		{
			Singleton<InkController>.Instance.SetStageManagerRef(in this._stageManager);
		}
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x000426A0 File Offset: 0x000408A0
	public void ToggleOpen(bool flag)
	{
		this.dialogBox.ToggleOpen(flag);
		if (flag)
		{
			this.dialogBox.ClearDialog(true);
			this.open = true;
			this.CheckAndCreateManagers();
			this.dialogueTagManager.SetBinding(Singleton<InkController>.Instance.binding);
			base.gameObject.SetActive(flag);
			this.VnAnim.SetBool("Open", true);
			this.inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.UIWithChat, this);
			return;
		}
		this.open = false;
		this.ClearChoices();
		this.OnReturnToHome();
		this.VnAnim.SetBool("Open", false);
		this.dialogueTagManager.ResetBinding(Singleton<InkController>.Instance.binding);
		InputModeHandle inputModeHandle = this.inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this.inputModeHandle = null;
		this.dialogBox.ClearDialogInvoked();
		this.isWaitingForFtb = false;
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0004277D File Offset: 0x0004097D
	public void OnReturnToHome()
	{
		this._stageManager.OnReturnToHome();
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x0004278C File Offset: 0x0004098C
	public void InitializeCurrentDialogue(bool fromSave)
	{
		this.backButton.SetActive(false);
		this.currentDatable = Singleton<GameController>.Instance.GetCurrentActiveDatable();
		UISpeaker[] array = this.speakerPortraits;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Deactivate();
		}
		this.datablePrevTextBG.gameObject.SetActive(false);
		if (fromSave)
		{
			this.InitTalkingUIFromLoad();
		}
		Singleton<Dateviators>.Instance.ResetTriggers();
		Singleton<GameController>.Instance.HUDanimator.Play("HudSlideOut");
		Singleton<Dateviators>.Instance.DisableReticle();
		this.justOpened = true;
		base.StartCoroutine(this.RefreshView(true, false));
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0004282C File Offset: 0x00040A2C
	public void InitTalkingUIFromLoad()
	{
		NameToStagePosition characterToStagePosition = Save.GetSaveData(false).characterToStagePosition;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (string text in characterToStagePosition.Keys)
		{
			dictionary.Add(text, characterToStagePosition[text]);
		}
		InkBinding inkBinding = Singleton<InkController>.Instance.FinishedLoadingIn();
		this.dialogueTagManager.SetBinding(inkBinding);
		this._stageManager.SetupStageFromSave(dictionary);
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x000428B8 File Offset: 0x00040AB8
	private IEnumerator RefreshViewInternal(bool goForward = true, bool delayInkContinue = false)
	{
		yield return new WaitUntil(() => !this.pauseCoroutine);
		this.AnimateOutChoices();
		if (delayInkContinue)
		{
			yield return new WaitForSeconds(1f);
		}
		this.dialogBox.ToggleNextArrow(false);
		this.SetFullScreenbuttonState(false);
		if (Save.GetSaveData(false).newGamePlus && Services.GameSettings.GetString("skipText") == "true")
		{
			while (Singleton<InkController>.Instance.CanContinue() && goForward)
			{
				Singleton<InkController>.Instance.Continue();
				if (Singleton<InkController>.Instance.ShouldGoHome())
				{
					UISpeaker[] array = this.speakerPortraits;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].StartExit();
					}
					Singleton<GameController>.Instance.ReturnToHouse(this.currentDatable);
					yield break;
				}
			}
		}
		else if (Singleton<InkController>.Instance.CanContinue() && goForward)
		{
			Singleton<InkController>.Instance.Continue();
			if (Singleton<InkController>.Instance.ShouldForceGoHomeAfterCritPath())
			{
				Singleton<GameController>.Instance.ReturnToHouse(this.currentDatable);
				yield break;
			}
		}
		else if (!Singleton<InkController>.Instance.CanContinue())
		{
			Singleton<InkController>.Instance.SetGoHome(true);
		}
		this.tempText = Singleton<InkController>.Instance.GetText();
		if (this.tempText != string.Empty)
		{
			this.text = this.tempText;
		}
		this.tagsToUse = Singleton<InkController>.Instance.GetTags();
		int num;
		bool flag = this.CheckDelayTagEndDialogue(true, out num);
		int num2;
		bool flag2 = this.CheckDelayTag(true, out num2);
		int num3;
		bool flag3 = this.CheckDelayTag(false, out num3);
		if (!flag && !flag2 && !flag3)
		{
			this.RefreshViewStep2();
		}
		else
		{
			this.dialogBox.gameObject.SetActive(false);
			if (flag)
			{
				this.isWaitingForFtb = true;
				string text = this.CheckLocationTag();
				Singleton<DayNightCycle>.Instance.FadeToBlack(text, 0.5f, 1f, 0.5f, true);
			}
			else if (flag2)
			{
				this.isWaitingForFtb = true;
				string text2 = this.CheckLocationTag();
				Singleton<DayNightCycle>.Instance.FadeToBlack(text2, 1.5f, 2f, 1f, false);
			}
			else if (flag3)
			{
				foreach (UISpeaker uispeaker in this.speakerPortraits)
				{
					if (uispeaker != null && uispeaker.character != "")
					{
						uispeaker.Hide();
					}
				}
				Singleton<DayNightCycle>.Instance.FadeToWhite();
			}
			if (this.tagsToUse.Count > 1)
			{
				int num4 = -1;
				if (flag)
				{
					num4 = num;
				}
				else if (flag2)
				{
					num4 = num2;
				}
				else if (flag3 && num3 >= 0 && num3 < num2)
				{
					num4 = num3;
				}
				if (num4 > 0)
				{
					List<ParsedTag> range = this.tagsToUse.GetRange(0, num4);
					List<ParsedTag> range2 = this.tagsToUse.GetRange(num4, this.tagsToUse.Count - num4);
					this.tagsToUse = range;
					this.ParseTag();
					this.tagsToUse = range2;
				}
			}
			this.ClearChoicesButtons();
			yield return new WaitForSeconds(4.5f);
			this.isWaitingForFtb = false;
			this.dialogBox.gameObject.SetActive(true);
			foreach (UISpeaker uispeaker2 in this.speakerPortraits)
			{
				if (uispeaker2 != null && uispeaker2.character != "")
				{
					uispeaker2.UnHide();
				}
			}
			this.ParseTag();
		}
		yield break;
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x000428D8 File Offset: 0x00040AD8
	private void RefreshViewStep2()
	{
		this.dialogBox.gameObject.SetActive(true);
		foreach (UISpeaker uispeaker in this.speakerPortraits)
		{
			if (uispeaker != null && uispeaker.character != "")
			{
				uispeaker.gameObject.SetActive(true);
			}
		}
		this.ParseTag();
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x0004293C File Offset: 0x00040B3C
	private void RefreshViewStep2_unhide()
	{
		this.dialogBox.gameObject.SetActive(true);
		foreach (UISpeaker uispeaker in this.speakerPortraits)
		{
			if (uispeaker != null && uispeaker.character != "")
			{
				uispeaker.UnHide();
			}
		}
		this.ParseTag();
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x0004299C File Offset: 0x00040B9C
	private void RefreshViewStep3AfterParseTag()
	{
		if (Singleton<InkController>.Instance.ShouldGoHome())
		{
			Singleton<GameController>.Instance.ReturnToHouse(this.currentDatable);
			return;
		}
		string[] array = this.text.Trim().Split(new string[] { "::" }, StringSplitOptions.None);
		if (this.tempText != string.Empty)
		{
			if (array.Length == 2)
			{
				this.dialogBox.UpdateDialog(array[0].Trim(), array[1].Trim());
				this._stageManager.OnPlayerSpeaking(array[0].Trim());
			}
			else
			{
				this.dialogBox.UpdateDialog(array[0].Trim());
			}
		}
		if (Singleton<InkController>.Instance.story.currentChoices.Count > 0)
		{
			this.choicesCanvasGroup.alpha = 0f;
			this.choicesVerticalGroup.enabled = true;
			this.SetFullScreenbuttonState(true);
			this.dialogBox.ToggleNextArrow(true);
			this.choicesArea.SetActive(false);
			this.ClearChoices();
			Button button2 = null;
			int num = 0;
			while (num < Singleton<InkController>.Instance.story.currentChoices.Count && num < 4)
			{
				Choice choice = Singleton<InkController>.Instance.story.currentChoices[num];
				Button button = this.CreateChoice(choice.text.Trim(), button2, choice.tags);
				this.choicesButtons.Add(button);
				button.onClick.AddListener(delegate
				{
					Singleton<MessageLogManager>.Instance.Additem(MessageType.Text, "(" + choice.text + ")", null);
				});
				button.onClick.AddListener(delegate
				{
					this.OnClickChoiceButton(choice, button, true);
				});
				UIRectResizer component = button.gameObject.GetComponent<UIRectResizer>();
				if (component != null)
				{
					component.Resize();
				}
				button.GetComponent<DoCodeAnimation>().delayMultiplier = (float)num;
				num++;
			}
			for (int i = 0; i < this.choicesButtons.Count; i++)
			{
				Navigation navigation = this.choicesButtons[i].navigation;
				navigation.selectOnUp = this.choicesButtons[Mathf.Clamp(i - 1, 0, this.choicesButtons.Count - 1)];
				navigation.selectOnLeft = this.choicesButtons[Mathf.Clamp(i - 1, 0, this.choicesButtons.Count - 1)];
				navigation.selectOnDown = this.choicesButtons[Mathf.Clamp(i + 1, 0, this.choicesButtons.Count - 1)];
				navigation.selectOnRight = this.choicesButtons[Mathf.Clamp(i + 1, 0, this.choicesButtons.Count - 1)];
				navigation.mode = Navigation.Mode.Explicit;
				this.choicesButtons[i].navigation = navigation;
			}
			if (Save.GetSaveData(false).newGamePlus && Services.GameSettings.GetString("skipText") == "true")
			{
				this.SetFullScreenbuttonState(false);
				this.choicesArea.SetActive(true);
				this.dialogBox.OnChoicesDisplayed();
				this.TryHighlightAutoSelect();
				return;
			}
		}
		else
		{
			this.SetFullScreenbuttonState(true);
			this.autoSelect = this.fullscreenContinueButton.GetComponent<Selectable>();
			this.dialogBox.ToggleNextArrow(true);
		}
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00042CFA File Offset: 0x00040EFA
	private IEnumerator SelectFirstChoiceEOF()
	{
		yield return new WaitForEndOfFrame();
		ControllerMenuUI.SetCurrentlySelected(this.choicesButtons[0].gameObject, ControllerMenuUI.Direction.Down, false, false);
		yield break;
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00042D0C File Offset: 0x00040F0C
	public void UnpauseCoroutine(string source)
	{
		if (source.ToLowerInvariant().Contains("result") || source.ToLowerInvariant().Contains("spec"))
		{
			if (!Singleton<SpecStatMain>.Instance.visible || !ResultSplashScreen.Instance.isOpen)
			{
				this.pauseTagExecution = false;
				this.pauseCoroutine = false;
				return;
			}
		}
		else
		{
			this.pauseTagExecution = false;
			this.pauseCoroutine = false;
		}
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00042D74 File Offset: 0x00040F74
	private void ParseTag()
	{
		this.skipLocket = false;
		using (List<ParsedTag>.Enumerator enumerator = this.tagsToUse.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.name == "enter")
				{
					this.skipLocket = true;
				}
			}
		}
		this.autoPlayNextVo = true;
		foreach (ParsedTag parsedTag in this.tagsToUse)
		{
			if (parsedTag.name == "awakened" || parsedTag.name == "ending" || parsedTag.name == "realized" || parsedTag.name == "realize" || parsedTag.name == "realised" || parsedTag.name == "realise")
			{
				string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
				bool flag;
				if (parsedTag.name == "awakened")
				{
					if (parsedTag.properties != null && parsedTag.properties.Trim().Split(' ', StringSplitOptions.None).Length != 0)
					{
						text = parsedTag.properties.Trim().Split(' ', StringSplitOptions.None)[0].Trim();
					}
					string text2 = Path.Combine(new string[] { "Audio", "Sfx", "VoiceOver", "awaken", text });
					AudioClip audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text2, false);
					flag = true;
					if (audioClip != null && audioClip.length > 0f)
					{
						float length = audioClip.length;
						flag = true;
					}
				}
				else if (parsedTag.name == "ending")
				{
					this.autoPlayNextVo = false;
					RelationshipStatus relationshipStatus = DateADex.ParseRelationshipStatus(parsedTag.properties.Trim().Split(' ', StringSplitOptions.None)[0].Trim());
					if (parsedTag.properties != null && parsedTag.properties.Trim().Split(' ', StringSplitOptions.None).Length > 1)
					{
						text = parsedTag.properties.Trim().Split(' ', StringSplitOptions.None)[1].Trim();
					}
					string text3 = "";
					if (relationshipStatus == RelationshipStatus.Hate)
					{
						text3 = Path.Combine(new string[] { "Audio", "Sfx", "VoiceOver", "hate", text });
					}
					else if (relationshipStatus == RelationshipStatus.Love)
					{
						text3 = Path.Combine(new string[] { "Audio", "Sfx", "VoiceOver", "love", text });
					}
					else if (relationshipStatus == RelationshipStatus.Friend)
					{
						text3 = Path.Combine(new string[] { "Audio", "Sfx", "VoiceOver", "friend", text });
					}
					AudioClip audioClip2 = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text3, false);
					flag = true;
					if (audioClip2 != null && audioClip2.length > 0f)
					{
						float length2 = audioClip2.length;
						flag = true;
					}
				}
				else
				{
					this.autoPlayNextVo = false;
					if (parsedTag.properties != null && parsedTag.properties.Trim().Split(' ', StringSplitOptions.None).Length != 0)
					{
						text = parsedTag.properties.Trim().Split(' ', StringSplitOptions.None)[0].Trim();
					}
					string text4 = Path.Combine(new string[] { "Audio", "Sfx", "VoiceOver", "realize", text });
					AudioClip audioClip3 = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text4, false);
					flag = true;
					if (audioClip3 != null && audioClip3.length > 0f)
					{
						float length3 = audioClip3.length;
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
				CommandID commandID = new CommandID(parsedTag.name);
				if (!parsedTag.HasProperties())
				{
					string[] array = new string[0];
					InkCommand inkCommand = new InkCommand(commandID, array);
					if (!inkCommand.IsEmpty)
					{
						Singleton<InkController>.Instance.binding.Invoke(inkCommand);
						break;
					}
				}
				else
				{
					string[] array2 = parsedTag.properties.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					if (array2.Count<string>() <= 0)
					{
						break;
					}
					IEnumerable<string> enumerable = array2.Select((string p) => p.Trim().ToLowerInvariant());
					InkCommand inkCommand2 = new InkCommand(commandID, enumerable);
					if (!inkCommand2.IsEmpty)
					{
						Singleton<InkController>.Instance.binding.Invoke(inkCommand2);
						break;
					}
				}
			}
		}
		base.StartCoroutine(this.ExecuteTags());
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00043250 File Offset: 0x00041450
	public bool AreCharactersAnimating()
	{
		bool flag = false;
		UISpeaker[] array = this.speakerPortraits;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isAnimating)
			{
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00043284 File Offset: 0x00041484
	private string CheckLocationTag()
	{
		foreach (ParsedTag parsedTag in this.tagsToUse)
		{
			if (parsedTag.name == "location_hidden")
			{
				return parsedTag.properties;
			}
		}
		return null;
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x000432F0 File Offset: 0x000414F0
	private bool CheckDelayTagEndDialogue(bool isFtb, out int index)
	{
		index = -1;
		int num = 0;
		foreach (ParsedTag parsedTag in this.tagsToUse)
		{
			if (parsedTag.name == "ftbend" && isFtb)
			{
				index = num;
				return true;
			}
			if (parsedTag.name == "ftw" && !isFtb)
			{
				index = num;
				return true;
			}
			num++;
		}
		return false;
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00043380 File Offset: 0x00041580
	private bool CheckDelayTag(bool isFtb, out int index)
	{
		index = -1;
		int num = 0;
		foreach (ParsedTag parsedTag in this.tagsToUse)
		{
			if (parsedTag.name == "ftb" && isFtb)
			{
				index = num;
				return true;
			}
			if (parsedTag.name == "ftw" && !isFtb)
			{
				index = num;
				return true;
			}
			num++;
		}
		return false;
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00043410 File Offset: 0x00041610
	private IEnumerator ExecuteTags()
	{
		foreach (ParsedTag tag in this.tagsToUse)
		{
			if (tag.name == "audio_name" && this.justOpened)
			{
				yield return new WaitForSeconds(0.7f);
				this.justOpened = false;
			}
			if (!this.autoPlayNextVo && tag.name == "audio_name")
			{
				this.nextAudioVo = tag;
			}
			if (tag.name == "awakened" || tag.name == "ending" || tag.name == "realized" || tag.name == "realize" || tag.name == "realised" || tag.name == "realise")
			{
				this.pauseTagExecution = true;
				yield return new WaitUntil(() => !this.pauseTagExecution);
			}
			else if (tag.name != "open_locket" || (!this.skipLocket && tag.name == "open_locket"))
			{
				CommandID commandID = new CommandID(tag.name);
				if (commandID == "home")
				{
					Singleton<InkController>.Instance.forceGoHome = true;
				}
				else if (!tag.HasProperties())
				{
					string[] array = new string[0];
					InkCommand command = new InkCommand(commandID, array);
					if (command.IsEmpty)
					{
						continue;
					}
					if (tag.name.Contains("reggie"))
					{
						yield return new WaitUntil(() => !this.pauseTagExecution);
					}
					Singleton<InkController>.Instance.binding.Invoke(command);
					if (tag.name.Contains("specs"))
					{
						this.pauseTagExecution = true;
						yield return new WaitUntil(() => !this.pauseTagExecution);
					}
					command = null;
				}
				else
				{
					string[] array2 = tag.properties.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					if (array2.Count<string>() > 0)
					{
						IEnumerable<string> enumerable = array2.Select((string p) => p.Trim().ToLowerInvariant());
						InkCommand command = new InkCommand(commandID, enumerable);
						if (command.IsEmpty)
						{
							continue;
						}
						if (tag.name.Contains("reggie"))
						{
							yield return new WaitUntil(() => !this.pauseTagExecution);
						}
						Singleton<InkController>.Instance.binding.Invoke(command);
						if (tag.name.Contains("specs"))
						{
							this.pauseTagExecution = true;
							yield return new WaitUntil(() => !this.pauseTagExecution);
						}
						command = null;
					}
				}
			}
			tag = default(ParsedTag);
		}
		List<ParsedTag>.Enumerator enumerator = default(List<ParsedTag>.Enumerator);
		this.RefreshViewStep3AfterParseTag();
		yield break;
		yield break;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0004341F File Offset: 0x0004161F
	private void OnClickChoiceButton(Choice choice, Button btn)
	{
		this.OnClickChoiceButton(choice, btn, false);
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x0004342C File Offset: 0x0004162C
	private void OnClickChoiceButton(Choice choice, Button btn, bool delayInkContinue = false)
	{
		if (this.choicesButtons == null || this.choicesButtons.Count == 0)
		{
			return;
		}
		if (this.AreAnyChoicesAnimating())
		{
			return;
		}
		this._lastClickedBtn = btn;
		this.SavePrevChoice(false);
		Singleton<Analytics>.Instance.AddPlayerChoice(choice.sourcePath);
		Singleton<Analytics>.Instance.AddPlayerChoice(choice.text);
		Singleton<InkController>.Instance.ChooseChoiceIndex(choice.index);
		if (Singleton<InkController>.Instance.story.currentChoices.Count == 0)
		{
			string text = Singleton<InkController>.Instance.ContinueStory().Trim();
			if (text.ToLowerInvariant() != "continue")
			{
				text.ToLowerInvariant() != "";
			}
		}
		base.StartCoroutine(this.RefreshView(true, delayInkContinue));
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x000434F0 File Offset: 0x000416F0
	private Button CreateChoice(string text, Button prevButton, [CanBeNull] List<string> tags)
	{
		Button button = global::UnityEngine.Object.Instantiate<Button>(this.choicePrefab);
		button.transform.SetParent(this.choicesScroll.gameObject.transform, false);
		bool flag = true;
		DialogueButton component = button.GetComponent<DialogueButton>();
		if (tags != null)
		{
			if (tags[0].Contains("StatCheck"))
			{
				string[] array = tags[0].Split(" ", StringSplitOptions.None);
				string text2 = array[1];
				int num = int.Parse(array[2]);
				bool flag2 = num >= 0;
				if (!flag2)
				{
					num = -num;
				}
				int num2 = int.Parse(Singleton<InkController>.Instance.story.variablesState[text2].ToString());
				int num3;
				if (Singleton<DeluxeEditionController>.Instance != null && Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && int.TryParse(Singleton<InkController>.Instance.story.variablesState[text2 + "_dlc"].ToString(), out num3))
				{
					num2 += num3;
				}
				char[] array2 = text2.ToCharArray();
				array2[0] = char.ToUpper(array2[0]);
				text2 = new string(array2);
				flag = (flag2 ? (num2 >= num) : (num2 < num));
				UIButtonSoundEvent component2 = button.GetComponent<UIButtonSoundEvent>();
				if (flag2)
				{
					if (flag)
					{
						text = string.Format("[{0} {1}] ", text2, num) + text;
						component2.dialogueOptionSpecCheck = true;
						component.triggerSpecAnimation = true;
						component.SetStatAnimationIconSprites(text2);
					}
					else
					{
						text = string.Format("[{0} {1}/{2}] ", text2, num2, num) + text;
						component2.dialogueOptionSpecCheckInactive = true;
					}
				}
				else if (!flag)
				{
					button.gameObject.SetActive(false);
				}
			}
			else if (tags[0].ToLowerInvariant().Contains("recipe_check"))
			{
				string text3 = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
				string text4 = tags[0].ToLowerInvariant().Replace("recipe_check", "").Trim();
				if (tags[0].Trim().Contains(" "))
				{
					text3 = tags[0].Trim().Split(' ', StringSplitOptions.None)[1].Trim();
				}
				if (text3 == "cf")
				{
					text3 = "celia";
				}
				else if (text3 == "clarence")
				{
					text3 = "dirk";
				}
				bool flag3;
				if (text4.Contains('_'))
				{
					flag3 = DateADex.Instance.RecipeCheckCustom(text3);
				}
				else
				{
					flag3 = DateADex.Instance.RecipeCheck(text3);
				}
				if (!flag3)
				{
					flag = false;
				}
			}
		}
		button.interactable = flag;
		if (!flag)
		{
			Transform child = button.transform.GetChild(2);
			child.gameObject.SetActive(true);
			button.image = child.GetComponent<Image>();
		}
		TextMeshProUGUI componentInChildren = button.GetComponentInChildren<TextMeshProUGUI>();
		componentInChildren.text = text;
		componentInChildren.color = (flag ? componentInChildren.color : Color.grey);
		component.UpdateCandyIcon(text);
		componentInChildren.ForceMeshUpdate(true, false);
		RectTransform component3 = button.GetComponent<RectTransform>();
		if (prevButton == null)
		{
			component3.anchoredPosition = new Vector2(component3.anchoredPosition.x, 0f);
		}
		else
		{
			RectTransform component4 = prevButton.GetComponent<RectTransform>();
			TextMeshProUGUI componentInChildren2 = component4.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren2.ForceMeshUpdate(true, false);
			int num4 = componentInChildren2.textInfo.lineCount * 40;
			if (num4 < 50)
			{
				num4 = 50;
			}
			component3.anchoredPosition = new Vector2(component3.anchoredPosition.x, component4.anchoredPosition.y - (float)num4);
		}
		return button;
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00043874 File Offset: 0x00041A74
	private void AnimateOutChoices()
	{
		if (!this.choicesVerticalGroup.enabled && this.choicesButtons != null)
		{
			int count = this.choicesButtons.Count;
		}
		if (this.AreAnyChoicesAnimating())
		{
			return;
		}
		this.choicesVerticalGroup.enabled = false;
		for (int i = this.choicesButtons.Count - 1; i >= 0; i--)
		{
			if (this.choicesButtons[i])
			{
				DoCodeAnimation component = this.choicesButtons[i].gameObject.GetComponent<DoCodeAnimation>();
				component.delayMultiplier = (float)i;
				if (this.choicesButtons[i] == this._lastClickedBtn)
				{
					component.delayMultiplier = 8f;
				}
				component.destroyOnFinish = true;
				component.TriggerAnimationAtIndex(1);
				this.choicesButtons[i].onClick.RemoveAllListeners();
			}
		}
		this.choicesButtons = new List<Button>();
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0004395C File Offset: 0x00041B5C
	public void ClearChoicesButtons()
	{
		for (int i = this.choicesButtons.Count - 1; i >= 0; i--)
		{
			this.choicesButtons[i].onClick.RemoveAllListeners();
		}
		this.choicesButtons = new List<Button>();
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x000439A4 File Offset: 0x00041BA4
	private void ClearChoices()
	{
		foreach (object obj in this.choicesArea.transform)
		{
			Transform transform = (Transform)obj;
			Button[] components = transform.GetComponents<Button>();
			if (components != null)
			{
				int i = 0;
				int num = components.Length;
				while (i < num)
				{
					components[i].onClick.RemoveAllListeners();
					i++;
				}
			}
			global::UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00043A34 File Offset: 0x00041C34
	private bool IsTalkingUIAnimatorFinished()
	{
		return this.VnAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00043A60 File Offset: 0x00041C60
	public void ContinuePressed()
	{
		if (!this.IsTalkingUIAnimatorFinished())
		{
			return;
		}
		if (this.IsRefreshViewRunning)
		{
			return;
		}
		if (this.isWaitingForFtb)
		{
			return;
		}
		if (this.awakenScreen == null)
		{
			this.awakenScreen = global::UnityEngine.Object.FindObjectOfType<AwakenSplashScreen>(true);
		}
		if (this.awakenScreen != null && this.awakenScreen.isOpen)
		{
			return;
		}
		bool flag = Singleton<InkController>.Instance.story.currentChoices.Count > 0 && !this.choicesArea.gameObject.activeSelf;
		if (this.dialogBox.TextAnimating)
		{
			if (!Singleton<GameController>.Instance.isUnskippable)
			{
				this.dialogBox.SkipTextAnimation();
				this.SetFullScreenbuttonState(true);
				return;
			}
		}
		else if (flag)
		{
			if (this.AreAnyChoicesAnimating())
			{
				return;
			}
			this.SetFullScreenbuttonState(false);
			this.choicesArea.SetActive(true);
			base.StartCoroutine(this.DelayedChoicesfadeIn());
			this.dialogBox.OnChoicesDisplayed();
			this.TryHighlightAutoSelect();
			return;
		}
		else
		{
			this.SavePrevChoice(true);
			base.StartCoroutine(this.RefreshView(true, false));
		}
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x00043B6C File Offset: 0x00041D6C
	private IEnumerator DelayedChoicesfadeIn()
	{
		yield return null;
		yield return null;
		this.choicesCanvasGroup.DOFade(1f, 0.5f);
		yield break;
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00043B7B File Offset: 0x00041D7B
	// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x00043B83 File Offset: 0x00041D83
	public bool IsRefreshViewRunning { get; private set; }

	// Token: 0x06000BAA RID: 2986 RVA: 0x00043B8C File Offset: 0x00041D8C
	private IEnumerator RefreshView(bool goForward = true, bool delayInkContinue = false)
	{
		if (this.IsRefreshViewRunning)
		{
			T17Debug.LogError("'RefreshView' called when already running. This shouldn't happen, but not returning incase something excepted this to work.");
		}
		this.IsRefreshViewRunning = true;
		yield return this.RefreshViewInternal(goForward, delayInkContinue);
		this.IsRefreshViewRunning = false;
		yield break;
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00043BA9 File Offset: 0x00041DA9
	private void Update()
	{
		this.ProcessInput();
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00043BB4 File Offset: 0x00041DB4
	private void ProcessInput()
	{
		bool flag = !Services.InputService.IsLastActiveInputController() || !Singleton<InkController>.Instance.AllowQuickResponseChoices;
		bool buttonDown = this.player.GetButtonDown("Confirm");
		bool flag2 = Singleton<ControllerMenuUI>.Instance.IsUIInteractionEnabled();
		int priorityOfCurrentSelected = Singleton<ControllerMenuUI>.Instance.PriorityOfCurrentSelected;
		if (flag2 && buttonDown && priorityOfCurrentSelected <= 0 && flag && this.autoSelect == this.fullscreenContinueButton.GetComponent<Selectable>() && !Singleton<PhoneManager>.Instance.SubMenuOpen && !Locket.IsLocketEnabled())
		{
			ControllerMenuUI.SetCurrentlySelected(this.autoSelect.gameObject, ControllerMenuUI.Direction.Down, false, false);
		}
		if (flag && this.player.GetButtonDown("UICancel"))
		{
			this.TryHighlightAutoSelect();
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00043C6C File Offset: 0x00041E6C
	private void TryHighlightAutoSelect()
	{
		bool flag = Services.InputService.IsLastActiveInputController();
		if (this.choicesArea.activeInHierarchy && this.choicesButtons.Count > 0)
		{
			using (List<Button>.Enumerator enumerator = this.choicesButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Button button = enumerator.Current;
					if (button && button.gameObject.activeInHierarchy)
					{
						this.autoSelect = button;
						ControllerMenuUI.SetCurrentlySelected(button.gameObject, ControllerMenuUI.Direction.Down, false, !flag);
						break;
					}
				}
				return;
			}
		}
		if (this.autoSelect != null)
		{
			ControllerMenuUI.SetCurrentlySelected(this.autoSelect.gameObject, ControllerMenuUI.Direction.Down, false, !flag);
			return;
		}
		if (this.fullscreenContinueButton.activeInHierarchy)
		{
			this.autoSelect = this.fullscreenContinueButton.GetComponent<Selectable>();
			ControllerMenuUI.SetCurrentlySelected(this.autoSelect.gameObject, ControllerMenuUI.Direction.Down, false, !flag);
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00043D68 File Offset: 0x00041F68
	private void SavePrevChoice(bool lcc)
	{
		Singleton<InkController>.Instance.SavePrevChoice(lcc);
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00043D78 File Offset: 0x00041F78
	public void GetLastChoice()
	{
		PrevChoice prevChoice;
		if (Singleton<InkController>.Instance.GetLastChoice(out prevChoice))
		{
			Singleton<InkController>.Instance.SetState(prevChoice);
			if (Singleton<InkController>.Instance.PreviousChoiceCount() == 0)
			{
				this.backButton.SetActive(false);
			}
			base.StartCoroutine(this.RefreshView(!prevChoice.lastChoiceContinue, false));
			return;
		}
		this.backButton.SetActive(false);
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00043DDA File Offset: 0x00041FDA
	public void EnterTalkingUI()
	{
		if (!this.locationOverride)
		{
			this.DialogueCamera = Singleton<CameraSpaces>.Instance.PlayerZone().DialogueCamera;
			this.DialogueCamera.Priority = 99;
		}
		this.playerModel.SetActive(false);
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x00043E14 File Offset: 0x00042014
	public void ChangeCameraByRoom(string room)
	{
		this.locationOverride = true;
		if (this.DialogueCamera != null)
		{
			this.DialogueCamera.Priority = 10;
		}
		this.DialogueCamera = Singleton<CameraSpaces>.Instance.GetCameraByRoom(room).DialogueCamera;
		this.DialogueCamera.Priority = 99;
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00043E68 File Offset: 0x00042068
	public void ChangeCameraByListedNearest(List<string> rooms)
	{
		this.locationOverride = true;
		string text = null;
		float num = 9999f;
		float num2 = -1f;
		foreach (string text2 in rooms)
		{
			CinemachineVirtualCamera dialogueCamera = Singleton<CameraSpaces>.Instance.GetCameraByRoom(text2).DialogueCamera;
			float num3 = Vector3.Distance(this.playerCamera.transform.position, dialogueCamera.transform.position);
			float num4 = Vector3.Dot(this.playerCamera.transform.forward, dialogueCamera.transform.forward);
			bool flag = false;
			if (num3 <= 25f)
			{
				RaycastHit raycastHit;
				flag = Physics.Raycast(this.playerCamera.transform.position, dialogueCamera.transform.position - this.playerCamera.transform.position, out raycastHit, 25f);
				if (raycastHit.transform != null)
				{
					InteractableObj interactableObj;
					raycastHit.transform.TryGetComponent<InteractableObj>(out interactableObj);
					flag = interactableObj != null;
				}
			}
			if (num4 >= num2)
			{
				num2 = num4;
			}
			if (num3 < num)
			{
				num = num3;
				text = text2;
			}
			if (num3 <= num && num4 > -0.5f && flag)
			{
				text = text2;
				num = num3;
			}
			if (num3 < 5f && num > num3 && num4 >= num2)
			{
				text = text2;
				num = num3;
				num2 = num4;
			}
		}
		if (text == null)
		{
			foreach (string text3 in rooms)
			{
				CinemachineVirtualCamera dialogueCamera2 = Singleton<CameraSpaces>.Instance.GetCameraByRoom(text3).DialogueCamera;
				float num5 = Vector3.Distance(this.playerCamera.transform.position, dialogueCamera2.transform.position);
				Vector3.Dot(this.playerCamera.transform.forward, dialogueCamera2.transform.forward);
				if (Mathf.Abs(num5) < num)
				{
					text = text3;
					num = num5;
				}
			}
		}
		if (this.DialogueCamera != null)
		{
			this.DialogueCamera.Priority = 10;
		}
		if (text != null)
		{
			this.DialogueCamera = Singleton<CameraSpaces>.Instance.GetCameraByRoom(text).DialogueCamera;
			this.DialogueCamera.Priority = 99;
			return;
		}
		this.DialogueCamera = Singleton<CameraSpaces>.Instance.PlayerZone().DialogueCamera;
		this.DialogueCamera.Priority = 99;
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x0004410C File Offset: 0x0004230C
	public void AddRoomer()
	{
		if (Singleton<Save>.Instance.GetDateStatus("maggie_mglass") == RelationshipStatus.Unmet)
		{
			return;
		}
		global::UnityEngine.Object.Instantiate<GameObject>(this.newRoomerObj, this.newRoomerObj.transform.parent).SetActive(true);
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00044141 File Offset: 0x00042341
	private void SetRoomerInactive()
	{
		this.newRoomerObj.SetActive(false);
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x0004414F File Offset: 0x0004234F
	public void AddDexEntry()
	{
		this.newDexEntryObj.SetActive(true);
		base.Invoke("SetDexEntryInactive", 10f);
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x0004416D File Offset: 0x0004236D
	private void SetDexEntryInactive()
	{
		this.newDexEntryObj.SetActive(false);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0004417B File Offset: 0x0004237B
	public void ShowSpotlight()
	{
		this.spotlightAnimator.gameObject.SetActive(true);
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0004418E File Offset: 0x0004238E
	public void HideSpotlight()
	{
		this.spotlightAnimator.SetTrigger("EndSpotlight");
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x000441A0 File Offset: 0x000423A0
	public void ExitTalkingUI()
	{
		Singleton<GameController>.Instance.isUnskippable = false;
		Singleton<GameController>.Instance.isSubtitle = false;
		if (this.DialogueCamera != null)
		{
			this.DialogueCamera.Priority = 10;
			this.DialogueCamera = null;
		}
		this.locationOverride = false;
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x000441EC File Offset: 0x000423EC
	public void CloseTalkingUI()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x000441FA File Offset: 0x000423FA
	public void ShowEmote(string characterName, EmoteReaction emoteReaction, EmoteHeight height)
	{
		if (this._stageManager != null)
		{
			this._stageManager.ShowEmote(characterName, emoteReaction, height);
		}
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00044212 File Offset: 0x00042412
	public void HideEmote(string characterName, EmoteReaction emote)
	{
		if (this._stageManager != null)
		{
			this._stageManager.HideEmote(characterName, emote);
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00044229 File Offset: 0x00042429
	public void HideEmotes(string characterName)
	{
		if (this._stageManager != null)
		{
			this._stageManager.HideEmotes(characterName);
		}
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0004423F File Offset: 0x0004243F
	private void SetFullScreenbuttonState(bool enableButton)
	{
		this.fullscreenContinueButton.SetActive(enableButton);
		if (enableButton)
		{
			ControllerMenuUI.SetCurrentlySelected(this.fullscreenContinueButton, ControllerMenuUI.Direction.Down, false, false);
		}
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x00044260 File Offset: 0x00042460
	private bool AreAnyChoicesAnimating()
	{
		if (this.choicesButtons != null)
		{
			int i = 0;
			int count = this.choicesButtons.Count;
			while (i < count)
			{
				if (this.choicesButtons[i] != null)
				{
					DoCodeAnimation component = this.choicesButtons[i].GetComponent<DoCodeAnimation>();
					if (component != null && component.IsAnimating)
					{
						return true;
					}
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x04000A52 RID: 2642
	public static TalkingUI Instance;

	// Token: 0x04000A53 RID: 2643
	[Header("Prefabs")]
	[FormerlySerializedAs("buttonPrefab")]
	[SerializeField]
	private Button choicePrefab;

	// Token: 0x04000A54 RID: 2644
	[Header("UI Elements")]
	[SerializeField]
	private DialogBoxBehavior dialogBox;

	// Token: 0x04000A55 RID: 2645
	public ScrollRect choicesScroll;

	// Token: 0x04000A56 RID: 2646
	public CanvasGroup choicesCanvasGroup;

	// Token: 0x04000A57 RID: 2647
	public GameObject backButton;

	// Token: 0x04000A58 RID: 2648
	public GameObject fullscreenContinueButton;

	// Token: 0x04000A59 RID: 2649
	public GameObject choicesArea;

	// Token: 0x04000A5A RID: 2650
	public GameObject newRoomerObj;

	// Token: 0x04000A5B RID: 2651
	public GameObject newDexEntryObj;

	// Token: 0x04000A5C RID: 2652
	[SerializeField]
	private VerticalLayoutGroup choicesVerticalGroup;

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	private Animator spotlightAnimator;

	// Token: 0x04000A5E RID: 2654
	[SerializeField]
	private Animator VnAnim;

	// Token: 0x04000A5F RID: 2655
	[SerializeField]
	private Image bgImg;

	// Token: 0x04000A60 RID: 2656
	[SerializeField]
	private GameObject foregroundImage;

	// Token: 0x04000A61 RID: 2657
	[Header("Speaker Elements")]
	public UISpeaker[] speakerPortraits;

	// Token: 0x04000A62 RID: 2658
	public Transform speakerPositions;

	// Token: 0x04000A63 RID: 2659
	private Dictionary<string, SpeakingCharacter> currentSpeakers;

	// Token: 0x04000A64 RID: 2660
	private List<PrevChoice> previousChoices = new List<PrevChoice>();

	// Token: 0x04000A65 RID: 2661
	private InteractableObj currentDatable;

	// Token: 0x04000A66 RID: 2662
	private string currentSpeakerName;

	// Token: 0x04000A67 RID: 2663
	private List<Button> choicesButtons = new List<Button>();

	// Token: 0x04000A68 RID: 2664
	private string text = "";

	// Token: 0x04000A69 RID: 2665
	private bool locationOverride;

	// Token: 0x04000A6A RID: 2666
	private bool isWaitingForFtb;

	// Token: 0x04000A6B RID: 2667
	[Header("Debug")]
	public bool open;

	// Token: 0x04000A6C RID: 2668
	public bool justOpened;

	// Token: 0x04000A6D RID: 2669
	public Image datablePrevTextBG;

	// Token: 0x04000A6E RID: 2670
	public TextMeshProUGUI datablePrevText;

	// Token: 0x04000A6F RID: 2671
	[Header("Public variables")]
	public CinemachineVirtualCamera DialogueCamera;

	// Token: 0x04000A70 RID: 2672
	public GameObject playerModel;

	// Token: 0x04000A71 RID: 2673
	public GameObject playerCamera;

	// Token: 0x04000A72 RID: 2674
	private Player player;

	// Token: 0x04000A73 RID: 2675
	private StageManager _stageManager;

	// Token: 0x04000A74 RID: 2676
	private DialogueTagManager dialogueTagManager;

	// Token: 0x04000A75 RID: 2677
	private Selectable autoSelect;

	// Token: 0x04000A76 RID: 2678
	[SerializeField]
	private AwakenSplashScreen awakenScreen;

	// Token: 0x04000A77 RID: 2679
	private InputModeHandle inputModeHandle;

	// Token: 0x04000A78 RID: 2680
	private string tempText;

	// Token: 0x04000A79 RID: 2681
	private List<ParsedTag> tagsToUse;

	// Token: 0x04000A7A RID: 2682
	private bool skipLocket;

	// Token: 0x04000A7B RID: 2683
	public bool autoPlayNextVo = true;

	// Token: 0x04000A7C RID: 2684
	public ParsedTag nextAudioVo;

	// Token: 0x04000A7D RID: 2685
	private bool pauseCoroutine;

	// Token: 0x04000A7E RID: 2686
	public bool pauseTagExecution;

	// Token: 0x04000A7F RID: 2687
	private Button _lastClickedBtn;
}
