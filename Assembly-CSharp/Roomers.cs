using System;
using System.Collections;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using Rewired;
using T17.Services;
using Team17.Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F9 RID: 249
public class Roomers : MonoBehaviour
{
	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000893 RID: 2195 RVA: 0x00032ED9 File Offset: 0x000310D9
	public Roomers.Roomers_Screen CurrentScreen
	{
		get
		{
			return this.currentScreen;
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00032EE1 File Offset: 0x000310E1
	private void Start()
	{
		if (Roomers.Instance != null && Roomers.Instance != this)
		{
			Object.Destroy(this);
			return;
		}
		Roomers.Instance = this;
		this.SwitchScreen(Roomers.Roomers_Screen.ActiveEntries);
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00032F11 File Offset: 0x00031111
	public void Awake()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.ActivateIfNeeded();
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00032F2C File Offset: 0x0003112C
	public void Update()
	{
		if (this.player == null)
		{
			this.player = (this.player = ReInput.players.GetPlayer(0));
			return;
		}
		if (this.RoomersWindow.activeSelf)
		{
			if (this.player.GetAxisRawDelta("Move Vertical") != 0f && this.currentScreen == Roomers.Roomers_Screen.AllEntries)
			{
				if (this.player.GetAxisRaw("Move Vertical") >= 0.99f)
				{
					this.switchEntry(-1);
				}
				else if (this.player.GetAxisRaw("Move Vertical") <= -0.99f)
				{
					this.switchEntry(1);
				}
			}
			else if (this.player.GetAxisRawDelta("Move Horizontal") != 0f && Mathf.Abs(this.player.GetAxisRaw("Move Horizontal")) >= 0.99f)
			{
				if (this.player.GetAxisRaw("Move Horizontal") > 0f && this.currentScreen < Roomers.Roomers_Screen.AllEntries)
				{
					this.SwitchScreen(this.currentScreen + 1);
				}
				else
				{
					this.OnClickBack();
				}
			}
			else if (this.player.GetButtonDown(50))
			{
				this.CycleScreen();
			}
			if (this.IsShowingEmptyList() && Services.UIInputService.IsUICancelDown())
			{
				this.PressBackButtonManually();
			}
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003306E File Offset: 0x0003126E
	private void PressBackButtonManually()
	{
		Button.ButtonClickedEvent onClick = this.backButton.GetComponent<Button>().onClick;
		if (onClick == null)
		{
			return;
		}
		onClick.Invoke();
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0003308A File Offset: 0x0003128A
	private bool IsShowingEmptyList()
	{
		return this.NoItemsToShow.activeSelf;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00033097 File Offset: 0x00031297
	private void SwitchScreen(Roomers.Roomers_Screen destination)
	{
		if (destination == this.currentScreen)
		{
			return;
		}
		switch (destination)
		{
		case Roomers.Roomers_Screen.AllEntries:
			this.currentScreen = Roomers.Roomers_Screen.AllEntries;
			return;
		case Roomers.Roomers_Screen.ActiveEntries:
			this.currentScreen = Roomers.Roomers_Screen.ActiveEntries;
			return;
		case Roomers.Roomers_Screen.CompletedEntries:
			this.currentScreen = Roomers.Roomers_Screen.CompletedEntries;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x000330D0 File Offset: 0x000312D0
	public void CycleScreen()
	{
		switch (this.currentScreen)
		{
		case Roomers.Roomers_Screen.AllEntries:
			this.currentScreen = Roomers.Roomers_Screen.ActiveEntries;
			this.GoToActiveEntries();
			return;
		case Roomers.Roomers_Screen.ActiveEntries:
			this.currentScreen = Roomers.Roomers_Screen.CompletedEntries;
			this.GoToCompletedEntries();
			return;
		case Roomers.Roomers_Screen.CompletedEntries:
			this.currentScreen = Roomers.Roomers_Screen.AllEntries;
			this.GoToAllEntries();
			return;
		default:
			return;
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00033120 File Offset: 0x00031320
	public void OnClickBack()
	{
		Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreen();
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0003312C File Offset: 0x0003132C
	public void LoadInitialDataOnGameLoad()
	{
		if (Singleton<Save>.Instance.GetRoomers().Count == 0)
		{
			Singleton<Save>.Instance.InitiateRoomersData();
		}
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0003314C File Offset: 0x0003134C
	public void SetupData()
	{
		DateADex.Instance.GetCharacterInternalNameMap();
		this.LoadInitialDataOnGameLoad();
		this.AllItems = new List<Save.RoomersStruct>();
		this.CompletedItems = new List<Save.RoomersStruct>();
		this.ActiveItems = new List<Save.RoomersStruct>();
		foreach (Save.RoomersStruct roomersStruct in Singleton<Save>.Instance.GetRoomers())
		{
			roomersStruct.isFound = false;
			if (roomersStruct.character == "skylar" && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_TALKED_TO_SKYLAR_DAY_TWO))
			{
				roomersStruct.isFound = false;
			}
			else if (roomersStruct.character == "skylar" && Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_TALKED_TO_SKYLAR_DAY_TWO))
			{
				roomersStruct.isFound = true;
			}
			else if (Singleton<Save>.Instance.GetDateStatus(roomersStruct.character) != RelationshipStatus.Unmet)
			{
				roomersStruct.isFound = true;
			}
			if (roomersStruct.isFound || roomersStruct.HasTipFound())
			{
				this.AllItems.Add(roomersStruct);
				if (roomersStruct.character == "skylar")
				{
					if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_TALKED_TO_SKYLAR_DAY_TWO) && roomersStruct.isFound)
					{
						this.CompletedItems.Add(roomersStruct);
					}
					else
					{
						this.ActiveItems.Add(roomersStruct);
					}
				}
				else if (roomersStruct.isFound)
				{
					this.CompletedItems.Add(roomersStruct);
				}
				else if (roomersStruct.HasTipFound())
				{
					this.ActiveItems.Add(roomersStruct);
				}
			}
		}
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x000332E8 File Offset: 0x000314E8
	public void SelectCurrentEntryButton()
	{
		if (this.RoomersEntries.Count <= this.currentEntry)
		{
			EventSystem.current.SetSelectedGameObject(null);
			return;
		}
		RoomersButton component = this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>();
		if (component == null)
		{
			T17Debug.LogError("Roomers button is null!");
			return;
		}
		ControllerMenuUI.SetCurrentlySelected(component.gameObject, ControllerMenuUI.Direction.Down, false, false);
		component.ButtonClicked();
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00033354 File Offset: 0x00031554
	public void SetupMenus()
	{
		this.RoomersEntries.ForEach(delegate(GameObject entry)
		{
			Object.Destroy(entry);
		});
		this.GalleryButtons.ForEach(delegate(GameObject entry)
		{
			Object.Destroy(entry);
		});
		this.RoomersEntryButtons.Clear();
		this.GalleryButtons.Clear();
		this.RoomersEntries.Clear();
		if (this.currentScreen == Roomers.Roomers_Screen.AllEntries)
		{
			this.PopulateItemList(this.AllItems);
		}
		if (this.currentScreen == Roomers.Roomers_Screen.ActiveEntries)
		{
			this.PopulateItemList(this.ActiveItems);
		}
		if (this.currentScreen == Roomers.Roomers_Screen.CompletedEntries)
		{
			this.PopulateItemList(this.CompletedItems);
		}
		this.OpenEntry();
		this.SelectCurrentEntryButton();
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00033420 File Offset: 0x00031620
	private void PopulateItemList(List<Save.RoomersStruct> list)
	{
		int num = 0;
		if (this.currentEntry == -1)
		{
			this.currentEntry = 0;
		}
		foreach (Save.RoomersStruct roomersStruct in list)
		{
			if (roomersStruct == null)
			{
				T17Debug.LogError("Entry was not found!");
			}
			else
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.roomerEntryButtonPrefab, this.RoomerButtonHolder.transform);
				RoomersEntryButton component = gameObject.GetComponent<RoomersEntryButton>();
				RoomersButton component2 = gameObject.GetComponent<RoomersButton>();
				this.RoomersEntryButtons.Add(component);
				this.RoomersEntries.Add(gameObject);
				component.index = num;
				component.UpdateContent(roomersStruct);
				component2.roomersInfo = this.roomersScreenInfo;
				if (component.roomersEntry.hasNew)
				{
					component.newImage.enabled = true;
				}
				num++;
			}
		}
		if (list.Count == 0)
		{
			this.NoItemsToShow.SetActive(true);
		}
		else
		{
			this.NoItemsToShow.SetActive(false);
		}
		this.roomersScreenInfo.ClearPanel();
		this.StaggerEntries();
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0003353C File Offset: 0x0003173C
	private void StaggerEntries()
	{
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0003353E File Offset: 0x0003173E
	public void OnEntryFocused(ListBox prevFocusingBox, ListBox curFocusingBox)
	{
		Save.RoomersStruct roomersEntry = (curFocusingBox as RoomersEntryButton).roomersEntry;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0003354C File Offset: 0x0003174C
	public void OpenEntry()
	{
		if (this.RoomersEntries.Count > this.currentEntry)
		{
			RoomersButton roomersButton = this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>();
			roomersButton = this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>();
			roomersButton.roomersEntryButton.roomersEntry.hasNew = false;
			roomersButton.roomersEntryButton.newImage.enabled = false;
			roomersButton.roomersInfo.SetUpPanel(roomersButton.roomersEntryButton.roomersEntry);
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x000335D4 File Offset: 0x000317D4
	public void switchEntry(int dir, bool andDisplayInfo = false)
	{
		if (this.RoomersEntries != null)
		{
			int num = (this.currentEntry + dir) % this.RoomersEntries.Count;
			if (num < 0)
			{
				num = this.RoomersEntries.Count - 1;
			}
			this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>().AnimateDeselected();
			this.currentEntry = num;
			RoomersButton component = this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>();
			component.AnimateSelected();
			component.OnPointerEnter(null);
			if (andDisplayInfo)
			{
				this.OpenEntry();
			}
		}
		this.StaggerEntries();
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00033664 File Offset: 0x00031864
	public void switchEntry(int dir)
	{
		if (this.RoomersEntries != null && this.RoomersEntries.Count > 0)
		{
			int num = (this.currentEntry + dir) % this.RoomersEntries.Count;
			if (num < 0)
			{
				num = this.RoomersEntries.Count - 1;
			}
			this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>().AnimateDeselected();
			this.currentEntry = num;
			RoomersButton component = this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>();
			component.AnimateSelected();
			component.OnPointerEnter(null);
			this.OpenEntry();
		}
		this.StaggerEntries();
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00033700 File Offset: 0x00031900
	public void switchEntryIndex(int index, bool andDisplayInfo = false)
	{
		if (this.RoomersEntries != null)
		{
			int num = index;
			if (num < 0)
			{
				num = this.RoomersEntries.Count - 1;
			}
			RoomersButton roomersButton = this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>();
			roomersButton.AnimateDeselected();
			this.currentEntry = num;
			roomersButton = this.RoomersEntries[this.currentEntry].GetComponent<RoomersButton>();
			roomersButton.AnimateSelected();
			roomersButton.OnPointerEnter(null);
			if (andDisplayInfo)
			{
				roomersButton.roomersEntryButton.newImage.enabled = false;
				roomersButton.roomersInfo.SetUpPanel(roomersButton.roomersEntryButton.roomersEntry);
			}
		}
		this.StaggerEntries();
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x000337A4 File Offset: 0x000319A4
	public string GetInternalName(string name)
	{
		return Singleton<CharacterHelper>.Instance._characters.GetInternalName(name.ToLowerInvariant());
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x000337BB File Offset: 0x000319BB
	public string GetInkFileNamePrefix(string name)
	{
		if (name.Contains("."))
		{
			return name.Replace(".ink", "").Split('.', StringSplitOptions.None)[0];
		}
		return name.Replace(".ink", "");
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x000337F5 File Offset: 0x000319F5
	private IEnumerator SelectBackButton()
	{
		if (this.backButton.GetComponent<ControllerDisplayComponent>().IsSelectionBlocked())
		{
			IsSelectableRegistered isSelectableRegistered = this.sortButton;
		}
		else
		{
			IsSelectableRegistered isSelectableRegistered2 = this.backButton;
		}
		yield return new WaitUntil(() => this.sortButton.IsRegistered);
		ControllerMenuUI.SetCurrentlySelected(this.sortButton.gameObject, ControllerMenuUI.Direction.Down, false, false);
		yield break;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00033804 File Offset: 0x00031A04
	private void ActivateIfNeeded()
	{
		this.roomersScreen.SetActive(false);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0003381E File Offset: 0x00031A1E
	public void EnableScreen()
	{
		this.roomersScreen.SetActive(true);
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0003382C File Offset: 0x00031A2C
	public void DisableScreen()
	{
		this.roomersScreen.SetActive(false);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0003383C File Offset: 0x00031A3C
	public Save.RoomersStruct GetEntry(string name)
	{
		foreach (Save.RoomersStruct roomersStruct in this.AllItems)
		{
			if (roomersStruct.character == name)
			{
				return roomersStruct;
			}
		}
		return null;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x000338A0 File Offset: 0x00031AA0
	private void ClearGalleryButtons()
	{
		this.GalleryButtons.ForEach(delegate(GameObject x)
		{
			Object.Destroy(x);
		});
		this.GalleryButtons.Clear();
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x000338D7 File Offset: 0x00031AD7
	public void GoToAllEntries()
	{
		this.currentEntry = 0;
		this.currentScreen = Roomers.Roomers_Screen.AllEntries;
		this.ClearGalleryButtons();
		this.SetupMenus();
		this.StaggerEntries();
		this.screenNameText.text = "ALL";
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00033909 File Offset: 0x00031B09
	public void GoToActiveEntries()
	{
		this.currentEntry = 0;
		this.currentScreen = Roomers.Roomers_Screen.ActiveEntries;
		this.ClearGalleryButtons();
		this.SetupMenus();
		this.StaggerEntries();
		this.screenNameText.text = "ACTIVE";
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0003393B File Offset: 0x00031B3B
	public void GoToCompletedEntries()
	{
		this.currentEntry = 0;
		this.currentScreen = Roomers.Roomers_Screen.CompletedEntries;
		this.ClearGalleryButtons();
		this.SetupMenus();
		this.StaggerEntries();
		this.screenNameText.text = "RESOLVED";
	}

	// Token: 0x040007DC RID: 2012
	public static Roomers Instance;

	// Token: 0x040007DD RID: 2013
	[Header("Status")]
	[SerializeField]
	private int currentEntry;

	// Token: 0x040007DE RID: 2014
	[SerializeField]
	private Roomers.Roomers_Screen currentScreen = Roomers.Roomers_Screen.ActiveEntries;

	// Token: 0x040007DF RID: 2015
	public GameObject RoomerButtonHolder;

	// Token: 0x040007E0 RID: 2016
	public GameObject NoItemsToShow;

	// Token: 0x040007E1 RID: 2017
	[Header("Roomers Info")]
	[SerializeField]
	private List<Save.RoomersStruct> AllItems;

	// Token: 0x040007E2 RID: 2018
	[SerializeField]
	private List<Save.RoomersStruct> ActiveItems;

	// Token: 0x040007E3 RID: 2019
	[SerializeField]
	private List<Save.RoomersStruct> CompletedItems;

	// Token: 0x040007E4 RID: 2020
	[SerializeField]
	private List<GameObject> RoomersEntries = new List<GameObject>();

	// Token: 0x040007E5 RID: 2021
	[SerializeField]
	private List<RoomersEntryButton> RoomersEntryButtons = new List<RoomersEntryButton>();

	// Token: 0x040007E6 RID: 2022
	[Header("Gallery")]
	[SerializeField]
	private List<GameObject> GalleryButtons = new List<GameObject>();

	// Token: 0x040007E7 RID: 2023
	[Header("Other References")]
	public RoomersInfo roomersScreenInfo;

	// Token: 0x040007E8 RID: 2024
	public GameObject roomersScreen;

	// Token: 0x040007E9 RID: 2025
	public Image screenNameIcon;

	// Token: 0x040007EA RID: 2026
	public TextMeshProUGUI screenNameText;

	// Token: 0x040007EB RID: 2027
	public GameObject RoomersWindow;

	// Token: 0x040007EC RID: 2028
	[SerializeField]
	private IsSelectableRegistered backButton;

	// Token: 0x040007ED RID: 2029
	[SerializeField]
	private IsSelectableRegistered sortButton;

	// Token: 0x040007EE RID: 2030
	[Header("Prefabs")]
	public GameObject roomerEntryButtonPrefab;

	// Token: 0x040007EF RID: 2031
	public GameObject autoSelectFallback;

	// Token: 0x040007F0 RID: 2032
	[Header("Rewired")]
	private Player player;

	// Token: 0x02000315 RID: 789
	public enum Roomers_Screen
	{
		// Token: 0x0400124E RID: 4686
		AllEntries,
		// Token: 0x0400124F RID: 4687
		ActiveEntries,
		// Token: 0x04001250 RID: 4688
		CompletedEntries
	}
}
