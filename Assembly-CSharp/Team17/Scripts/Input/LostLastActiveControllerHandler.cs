using System;
using Rewired;
using T17.Services;
using Team17.Common;

namespace Team17.Scripts.Input
{
	// Token: 0x02000239 RID: 569
	public class LostLastActiveControllerHandler : IService
	{
		// Token: 0x0600122F RID: 4655 RVA: 0x000592A2 File Offset: 0x000574A2
		public LostLastActiveControllerHandler(IPostEngagementService controllerDisconnectHandlerService)
		{
			this._controllerDisconnectHandlerService = controllerDisconnectHandlerService;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x000592B1 File Offset: 0x000574B1
		private void OnControllerPreDisconnectEvent(ControllerStatusChangedEventArgs eventArgs)
		{
			if (Services.InputService.LastControllerId != eventArgs.controllerId || !Services.InputService.IsLastActiveInputController())
			{
				return;
			}
			this._controllerDisconnectHandlerService.EnsureUserAndControllerEngaged(true);
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000592DE File Offset: 0x000574DE
		private void BindDelegates()
		{
			ReInput.ControllerPreDisconnectEvent += this.OnControllerPreDisconnectEvent;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000592F1 File Offset: 0x000574F1
		private void UnbindDelegates()
		{
			ReInput.ControllerPreDisconnectEvent -= this.OnControllerPreDisconnectEvent;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00059304 File Offset: 0x00057504
		public void OnStartUp()
		{
			this.BindDelegates();
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0005930C File Offset: 0x0005750C
		public void OnCleanUp()
		{
			this.UnbindDelegates();
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00059314 File Offset: 0x00057514
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x04000E8D RID: 3725
		private readonly IPostEngagementService _controllerDisconnectHandlerService;
	}
}
