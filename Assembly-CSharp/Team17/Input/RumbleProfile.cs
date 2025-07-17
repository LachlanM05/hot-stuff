using System;
using UnityEngine;

namespace Team17.Input
{
	// Token: 0x020001E0 RID: 480
	[CreateAssetMenu(menuName = "Settings/Controller Rumble/Rumble Profile")]
	public class RumbleProfile : RumbleProfileData
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0005562C File Offset: 0x0005382C
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00055634 File Offset: 0x00053834
		public RumbleProfileData GetRuntimeProfileData()
		{
			return this;
		}

		// Token: 0x04000DDF RID: 3551
		[Header("Enabled")]
		[SerializeField]
		private bool _enabled = true;

		// Token: 0x04000DE0 RID: 3552
		[Header("Overrides")]
		[SerializeField]
		private RumbleProfileData _overridePS5;

		// Token: 0x04000DE1 RID: 3553
		[SerializeField]
		private RumbleProfileData _overrideSwitch;
	}
}
