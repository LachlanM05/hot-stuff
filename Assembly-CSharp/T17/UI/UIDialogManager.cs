using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace T17.UI
{
	// Token: 0x02000243 RID: 579
	public class UIDialogManager : MonoBehaviour
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0005A452 File Offset: 0x00058652
		// (set) Token: 0x060012A7 RID: 4775 RVA: 0x0005A44A File Offset: 0x0005864A
		public static UIDialogManager Instance { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060012A9 RID: 4777 RVA: 0x0005A459 File Offset: 0x00058659
		public bool HasActiveDialogs
		{
			get
			{
				return this._activeDialogs.Count > 0;
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0005A46C File Offset: 0x0005866C
		public uint ShowOKDialog(string title, string body, Action onOKSelected, bool autoCloseOnOptionSelected = true)
		{
			return this.ShowDialog(title, body, this.GetLocalisedText("OK"), onOKSelected, "", null, "", null, 0, autoCloseOnOptionSelected);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0005A49C File Offset: 0x0005869C
		public uint ShowOkCancelDialog(string title, string body, Action onOKSelected, Action onCancelSelected)
		{
			return this.ShowDialog(title, body, this.GetLocalisedText("OK"), onOKSelected, this.GetLocalisedText("Cancel"), onCancelSelected, "", null, 0, true);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0005A4D4 File Offset: 0x000586D4
		public uint ShowYesNoDialog(string title, string body, Action onYesSelected, Action onNoSelected)
		{
			return this.ShowDialog(title, body, this.GetLocalisedText("Yes"), onYesSelected, this.GetLocalisedText("No"), onNoSelected, "", null, 0, true);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0005A50C File Offset: 0x0005870C
		public uint ShowAutoYesNoDialog(string title, string body, Action onYesSelected, Action onNoSelected, int autoSelecttime)
		{
			return this.ShowDialog(title, body, this.GetLocalisedText("Yes"), onYesSelected, this.GetLocalisedText("No"), onNoSelected, "", null, autoSelecttime, true);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0005A544 File Offset: 0x00058744
		public uint ShowDialog(string title, string body, string button1Text = null, Action onButton1Selected = null, string button2Text = "", Action onButton2Selected = null, string button3Text = "", Action onButton3Selected = null, int autoSelectIime = 0, bool autoCloseOnOptionSelected = true)
		{
			uint num = this.GenetateDialogHandle();
			string.IsNullOrEmpty(title);
			UIDialogButton uidialogButton = null;
			UIDialogButton uidialogButton2 = null;
			UIDialogButton uidialogButton3 = null;
			UIDialogButton uidialogButton4 = null;
			UIDialogButton uidialogButton5 = null;
			UIDialog dialogFromPool = this.GetDialogFromPool();
			if (!string.IsNullOrEmpty(button1Text))
			{
				uidialogButton = this.GetButtonFromPool();
				uidialogButton.Setup(button1Text, onButton1Selected, num, dialogFromPool.ButtonInstanceSpot);
				uidialogButton4 = (uidialogButton5 = uidialogButton);
			}
			if (!string.IsNullOrEmpty(button2Text))
			{
				uidialogButton2 = this.GetButtonFromPool();
				uidialogButton2.Setup(button2Text, onButton2Selected, num, dialogFromPool.ButtonInstanceSpot);
				uidialogButton4 = uidialogButton2;
				uidialogButton5 = ((uidialogButton5 == null) ? uidialogButton2 : uidialogButton5);
			}
			if (!string.IsNullOrEmpty(button3Text))
			{
				uidialogButton3 = this.GetButtonFromPool();
				uidialogButton3.Setup(button3Text, onButton3Selected, num, dialogFromPool.ButtonInstanceSpot);
				uidialogButton4 = uidialogButton3;
				uidialogButton5 = ((uidialogButton5 == null) ? uidialogButton3 : uidialogButton5);
			}
			if (uidialogButton4 != uidialogButton5)
			{
				uidialogButton4.SetupButtonSoundEvent(true);
			}
			GameObject gameObject = null;
			if (EventSystem.current != null)
			{
				gameObject = EventSystem.current.currentSelectedGameObject;
			}
			dialogFromPool.Setup(num, title, body, uidialogButton, uidialogButton2, uidialogButton3, gameObject, autoCloseOnOptionSelected);
			if (!string.IsNullOrEmpty(this.OpenSFX) && Singleton<AudioManager>.Instance != null)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.OpenSFX, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			}
			this.InputBlocker.enabled = true;
			int num2 = this._defaultCanvasSortOrder + this._activeDialogs.Count;
			float num3 = (float)(this._activeDialogs.Count - 1) % this.kMaxNumOffsetDialogs * this.kLayeredPositionoffset;
			dialogFromPool.Show(num2, num3);
			if (autoSelectIime == 0)
			{
				this.SelectButton(uidialogButton5);
			}
			else
			{
				if (uidialogButton4 != null)
				{
					uidialogButton4.SetAutoSelect(autoSelectIime);
				}
				this.SelectButton(uidialogButton4);
			}
			return num;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0005A6E0 File Offset: 0x000588E0
		public void CloseDialog(uint handle)
		{
			UIDialog activeDialog = this.GetActiveDialog(handle);
			if (activeDialog != null)
			{
				GameObject previouslySelectedGameObject = activeDialog.PreviouslySelectedGameObject;
				UIDialog dialogAbove = this.GetDialogAbove(handle);
				this.ReturnButtonsToPool(activeDialog.Buttons);
				this.ReturnDialogToPool(activeDialog);
				activeDialog.Hide();
				if (dialogAbove == null)
				{
					ControllerMenuUI.SetCurrentlySelected(previouslySelectedGameObject, ControllerMenuUI.Direction.Down, false, false);
				}
				else
				{
					dialogAbove.PreviouslySelectedGameObject = previouslySelectedGameObject;
				}
				if (!string.IsNullOrEmpty(this.CloseSFX) && Singleton<AudioManager>.Instance != null)
				{
					Singleton<AudioManager>.Instance.PlayTrack(this.CloseSFX, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
				}
				if (this._activeDialogs.Count == 0)
				{
					this.InputBlocker.enabled = false;
				}
			}
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0005A790 File Offset: 0x00058990
		private void onDialogCallback(UIDialogButton buttonSelected)
		{
			Action onButtonSelected = buttonSelected.OnButtonSelected;
			if (this.GetActiveDialog(buttonSelected.DialogHandle).AutoCloseOnOptionSelected)
			{
				this.CloseDialog(buttonSelected.DialogHandle);
			}
			if (onButtonSelected == null)
			{
				return;
			}
			onButtonSelected();
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0005A7C4 File Offset: 0x000589C4
		public bool SetDialogText(uint handle, string bodyText)
		{
			UIDialog activeDialog = this.GetActiveDialog(handle);
			if (activeDialog != null)
			{
				activeDialog.SetBodyText(bodyText);
			}
			return activeDialog != null;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0005A7E8 File Offset: 0x000589E8
		public void Awake()
		{
			if (UIDialogManager.Instance != null)
			{
				Object.Destroy(this);
				return;
			}
			UIDialogManager.Instance = this;
			if (!this._isInitialised)
			{
				for (int i = 0; i < 2; i++)
				{
					this._dialogPool.Push(this.AllocNewDialog());
				}
				for (int j = 0; j < 6; j++)
				{
					this._dialogButtonPool.Push(this.AllocNewButton());
				}
				Canvas componentInParent = base.transform.GetComponentInParent<Canvas>();
				if (componentInParent.isRootCanvas)
				{
					this._defaultCanvasSortOrder = componentInParent.sortingOrder;
				}
				else
				{
					this._defaultCanvasSortOrder = componentInParent.rootCanvas.sortingOrder;
				}
				this._isInitialised = true;
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0005A88C File Offset: 0x00058A8C
		public void Update()
		{
			int i = 0;
			int count = this._activeDialogs.Count;
			while (i < count)
			{
				this._activeDialogs[i].Update(Time.deltaTime);
				i++;
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0005A8C8 File Offset: 0x00058AC8
		private uint GenetateDialogHandle()
		{
			uint currentDialogHandleId = this._currentDialogHandleId;
			this._currentDialogHandleId = currentDialogHandleId + 1U;
			return currentDialogHandleId;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0005A8E8 File Offset: 0x00058AE8
		private UIDialog GetDialogFromPool()
		{
			UIDialog uidialog;
			if (this._dialogPool.Count > 0)
			{
				uidialog = this._dialogPool.Pop();
			}
			else
			{
				uidialog = this.AllocNewDialog();
			}
			this._activeDialogs.Add(uidialog);
			return uidialog;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0005A927 File Offset: 0x00058B27
		private UIDialog AllocNewDialog()
		{
			UIDialog uidialog = new UIDialog();
			uidialog.Initialise(this.DialogPrefab, base.transform);
			return uidialog;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0005A940 File Offset: 0x00058B40
		private void ReturnDialogToPool(UIDialog dialog)
		{
			this._dialogPool.Push(dialog);
			this._activeDialogs.Remove(dialog);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0005A95C File Offset: 0x00058B5C
		private UIDialogButton GetButtonFromPool()
		{
			UIDialogButton uidialogButton;
			if (this._dialogButtonPool.Count > 0)
			{
				uidialogButton = this._dialogButtonPool.Pop();
			}
			else
			{
				uidialogButton = this.AllocNewButton();
			}
			return uidialogButton;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0005A990 File Offset: 0x00058B90
		private void ReturnButtonsToPool(UIDialogButton[] buttons)
		{
			foreach (UIDialogButton uidialogButton in buttons)
			{
				if (uidialogButton != null)
				{
					uidialogButton.Reset(base.transform);
					this._dialogButtonPool.Push(uidialogButton);
				}
			}
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0005A9CC File Offset: 0x00058BCC
		private UIDialogButton AllocNewButton()
		{
			UIDialogButton dialogButton = new UIDialogButton();
			dialogButton.Initialise(this.ButtonPrefab, base.transform, delegate
			{
				this.onDialogCallback(dialogButton);
			});
			return dialogButton;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0005AA1C File Offset: 0x00058C1C
		private UIDialog GetActiveDialog(uint handle)
		{
			for (int i = 0; i < this._activeDialogs.Count; i++)
			{
				UIDialog uidialog = this._activeDialogs[i];
				if (uidialog.Handle == handle)
				{
					return uidialog;
				}
			}
			return null;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0005AA58 File Offset: 0x00058C58
		private UIDialog GetDialogAbove(uint handle)
		{
			int i = 0;
			int count = this._activeDialogs.Count;
			while (i < count)
			{
				if (this._activeDialogs[i].Handle > handle)
				{
					return this._activeDialogs[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0005AAA0 File Offset: 0x00058CA0
		public void SelectButton(UIDialogButton uiButton)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				ChatButton component = EventSystem.current.currentSelectedGameObject.GetComponent<ChatButton>();
				if (component != null)
				{
					component.SetSelected(false);
				}
			}
			if (uiButton == null)
			{
				EventSystem.current.SetSelectedGameObject(null);
				return;
			}
			uiButton.Button.SetSelected(true);
			ControllerMenuUI.SetCurrentlySelected(uiButton.Button.gameObject, ControllerMenuUI.Direction.Down, false, false);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0005AB1B File Offset: 0x00058D1B
		private string GetLocalisedText(string key)
		{
			return key;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0005AB1E File Offset: 0x00058D1E
		public bool AreAnyDialogsActive()
		{
			return this._activeDialogs.Count > 0;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0005AB2E File Offset: 0x00058D2E
		public bool IsDialogButton(GameObject buttonObj)
		{
			return this.AreAnyDialogsActive() && this._activeDialogs[this._activeDialogs.Count - 1].IsDialogButton(buttonObj);
		}

		// Token: 0x04000EB4 RID: 3764
		public const uint kInvalidHandle = 0U;

		// Token: 0x04000EB5 RID: 3765
		private const int kNumInitialDialogs = 2;

		// Token: 0x04000EB6 RID: 3766
		private const int kNumInitialButtons = 6;

		// Token: 0x04000EB7 RID: 3767
		private const string kOKText = "OK";

		// Token: 0x04000EB8 RID: 3768
		private const string kCancelText = "Cancel";

		// Token: 0x04000EB9 RID: 3769
		private const string kYesText = "Yes";

		// Token: 0x04000EBA RID: 3770
		private const string kNoText = "No";

		// Token: 0x04000EBB RID: 3771
		[Header("Prefabs")]
		public GameObject DialogPrefab;

		// Token: 0x04000EBC RID: 3772
		public GameObject ButtonPrefab;

		// Token: 0x04000EBD RID: 3773
		public Image InputBlocker;

		// Token: 0x04000EBE RID: 3774
		[Header("Audio")]
		public string OpenSFX = "";

		// Token: 0x04000EBF RID: 3775
		public string CloseSFX = "";

		// Token: 0x04000EC0 RID: 3776
		[Header("Layering")]
		[Tooltip("How much to offset the dialog by when an existing dialog is visible")]
		public float kLayeredPositionoffset = 0.5f;

		// Token: 0x04000EC1 RID: 3777
		public float kMaxNumOffsetDialogs = 5f;

		// Token: 0x04000EC2 RID: 3778
		private List<UIDialog> _activeDialogs = new List<UIDialog>();

		// Token: 0x04000EC3 RID: 3779
		private Stack<UIDialog> _dialogPool = new Stack<UIDialog>();

		// Token: 0x04000EC4 RID: 3780
		private Stack<UIDialogButton> _dialogButtonPool = new Stack<UIDialogButton>();

		// Token: 0x04000EC5 RID: 3781
		private uint _currentDialogHandleId = 1U;

		// Token: 0x04000EC6 RID: 3782
		private int _defaultCanvasSortOrder;

		// Token: 0x04000EC7 RID: 3783
		private bool _isInitialised;
	}
}
