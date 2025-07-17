using System;
using Team17.Common;

namespace Team17.Scripts.Services.ApplicationStatus
{
	// Token: 0x02000232 RID: 562
	public interface IApplicationStatusPlatformService : IService
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001211 RID: 4625
		// (remove) Token: 0x06001212 RID: 4626
		event IApplicationStatusPlatformService.ApplicationStatusChangedEvent OnApplicationStatusChangedEvent;

		// Token: 0x06001213 RID: 4627
		void InformApplicationStatusChanged(EApplicationStatus applicationStatus);

		// Token: 0x020003C4 RID: 964
		// (Invoke) Token: 0x06001887 RID: 6279
		public delegate void ApplicationStatusChangedEvent(EApplicationStatus applicationStatus);
	}
}
