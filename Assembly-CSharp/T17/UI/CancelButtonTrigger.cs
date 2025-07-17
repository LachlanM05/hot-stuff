using System;
using Team17.Scripts.UI_Components;
using UnityEngine;
using UnityEngine.UI;

namespace T17.UI
{
	// Token: 0x0200023F RID: 575
	[DisallowMultipleComponent]
	public class CancelButtonTrigger : MonoBehaviour
	{
		// Token: 0x06001284 RID: 4740 RVA: 0x00059C33 File Offset: 0x00057E33
		private void OnEnable()
		{
			if (!this.registered)
			{
				if (this.button == null)
				{
					this.button = base.GetComponent<Button>();
				}
				ControllerMenuUI instance = Singleton<ControllerMenuUI>.Instance;
				if (instance != null)
				{
					instance.RegisterCancelButton(true, this);
				}
				this.registered = true;
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00059C70 File Offset: 0x00057E70
		private void OnDisable()
		{
			if (this.registered)
			{
				ControllerMenuUI instance = Singleton<ControllerMenuUI>.Instance;
				if (instance != null)
				{
					instance.RegisterCancelButton(false, this);
				}
				this.registered = false;
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00059C94 File Offset: 0x00057E94
		public void TriggerButton()
		{
			if (this.ignoreIfDialogShown)
			{
				if (UIDialogManager.Instance != null && UIDialogManager.Instance.AreAnyDialogsActive())
				{
					return;
				}
				if (Singleton<Popup>.Instance != null && Singleton<Popup>.Instance.IsPopupOpen())
				{
					return;
				}
			}
			if (this.ignoreIfQuickResponseActive && Singleton<QuickResponseService>.Instance != null && Singleton<QuickResponseService>.Instance.IsQuickResponseEnabled())
			{
				return;
			}
			if ((this.button != null || (this.button = base.GetComponent<Button>()) != null) && this.button.interactable && this.button.onClick != null)
			{
				this.button.onClick.Invoke();
			}
		}

		// Token: 0x04000E9C RID: 3740
		private Button button;

		// Token: 0x04000E9D RID: 3741
		private bool registered;

		// Token: 0x04000E9E RID: 3742
		public bool ignoreIfDialogShown = true;

		// Token: 0x04000E9F RID: 3743
		public bool ignoreIfQuickResponseActive;
	}
}
