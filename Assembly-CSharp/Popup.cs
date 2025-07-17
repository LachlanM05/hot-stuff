using System;
using System.Collections.Generic;
using T17.Services;
using Team17.Scripts.Services.Input;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000123 RID: 291
public class Popup : Singleton<Popup>
{
	// Token: 0x06000A11 RID: 2577 RVA: 0x0003975C File Offset: 0x0003795C
	public void CreatePopup(string title, string text, bool canAwakenTbc = true)
	{
		if (this.PopUp.activeInHierarchy)
		{
			return;
		}
		this.ClearOptionsButtons();
		this.canAwakenTbc = false;
		if (canAwakenTbc && Singleton<Save>.Instance.GetDateStatusRealized("tbc_ui") != RelationshipStatus.Realized)
		{
			base.Invoke("SetCanAwakenTbcOn", 1f);
		}
		this.isMultipleChoices = false;
		if (title == "")
		{
			this.title.transform.gameObject.SetActive(false);
		}
		else
		{
			this.title.transform.gameObject.SetActive(true);
		}
		this.title.text = title;
		this.text.text = text;
		this.PopUp.SetActive(true);
		this.ScreenDarken.SetActive(true);
		ChatButton chatButton = this.InstantiateButton("OK", false, null);
		Navigation navigation = chatButton.button.navigation;
		navigation.mode = Navigation.Mode.None;
		chatButton.button.navigation = navigation;
		this.SwitchPopUpGraphic();
		this.UpdateLegendVisibility();
		this.OnShow();
		this.SelectButton(chatButton);
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00039862 File Offset: 0x00037A62
	private void SetCanAwakenTbcOn()
	{
		this.canAwakenTbc = true;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0003986C File Offset: 0x00037A6C
	public void SelectButton(ChatButton b)
	{
		if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<ChatButton>())
		{
			EventSystem.current.currentSelectedGameObject.GetComponent<ChatButton>().SetSelected(false);
		}
		b.SetSelected(true);
		ControllerMenuUI.SetCurrentlySelected(b.gameObject, ControllerMenuUI.Direction.Down, false, false);
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x000398CC File Offset: 0x00037ACC
	public void CreatePopup(string title, string text, UnityEvent Triggr, bool canAwakenTbc = true)
	{
		if (this.PopUp.activeInHierarchy)
		{
			return;
		}
		this.ClearOptionsButtons();
		this.canAwakenTbc = canAwakenTbc;
		this.isMultipleChoices = false;
		if (title == "")
		{
			this.title.transform.gameObject.SetActive(false);
		}
		else
		{
			this.title.transform.gameObject.SetActive(true);
		}
		this.title.text = title;
		this.text.text = text;
		this.PopUp.SetActive(true);
		this.ScreenDarken.SetActive(true);
		ChatButton chatButton = this.InstantiateButton("OK", false, new UnityAction(Triggr.Invoke));
		Navigation navigation = chatButton.button.navigation;
		navigation.mode = Navigation.Mode.None;
		chatButton.button.navigation = navigation;
		this.SwitchPopUpGraphic();
		this.UpdateLegendVisibility();
		this.OnShow();
		this.SelectButton(chatButton);
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x000399BC File Offset: 0x00037BBC
	public void CreatePopup(string title, string text, UnityEvent yes, UnityEvent no, bool canAwakenTbc = true)
	{
		if (this.PopUp.activeInHierarchy)
		{
			return;
		}
		this.ClearOptionsButtons();
		this.canAwakenTbc = canAwakenTbc;
		this.isMultipleChoices = true;
		if (title == "")
		{
			this.title.transform.gameObject.SetActive(false);
		}
		else
		{
			this.title.transform.gameObject.SetActive(true);
		}
		this.title.text = title;
		this.text.text = text;
		this.PopUp.SetActive(true);
		this.ScreenDarken.SetActive(true);
		ChatButton chatButton = this.InstantiateButton("Yes", false, new UnityAction(yes.Invoke));
		ChatButton chatButton2 = this.InstantiateButton("No", true, new UnityAction(no.Invoke));
		Navigation navigation = chatButton.button.navigation;
		navigation.selectOnUp = chatButton2.button;
		navigation.selectOnLeft = chatButton2.button;
		navigation.selectOnDown = chatButton2.button;
		navigation.selectOnRight = chatButton2.button;
		navigation.mode = Navigation.Mode.Explicit;
		chatButton.button.navigation = navigation;
		navigation = chatButton2.button.navigation;
		navigation.selectOnUp = chatButton.button;
		navigation.selectOnLeft = chatButton.button;
		navigation.selectOnDown = chatButton.button;
		navigation.selectOnRight = chatButton.button;
		navigation.mode = Navigation.Mode.Explicit;
		chatButton2.button.navigation = navigation;
		this.SwitchPopUpGraphic();
		this.UpdateLegendVisibility();
		this.OnShow();
		this.SelectButton(chatButton);
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00039B4B File Offset: 0x00037D4B
	public bool IsPopupOpen()
	{
		return this.PopUp.activeInHierarchy;
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00039B58 File Offset: 0x00037D58
	public void StartChatWithDateable()
	{
		if (this.canAwakenTbc && Singleton<Save>.Instance.GetFullTutorialFinished())
		{
			if (Singleton<GameController>.Instance.CanSelectObj(this.PopUp.GetComponent<InteractableObj>(), true, false, false, false))
			{
				if (Singleton<PhoneManager>.Instance.IsPhoneMenuOpened())
				{
					Singleton<PhoneManager>.Instance.ClosePhoneAsync(null, true);
				}
				this.ClosePopup();
				base.Invoke("SelectObj", 1f);
				return;
			}
			this.SelectObj();
		}
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00039BC9 File Offset: 0x00037DC9
	private void SelectObj()
	{
		Singleton<GameController>.Instance.SelectObj(this.PopUp.GetComponent<InteractableObj>(), true, null, false, false, false);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00039BE8 File Offset: 0x00037DE8
	private void UpdateLegendVisibility()
	{
		if (this.LegendConfirmText != null)
		{
			this.LegendConfirmText.gameObject.SetActive(this._buttons.Count > 0);
		}
		if (this.LegendNavigateText != null)
		{
			this.LegendNavigateText.gameObject.SetActive(this._buttons.Count > 1);
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00039C4D File Offset: 0x00037E4D
	private void OnShow()
	{
		this._inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.UI, this);
		this.onShow.Invoke();
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00039C6C File Offset: 0x00037E6C
	private void ClearOptionsButtons()
	{
		this._buttons.Clear();
		foreach (object obj in this.ButtonInstanceSpot.transform)
		{
			Object.Destroy(((Transform)obj).gameObject);
		}
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00039CD8 File Offset: 0x00037ED8
	public bool IsPopupButton(GameObject buttonObj)
	{
		for (int i = this._buttons.Count - 1; i >= 0; i--)
		{
			if (buttonObj == this._buttons[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00039D10 File Offset: 0x00037F10
	public void ClosePopup()
	{
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = null;
		this.ClearOptionsButtons();
		this.PopUp.SetActive(false);
		this.ScreenDarken.SetActive(false);
		this.onHide.Invoke();
		Singleton<ControllerMenuUI>.Instance.HighlightAButton(false);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x00039D6C File Offset: 0x00037F6C
	public void SwitchPopUpGraphic()
	{
		if (Singleton<Dateviators>.Instance == null || !Singleton<Dateviators>.Instance.Equipped)
		{
			this.PopUpContainer.GetComponent<Image>().sprite = this.popupWindowTextures[0];
			this.title.color = new Color(0.04313f, 0.0588f, 0.1843f, 1f);
			this.text.color = new Color(0.04313f, 0.0588f, 0.1843f, 1f);
			return;
		}
		this.PopUpContainer.GetComponent<Image>().sprite = this.popupWindowTextures[1];
		this.title.color = new Color(0.9098f, 0f, 0.3529f, 1f);
		this.text.color = new Color(1f, 0.95686f, 0.9529f, 1f);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00039E54 File Offset: 0x00038054
	private ChatButton InstantiateButton(string buttonText, bool isNegativeAction, UnityAction onClick)
	{
		ChatButton component = Object.Instantiate<GameObject>(this.Button, this.ButtonInstanceSpot.transform).GetComponent<ChatButton>();
		component.CharacterName.text = buttonText;
		component.button.onClick.AddListener(new UnityAction(this.ClosePopup));
		if (onClick != null)
		{
			component.button.onClick.AddListener(onClick);
		}
		this.EnsureButtonHasUIPriorityComp(component);
		this._buttons.Add(component.button.gameObject);
		return component;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00039ED7 File Offset: 0x000380D7
	private void EnsureButtonHasUIPriorityComp(ChatButton button)
	{
		if (button.gameObject.GetComponent<UIFocusPriority>() == null)
		{
			button.gameObject.AddComponent<UIFocusPriority>().UIPriority = 10;
		}
	}

	// Token: 0x04000935 RID: 2357
	private const int kDefaultUIPriority = 10;

	// Token: 0x04000936 RID: 2358
	public GameObject PopUp;

	// Token: 0x04000937 RID: 2359
	public GameObject PopUpContainer;

	// Token: 0x04000938 RID: 2360
	public GameObject ScreenDarken;

	// Token: 0x04000939 RID: 2361
	public GameObject Button;

	// Token: 0x0400093A RID: 2362
	public GameObject ButtonInstanceSpot;

	// Token: 0x0400093B RID: 2363
	public TextMeshProUGUI title;

	// Token: 0x0400093C RID: 2364
	public TextMeshProUGUI text;

	// Token: 0x0400093D RID: 2365
	public TextMeshProUGUI LegendNavigateText;

	// Token: 0x0400093E RID: 2366
	public TextMeshProUGUI LegendConfirmText;

	// Token: 0x0400093F RID: 2367
	public UnityEvent onShow;

	// Token: 0x04000940 RID: 2368
	public UnityEvent onHide;

	// Token: 0x04000941 RID: 2369
	public Sprite[] popupWindowTextures;

	// Token: 0x04000942 RID: 2370
	public bool isMultipleChoices;

	// Token: 0x04000943 RID: 2371
	public bool canAwakenTbc;

	// Token: 0x04000944 RID: 2372
	private InputModeHandle _inputModeHandle;

	// Token: 0x04000945 RID: 2373
	private List<GameObject> _buttons = new List<GameObject>(3);
}
