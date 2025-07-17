using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using Rewired;
using T17.Services;
using T17.UI;
using Team17.Scripts.UI_Components;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000EB RID: 235
public class ControllerMenuUI : Singleton<ControllerMenuUI>
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002B31F File Offset: 0x0002951F
	// (set) Token: 0x060007BD RID: 1981 RVA: 0x0002B328 File Offset: 0x00029528
	public GameObject currentlySelected
	{
		get
		{
			return this._currentlySelected;
		}
		private set
		{
			if (this._currentlySelected != value)
			{
				this._currentlySelected = value;
				if (this._currentlySelected != null)
				{
					this.AddToSelectedStack(this._currentlySelected);
					this.CurrentlySelectedIsAQuickResponseButton = this._currentlySelected.GetComponent<QuickResponseButton>() != null;
					this.CurrentlySelectedIsACycleBlockerComponent = this._currentlySelected.GetComponent<CycleBlockerComponent>() != null;
				}
			}
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060007BE RID: 1982 RVA: 0x0002B392 File Offset: 0x00029592
	// (set) Token: 0x060007BF RID: 1983 RVA: 0x0002B39A File Offset: 0x0002959A
	public bool CurrentlySelectedIsAQuickResponseButton { get; private set; }

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0002B3A3 File Offset: 0x000295A3
	// (set) Token: 0x060007C1 RID: 1985 RVA: 0x0002B3AB File Offset: 0x000295AB
	public bool CurrentlySelectedIsACycleBlockerComponent { get; private set; }

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060007C2 RID: 1986 RVA: 0x0002B3B4 File Offset: 0x000295B4
	public int PriorityOfCurrentSelected
	{
		get
		{
			if (this._currentlySelected != null)
			{
				UIFocusPriority component = this._currentlySelected.GetComponent<UIFocusPriority>();
				if (component != null)
				{
					return component.UIPriority;
				}
			}
			return -1;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0002B3EC File Offset: 0x000295EC
	public static bool WasMovedManually
	{
		get
		{
			return ControllerMenuUI.c_ManualMove;
		}
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0002B3F4 File Offset: 0x000295F4
	public override void AwakeSingleton()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		this.upRepeat = base.gameObject.AddComponent<AutoRepeat>();
		this.upRepeat.inputDirection = ControllerMenuUI.Direction.Up;
		this.downRepeat = base.gameObject.AddComponent<AutoRepeat>();
		this.downRepeat.inputDirection = ControllerMenuUI.Direction.Down;
		this.leftRepeat = base.gameObject.AddComponent<AutoRepeat>();
		this.leftRepeat.inputDirection = ControllerMenuUI.Direction.Left;
		this.rightRepeat = base.gameObject.AddComponent<AutoRepeat>();
		this.rightRepeat.inputDirection = ControllerMenuUI.Direction.Right;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x0002B480 File Offset: 0x00029680
	public void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.currentEventSystem = EventSystem.current;
		if (this.currentEventSystem != null)
		{
			this.currentlySelected = this.currentEventSystem.currentSelectedGameObject;
		}
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x0002B4BD File Offset: 0x000296BD
	public void OnEnable()
	{
		this.currentEventSystem = EventSystem.current;
		if (this.currentEventSystem != null)
		{
			this.currentlySelected = this.currentEventSystem.currentSelectedGameObject;
		}
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x0002B4E9 File Offset: 0x000296E9
	public static GameObject GetCurrentSelectedControl()
	{
		if (Singleton<ControllerMenuUI>.Instance != null)
		{
			return Singleton<ControllerMenuUI>.Instance.currentlySelected;
		}
		return null;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0002B504 File Offset: 0x00029704
	public static void SetCurrentlySelected(GameObject current, ControllerMenuUI.Direction currentDirection = ControllerMenuUI.Direction.Down, bool manualSelected = false, bool isViaPointer = false)
	{
		if (!ControllerMenuUI.IsAllowedToNavigateTo(current, isViaPointer))
		{
			return;
		}
		if (Singleton<ControllerMenuUI>.Instance != null)
		{
			ControllerMenuUI.forwardNavigationCount = 0;
			Singleton<ControllerMenuUI>.Instance.SetCurrentlySelected_Impl(current, currentDirection, manualSelected, isViaPointer);
			return;
		}
		if (current != null && current.activeInHierarchy)
		{
			Selectable component = current.GetComponent<Selectable>();
			if (component != null && component.interactable)
			{
				ControllerMenuUI.c_ManualMove = manualSelected;
				component.Select();
				if (EventSystem.current != null)
				{
					EventSystem.current.SetSelectedGameObject(current);
				}
			}
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0002B58C File Offset: 0x0002978C
	private void SetCurrentlySelected_Impl(GameObject objectToSelect, ControllerMenuUI.Direction currentDirection = ControllerMenuUI.Direction.Down, bool manualSelected = false, bool isViaPointer = false)
	{
		this.ClearPendingSelection();
		if (objectToSelect == null)
		{
			return;
		}
		Selectable component = objectToSelect.GetComponent<Selectable>();
		if (component == null)
		{
			return;
		}
		if (this.currentEventSystem != null && this.currentEventSystem.alreadySelecting)
		{
			this.pendingSelection = objectToSelect;
			this.pendingDirection = currentDirection;
			return;
		}
		if (!objectToSelect.activeInHierarchy || !component.IsInteractable() || !ControllerMenuUI.IsAllowedToNavigateTo(objectToSelect, isViaPointer))
		{
			if (ControllerMenuUI.forwardNavigationCount++ < 7 && objectToSelect.GetComponent<ForwardNavigation>() != null)
			{
				Selectable selectable = null;
				switch (currentDirection)
				{
				case ControllerMenuUI.Direction.Up:
					selectable = component.navigation.selectOnUp;
					break;
				case ControllerMenuUI.Direction.Down:
					selectable = component.navigation.selectOnDown;
					break;
				case ControllerMenuUI.Direction.Right:
					selectable = component.navigation.selectOnRight;
					break;
				case ControllerMenuUI.Direction.Left:
					selectable = component.navigation.selectOnLeft;
					break;
				}
				if (selectable != null)
				{
					this.SetCurrentlySelected_Impl(selectable.gameObject, currentDirection, manualSelected, false);
				}
			}
			return;
		}
		bool flag = true;
		bool flag2 = Singleton<Popup>.Instance != null && Singleton<Popup>.Instance.IsPopupOpen();
		bool flag3 = UIDialogManager.Instance != null && UIDialogManager.Instance.AreAnyDialogsActive();
		int num = 0;
		int num2 = 0;
		if (this.currentlySelected != null && this.currentlySelected.activeInHierarchy)
		{
			UIFocusPriority component2 = this.currentlySelected.GetComponent<UIFocusPriority>();
			if (component2 != null)
			{
				num = component2.UIPriority;
			}
			UIFocusPriority component3 = objectToSelect.GetComponent<UIFocusPriority>();
			if (component3 != null)
			{
				num2 = component3.UIPriority;
			}
			if (num2 < num)
			{
				flag = false;
			}
		}
		if (flag2 || flag3)
		{
			flag = false;
			if (flag2 && Singleton<Popup>.Instance.IsPopupButton(objectToSelect))
			{
				flag = true;
			}
			else if (flag3 && UIDialogManager.Instance.IsDialogButton(objectToSelect))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.InsertIntoObjectStack(objectToSelect, flag2, flag3);
			return;
		}
		if (this.currentlySelected != null && this.currentlySelected.GetComponent<ChatButton>() != null)
		{
			this.currentlySelected.GetComponent<ChatButton>().SetSelected(false);
		}
		this.currentlySelected = objectToSelect;
		ControllerMenuUI.c_ManualMove = manualSelected;
		component.Select();
		if (this.currentEventSystem != null)
		{
			this.currentEventSystem.SetSelectedGameObject(objectToSelect);
			this.controlRecentlySelected = true;
		}
		if (this.currentlySelected.GetComponent<ChatButton>() != null)
		{
			this.currentlySelected.GetComponent<ChatButton>().SetSelected(true);
		}
		if (this.currentlySelected.GetComponent<ScrollOption>() != null && this.buttonPressed)
		{
			this.currentlySelected.GetComponent<ScrollOption>().ScrollPosition();
		}
		if (this.currentlySelected.GetComponent<DexEntryButton>() != null)
		{
			DateADex.Instance.ScrollToEntryButton(this.currentlySelected.GetComponent<DexEntryButton>());
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0002B858 File Offset: 0x00029A58
	private void TrackSelected()
	{
		if (this.currentEventSystem != null)
		{
			GameObject currentSelectedGameObject = this.currentEventSystem.currentSelectedGameObject;
			if (currentSelectedGameObject != null && this.currentlySelected != currentSelectedGameObject)
			{
				ControllerMenuUI.SetCurrentlySelected(currentSelectedGameObject, ControllerMenuUI.Direction.Down, false, false);
			}
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0002B89C File Offset: 0x00029A9C
	public bool HighlightAButton(bool force = false)
	{
		if (force)
		{
			this.controlRecentlySelected = true;
		}
		GameObject selectedObjectFromStack = this.GetSelectedObjectFromStack();
		if (selectedObjectFromStack != null)
		{
			ControllerMenuUI.SetCurrentlySelected(selectedObjectFromStack, ControllerMenuUI.Direction.Down, false, false);
			return true;
		}
		if (this.controlRecentlySelected)
		{
			this.controlRecentlySelected = false;
			Button[] array = Object.FindObjectsOfType<Button>();
			for (int i = array.Length - 1; i >= 0; i--)
			{
				Button button = array[i];
				if (button.gameObject.activeInHierarchy && button.IsInteractable())
				{
					ControllerMenuUI.SetCurrentlySelected(button.gameObject, ControllerMenuUI.Direction.Down, false, false);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0002B920 File Offset: 0x00029B20
	public bool IsUIInteractionEnabled()
	{
		if (!Services.UIInputService.AllowUIInteraction())
		{
			return false;
		}
		if (this.currentlySelected == null || this.currentlySelected.GetComponent<Selectable>() == null || !this.currentlySelected.activeInHierarchy)
		{
			return false;
		}
		TMP_InputField component = this.currentlySelected.GetComponent<TMP_InputField>();
		return (!(component != null) || !component.isFocused || Services.InputService.IsLastActiveInputController()) && !(Singleton<PlayerPauser>.Instance == null) && (Singleton<PlayerPauser>.Instance.PauseList.Count > 0 || string.CompareOrdinal(SceneManager.GetActiveScene().name, SceneConsts.kMainMenu) == 0 || Singleton<PhoneManager>.Instance.IsPhoneAppOpened() || UIDialogManager.Instance.HasActiveDialogs || Singleton<PhoneManager>.Instance.SubMenuOpen || DateADex.Instance.startedEnding);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0002BA00 File Offset: 0x00029C00
	private void ProcessUIInput()
	{
		if (!this.IsUIInteractionEnabled())
		{
			return;
		}
		Selectable component = this.currentlySelected.GetComponent<Selectable>();
		Navigation navigation = component.navigation;
		bool flag = Services.UIInputService.IsUISubmitUp();
		bool flag2 = Services.UIInputService.IsUISubmitDown();
		bool flag3 = Services.UIInputService.IsUICancelDown();
		bool flag4 = false;
		bool flag5 = Services.UIInputService.IsUILeftDown() || this.leftRepeat.HasDirectionRepeated();
		bool flag6 = Services.UIInputService.IsUIRightDown() || this.rightRepeat.HasDirectionRepeated();
		bool flag7 = Services.UIInputService.IsUIUpDown() || this.upRepeat.HasDirectionRepeated();
		bool flag8 = Services.UIInputService.IsUIDownDown() || this.downRepeat.HasDirectionRepeated();
		bool flag9 = Services.UIInputService.IsUICycleUpDown() && !this.CurrentlySelectedIsACycleBlockerComponent;
		bool flag10 = Services.UIInputService.IsUICycleDownDown() && !this.CurrentlySelectedIsACycleBlockerComponent;
		Services.UIInputService.GetScrollInput();
		if (flag2)
		{
			this.selectedControl = component;
		}
		else if (flag)
		{
			if (this.selectedControl != null && component == this.selectedControl)
			{
				flag4 = true;
			}
			this.selectedControl = null;
		}
		if (navigation.mode == Navigation.Mode.Automatic)
		{
			Selectable selectable = null;
			DynamicNavigationOverride component2 = component.GetComponent<DynamicNavigationOverride>();
			ControllerMenuUI.Direction direction = ControllerMenuUI.Direction.Up;
			if (flag7)
			{
				selectable = (component2 ? component2.FindSelectableOnUp() : component.FindSelectableOnUp());
				direction = ControllerMenuUI.Direction.Up;
			}
			else if (flag8)
			{
				selectable = (component2 ? component2.FindSelectableOnDown() : component.FindSelectableOnDown());
				direction = ControllerMenuUI.Direction.Down;
			}
			else if (flag5)
			{
				selectable = (component2 ? component2.FindSelectableOnLeft() : component.FindSelectableOnLeft());
				direction = ControllerMenuUI.Direction.Left;
			}
			else if (flag6)
			{
				selectable = (component2 ? component2.FindSelectableOnRight() : component.FindSelectableOnRight());
				direction = ControllerMenuUI.Direction.Down;
			}
			else if (flag9)
			{
				Selectable selectable2 = component;
				direction = ControllerMenuUI.Direction.Up;
				int num = 0;
				while (num < 7 && selectable2.FindSelectableOnUp() != null && (selectable2.FindSelectableOnUp().GetComponentInParent<ScrollRect>() != null || selectable2.FindSelectableOnUp().GetComponentInParent<CircularScrollingList>() != null))
				{
					selectable2 = selectable2.FindSelectableOnUp();
					num++;
				}
				selectable = selectable2;
			}
			else if (flag10)
			{
				Selectable selectable3 = component;
				direction = ControllerMenuUI.Direction.Down;
				int num2 = 0;
				while (num2 < 7 && selectable3.FindSelectableOnDown() != null && (selectable3.FindSelectableOnDown().GetComponentInParent<ScrollRect>() != null || selectable3.FindSelectableOnDown().GetComponentInParent<CircularScrollingList>() != null))
				{
					selectable3 = selectable3.FindSelectableOnDown();
					num2++;
				}
				selectable = selectable3;
			}
			if (selectable != null)
			{
				this.buttonPressed = true;
				ControllerMenuUI.SetCurrentlySelected(selectable.gameObject, direction, true, false);
				return;
			}
		}
		else if (navigation.mode == Navigation.Mode.Explicit)
		{
			if (flag7 && navigation.selectOnUp != null)
			{
				this.buttonPressed = true;
				ControllerMenuUI.SetCurrentlySelected(navigation.selectOnUp.gameObject, ControllerMenuUI.Direction.Up, true, false);
				return;
			}
			if (flag8 && navigation.selectOnDown != null)
			{
				this.buttonPressed = true;
				ControllerMenuUI.SetCurrentlySelected(navigation.selectOnDown.gameObject, ControllerMenuUI.Direction.Down, true, false);
				return;
			}
			if (flag6 && navigation.selectOnRight != null)
			{
				this.buttonPressed = true;
				ControllerMenuUI.SetCurrentlySelected(navigation.selectOnRight.gameObject, ControllerMenuUI.Direction.Right, true, false);
				return;
			}
			if (flag5 && navigation.selectOnLeft != null)
			{
				this.buttonPressed = true;
				ControllerMenuUI.SetCurrentlySelected(navigation.selectOnLeft.gameObject, ControllerMenuUI.Direction.Left, true, false);
				return;
			}
			if (flag9 && navigation.selectOnUp != null && component.GetComponentInParent<ScrollRect>() != null)
			{
				this.buttonPressed = true;
				Selectable selectable4 = component;
				int num3 = 0;
				while (num3 < 7 && selectable4.navigation.selectOnUp != null && (selectable4.navigation.selectOnUp.GetComponentInParent<ScrollRect>() != null || selectable4.navigation.selectOnUp.GetComponentInParent<CircularScrollingList>() != null))
				{
					selectable4 = selectable4.navigation.selectOnUp;
					num3++;
				}
				ControllerMenuUI.SetCurrentlySelected(selectable4.gameObject, ControllerMenuUI.Direction.Up, true, false);
				return;
			}
			if (flag10 && navigation.selectOnDown != null && component.GetComponentInParent<ScrollRect>() != null)
			{
				this.buttonPressed = true;
				Selectable selectable5 = component;
				int num4 = 0;
				while (num4 < 7 && selectable5.navigation.selectOnDown != null && selectable5.navigation.selectOnDown.GetComponentInParent<ScrollRect>() != null)
				{
					selectable5 = selectable5.navigation.selectOnDown;
					num4++;
				}
				ControllerMenuUI.SetCurrentlySelected(selectable5.gameObject, ControllerMenuUI.Direction.Down, true, false);
				return;
			}
		}
		else if ((navigation.mode & Navigation.Mode.Horizontal) == Navigation.Mode.Horizontal)
		{
			if (flag7 || flag8)
			{
				Scrollbar component3 = this.currentlySelected.GetComponent<Scrollbar>();
				if (component3 != null)
				{
					if (flag7)
					{
						component3.value = Mathf.Clamp01(component3.value - 0.1f);
						return;
					}
					if (flag8)
					{
						component3.value = Mathf.Clamp01(component3.value + 0.1f);
						return;
					}
				}
				else
				{
					Slider component4 = this.currentlySelected.GetComponent<Slider>();
					if (component4 != null)
					{
						float num5 = (component4.maxValue - component4.minValue) / 10f;
						if (component4.direction == Slider.Direction.RightToLeft || component4.direction == Slider.Direction.BottomToTop)
						{
							num5 *= -1f;
						}
						if (flag7)
						{
							component4.value = Mathf.Clamp(component4.value + num5, component4.minValue, component4.maxValue);
							return;
						}
						if (flag8)
						{
							component4.value = Mathf.Clamp(component4.value - num5, component4.minValue, component4.maxValue);
							return;
						}
					}
				}
			}
			if ((navigation.mode & Navigation.Mode.Explicit) == Navigation.Mode.Explicit)
			{
				if (flag6 && navigation.selectOnRight != null)
				{
					this.buttonPressed = true;
					ControllerMenuUI.SetCurrentlySelected(navigation.selectOnRight.gameObject, ControllerMenuUI.Direction.Right, true, false);
					return;
				}
				if (flag5 && navigation.selectOnLeft != null)
				{
					this.buttonPressed = true;
					ControllerMenuUI.SetCurrentlySelected(navigation.selectOnLeft.gameObject, ControllerMenuUI.Direction.Left, true, false);
					return;
				}
			}
			else
			{
				Selectable selectable6 = null;
				ControllerMenuUI.Direction direction2 = ControllerMenuUI.Direction.Up;
				if (flag5)
				{
					selectable6 = component.FindSelectableOnLeft();
					direction2 = ControllerMenuUI.Direction.Left;
				}
				else if (flag6)
				{
					selectable6 = component.FindSelectableOnRight();
					direction2 = ControllerMenuUI.Direction.Right;
				}
				if (selectable6 != null)
				{
					this.buttonPressed = true;
					ControllerMenuUI.SetCurrentlySelected(selectable6.gameObject, direction2, true, false);
					return;
				}
			}
		}
		else if ((navigation.mode & Navigation.Mode.Vertical) == Navigation.Mode.Vertical)
		{
			if (flag5 || flag6)
			{
				Scrollbar component5 = this.currentlySelected.GetComponent<Scrollbar>();
				if (component5 != null)
				{
					if (flag5)
					{
						component5.value = Mathf.Clamp01(component5.value - 0.1f);
						return;
					}
					if (flag6)
					{
						component5.value = Mathf.Clamp01(component5.value + 0.1f);
						return;
					}
				}
				else
				{
					Slider component6 = this.currentlySelected.GetComponent<Slider>();
					if (component6 != null)
					{
						float num6 = (component6.maxValue - component6.minValue) / 10f;
						if (component6.direction == Slider.Direction.RightToLeft || component6.direction == Slider.Direction.BottomToTop)
						{
							num6 *= -1f;
						}
						if (flag6)
						{
							component6.value = Mathf.Clamp(component6.value + num6, component6.minValue, component6.maxValue);
							return;
						}
						if (flag5)
						{
							component6.value = Mathf.Clamp(component6.value - num6, component6.minValue, component6.maxValue);
							return;
						}
					}
				}
			}
			if ((navigation.mode & Navigation.Mode.Explicit) == Navigation.Mode.Explicit)
			{
				if (flag7 && navigation.selectOnUp != null)
				{
					this.buttonPressed = true;
					ControllerMenuUI.SetCurrentlySelected(navigation.selectOnUp.gameObject, ControllerMenuUI.Direction.Up, true, false);
					return;
				}
				if (flag8 && navigation.selectOnDown != null)
				{
					this.buttonPressed = true;
					ControllerMenuUI.SetCurrentlySelected(navigation.selectOnDown.gameObject, ControllerMenuUI.Direction.Down, true, false);
					return;
				}
			}
			else
			{
				Selectable selectable7 = null;
				ControllerMenuUI.Direction direction3 = ControllerMenuUI.Direction.Up;
				if (flag7)
				{
					selectable7 = component.FindSelectableOnUp();
					direction3 = ControllerMenuUI.Direction.Up;
				}
				else if (flag8)
				{
					selectable7 = component.FindSelectableOnDown();
					direction3 = ControllerMenuUI.Direction.Down;
				}
				if (selectable7 != null)
				{
					this.buttonPressed = true;
					ControllerMenuUI.SetCurrentlySelected(selectable7.gameObject, direction3, true, false);
					return;
				}
			}
		}
		if (flag4)
		{
			Button component7 = this.currentlySelected.GetComponent<Button>();
			if (component7 != null && component7.isActiveAndEnabled && component7.interactable && component7.onClick != null && this.IsAllowedToRespondToConfirm(component7))
			{
				component7.onClick.Invoke();
				return;
			}
		}
		if (flag5 && this.currentlySelected.GetComponent<Slider>() != null)
		{
			this.currentlySelected.GetComponent<Slider>().value = this.currentlySelected.GetComponent<Slider>().value - 0.001f;
			return;
		}
		if (flag6 && this.currentlySelected.GetComponent<Slider>() != null)
		{
			this.currentlySelected.GetComponent<Slider>().value = this.currentlySelected.GetComponent<Slider>().value + 0.001f;
			return;
		}
		if (flag5 && this.currentlySelected.GetComponent<TMP_Dropdown>() != null)
		{
			this.currentlySelected.GetComponent<TMP_Dropdown>().value = Mathf.Clamp(this.currentlySelected.GetComponent<TMP_Dropdown>().value - 1, 0, this.currentlySelected.GetComponent<TMP_Dropdown>().options.Count);
			return;
		}
		if (flag6 && this.currentlySelected.GetComponent<TMP_Dropdown>() != null)
		{
			this.currentlySelected.GetComponent<TMP_Dropdown>().value = Mathf.Clamp(this.currentlySelected.GetComponent<TMP_Dropdown>().value + 1, 0, this.currentlySelected.GetComponent<TMP_Dropdown>().options.Count);
			return;
		}
		if (flag4 && this.currentlySelected.GetComponent<Toggle>() != null)
		{
			this.currentlySelected.GetComponent<Toggle>().isOn = !this.currentlySelected.GetComponent<Toggle>().isOn;
			return;
		}
		if (flag7 && this.currentlySelected.GetComponent<DexEntryButton>() != null)
		{
			DateADex.Instance.ScrollToEntryButton(this.currentlySelected.GetComponent<DexEntryButton>());
			return;
		}
		if (flag8 && this.currentlySelected.GetComponent<DexEntryButton>() != null)
		{
			DateADex.Instance.ScrollToEntryButton(this.currentlySelected.GetComponent<DexEntryButton>());
			return;
		}
		if (flag9 && this.currentlySelected.GetComponent<DexEntryButton>() != null)
		{
			DateADex.Instance._scrollingList.scrollSfxNumber = 0;
			DateADex.Instance.PageEntries(true);
			return;
		}
		if (flag10 && this.currentlySelected.GetComponent<DexEntryButton>() != null)
		{
			DateADex.Instance._scrollingList.scrollSfxNumber = 0;
			DateADex.Instance.PageEntries(false);
			return;
		}
		if (flag9 && this.currentlySelected.GetComponent<MusicEntryButton>() != null)
		{
			MusicPlayer.Instance.PageEntries(true);
			return;
		}
		if (flag10 && this.currentlySelected.GetComponent<MusicEntryButton>() != null)
		{
			MusicPlayer.Instance.PageEntries(false);
			return;
		}
		if (flag9 && this.currentlySelected.GetComponent<ArtEntryButton>() != null)
		{
			Singleton<ArtPlayer>.Instance.PageEntries(true);
			return;
		}
		if (flag10 && this.currentlySelected.GetComponent<ArtEntryButton>() != null)
		{
			Singleton<ArtPlayer>.Instance.PageEntries(false);
			return;
		}
		if (!flag3)
		{
			if (this.cancelPressed)
			{
				this.TriggerCancelButton();
				this.cancelPressed = false;
				return;
			}
		}
		else
		{
			this.cancelPressed = true;
		}
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0002C585 File Offset: 0x0002A785
	private static bool IsAllowedToNavigateTo(MonoBehaviour monoBehaviour)
	{
		return ControllerMenuUI.IsAllowedToNavigateTo(monoBehaviour.gameObject, false);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0002C594 File Offset: 0x0002A794
	private static bool IsAllowedToNavigateTo(GameObject gameObject, bool viaPointer)
	{
		if (gameObject == null)
		{
			return true;
		}
		if (gameObject.GetComponent<ControllerDisplayComponent>() != null && gameObject.GetComponent<ControllerDisplayComponent>().IsSelectionBlocked())
		{
			return false;
		}
		bool flag = Singleton<QuickResponseService>.Instance.IsQuickResponseEnabled() && gameObject.GetComponent<QuickResponseButton>();
		return (!Services.InputService.IsLastActiveInputController() && (viaPointer || !flag)) || (!(gameObject.GetComponent<DisallowControllerNavigationComponent>() != null) && !flag);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0002C610 File Offset: 0x0002A810
	private bool IsAllowedToRespondToConfirm(Button btn)
	{
		if (btn != null)
		{
			if (Services.InputService.IsLastActiveInputController() && btn.GetComponent<DisallowConfirmPressOnController>())
			{
				return false;
			}
			if (!Singleton<QuickResponseService>.Instance.IsQuickResponseEnabled() || !btn.gameObject.GetComponent<QuickResponseButton>())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002C668 File Offset: 0x0002A868
	private void Update()
	{
		if (this.currentEventSystem == null || !this.currentEventSystem.enabled)
		{
			this.currentEventSystem = EventSystem.current;
		}
		if (this.pendingSelection != null)
		{
			ControllerMenuUI.SetCurrentlySelected(this.pendingSelection, this.pendingDirection, false, false);
			return;
		}
		this.TrackSelected();
		this.buttonPressed = false;
		this.ProcessUIInput();
		if (!(this.currentEventSystem != null) || !(this.currentEventSystem.currentSelectedGameObject != null) || !(this.currentlySelected != this.currentEventSystem.currentSelectedGameObject))
		{
			if (!Services.InputService.WasLastControllerAPointer() && this.currentEventSystem != null && (this.currentEventSystem.currentSelectedGameObject == null || !this.currentEventSystem.currentSelectedGameObject.activeInHierarchy))
			{
				if (this.currentlySelected != null && this.currentlySelected.activeInHierarchy)
				{
					ControllerMenuUI.SetCurrentlySelected(this.currentlySelected, ControllerMenuUI.Direction.Down, false, false);
					return;
				}
				this.HighlightAButton(false);
			}
			return;
		}
		if (this.currentEventSystem.currentSelectedGameObject.activeInHierarchy)
		{
			this.currentlySelected = this.currentEventSystem.currentSelectedGameObject;
			return;
		}
		this.HighlightAButton(false);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002C7A7 File Offset: 0x0002A9A7
	public void RegisterCancelButton(bool add, CancelButtonTrigger cancelButton)
	{
		this.cancelButtons.Remove(cancelButton);
		if (add)
		{
			this.cancelButtons.Add(cancelButton);
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002C7C8 File Offset: 0x0002A9C8
	private void TriggerCancelButton()
	{
		for (int i = this.cancelButtons.Count - 1; i >= 0; i--)
		{
			if (!(this.cancelButtons[i] == null))
			{
				this.cancelButtons[i].TriggerButton();
				return;
			}
			this.cancelButtons.RemoveAt(i);
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002C821 File Offset: 0x0002AA21
	private void AddToSelectedStack(GameObject theGameObject)
	{
		this.selectedObjectStack.Remove(theGameObject);
		this.selectedObjectStack.Add(theGameObject);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002C83C File Offset: 0x0002AA3C
	private void InsertIntoObjectStack(GameObject theGameObject, bool popup, bool dialog)
	{
		int num = 0;
		UIFocusPriority component = theGameObject.GetComponent<UIFocusPriority>();
		if (component != null)
		{
			num = component.UIPriority;
		}
		this.selectedObjectStack.Remove(theGameObject);
		int i = this.selectedObjectStack.Count - 1;
		while (i >= 0)
		{
			if (this.selectedObjectStack[i] != null)
			{
				UIFocusPriority component2 = this.selectedObjectStack[i].GetComponent<UIFocusPriority>();
				if (component2 != null && component2.UIPriority > num)
				{
					i--;
					continue;
				}
			}
			if (i == this.selectedObjectStack.Count - 1)
			{
				this.selectedObjectStack.Add(theGameObject);
				return;
			}
			this.selectedObjectStack.Insert(i + 1, theGameObject);
			return;
		}
		this.selectedObjectStack.Insert(0, theGameObject);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0002C8FC File Offset: 0x0002AAFC
	private GameObject GetSelectedObjectFromStack()
	{
		for (int i = this.selectedObjectStack.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = this.selectedObjectStack[i];
			this.selectedObjectStack.RemoveAt(i);
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				Selectable component = gameObject.GetComponent<Selectable>();
				if (component != null && component.enabled && component.interactable)
				{
					return gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0002C970 File Offset: 0x0002AB70
	public string OutputAllAbailableButtons()
	{
		List<string> list = new List<string>(4);
		string text = "";
		int num = 0;
		Button[] array = Object.FindObjectsOfType<Button>();
		for (int i = array.Length - 1; i >= 0; i--)
		{
			Button button = array[i];
			if (button.gameObject.activeInHierarchy && button.IsInteractable())
			{
				Transform transform = array[i].transform;
				list.Clear();
				int num2 = 4;
				do
				{
					list.Add(transform.gameObject.name);
					num2--;
					transform = transform.parent;
				}
				while (transform != null && num2 > 0);
				string text2 = string.Format("{0}: ", num++);
				for (int j = list.Count - 1; j >= 0; j--)
				{
					text2 = text2 + " -> " + list[j];
				}
				text = text + text2 + "\n";
			}
		}
		return text;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002CA65 File Offset: 0x0002AC65
	private void ClearPendingSelection()
	{
		this.pendingSelection = null;
	}

	// Token: 0x040006F1 RID: 1777
	private EventSystem currentEventSystem;

	// Token: 0x040006F2 RID: 1778
	private GameObject _currentlySelected;

	// Token: 0x040006F5 RID: 1781
	private Player player;

	// Token: 0x040006F6 RID: 1782
	private List<GameObject> selectedObjectStack = new List<GameObject>(10);

	// Token: 0x040006F7 RID: 1783
	private bool controlRecentlySelected = true;

	// Token: 0x040006F8 RID: 1784
	private bool buttonPressed;

	// Token: 0x040006F9 RID: 1785
	private List<CancelButtonTrigger> cancelButtons = new List<CancelButtonTrigger>();

	// Token: 0x040006FA RID: 1786
	private bool cancelPressed;

	// Token: 0x040006FB RID: 1787
	private Selectable selectedControl;

	// Token: 0x040006FC RID: 1788
	private static int forwardNavigationCount;

	// Token: 0x040006FD RID: 1789
	private const int k_MaxForwardNavigation = 7;

	// Token: 0x040006FE RID: 1790
	private static bool c_ManualMove;

	// Token: 0x040006FF RID: 1791
	private AutoRepeat upRepeat;

	// Token: 0x04000700 RID: 1792
	private AutoRepeat downRepeat;

	// Token: 0x04000701 RID: 1793
	private AutoRepeat leftRepeat;

	// Token: 0x04000702 RID: 1794
	private AutoRepeat rightRepeat;

	// Token: 0x04000703 RID: 1795
	private GameObject pendingSelection;

	// Token: 0x04000704 RID: 1796
	private ControllerMenuUI.Direction pendingDirection = ControllerMenuUI.Direction.Down;

	// Token: 0x02000303 RID: 771
	public enum Direction
	{
		// Token: 0x040011FF RID: 4607
		Up,
		// Token: 0x04001200 RID: 4608
		Down,
		// Token: 0x04001201 RID: 4609
		Right,
		// Token: 0x04001202 RID: 4610
		Left
	}
}
