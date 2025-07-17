using System;
using UnityEngine;
using UnityEngine.UI;

namespace T17.UI
{
	// Token: 0x02000240 RID: 576
	public class SelectOnEnabled : MonoBehaviour
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x00059D60 File Offset: 0x00057F60
		private void OnEnable()
		{
			Selectable component = base.GetComponent<Selectable>();
			if (component != null && component.interactable)
			{
				ControllerMenuUI.SetCurrentlySelected(base.gameObject, ControllerMenuUI.Direction.Down, false, false);
			}
		}
	}
}
