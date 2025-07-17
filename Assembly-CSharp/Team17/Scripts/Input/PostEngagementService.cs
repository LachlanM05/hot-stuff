using System;
using System.Collections.Generic;
using T17.Services;
using T17.UI;
using Team17.Common;
using Team17.Platform.EngagementService;
using Team17.Scripts.Platforms.Enums;
using Team17.Scripts.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.Scripts.Input
{
	// Token: 0x0200023A RID: 570
	public class PostEngagementService : IPostEngagementService, IService
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x00059316 File Offset: 0x00057516
		private bool IsDialogAlreadyOpen
		{
			get
			{
				return this._dialogHandle > 0U;
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001237 RID: 4663 RVA: 0x00059324 File Offset: 0x00057524
		// (remove) Token: 0x06001238 RID: 4664 RVA: 0x0005935C File Offset: 0x0005755C
		public event Action OnHandlingLostUserEvent;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001239 RID: 4665 RVA: 0x00059394 File Offset: 0x00057594
		// (remove) Token: 0x0600123A RID: 4666 RVA: 0x000593CC File Offset: 0x000575CC
		public event Action OnHandlingLostControllerEvent;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x0600123B RID: 4667 RVA: 0x00059404 File Offset: 0x00057604
		// (remove) Token: 0x0600123C RID: 4668 RVA: 0x0005943C File Offset: 0x0005763C
		public event Action OnCompletedHandlingLostEvent;

		// Token: 0x0600123D RID: 4669 RVA: 0x00059471 File Offset: 0x00057671
		public PostEngagementService()
		{
			this._services.Add(new LostActiveUserHandler(this));
			this._services.Add(new LostLastActiveControllerHandler(this));
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000594A6 File Offset: 0x000576A6
		public void EnsureUserAndControllerEngaged(bool controllerLostEvent)
		{
			if (this._handlingLostEvent)
			{
				return;
			}
			if (!this.HasProgressedPassedIIS())
			{
				return;
			}
			this.BeginHandlingLostEvent(controllerLostEvent);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x000594C4 File Offset: 0x000576C4
		private bool HasProgressedPassedIIS()
		{
			return string.CompareOrdinal(SceneManager.GetActiveScene().name, SceneConsts.kEngagementScene) != 0 && string.CompareOrdinal(SceneManager.GetActiveScene().name, SceneConsts.kIdentScene) != 0;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00059508 File Offset: 0x00057708
		private void BeginHandlingLostEvent(bool controllerLostEvent)
		{
			this._handlingLostEvent = true;
			this._inputModeHandle = null;
			if (controllerLostEvent)
			{
				Action onHandlingLostControllerEvent = this.OnHandlingLostControllerEvent;
				if (onHandlingLostControllerEvent != null)
				{
					onHandlingLostControllerEvent();
				}
			}
			else
			{
				Action onHandlingLostUserEvent = this.OnHandlingLostUserEvent;
				if (onHandlingLostUserEvent != null)
				{
					onHandlingLostUserEvent();
				}
			}
			if (!Services.PlatformService.HasFlag(EPlatformFlags.PlatformHandlesControllerDisconnects))
			{
				this._dialogHandle = UIDialogManager.Instance.ShowOKDialog("WIRELESS CONTROLLER DISCONNECTED", "Please connect a controller and select “OK” to continue", new Action(this.OnDialogOk), false);
				this._inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.Engagement, this);
				Services.InputUserService.RemoveAllDevicesFromUser(0);
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0005959C File Offset: 0x0005779C
		private void EndHandlingLostEvent()
		{
			this._handlingLostEvent = false;
			UIDialogManager.Instance.CloseDialog(this._dialogHandle);
			this._dialogHandle = 0U;
			if (this._inputModeHandle != null)
			{
				Services.InputService.PopHandle(this._inputModeHandle);
			}
			if (ControllerMenuUI.GetCurrentSelectedControl() == null)
			{
				this.RelocateFocus();
			}
			Action onCompletedHandlingLostEvent = this.OnCompletedHandlingLostEvent;
			if (onCompletedHandlingLostEvent == null)
			{
				return;
			}
			onCompletedHandlingLostEvent();
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00059604 File Offset: 0x00057804
		private void RelocateFocus()
		{
			foreach (MenuComponent menuComponent in Object.FindObjectsOfType<MenuComponent>())
			{
				if (menuComponent != null && menuComponent.isActiveAndEnabled)
				{
					menuComponent.AutoSelect();
					return;
				}
			}
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00059641 File Offset: 0x00057841
		private void OnDialogOk()
		{
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00059643 File Offset: 0x00057843
		private void AttemptEndHandlingLostEvent()
		{
			if (this.IsEngaged())
			{
				this.EndHandlingLostEvent();
			}
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00059653 File Offset: 0x00057853
		private void OnEngagementStateChanged(EngagementState from, EngagementState to)
		{
			if (!this._handlingLostEvent)
			{
				return;
			}
			if (to != EngagementState.Idle)
			{
				return;
			}
			this.AttemptEndHandlingLostEvent();
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0005966C File Offset: 0x0005786C
		public void OnStartUp()
		{
			foreach (IService service in this._services)
			{
				service.OnStartUp();
			}
			Services.EngagementService.OnEngagementStateChangedEvent += this.OnEngagementStateChanged;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x000596D4 File Offset: 0x000578D4
		public void OnCleanUp()
		{
			foreach (IService service in this._services)
			{
				service.OnCleanUp();
			}
			Services.EngagementService.OnEngagementStateChangedEvent -= this.OnEngagementStateChanged;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0005973C File Offset: 0x0005793C
		public void OnUpdate(float deltaTime)
		{
			foreach (IService service in this._services)
			{
				service.OnUpdate(deltaTime);
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00059790 File Offset: 0x00057990
		private bool IsEngaged()
		{
			IEngagementService engagementService = Services.EngagementService;
			return engagementService != null && engagementService.State == EngagementState.Idle;
		}

		// Token: 0x04000E8E RID: 3726
		public const string ControllerDisconnectedTitle = "WIRELESS CONTROLLER DISCONNECTED";

		// Token: 0x04000E8F RID: 3727
		public const string ControllerDisconnectedMessage = "Please connect a controller and select “OK” to continue";

		// Token: 0x04000E90 RID: 3728
		private readonly List<IService> _services = new List<IService>();

		// Token: 0x04000E91 RID: 3729
		private uint _dialogHandle;

		// Token: 0x04000E95 RID: 3733
		private InputModeHandle _inputModeHandle;

		// Token: 0x04000E96 RID: 3734
		private bool _handlingLostEvent;
	}
}
