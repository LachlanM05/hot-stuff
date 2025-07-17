using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace T17.UI
{
	// Token: 0x02000242 RID: 578
	public class UIDialogButton
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x0005A1D8 File Offset: 0x000583D8
		// (set) Token: 0x0600129A RID: 4762 RVA: 0x0005A1CF File Offset: 0x000583CF
		public ChatButton Button { get; private set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0005A1E9 File Offset: 0x000583E9
		// (set) Token: 0x0600129C RID: 4764 RVA: 0x0005A1E0 File Offset: 0x000583E0
		public uint DialogHandle { get; private set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x0005A1FA File Offset: 0x000583FA
		// (set) Token: 0x0600129E RID: 4766 RVA: 0x0005A1F1 File Offset: 0x000583F1
		public Action OnButtonSelected { get; private set; }

		// Token: 0x060012A0 RID: 4768 RVA: 0x0005A202 File Offset: 0x00058402
		public void Initialise(GameObject prefab, Transform parent, UnityAction selectionCallback)
		{
			this.Button = Object.Instantiate<GameObject>(prefab, parent).GetComponent<ChatButton>();
			this.Button.gameObject.SetActive(false);
			this.Button.button.onClick.AddListener(selectionCallback);
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0005A240 File Offset: 0x00058440
		public void Setup(string buttonText, Action onButtonSelected, uint dialogHandle, Transform buttonLocator)
		{
			this.DialogHandle = dialogHandle;
			this.OnButtonSelected = onButtonSelected;
			if (this.Button.CharacterName != null)
			{
				this.Button.CharacterName.text = buttonText;
			}
			this.Button.transform.position.Set(0f, 0f, 0f);
			this.Button.transform.SetParent(buttonLocator);
			this.Button.gameObject.SetActive(true);
			this._autoSelectTime = float.MaxValue;
			this._buttonText = buttonText;
			this.SetupButtonSoundEvent(false);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0005A2E4 File Offset: 0x000584E4
		public void SetupButtonSoundEvent(bool isNegativeAction)
		{
			UIButtonSoundEvent component = this.Button.gameObject.GetComponent<UIButtonSoundEvent>();
			if (component != null)
			{
				component.SetAsDialogOption(isNegativeAction);
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0005A312 File Offset: 0x00058512
		public void SetAutoSelect(int autoSelectTime)
		{
			this._autoSelectTime = (float)autoSelectTime;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0005A31C File Offset: 0x0005851C
		public void Reset(Transform parent)
		{
			this.Button.transform.SetParent(parent);
			this.Button.transform.position.Set(0f, 0f, 0f);
			this.Button.gameObject.SetActive(false);
			this.DialogHandle = 0U;
			this.OnButtonSelected = null;
			this._autoSelectTime = float.MaxValue;
			this._buttonText = string.Empty;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0005A398 File Offset: 0x00058598
		public void Update(float deltaTime)
		{
			if (this._autoSelectTime != 3.4028235E+38f)
			{
				this._autoSelectTime -= deltaTime;
				if (this.Button.CharacterName != null)
				{
					this.Button.CharacterName.text = string.Format("{0}  ({1})", this._buttonText, (int)this._autoSelectTime + 1);
				}
				if (this._autoSelectTime <= 0f)
				{
					Button.ButtonClickedEvent onClick = this.Button.button.onClick;
					if (onClick != null)
					{
						onClick.Invoke();
					}
					this._autoSelectTime = float.MaxValue;
				}
			}
		}

		// Token: 0x04000EB1 RID: 3761
		public float _autoSelectTime = float.MaxValue;

		// Token: 0x04000EB2 RID: 3762
		private string _buttonText;
	}
}
