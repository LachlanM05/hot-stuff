using System;
using System.Collections.Generic;
using System.IO;
using Ink.Runtime;
using Team17.Common;
using Team17.Scripts.UI_Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000155 RID: 341
public class ParallelChat : MonoBehaviour
{
	// Token: 0x06000CA8 RID: 3240 RVA: 0x00049228 File Offset: 0x00047428
	public void enddialogue()
	{
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0004922A File Offset: 0x0004742A
	public void TrySelectChatButton()
	{
		if (Singleton<ChatMaster>.Instance == null)
		{
			return;
		}
		Singleton<ChatMaster>.Instance.SelectChatButton(this);
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x00049248 File Offset: 0x00047448
	private void DisplayText(string dialogue, string sender, string reaction, bool ignoreComputer = false)
	{
		if (!ignoreComputer && Singleton<ChatMaster>.Instance.ciu.isActiveAndEnabled)
		{
			Singleton<ChatMaster>.Instance.ciu.UpdateInfo();
		}
		if (!(dialogue == ""))
		{
			global::UnityEngine.Object.Instantiate<ChatTextBox>((sender == "Player") ? this.youSentChatboxPrefab : this.theySentChatboxPrefab, this.Chatbox).Init(dialogue, Singleton<InkController>.Instance.story.currentFlowName, this.GetReaction(reaction.ToLowerInvariant()));
		}
		this.ForceScrollToEnd();
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x000492D4 File Offset: 0x000474D4
	public void ForceScrollToEnd()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.pendingForceScrollToEnd = true;
			return;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		LayoutGroup[] componentsInParent = this.Chatbox.GetComponentsInParent<LayoutGroup>();
		for (int i = 0; i < componentsInParent.Length; i++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)componentsInParent[i].transform);
		}
		this.screct.normalizedPosition = new Vector2(0f, 0f);
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0004934C File Offset: 0x0004754C
	public void ShowChat()
	{
		this.appMessage.MoveNextToHistory();
		string currentFlowName = Singleton<InkController>.Instance.story.currentFlowName;
		Singleton<InkController>.Instance.story.SwitchFlow(this.appMessage.GetLastStitch());
		Singleton<InkController>.Instance.story.ChoosePathString(this.appMessage.GetLastStitch(), true, Array.Empty<object>());
		Singleton<InkController>.Instance.ContinueStory();
		base.gameObject.SetActive(true);
		if (this.appMessage.NextStitches.Count > 0)
		{
			this.appMessage.NextStitches.RemoveAt(0);
		}
		this.GetText(true, true, false);
		this.showbuttons(false, true, false, false);
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x00049400 File Offset: 0x00047600
	public bool GetText(bool ignoreComputer = false, bool updateSaveData = true, bool isLoadingGame = false)
	{
		bool flag = false;
		string text = Singleton<InkController>.Instance.story.currentText;
		string currentFlowName = Singleton<InkController>.Instance.story.currentFlowName;
		for (int i = 0; i < this.Chatbox.childCount; i++)
		{
			ChatTextBox component = this.Chatbox.GetChild(i).GetComponent<ChatTextBox>();
			if (component != null && text.Equals(component.Dialogue.text, StringComparison.Ordinal) && component.InkFlowName == currentFlowName)
			{
				return false;
			}
		}
		while (text.StartsWith("false") && Singleton<InkController>.Instance.story.canContinue)
		{
			Singleton<InkController>.Instance.ContinueStory();
			text = Singleton<InkController>.Instance.story.currentText;
		}
		List<string> currentTags = Singleton<InkController>.Instance.story.currentTags;
		string text2 = "null";
		foreach (string text3 in currentTags)
		{
			if (text3.Contains("Reaction"))
			{
				text2 = text3.Split(": ", StringSplitOptions.None)[1];
			}
			else if (text3.StartsWith("audio_name") && updateSaveData && !this.isWaitingForNextAutomaticText)
			{
				string text4 = text3.Substring(text3.IndexOf(" ") + 1);
				string text5 = Path.Combine("english", "apps", text4);
				string nodePrefix = this.appMessage.NodePrefix;
				if ((Singleton<ChatMaster>.Instance.activewindow == ChatType.Canopy && Singleton<ChatMaster>.Instance.ActiveChatCanopy != null && Singleton<ChatMaster>.Instance.ActiveChatCanopy.appMessage.NodePrefix == nodePrefix) || (Singleton<ChatMaster>.Instance.activewindow == ChatType.Wrkspce && Singleton<ChatMaster>.Instance.ActiveChatWorkspace != null && Singleton<ChatMaster>.Instance.ActiveChatWorkspace.appMessage.NodePrefix == nodePrefix) || (Singleton<ChatMaster>.Instance.activewindow == ChatType.Thiscord && Singleton<ChatMaster>.Instance.ActiveChatThiscord != null && Singleton<ChatMaster>.Instance.ActiveChatThiscord.appMessage.NodePrefix == nodePrefix))
				{
					float num = Singleton<AudioManager>.Instance.PlayTrack(text5, AUDIO_TYPE.DIALOGUE, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
					if (num > 0f)
					{
						flag = true;
						this.isWaitingForNextAutomaticText = true;
						this.ScheduleProcessNextOption(num, 0.3f);
					}
				}
				else if (!Singleton<ChatMaster>.Instance.AudioVOQueue.ContainsKey(nodePrefix))
				{
					Singleton<ChatMaster>.Instance.AudioVOQueue.Add(nodePrefix, text5);
				}
			}
			else if (text3.StartsWith("anime") && !isLoadingGame)
			{
				MovingDateable.MoveDateable("MovingAnimeFigure", "visible", true);
			}
			else if (text3.StartsWith("recipes_unlocked") && !isLoadingGame)
			{
				Singleton<Save>.Instance.SetInteractableState("DateADexRecipe", true);
			}
			else if (text3.StartsWith("drones") && !isLoadingGame)
			{
				MovingDateable.MoveDateable("MovingDrones", "level" + text3.Replace("drones", "").Trim(), true);
			}
			else if (text3.StartsWith("helicopter_fly") && !isLoadingGame)
			{
				Singleton<TutorialController>.Instance.HideCars();
				global::UnityEngine.Object.FindObjectOfType<FrontDoor>().SwapToHelicopterAnim();
				MovingDateable.MoveDateable("MovingHelicopter", "flying", true);
			}
			else if (text3.StartsWith("yacht") && !isLoadingGame)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.yachtDelivery, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				MovingDateable.MoveDateable("MovingYacht", "visible", true);
			}
		}
		string text6 = text;
		string text7 = "";
		if (text.Contains(":: "))
		{
			text6 = text.Split(":: ", StringSplitOptions.None)[1];
			text7 = text.Split(":: ", StringSplitOptions.None)[0];
		}
		if (!flag && updateSaveData && !this.isWaitingForNextAutomaticText && string.Compare(text7, "player", StringComparison.OrdinalIgnoreCase) == 0)
		{
			flag = true;
			this.isWaitingForNextAutomaticText = true;
			this.ScheduleProcessNextOption(0f, 0.3f);
		}
		this.DisplayText(text6, text7, text2, ignoreComputer);
		this.showbuttons(ignoreComputer, updateSaveData, flag, isLoadingGame);
		return flag;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0004985C File Offset: 0x00047A5C
	public void ScheduleProcessNextOption(float audioLength, float delay = 0.3f)
	{
		base.CancelInvoke();
		base.Invoke("ProcessNextOption", audioLength + 0.3f);
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x00049878 File Offset: 0x00047A78
	public void showbuttons(bool ignoreComputer = false, bool updateSaveData = true, bool alreadyHandledAutoNextText = false, bool isLoadingGame = false)
	{
		Button[] options = this.Options;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].gameObject.SetActive(false);
		}
		this.chatEndedText.gameObject.SetActive(false);
		if (this.latestChatBreakLine != null)
		{
			this.latestChatBreakLine.SetActive(true);
		}
		this.EnableNavigation(false);
		if (!Singleton<InkController>.Instance.story.currentFlowName.StartsWith(this.appMessage.NodePrefix))
		{
			Singleton<InkController>.Instance.story.SwitchFlow(this.appMessage.GetLastStitch());
		}
		List<Choice> currentChoices = Singleton<InkController>.Instance.story.currentChoices;
		if (currentChoices.Count > 0 && currentChoices.Count <= this.Options.Length && !Singleton<InkController>.Instance.story.canContinue)
		{
			this.isPendingShowChoices = true;
			this.ContinueOption(updateSaveData, alreadyHandledAutoNextText);
			return;
		}
		if (Singleton<InkController>.Instance.story.canContinue)
		{
			this.ContinueOption(updateSaveData, alreadyHandledAutoNextText);
			return;
		}
		Save.AppMessage playerAppMessageVisited = Singleton<Save>.Instance.GetPlayerAppMessageVisited(Singleton<InkController>.Instance.story.currentFlowName);
		if (Singleton<ChatMaster>.Instance.Workspace.activeInHierarchy && playerAppMessageVisited != null && playerAppMessageVisited.IsLastHistoryNewChat())
		{
			string lastHistoryNewChat = playerAppMessageVisited.GetLastHistoryNewChat();
			Singleton<InkController>.Instance.story.SwitchFlow(lastHistoryNewChat);
			Singleton<InkController>.Instance.story.ChoosePathString(lastHistoryNewChat, true, Array.Empty<object>());
			Singleton<InkController>.Instance.ContinueStory();
			this.EnableNavigation(false);
			this.chatEnded = false;
			this.appendedChat = true;
			this.lastAppendedChat = lastHistoryNewChat;
			this.ContinueOption(true, alreadyHandledAutoNextText);
			return;
		}
		if (updateSaveData)
		{
			Singleton<InkController>.Instance.story.SwitchToDefaultFlow();
			Singleton<InkController>.Instance.story.RemoveFlow(this.appMessage.GetLastStitch());
		}
		Save.AppMessageStitch lastStitchObj = this.appMessage.GetLastStitchObj();
		if (lastStitchObj != null)
		{
			lastStitchObj.Ended = true;
		}
		if (this.appMessage.NextStitches.Count > 0 && !isLoadingGame)
		{
			this.ShowChat();
			this.chatEnded = false;
			return;
		}
		if (this.appMessage.NextStitches.Count > 0 && isLoadingGame)
		{
			this.chatEnded = false;
		}
		this.chatEndedText.gameObject.SetActive(true);
		if (this.latestChatBreakLine != null)
		{
			this.latestChatBreakLine.SetActive(false);
		}
		this.chatEndedText.transform.SetAsLastSibling();
		this.ForceScrollToEnd();
		this.button.GetComponent<ChatButton>().SetNewMessages(false);
		this.LastContinueOption();
		if (!this.chatEnded)
		{
			this.chatEnded = true;
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.chatEndedText, this.chatEndedText.transform.parent);
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("", true);
			gameObject.SetActive(false);
			if (this.latestChatBreakLine != null)
			{
				this.latestChatBreakLine.SetActive(true);
			}
			this.latestChatBreakLine = gameObject;
			this.ForceScrollToEnd();
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
			ChatButton component = this.button.GetComponent<ChatButton>();
			if (!ignoreComputer && component != null)
			{
				ControllerMenuUI.SetCurrentlySelected(component.button.gameObject, ControllerMenuUI.Direction.Down, false, false);
			}
			if (this.appendedChat)
			{
				Singleton<Save>.Instance.AddFinishedMessage(this.lastAppendedChat, true);
			}
			else
			{
				Save instance = Singleton<Save>.Instance;
				Save.AppMessageStitch lastStitchObj2 = this.appMessage.GetLastStitchObj();
				instance.AddFinishedMessage((lastStitchObj2 != null) ? lastStitchObj2.Stitch : null, false);
			}
		}
		else
		{
			this.FinishLastVoLine();
		}
		this.EnableNavigation(this.chatEnded);
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x00049BFC File Offset: 0x00047DFC
	private void ShowChoices()
	{
		this.isPendingShowChoices = false;
		List<Choice> list = Singleton<InkController>.Instance.story.currentChoices;
		if (list == null || list.Count == 0)
		{
			this.continuetext(true, false);
			list = Singleton<InkController>.Instance.story.currentChoices;
		}
		int num = 0;
		while (num < list.Count && num < this.Options.Length)
		{
			this.Options[num].gameObject.SetActive(true);
			this.Options[num].GetComponentInChildren<TextMeshProUGUI>().text = list[num].text;
			this.Options[num].onClick.RemoveAllListeners();
			string currentStoryKnotChoice = list[num].pathStringOnChoice.ToString();
			this.Options[num].onClick.AddListener(delegate
			{
				this.ChooseJump(currentStoryKnotChoice, true);
			});
			Navigation navigation = this.Options[num].navigation;
			if (num == 0)
			{
				ControllerMenuUI.SetCurrentlySelected(this.Options[0].gameObject, ControllerMenuUI.Direction.Down, false, false);
			}
			else
			{
				navigation.selectOnUp = this.Options[Mathf.Clamp(num - 1, 0, list.Count - 1)];
			}
			if (num == list.Count - 1 || num == this.Options.Length - 1)
			{
				ControllerMenuUI.SetCurrentlySelected(this.Options[0].gameObject, ControllerMenuUI.Direction.Down, false, false);
			}
			else
			{
				navigation.selectOnDown = this.Options[Mathf.Clamp(num + 1, 0, list.Count - 1)];
			}
			navigation.mode = Navigation.Mode.Automatic;
			this.Options[num].navigation = navigation;
			num++;
		}
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x00049D98 File Offset: 0x00047F98
	private void LastContinueOption()
	{
		this.Options[0].gameObject.SetActive(true);
		this.Options[0].GetComponentInChildren<TextMeshProUGUI>().text = "...";
		this.Options[0].onClick.RemoveAllListeners();
		this.Options[0].onClick.AddListener(delegate
		{
			this.FinishLastVoLine();
		});
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00049E00 File Offset: 0x00048000
	private void FinishLastVoLine()
	{
		this.Options[0].gameObject.SetActive(false);
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.5f);
		Save.AppMessageStitch lastStitchObj = this.appMessage.GetLastStitchObj();
		if (lastStitchObj != null)
		{
			lastStitchObj.Ended = true;
			Singleton<Save>.Instance.AddFinishedMessage(lastStitchObj.Stitch, false);
			Singleton<Save>.Instance.CheckFinishedMessagesRules(lastStitchObj.Stitch);
		}
		this.isWaitingForNextAutomaticText = false;
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00049E70 File Offset: 0x00048070
	private void ContinueOption(bool updateSaveData = true, bool alreadyHandledAutoNextText = false)
	{
		this.Options[0].gameObject.SetActive(true);
		this.Options[0].GetComponentInChildren<TextMeshProUGUI>().text = "...";
		this.Options[0].onClick.RemoveAllListeners();
		if (this.isPendingShowChoices)
		{
			this.Options[0].onClick.AddListener(delegate
			{
				this.ProcessNextOption();
			});
		}
		else
		{
			this.Options[0].onClick.AddListener(delegate
			{
				this.continuetext(updateSaveData, true);
			});
		}
		if (!alreadyHandledAutoNextText && !this.isWaitingForNextAutomaticText)
		{
			this.isWaitingForNextAutomaticText = true;
		}
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x00049F25 File Offset: 0x00048125
	public void AutoContinueText(bool updateSaveData = true)
	{
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x00049F27 File Offset: 0x00048127
	public void continuetext()
	{
		this.continuetext(true, false);
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x00049F34 File Offset: 0x00048134
	private void ProcessNextOption()
	{
		base.CancelInvoke();
		if (this.chatEnded)
		{
			this.FinishLastVoLine();
			return;
		}
		this.isWaitingForNextAutomaticText = false;
		if (this.isPendingShowChoices)
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.5f);
			this.ShowChoices();
			this.ForceScrollToEnd();
			return;
		}
		this.continuetext(true, false);
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x00049F8C File Offset: 0x0004818C
	public void continuetext(bool updateSaveData, bool forceResetWaitForNextAutomaticText = false)
	{
		if (forceResetWaitForNextAutomaticText)
		{
			this.isWaitingForNextAutomaticText = false;
		}
		if ((this.appMessage.NodePrefix.StartsWith("canopy_work") && Singleton<ChatMaster>.Instance.ActiveChatCanopy != null && Singleton<ChatMaster>.Instance.ActiveChatCanopy.appMessage.NodePrefix == this.appMessage.NodePrefix) || (this.appMessage.NodePrefix.StartsWith("wrkspace_chat") && Singleton<ChatMaster>.Instance.ActiveChatWorkspace != null && Singleton<ChatMaster>.Instance.ActiveChatWorkspace.appMessage.NodePrefix == this.appMessage.NodePrefix) || (this.appMessage.NodePrefix.StartsWith("thiscord_phone") && Singleton<ChatMaster>.Instance.ActiveChatThiscord != null && Singleton<ChatMaster>.Instance.ActiveChatThiscord.appMessage.NodePrefix == this.appMessage.NodePrefix))
		{
			this.continuetext(-1, "", updateSaveData, false);
		}
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x0004A0A4 File Offset: 0x000482A4
	public bool continuetext(int choice = -1, string node = "", bool updateSaveData = true, bool isLoadingGame = false)
	{
		if (updateSaveData)
		{
			if (this.UpdateSaveData(node))
			{
				bool flag = choice > -1;
			}
		}
		if (Singleton<InkController>.Instance.story.currentFlowName.StartsWith(this.appMessage.NodePrefix))
		{
			if (Singleton<InkController>.Instance.story.canContinue && Singleton<InkController>.Instance.story.currentText.Trim() != "You are in your own house. Everything is alive and sexy.  What do you do?")
			{
				Singleton<InkController>.Instance.ContinueStory();
				return this.GetText(false, updateSaveData, isLoadingGame);
			}
			this.FinishLastVoLine();
			return false;
		}
		else
		{
			if (this.appMessage.StitchHistory.Count > 0)
			{
				Singleton<InkController>.Instance.story.SwitchFlow(this.appMessage.GetLastStitchObj().Stitch);
				Singleton<InkController>.Instance.story.ChoosePathString(this.appMessage.GetLastStitchObj().Stitch, true, Array.Empty<object>());
				Singleton<InkController>.Instance.ContinueStory();
				return this.GetText(false, updateSaveData, isLoadingGame);
			}
			return false;
		}
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x0004A1A3 File Offset: 0x000483A3
	private bool UpdateSaveData(string step)
	{
		return Singleton<Save>.Instance.AddPlayerAppMessageStep(this.appMessage.GetLastStitch(), step);
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0004A1BC File Offset: 0x000483BC
	private void closechat()
	{
		Singleton<InkController>.Instance.story.SwitchToDefaultFlow();
		Singleton<InkController>.Instance.story.RemoveFlow(this.appMessage.GetLastStitch());
		Singleton<ChatMaster>.Instance.deletechat(this);
		global::UnityEngine.Object.Destroy(this.button);
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0004A214 File Offset: 0x00048414
	public void ChooseJump(string knot, bool updateSaveData = true)
	{
		if (knot == null)
		{
			T17Debug.LogError("Provided path " + knot + " is null. Cannot jump to knot.");
			return;
		}
		this.isPendingShowChoices = false;
		string currentPathString = Singleton<InkController>.Instance.story.state.currentPathString;
		Singleton<InkController>.Instance.JumpToKnot(knot);
		if (Singleton<InkController>.Instance.CanContinue())
		{
			Singleton<InkController>.Instance.ContinueStory();
		}
		this.continuetext(-1, knot, updateSaveData, false);
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0004A283 File Offset: 0x00048483
	private Sprite GetReaction(string reactionFileName)
	{
		return null;
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0004A286 File Offset: 0x00048486
	public void ClearChatEndedText()
	{
		if (this.latestChatBreakLine != null)
		{
			this.latestChatBreakLine.SetActive(true);
		}
		this.chatEnded = false;
		this.chatEndedText.gameObject.SetActive(false);
		this.EnableNavigation(true);
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x0004A2C4 File Offset: 0x000484C4
	private void EnableNavigation(bool enable)
	{
		ChatButton component = this.button.GetComponent<ChatButton>();
		if (component != null)
		{
			Navigation navigation = component.button.navigation;
			navigation.mode = (enable ? Navigation.Mode.Explicit : Navigation.Mode.None);
			component.button.navigation = navigation;
		}
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x0004A30C File Offset: 0x0004850C
	public void Update()
	{
		if (this.pendingForceScrollToEnd)
		{
			this.ForceScrollToEnd();
			this.pendingForceScrollToEnd = false;
		}
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0004A324 File Offset: 0x00048524
	public void ForceUsePrimaryActionIfOnlySingleResponse(bool forceSingleResponse = true)
	{
		for (int i = 0; i < this.Options.Length; i++)
		{
			QuickResponseButton component = this.Options[i].gameObject.GetComponent<QuickResponseButton>();
			if (component != null)
			{
				component.ForceUsePrimaryActionIfOnlySingleResponse = forceSingleResponse;
			}
		}
	}

	// Token: 0x04000B63 RID: 2915
	public GameObject button;

	// Token: 0x04000B64 RID: 2916
	public Transform Chatbox;

	// Token: 0x04000B65 RID: 2917
	[SerializeField]
	private ChatTextBox youSentChatboxPrefab;

	// Token: 0x04000B66 RID: 2918
	[SerializeField]
	private ChatTextBox theySentChatboxPrefab;

	// Token: 0x04000B67 RID: 2919
	public ScrollRect screct;

	// Token: 0x04000B68 RID: 2920
	public Button[] Options;

	// Token: 0x04000B69 RID: 2921
	public Save.AppMessage appMessage;

	// Token: 0x04000B6A RID: 2922
	[SerializeField]
	public GameObject chatEndedText;

	// Token: 0x04000B6B RID: 2923
	public GameObject latestChatBreakLine;

	// Token: 0x04000B6C RID: 2924
	public bool chatEnded;

	// Token: 0x04000B6D RID: 2925
	private bool appendedChat;

	// Token: 0x04000B6E RID: 2926
	private string lastAppendedChat;

	// Token: 0x04000B6F RID: 2927
	public string Name;

	// Token: 0x04000B70 RID: 2928
	public bool isWaitingForNextAutomaticText;

	// Token: 0x04000B71 RID: 2929
	public bool isPendingShowChoices;

	// Token: 0x04000B72 RID: 2930
	public bool pendingForceScrollToEnd;
}
