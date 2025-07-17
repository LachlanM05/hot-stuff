using System;
using T17.Services;
using Team17.Common;
using Team17.Platform.User;

namespace Team17.Scripts.Services.ApplicationStatus
{
	// Token: 0x02000234 RID: 564
	public class EntitlementsRefreshOnResumeService : IService
	{
		// Token: 0x0600121B RID: 4635 RVA: 0x000591C6 File Offset: 0x000573C6
		public void OnStartUp()
		{
			Services.ApplicationStatusPlatformService.OnApplicationStatusChangedEvent += this.OnApplicationStatusChanged;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x000591DE File Offset: 0x000573DE
		private void OnApplicationStatusChanged(EApplicationStatus applicationStatus)
		{
			if (applicationStatus != EApplicationStatus.ResumedFromSuspend)
			{
				return;
			}
			this.Refresh();
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x000591EB File Offset: 0x000573EB
		public void OnCleanUp()
		{
			Services.ApplicationStatusPlatformService.OnApplicationStatusChangedEvent -= this.OnApplicationStatusChanged;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00059203 File Offset: 0x00057403
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00059208 File Offset: 0x00057408
		private void Refresh()
		{
			IUser user;
			if (Services.UserService.TryGetPrimaryUser(out user))
			{
				Services.Entitlements.SetUser(user);
			}
		}
	}
}
