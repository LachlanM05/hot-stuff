using System;
using T17.Services;
using Team17.Common;
using Team17.Scripts.Services.ApplicationStatus;
using UnityEngine;

namespace Team17.Scripts.Services.Suspend
{
	// Token: 0x02000230 RID: 560
	public class UnityApplicationFocusListenerService : IService
	{
		// Token: 0x0600120C RID: 4620 RVA: 0x000590E7 File Offset: 0x000572E7
		public void OnStartUp()
		{
			Application.focusChanged += this.OnFocusChanged;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x000590FA File Offset: 0x000572FA
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x000590FC File Offset: 0x000572FC
		public void OnCleanUp()
		{
			Application.focusChanged -= this.OnFocusChanged;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00059110 File Offset: 0x00057310
		private void OnFocusChanged(bool hasFocus)
		{
			EApplicationStatus eapplicationStatus = (hasFocus ? EApplicationStatus.FocusGained : EApplicationStatus.FocusLost);
			Services.ApplicationStatusPlatformService.InformApplicationStatusChanged(eapplicationStatus);
		}
	}
}
