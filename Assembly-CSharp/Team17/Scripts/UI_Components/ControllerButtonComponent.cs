using System;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001FA RID: 506
	public class ControllerButtonComponent : MonoBehaviour
	{
		// Token: 0x060010AB RID: 4267 RVA: 0x0005611D File Offset: 0x0005431D
		private void Awake()
		{
			if (!string.IsNullOrEmpty(this._rewiredInputAction))
			{
				this.valid = true;
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00056134 File Offset: 0x00054334
		private void Update()
		{
			if (this.valid && (this.player != null || (this.player = this.GetPrimaryPlayer()) != null) && this.player.GetButtonDown(this._rewiredInputAction))
			{
				this.PressButton();
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0005617B File Offset: 0x0005437B
		private bool CanPressButton(Button button)
		{
			return button != null && button.gameObject.activeInHierarchy && button.interactable;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0005619C File Offset: 0x0005439C
		private void PressButton()
		{
			Button component = base.GetComponent<Button>();
			if (this.CanPressButton(component))
			{
				Button.ButtonClickedEvent onClick = component.onClick;
				if (onClick == null)
				{
					return;
				}
				onClick.Invoke();
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x000561C9 File Offset: 0x000543C9
		private Player GetPrimaryPlayer()
		{
			return ReInput.players.GetPlayer(0);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x000561D6 File Offset: 0x000543D6
		public void SetRewiredInputAction(string newAction)
		{
			this._rewiredInputAction = newAction;
		}

		// Token: 0x04000E06 RID: 3590
		[SerializeField]
		private string _rewiredInputAction = string.Empty;

		// Token: 0x04000E07 RID: 3591
		private bool valid;

		// Token: 0x04000E08 RID: 3592
		private Player player;
	}
}
