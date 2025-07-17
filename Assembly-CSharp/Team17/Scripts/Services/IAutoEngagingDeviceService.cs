using System;
using Team17.Common;
using Team17.Input;

namespace Team17.Scripts.Services
{
	// Token: 0x02000216 RID: 534
	public interface IAutoEngagingDeviceService : IService
	{
		// Token: 0x0600115F RID: 4447
		bool TryGetAutoEngagingController(out IDeviceId deviceId);

		// Token: 0x06001160 RID: 4448
		void SetAutoEngagingController(IDeviceId deviceId);

		// Token: 0x06001161 RID: 4449
		void ClearAutoEngagingController();
	}
}
