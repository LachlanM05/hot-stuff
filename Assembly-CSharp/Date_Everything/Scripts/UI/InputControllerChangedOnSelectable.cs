using System;
using Rewired;
using T17.Services;
using UnityEngine;
using UnityEngine.Events;

namespace Date_Everything.Scripts.UI
{
	// Token: 0x0200026B RID: 619
	public class InputControllerChangedOnSelectable : MonoBehaviour
	{
		// Token: 0x06001410 RID: 5136 RVA: 0x000608CA File Offset: 0x0005EACA
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ControllerChanged));
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000608EA File Offset: 0x0005EAEA
		private void OnDisable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ControllerChanged));
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0006090C File Offset: 0x0005EB0C
		private void ControllerChanged(Controller controller)
		{
			if (ControllerMenuUI.GetCurrentSelectedControl() == base.gameObject)
			{
				UnityEvent onChanged = this._onChanged;
				if (onChanged != null)
				{
					onChanged.Invoke();
				}
				if (Services.InputService.IsLastActiveInputController())
				{
					UnityEvent onChangedToController = this._onChangedToController;
					if (onChangedToController == null)
					{
						return;
					}
					onChangedToController.Invoke();
					return;
				}
				else
				{
					UnityEvent onChangedToKeyboard = this._onChangedToKeyboard;
					if (onChangedToKeyboard == null)
					{
						return;
					}
					onChangedToKeyboard.Invoke();
				}
			}
		}

		// Token: 0x04000F75 RID: 3957
		[SerializeField]
		private UnityEvent _onChangedToController;

		// Token: 0x04000F76 RID: 3958
		[SerializeField]
		private UnityEvent _onChangedToKeyboard;

		// Token: 0x04000F77 RID: 3959
		[SerializeField]
		private UnityEvent _onChanged;
	}
}
