using System;
using System.Diagnostics;
using Rewired;
using Team17.Common;

namespace Team17.Scripts.Services
{
	// Token: 0x02000219 RID: 537
	internal class InternalInputMapsService : IInternalInputMapsService, IService
	{
		// Token: 0x0600116F RID: 4463 RVA: 0x00057E77 File Offset: 0x00056077
		public void OnStartUp()
		{
			ReInput.ControllerConnectedEvent += this.OnControllerConnectedEvent;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00057E8A File Offset: 0x0005608A
		public void OnCleanUp()
		{
			ReInput.ControllerConnectedEvent -= this.OnControllerConnectedEvent;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00057E9D File Offset: 0x0005609D
		public void OnUpdate(float deltaTime)
		{
			if (this._pendingControllerStateChange != null)
			{
				this.HandlePendingControllerStateChange();
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00057EB0 File Offset: 0x000560B0
		private void HandlePendingControllerStateChange()
		{
			Player rewiredPlayer = this.GetRewiredPlayer();
			if (rewiredPlayer != null)
			{
				bool flag = false;
				if (this._pendingControllerStateChange.controller == null)
				{
					flag = true;
				}
				else
				{
					foreach (Controller controller in rewiredPlayer.controllers.Controllers)
					{
						if (controller.type == this._pendingControllerStateChange.controllerType && controller.id == this._pendingControllerStateChange.controller.id)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					this.ApplyCurrentState();
					this._pendingControllerStateChange = null;
				}
			}
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00057F5C File Offset: 0x0005615C
		public void DisableAllMaps()
		{
			Player rewiredPlayer = this.GetRewiredPlayer();
			if (rewiredPlayer == null)
			{
				return;
			}
			rewiredPlayer.controllers.maps.SetAllMapsEnabled(false);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00057F7A File Offset: 0x0005617A
		public void SetToEngagementMode()
		{
			this._state = InternalInputMapsService.EState.Engagement;
			this.ApplyCurrentState();
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00057F89 File Offset: 0x00056189
		public void SetToGameplayMode()
		{
			this._state = InternalInputMapsService.EState.Gameplay;
			this.ApplyCurrentState();
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00057F98 File Offset: 0x00056198
		public void SetToUIOnlyMode()
		{
			this._state = InternalInputMapsService.EState.UI;
			this.ApplyCurrentState();
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00057FA7 File Offset: 0x000561A7
		public void SetToUIWithChatMode()
		{
			this._state = InternalInputMapsService.EState.UIWithChat;
			this.ApplyCurrentState();
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00057FB8 File Offset: 0x000561B8
		private void ApplyCurrentState()
		{
			this.DisableAllMaps();
			switch (this._state)
			{
			case InternalInputMapsService.EState.NotApplied:
				return;
			case InternalInputMapsService.EState.Engagement:
				this.SetMapsEnabledForAllControllers(true, 3);
				return;
			case InternalInputMapsService.EState.UI:
				this.SetMapsEnabledForAllControllers(true, 0);
				this.SetMapsEnabledForAllControllers(true, 5);
				this.SetMapsEnabledForAllControllers(true, 6);
				return;
			case InternalInputMapsService.EState.UIWithChat:
				this.SetMapsEnabledForAllControllers(true, 0);
				this.SetMapsEnabledForAllControllers(true, 5);
				this.SetMapsEnabledForAllControllers(true, 1);
				return;
			case InternalInputMapsService.EState.Gameplay:
				this.SetMapsEnabledForAllControllers(true, 2);
				this.SetMapsEnabledForAllControllers(true, 0);
				this.SetMapsEnabledForAllControllers(true, 6);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00058048 File Offset: 0x00056248
		private void SetMapsEnabledForAllControllers(bool enabled, int category)
		{
			Player rewiredPlayer = this.GetRewiredPlayer();
			if (rewiredPlayer == null)
			{
				return;
			}
			rewiredPlayer.controllers.maps.SetMapsEnabled(enabled, ControllerType.Keyboard, category);
			rewiredPlayer.controllers.maps.SetMapsEnabled(enabled, ControllerType.Custom, category);
			rewiredPlayer.controllers.maps.SetMapsEnabled(enabled, ControllerType.Joystick, category);
			rewiredPlayer.controllers.maps.SetMapsEnabled(enabled, ControllerType.Mouse, category);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000580B1 File Offset: 0x000562B1
		private Player GetRewiredPlayer()
		{
			if (ReInput.isReady)
			{
				return ReInput.players.GetPlayer(0);
			}
			return null;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000580C7 File Offset: 0x000562C7
		private void OnControllerConnectedEvent(ControllerStatusChangedEventArgs eventArgs)
		{
			this._pendingControllerStateChange = eventArgs;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000580D0 File Offset: 0x000562D0
		[Conditional("DEBUG")]
		[Conditional("QA")]
		private void EnableDebugMaps()
		{
			this.SetMapsEnabledForAllControllers(true, 4);
		}

		// Token: 0x04000E56 RID: 3670
		private InternalInputMapsService.EState _state;

		// Token: 0x04000E57 RID: 3671
		private ControllerStatusChangedEventArgs _pendingControllerStateChange;

		// Token: 0x020003BD RID: 957
		private enum EState
		{
			// Token: 0x040014D4 RID: 5332
			NotApplied,
			// Token: 0x040014D5 RID: 5333
			Engagement,
			// Token: 0x040014D6 RID: 5334
			UI,
			// Token: 0x040014D7 RID: 5335
			UIWithChat,
			// Token: 0x040014D8 RID: 5336
			Gameplay
		}
	}
}
