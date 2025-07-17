using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ink.Runtime;
using Rewired;
using T17.Services;
using Team17.Scripts.Services.Input;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000151 RID: 337
public class ChatMaster : Singleton<ChatMaster>
{
	// Token: 0x06000C67 RID: 3175 RVA: 0x00046F82 File Offset: 0x00045182
	private void OnEnable()
	{
		this._inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.UIWithChat, this);
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00046F96 File Offset: 0x00045196
	private void OnDisable()
	{
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = null;
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00046FB0 File Offset: 0x000451B0
	public void UpdatePlayerData()
	{
		this.PlayerName.GetComponent<TextMeshProUGUI>().SetText(Singleton<Save>.Instance.GetPlayerName().ToUpperInvariant(), true);
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00046FD2 File Offset: 0x000451D2
	public void EnableCloseButton()
	{
		this.closeButton.gameObject.SetActive(true);
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00046FE8 File Offset: 0x000451E8
	public void ClearChatHistory()
	{
		this.AudioVOQueue.Clear();
		this.workspacepings = 0;
		this.ActiveChatWorkspace = null;
		this.WorkspaceChats.Clear();
		this.WorkspaceChatKeys.Clear();
		this.canopypings = 0;
		this.ActiveChatCanopy = null;
		this.CanopyChats.Clear();
		this.CanopyChatKeys.Clear();
		this.ActiveChatThiscord = null;
		this.ThiscordChats.Clear();
		this.ThiscordChatKeys.Clear();
		this.ClearChats();
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x0004706C File Offset: 0x0004526C
	private Sprite GetFriendIcon(string name)
	{
		string text = Path.Combine("Images", "UI", "Thiscord", "pfp_" + name.ToLowerInvariant());
		return Services.AssetProviderService.LoadResourceAsset<Sprite>(text, false);
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x000470AC File Offset: 0x000452AC
	public void ClearChats()
	{
		ParallelChat[] array = this.CanopyChatHolder.GetComponentsInChildren<ParallelChat>(true);
		for (int i = 0; i < array.Length; i++)
		{
			global::UnityEngine.Object.Destroy(array[i].gameObject);
		}
		ChatButton[] array2 = this.CanopyButtonHolder.GetComponentsInChildren<ChatButton>(true);
		for (int i = 0; i < array2.Length; i++)
		{
			global::UnityEngine.Object.Destroy(array2[i].gameObject);
		}
		array = this.WorkspaceChatHolder.GetComponentsInChildren<ParallelChat>(true);
		for (int i = 0; i < array.Length; i++)
		{
			global::UnityEngine.Object.Destroy(array[i].gameObject);
		}
		array2 = this.WorkspaceButtonHolder.GetComponentsInChildren<ChatButton>(true);
		for (int i = 0; i < array2.Length; i++)
		{
			global::UnityEngine.Object.Destroy(array2[i].gameObject);
		}
		array = this.ThiscordChatHolder.GetComponentsInChildren<ParallelChat>(true);
		for (int i = 0; i < array.Length; i++)
		{
			global::UnityEngine.Object.Destroy(array[i].gameObject);
		}
		array2 = this.ThiscordButtonHolder.GetComponentsInChildren<ChatButton>(true);
		for (int i = 0; i < array2.Length; i++)
		{
			global::UnityEngine.Object.Destroy(array2[i].gameObject);
		}
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x000471AC File Offset: 0x000453AC
	public void StartChat(string node, ChatType type, bool openChat = false, bool playSound = true)
	{
		List<Save.AppMessage> list = null;
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		if (type == ChatType.Wrkspce)
		{
			list = Singleton<Save>.Instance.GetPlayerWorkspaceVisited();
			gameObject = this.WorkspaceButtonHolder;
			gameObject2 = this.WorkspaceChatHolder;
			if (playSound)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_jobs_complete, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
		}
		else if (type == ChatType.Canopy)
		{
			list = Singleton<Save>.Instance.GetPlayerCanopyVisited();
			gameObject = this.CanopyButtonHolder;
			gameObject2 = this.CanopyChatHolder;
		}
		else if (type == ChatType.Thiscord)
		{
			list = Singleton<Save>.Instance.GetPlayerThiscordVisited();
			gameObject = this.ThiscordButtonHolder;
			gameObject2 = this.ThiscordChatHolder;
		}
		this.GetCorrespondingAppMessage(node, list, gameObject, gameObject2, type, openChat);
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00047250 File Offset: 0x00045450
	private void GetCorrespondingAppMessage(string node, List<Save.AppMessage> listPlayerVisited, GameObject ButtonHolder, GameObject ChatHolder, ChatType type, bool openChat = false)
	{
		foreach (Save.AppMessage appMessage in listPlayerVisited)
		{
			if (node.StartsWith(appMessage.NodePrefix))
			{
				if (appMessage.ContainsStitch(node))
				{
					return;
				}
				appMessage.NextStitches.Add(node);
				foreach (ParallelChat parallelChat in ChatHolder.GetComponentsInChildren<ParallelChat>(true))
				{
					if (parallelChat.appMessage.NodePrefix == appMessage.NodePrefix)
					{
						this.nodeCritPathTemp = node;
						this.chatCritPathTemp = parallelChat;
						this.LateContinueTextOnCritPath();
						break;
					}
				}
				foreach (ChatButton chatButton in ButtonHolder.GetComponentsInChildren<ChatButton>(true))
				{
					if (chatButton.NodePrefix == appMessage.NodePrefix)
					{
						if (type == ChatType.Wrkspce)
						{
							Singleton<AudioManager>.Instance.PlayTrack(this.workspaceNotification, AUDIO_TYPE.SFX, true, false, 0f, false, 1f, null, false, SFX_SUBGROUP.NONE, false);
						}
						chatButton.SetNewMessages(true);
						break;
					}
				}
				if (openChat)
				{
					this.ShowChat(appMessage, type, openChat, true);
				}
				return;
			}
		}
		Save.AppMessage appMessage2 = new Save.AppMessage();
		appMessage2.ChatType = type;
		appMessage2.Name = this.GetNameFromStitch(node);
		if (type == ChatType.Canopy)
		{
			appMessage2.NodePrefix = node;
		}
		else
		{
			appMessage2.NodePrefix = node.Substring(0, node.LastIndexOf("_") + 1);
		}
		if (!appMessage2.ContainsStitch(node))
		{
			appMessage2.NextStitches.Add(node);
		}
		Singleton<Save>.Instance.AddAppMessage(appMessage2);
		if (openChat)
		{
			this.ShowChat(appMessage2, type, openChat, true);
			return;
		}
		if (type == ChatType.Wrkspce)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.workspaceNotification, AUDIO_TYPE.SFX, true, false, 0f, false, 1f, null, false, SFX_SUBGROUP.NONE, false);
		}
		this.ShowChatButton(appMessage2, type, true);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00047448 File Offset: 0x00045648
	public void DelayAddNewMessage(string stitch, ChatType type)
	{
		this.newMessageStitchTemp = stitch;
		this.newMessageType = type;
		base.Invoke("AddNewMessage", 2f);
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x00047468 File Offset: 0x00045668
	private void AddNewMessage()
	{
		ParallelChat parallelChat = null;
		bool flag = false;
		if (this.activewindow == this.newMessageType)
		{
			switch (this.activewindow)
			{
			case ChatType.Wrkspce:
				parallelChat = this.ActiveChatWorkspace;
				break;
			case ChatType.Canopy:
				parallelChat = this.ActiveChatCanopy;
				break;
			case ChatType.Thiscord:
				parallelChat = this.ActiveChatThiscord;
				break;
			}
			if (parallelChat != null && this.newMessageStitchTemp.StartsWith(parallelChat.appMessage.NodePrefix))
			{
				flag = true;
			}
		}
		this.StartChat(this.newMessageStitchTemp, this.newMessageType, false, true);
		if (flag)
		{
			this.SwitchToChat(parallelChat, this.newMessageType, true);
		}
		Singleton<PhoneManager>.Instance.SetNewMessageAlert(this.newMessageType == ChatType.Wrkspce);
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x00047518 File Offset: 0x00045718
	private string GetNameFromStitch(string node)
	{
		if (node == "canopy_work.canopy_sewingmachine")
		{
			return "Sewing Machine";
		}
		if (node == "canopy_work.canopy_couch")
		{
			return "Inflatable Couch";
		}
		if (node == "canopy_work.canopy_cobalt")
		{
			return "Cobalt";
		}
		if (node == "canopy_work.canopy_bobbook")
		{
			return "Self-Help Book";
		}
		if (node == "canopy_work.canopy_paperclips")
		{
			return "Box of Paper Clips (500)";
		}
		if (node.StartsWith("thiscord_phone.tfh_"))
		{
			return "tinfoilhat";
		}
		if (node.StartsWith("thiscord_phone.sam_"))
		{
			return "Sam";
		}
		if (node.StartsWith("wrkspace_chat.mostmail_"))
		{
			return "David Most";
		}
		if (node.StartsWith("wrkspace_chat.tom_"))
		{
			return "Tom";
		}
		if (node.StartsWith("wrkspace_chat.sam_"))
		{
			return "Sam";
		}
		if (node.StartsWith("wrkspace_chat.val_"))
		{
			return "Val 9000";
		}
		return "";
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x000475FC File Offset: 0x000457FC
	private void LateContinueTextOnCritPath()
	{
		this.chatCritPathTemp.ClearChatEndedText();
		string currentFlowName = Singleton<InkController>.Instance.story.currentFlowName;
		Singleton<InkController>.Instance.story.SwitchFlow(this.nodeCritPathTemp);
		this.chatCritPathTemp.continuetext(-1, "", true, false);
		if (!this.chatCritPathTemp.chatEnded)
		{
			Singleton<PhoneManager>.Instance.SetNewMessageAlert(this.chatCritPathTemp.appMessage.NodePrefix.Contains("wrkspace"));
		}
		if (currentFlowName != null && currentFlowName != "DEFAULT_FLOW" && currentFlowName != "")
		{
			Singleton<InkController>.Instance.story.SwitchFlow(currentFlowName);
		}
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x000476AC File Offset: 0x000458AC
	public void LoadChatHistoryFromSave(Save.AppMessage node, ChatType type)
	{
		ParallelChat parallelChat = this.ShowChatButton(node, type, false);
		this.ShowAllNodeHistory(parallelChat, node, true);
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x000476CC File Offset: 0x000458CC
	public ParallelChat ShowChatButton(Save.AppMessage node, ChatType type, bool saveData = false)
	{
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
			this.player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnAwakenButtonUp), UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Awaken");
		}
		ParallelChat SetupChat = null;
		ChatButton chatButton = null;
		if (type == ChatType.Wrkspce)
		{
			SetupChat = global::UnityEngine.Object.Instantiate<GameObject>(this.WorkspaceChat, this.WorkspaceChatHolder.transform).GetComponent<ParallelChat>();
			chatButton = global::UnityEngine.Object.Instantiate<GameObject>(this.WorkspaceButtonPrefab, this.WorkspaceButtonHolder.transform).GetComponent<ChatButton>();
		}
		else if (type == ChatType.Canopy)
		{
			SetupChat = global::UnityEngine.Object.Instantiate<GameObject>(this.CanopyChat, this.CanopyChatHolder.transform).GetComponent<ParallelChat>();
			chatButton = global::UnityEngine.Object.Instantiate<GameObject>(this.CanopyButtonPrefab, this.CanopyButtonHolder.transform).GetComponent<ChatButton>();
		}
		else if (type == ChatType.Thiscord)
		{
			SetupChat = global::UnityEngine.Object.Instantiate<GameObject>(this.ThiscordChat, this.ThiscordChatHolder.transform).GetComponent<ParallelChat>();
			chatButton = global::UnityEngine.Object.Instantiate<GameObject>(this.ThiscordButtonPrefab, this.ThiscordButtonHolder.transform).GetComponent<ChatButton>();
		}
		chatButton.button.onClick.AddListener(delegate
		{
			this.SwitchToChat(SetupChat, type, false);
		});
		SetupChat.button = chatButton.gameObject;
		SetupChat.ForceUsePrimaryActionIfOnlySingleResponse(true);
		JoystickScroller componentInChildren = SetupChat.GetComponentInChildren<JoystickScroller>();
		if (componentInChildren != null)
		{
			componentInChildren.SetActivationObjects(new GameObject[] { base.gameObject });
		}
		SetupChat.appMessage = node;
		chatButton.CharacterName.text = node.Name;
		SetupChat.Name = node.Name;
		chatButton.NodePrefix = node.NodePrefix;
		Sprite characterIcon = chatButton.GetCharacterIcon();
		if (chatButton.Icon != null)
		{
			chatButton.Icon.sprite = characterIcon;
		}
		this.SetupButtonNavigation(chatButton, type);
		if (type == ChatType.Canopy)
		{
			this.CanopyChats.Add(SetupChat);
			if (this.activewindow != ChatType.Canopy)
			{
				this.canopypings++;
			}
		}
		else if (type == ChatType.Wrkspce)
		{
			this.WorkspaceChats.Add(SetupChat);
			if (this.activewindow != ChatType.Wrkspce)
			{
				this.workspacepings++;
			}
		}
		else if (type == ChatType.Thiscord)
		{
			this.ThiscordChats.Add(SetupChat);
			if (this.activewindow != ChatType.Thiscord)
			{
				this.thiscordpings++;
			}
		}
		WorkspaceChannelButtonStyle component = SetupChat.button.GetComponent<WorkspaceChannelButtonStyle>();
		if (component != null)
		{
			component.ToggleSelected(false);
		}
		SetupChat.gameObject.SetActive(false);
		return SetupChat;
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x000479A0 File Offset: 0x00045BA0
	private void SetupButtonNavigation(ChatButton chatButton, ChatType chatType)
	{
		List<ParallelChat> list = null;
		Navigation navigation = chatButton.button.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnLeft = null;
		navigation.selectOnRight = null;
		navigation.selectOnDown = null;
		navigation.selectOnUp = null;
		switch (chatType)
		{
		case ChatType.Wrkspce:
			list = this.WorkspaceChats;
			break;
		case ChatType.Canopy:
			list = this.CanopyChats;
			break;
		case ChatType.Thiscord:
			list = this.ThiscordChats;
			break;
		}
		if (list != null && list.Count > 0)
		{
			ChatButton component = list[list.Count - 1].button.GetComponent<ChatButton>();
			if (component != null)
			{
				Navigation navigation2 = component.button.navigation;
				navigation2.selectOnDown = chatButton.button;
				component.button.navigation = navigation2;
				navigation.selectOnUp = component.button;
			}
		}
		chatButton.button.navigation = navigation;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00047A7C File Offset: 0x00045C7C
	public ParallelChat ShowChat(Save.AppMessage node, ChatType type, bool openChat = false, bool saveData = false)
	{
		if (openChat)
		{
			node.MoveNextToHistory();
		}
		ParallelChat parallelChat = this.ShowChatButton(node, type, saveData);
		string currentFlowName = Singleton<InkController>.Instance.story.currentFlowName;
		Singleton<InkController>.Instance.story.SwitchFlow(node.GetLastStitch());
		Singleton<InkController>.Instance.story.ChoosePathString(node.GetLastStitch(), true, Array.Empty<object>());
		Singleton<InkController>.Instance.ContinueStory();
		parallelChat.GetText(true, saveData, false);
		parallelChat.showbuttons(false, saveData, false, false);
		Singleton<InkController>.Instance.story.SwitchFlow(currentFlowName);
		parallelChat.gameObject.SetActive(false);
		if (openChat)
		{
			this.SetSelected(parallelChat.button);
			this.SwitchToChat(parallelChat, type, false);
		}
		else
		{
			WorkspaceChannelButtonStyle component = parallelChat.button.GetComponent<WorkspaceChannelButtonStyle>();
			if (component != null)
			{
				component.ToggleSelected(false);
			}
		}
		return parallelChat;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x00047B54 File Offset: 0x00045D54
	public void CloseComputerTutorialFix()
	{
		if (Singleton<Save>.Instance.GetTutorialFinished())
		{
			if (Singleton<PhoneManager>.Instance.CanCloseCommApp())
			{
				Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
				Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreenRotate();
				this.ActiveChatWorkspace = null;
			}
			return;
		}
		if (!Singleton<ComputerManager>.Instance.ReadyToExit())
		{
			Singleton<Popup>.Instance.CreatePopup("VALDIVIAN", "You still haven't finished your work for the day!", true);
			return;
		}
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
		{
			this.ConfirmCloseComputer();
			return;
		}
		UnityEvent unityEvent = new UnityEvent();
		unityEvent.AddListener(new UnityAction(this.ConfirmCloseComputer));
		UnityEvent unityEvent2 = new UnityEvent();
		unityEvent2.AddListener(new UnityAction(this.CloseComputerAlert));
		Singleton<ChatMaster>.Instance.workspaceping.SetActive(false);
		Singleton<Popup>.Instance.CreatePopup("VALDIVIAN", "Are you ready to leave work?", unityEvent, unityEvent2, true);
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x00047C2A File Offset: 0x00045E2A
	private void ConfirmCloseComputer()
	{
		Singleton<TutorialController>.Instance.TutorialFinishedWork();
		Singleton<ComputerManager>.Instance.ExitOutOfCanopyMenu();
		MovingDateable.MoveDateable("Computer", "after", true);
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00047C50 File Offset: 0x00045E50
	private void CloseComputerAlert()
	{
		Singleton<Popup>.Instance.CreatePopup("VALDIVIAN", "When you are ready, select the 'X' in the top right corner.", true);
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x00047C67 File Offset: 0x00045E67
	public void CloseComputerThiscord()
	{
		if (!Singleton<ComputerManager>.Instance.ThiscordReadyToExit())
		{
			Singleton<Popup>.Instance.CreatePopup("THISCORD", "You still haven't read all your messages!", true);
			return;
		}
		Singleton<TutorialController>.Instance.ForceCloseThiscord();
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x00047C95 File Offset: 0x00045E95
	public void CloseComputer()
	{
		Singleton<TutorialController>.Instance.ForceCloseComputer2();
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x00047CA4 File Offset: 0x00045EA4
	private void ShowAllNodeHistory(ParallelChat SetupChat, Save.AppMessage node, bool isLoadingGame = false)
	{
		for (int i = 0; i < node.StitchHistory.Count; i++)
		{
			Save.AppMessageStitch appMessageStitch = node.StitchHistory[i];
			if (appMessageStitch.Stitch.StartsWith("canopy_work.") || appMessageStitch.Stitch.StartsWith("thiscord_phone.") || appMessageStitch.Stitch.StartsWith("wrkspace_chat."))
			{
				Singleton<InkController>.Instance.story.SwitchFlow(appMessageStitch.Stitch);
				Singleton<InkController>.Instance.story.ChoosePathString(appMessageStitch.Stitch, true, Array.Empty<object>());
				SetupChat.continuetext(-1, "", false, isLoadingGame);
				int num = 0;
				List<Choice> list = Singleton<InkController>.Instance.story.currentChoices;
				while (list.Count > 0 || Singleton<InkController>.Instance.CanContinue())
				{
					if (list.Count > 0)
					{
						if (appMessageStitch.ChatHistoryOptionsSelected.Count > num)
						{
							SetupChat.ChooseJump(appMessageStitch.ChatHistoryOptionsSelected[num], false);
							num++;
						}
						else
						{
							SetupChat.ChooseJump(list[0].targetPath.ToString(), false);
						}
					}
					else
					{
						SetupChat.continuetext(-1, "", false, isLoadingGame);
					}
					list = Singleton<InkController>.Instance.story.currentChoices;
				}
			}
		}
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00047DE4 File Offset: 0x00045FE4
	private void OnAwakenButtonUp(InputActionEventData data)
	{
		if (this.WillieInteraction())
		{
			Singleton<Save>.Instance.MeetDatableIfUnmet("willi");
			if (!Singleton<PhoneManager>.Instance.IsPhoneMenuOpened() && Singleton<Save>.Instance.GetDateStatusRealized("willi_work") != RelationshipStatus.Realized)
			{
				Singleton<ComputerManager>.Instance.ExitOutOfCanopyMenu();
				Singleton<GameController>.Instance.ForceDialogue("willi_work", null, false, false);
			}
		}
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x00047E43 File Offset: 0x00046043
	private List<ParallelChat> GetChatsByType(ChatType type)
	{
		if (type == ChatType.Wrkspce)
		{
			return this.WorkspaceChats;
		}
		if (type == ChatType.Canopy)
		{
			return this.CanopyChats;
		}
		return this.ThiscordChats;
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x00047E60 File Offset: 0x00046060
	private bool WillieInteraction()
	{
		return Singleton<Save>.Instance.GetDateableTalkedTo("willi") != Singleton<DayNightCycle>.Instance.GetDaysSinceStart() && base.gameObject.activeInHierarchy && this.IsWrkspceOpened() && Singleton<Save>.Instance.GetFullTutorialFinished() && Singleton<Dateviators>.Instance.Equipped && Singleton<Dateviators>.Instance.CanConsumeCharge() && !BetterPlayerControl.Instance.isGameEndingOn;
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x00047ED0 File Offset: 0x000460D0
	private bool IsChatActive(ParallelChat chat)
	{
		return !(chat == null) && (this.ActiveChatCanopy == chat || this.ActiveChatWorkspace == chat || this.ActiveChatThiscord == chat);
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x00047F08 File Offset: 0x00046108
	public void SwitchToChat(ParallelChat chat, ChatType type, bool force = false)
	{
		if (this.activewindow == type && this.IsChatActive(chat) && (this.ActiveChatCanopy != chat || this.CanopyChats.Count > 0))
		{
			chat.ForceScrollToEnd();
			if (!force)
			{
				return;
			}
		}
		if (!force && !this.HasCurrentChatEnded())
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		foreach (ParallelChat parallelChat in this.GetChatsByType(type))
		{
			parallelChat.gameObject.SetActive(parallelChat == chat);
			if (parallelChat.button != null)
			{
				WorkspaceChannelButtonStyle component = parallelChat.button.GetComponent<WorkspaceChannelButtonStyle>();
				if (component != null)
				{
					component.ToggleSelected(parallelChat == chat);
				}
			}
		}
		if (this.activewindow != type || this.AudioVOQueue.Count > 0)
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.5f);
		}
		if (chat != null && !Singleton<InkController>.Instance.story.currentFlowName.Contains(chat.appMessage.NodePrefix))
		{
			Singleton<InkController>.Instance.story.SwitchFlow(chat.appMessage.GetLastStitch());
		}
		foreach (string text in this.AudioVOQueue.Keys.ToList<string>())
		{
			if (chat != null && chat.appMessage.NodePrefix.StartsWith(text))
			{
				float num = Singleton<AudioManager>.Instance.PlayTrack(this.AudioVOQueue[text], AUDIO_TYPE.DIALOGUE, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
				if (num > 0f)
				{
					chat.isWaitingForNextAutomaticText = true;
					chat.ScheduleProcessNextOption(num, 0.3f);
				}
				this.AudioVOQueue.Remove(text);
			}
		}
		this.activewindow = type;
		switch (type)
		{
		case ChatType.Wrkspce:
			this.UpdateVisualsWorkspace();
			if (this.workspacepings > 0)
			{
				Singleton<Dateviators>.Instance.ClearNewPhoneMessageInHud();
			}
			this.workspacepings = 0;
			this.Workspace.SetActive(true);
			this.Canopy.SetActive(false);
			this.Thiscord.SetActive(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			this.ActiveChatWorkspace = chat;
			this.BigCharacterIcon.gameObject.SetActive(true);
			this.CharacterNameText.SetActive(true);
			this.RatingText.SetActive(true);
			if (chat != null)
			{
				this.BigCharacterIcon.GetComponent<Image>().sprite = chat.button.GetComponent<ChatButton>().GetCharacterIcon();
				this.CharacterNameText.GetComponent<TextMeshProUGUI>().text = chat.appMessage.Name.ToUpperInvariant();
				this.ProcessRatingStars(chat.appMessage.Name);
				this.ActiveChatWorkspace.showbuttons(false, true, false, false);
			}
			break;
		case ChatType.Canopy:
			this.UpdateVisualsCanopy();
			this.canopypings = 0;
			this.Workspace.SetActive(false);
			this.Canopy.SetActive(true);
			this.Thiscord.SetActive(false);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			this.ActiveChatCanopy = chat;
			if (chat != null)
			{
				this.CanopyEmptyMessage.SetActive(false);
				this.ActiveChatCanopy.showbuttons(false, true, false, false);
			}
			else
			{
				this.CanopyEmptyMessage.SetActive(true);
			}
			break;
		case ChatType.Thiscord:
			if (this.thiscordpings > 0)
			{
				Singleton<Dateviators>.Instance.ClearNewPhoneMessageInHud();
			}
			this.thiscordpings = 0;
			this.Workspace.SetActive(false);
			this.Canopy.SetActive(false);
			this.Thiscord.SetActive(true);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			this.ActiveChatThiscord = chat;
			if (chat == null)
			{
				this.FriendName.SetActive(false);
				this.FriendIcon.SetActive(false);
				return;
			}
			this.FriendName.SetActive(true);
			this.FriendIcon.SetActive(true);
			this.FriendName.GetComponent<TextMeshProUGUI>().SetText(chat.appMessage.Name.ToUpperInvariant(), true);
			this.FriendIcon.GetComponent<Image>().sprite = this.GetFriendIcon(chat.appMessage.Name);
			this.ActiveChatThiscord.showbuttons(false, true, false, false);
			break;
		}
		if (chat != null)
		{
			chat.AutoContinueText(true);
		}
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x000483F4 File Offset: 0x000465F4
	private void ProcessRatingStars(string Name)
	{
		this.LeftStars.gameObject.SetActive(true);
		string text = Name.Trim();
		if (text == "David Most")
		{
			this.LeftStars.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "stars_5"), false);
			return;
		}
		if (text == "foil")
		{
			this.LeftStars.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "stars_5"), false);
			return;
		}
		if (text == "Val 9000")
		{
			this.LeftStars.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "stars_5"), false);
			return;
		}
		if (text == "Tom")
		{
			this.LeftStars.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "stars_4"), false);
			return;
		}
		if (!(text == "Sam"))
		{
			this.LeftStars.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "stars_1"), false);
			return;
		}
		this.LeftStars.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "stars_3"), false);
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x00048588 File Offset: 0x00046788
	public void deletechat(ParallelChat c)
	{
		foreach (ParallelChat parallelChat in this.CanopyChats)
		{
			if (c == parallelChat)
			{
				this.CanopyChats.Remove(parallelChat);
				return;
			}
		}
		foreach (ParallelChat parallelChat2 in this.WorkspaceChats)
		{
			if (c == parallelChat2)
			{
				this.WorkspaceChats.Remove(parallelChat2);
				return;
			}
		}
		foreach (ParallelChat parallelChat3 in this.ThiscordChats)
		{
			if (c == parallelChat3)
			{
				this.ThiscordChats.Remove(parallelChat3);
				break;
			}
		}
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x00048698 File Offset: 0x00046898
	public void UpdateVisualsCanopy()
	{
		this.canopyping.SetActive(this.canopypings > 0);
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x000486B0 File Offset: 0x000468B0
	public void UpdateVisualsWorkspace()
	{
		this.workspaceping.SetActive(this.workspacepings > 0);
		if (this.workspacepings > 0)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_jobs_complete, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00048700 File Offset: 0x00046900
	public void Update()
	{
		if (this._pendingFocus != null)
		{
			this.SetSelected(this._pendingFocus);
		}
		this.ProcessInput();
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00048724 File Offset: 0x00046924
	private void ProcessInput()
	{
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
			this.player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnAwakenButtonUp), UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Awaken");
		}
		if (this.activewindow == ChatType.Wrkspce && this.player.GetButton(38) && this.canopyButton.interactable && this.canopyButton.gameObject.activeInHierarchy)
		{
			Button.ButtonClickedEvent onClick = this.canopyButton.onClick;
			if (onClick != null)
			{
				onClick.Invoke();
			}
		}
		if (this.activewindow == ChatType.Canopy && this.player.GetButton(37) && this.workspaceButton.interactable && this.workspaceButton.gameObject.activeInHierarchy)
		{
			Button.ButtonClickedEvent onClick2 = this.workspaceButton.onClick;
			if (onClick2 != null)
			{
				onClick2.Invoke();
			}
		}
		int num = Mathf.Clamp(Mathf.RoundToInt(float.Parse(Singleton<InkController>.Instance.GetVariable("star"))) - 1, 0, this.RatingStarSprites.Length - 1);
		for (int i = 0; i < this.RatingStars.Length; i++)
		{
			this.RatingStars[i].sprite = this.RatingStarSprites[num];
		}
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00048858 File Offset: 0x00046A58
	public void StartNewCanopyChat()
	{
		if (this.canopyWorksDay1 != null && this.canopyWorksDay1.Count > 0)
		{
			string text = this.canopyWorksDay1[0];
			this.canopyWorksDay1.RemoveAt(0);
			Singleton<ChatMaster>.Instance.StartChat(text, ChatType.Canopy, false, true);
		}
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x000488A4 File Offset: 0x00046AA4
	private void SelectLastSelectedOrDefaultWRKChatButton()
	{
		ParallelChat parallelChat = this.ActiveChatWorkspace;
		if (parallelChat == null)
		{
			if (this.WorkspaceChats.Count == 0)
			{
				return;
			}
			parallelChat = this.WorkspaceChats.First<ParallelChat>();
		}
		this.SelectChatButton(parallelChat);
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x000488E4 File Offset: 0x00046AE4
	private void SelectLastSelectedOrDefaultCanopyChatButton()
	{
		ParallelChat parallelChat = this.ActiveChatCanopy;
		if (parallelChat == null && this.CanopyChats.Count > 0)
		{
			parallelChat = this.CanopyChats.First<ParallelChat>();
		}
		this.SelectChatButton(parallelChat);
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x00048924 File Offset: 0x00046B24
	private void SelectLastSelectedOrDefaultThiscordChatButton()
	{
		ParallelChat parallelChat = this.ActiveChatThiscord;
		if (parallelChat == null && this.ThiscordChats.Count > 0)
		{
			parallelChat = this.ThiscordChats.First<ParallelChat>();
		}
		this.SelectChatButton(parallelChat);
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00048964 File Offset: 0x00046B64
	public void SelectCurrentActiveChatButton()
	{
		if (this.activewindow == ChatType.Thiscord && this.ActiveChatThiscord != null)
		{
			this.SelectChatButton(this.ActiveChatThiscord);
			return;
		}
		if (this.activewindow == ChatType.Wrkspce && this.ActiveChatWorkspace != null)
		{
			this.SelectChatButton(this.ActiveChatWorkspace);
			return;
		}
		if (this.activewindow == ChatType.Canopy && this.ActiveChatCanopy != null)
		{
			this.SelectChatButton(this.ActiveChatCanopy);
			return;
		}
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x000489DC File Offset: 0x00046BDC
	public void SelectChatButton(ParallelChat chat)
	{
		if (chat == null || chat.button == null)
		{
			return;
		}
		this.SetSelected(chat.button);
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x00048A02 File Offset: 0x00046C02
	public bool IsWrkspceOpened()
	{
		return this.Workspace && this.Workspace.activeInHierarchy;
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00048A20 File Offset: 0x00046C20
	public void SwitchtoWRK()
	{
		if (!this.HasCurrentChatEnded())
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		this.Workspace.SetActive(true);
		this.Canopy.SetActive(false);
		this.Thiscord.SetActive(false);
		if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
		{
			this.StartChat("wrkspace_chat.mostmail_1", ChatType.Wrkspce, true, true);
		}
		this.ActiveChatWorkspace = null;
		if (this.ActiveChatWorkspace)
		{
			this.SwitchToChat(this.ActiveChatWorkspace, ChatType.Wrkspce, false);
		}
		this.SelectLastSelectedOrDefaultWRKChatButton();
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x00048AC8 File Offset: 0x00046CC8
	public void SwitchtoCNPY()
	{
		if (!this.HasCurrentChatEnded())
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		if (Singleton<Save>.Instance.HasFinishedMessage("wrkspace_chat.mostmail_1") && Singleton<Save>.Instance.HasFinishedMessage("wrkspace_chat.tom_1") && Singleton<Save>.Instance.HasFinishedMessage("wrkspace_chat.sam_1"))
		{
			this.Workspace.SetActive(false);
			this.Canopy.SetActive(true);
			this.Thiscord.SetActive(false);
			this.SwitchToChat(this.ActiveChatCanopy, ChatType.Canopy, false);
			this.SelectLastSelectedOrDefaultCanopyChatButton();
			if (!this.playedCanopySound)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_populate, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				this.playedCanopySound = true;
				return;
			}
		}
		else
		{
			Singleton<Popup>.Instance.CreatePopup("VALDIVIAN", "You still haven't read through all your WRKSPCE messages!", true);
		}
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x00048BC0 File Offset: 0x00046DC0
	public void SwitchtoThiscord()
	{
		this.Workspace.SetActive(false);
		this.Canopy.SetActive(false);
		this.Thiscord.SetActive(true);
		if (!Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_2_SAW_THISCORD) && Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
		{
			this.StartChat("thiscord_phone.tfh_1", ChatType.Thiscord, true, false);
		}
		this.ActiveChatThiscord = null;
		this.FriendName.SetActive(false);
		this.FriendIcon.SetActive(false);
		if (this.ActiveChatThiscord)
		{
			this.SwitchToChat(this.ActiveChatThiscord, ChatType.Thiscord, false);
		}
		this.SelectLastSelectedOrDefaultThiscordChatButton();
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x00048C64 File Offset: 0x00046E64
	public void SetSelected(GameObject button)
	{
		this._pendingFocus = null;
		if (button != null)
		{
			if (button.activeInHierarchy)
			{
				Selectable component = button.GetComponent<Selectable>();
				if (component != null && component.isActiveAndEnabled && component.interactable)
				{
					ControllerMenuUI.SetCurrentlySelected(button, ControllerMenuUI.Direction.Down, false, false);
					return;
				}
			}
			this._pendingFocus = button;
		}
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x00048CBC File Offset: 0x00046EBC
	public bool HasCurrentChatEnded()
	{
		switch (this.activewindow)
		{
		case ChatType.Wrkspce:
			if (this.ActiveChatWorkspace != null)
			{
				return this.ActiveChatWorkspace.chatEnded;
			}
			break;
		case ChatType.Canopy:
			if (this.ActiveChatCanopy != null)
			{
				return this.ActiveChatCanopy.chatEnded;
			}
			break;
		case ChatType.Thiscord:
			if (this.ActiveChatThiscord != null)
			{
				return this.ActiveChatThiscord.chatEnded;
			}
			break;
		}
		return true;
	}

	// Token: 0x04000B1F RID: 2847
	[Header("Visuals")]
	public CanopyInfoUpdater ciu;

	// Token: 0x04000B20 RID: 2848
	public GameObject canopyping;

	// Token: 0x04000B21 RID: 2849
	public GameObject workspaceping;

	// Token: 0x04000B22 RID: 2850
	public int canopypings;

	// Token: 0x04000B23 RID: 2851
	public int workspacepings;

	// Token: 0x04000B24 RID: 2852
	public int thiscordpings;

	// Token: 0x04000B25 RID: 2853
	[Header("Main")]
	public ChatType activewindow;

	// Token: 0x04000B26 RID: 2854
	public GameObject BigCharacterIcon;

	// Token: 0x04000B27 RID: 2855
	public GameObject Canopy;

	// Token: 0x04000B28 RID: 2856
	public GameObject Workspace;

	// Token: 0x04000B29 RID: 2857
	public GameObject Thiscord;

	// Token: 0x04000B2A RID: 2858
	public GameObject CharacterNameText;

	// Token: 0x04000B2B RID: 2859
	public GameObject RatingText;

	// Token: 0x04000B2C RID: 2860
	public Image LeftStars;

	// Token: 0x04000B2D RID: 2861
	public Image[] RatingStars;

	// Token: 0x04000B2E RID: 2862
	public Sprite[] RatingStarSprites;

	// Token: 0x04000B2F RID: 2863
	public ParallelChat ActiveChatCanopy;

	// Token: 0x04000B30 RID: 2864
	public ParallelChat ActiveChatWorkspace;

	// Token: 0x04000B31 RID: 2865
	public ParallelChat ActiveChatThiscord;

	// Token: 0x04000B32 RID: 2866
	[SerializeField]
	private DesktopTabButton canopyTabButtonStyle;

	// Token: 0x04000B33 RID: 2867
	[SerializeField]
	private DesktopTabButton workspaceTabButtonStyle;

	// Token: 0x04000B34 RID: 2868
	[SerializeField]
	private DesktopTabButton thiscordTabButtonStyle;

	// Token: 0x04000B35 RID: 2869
	[SerializeField]
	private AudioClip workspaceNotification;

	// Token: 0x04000B36 RID: 2870
	[Header("Current Active Chats")]
	public List<ParallelChat> WorkspaceChats;

	// Token: 0x04000B37 RID: 2871
	public List<ParallelChat> CanopyChats;

	// Token: 0x04000B38 RID: 2872
	public List<ParallelChat> ThiscordChats;

	// Token: 0x04000B39 RID: 2873
	public List<string> WorkspaceChatKeys;

	// Token: 0x04000B3A RID: 2874
	public List<string> CanopyChatKeys;

	// Token: 0x04000B3B RID: 2875
	public List<string> ThiscordChatKeys;

	// Token: 0x04000B3C RID: 2876
	[Header("Chat Holders / Instantiators")]
	public GameObject WorkspaceChatHolder;

	// Token: 0x04000B3D RID: 2877
	public GameObject CanopyChatHolder;

	// Token: 0x04000B3E RID: 2878
	public GameObject ThiscordChatHolder;

	// Token: 0x04000B3F RID: 2879
	public GameObject WorkspaceChat;

	// Token: 0x04000B40 RID: 2880
	public GameObject CanopyChat;

	// Token: 0x04000B41 RID: 2881
	public GameObject ThiscordChat;

	// Token: 0x04000B42 RID: 2882
	[Header("Button Holders / Instantiators")]
	public ScrollRect WorkspaceButtonScrollRect;

	// Token: 0x04000B43 RID: 2883
	public GameObject WorkspaceButtonHolder;

	// Token: 0x04000B44 RID: 2884
	public GameObject CanopyButtonHolder;

	// Token: 0x04000B45 RID: 2885
	public GameObject ThiscordButtonHolder;

	// Token: 0x04000B46 RID: 2886
	public GameObject WorkspaceButtonPrefab;

	// Token: 0x04000B47 RID: 2887
	public GameObject CanopyButtonPrefab;

	// Token: 0x04000B48 RID: 2888
	public GameObject ThiscordButtonPrefab;

	// Token: 0x04000B49 RID: 2889
	private Player player;

	// Token: 0x04000B4A RID: 2890
	public Button canopyButton;

	// Token: 0x04000B4B RID: 2891
	public Button workspaceButton;

	// Token: 0x04000B4C RID: 2892
	public Button closeButton;

	// Token: 0x04000B4D RID: 2893
	public Button closeButtonThiscord;

	// Token: 0x04000B4E RID: 2894
	public Dictionary<string, string> AudioVOQueue = new Dictionary<string, string>();

	// Token: 0x04000B4F RID: 2895
	[Header("Thiscord")]
	public GameObject PlayerName;

	// Token: 0x04000B50 RID: 2896
	public GameObject FriendName;

	// Token: 0x04000B51 RID: 2897
	public GameObject FriendIcon;

	// Token: 0x04000B52 RID: 2898
	private InputModeHandle _inputModeHandle;

	// Token: 0x04000B53 RID: 2899
	private GameObject _pendingFocus;

	// Token: 0x04000B54 RID: 2900
	public GameObject CanopyEmptyMessage;

	// Token: 0x04000B55 RID: 2901
	private string newMessageStitchTemp;

	// Token: 0x04000B56 RID: 2902
	private ChatType newMessageType;

	// Token: 0x04000B57 RID: 2903
	private string nodeCritPathTemp;

	// Token: 0x04000B58 RID: 2904
	private ParallelChat chatCritPathTemp;

	// Token: 0x04000B59 RID: 2905
	public List<string> canopyWorksDay1;

	// Token: 0x04000B5A RID: 2906
	private bool playedCanopySound;
}
