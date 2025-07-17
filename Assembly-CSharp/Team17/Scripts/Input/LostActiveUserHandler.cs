using System;
using T17.Services;
using Team17.Common;
using Team17.Platform.EngagementService;

namespace Team17.Scripts.Input
{
	// Token: 0x02000238 RID: 568
	public class LostActiveUserHandler : IService
	{
		// Token: 0x06001228 RID: 4648 RVA: 0x00059236 File Offset: 0x00057436
		public LostActiveUserHandler(IPostEngagementService controllerDisconnectHandlerService)
		{
			this._controllerDisconnectHandlerService = controllerDisconnectHandlerService;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00059245 File Offset: 0x00057445
		public void OnStartUp()
		{
			this.BindDelegates();
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0005924D File Offset: 0x0005744D
		public void OnCleanUp()
		{
			this.UnbindDelegates();
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00059255 File Offset: 0x00057455
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00059257 File Offset: 0x00057457
		private void OnEngagementStateChanged(EngagementState from, EngagementState to)
		{
			if (from != EngagementState.Idle)
			{
				return;
			}
			if (to == EngagementState.Reengagement || to == EngagementState.Engagement)
			{
				this._controllerDisconnectHandlerService.EnsureUserAndControllerEngaged(false);
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00059272 File Offset: 0x00057472
		private void BindDelegates()
		{
			Services.EngagementService.OnEngagementStateChangedEvent += this.OnEngagementStateChanged;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0005928A File Offset: 0x0005748A
		private void UnbindDelegates()
		{
			Services.EngagementService.OnEngagementStateChangedEvent -= this.OnEngagementStateChanged;
		}

		// Token: 0x04000E8C RID: 3724
		private readonly IPostEngagementService _controllerDisconnectHandlerService;
	}
}
