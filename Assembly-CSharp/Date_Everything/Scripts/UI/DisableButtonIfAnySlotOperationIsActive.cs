using System;
using UnityEngine;
using UnityEngine.UI;

namespace Date_Everything.Scripts.UI
{
	// Token: 0x0200026A RID: 618
	[RequireComponent(typeof(Button))]
	public class DisableButtonIfAnySlotOperationIsActive : MonoBehaviour
	{
		// Token: 0x0600140C RID: 5132 RVA: 0x00060887 File Offset: 0x0005EA87
		private void Awake()
		{
			this._button = base.GetComponent<Button>();
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00060895 File Offset: 0x0005EA95
		private void Update()
		{
			this.UpdateButtonInteractableState(!SaveSlot.IsDoingOperation());
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x000608A5 File Offset: 0x0005EAA5
		private void UpdateButtonInteractableState(bool interactable)
		{
			if (this._button.interactable == interactable)
			{
				return;
			}
			this._button.interactable = interactable;
		}

		// Token: 0x04000F74 RID: 3956
		private Button _button;
	}
}
