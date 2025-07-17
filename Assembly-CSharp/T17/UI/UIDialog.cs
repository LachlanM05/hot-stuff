using System;
using T17.Services;
using Team17.Scripts.Services.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace T17.UI
{
	// Token: 0x02000241 RID: 577
	public class UIDialog
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00059DA4 File Offset: 0x00057FA4
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x00059D9B File Offset: 0x00057F9B
		public uint Handle { get; private set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x00059DAC File Offset: 0x00057FAC
		public UIDialogButton[] Buttons
		{
			get
			{
				return this._buttons;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x00059DBD File Offset: 0x00057FBD
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x00059DB4 File Offset: 0x00057FB4
		public Transform ButtonInstanceSpot { get; private set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00059DCE File Offset: 0x00057FCE
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x00059DC5 File Offset: 0x00057FC5
		public GameObject PreviouslySelectedGameObject { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00059DD6 File Offset: 0x00057FD6
		public bool AutoCloseOnOptionSelected
		{
			get
			{
				return this._autoCloseOnOptionSelected;
			}
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00059DE0 File Offset: 0x00057FE0
		public void Initialise(GameObject dialogPrefab, Transform parent)
		{
			this._theDialog = Object.Instantiate<GameObject>(dialogPrefab, parent);
			this._theDialog.SetActive(false);
			this._canvas = this._theDialog.gameObject.GetComponent<Canvas>();
			TextMeshProUGUI[] componentsInChildren = this._theDialog.GetComponentsInChildren<TextMeshProUGUI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.name.Contains("Title", StringComparison.OrdinalIgnoreCase))
				{
					this._title = componentsInChildren[i];
				}
				else if (componentsInChildren[i].gameObject.name.Contains("Text", StringComparison.OrdinalIgnoreCase))
				{
					this._bodyText = componentsInChildren[i];
				}
			}
			Transform transform = this._theDialog.transform.Find("PopupContainer");
			if (transform)
			{
				Transform transform2 = transform.Find("PopupContents");
				if (transform2)
				{
					this.ButtonInstanceSpot = transform2.Find("Options");
				}
			}
			this.Handle = 0U;
			for (int j = 0; j < 3; j++)
			{
				this._buttons[j] = null;
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00059EE4 File Offset: 0x000580E4
		public void Setup(uint handle, string title, string body, UIDialogButton button1Data = null, UIDialogButton button2Data = null, UIDialogButton button3Data = null, GameObject previouslySelectedGameObject = null, bool autoCloseOnOptionSelected = true)
		{
			this.Handle = handle;
			this._buttons[0] = button1Data;
			this._buttons[1] = button2Data;
			this._buttons[2] = button3Data;
			this.PreviouslySelectedGameObject = previouslySelectedGameObject;
			this._autoCloseOnOptionSelected = autoCloseOnOptionSelected;
			if (string.IsNullOrEmpty(title))
			{
				this._title.gameObject.SetActive(false);
			}
			else
			{
				this._title.gameObject.SetActive(true);
				this._title.text = title;
			}
			this._bodyText.text = body;
			ChatButton chatButton = null;
			Navigation defaultNavigation = Navigation.defaultNavigation;
			defaultNavigation.mode = Navigation.Mode.Explicit;
			for (int i = 0; i < this._buttons.Length; i++)
			{
				if (this._buttons[i] != null)
				{
					if (chatButton != null)
					{
						defaultNavigation.selectOnRight = this._buttons[i].Button.button;
						chatButton.button.navigation = defaultNavigation;
						defaultNavigation.selectOnLeft = chatButton.button;
					}
					chatButton = this._buttons[i].Button;
				}
			}
			if (chatButton != null)
			{
				defaultNavigation.selectOnRight = null;
				chatButton.button.navigation = defaultNavigation;
			}
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0005A000 File Offset: 0x00058200
		public void Show(int sortLayer = 0, float positionOffset = 0f)
		{
			this._inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.UI, this);
			CursorLocker.Unlock();
			for (int i = 0; i < this._buttons.Length; i++)
			{
				if (this._buttons[i] != null && this._buttons[i].DialogHandle != 0U)
				{
					this._buttons[i].Button.gameObject.SetActive(true);
				}
			}
			this._theDialog.SetActive(true);
			Vector3 localPosition = this._theDialog.transform.localPosition;
			localPosition.x = positionOffset;
			localPosition.y = -positionOffset;
			this._theDialog.transform.localPosition = localPosition;
			if (sortLayer != 0)
			{
				this._canvas.overrideSorting = true;
				this._canvas.sortingOrder = sortLayer;
				return;
			}
			this._canvas.overrideSorting = false;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0005A0D0 File Offset: 0x000582D0
		public void Update(float deltaTime)
		{
			int i = 0;
			int num = this._buttons.Length;
			while (i < num)
			{
				UIDialogButton uidialogButton = this._buttons[i];
				if (uidialogButton != null)
				{
					uidialogButton.Update(deltaTime);
				}
				i++;
			}
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0005A108 File Offset: 0x00058308
		public void Hide()
		{
			InputModeHandle inputModeHandle = this._inputModeHandle;
			if (inputModeHandle != null)
			{
				inputModeHandle.Dispose();
			}
			this._inputModeHandle = null;
			CursorLocker.Lock();
			for (int i = 0; i < 3; i++)
			{
				this._buttons[i] = null;
			}
			this._theDialog.SetActive(false);
			this.PreviouslySelectedGameObject = null;
			this.Handle = 0U;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0005A161 File Offset: 0x00058361
		public void SetBodyText(string text)
		{
			this._bodyText.text = text;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0005A170 File Offset: 0x00058370
		public bool IsDialogButton(GameObject buttonObj)
		{
			for (int i = this._buttons.Length - 1; i >= 0; i--)
			{
				if (this._buttons[i] != null && buttonObj == this._buttons[i].Button.gameObject)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000EA0 RID: 3744
		public const int kMaxNumButtons = 3;

		// Token: 0x04000EA1 RID: 3745
		private const string kTitleObjectName = "Title";

		// Token: 0x04000EA2 RID: 3746
		private const string kBodyTextObjectName = "Text";

		// Token: 0x04000EA3 RID: 3747
		private const string kButtonLocatorObjectName = "Options";

		// Token: 0x04000EA7 RID: 3751
		private GameObject _theDialog;

		// Token: 0x04000EA8 RID: 3752
		private TextMeshProUGUI _title;

		// Token: 0x04000EA9 RID: 3753
		private TextMeshProUGUI _bodyText;

		// Token: 0x04000EAA RID: 3754
		private bool _autoCloseOnOptionSelected = true;

		// Token: 0x04000EAB RID: 3755
		private Canvas _canvas;

		// Token: 0x04000EAC RID: 3756
		private UIDialogButton[] _buttons = new UIDialogButton[3];

		// Token: 0x04000EAD RID: 3757
		private InputModeHandle _inputModeHandle;
	}
}
