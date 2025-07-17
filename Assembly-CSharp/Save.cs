using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Assets.Date_Everything.Scripts.SaveVersionAutoFix;
using Date_Everything.Scripts;
using Date_Everything.Scripts.Ink;
using Newtonsoft.Json;
using T17.Flow;
using T17.Services;
using Team17.Common;
using Team17.Platform.SaveGame;
using Team17.Services.Save;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000C2 RID: 194
public class Save : Singleton<Save>
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060005FA RID: 1530 RVA: 0x00021DAC File Offset: 0x0001FFAC
	// (remove) Token: 0x060005FB RID: 1531 RVA: 0x00021DE0 File Offset: 0x0001FFE0
	public static event Save.OnGameLoad onGameLoad;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060005FC RID: 1532 RVA: 0x00021E14 File Offset: 0x00020014
	// (remove) Token: 0x060005FD RID: 1533 RVA: 0x00021E48 File Offset: 0x00020048
	public static event Save.AfterGameLoadReset afterGameLoadReset;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060005FE RID: 1534 RVA: 0x00021E7C File Offset: 0x0002007C
	// (remove) Token: 0x060005FF RID: 1535 RVA: 0x00021EB0 File Offset: 0x000200B0
	public static event Save.AfterGameLoad afterGameLoad;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000600 RID: 1536 RVA: 0x00021EE4 File Offset: 0x000200E4
	// (remove) Token: 0x06000601 RID: 1537 RVA: 0x00021F18 File Offset: 0x00020118
	public static event Save.OnGameSave onGameSave;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000602 RID: 1538 RVA: 0x00021F4C File Offset: 0x0002014C
	// (remove) Token: 0x06000603 RID: 1539 RVA: 0x00021F80 File Offset: 0x00020180
	public static event Save.OnGameSaveCompleted onGameSaveCompleted;

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000604 RID: 1540 RVA: 0x00021FB3 File Offset: 0x000201B3
	public static int MAX_SAVE_SLOTS
	{
		get
		{
			return Services.MaxSaveSlotsProvider.GetMaxSaveSlots();
		}
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000605 RID: 1541 RVA: 0x00021FC0 File Offset: 0x000201C0
	// (remove) Token: 0x06000606 RID: 1542 RVA: 0x00021FF4 File Offset: 0x000201F4
	public static event Save.OnAwakening onAwakening;

	// Token: 0x06000607 RID: 1543 RVA: 0x00022027 File Offset: 0x00020227
	public override void AwakeSingleton()
	{
		Save._saveDirectory = Application.dataPath + "/Saves/";
		this._uiUtilities = Object.FindObjectOfType<UIUtilities>();
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00022048 File Offset: 0x00020248
	public void NewGame()
	{
		if (Singleton<DeluxeEditionController>.Instance != null)
		{
			Singleton<DeluxeEditionController>.Instance.UpdateEntitlements();
		}
		Save._saveData = new Save.SaveData();
		if (Save._saveData == null)
		{
			T17Debug.LogError("Save data is null! Save data failed to initialize.");
		}
		Save._saveData.dateviators_currentCharges = 5;
		Save._saveData.dayNightCycle_presentDayPresentTime = new DateTime(2024, 10, 1);
		Save._saveData.dayNightCycle_currentDayPhase = DayPhase.MORNING;
		Save._saveData.dayNightCycle_daysSinceStart = 0;
		this.SafetyTimeOnGameLoad = (long)DateTime.UtcNow.Millisecond;
		this._pendingFinishedMessages.Clear();
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x000220DE File Offset: 0x000202DE
	public void InvokeSaveEvent()
	{
		Save.OnGameSave onGameSave = Save.onGameSave;
		if (onGameSave == null)
		{
			return;
		}
		onGameSave();
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x000220EF File Offset: 0x000202EF
	public void InvokeSaveCompletedEvent(SaveGameError result)
	{
		Save.OnGameSaveCompleted onGameSaveCompleted = Save.onGameSaveCompleted;
		if (onGameSaveCompleted == null)
		{
			return;
		}
		onGameSaveCompleted(result);
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x00022101 File Offset: 0x00020301
	public static void InvokeResetEvent()
	{
		Save.AfterGameLoadReset afterGameLoadReset = Save.afterGameLoadReset;
		if (afterGameLoadReset == null)
		{
			return;
		}
		afterGameLoadReset();
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00022112 File Offset: 0x00020312
	public static void InvokeLoadEvent()
	{
		Save.AfterGameLoad afterGameLoad = Save.afterGameLoad;
		if (afterGameLoad == null)
		{
			return;
		}
		afterGameLoad();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00022124 File Offset: 0x00020324
	public void AddFinishedMessage(string messageNode, bool allowDuplicates = true)
	{
		if (Save._saveData.IsAddFinishedMessageInProgress)
		{
			if (!allowDuplicates)
			{
				using (Queue<Tuple<string, float>>.Enumerator enumerator = this._pendingFinishedMessages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Item1 == messageNode)
						{
							return;
						}
					}
				}
				if (this.DoFinishedMessagesContain(messageNode))
				{
					return;
				}
			}
			float lengthOfCurrentSongOfType = Singleton<AudioManager>.Instance.GetLengthOfCurrentSongOfType(AUDIO_TYPE.DIALOGUE);
			Tuple<string, float> tuple = new Tuple<string, float>(messageNode, lengthOfCurrentSongOfType);
			this._pendingFinishedMessages.Enqueue(tuple);
			return;
		}
		Save._saveData.AddFinishedMessage(messageNode, -1f, allowDuplicates);
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x000221CC File Offset: 0x000203CC
	public List<Save.AppMessage> GetAllChatHistory()
	{
		return Save._saveData.AllChatHistory;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x000221D8 File Offset: 0x000203D8
	public void SetDexEntriesInDateADex()
	{
		Save._saveData.SetDexEntriesInDateADex();
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x000221E4 File Offset: 0x000203E4
	public long GetPlayTimeInMillis()
	{
		return Save._saveData.playTimeInMillis;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x000221F0 File Offset: 0x000203F0
	public List<Save.AppMessageStitch> GetFinishedMessages()
	{
		List<Save.AppMessageStitch> list = new List<Save.AppMessageStitch>();
		foreach (Save.AppMessage appMessage in Save._saveData.AllChatHistory)
		{
			foreach (Save.AppMessageStitch appMessageStitch in appMessage.StitchHistory)
			{
				if (appMessageStitch.Ended)
				{
					list.Add(appMessageStitch);
				}
			}
		}
		return list;
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00022290 File Offset: 0x00020490
	public bool HasFinishedMessage(string message)
	{
		foreach (Save.AppMessage appMessage in Save._saveData.AllChatHistory)
		{
			using (List<Save.AppMessageStitch>.Enumerator enumerator2 = appMessage.StitchHistory.GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Save.AppMessageStitch appMessageStitch = enumerator2.Current;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00022320 File Offset: 0x00020520
	[Obsolete("Legacy code which is no longer be used. Either change the calling code to call Save.SaveGame() instead or fix up Save.SaveNewGamePlus() to have the required functionality")]
	public async Task SaveNewGamePlus(int saveSlotID)
	{
		await this.SaveGameAsync(saveSlotID, true);
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0002236B File Offset: 0x0002056B
	public static string GetFormattedTimeNow()
	{
		return Save.FormatDateForSaving(DateTime.UtcNow);
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00022377 File Offset: 0x00020577
	private static string FormatDateForSaving(DateTime dateTime)
	{
		return dateTime.ToString("o");
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00022388 File Offset: 0x00020588
	public static bool TryParseSavedDateTime(string savedDateTime, out DateTime dateTime)
	{
		if (DateTime.TryParse(savedDateTime, null, DateTimeStyles.RoundtripKind, out dateTime))
		{
			return true;
		}
		if (DateTime.TryParse(savedDateTime, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dateTime))
		{
			return true;
		}
		foreach (CultureInfo cultureInfo in Save.FallbackCultures)
		{
			if (DateTime.TryParse(savedDateTime, cultureInfo.DateTimeFormat, DateTimeStyles.None, out dateTime))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x000223E8 File Offset: 0x000205E8
	public async Task SaveGameAsync(int saveSlotID, bool forceIsNewGamePlus = false)
	{
		if (!Services.SaveGameService.IsBusy)
		{
			this.InvokeSaveEvent();
			Save._saveData.SceneName = SceneManager.GetActiveScene().name;
			if (!forceIsNewGamePlus)
			{
				Save._saveData.PlayerPosition = BetterPlayerControl.Instance.transform.position;
				Save._saveData.PlayerRotation = BetterPlayerControl.Instance.transform.rotation;
				Save._saveData.TimeStamp = Save.GetFormattedTimeNow();
			}
			if (forceIsNewGamePlus)
			{
				Save._saveData.newGamePlus = forceIsNewGamePlus;
			}
			Save._saveData.inDialogue = Singleton<GameController>.Instance.viewState == VIEW_STATE.TALKING && !forceIsNewGamePlus;
			GameObject dateable = MovingDateable.GetDateable("MovingHelicopter", "landed");
			if (dateable != null && dateable.activeInHierarchy)
			{
				MovingDateable.MoveDateable("MovingHelicopter", "empty", true);
			}
			Save._saveData.characterToStagePosition = new NameToStagePosition();
			if (Save._saveData.inDialogue && Singleton<InkController>.Instance.IsInChoice())
			{
				Save._saveData.StoryJSON = Singleton<InkController>.Instance.GetPreDialogueState();
				PrevChoice prevChoice;
				if (Singleton<InkController>.Instance.GetLastChoiceSave(out prevChoice))
				{
					Save._saveData.DialogueState = prevChoice.prevState;
				}
				else
				{
					Save._saveData.DialogueState = Singleton<InkController>.Instance.GetState();
				}
				Save._saveData.lastDialogueDateable = Singleton<InkController>.Instance.GetInkKnotLoaded();
				Dictionary<string, string> saveData = Singleton<InkController>.Instance.stageManager.GetSaveData();
				using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = saveData.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						Save._saveData.characterToStagePosition.Add(text, saveData[text]);
					}
					goto IL_02CA;
				}
			}
			if (Save._saveData.inDialogue && !forceIsNewGamePlus)
			{
				Save._saveData.StoryJSON = Singleton<InkController>.Instance.GetPreDialogueState();
				Save._saveData.DialogueState = Singleton<InkController>.Instance.GetState();
				Save._saveData.lastDialogueDateable = Singleton<InkController>.Instance.GetInkKnotLoaded();
				Dictionary<string, string> saveData2 = Singleton<InkController>.Instance.stageManager.GetSaveData();
				using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = saveData2.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text2 = enumerator.Current;
						Save._saveData.characterToStagePosition.Add(text2, saveData2[text2]);
					}
					goto IL_02CA;
				}
			}
			if (!forceIsNewGamePlus)
			{
				Save._saveData.StoryJSON = Singleton<InkController>.Instance.GetState();
				Save._saveData.newGamePlusSpecsSnapshot = null;
			}
			IL_02CA:
			Save._saveData.objectSaveDataList.Clear();
			Save._saveData.objectSaveDataDictionary.Clear();
			if (!forceIsNewGamePlus)
			{
				foreach (InteractableObj interactableObj in Object.FindObjectsOfType(typeof(InteractableObj), true))
				{
					string text3 = interactableObj.Id;
					if (string.IsNullOrEmpty(interactableObj.Id))
					{
						text3 = interactableObj.objSaveData.gameObjectName;
					}
					ObjectSaveData objectSaveData;
					if (!Save._saveData.objectSaveDataDictionary.ContainsKey(text3))
					{
						if (interactableObj.objSaveData != null)
						{
							interactableObj.StoreSaveData();
							Save._saveData.objectSaveDataList.Add(new ObjectSaveData(text3, interactableObj.objSaveData.activeSelf, interactableObj.objSaveData.activatedAnimation, interactableObj.objSaveData.isClean, interactableObj.objSaveData.hasNormalInteracted, interactableObj.objSaveData.positionWhenInteracted));
							Save._saveData.objectSaveDataDictionary.Add(text3, interactableObj.objSaveData);
						}
						else
						{
							T17Debug.LogError("[save] interactable object " + interactableObj.name + " has a NULL SaveData object");
						}
					}
					else if (Save._saveData.objectSaveDataDictionary.TryGetValue(text3, out objectSaveData) && objectSaveData.activeSelf != interactableObj.gameObject.activeInHierarchy)
					{
						objectSaveData.activeSelf = interactableObj.gameObject.activeInHierarchy;
						Save._saveData.objectSaveDataDictionary[text3] = objectSaveData;
						int j = 0;
						int count = Save._saveData.objectSaveDataList.Count;
						while (j < count)
						{
							if (Save._saveData.objectSaveDataList[j].gameObjectName == text3)
							{
								Save._saveData.objectSaveDataList[j].activeSelf = interactableObj.gameObject.activeInHierarchy;
							}
							j++;
						}
					}
				}
			}
			Singleton<InkController>.Instance.story.variablesState["can_skip_tutorial"] = true;
			Services.GameSettings.SetInt(Save.SettingKeySkipTutorial, 1);
			SaveGameError saveGameError;
			if (!forceIsNewGamePlus)
			{
				saveGameError = await Services.SaveGameService.SaveGameplayDataAsync(saveSlotID, Save._saveData);
			}
			else
			{
				CharacterEntryToCollectableBoolMap charToCollectableMap = Save._saveData.charToCollectableMap;
				Save.PlayerData playerData = Save._saveData.PlayerData;
				string sceneName = Save._saveData.SceneName;
				BaseSpecsSnapshot baseSpecsSnapshot = BaseSpecsSnapshot.CreateFromCurrentStory();
				Singleton<InkStoryProvider>.Instance.Story.ResetState();
				this.SetInkVariables();
				Save._saveData = new Save.SaveData();
				Save._saveData.TimeStamp = Save.GetFormattedTimeNow();
				Save._saveData.dateviators_currentCharges = 5;
				Save._saveData.dayNightCycle_presentDayPresentTime = new DateTime(2024, 10, 1);
				Save._saveData.dayNightCycle_currentDayPhase = DayPhase.MORNING;
				Save._saveData.dayNightCycle_daysSinceStart = 0;
				Save._saveData.SceneName = sceneName;
				this.SafetyTimeOnGameLoad = (long)DateTime.UtcNow.Millisecond;
				this._pendingFinishedMessages.Clear();
				Save._saveData.newGamePlusSpecsSnapshot = baseSpecsSnapshot;
				Save._saveData.charToCollectableMap = charToCollectableMap;
				Save._saveData.PlayerData = playerData;
				Save._saveData.newGamePlus = true;
				saveGameError = await Services.SaveGameService.SaveGameplayDataAsync(saveSlotID, Save._saveData);
			}
			this.InvokeSaveCompletedEvent(saveGameError);
		}
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0002243C File Offset: 0x0002063C
	public async Task DeleteGameAsync(int saveSlotID)
	{
		if (!Services.SaveGameService.IsBusy)
		{
			await Services.SaveGameService.DeleteGameplayDataAsync(saveSlotID);
		}
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0002247F File Offset: 0x0002067F
	public void CallGameLoad()
	{
		Save.OnGameLoad onGameLoad = Save.onGameLoad;
		if (onGameLoad == null)
		{
			return;
		}
		onGameLoad();
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00022490 File Offset: 0x00020690
	public void TemporaryUnlockAwakening(string _name)
	{
		if (Singleton<Save>.Instance.GetDateStatus(_name) == RelationshipStatus.Unmet)
		{
			Singleton<Save>.Instance.SetDateStatus(_name, RelationshipStatus.Single);
			Save._saveData.StatusToBeReverted.Add(_name);
		}
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x000224BC File Offset: 0x000206BC
	public void RevertTemporaryAwakening()
	{
		if (Save._saveData.StatusToBeReverted != null)
		{
			foreach (string text in Save._saveData.StatusToBeReverted)
			{
				Singleton<Save>.Instance.SetDateStatus(text, RelationshipStatus.Unmet);
			}
			Save._saveData.StatusToBeReverted.Clear();
		}
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00022534 File Offset: 0x00020734
	public async Task LoadGameAsync(int saveSlotID)
	{
		if (Singleton<DeluxeEditionController>.Instance != null)
		{
			Singleton<DeluxeEditionController>.Instance.UpdateEntitlements();
		}
		while (Services.SaveGameService.IsBusy)
		{
			await Task.Yield();
		}
		string saveString = string.Empty;
		SceneTransitionManager.TransitionToScene("");
		ref SaveResult<bool> ptr = await Services.SaveGameService.DoesGameplaySlotExistAsync(saveSlotID);
		uint num = Save.DataVersion.Latest;
		if (!ptr.Result)
		{
			if (!Save.TryLegacyLoad(saveSlotID, ref saveString))
			{
				T17Debug.LogError(string.Format("[Save] failed to load the save game as specified slot {0} does not exist", saveSlotID));
				return;
			}
		}
		else
		{
			SaveResult<string> saveResult = await Services.SaveGameService.LoadGameplayDataAsync(saveSlotID);
			if (saveResult.Error != SaveGameErrorType.None || string.IsNullOrEmpty(saveResult.Result))
			{
				return;
			}
			saveString = saveResult.Result;
			num = saveResult.Version;
		}
		Save.SaveData saveData;
		if (num == Save.DataVersion.Latest || num == 1U || num == 3U || num == 4294967295U)
		{
			saveData = Save.GetSaveData(saveString, num);
		}
		else if (num == 2U)
		{
			saveData = Save.GetSaveData(saveString, num);
		}
		else
		{
			saveData = Save.GetSaveData(saveString, num);
		}
		if (saveData.AllChatHistory == null || saveData.AllChatHistory.Count == 0)
		{
			saveData.AllChatHistory = new List<Save.AppMessage>();
			saveData.AllChatHistory.AddRange(this.ConvertAppMessagesNewLayout(saveData.thiscordChatHistory, ChatType.Thiscord));
			saveData.AllChatHistory.AddRange(this.ConvertAppMessagesNewLayout(saveData.workspaceChatHistory, ChatType.Wrkspce));
			saveData.AllChatHistory.AddRange(this.ConvertAppMessagesNewLayout(saveData.canopyChatHistory, ChatType.Canopy));
		}
		saveData.StatusToBeReverted = new List<string>();
		Save._saveData = saveData;
		this.OnGameLoaded();
		Save.OnGameLoad onGameLoad = Save.onGameLoad;
		if (onGameLoad != null)
		{
			onGameLoad();
		}
		this.SafetyTimeOnGameLoad = (long)DateTime.UtcNow.AddSeconds(5.0).Millisecond;
		Save._saveData.Load();
		Save._saveData.objectSaveDataDictionary.Clear();
		foreach (ObjectSaveData objectSaveData in Save.GetSaveData(false).objectSaveDataList)
		{
			Save._saveData.objectSaveDataDictionary.Add(objectSaveData.gameObjectName, new ObjectSaveData(objectSaveData.gameObjectName, objectSaveData.activeSelf, objectSaveData.activatedAnimation, objectSaveData.isClean, objectSaveData.hasNormalInteracted, objectSaveData.positionWhenInteracted));
		}
		CanvasUIManager.SwitchMenu("HUD");
		SceneManager.sceneLoaded += Save.OnSceneLoaded;
		Services.ActivitiesService.ResetToDefault();
		this._uiUtilities.LoadSceneAsyncSingle(Save._saveData.SceneName, false);
		this.InitiateRoomersData();
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00022580 File Offset: 0x00020780
	private List<Save.AppMessage> ConvertAppMessagesNewLayout(List<Save.AppMessage> appMessages, ChatType type)
	{
		foreach (Save.AppMessage appMessage in appMessages)
		{
			appMessage.ChatType = type;
			appMessage.NodePrefix = Singleton<InkController>.Instance.GetStoryAppPrefix(appMessage.Node);
			Save.AppMessageStitch appMessageStitch = new Save.AppMessageStitch();
			appMessageStitch.Stitch = appMessage.Node;
			appMessageStitch.Status = appMessage.Status;
			appMessageStitch.Ended = true;
			appMessageStitch.ProcessedEndedCondition = true;
			foreach (string text in appMessage.ChatHistory)
			{
				if (text != "" && text.StartsWith(appMessageStitch.Stitch))
				{
					appMessageStitch.ChatHistoryOptionsSelected.Add(text);
				}
				else if (text != "")
				{
					appMessage.StitchHistory.Add(appMessageStitch);
					appMessageStitch = new Save.AppMessageStitch();
					appMessageStitch.Stitch = text;
					appMessageStitch.Status = appMessage.Status;
					appMessageStitch.Ended = true;
					appMessageStitch.ProcessedEndedCondition = true;
				}
			}
			if (appMessage.GetLastStitch() != appMessageStitch.Stitch)
			{
				appMessage.StitchHistory.Add(appMessageStitch);
			}
		}
		return appMessages;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x000226F8 File Offset: 0x000208F8
	public bool SafetyCheckOnGameLoad()
	{
		return this.SafetyTimeOnGameLoad == 0L || (long)DateTime.UtcNow.Millisecond > this.SafetyTimeOnGameLoad;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00022728 File Offset: 0x00020928
	protected static bool TryLegacyLoad(int saveSlotID, ref string saveString)
	{
		string text = "/save" + saveSlotID.ToString() + ".json";
		string text2 = Save._saveDirectory + text;
		if (Save.LegacySaveExists(saveSlotID))
		{
			saveString = File.ReadAllText(text2);
			return true;
		}
		return false;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0002276C File Offset: 0x0002096C
	public static bool LegacySaveExists(int saveSlotID)
	{
		string text = "/save" + saveSlotID.ToString() + ".json";
		return File.Exists(Save._saveDirectory + text);
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x000227A8 File Offset: 0x000209A8
	private static void TreatAppHistoryInSave(Save.SaveData saveData)
	{
		foreach (Save.AppMessage appMessage in saveData.AllChatHistory)
		{
			while (saveData.DialogueState.Contains(appMessage.NodePrefix))
			{
				saveData.DialogueState = Save.RemoveAppHistory(saveData.DialogueState, appMessage.NodePrefix);
			}
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00022820 File Offset: 0x00020A20
	private static string RemoveAppHistory(string json, string node)
	{
		string text = json.Substring(0, json.IndexOf(node) - 1);
		string text2 = json.Substring(json.IndexOf(node));
		text2 = text2.Substring(text2.IndexOf(",") + 1);
		return text + text2;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00022868 File Offset: 0x00020A68
	public void InitiateRoomersData()
	{
		if (Save._saveData.roomers == null)
		{
			Save._saveData.roomers = new List<Save.RoomersStruct>();
		}
		foreach (string text in Singleton<CharacterHelper>.Instance._characters)
		{
			if (text != "dipodgenes" && text != "hank1" && text != "hank2" && text != "hank3" && text != "hank4" && text != "hank5" && text != "deenah" && text != "volt" && text != "katie" && text != "lauren" && text != "lint" && text != "tresta" && text != "curt" && text != "rod")
			{
				Singleton<InkController>.Instance.JumpToKnot("roomers.roomer_" + text);
				if (Singleton<InkController>.Instance.story.state.currentPathString.Contains("roomers.roomer_" + text) && Singleton<InkController>.Instance.story.canContinue)
				{
					Singleton<InkController>.Instance.ContinueStory();
					string text2 = "Title";
					string text3 = "No description.";
					string text4 = "";
					string text5 = "Clue ";
					Save.RoomersStruct roomersStruct = new Save.RoomersStruct(text, false);
					roomersStruct.tips = new List<Save.RoomersTipStruct>();
					foreach (string text6 in Singleton<InkController>.Instance.story.currentTags)
					{
						if (text6.StartsWith("Title:"))
						{
							text2 = text6.Substring(text6.IndexOf("Title:") + 6).Trim();
						}
						else if (text6.StartsWith("Desc:"))
						{
							text3 = text6.Substring(text6.IndexOf("Desc:") + 5).Trim();
						}
						else if (text6.StartsWith("Clue"))
						{
							string text7 = text6.Substring(text6.IndexOf("Clue") + 6).Trim();
							if (text7.StartsWith(":"))
							{
								text7 = text7.Substring(1).Trim();
							}
							roomersStruct.tips.Add(new Save.RoomersTipStruct(text5, text7, false));
						}
						else if (text6.StartsWith("Skylar:"))
						{
							text4 = text6.Substring(text6.IndexOf("Skylar:") + 7).Trim();
						}
						else if (text6.StartsWith("Name"))
						{
							text5 = text6.Substring(text6.IndexOf("Name") + 6).Trim();
							if (text5.StartsWith(":"))
							{
								text5 = text5.Substring(1).Trim();
							}
						}
					}
					if (text2 != "")
					{
						roomersStruct.character = text;
						roomersStruct.questName = text2;
						roomersStruct.description = text3;
						roomersStruct.skylar = text4;
						roomersStruct.isFound = this.GetDateStatus(text) > RelationshipStatus.Unmet;
						bool flag = false;
						foreach (Save.RoomersStruct roomersStruct2 in Save._saveData.roomers)
						{
							if (roomersStruct2.character == text)
							{
								roomersStruct2.questName = text2;
								roomersStruct2.description = text3;
								flag = true;
							}
						}
						if (!flag)
						{
							Save._saveData.roomers.Add(roomersStruct);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00022C9C File Offset: 0x00020E9C
	public float GetVoiceOverVolume()
	{
		return Services.GameSettings.GetFloat("voiceVolume", 1f);
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00022CB2 File Offset: 0x00020EB2
	public static Save.SaveData GetSaveData(bool suppressWarning = false)
	{
		if (Save._saveData != null)
		{
			return Save._saveData;
		}
		return null;
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x00022CC4 File Offset: 0x00020EC4
	public static async Task<Save.SaveData> GetSaveData(int saveSlotID)
	{
		TaskAwaiter<SaveResult<bool>> taskAwaiter = Services.SaveGameService.DoesGameplaySlotExistAsync(saveSlotID).GetAwaiter();
		if (!taskAwaiter.IsCompleted)
		{
			await taskAwaiter;
			TaskAwaiter<SaveResult<bool>> taskAwaiter2;
			taskAwaiter = taskAwaiter2;
			taskAwaiter2 = default(TaskAwaiter<SaveResult<bool>>);
		}
		Save.SaveData saveData;
		if (!taskAwaiter.GetResult().Result)
		{
			string empty = string.Empty;
			if (Save.TryLegacyLoad(saveSlotID, ref empty))
			{
				saveData = Save.GetSaveData(empty, 0U);
			}
			else
			{
				saveData = null;
			}
		}
		else
		{
			SaveResult<string> saveResult = await Services.SaveGameService.LoadGameplayDataAsync(saveSlotID);
			if (saveResult.Error == SaveGameErrorType.None)
			{
				saveData = Save.GetSaveData(saveResult.Result, saveResult.Version);
			}
			else
			{
				saveData = null;
			}
		}
		return saveData;
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x00022D08 File Offset: 0x00020F08
	public static Save.SaveData LegacyGetSaveData(int saveSlotID)
	{
		string text = "/save" + saveSlotID.ToString() + ".json";
		string text2 = Save._saveDirectory + text;
		if (Save.LegacySaveExists(saveSlotID))
		{
			return JsonUtility.FromJson<Save.SaveData>(File.ReadAllText(text2));
		}
		return null;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x00022D4D File Offset: 0x00020F4D
	public static Save.SaveData GetSaveData(string json, uint version)
	{
		if (version >= 2U)
		{
			return JsonConvert.DeserializeObject<Save.SaveData>(json);
		}
		return JsonUtility.FromJson<Save.SaveData>(json);
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00022D60 File Offset: 0x00020F60
	public static void AutoSaveGame()
	{
		if (DeluxeEditionController.IS_DEMO_EDITION || BetterPlayerControl.Instance.isGameEndingOn)
		{
			return;
		}
		if (!Services.PlatformService.SaveOperationRateLimiter.CanMakeRequest(Time.unscaledTime))
		{
			return;
		}
		Save.GetSaveData(false).AdvanceAutoSaveSlot();
		Save.autoSaveSlots[Save.GetSaveData(false).autoSaveSlot].TrySaveGameDataAsync(false, false, null, false);
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x00022DC0 File Offset: 0x00020FC0
	public bool HasAutoSaveSlots()
	{
		foreach (SaveSlot saveSlot in Save.autoSaveSlots)
		{
			if (saveSlot != null && saveSlot.IsAutoSave)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00022DFC File Offset: 0x00020FFC
	public static bool SaveExists(int saveSlotID)
	{
		SaveSlotMetadata slotInfo = Services.SaveGameService.GetSlotInfo(saveSlotID);
		if (slotInfo == null)
		{
			return Save.LegacySaveExists(saveSlotID);
		}
		return slotInfo != null;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00022E23 File Offset: 0x00021023
	public List<Save.AppMessage> GetPlayerThiscordVisited()
	{
		return this.GetChatHistoryPerType(ChatType.Thiscord);
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00022E2C File Offset: 0x0002102C
	private List<Save.AppMessage> GetChatHistoryPerType(ChatType type)
	{
		return Save._saveData.AllChatHistory.FindAll((Save.AppMessage x) => x.ChatType == type).ToList<Save.AppMessage>();
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00022E66 File Offset: 0x00021066
	public List<Save.AppMessage> GetPlayerCanopyVisited()
	{
		return this.GetChatHistoryPerType(ChatType.Canopy);
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00022E6F File Offset: 0x0002106F
	public List<Save.AppMessage> GetPlayerWorkspaceVisited()
	{
		return this.GetChatHistoryPerType(ChatType.Wrkspce);
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00022E78 File Offset: 0x00021078
	public List<string> GetPlayerHistoryForStitch(string stitch)
	{
		foreach (Save.AppMessage appMessage in Save._saveData.AllChatHistory)
		{
			if (stitch.StartsWith(appMessage.NodePrefix))
			{
				foreach (Save.AppMessageStitch appMessageStitch in appMessage.StitchHistory)
				{
					if (appMessageStitch.Stitch == stitch)
					{
						return appMessageStitch.ChatHistoryOptionsSelected;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00022F30 File Offset: 0x00021130
	public string GetLastStitchFromHistory(List<string> history)
	{
		for (int i = history.Count - 1; i >= 0; i--)
		{
			if (history[i] != "")
			{
				return history[i];
			}
		}
		return null;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00022F6C File Offset: 0x0002116C
	public Save.AppMessage GetPlayerAppMessageVisited(string node)
	{
		foreach (Save.AppMessage appMessage in Save._saveData.AllChatHistory)
		{
			if (node.StartsWith(appMessage.NodePrefix))
			{
				return appMessage;
			}
		}
		return null;
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00022FD4 File Offset: 0x000211D4
	public void AddAppMessage(Save.AppMessage appMessage)
	{
		foreach (Save.AppMessage appMessage2 in Save._saveData.AllChatHistory)
		{
			if (appMessage2.NodePrefix == appMessage.NodePrefix)
			{
				appMessage2.ChatHistory.AddRange(appMessage.ChatHistory);
				appMessage2.NextStitches.AddRange(appMessage.NextStitches);
				return;
			}
		}
		Save._saveData.AllChatHistory.Add(appMessage);
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0002306C File Offset: 0x0002126C
	public bool AddPlayerAppMessageStep(string node, string stepChoice)
	{
		if (stepChoice == null || stepChoice == "")
		{
			return false;
		}
		for (int i = 0; i < Save._saveData.AllChatHistory.Count; i++)
		{
			if (node.StartsWith(Save._saveData.AllChatHistory[i].NodePrefix))
			{
				foreach (Save.AppMessageStitch appMessageStitch in Save._saveData.AllChatHistory[i].StitchHistory)
				{
					if (appMessageStitch.Stitch == node)
					{
						if (!appMessageStitch.ChatHistoryOptionsSelected.Contains(stepChoice))
						{
							appMessageStitch.ChatHistoryOptionsSelected.Add(stepChoice);
							return false;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0002314C File Offset: 0x0002134C
	public void SetDateableTalkedTo(string dateableInternalName)
	{
		if (!Save._saveData.charToLastDayTalkedTo.ContainsKey(dateableInternalName))
		{
			Save._saveData.charToLastDayTalkedTo.Add(dateableInternalName, Singleton<DayNightCycle>.Instance.GetDaysSinceStart());
			return;
		}
		Save._saveData.charToLastDayTalkedTo[dateableInternalName] = Singleton<DayNightCycle>.Instance.GetDaysSinceStart();
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x000231A0 File Offset: 0x000213A0
	public void ResetDateableTalkedTo(string dateableInternalName)
	{
		if (!Save._saveData.charToLastDayTalkedTo.ContainsKey(dateableInternalName))
		{
			Save._saveData.charToLastDayTalkedTo.Add(dateableInternalName, Singleton<DayNightCycle>.Instance.GetDaysSinceStart() - 1);
			return;
		}
		Save._saveData.charToLastDayTalkedTo[dateableInternalName] = Singleton<DayNightCycle>.Instance.GetDaysSinceStart() - 1;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x000231F8 File Offset: 0x000213F8
	public void SetShrinkAmount(float shrinkAmount)
	{
		Save._saveData.shrinkAmount = shrinkAmount;
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00023205 File Offset: 0x00021405
	public int GetDateableTalkedTo(string dateableInternalName)
	{
		if (Save._saveData.charToLastDayTalkedTo.ContainsKey(dateableInternalName))
		{
			return Save._saveData.charToLastDayTalkedTo[dateableInternalName];
		}
		return -1;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0002322B File Offset: 0x0002142B
	public void SetInteractableState(string interactable, bool isActiveState)
	{
		if (isActiveState)
		{
			if (!Save._saveData.interactablesStates.Contains(interactable))
			{
				Save._saveData.interactablesStates.Add(interactable);
				return;
			}
		}
		else
		{
			Save._saveData.interactablesStates.Remove(interactable);
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00023264 File Offset: 0x00021464
	public bool HasInteractableState(string interactable)
	{
		return Save._saveData.interactablesStates.Contains(interactable);
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00023276 File Offset: 0x00021476
	public bool GetInteractableState(string interactable)
	{
		return Save._saveData.interactablesStates.Contains(interactable);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00023288 File Offset: 0x00021488
	public void SetTutorialThresholdState(string item)
	{
		if (!Save._saveData.tutorialThresholdStates.Contains(item))
		{
			Save._saveData.tutorialThresholdStates.Add(item);
			if (item == TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO)
			{
				Singleton<InkController>.Instance.story.variablesState["skylar_intro_done"] = true;
			}
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x000232E3 File Offset: 0x000214E3
	public void RemoveTutorialThresholdState(string item)
	{
		if (Save._saveData.tutorialThresholdStates.Contains(item))
		{
			Save._saveData.tutorialThresholdStates.Remove(item);
		}
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00023308 File Offset: 0x00021508
	public bool GetTutorialThresholdState(string item)
	{
		return Save._saveData.tutorialThresholdStates.Contains(item);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0002331A File Offset: 0x0002151A
	public string GetPlayerName()
	{
		return Save._saveData.PlayerData.Name;
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0002332B File Offset: 0x0002152B
	public string GetPlayerTown()
	{
		return Save._saveData.PlayerData.TownName;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0002333C File Offset: 0x0002153C
	public int GetPlayerGender()
	{
		return Save._saveData.PlayerData.Pronouns;
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0002334D File Offset: 0x0002154D
	public void SetPlayerName(string name)
	{
		Save._saveData.PlayerData.Name = name;
		Save._saveData.SetPlayerNameInk();
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00023369 File Offset: 0x00021569
	public void SetPlayerFavoriteThing(string favoriteThing)
	{
		Save._saveData.PlayerData.FavoriteThing = favoriteThing;
		Save._saveData.SetPlayerFavoriteThingInk();
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00023385 File Offset: 0x00021585
	public void SetPlayerTown(string town)
	{
		Save._saveData.PlayerData.TownName = town;
		Save._saveData.SetPlayerTownInk();
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x000233A4 File Offset: 0x000215A4
	public void SetPlayerBirthday(string date)
	{
		DateTime dateTime;
		if (DateTime.TryParse(date, out dateTime))
		{
			Save._saveData.PlayerData.BirthdaydMonth = dateTime.Month;
			Save._saveData.PlayerData.BirthdaydDay = dateTime.Day;
			Save._saveData.PlayerData.BirthdaydYear = dateTime.Year;
			Save._saveData.SetPlayerBirthdayInk();
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00023407 File Offset: 0x00021607
	public void SetPlayerPronouns(int pronouns)
	{
		Save._saveData.PlayerData.Pronouns = pronouns;
		Save._saveData.SetPlayerPronounsInk();
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00023423 File Offset: 0x00021623
	public void SetPhoneBackgroundIndex(int index)
	{
		Save._saveData.phoneBackgroundIndex = index;
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00023430 File Offset: 0x00021630
	public int GetPhoneBackgroundIndex()
	{
		return Save._saveData.phoneBackgroundIndex;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0002343C File Offset: 0x0002163C
	public void SetInkVariables()
	{
		Save._saveData.SetPlayerNameInk();
		Save._saveData.SetPlayerPronounsInk();
		Save._saveData.SetPlayerBirthdayInk();
		Save._saveData.SetPlayerTownInk();
		Save._saveData.SetPlayerFavoriteThingInk();
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x00023470 File Offset: 0x00021670
	public bool MeetDatableIfUnmet(string name)
	{
		return this.GetDateStatus(name) == RelationshipStatus.Unmet;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0002347E File Offset: 0x0002167E
	public void MeetDatable(string name)
	{
		this.SetDateStatus(DateADex.Instance.GetCharIndex(name), RelationshipStatus.Single);
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00023494 File Offset: 0x00021694
	public void SetDateStatus(string name, RelationshipStatus status)
	{
		this.SetDateStatus(DateADex.Instance.GetCharIndex(name), status);
		if (status != RelationshipStatus.Unmet)
		{
			if (name != "skylar")
			{
				this.SetRoomerFound(name);
				return;
			}
			if (name == "skylar" && Singleton<InkController>.Instance.story.variablesState["skylar_roomer"].ToString() == "complete")
			{
				this.SetRoomerFound(name);
			}
		}
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0002350C File Offset: 0x0002170C
	public int AddBoxExamenData(string boxStitch)
	{
		if (!Save._saveData.boxExamenDictionary.ContainsKey(boxStitch))
		{
			int num = Save._saveData.boxExamenDictionary.Keys.Count + 1;
			Save._saveData.boxExamenDictionary.Add(boxStitch, num);
			return num;
		}
		return Save._saveData.boxExamenDictionary[boxStitch];
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x00023565 File Offset: 0x00021765
	public Dictionary<string, int> GetBoxExamenData()
	{
		return Save._saveData.boxExamenDictionary;
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00023574 File Offset: 0x00021774
	public void SetDateStatus(int id, RelationshipStatus status)
	{
		if (id >= 0)
		{
			if (status == RelationshipStatus.Single && Save._saveData.datestates[id] == 0)
			{
				Save.onAwakening();
			}
			if (status == RelationshipStatus.Realized)
			{
				Save._saveData.datestatesRealized[id] = (int)status;
				Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.REALIZE);
				return;
			}
			Save._saveData.datestates[id] = (int)status;
			this.CheckForAchievements(status);
		}
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x000235D3 File Offset: 0x000217D3
	private void CheckForAchievements(RelationshipStatus status)
	{
		if (status == RelationshipStatus.Single)
		{
			Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.AWAKEN_DATEABLE);
		}
		if (status == RelationshipStatus.Love || status == RelationshipStatus.Hate || status == RelationshipStatus.Friend)
		{
			Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.ENDINGS);
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x000235FD File Offset: 0x000217FD
	public int[] GetDateablesStatus()
	{
		return Save._saveData.datestates;
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00023609 File Offset: 0x00021809
	public int[] GetDateablesStatusRealized()
	{
		return Save._saveData.datestatesRealized;
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00023615 File Offset: 0x00021815
	public string[] GetDateablesInternalNames()
	{
		return Singleton<CharacterHelper>.Instance._characters.ToArray<string>();
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x00023626 File Offset: 0x00021826
	public int TotalMetDatables()
	{
		return Save._saveData.datestates.Count((int x) => x != 0);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x00023656 File Offset: 0x00021856
	public int TotalFriendEndings()
	{
		return Save._saveData.datestates.Count((int x) => x == 3);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00023686 File Offset: 0x00021886
	public int TotalLoveEndings()
	{
		return Save._saveData.datestates.Count((int x) => x == 2);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x000236B6 File Offset: 0x000218B6
	public int TotalHateEndings()
	{
		return Save._saveData.datestates.Count((int x) => x == -1);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x000236E6 File Offset: 0x000218E6
	public int TotalDatables()
	{
		return Save._saveData.datestates.Count<int>();
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x000236F7 File Offset: 0x000218F7
	public int TotalRealizedDatables()
	{
		return Save._saveData.datestatesRealized.Count((int x) => x == 4);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00023727 File Offset: 0x00021927
	public int AvailableTotalMetDatables()
	{
		return this.GetAvailableDateStates().Count((int x) => x != 0);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00023753 File Offset: 0x00021953
	public int AvailableTotalFriendEndings()
	{
		return this.GetAvailableDateStates().Count((int x) => x == 3);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0002377F File Offset: 0x0002197F
	public int AvailableTotalLoveEndings()
	{
		return this.GetAvailableDateStates().Count((int x) => x == 2);
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000237AB File Offset: 0x000219AB
	public int AvailableTotalHateEndings()
	{
		return this.GetAvailableDateStates().Count((int x) => x == -1);
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000237D7 File Offset: 0x000219D7
	public int AvailableTotalRealizedDatables()
	{
		return this.GetAvailableDateablesRealized().Count((int x) => x == 4);
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00023803 File Offset: 0x00021A03
	public int AvailableTotalDatables()
	{
		if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			return this.TotalDatables();
		}
		return this.TotalDatables() - DeluxeEditionController.NUMBER_OF_DELUXE_CHARACTERS;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00023824 File Offset: 0x00021A24
	private IEnumerable<int> GetAvailableDateStates()
	{
		return Save._saveData.datestates.Take(this.AvailableTotalDatables());
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002383B File Offset: 0x00021A3B
	private IEnumerable<int> GetAvailableDateablesRealized()
	{
		return Save._saveData.datestatesRealized.Take(this.AvailableTotalDatables());
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00023852 File Offset: 0x00021A52
	public RelationshipStatus GetDateStatus(string name)
	{
		if (DateADex.Instance != null)
		{
			return this.GetDateStatus(DateADex.Instance.GetCharIndex(name));
		}
		return RelationshipStatus.Unmet;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00023874 File Offset: 0x00021A74
	public RelationshipStatus GetDateStatusRealized(string name)
	{
		if (DateADex.Instance != null)
		{
			return this.GetDateStatusRealized(DateADex.Instance.GetCharIndex(name));
		}
		return RelationshipStatus.Unmet;
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00023898 File Offset: 0x00021A98
	public string GetAllDateStatus()
	{
		StringBuilder stringBuilder = new StringBuilder("");
		for (int i = 0; i < Save._saveData.datestates.Length; i++)
		{
			int num = Save._saveData.datestates[i];
			if (num < 0)
			{
				num = 5;
			}
			if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION || i + 1 < Save._saveData.datestates.Length)
			{
				stringBuilder.Append(num);
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00023905 File Offset: 0x00021B05
	public RelationshipStatus GetDateStatus(int id)
	{
		if (id < 0)
		{
			return RelationshipStatus.Unmet;
		}
		return (RelationshipStatus)Save._saveData.datestates[id];
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00023919 File Offset: 0x00021B19
	public RelationshipStatus GetDateStatusRealized(int id)
	{
		if (id < 0)
		{
			return RelationshipStatus.Unmet;
		}
		return (RelationshipStatus)Save._saveData.datestatesRealized[id];
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00023930 File Offset: 0x00021B30
	public void UnlockCollectable(string internalName, int collectableNumber)
	{
		if (Save._saveData.charToCollectableMap[internalName].ContainsKey(collectableNumber))
		{
			Save._saveData.charToCollectableMap[internalName][collectableNumber] = true;
		}
		else
		{
			Save._saveData.charToCollectableMap[internalName].Add(collectableNumber, true);
		}
		Services.StatsService.OnUnlockedCollectable(internalName, collectableNumber);
		Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.COLLECTABLE);
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x000239A0 File Offset: 0x00021BA0
	private void FixCharToCollectableMap(string internalName)
	{
		if (Save._saveData.charToCollectableMap[internalName].Count <= 3)
		{
			Sprite[] array = Services.AssetProviderService.LoadResourceAssets<Sprite>(Path.Combine("Images", "Collectables", internalName));
			int num = 1;
			foreach (Sprite sprite in array)
			{
				if (!Save._saveData.charToCollectableMap[internalName].ContainsKey(num))
				{
					Save._saveData.charToCollectableMap[internalName][num] = false;
				}
				num++;
			}
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00023A28 File Offset: 0x00021C28
	public int GetDateableCollectablesNumber(string internalName)
	{
		this.FixCharToCollectableMap(internalName);
		int num = 0;
		foreach (KeyValuePair<int, bool> keyValuePair in Save._saveData.charToCollectableMap[internalName])
		{
			if (keyValuePair.Value)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00023A90 File Offset: 0x00021C90
	public CollectableToBool GetDateableCollectables(string internalName)
	{
		this.FixCharToCollectableMap(internalName);
		return Save._saveData.charToCollectableMap[internalName];
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00023AA9 File Offset: 0x00021CA9
	public bool IsCollectableUnlockedByName(string characterName, int collectableNumber)
	{
		return this.IsCollectableUnlocked(Singleton<CharacterHelper>.Instance._characters.GetInternalName(characterName), collectableNumber);
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00023AC4 File Offset: 0x00021CC4
	public bool IsCollectableUnlocked(string internalName, int collectableNumber)
	{
		bool flag;
		return Save._saveData.charToCollectableMap[internalName].TryGetValue(collectableNumber, out flag) && flag;
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00023AEE File Offset: 0x00021CEE
	public int GetTotalCollectables(bool addDeluxeEdition)
	{
		if (addDeluxeEdition)
		{
			return 409;
		}
		return 404;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00023B00 File Offset: 0x00021D00
	public int GetTotalUnlockedCollectables(bool addDeluxeEdition)
	{
		int num = 0;
		foreach (KeyValuePair<string, CollectableToBool> keyValuePair in Save._saveData.charToCollectableMap)
		{
			if (addDeluxeEdition || (keyValuePair.Key != DeluxeEditionController.DELUXE_CHARACTER_1 && keyValuePair.Key != DeluxeEditionController.DELUXE_CHARACTER_2))
			{
				foreach (KeyValuePair<int, bool> keyValuePair2 in keyValuePair.Value)
				{
					if (keyValuePair2.Value)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00023BBC File Offset: 0x00021DBC
	public static string OutputCollectables()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<string, CollectableToBool> keyValuePair in Save._saveData.charToCollectableMap)
		{
			num2++;
			if (keyValuePair.Key == DeluxeEditionController.DELUXE_CHARACTER_1 || keyValuePair.Key == DeluxeEditionController.DELUXE_CHARACTER_2)
			{
				stringBuilder.AppendLine("[" + keyValuePair.Key + "]");
			}
			else
			{
				stringBuilder.AppendLine(keyValuePair.Key);
			}
			foreach (KeyValuePair<int, bool> keyValuePair2 in keyValuePair.Value)
			{
				stringBuilder.Append(string.Format("  {0} = ", keyValuePair2.Key));
				if (keyValuePair2.Value)
				{
					stringBuilder.Append("COLLECTED");
					num++;
				}
				stringBuilder.Append("\n");
			}
		}
		stringBuilder.AppendLine(string.Format("\nTotal Chars {0}", num2));
		stringBuilder.AppendLine(string.Format("\nTotal Collectables {0}", num));
		return stringBuilder.ToString();
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00023D20 File Offset: 0x00021F20
	public void ResetCollectables()
	{
		foreach (KeyValuePair<string, CollectableToBool> keyValuePair in Save._saveData.charToCollectableMap)
		{
			Save._saveData.charToCollectableMap[keyValuePair.Key].Clear();
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00023D88 File Offset: 0x00021F88
	public void UnlockDexEntry(string internalName, int dexNumber)
	{
		if (Save._saveData.charToDexEntriesMap[internalName].ContainsKey(dexNumber))
		{
			Save._saveData.charToDexEntriesMap[internalName][dexNumber] = true;
		}
		else
		{
			Save._saveData.charToDexEntriesMap[internalName].Add(dexNumber, true);
		}
		if (!Save._saveData.charactersWithNewStatusOnDateADex.Contains(internalName))
		{
			Save._saveData.charactersWithNewStatusOnDateADex.Add(internalName);
		}
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00023DFF File Offset: 0x00021FFF
	public CollectableToBool GetDexEntriesUnlocked(string internalName)
	{
		return Save._saveData.charToDexEntriesMap[internalName];
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00023E11 File Offset: 0x00022011
	public bool GetCharactersWithNewStatusOnDateADex(string internalName)
	{
		return Save._saveData.charactersWithNewStatusOnDateADex.Contains(internalName);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00023E23 File Offset: 0x00022023
	public void RemoveCharactersWithNewStatusOnDateADex(string internalName)
	{
		Save._saveData.charactersWithNewStatusOnDateADex.Remove(internalName);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00023E36 File Offset: 0x00022036
	public List<Save.RoomersStruct> GetRoomers()
	{
		return Save._saveData.roomers;
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00023E42 File Offset: 0x00022042
	public List<Save.RoomersStruct> GetRoomersFound()
	{
		return Save._saveData.roomers.Where((Save.RoomersStruct x) => x.isFound).ToList<Save.RoomersStruct>();
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00023E78 File Offset: 0x00022078
	public void SetRoomerFound(string character)
	{
		foreach (Save.RoomersStruct roomersStruct in Save._saveData.roomers)
		{
			if (character == roomersStruct.character)
			{
				roomersStruct.isFound = true;
				break;
			}
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00023EE0 File Offset: 0x000220E0
	public bool TryGetInternalName(string nameToCheck, out string internalName)
	{
		string text = nameToCheck.ToLowerInvariant().Trim();
		if (string.IsNullOrEmpty(text))
		{
			internalName = "";
			return false;
		}
		if (!Singleton<CharacterHelper>.Instance._characters.IsNameInSet(text))
		{
			internalName = Singleton<CharacterHelper>.Instance._characters.GetInternalName(text);
			return !(internalName == text);
		}
		internalName = text;
		return true;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x00023F40 File Offset: 0x00022140
	public bool TryGetNameByInternalName(string internalNameToCheck, out string name)
	{
		string text = internalNameToCheck.ToLowerInvariant().Trim();
		if (string.IsNullOrEmpty(text))
		{
			name = "";
			return false;
		}
		if (!Singleton<CharacterHelper>.Instance._characters.IsGlobalNameInSet(text))
		{
			name = Singleton<CharacterHelper>.Instance._characters.GetInternalName(text);
			return !(name == text);
		}
		name = text;
		return true;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00023FA0 File Offset: 0x000221A0
	public Save.RoomersStruct GetRoomer(string character)
	{
		foreach (Save.RoomersStruct roomersStruct in Save._saveData.roomers)
		{
			if (character == roomersStruct.character)
			{
				roomersStruct.isFound = this.GetDateStatus(character) > RelationshipStatus.Unmet;
				return roomersStruct;
			}
		}
		return new Save.RoomersStruct(character, this.GetDateStatus(character) > RelationshipStatus.Unmet);
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00024024 File Offset: 0x00022224
	public void AddRoomerTip(string character, string clue)
	{
		string text = "";
		bool flag = Singleton<Save>.Instance.TryGetInternalName(character, out text);
		if (clue.Trim() == "skylar")
		{
			using (List<Save.RoomersStruct>.Enumerator enumerator = Save._saveData.roomers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Save.RoomersStruct roomersStruct = enumerator.Current;
					if (character == roomersStruct.character || (flag && text == roomersStruct.character))
					{
						roomersStruct.skylarTipIsFound = true;
					}
				}
				return;
			}
		}
		int num = int.Parse(clue.ToLowerInvariant().Replace("clue", "").Trim());
		foreach (Save.RoomersStruct roomersStruct2 in Save._saveData.roomers)
		{
			if ((character == roomersStruct2.character || (flag && text == roomersStruct2.character)) && num > 0)
			{
				roomersStruct2.hasNew = true;
				if (roomersStruct2.tips.Count > num - 1)
				{
					roomersStruct2.tips[num - 1].isFound = true;
				}
			}
		}
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x0002417C File Offset: 0x0002237C
	public void SetNewGamePlus(bool finished)
	{
		Save._saveData.newGamePlus = finished;
		if (finished)
		{
			Singleton<Dateviators>.Instance.ResetCharges();
			Save._saveData.dateviators_currentCharges = 5;
			Singleton<DayNightCycle>.Instance.InitializeDateTime();
			Singleton<DayNightCycle>.Instance.DaysSinceStart = 0;
			Save._saveData.dayNightCycle_presentDayPresentTime = Singleton<DayNightCycle>.Instance.PresentDayPresentTime;
			Save._saveData.dayNightCycle_currentDayPhase = Singleton<DayNightCycle>.Instance.CurrentDayPhase;
			Save._saveData.dayNightCycle_daysSinceStart = Singleton<DayNightCycle>.Instance.DaysSinceStart;
			Singleton<InkController>.Instance.story.variablesState["ng_plus"] = true;
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0002421F File Offset: 0x0002241F
	public bool GetNewGamePlus()
	{
		return Save._saveData.newGamePlus;
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0002422B File Offset: 0x0002242B
	public void SetTutorialFinished(bool finished)
	{
		Save._saveData.tutorialIsFinished = finished;
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00024238 File Offset: 0x00022438
	public void SetDaemonGlitchStrength(float strength)
	{
		Save._saveData.daemonGlitchStrength = strength;
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000680 RID: 1664 RVA: 0x00024245 File Offset: 0x00022445
	public bool speedBoost
	{
		get
		{
			return Save._saveData.speedBoost;
		}
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00024251 File Offset: 0x00022451
	public void SetPlayerSpeedFast()
	{
		Save._saveData.speedBoost = true;
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0002425E File Offset: 0x0002245E
	public void SetPlayerSpeedNormal()
	{
		Save._saveData.speedBoost = false;
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0002426B File Offset: 0x0002246B
	public float GetDaemonGlitchStrength()
	{
		return Save._saveData.daemonGlitchStrength;
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00024277 File Offset: 0x00022477
	public bool GetTutorialFinished()
	{
		return Save._saveData.tutorialIsFinished;
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00024283 File Offset: 0x00022483
	public bool GetFullTutorialFinished()
	{
		return this.GetDateStatus("betty") > RelationshipStatus.Unmet;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00024293 File Offset: 0x00022493
	public void CheckFinishedMessagesRules(string currentFinishedNode)
	{
		Save._saveData.CheckFinishedMessagesRulesForCanopy(currentFinishedNode);
		Save._saveData.CheckFinishedMessagesRules(currentFinishedNode);
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x000242AB File Offset: 0x000224AB
	public bool DoFinishedMessagesContain(string messageNode)
	{
		return Save._saveData.finishedMessages.Contains(messageNode);
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000242C0 File Offset: 0x000224C0
	public static void SetAutoSaveSlots()
	{
		foreach (SaveSlot saveSlot in Object.FindObjectsOfType(typeof(SaveSlot), true))
		{
			for (int j = 0; j < Save.MAX_AUTOSAVE_SLOTS; j++)
			{
				if (saveSlot.SaveSlotID == j)
				{
					Save.autoSaveSlots[j] = saveSlot;
				}
			}
		}
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00024318 File Offset: 0x00022518
	public static void SetDeluxeEditionVariables()
	{
		Singleton<InkController>.Instance.story.variablesState["deluxe"] = Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION;
		Singleton<InkController>.Instance.story.variablesState["frs"] = Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION;
		if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			DateADex.Instance.UnlockCollectable("skylar", 4, true, false, false);
		}
		DateADex.Instance.UpdateDateStatusInkVariables();
		DateADex.Instance.UpdateRealizedInkVariable();
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x000243A8 File Offset: 0x000225A8
	private static void SetStoryJson(Save.SaveData saveData)
	{
		if (!string.IsNullOrEmpty(saveData.StoryJSON))
		{
			Singleton<InkController>.Instance.SetState(saveData.StoryJSON);
			Save.SetDeluxeEditionVariables();
		}
		else
		{
			Singleton<InkStoryProvider>.Instance.Story.ResetState();
			Singleton<Save>.Instance.SetInkVariables();
		}
		if (saveData.newGamePlus && saveData.newGamePlusSpecsSnapshot != null)
		{
			BaseSpecsSnapshot.ApplySnapshot(saveData.newGamePlusSpecsSnapshot);
		}
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00024410 File Offset: 0x00022610
	private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != Save._saveData.SceneName)
		{
			return;
		}
		BetterPlayerControl betterPlayerControl = Object.FindObjectOfType<BetterPlayerControl>();
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS))
		{
			betterPlayerControl.Move(Save._saveData.PlayerPosition, Save._saveData.PlayerRotation);
		}
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(Save._saveData.SceneName));
		if (Save._saveData.inDialogue)
		{
			Save.SetStoryJson(Save._saveData);
			Singleton<InkController>.Instance.SetSaveToLoad(Save._saveData.DialogueState);
		}
		else
		{
			Save.SetStoryJson(Save._saveData);
		}
		Singleton<Save>.Instance.StartCoroutine(Singleton<Save>.Instance.LoadMessageHistoryFromSave());
		SaveAutoFixManager.StartSaveFixes();
		SceneManager.sceneLoaded -= Save.OnSceneLoaded;
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x000244DD File Offset: 0x000226DD
	private IEnumerator LoadMessageHistoryFromSave()
	{
		this._isLoadingMessageHistory = true;
		yield return null;
		Save._saveData.SetMessagesFromSave(ChatType.Wrkspce);
		Save._saveData.SetMessagesFromSave(ChatType.Canopy);
		Save._saveData.SetMessagesFromSave(ChatType.Thiscord);
		Save.SetStoryJson(Save._saveData);
		this._isLoadingMessageHistory = false;
		yield break;
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x000244EC File Offset: 0x000226EC
	public void Update()
	{
		if (this._pendingFinishedMessages.Count != 0 && !Save._saveData.IsAddFinishedMessageInProgress)
		{
			Tuple<string, float> tuple = this._pendingFinishedMessages.Dequeue();
			Save._saveData.AddFinishedMessage(tuple.Item1, tuple.Item2, true);
		}
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00024535 File Offset: 0x00022735
	public void SetSeenSpecsTutorialMessages()
	{
		Save._saveData.hasSeenSpecsTutorial = true;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00024542 File Offset: 0x00022742
	public bool HasSeenSpecsTutorialMessages()
	{
		return Save._saveData.hasSeenSpecsTutorial;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002454E File Offset: 0x0002274E
	private void OnGameLoaded()
	{
		Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS);
	}

	// Token: 0x040005C0 RID: 1472
	private static Save.SaveData _saveData;

	// Token: 0x040005C1 RID: 1473
	public static readonly int MAX_AUTOSAVE_SLOTS = 1;

	// Token: 0x040005C2 RID: 1474
	private static string _saveDirectory;

	// Token: 0x040005C3 RID: 1475
	private UIUtilities _uiUtilities;

	// Token: 0x040005C4 RID: 1476
	private static SaveSlot[] autoSaveSlots = new SaveSlot[Save.MAX_AUTOSAVE_SLOTS];

	// Token: 0x040005C6 RID: 1478
	private long SafetyTimeOnGameLoad;

	// Token: 0x040005C7 RID: 1479
	private bool _isLoadingMessageHistory;

	// Token: 0x040005C8 RID: 1480
	private Queue<Tuple<string, float>> _pendingFinishedMessages = new Queue<Tuple<string, float>>();

	// Token: 0x040005C9 RID: 1481
	public static string SettingKeySkipTutorial = "CanSkipTutorial";

	// Token: 0x040005CA RID: 1482
	private static readonly CultureInfo[] FallbackCultures = new CultureInfo[]
	{
		new CultureInfo("en-GB"),
		new CultureInfo("en-US"),
		new CultureInfo("pl-PL")
	};

	// Token: 0x020002E2 RID: 738
	public static class DataVersion
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x00066D53 File Offset: 0x00064F53
		public static uint Latest
		{
			get
			{
				return 3U;
			}
		}

		// Token: 0x04001166 RID: 4454
		public const int Initial = 0;

		// Token: 0x04001167 RID: 4455
		public const int CombinedSave = 1;

		// Token: 0x04001168 RID: 4456
		public const int NewtonsoftJson = 2;

		// Token: 0x04001169 RID: 4457
		public const int JsonAsBase64 = 3;
	}

	// Token: 0x020002E3 RID: 739
	// (Invoke) Token: 0x06001618 RID: 5656
	public delegate void OnGameLoad();

	// Token: 0x020002E4 RID: 740
	// (Invoke) Token: 0x0600161C RID: 5660
	public delegate void AfterGameLoadReset();

	// Token: 0x020002E5 RID: 741
	// (Invoke) Token: 0x06001620 RID: 5664
	public delegate void AfterGameLoad();

	// Token: 0x020002E6 RID: 742
	// (Invoke) Token: 0x06001624 RID: 5668
	public delegate void OnGameSave();

	// Token: 0x020002E7 RID: 743
	// (Invoke) Token: 0x06001628 RID: 5672
	public delegate void OnGameSaveCompleted(SaveGameError result);

	// Token: 0x020002E8 RID: 744
	// (Invoke) Token: 0x0600162C RID: 5676
	public delegate void OnAwakening();

	// Token: 0x020002E9 RID: 745
	[Serializable]
	public class SaveData
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06001630 RID: 5680 RVA: 0x00066D5F File Offset: 0x00064F5F
		// (set) Token: 0x0600162F RID: 5679 RVA: 0x00066D56 File Offset: 0x00064F56
		public bool IsAddFinishedMessageInProgress { get; private set; }

		// Token: 0x06001631 RID: 5681 RVA: 0x00066D68 File Offset: 0x00064F68
		public SaveData()
		{
			this.PlayerData = new Save.PlayerData();
			this.tutorialIsFinished = false;
			this.newGamePlus = false;
			this.statnames = new List<string>();
			this.statvalues = new List<int>();
			this.interactablesStates = new List<string>();
			this.charToLastDayTalkedTo = new Dictionary<string, int>();
			this.tutorialThresholdStates = new List<string>();
			this.objectSaveDataDictionary = new Dictionary<string, ObjectSaveData>();
			this.canopyChatHistory = new List<Save.AppMessage>();
			this.workspaceChatHistory = new List<Save.AppMessage>();
			this.thiscordChatHistory = new List<Save.AppMessage>();
			this.AllChatHistory = new List<Save.AppMessage>();
			this.finishedMessages = new List<string>();
			this.datestates = new int[102];
			this.boxExamenDictionary = new Dictionary<string, int>();
			this.datestatesRealized = new int[102];
			this.inDialogue = false;
			this.lastDialogueDateable = "";
			this.objectSaveDataList = new List<ObjectSaveData>();
			this.roomers = new List<Save.RoomersStruct>();
			this.progressThroughRecentSamMessage = new List<int>();
			this.speedBoost = false;
			this.characterToStagePosition = new NameToStagePosition();
			this.charToCollectableMap = new CharacterEntryToCollectableBoolMap();
			this.charToDexEntriesMap = new CharacterEntryToCollectableBoolMap();
			this.charactersWithNewStatusOnDateADex = new List<string>();
			this.shrinkAmount = 0f;
			this.daemonGlitchStrength = 0f;
			this.phoneBackgroundIndex = 0;
			this.SetupCharToCollectableMap();
			this.SetupCharToDexEntriesMap();
			this.TimeAtLastLoginEvent = DateTime.UtcNow;
			this.playTimeInMillis = 0L;
			this.IsAddFinishedMessageInProgress = false;
			this.StatusToBeReverted = new List<string>();
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00066F04 File Offset: 0x00065104
		private void SetupCharToCollectableMap()
		{
			if (Singleton<CharacterHelper>.Instance != null)
			{
				foreach (string text in Singleton<CharacterHelper>.Instance._characters)
				{
					CollectableToBool collectableToBool = new CollectableToBool();
					for (int i = 1; i <= 3; i++)
					{
						collectableToBool.Add(i, false);
					}
					this.charToCollectableMap.Add(text, collectableToBool);
				}
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00066F84 File Offset: 0x00065184
		private void SetupCharToDexEntriesMap()
		{
			if (Singleton<CharacterHelper>.Instance != null)
			{
				foreach (string text in Singleton<CharacterHelper>.Instance._characters)
				{
					CollectableToBool collectableToBool = new CollectableToBool();
					this.charToDexEntriesMap.Add(text, collectableToBool);
				}
			}
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00066FF0 File Offset: 0x000651F0
		public void Load()
		{
			this.SetPlayerNameInk();
			this.SetPlayerPronounsInk();
			this.SetPlayerBirthdayInk();
			this.SetPlayerFavoriteThingInk();
			this.SetPlayerTownInk();
			Singleton<ChatMaster>.Instance.ClearChatHistory();
			this.TimeAtLastLoginEvent = DateTime.UtcNow;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00067028 File Offset: 0x00065228
		public string SaveToString()
		{
			TimeSpan timeSpan = DateTime.UtcNow - this.TimeAtLastLoginEvent;
			this.playTimeInMillis += (long)timeSpan.TotalMilliseconds;
			this.TimeAtLastLoginEvent = DateTime.UtcNow;
			string text = string.Empty;
			try
			{
				text = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					ContractResolver = new IgnoreVector3PropertiesResolver()
				});
			}
			catch (Exception ex)
			{
				T17Debug.LogError("[Save.SaveToString] Exception serialising save data to JSON. exception=" + ex.ToString());
			}
			return text;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000670B8 File Offset: 0x000652B8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.SceneName,
				", ",
				this.PlayerData.Name,
				", ",
				this.TimeStamp
			});
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000670F5 File Offset: 0x000652F5
		public void SetPlayerNameInk()
		{
			Singleton<InkController>.Instance.story.variablesState["player_name"] = this.PlayerData.Name;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0006711C File Offset: 0x0006531C
		public void SetPlayerPronounsInk()
		{
			switch (this.PlayerData.Pronouns)
			{
			case 1:
				Singleton<InkController>.Instance.story.variablesState["pronoun1"] = "he";
				return;
			case 2:
				Singleton<InkController>.Instance.story.variablesState["pronoun1"] = "she";
				return;
			}
			Singleton<InkController>.Instance.story.variablesState["pronoun1"] = "they";
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x000671A8 File Offset: 0x000653A8
		public void SetPlayerBirthdayInk()
		{
			DateTime dateTime = new DateTime(this.PlayerData.BirthdaydYear, this.PlayerData.BirthdaydMonth, this.PlayerData.BirthdaydDay);
			Singleton<InkController>.Instance.story.variablesState["player_birth_month"] = dateTime.ToString("MMMM");
			Singleton<InkController>.Instance.story.variablesState["player_birth_day"] = this.PlayerData.BirthdaydDay;
			Singleton<InkController>.Instance.story.variablesState["player_birth_year"] = this.PlayerData.BirthdaydYear;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00067254 File Offset: 0x00065454
		public void SetPlayerTownInk()
		{
			Singleton<InkController>.Instance.story.variablesState["town_name"] = this.PlayerData.TownName;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0006727A File Offset: 0x0006547A
		public void SetPlayerFavoriteThingInk()
		{
			Singleton<InkController>.Instance.story.variablesState["favorite_thing"] = this.PlayerData.FavoriteThing;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000672A0 File Offset: 0x000654A0
		public void SetMessagesFromSave(ChatType type)
		{
			List<Save.AppMessage> allChatHistory = this.AllChatHistory;
			Predicate<Save.AppMessage> <>9__0;
			Predicate<Save.AppMessage> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (Save.AppMessage x) => x.ChatType == type);
			}
			foreach (Save.AppMessage appMessage in allChatHistory.FindAll(predicate))
			{
				Singleton<ChatMaster>.Instance.LoadChatHistoryFromSave(appMessage, type);
			}
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00067330 File Offset: 0x00065530
		public void SetDexEntriesInDateADex()
		{
			foreach (KeyValuePair<string, CollectableToBool> keyValuePair in this.charToDexEntriesMap)
			{
				foreach (int num in keyValuePair.Value.Keys)
				{
					if (keyValuePair.Value[num])
					{
						DateADex.Instance.UnlockDexEntry(keyValuePair.Key, num, true);
					}
				}
			}
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000673D4 File Offset: 0x000655D4
		public void AddFinishedMessage(string messageNode, float trackLength = -1f, bool allowDuplicates = true)
		{
			if (Singleton<Save>.Instance._isLoadingMessageHistory)
			{
				return;
			}
			this.IsAddFinishedMessageInProgress = true;
			if (!this.finishedMessages.Contains(messageNode))
			{
				this.finishedMessages.Add(messageNode);
			}
			else
			{
				if (!allowDuplicates)
				{
					this.IsAddFinishedMessageInProgress = false;
					return;
				}
				if (messageNode.Contains("_"))
				{
					bool flag = false;
					string text = messageNode.Substring(0, messageNode.LastIndexOf("_") + 1);
					int num = -1;
					if (int.TryParse(messageNode.Substring(messageNode.LastIndexOf("_") + 1), out num))
					{
						while (!flag)
						{
							num++;
							if (!this.finishedMessages.Contains(text + num.ToString()))
							{
								flag = true;
								messageNode = text + num.ToString();
								this.finishedMessages.Add(messageNode);
							}
						}
					}
					else
					{
						this.finishedMessages.Add(messageNode);
					}
				}
			}
			if (trackLength < 0f)
			{
				trackLength = Singleton<AudioManager>.Instance.GetLengthOfCurrentSongOfType(AUDIO_TYPE.DIALOGUE);
			}
			if (trackLength == 0f)
			{
				trackLength = 0.5f;
			}
			Singleton<TutorialController>.Instance.StartCoroutine(this.delayMessageRuleReference(messageNode, trackLength));
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x000674EB File Offset: 0x000656EB
		private IEnumerator delayMessageRuleReference(string messageNode, float timeDelay)
		{
			if (this.IsAddFinishedMessageInProgress)
			{
				this.CheckFinishedMessagesRulesForCanopy(messageNode);
			}
			yield return new WaitForSeconds(timeDelay);
			if (this.IsAddFinishedMessageInProgress)
			{
				this.CheckFinishedMessagesRules(messageNode);
				this.IsAddFinishedMessageInProgress = false;
				yield break;
			}
			yield break;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00067508 File Offset: 0x00065708
		public void CheckFinishedMessagesRulesForCanopy(string currentFinishedNode)
		{
			if (currentFinishedNode.StartsWith("canopy_work.") && this.GetNumberOfCanopyFinishedJobs() == 2)
			{
				Singleton<ChatMaster>.Instance.canopyping.SetActive(false);
				Singleton<ChatMaster>.Instance.workspaceping.SetActive(true);
				Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat.mostmail_2", ChatType.Wrkspce, false, false);
			}
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00067560 File Offset: 0x00065760
		public void CheckFinishedMessagesRules(string currentFinishedNode)
		{
			List<Save.AppMessageStitch> list = Singleton<Save>.Instance.GetFinishedMessages();
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].Stitch == currentFinishedNode)
				{
					if (list[i].ProcessedEndedCondition)
					{
						break;
					}
					if (currentFinishedNode == "wrkspace_chat.mostmail_1")
					{
						Singleton<ChatMaster>.Instance.DelayAddNewMessage("wrkspace_chat.tom_1", ChatType.Wrkspce);
						list[i].ProcessedEndedCondition = true;
						return;
					}
					if (currentFinishedNode == "wrkspace_chat.tom_1")
					{
						Singleton<ChatMaster>.Instance.DelayAddNewMessage("wrkspace_chat.sam_1", ChatType.Wrkspce);
						list[i].ProcessedEndedCondition = true;
						return;
					}
					if (currentFinishedNode == "wrkspace_chat.sam_1")
					{
						Singleton<Save>.Instance.StartCoroutine(this.GetNewCanopyChat());
						list[i].ProcessedEndedCondition = true;
						return;
					}
					if (currentFinishedNode == "wrkspace_chat.mostmail_2")
					{
						Singleton<ChatMaster>.Instance.DelayAddNewMessage("wrkspace_chat.tom_2", ChatType.Wrkspce);
						list[i].ProcessedEndedCondition = true;
						return;
					}
					if (currentFinishedNode == "wrkspace_chat.tom_2")
					{
						Singleton<ChatMaster>.Instance.DelayAddNewMessage("wrkspace_chat.sam_2", ChatType.Wrkspce);
						list[i].ProcessedEndedCondition = true;
						return;
					}
					if (currentFinishedNode == "wrkspace_chat.sam_2" && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK))
					{
						Singleton<TutorialController>.Instance.ForceCloseComputer();
						Singleton<ChatMaster>.Instance.DelayAddNewMessage("thiscord_phone.tfh_1", ChatType.Thiscord);
						list[i].ProcessedEndedCondition = true;
						return;
					}
					if (currentFinishedNode == "thiscord_phone.tfh_1" && !Singleton<Save>.Instance.GetTutorialFinished())
					{
						Singleton<TutorialController>.Instance.DeliverGiftBox0();
						list[i].ProcessedEndedCondition = true;
						return;
					}
					if (currentFinishedNode == "wrkspace_chat.tfh_8" && (int)Singleton<InkController>.Instance.story.variablesState["hate_gates"] != 4)
					{
						Singleton<InkController>.Instance.story.variablesState["realize_skylar_asap"] = "on";
						list[i].ProcessedEndedCondition = true;
						return;
					}
					list[i].ProcessedEndedCondition = true;
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00067770 File Offset: 0x00065970
		private int GetNumberOfCanopyFinishedJobs()
		{
			int num = 0;
			using (List<string>.Enumerator enumerator = this.finishedMessages.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.StartsWith("canopy_work."))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x000677D0 File Offset: 0x000659D0
		private IEnumerator GetNewCanopyChat()
		{
			if (Singleton<Save>.Instance.GetPlayerCanopyVisited().Count == 0)
			{
				yield return new WaitForSeconds(Random.Range(2f, 3f));
				if (this.GetNumberOfCanopyFinishedJobs() == 0)
				{
					Singleton<ChatMaster>.Instance.canopypings++;
					Singleton<ChatMaster>.Instance.UpdateVisualsCanopy();
				}
				List<string> list = Singleton<ComputerManager>.Instance.GenerateDailyChats(0);
				bool isFirst = true;
				if (Singleton<AudioManager>.Instance != null)
				{
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_newtasks, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				}
				foreach (string text in list)
				{
					if (isFirst)
					{
						bool flag = Singleton<ChatMaster>.Instance.activewindow == ChatType.Canopy && Singleton<ChatMaster>.Instance.ActiveChatCanopy == null;
						Singleton<ChatMaster>.Instance.StartChat(text, ChatType.Canopy, flag, false);
						isFirst = false;
						yield return new WaitForSeconds(1.5f);
					}
					else
					{
						Singleton<ChatMaster>.Instance.StartChat(text, ChatType.Canopy, false, false);
						yield return new WaitForSeconds(0.3f);
					}
				}
				List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x000677E0 File Offset: 0x000659E0
		public void SetWorkspaceMessages()
		{
			foreach (Save.AppMessage appMessage in new List<Save.AppMessage>(this.workspaceChatHistory))
			{
				Singleton<ChatMaster>.Instance.LoadChatHistoryFromSave(appMessage, ChatType.Wrkspce);
			}
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00067840 File Offset: 0x00065A40
		public void SetCanopyMessages()
		{
			foreach (Save.AppMessage appMessage in new List<Save.AppMessage>(this.canopyChatHistory))
			{
				Singleton<ChatMaster>.Instance.LoadChatHistoryFromSave(appMessage, ChatType.Canopy);
			}
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x000678A0 File Offset: 0x00065AA0
		public void AdvanceAutoSaveSlot()
		{
			this.autoSaveSlot++;
			if (this.autoSaveSlot >= Save.MAX_AUTOSAVE_SLOTS)
			{
				this.autoSaveSlot = 0;
			}
		}

		// Token: 0x0400116A RID: 4458
		public Save.PlayerData PlayerData;

		// Token: 0x0400116B RID: 4459
		public string SceneName;

		// Token: 0x0400116C RID: 4460
		public bool tutorialIsFinished;

		// Token: 0x0400116D RID: 4461
		public bool hasSeenSpecsTutorial;

		// Token: 0x0400116E RID: 4462
		public bool newGamePlus;

		// Token: 0x0400116F RID: 4463
		public BaseSpecsSnapshot newGamePlusSpecsSnapshot;

		// Token: 0x04001170 RID: 4464
		public bool speedBoost;

		// Token: 0x04001171 RID: 4465
		public float daemonGlitchStrength;

		// Token: 0x04001172 RID: 4466
		public float shrinkAmount;

		// Token: 0x04001173 RID: 4467
		public int phoneBackgroundIndex;

		// Token: 0x04001174 RID: 4468
		public string DialogueState;

		// Token: 0x04001175 RID: 4469
		public string lastDialogueDateable;

		// Token: 0x04001176 RID: 4470
		public bool inDialogue;

		// Token: 0x04001177 RID: 4471
		public NameToStagePosition characterToStagePosition;

		// Token: 0x04001178 RID: 4472
		public List<string> statnames;

		// Token: 0x04001179 RID: 4473
		public List<int> statvalues;

		// Token: 0x0400117A RID: 4474
		public List<string> interactablesStates;

		// Token: 0x0400117B RID: 4475
		public List<string> tutorialThresholdStates;

		// Token: 0x0400117C RID: 4476
		public Dictionary<string, ObjectSaveData> objectSaveDataDictionary;

		// Token: 0x0400117D RID: 4477
		public Dictionary<string, int> boxExamenDictionary;

		// Token: 0x0400117E RID: 4478
		public int[] datestates;

		// Token: 0x0400117F RID: 4479
		public int[] datestatesRealized;

		// Token: 0x04001180 RID: 4480
		public int dateviators_currentCharges;

		// Token: 0x04001181 RID: 4481
		public DateTime dayNightCycle_presentDayPresentTime;

		// Token: 0x04001182 RID: 4482
		public DayPhase dayNightCycle_currentDayPhase;

		// Token: 0x04001183 RID: 4483
		public long dayNightCycle_ticks;

		// Token: 0x04001184 RID: 4484
		public int dayNightCycle_daysSinceStart;

		// Token: 0x04001185 RID: 4485
		public CharacterEntryToCollectableBoolMap charToCollectableMap;

		// Token: 0x04001186 RID: 4486
		public CharacterEntryToCollectableBoolMap charToDexEntriesMap;

		// Token: 0x04001187 RID: 4487
		public List<string> charactersWithNewStatusOnDateADex;

		// Token: 0x04001188 RID: 4488
		public Dictionary<string, int> charToLastDayTalkedTo;

		// Token: 0x04001189 RID: 4489
		public List<Save.AppMessage> canopyChatHistory;

		// Token: 0x0400118A RID: 4490
		public List<Save.AppMessage> workspaceChatHistory;

		// Token: 0x0400118B RID: 4491
		public List<Save.AppMessage> thiscordChatHistory;

		// Token: 0x0400118C RID: 4492
		public List<Save.AppMessage> AllChatHistory;

		// Token: 0x0400118D RID: 4493
		public List<string> finishedMessages;

		// Token: 0x0400118E RID: 4494
		public Vector3 PlayerPosition;

		// Token: 0x0400118F RID: 4495
		public Quaternion PlayerRotation;

		// Token: 0x04001190 RID: 4496
		public string TimeStamp;

		// Token: 0x04001191 RID: 4497
		public string StoryJSON;

		// Token: 0x04001192 RID: 4498
		public string playerDataJSON;

		// Token: 0x04001193 RID: 4499
		public List<ObjectSaveData> objectSaveDataList;

		// Token: 0x04001194 RID: 4500
		public int unreadMessages;

		// Token: 0x04001195 RID: 4501
		public string mostRecentSamMessage = "";

		// Token: 0x04001196 RID: 4502
		public List<int> progressThroughRecentSamMessage = new List<int>();

		// Token: 0x04001197 RID: 4503
		public int autoSaveSlot = -1;

		// Token: 0x04001198 RID: 4504
		public List<Save.RoomersStruct> roomers;

		// Token: 0x04001199 RID: 4505
		public long playTimeInMillis;

		// Token: 0x0400119A RID: 4506
		public DateTime TimeAtLastLoginEvent;

		// Token: 0x0400119C RID: 4508
		public List<string> StatusToBeReverted;
	}

	// Token: 0x020002EA RID: 746
	[Serializable]
	public class PlayerData
	{
		// Token: 0x0400119D RID: 4509
		public string Name = "null";

		// Token: 0x0400119E RID: 4510
		public int Pronouns;

		// Token: 0x0400119F RID: 4511
		public int BirthdaydDay = 1;

		// Token: 0x040011A0 RID: 4512
		public int BirthdaydMonth = 1;

		// Token: 0x040011A1 RID: 4513
		public int BirthdaydYear = 1980;

		// Token: 0x040011A2 RID: 4514
		public string TownName = "nullsville";

		// Token: 0x040011A3 RID: 4515
		public string FavoriteThing = "nullthing";
	}

	// Token: 0x020002EB RID: 747
	[Serializable]
	public class AppMessageStitch
	{
		// Token: 0x040011A4 RID: 4516
		public string Stitch = "";

		// Token: 0x040011A5 RID: 4517
		public string Status = "";

		// Token: 0x040011A6 RID: 4518
		public bool Ended;

		// Token: 0x040011A7 RID: 4519
		public bool ProcessedEndedCondition;

		// Token: 0x040011A8 RID: 4520
		public List<string> ChatHistoryOptionsSelected = new List<string>();
	}

	// Token: 0x020002EC RID: 748
	[Serializable]
	public class AppMessage
	{
		// Token: 0x06001649 RID: 5705 RVA: 0x0006793C File Offset: 0x00065B3C
		public void MoveNextToHistory()
		{
			if (this.NextStitches.Count > 0)
			{
				string text = this.NextStitches[0];
				Save.AppMessageStitch appMessageStitch = new Save.AppMessageStitch();
				appMessageStitch.Stitch = text;
				appMessageStitch.Status = this.GetStatusFromStitch(text);
				this.StitchHistory.Add(appMessageStitch);
				this.NextStitches.RemoveAt(0);
			}
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00067998 File Offset: 0x00065B98
		private string GetStatusFromStitch(string node)
		{
			if (node.StartsWith("thiscord_phone.tfh_"))
			{
				return "8x8bc.0b";
			}
			if (node.StartsWith("thiscord_phone.sam_3"))
			{
				return "uuhhhhhhhhhhhhhhhhhhhh";
			}
			if (node.StartsWith("thiscord_phone.sam_4"))
			{
				return "much 2 consider.....";
			}
			if (node.StartsWith("thiscord_phone.sam_5"))
			{
				return "still thinking about hero hime...";
			}
			if (node.StartsWith("thiscord_phone.sam_6"))
			{
				return "can someone bring me some cheesecake...";
			}
			if (node.StartsWith("thiscord_phone.sam_7"))
			{
				return "mainichi benkyou shite imasu! n.n";
			}
			if (node.StartsWith("thiscord_phone.sam_8"))
			{
				return "what is my life rn lmao";
			}
			if (node.StartsWith("thiscord_phone.sam_9"))
			{
				return "fine... just fine... lol";
			}
			if (node.StartsWith("thiscord_phone.sam_10"))
			{
				return "margaritas at the midnight buffet";
			}
			if (node.StartsWith("thiscord_phone.sam_11"))
			{
				return "oh how the turntables...";
			}
			if (node.StartsWith("thiscord_phone.sam_quit_1"))
			{
				return "helloooo nurse (its me im the nurse)";
			}
			if (node.StartsWith("thiscord_phone.sam_12"))
			{
				return "what is this feeling...";
			}
			if (node.StartsWith("thiscord_phone.sam_quit_2"))
			{
				return "just call me dr love lol";
			}
			if (node.StartsWith("wrkspace_chat.mostmail_"))
			{
				return "Unassailable";
			}
			if (node.StartsWith("wrkspace_chat.most_hate"))
			{
				return "Feeling Violent!";
			}
			if (node.StartsWith("wrkspace_chat.tom_1"))
			{
				return "Every day is leg day";
			}
			if (node.StartsWith("wrkspace_chat.tom_2"))
			{
				return "uh oh";
			}
			if (node.StartsWith("wrkspace_chat.tom_3"))
			{
				return "Whoah Nelly";
			}
			if (node.StartsWith("wrkspace_chat.tom_4"))
			{
				return "Whoah Nelly";
			}
			if (node.StartsWith("wrkspace_chat.tom_5"))
			{
				return "Whoah Nelly";
			}
			if (node.StartsWith("wrkspace_chat.tom_hate"))
			{
				return "Whoah Nelly";
			}
			if (node.StartsWith("wrkspace_chat.sam_1"))
			{
				return "is it Friday yet?";
			}
			if (node.StartsWith("wrkspace_chat.sam_2"))
			{
				return "i hate it here lmao";
			}
			if (node.StartsWith("wrkspace_chat.val_1"))
			{
				return "Feeling Helpful!";
			}
			if (node.StartsWith("wrkspace_chat.val_"))
			{
				return "Alive and Well";
			}
			return "";
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00067B85 File Offset: 0x00065D85
		public Save.AppMessageStitch GetLastStitchObj()
		{
			if (this.StitchHistory.Count > 0)
			{
				return this.StitchHistory.Last<Save.AppMessageStitch>();
			}
			return null;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00067BA4 File Offset: 0x00065DA4
		public string GetLastStitch()
		{
			if (this.NextStitches.Count > 0)
			{
				return this.NextStitches[0];
			}
			if (this.StitchHistory != null && this.StitchHistory.Count > 0)
			{
				Save.AppMessageStitch appMessageStitch = this.StitchHistory.Last<Save.AppMessageStitch>();
				if (appMessageStitch != null)
				{
					return appMessageStitch.Stitch;
				}
			}
			return "";
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00067C00 File Offset: 0x00065E00
		public bool ContainsStitch(string newStitch)
		{
			using (List<Save.AppMessageStitch>.Enumerator enumerator = this.StitchHistory.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Stitch == newStitch)
					{
						return true;
					}
				}
			}
			using (List<string>.Enumerator enumerator2 = this.NextStitches.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == newStitch)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00067CA8 File Offset: 0x00065EA8
		public bool IsLastHistoryNewChat()
		{
			return this.StitchHistory.Count != 0 && this.StitchHistory[this.StitchHistory.Count - 1].Equals(this.NodePrefix);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00067CDC File Offset: 0x00065EDC
		public string GetLastHistoryNewChat()
		{
			if (this.StitchHistory.Count == 0)
			{
				return "";
			}
			return this.StitchHistory[this.StitchHistory.Count - 1].Stitch;
		}

		// Token: 0x040011A9 RID: 4521
		public string Name = "";

		// Token: 0x040011AA RID: 4522
		public string NodePrefix = "";

		// Token: 0x040011AB RID: 4523
		public ChatType ChatType;

		// Token: 0x040011AC RID: 4524
		public List<Save.AppMessageStitch> StitchHistory = new List<Save.AppMessageStitch>();

		// Token: 0x040011AD RID: 4525
		public List<string> NextStitches = new List<string>();

		// Token: 0x040011AE RID: 4526
		public string Status = "";

		// Token: 0x040011AF RID: 4527
		public string Node = "";

		// Token: 0x040011B0 RID: 4528
		public List<string> ChatHistory = new List<string>();
	}

	// Token: 0x020002ED RID: 749
	[Serializable]
	public class RoomersStruct
	{
		// Token: 0x06001651 RID: 5713 RVA: 0x00067D70 File Offset: 0x00065F70
		public bool HasTipFound()
		{
			if (this.skylarTipIsFound)
			{
				return true;
			}
			using (List<Save.RoomersTipStruct>.Enumerator enumerator = this.tips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isFound)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00067DD4 File Offset: 0x00065FD4
		public RoomersStruct(string character, bool isFound)
		{
			this.character = character;
			this.isFound = isFound;
			this.hasNew = true;
		}

		// Token: 0x040011B1 RID: 4529
		public string character;

		// Token: 0x040011B2 RID: 4530
		public string questName;

		// Token: 0x040011B3 RID: 4531
		public string description;

		// Token: 0x040011B4 RID: 4532
		public string skylar;

		// Token: 0x040011B5 RID: 4533
		public bool isFound;

		// Token: 0x040011B6 RID: 4534
		public bool skylarTipIsFound;

		// Token: 0x040011B7 RID: 4535
		public bool hasNew;

		// Token: 0x040011B8 RID: 4536
		public List<Save.RoomersTipStruct> tips;
	}

	// Token: 0x020002EE RID: 750
	[Serializable]
	public class RoomersTipStruct
	{
		// Token: 0x06001653 RID: 5715 RVA: 0x00067DF1 File Offset: 0x00065FF1
		public RoomersTipStruct(string tipName, string tipInfo, bool isFound)
		{
			this.tipName = tipName;
			this.tipInfo = tipInfo;
			this.tipNameAfterValidation = tipName;
			this.tipInfoAfterValidation = tipInfo;
			this.isFound = isFound;
		}

		// Token: 0x040011B9 RID: 4537
		public string tipName;

		// Token: 0x040011BA RID: 4538
		public string tipInfo;

		// Token: 0x040011BB RID: 4539
		public string tipNameAfterValidation;

		// Token: 0x040011BC RID: 4540
		public string tipInfoAfterValidation;

		// Token: 0x040011BD RID: 4541
		public bool isFound;
	}
}
