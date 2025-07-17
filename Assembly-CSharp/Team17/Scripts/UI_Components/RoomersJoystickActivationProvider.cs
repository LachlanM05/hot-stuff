using System;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x0200020C RID: 524
	public class RoomersJoystickActivationProvider : MonoBehaviour, IJoystickActivationProvider
	{
		// Token: 0x06001122 RID: 4386 RVA: 0x0005752E File Offset: 0x0005572E
		public bool ShouldScrollerBeActive()
		{
			return this.IsRoomersInfoShowing();
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00057536 File Offset: 0x00055736
		private bool IsRoomersInfoShowing()
		{
			return !(Roomers.Instance == null) && Roomers.Instance.roomersScreenInfo.gameObject.activeInHierarchy;
		}
	}
}
