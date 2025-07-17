using System;
using Team17.Common;

namespace Team17.Input
{
	// Token: 0x020001E2 RID: 482
	public interface IControllerDeviceRumbleHandler : IService
	{
		// Token: 0x0600103C RID: 4156
		void SetDeviceRumble(float left, float right);

		// Token: 0x0600103D RID: 4157
		void StopDeviceRumble();
	}
}
