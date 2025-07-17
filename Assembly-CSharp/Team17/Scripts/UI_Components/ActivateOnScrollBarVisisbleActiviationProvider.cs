using System;
using UnityEngine;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001F4 RID: 500
	public class ActivateOnScrollBarVisisbleActiviationProvider : MonoBehaviour, IJoystickActivationProvider
	{
		// Token: 0x06001097 RID: 4247 RVA: 0x00055F48 File Offset: 0x00054148
		public bool ShouldScrollerBeActive()
		{
			return this._scrollbar.IsInteractable();
		}

		// Token: 0x04000DFA RID: 3578
		[SerializeField]
		private Scrollbar _scrollbar;
	}
}
