using System;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001FD RID: 509
	public class DateADexEntryJoystickActivationProvider : MonoBehaviour, IJoystickActivationProvider
	{
		// Token: 0x060010C0 RID: 4288 RVA: 0x000563E3 File Offset: 0x000545E3
		public bool ShouldScrollerBeActive()
		{
			return !(DateADex.Instance == null) && DateADex.Instance.IsInEntryScreen;
		}
	}
}
