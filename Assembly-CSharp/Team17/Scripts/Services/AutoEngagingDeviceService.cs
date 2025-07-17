using System;
using T17.Services;
using Team17.Common;
using Team17.Input;
using Team17.Platform.EngagementService;

namespace Team17.Scripts.Services
{
	// Token: 0x02000217 RID: 535
	public class AutoEngagingDeviceService : IAutoEngagingDeviceService, IService
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x00057DFE File Offset: 0x00055FFE
		public bool TryGetAutoEngagingController(out IDeviceId deviceId)
		{
			if (this._autoEngagingDeviceId == null)
			{
				deviceId = null;
				return false;
			}
			deviceId = this._autoEngagingDeviceId;
			return true;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00057E16 File Offset: 0x00056016
		public void SetAutoEngagingController(IDeviceId deviceId)
		{
			this._autoEngagingDeviceId = deviceId;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00057E1F File Offset: 0x0005601F
		public void ClearAutoEngagingController()
		{
			if (this._autoEngagingDeviceId == null)
			{
				return;
			}
			this._autoEngagingDeviceId = null;
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00057E31 File Offset: 0x00056031
		private void OnEngagementStateChangedEvent(EngagementState from, EngagementState to)
		{
			if (to == EngagementState.Idle)
			{
				this.ClearAutoEngagingController();
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00057E3D File Offset: 0x0005603D
		public void OnStartUp()
		{
			Services.EngagementService.OnEngagementStateChangedEvent += this.OnEngagementStateChangedEvent;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00057E55 File Offset: 0x00056055
		public void OnCleanUp()
		{
			Services.EngagementService.OnEngagementStateChangedEvent += this.OnEngagementStateChangedEvent;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00057E6D File Offset: 0x0005606D
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x04000E55 RID: 3669
		private IDeviceId _autoEngagingDeviceId;
	}
}
