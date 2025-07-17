using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Date_Everything.Scripts.UI;
using Rewired;
using T17.Services;
using T17.UI;
using Team17.Scripts.UI_Components;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012F RID: 303
public class SaveScreenManager : MonoBehaviour
{
	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0003BCC5 File Offset: 0x00039EC5
	public SaveScreenManager.ScreenState CurrentScreenState
	{
		get
		{
			return this._currentScreenState;
		}
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0003BCD0 File Offset: 0x00039ED0
	private void Awake()
	{
		if (this.BackButton != null && this.BackButton.GetComponent<CancelButtonTrigger>() == null)
		{
			this.BackButton.gameObject.AddComponent<CancelButtonTrigger>();
		}
		GameObject gameObject = this.newSaveSlot;
		Button button = ((gameObject != null) ? gameObject.GetComponentInChildren<Button>() : null);
		if (button == null)
		{
			return;
		}
		button.gameObject.AddComponent<DisableButtonIfAnySlotOperationIsActive>();
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0003BD34 File Offset: 0x00039F34
	private void Start()
	{
		this.CreateSaveSlots();
		Services.SaveGameService.OnSlotUpdated -= this.OnSlotUpdated;
		Services.SaveGameService.OnSlotUpdated += this.OnSlotUpdated;
		this.UpdateBackButtonNav();
		this.UpdateNextButtonNav();
		this.UpdateBackAction();
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0003BD88 File Offset: 0x00039F88
	private void UpdateBackAction()
	{
		if (this.BackButton.GetComponentInChildren<ActionOnDisable>() != null)
		{
			ActionOnDisable componentInChildren = this.BackButton.GetComponentInChildren<ActionOnDisable>();
			componentInChildren.onDisable = (Action)Delegate.Combine(componentInChildren.onDisable, new Action(this.RefocusFromDisabledBackButton));
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0003BDD4 File Offset: 0x00039FD4
	public void SetAvailableTabs(SaveScreenManager.ScreenState[] availableStates, SaveScreenManager.ScreenState startState = SaveScreenManager.ScreenState.LOAD)
	{
		foreach (SaveScreenManager.StateTabKVP stateTabKVP in this._stateTabLookup)
		{
			stateTabKVP.TabObj.SetActive(availableStates.Contains(stateTabKVP.ScreenState));
			SetTextColourOnSelect componentInChildren = stateTabKVP.TabObj.GetComponentInChildren<SetTextColourOnSelect>();
			if (componentInChildren != null)
			{
				componentInChildren.SetSelectedColour(stateTabKVP.ScreenState == startState);
			}
		}
		this._currentScreenState = startState;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0003BE40 File Offset: 0x0003A040
	public void ShowOnlyLoadTab()
	{
		this.SetAvailableTabs(new SaveScreenManager.ScreenState[1], SaveScreenManager.ScreenState.LOAD);
		this.showBothTabs = false;
		this._loadTitle.SetActive(false);
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0003BE62 File Offset: 0x0003A062
	public void ShowNoTabs()
	{
		this.SetAvailableTabs(new SaveScreenManager.ScreenState[] { SaveScreenManager.ScreenState.NONE }, SaveScreenManager.ScreenState.LOAD);
		this.showBothTabs = false;
		this._loadTitle.SetActive(true);
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0003BE88 File Offset: 0x0003A088
	public void ShowOnlySaveTab()
	{
		this.SetAvailableTabs(new SaveScreenManager.ScreenState[] { SaveScreenManager.ScreenState.SAVE }, SaveScreenManager.ScreenState.SAVE);
		this.showBothTabs = false;
		this._loadTitle.SetActive(false);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0003BEAE File Offset: 0x0003A0AE
	public void ShowAllTabs()
	{
		this.SetAvailableTabs(new SaveScreenManager.ScreenState[]
		{
			SaveScreenManager.ScreenState.LOAD,
			SaveScreenManager.ScreenState.SAVE
		}, SaveScreenManager.ScreenState.SAVE);
		this.showBothTabs = true;
		this._loadTitle.SetActive(false);
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0003BED4 File Offset: 0x0003A0D4
	private void OnDestroy()
	{
		Services.SaveGameService.OnSlotUpdated -= this.OnSlotUpdated;
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0003BEEC File Offset: 0x0003A0EC
	public void OnSlotUpdated(int slotId)
	{
		this.pendingNavUpdate = true;
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0003BEF8 File Offset: 0x0003A0F8
	public void CreateSaveSlots()
	{
		for (int i = this.slots.Count - 1; i >= 0; i--)
		{
			Object.Destroy(this.slots[i].gameObject);
		}
		this.slots.Clear();
		this.firstEmptySlot = null;
		for (int j = 0; j < Save.MAX_SAVE_SLOTS; j++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.SaveSlotPrefab, this.ScrollContentSave.transform);
			gameObject.name = string.Format("{0}", j);
			SaveSlot component = gameObject.GetComponent<SaveSlot>();
			this.slots.Add(component);
			component.SetUpSlot(j, this.BackButton, j < Save.MAX_AUTOSAVE_SLOTS, this, this.scrollRect, this.menuCamera, this.NextButton, this.closeOnSaveConfirmed);
		}
		this.scrollbar.value = 1f;
		if (this.scrollRect != null)
		{
			this.scrollRect.verticalNormalizedPosition = 1f;
		}
		this.UpdateNavigation();
		this.FilterSlots();
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0003BFFC File Offset: 0x0003A1FC
	public void CreateNewSave()
	{
		this.firstEmptySlot.TrySaveGameDataAsync(!this.IsNewGamePlusScreen, false, this.BackButton, this.IsNewGamePlusScreen && Singleton<Save>.Instance.GetNewGamePlus());
		this.firstEmptySlot.UpdateSlot();
		this.firstEmptySlot.SetSlotStateSave();
		this.newSaveSlot.SetActive(this.ScrollContentSave.transform.childCount < Save.MAX_SAVE_SLOTS);
		this.FilterSlots();
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0003C077 File Offset: 0x0003A277
	private void OnEnable()
	{
		this.CreateSaveSlots();
		this.UpdateBackButtonNav();
		this.UpdateNextButtonNav();
		if (this.deleteGlyph != null)
		{
			this.deleteGlyph.OnISOkToDisplayGlyth += this.ShouldWeDisplayDeleteGlyph;
		}
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0003C0B0 File Offset: 0x0003A2B0
	private void OnDisable()
	{
		if (this.deleteGlyph != null)
		{
			this.deleteGlyph.OnISOkToDisplayGlyth -= this.ShouldWeDisplayDeleteGlyph;
		}
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0003C0D8 File Offset: 0x0003A2D8
	private void ShouldWeDisplayDeleteGlyph(ControllerGlyphComponent.ResultEvent result)
	{
		if (this._currentScreenState == SaveScreenManager.ScreenState.LOAD)
		{
			result.result = false;
			return;
		}
		if (this.currentSlot != null)
		{
			if (this.currentSlot.IsAutoSave)
			{
				result.result = false;
				return;
			}
		}
		else if (this.currentSlot == null)
		{
			result.result = false;
		}
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0003C12D File Offset: 0x0003A32D
	public void SelectBackButton()
	{
		base.StartCoroutine(this.SetBackButtonSelected());
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0003C13C File Offset: 0x0003A33C
	private IEnumerator SetBackButtonSelected()
	{
		yield return new WaitUntil(() => this.scrollRect.IsActive());
		yield return new WaitUntil(() => !Services.SaveGameService.IsBusy);
		GameObject gameObject;
		if (this._currentScreenState == SaveScreenManager.ScreenState.SAVE && this.newSaveSlot.activeInHierarchy)
		{
			gameObject = this.newSaveSlot.transform.GetChild(0).gameObject;
		}
		else if (this.GetFirstselectableButtonOnSlot() != null)
		{
			gameObject = this.GetFirstselectableButtonOnSlot().gameObject;
		}
		else
		{
			gameObject = this.BackButton.gameObject;
		}
		ControllerMenuUI.SetCurrentlySelected(gameObject.gameObject, ControllerMenuUI.Direction.Down, false, false);
		yield break;
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0003C14C File Offset: 0x0003A34C
	private void UpdateBackButtonNav()
	{
		if (this.slots.Count > 0 && this.BackButton != null)
		{
			Navigation navigation = this.BackButton.navigation;
			Selectable firstselectableButtonOnSlot = this.GetFirstselectableButtonOnSlot();
			navigation.selectOnUp = null;
			if (firstselectableButtonOnSlot != null)
			{
				navigation.selectOnRight = firstselectableButtonOnSlot;
				navigation.selectOnUp = firstselectableButtonOnSlot;
			}
			else
			{
				navigation.selectOnUp = this._stateTabLookup.First((SaveScreenManager.StateTabKVP x) => x.ScreenState == SaveScreenManager.ScreenState.LOAD).TabObj.GetComponent<Selectable>();
			}
			if (this.IsNewGamePlusScreen)
			{
				navigation.selectOnRight = this.NextButton;
			}
			navigation.mode = Navigation.Mode.Explicit;
			this.BackButton.navigation = navigation;
		}
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0003C218 File Offset: 0x0003A418
	private void UpdateNextButtonNav()
	{
		if (this.IsNewGamePlusScreen && this.slots.Count > 0 && this.NextButton != null)
		{
			Navigation navigation = this.NextButton.navigation;
			Selectable firstselectableButtonOnSlot = this.GetFirstselectableButtonOnSlot();
			navigation.selectOnUp = null;
			if (firstselectableButtonOnSlot != null)
			{
				navigation.selectOnUp = firstselectableButtonOnSlot;
			}
			navigation.selectOnLeft = this.BackButton;
			navigation.mode = Navigation.Mode.Explicit;
			this.NextButton.navigation = navigation;
		}
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0003C298 File Offset: 0x0003A498
	private Selectable GetFirstselectableButtonOnSlot()
	{
		SaveSlot saveSlot = this.slots.FirstOrDefault((SaveSlot x) => x.HasData && x.gameObject.activeSelf);
		if (saveSlot != null)
		{
			return saveSlot.GetSelectableButton();
		}
		return null;
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0003C2E4 File Offset: 0x0003A4E4
	private void Update()
	{
		if (this.pendingNavUpdate && this.slots.Count > 0)
		{
			bool flag = false;
			int num = 0;
			int count = this.slots.Count;
			while (num < count && !flag)
			{
				flag = this.slots[num].IsChangePending();
				num++;
			}
			if (!flag)
			{
				this.pendingNavUpdate = false;
				this.UpdateNavigation();
			}
		}
		if (this.inputEnabled)
		{
			this.ProcessInput();
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0003C354 File Offset: 0x0003A554
	private void ProcessInput()
	{
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
		}
		if (this._currentScreenState == SaveScreenManager.ScreenState.SAVE && this.player.GetButtonDown(38))
		{
			GameObject tabObj = this._stateTabLookup.First((SaveScreenManager.StateTabKVP x) => x.ScreenState == SaveScreenManager.ScreenState.LOAD).TabObj;
			if (tabObj.GetComponent<Button>().interactable && tabObj.activeInHierarchy)
			{
				Button.ButtonClickedEvent onClick = tabObj.GetComponent<Button>().onClick;
				if (onClick != null)
				{
					onClick.Invoke();
				}
			}
		}
		if (this._currentScreenState == SaveScreenManager.ScreenState.LOAD && this.player.GetButtonDown(37))
		{
			GameObject tabObj2 = this._stateTabLookup.First((SaveScreenManager.StateTabKVP x) => x.ScreenState == SaveScreenManager.ScreenState.SAVE).TabObj;
			if (tabObj2.GetComponent<Button>().interactable && tabObj2.activeInHierarchy)
			{
				Button.ButtonClickedEvent onClick2 = tabObj2.GetComponent<Button>().onClick;
				if (onClick2 != null)
				{
					onClick2.Invoke();
				}
			}
		}
		if (this.player.GetButtonDown(50) && this.currentSlot != null && this.currentSlot.DeleteButton.gameObject.activeInHierarchy)
		{
			Button.ButtonClickedEvent onClick3 = this.currentSlot.DeleteButton.GetComponent<Button>().onClick;
			if (onClick3 == null)
			{
				return;
			}
			onClick3.Invoke();
		}
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0003C4B2 File Offset: 0x0003A6B2
	public void OnSaveDeleted()
	{
		this.UpdateNavigation();
		this.SelectBackButton();
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0003C4C0 File Offset: 0x0003A6C0
	private void UpdateNavigation()
	{
		Button componentInChildren = this.newSaveSlot.GetComponentInChildren<Button>();
		SaveSlot saveSlot = null;
		Selectable selectable = null;
		if (this.BackButton != null)
		{
			int i = 0;
			int count = this.slots.Count;
			while (i < count)
			{
				SaveSlot saveSlot2 = this.slots[i];
				if (saveSlot2.gameObject.activeSelf)
				{
					if (selectable != null)
					{
						Selectable selectableButton = saveSlot2.GetSelectableButton();
						if (selectableButton != null)
						{
							Navigation navigation = selectable.navigation;
							navigation.selectOnDown = selectableButton;
							selectable.navigation = navigation;
							navigation = selectableButton.navigation;
							navigation.selectOnUp = selectable;
							navigation.selectOnLeft = this.BackButton;
							selectableButton.navigation = navigation;
							selectable = selectableButton;
						}
					}
					else
					{
						selectable = saveSlot2.GetSelectableButton();
						if (selectable != null)
						{
							saveSlot = saveSlot2;
							Navigation navigation2 = this.BackButton.navigation;
							navigation2.selectOnRight = ((this.NextButton == null) ? selectable : this.NextButton);
							navigation2.selectOnUp = selectable;
							navigation2.mode = Navigation.Mode.Explicit;
							this.BackButton.navigation = navigation2;
							Navigation navigation3 = selectable.navigation;
							if (this._currentScreenState == SaveScreenManager.ScreenState.SAVE)
							{
								navigation3.selectOnUp = componentInChildren;
							}
							else if (this._currentScreenState == SaveScreenManager.ScreenState.LOAD)
							{
								navigation3.selectOnUp = this._stateTabLookup.First((SaveScreenManager.StateTabKVP x) => x.ScreenState == SaveScreenManager.ScreenState.LOAD).TabObj.GetComponent<Selectable>();
							}
							navigation3.selectOnLeft = this.BackButton;
							selectable.navigation = navigation3;
						}
					}
				}
				i++;
			}
			if (selectable != null)
			{
				Navigation navigation4 = selectable.navigation;
				navigation4.selectOnDown = (this.IsNewGamePlusScreen ? this.NextButton : this.BackButton);
				selectable.navigation = navigation4;
			}
		}
		Navigation navigation5 = componentInChildren.navigation;
		navigation5.selectOnUp = this._stateTabLookup.First((SaveScreenManager.StateTabKVP x) => x.ScreenState == this._currentScreenState).TabObj.GetComponent<Selectable>();
		if (saveSlot != null)
		{
			navigation5.selectOnDown = saveSlot.GetSelectableButton();
		}
		else
		{
			navigation5.selectOnDown = this.slots[0].gameObject.GetComponent<Selectable>();
		}
		componentInChildren.navigation = navigation5;
		for (int j = 0; j < this._stateTabLookup.Length; j++)
		{
			Selectable component = this._stateTabLookup[j].TabObj.GetComponent<Selectable>();
			Navigation navigation6 = component.navigation;
			if (this._currentScreenState == SaveScreenManager.ScreenState.SAVE)
			{
				navigation6.selectOnDown = componentInChildren;
			}
			else if (this.slots.Count > 0)
			{
				SaveSlot saveSlot3 = this.slots.FirstOrDefault((SaveSlot x) => x.HasData);
				if (saveSlot3 != null)
				{
					navigation6.selectOnDown = saveSlot3.GetSelectableButton();
				}
			}
			if (this.showBothTabs)
			{
				if (j == 0)
				{
					navigation6.selectOnLeft = this.BackButton;
					navigation6.selectOnRight = this._stateTabLookup[j + 1].TabObj.GetComponent<Selectable>();
				}
				else
				{
					navigation6.selectOnLeft = this._stateTabLookup[j - 1].TabObj.GetComponent<Selectable>();
					navigation6.selectOnRight = null;
				}
			}
			else
			{
				navigation6.selectOnRight = null;
				navigation6.selectOnLeft = null;
			}
			navigation6.selectOnUp = null;
			navigation6.mode = Navigation.Mode.Explicit;
			component.navigation = navigation6;
		}
		if (selectable != null)
		{
			this.autoSelectFallback = selectable.gameObject;
		}
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0003C84E File Offset: 0x0003AA4E
	public void TriggerLoadAnimation()
	{
		DoCodeAnimation doCodeAnimation = this.loadDOAnimator;
		if (doCodeAnimation == null)
		{
			return;
		}
		doCodeAnimation.Trigger();
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0003C860 File Offset: 0x0003AA60
	public void ShowSave()
	{
		if (this._currentScreenState == SaveScreenManager.ScreenState.SAVE)
		{
			return;
		}
		this._currentScreenState = SaveScreenManager.ScreenState.SAVE;
		this.UpdateTabs();
		this.FilterSlots();
		if (this.newSaveSlot.activeInHierarchy)
		{
			this.SelectNewSaveSlot();
			return;
		}
		this.SelectFirstSaveSlot();
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0003C899 File Offset: 0x0003AA99
	public void ShowLoad()
	{
		if (this._currentScreenState == SaveScreenManager.ScreenState.LOAD)
		{
			return;
		}
		this._currentScreenState = SaveScreenManager.ScreenState.LOAD;
		this.UpdateTabs();
		this.FilterSlots();
		this.SelectFirstSaveSlot();
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0003C8BD File Offset: 0x0003AABD
	private void SelectFirstElement()
	{
		if (this.newSaveSlot.activeInHierarchy)
		{
			this.SelectNewSaveSlot();
			return;
		}
		this.SelectFirstSaveSlot();
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0003C8D9 File Offset: 0x0003AAD9
	private void SelectFirstSaveSlot()
	{
		this.SelectSlot(this.GetFirstselectableButtonOnSlot().gameObject);
		this.scrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0003C8FC File Offset: 0x0003AAFC
	public void SelectNewSaveSlot()
	{
		this.SelectSlot(this.newSaveSlot.transform.GetChild(0).gameObject);
		this.scrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0003C92A File Offset: 0x0003AB2A
	private void SelectSlot(GameObject slotToSelect)
	{
		if (slotToSelect != null && slotToSelect.activeInHierarchy)
		{
			ControllerMenuUI.SetCurrentlySelected(slotToSelect, ControllerMenuUI.Direction.Down, false, false);
		}
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0003C948 File Offset: 0x0003AB48
	private void RefocusFromDisabledBackButton()
	{
		if (this.BackButton.gameObject == ControllerMenuUI.GetCurrentSelectedControl())
		{
			if (this.player == null)
			{
				this.player = ReInput.players.GetPlayer(0);
			}
			if (this.player.GetButtonDown(28))
			{
				base.StartCoroutine(this.RefocusBackButtonDelay());
				return;
			}
			this.SelectFirstElement();
		}
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0003C9A8 File Offset: 0x0003ABA8
	private IEnumerator RefocusBackButtonDelay()
	{
		this.backButtonLostFocus = true;
		yield return new WaitForSeconds(0.3f);
		if (this.backButtonLostFocus)
		{
			this.backButtonLostFocus = false;
			this.RefocusFromDisabledBackButton();
		}
		yield break;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0003C9B7 File Offset: 0x0003ABB7
	private void FlagBackButtonLostFocus()
	{
		if (this.BackButton.gameObject == ControllerMenuUI.GetCurrentSelectedControl())
		{
			base.StartCoroutine(this.RefocusBackButtonDelay());
		}
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0003C9DD File Offset: 0x0003ABDD
	public void CancelBackLostFocus()
	{
		this.backButtonLostFocus = false;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0003C9E8 File Offset: 0x0003ABE8
	private void UpdateTabs()
	{
		foreach (SaveScreenManager.StateTabKVP stateTabKVP in this._stateTabLookup)
		{
			SetTextColourOnSelect componentInChildren = stateTabKVP.TabObj.GetComponentInChildren<SetTextColourOnSelect>();
			if (componentInChildren != null)
			{
				componentInChildren.SetSelectedColour(stateTabKVP.ScreenState == this._currentScreenState);
			}
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0003CA3C File Offset: 0x0003AC3C
	private void FilterSlots()
	{
		this.firstEmptySlot = null;
		this.SortSlots();
		this.newSaveSlot.SetActive(this._currentScreenState == SaveScreenManager.ScreenState.SAVE && this.ScrollContentSave.transform.childCount < Save.MAX_SAVE_SLOTS);
		int num = 0;
		for (int i = 0; i < this.slots.Count; i++)
		{
			SaveSlot saveSlot = this.slots[i];
			if (!(saveSlot == null))
			{
				if (saveSlot.IsAutoSave)
				{
					saveSlot.gameObject.SetActive(this._currentScreenState == SaveScreenManager.ScreenState.LOAD && saveSlot.HasData);
					num++;
				}
				else
				{
					saveSlot.gameObject.SetActive(saveSlot.HasData);
					if (saveSlot.HasData)
					{
						num++;
					}
				}
				if (!saveSlot.gameObject.activeSelf)
				{
					if (!saveSlot.IsAutoSave)
					{
						if (this.firstEmptySlot == null)
						{
							this.firstEmptySlot = saveSlot;
						}
						else if (saveSlot.SaveSlotID < this.firstEmptySlot.SaveSlotID)
						{
							this.firstEmptySlot = saveSlot;
						}
					}
				}
				else
				{
					SaveScreenManager.ScreenState currentScreenState = this._currentScreenState;
					if (currentScreenState != SaveScreenManager.ScreenState.LOAD)
					{
						if (currentScreenState == SaveScreenManager.ScreenState.SAVE)
						{
							saveSlot.SetSlotStateSave();
						}
					}
					else
					{
						saveSlot.SetSlotStateLoad();
					}
				}
			}
		}
		this.newSaveSlot.SetActive(this._currentScreenState == SaveScreenManager.ScreenState.SAVE && num < Save.MAX_SAVE_SLOTS);
		this.UpdateNavigation();
		this.UpdateBackButtonNav();
		this.UpdateNextButtonNav();
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0003CB98 File Offset: 0x0003AD98
	private long OrderSaveSlotFunc(SaveSlot x)
	{
		long num = 0L;
		if (x.HasData)
		{
			try
			{
				num = DateTime.Parse(x.LastPlayedTimestamp, CultureInfo.CurrentCulture.DateTimeFormat).Ticks;
			}
			catch
			{
			}
		}
		return num;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0003CBE4 File Offset: 0x0003ADE4
	private void SortSlots()
	{
		this.slots = this.slots.OrderBy(new Func<SaveSlot, long>(this.OrderSaveSlotFunc)).Reverse<SaveSlot>().ToList<SaveSlot>();
		int num = 0;
		List<SaveSlot> list = new List<SaveSlot>();
		for (int i = 0; i < this.slots.Count; i++)
		{
			if (this.slots[i].IsAutoSave)
			{
				this.slots[i].transform.SetSiblingIndex(num + 1);
				list.Add(this.slots[i]);
				num++;
			}
		}
		for (int j = 0; j < this.slots.Count; j++)
		{
			SaveSlot saveSlot = this.slots[j];
			if (!list.Contains(saveSlot) && !saveSlot.IsAutoSave)
			{
				saveSlot.transform.SetSiblingIndex(j + 1 + num);
				list.Add(saveSlot);
			}
		}
		this.slots = new List<SaveSlot>(list);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0003CCD4 File Offset: 0x0003AED4
	public void OnNewSaveSlotSelected(Selectable button)
	{
		if (this.BackButton != null)
		{
			Navigation navigation = this.BackButton.navigation;
			navigation.selectOnRight = button;
			navigation.selectOnUp = button;
			this.BackButton.navigation = navigation;
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0003CD17 File Offset: 0x0003AF17
	public void UpdateCurrentSlot(SaveSlot slot, bool deletable)
	{
		this.currentSlot = slot;
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0003CD20 File Offset: 0x0003AF20
	public void UpdateInputState(bool value)
	{
		this.inputEnabled = value;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0003CD29 File Offset: 0x0003AF29
	public void DeselectThisSlot(SaveSlot slot)
	{
		if (slot == this.currentSlot)
		{
			this.currentSlot = null;
		}
	}

	// Token: 0x04000980 RID: 2432
	public Button BackButton;

	// Token: 0x04000981 RID: 2433
	public Button NextButton;

	// Token: 0x04000982 RID: 2434
	public GameObject ScrollContentSave;

	// Token: 0x04000983 RID: 2435
	public GameObject SaveSlotPrefab;

	// Token: 0x04000984 RID: 2436
	public Scrollbar scrollbar;

	// Token: 0x04000985 RID: 2437
	public bool IsNewGamePlusScreen;

	// Token: 0x04000986 RID: 2438
	[SerializeField]
	private DoCodeAnimation loadDOAnimator;

	// Token: 0x04000987 RID: 2439
	[SerializeField]
	private GameObject newSaveSlot;

	// Token: 0x04000988 RID: 2440
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04000989 RID: 2441
	[SerializeField]
	private Camera menuCamera;

	// Token: 0x0400098A RID: 2442
	[SerializeField]
	private bool closeOnSaveConfirmed = true;

	// Token: 0x0400098B RID: 2443
	private bool pendingNavUpdate;

	// Token: 0x0400098C RID: 2444
	private List<SaveSlot> slots = new List<SaveSlot>(Save.MAX_SAVE_SLOTS);

	// Token: 0x0400098D RID: 2445
	public int kVisibleSlots = 6;

	// Token: 0x0400098E RID: 2446
	private SaveSlot firstEmptySlot;

	// Token: 0x0400098F RID: 2447
	private bool showBothTabs;

	// Token: 0x04000990 RID: 2448
	public GameObject autoSelectFallback;

	// Token: 0x04000991 RID: 2449
	private Player player;

	// Token: 0x04000992 RID: 2450
	private SaveSlot currentSlot;

	// Token: 0x04000993 RID: 2451
	private bool inputEnabled;

	// Token: 0x04000994 RID: 2452
	private bool backButtonLostFocus;

	// Token: 0x04000995 RID: 2453
	private SaveScreenManager.ScreenState _currentScreenState;

	// Token: 0x04000996 RID: 2454
	[SerializeField]
	private SaveScreenManager.StateTabKVP[] _stateTabLookup;

	// Token: 0x04000997 RID: 2455
	[SerializeField]
	private GameObject _loadTitle;

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	private ControllerGlyphComponent deleteGlyph;

	// Token: 0x02000331 RID: 817
	[Serializable]
	public enum ScreenState
	{
		// Token: 0x0400129F RID: 4767
		LOAD,
		// Token: 0x040012A0 RID: 4768
		SAVE,
		// Token: 0x040012A1 RID: 4769
		NONE
	}

	// Token: 0x02000332 RID: 818
	[Serializable]
	public struct StateTabKVP
	{
		// Token: 0x040012A2 RID: 4770
		public SaveScreenManager.ScreenState ScreenState;

		// Token: 0x040012A3 RID: 4771
		public GameObject TabObj;
	}
}
