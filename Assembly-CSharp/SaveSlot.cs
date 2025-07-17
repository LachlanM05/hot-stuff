using System;
using System.Runtime.CompilerServices;
using Date_Everything.Scripts.UI;
using T17.Services;
using T17.UI;
using Team17.Common;
using Team17.Services.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class SaveSlot : MonoBehaviour
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0003CD83 File Offset: 0x0003AF83
	public bool IsAutoSave
	{
		get
		{
			return this.isAutoSave;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0003CD8B File Offset: 0x0003AF8B
	public bool HasData
	{
		get
		{
			return Services.SaveGameService.GetSlotInfo(this.SaveSlotID) != null;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0003CDA0 File Offset: 0x0003AFA0
	public string LastPlayedTimestamp
	{
		get
		{
			return this.lastPlayedTimestamp;
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0003CDA8 File Offset: 0x0003AFA8
	private void Awake()
	{
		Button saveButton = this.SaveButton;
		if (saveButton != null)
		{
			saveButton.gameObject.AddComponent<DisableButtonIfAnySlotOperationIsActive>();
		}
		Button loadButton = this.LoadButton;
		if (loadButton != null)
		{
			loadButton.gameObject.AddComponent<DisableButtonIfAnySlotOperationIsActive>();
		}
		Button deleteButton = this.DeleteButton;
		if (deleteButton == null)
		{
			return;
		}
		deleteButton.gameObject.AddComponent<DisableButtonIfAnySlotOperationIsActive>();
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0003CDF9 File Offset: 0x0003AFF9
	private void Start()
	{
		SaveGameErrorHandler.OnRetrySave -= this.OnRetrySave;
		SaveGameErrorHandler.OnRetrySave += this.OnRetrySave;
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0003CE1D File Offset: 0x0003B01D
	private void OnDestroy()
	{
		Services.SaveGameService.OnSlotUpdated -= this.OnSlotUpdated;
		Services.SaveGameService.OnSlotUpdated -= this.OnSlotUpdatePending;
		SaveGameErrorHandler.OnRetrySave -= this.OnRetrySave;
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0003CE5C File Offset: 0x0003B05C
	public void StopMusic()
	{
		Singleton<AudioManager>.Instance.StopTrack("file_select", 0.5f);
		Singleton<AudioManager>.Instance.StopTrack("main_menu", 0.5f);
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0003CE86 File Offset: 0x0003B086
	private void OnDisable()
	{
		Services.SaveGameService.OnSlotUpdated -= this.OnSlotUpdated;
		Services.SaveGameService.OnSlotUpdated += this.OnSlotUpdatePending;
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0003CEB4 File Offset: 0x0003B0B4
	private void OnEnable()
	{
		Services.SaveGameService.OnSlotUpdated -= this.OnSlotUpdatePending;
		Services.SaveGameService.OnSlotUpdated += this.OnSlotUpdated;
		if (this.pendingChange)
		{
			this.pendingChange = false;
			this.UpdateSlot();
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0003CF02 File Offset: 0x0003B102
	public void SetupSaveSlotNewGamePlus()
	{
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0003CF04 File Offset: 0x0003B104
	private void SetBackground(bool isNewGamePlus)
	{
		if (isNewGamePlus)
		{
			this.Background.sprite = this.SpriteNewGamePlusBg;
			this.NewGamePlusIcon.SetActive(true);
			return;
		}
		this.Background.sprite = this.SpriteDefaultBg;
		this.NewGamePlusIcon.SetActive(false);
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0003CF44 File Offset: 0x0003B144
	[Obsolete("Should not be used. kept around incase a reference on a prefab uses it.", true)]
	public void SetupSaveSlot()
	{
		T17Debug.LogError("I should not be called anymore.");
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0003CF50 File Offset: 0x0003B150
	[Obsolete("Use PromptLoadGameData instead. This is kept around as prefabs reference this function so we cant rename it.", true)]
	public void ConfirmLoad()
	{
		this.TryPromptLoadGameData();
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0003CF58 File Offset: 0x0003B158
	[Obsolete("Use PromptSaveGameData instead. This is kept around as prefabs reference this function so we cant rename it.", true)]
	public void ConfirmSave()
	{
		this.TryPromptSaveGameData();
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0003CF60 File Offset: 0x0003B160
	[Obsolete("Use PromptSaveGameData instead. This is kept around as prefabs reference this function so we cant rename it.", true)]
	public void DeleteGameData()
	{
		this.TryPromptDeleteGameData();
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0003CF68 File Offset: 0x0003B168
	public void TryPromptLoadGameData()
	{
		if (SaveSlot.CanDoNewOperation())
		{
			UIDialogManager.Instance.ShowDialog("Load?", "Are you sure you want to load this save?", "Load", new Action(this.TryLoadGameDataAsync), "Cancel", null, "", null, 0, true);
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x0003CFB0 File Offset: 0x0003B1B0
	public void TryPromptSaveGameData()
	{
		if (SaveSlot.CanDoNewOperation())
		{
			UIDialogManager.Instance.ShowDialog("Overwrite?", "Are you sure you want to overwrite this save?", "Overwrite", delegate
			{
				this.TrySaveGameDataAsync(this.closeOnSaveConfirmed, false, null, this.saveScreenManager.IsNewGamePlusScreen && Singleton<Save>.Instance.GetNewGamePlus());
			}, "Cancel", null, "", null, 0, true);
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0003CFF8 File Offset: 0x0003B1F8
	public void TryPromptDeleteGameData()
	{
		if (SaveSlot.CanDoNewOperation())
		{
			UIDialogManager.Instance.ShowDialog("Delete?", "Are you sure you want to delete this save?", "Delete", new Action(this.TryDeleteGameDataAsync), "Cancel", null, "", null, 0, true);
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0003D040 File Offset: 0x0003B240
	public void TryLoadGameDataAsync()
	{
		SaveSlot.<>c__DisplayClass54_0 CS$<>8__locals1 = new SaveSlot.<>c__DisplayClass54_0();
		CS$<>8__locals1.<>4__this = this;
		if (!SaveSlot.RequestSetOperation(SaveSlot.SlotOperation.Loading))
		{
			T17Debug.LogError(string.Format("Unable to load game. currently operation '{0}'", SaveSlot._currentOperation));
			return;
		}
		this.saveScreenManager.TriggerLoadAnimation();
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		Singleton<AudioManager>.Instance.FadeTrackOut("Main Menu ambience", 0.5f);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.start_game, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		CS$<>8__locals1.skipLoadWait = Singleton<PhoneManager>.Instance.phoneOpened;
		Singleton<PhoneManager>.Instance.ClosePhoneAsync(delegate
		{
			SaveSlot.<>c__DisplayClass54_0.<<TryLoadGameDataAsync>b__0>d <<TryLoadGameDataAsync>b__0>d;
			<<TryLoadGameDataAsync>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
			<<TryLoadGameDataAsync>b__0>d.<>4__this = CS$<>8__locals1;
			<<TryLoadGameDataAsync>b__0>d.<>1__state = -1;
			<<TryLoadGameDataAsync>b__0>d.<>t__builder.Start<SaveSlot.<>c__DisplayClass54_0.<<TryLoadGameDataAsync>b__0>d>(ref <<TryLoadGameDataAsync>b__0>d);
		}, true);
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0003D0F8 File Offset: 0x0003B2F8
	public async void TrySaveGameDataAsync(bool close, bool triggeredByAutoSave = false, Button backButtonOverride = null, bool isLocketNewGamePlusSave = false)
	{
		if (!SaveSlot.RequestSetOperation(SaveSlot.SlotOperation.Saving))
		{
			T17Debug.LogError("Unable to save game data. currently operation '" + SaveSlot._currentOperation.ToString() + "'");
		}
		else
		{
			SaveIndicatorController.triggeredByAutoSave = triggeredByAutoSave;
			try
			{
				await Singleton<Save>.Instance.SaveGameAsync(this.SaveSlotID, isLocketNewGamePlusSave);
			}
			finally
			{
				this.ResetCurrentOperation();
			}
			if (close)
			{
				if (backButtonOverride != null)
				{
					backButtonOverride.onClick.Invoke();
				}
				else if (this.BackButton != null)
				{
					this.BackButton.onClick.Invoke();
				}
			}
		}
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0003D150 File Offset: 0x0003B350
	private async void TryDeleteGameDataAsync()
	{
		if (!SaveSlot.RequestSetOperation(SaveSlot.SlotOperation.Deleting))
		{
			T17Debug.LogError("Unable to delete game data. currently operation '" + SaveSlot._currentOperation.ToString() + "'");
		}
		else
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_delete_file, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			try
			{
				await Singleton<Save>.Instance.DeleteGameAsync(this.SaveSlotID);
			}
			finally
			{
				this.ResetCurrentOperation();
			}
			this.saveScreenManager.OnSaveDeleted();
		}
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0003D187 File Offset: 0x0003B387
	public void OnSlotUpdated(int slotId)
	{
		if (slotId == this.SaveSlotID)
		{
			this.UpdateSlot();
		}
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0003D198 File Offset: 0x0003B398
	public void OnSlotUpdatePending(int slotId)
	{
		if (slotId == this.SaveSlotID)
		{
			this.pendingChange = true;
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0003D1AC File Offset: 0x0003B3AC
	public void SetUpSlot(int slotNumber, Button backButton, bool autoSaveSlot, SaveScreenManager manager, ScrollRect scrollRectInParent, Camera camera, Button nextButton, bool closeOnSaveConfirmed)
	{
		this.BackButton = backButton;
		this.NextButton = nextButton;
		this.isAutoSave = autoSaveSlot;
		this.SaveSlotID = slotNumber;
		this.saveScreenManager = manager;
		this.parentScrollRect = scrollRectInParent;
		this.scrollRectCamera = camera;
		this.closeOnSaveConfirmed = closeOnSaveConfirmed;
		this.UpdateSlot();
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0003D1FC File Offset: 0x0003B3FC
	private void SetButtonColours(bool isAutosave)
	{
		ColorBlock colorBlock = this.LoadButton.colors;
		colorBlock.normalColor = (this.isAutoSave ? this.autosaveColour : this.normalSaveColour);
		this.LoadButton.colors = colorBlock;
		colorBlock = this.SaveButton.colors;
		colorBlock.normalColor = (this.isAutoSave ? this.autosaveColour : this.normalSaveColour);
		this.SaveButton.colors = colorBlock;
		this.Background.color = (this.isAutoSave ? this.autosaveColour : this.normalSaveColour);
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0003D294 File Offset: 0x0003B494
	public static bool IsAutoSaveSlot(int SaveSlotID)
	{
		return Services.SaveGameService.GetSlotInfo(SaveSlotID) != null && SaveSlotID < Save.MAX_AUTOSAVE_SLOTS;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0003D2B0 File Offset: 0x0003B4B0
	public void UpdateSlot()
	{
		SaveSlotMetadata slotInfo = Services.SaveGameService.GetSlotInfo(this.SaveSlotID);
		string text;
		if (slotInfo != null)
		{
			if (this.saveScreenManager != null && this.saveScreenManager.CurrentScreenState == SaveScreenManager.ScreenState.SAVE)
			{
				this.loadButtonActive = false;
				this.deleteButtonActive = true;
				this.saveButtonActive = true;
			}
			else
			{
				this.loadButtonActive = true;
				this.saveButtonActive = false;
				this.deleteButtonActive = false;
			}
			if (this.isAutoSave)
			{
				text = "[Auto] " + slotInfo.Name;
			}
			else
			{
				text = slotInfo.Name ?? "";
			}
			this.SetButtonColours(this.isAutoSave);
			this.lastPlayedTimestamp = slotInfo.TimeStamp;
			DateTime dateTime;
			if (Save.TryParseSavedDateTime(this.lastPlayedTimestamp, out dateTime))
			{
				this.Date.text = "Date: " + dateTime.ToShortDateString();
				this.Time.text = "Time: " + dateTime.ToShortTimeString();
			}
			else
			{
				this.Date.text = string.Empty;
				this.Time.text = string.Empty;
			}
			this.IsNewGamePlusSave = slotInfo.NewGamePlus;
			long num;
			if (long.TryParse(slotInfo.PlayTime, out num))
			{
				TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)num);
				this.playTime.text = string.Format("Time: {0:00}:{1:00}:{2:00}", timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
				this.playTime.gameObject.SetActive(true);
			}
			else
			{
				this.playTime.gameObject.SetActive(false);
			}
			this.daysPlayed.text = string.Format("No. Days: {0}", slotInfo.NumberOfDays + 1);
			int num2;
			int num3;
			if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
			{
				num2 = slotInfo.NumberOfDatablesAwakened + slotInfo.NumberOfDeluxeDatablesAwakened;
				num3 = slotInfo.NumberOfDatablesRealised + slotInfo.NumberOfDeluxeDatablesRealised;
			}
			else
			{
				num2 = slotInfo.NumberOfDatablesAwakened;
				num3 = slotInfo.NumberOfDatablesRealised;
			}
			this.activatedCharacters.text = num2.ToString();
			this.realisedCharacterUIParent.SetActive(num3 > 0);
			this.realisedCharacters.text = num3.ToString();
			base.gameObject.SetActive(true);
		}
		else
		{
			base.gameObject.SetActive(false);
			text = string.Format("{0}: Empty", this.SaveSlotID);
			this.Date.text = string.Empty;
			this.Time.text = string.Empty;
			this.loadButtonActive = false;
			this.saveButtonActive = !this.isAutoSave;
			this.deleteButtonActive = false;
			this.IsNewGamePlusSave = false;
			this.playTime.text = string.Empty;
			this.activatedCharacters.text = string.Empty;
			this.realisedCharacterUIParent.SetActive(false);
		}
		this.LoadButton.gameObject.SetActive(this.loadButtonActive);
		this.NewGamePlusIcon.SetActive(this.IsNewGamePlusSave);
		this.DeleteButton.gameObject.SetActive(this.deleteButtonActive);
		this.SaveButton.gameObject.SetActive(this.saveButtonActive);
		this.Name.text = text;
		this.SetBackground(this.IsNewGamePlusSave);
		this.SetLocalNavigation();
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0003D5F0 File Offset: 0x0003B7F0
	public void SetSlotStateSave()
	{
		this.loadButtonActive = false;
		this.deleteButtonActive = true;
		this.LoadButton.gameObject.SetActive(false);
		this.DeleteButton.gameObject.SetActive(true);
		this.SaveButton.gameObject.SetActive(true);
		this.SetLocalNavigation();
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0003D644 File Offset: 0x0003B844
	public void SetSlotStateLoad()
	{
		this.loadButtonActive = true;
		this.deleteButtonActive = false;
		this.LoadButton.gameObject.SetActive(true);
		this.DeleteButton.gameObject.SetActive(false);
		this.SaveButton.gameObject.SetActive(false);
		this.SetLocalNavigation();
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0003D698 File Offset: 0x0003B898
	private void SetLocalNavigation()
	{
		Selectable selectable = null;
		if (this.deleteButtonActive)
		{
			selectable = this.DeleteButton;
			Navigation navigation = this.DeleteButton.navigation;
			if (this.loadButtonActive)
			{
				navigation.selectOnLeft = this.LoadButton;
			}
			else if (this.saveButtonActive)
			{
				navigation.selectOnLeft = this.SaveButton;
			}
			navigation.selectOnRight = this.NextButton;
			navigation.mode = Navigation.Mode.Explicit;
			this.DeleteButton.navigation = navigation;
		}
		if (this.loadButtonActive)
		{
			Navigation navigation2 = this.LoadButton.navigation;
			navigation2.selectOnRight = selectable;
			navigation2.selectOnLeft = this.BackButton;
			navigation2.mode = Navigation.Mode.Explicit;
			this.LoadButton.navigation = navigation2;
		}
		if (this.saveButtonActive)
		{
			Navigation navigation3 = this.SaveButton.navigation;
			navigation3.selectOnRight = selectable;
			navigation3.selectOnLeft = this.BackButton;
			navigation3.mode = Navigation.Mode.Explicit;
			this.SaveButton.navigation = navigation3;
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0003D789 File Offset: 0x0003B989
	public Selectable GetSelectableButton()
	{
		if (this.loadButtonActive)
		{
			return this.LoadButton;
		}
		if (this.saveButtonActive)
		{
			return this.SaveButton;
		}
		return null;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0003D7AA File Offset: 0x0003B9AA
	public bool IsChangePending()
	{
		return this.pendingChange;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
	public void Selected(Selectable button)
	{
		if (this.BackButton != null && button != null)
		{
			Navigation navigation = this.BackButton.navigation;
			navigation.selectOnRight = button;
			navigation.selectOnUp = button;
			this.BackButton.navigation = navigation;
			if (this.NextButton != null)
			{
				navigation = this.NextButton.navigation;
				navigation.selectOnLeft = (this.deleteButtonActive ? this.DeleteButton : button);
				navigation.selectOnUp = button;
				this.NextButton.navigation = navigation;
			}
		}
		this.saveScreenManager.UpdateCurrentSlot(this, this.deleteButtonActive);
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0003D858 File Offset: 0x0003BA58
	public void Deselected(Selectable button)
	{
		this.saveScreenManager.DeselectThisSlot(this);
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x0003D866 File Offset: 0x0003BA66
	public void OnRetrySave(int slotId)
	{
		if (slotId == this.SaveSlotID)
		{
			this.TrySaveGameDataAsync(false, false, null, false);
		}
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0003D87B File Offset: 0x0003BA7B
	private void ResetCurrentOperation()
	{
		SaveSlot._currentOperation = SaveSlot.SlotOperation.None;
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0003D883 File Offset: 0x0003BA83
	private static bool RequestSetOperation(SaveSlot.SlotOperation operation)
	{
		if (!SaveSlot.CanDoNewOperation())
		{
			return false;
		}
		SaveSlot._currentOperation = operation;
		return true;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0003D895 File Offset: 0x0003BA95
	private static bool CanDoNewOperation()
	{
		return !SaveSlot.IsDoingOperation();
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x0003D8A1 File Offset: 0x0003BAA1
	public static bool IsDoingOperation()
	{
		return SaveSlot._currentOperation > SaveSlot.SlotOperation.None;
	}

	// Token: 0x04000999 RID: 2457
	public TextMeshProUGUI Name;

	// Token: 0x0400099A RID: 2458
	public TextMeshProUGUI Date;

	// Token: 0x0400099B RID: 2459
	public TextMeshProUGUI Time;

	// Token: 0x0400099C RID: 2460
	public Button SaveButton;

	// Token: 0x0400099D RID: 2461
	public Button LoadButton;

	// Token: 0x0400099E RID: 2462
	public Button DeleteButton;

	// Token: 0x0400099F RID: 2463
	public GameObject NewGamePlusIcon;

	// Token: 0x040009A0 RID: 2464
	public int SaveSlotID = -1;

	// Token: 0x040009A1 RID: 2465
	public Button BackButton;

	// Token: 0x040009A2 RID: 2466
	public bool IsNewGamePlusSave;

	// Token: 0x040009A3 RID: 2467
	public Image Background;

	// Token: 0x040009A4 RID: 2468
	public Sprite SpriteDefaultBg;

	// Token: 0x040009A5 RID: 2469
	public Sprite SpriteNewGamePlusBg;

	// Token: 0x040009A6 RID: 2470
	public Button NextButton;

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	private TextMeshProUGUI playTime;

	// Token: 0x040009A8 RID: 2472
	[SerializeField]
	private TextMeshProUGUI daysPlayed;

	// Token: 0x040009A9 RID: 2473
	[SerializeField]
	private TextMeshProUGUI activatedCharacters;

	// Token: 0x040009AA RID: 2474
	[SerializeField]
	private TextMeshProUGUI realisedCharacters;

	// Token: 0x040009AB RID: 2475
	[SerializeField]
	private GameObject realisedCharacterUIParent;

	// Token: 0x040009AC RID: 2476
	private string lastPlayedTimestamp;

	// Token: 0x040009AD RID: 2477
	private bool closeOnSaveConfirmed = true;

	// Token: 0x040009AE RID: 2478
	private bool isAutoSave;

	// Token: 0x040009AF RID: 2479
	private bool loadButtonActive;

	// Token: 0x040009B0 RID: 2480
	private bool saveButtonActive;

	// Token: 0x040009B1 RID: 2481
	private bool deleteButtonActive;

	// Token: 0x040009B2 RID: 2482
	private bool pendingChange;

	// Token: 0x040009B3 RID: 2483
	private SaveScreenManager saveScreenManager;

	// Token: 0x040009B4 RID: 2484
	private ScrollRect parentScrollRect;

	// Token: 0x040009B5 RID: 2485
	[SerializeField]
	private Color autosaveColour;

	// Token: 0x040009B6 RID: 2486
	[SerializeField]
	private Color normalSaveColour;

	// Token: 0x040009B7 RID: 2487
	private Camera scrollRectCamera;

	// Token: 0x040009B8 RID: 2488
	private static SaveSlot.SlotOperation _currentOperation;

	// Token: 0x02000336 RID: 822
	private enum SlotOperation
	{
		// Token: 0x040012B3 RID: 4787
		None,
		// Token: 0x040012B4 RID: 4788
		Loading,
		// Token: 0x040012B5 RID: 4789
		Saving,
		// Token: 0x040012B6 RID: 4790
		Deleting
	}
}
