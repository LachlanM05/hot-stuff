using System;
using Team17.Common;

namespace Team17.Input.Unity
{
	// Token: 0x020001E4 RID: 484
	public interface IControllerRumbleService : IService
	{
		// Token: 0x0600104A RID: 4170
		void RequestRumble(RumbleProfileData rumbleProfileData, bool looping);

		// Token: 0x0600104B RID: 4171
		void StopRumble(RumbleProfileData rumbleProfileData);

		// Token: 0x0600104C RID: 4172
		void StopAll();

		// Token: 0x0600104D RID: 4173
		void OnApplicationFocus(bool focus);
	}
}
