using System;
using Team17.Common;

namespace Team17.Scripts.Services.ApplicationStatus
{
	// Token: 0x02000233 RID: 563
	public class ApplicationStatusPlatformService : IApplicationStatusPlatformService, IService
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06001214 RID: 4628 RVA: 0x00059138 File Offset: 0x00057338
		// (remove) Token: 0x06001215 RID: 4629 RVA: 0x00059170 File Offset: 0x00057370
		public event IApplicationStatusPlatformService.ApplicationStatusChangedEvent OnApplicationStatusChangedEvent;

		// Token: 0x06001216 RID: 4630 RVA: 0x000591A5 File Offset: 0x000573A5
		public void OnStartUp()
		{
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x000591A7 File Offset: 0x000573A7
		public void OnCleanUp()
		{
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x000591A9 File Offset: 0x000573A9
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x000591AB File Offset: 0x000573AB
		public void InformApplicationStatusChanged(EApplicationStatus applicationStatus)
		{
			IApplicationStatusPlatformService.ApplicationStatusChangedEvent onApplicationStatusChangedEvent = this.OnApplicationStatusChangedEvent;
			if (onApplicationStatusChangedEvent == null)
			{
				return;
			}
			onApplicationStatusChangedEvent(applicationStatus);
		}
	}
}
