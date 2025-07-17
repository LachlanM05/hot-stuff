using System;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using Team17.Common;

namespace Team17.Scripts.Services.Input
{
	// Token: 0x0200021F RID: 543
	public class MirandaInputService : IMirandaInputService, IService
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600118F RID: 4495 RVA: 0x00058138 File Offset: 0x00056338
		// (remove) Token: 0x06001190 RID: 4496 RVA: 0x00058170 File Offset: 0x00056370
		public event Action OnStackChanged;

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x000581A5 File Offset: 0x000563A5
		public IReadOnlyCollection<InputModeState> CurrentInputModeStack
		{
			get
			{
				return this._inputStack;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x000581AD File Offset: 0x000563AD
		public IMirandaInputService.EInputMode CurrentMode
		{
			get
			{
				if (this._inputStack.Last != null)
				{
					return this._inputStack.Last.Value.RequestedMode;
				}
				return IMirandaInputService.EInputMode.None;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x000581D3 File Offset: 0x000563D3
		// (set) Token: 0x06001194 RID: 4500 RVA: 0x000581DB File Offset: 0x000563DB
		public int LastControllerId { get; private set; } = -1;

		// Token: 0x06001195 RID: 4501 RVA: 0x000581E4 File Offset: 0x000563E4
		public InputModeHandle PushMode(IMirandaInputService.EInputMode mode, object requester)
		{
			InputModeHandle inputModeHandle = new InputModeHandle();
			InputModeState inputModeState = new InputModeState(mode, inputModeHandle, requester);
			this.ApplyMode(mode);
			this._inputStack.AddLast(inputModeState);
			Action onStackChanged = this.OnStackChanged;
			if (onStackChanged != null)
			{
				onStackChanged();
			}
			return inputModeHandle;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00058228 File Offset: 0x00056428
		public void PopHandle(InputModeHandle handle)
		{
			if (!this.IsHandleInStack(handle))
			{
				T17Debug.LogError("[INPUT] attempt to pop handle from the stack, but handle is no longer in the stack.");
				return;
			}
			InputModeState inputModeState = this._inputStack.First((InputModeState state) => state.Handle == handle);
			this._inputStack.Remove(inputModeState);
			this.PeekStackAndApplyMode();
			Action onStackChanged = this.OnStackChanged;
			if (onStackChanged == null)
			{
				return;
			}
			onStackChanged();
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00058296 File Offset: 0x00056496
		public bool IsValidHandle(InputModeHandle inputModeHandle)
		{
			return this.IsHandleInStack(inputModeHandle);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000582A0 File Offset: 0x000564A0
		public bool IsLastActiveInputController()
		{
			ControllerType lastActiveControllerType = ReInput.controllers.GetLastActiveControllerType();
			return lastActiveControllerType == ControllerType.Joystick || lastActiveControllerType == ControllerType.Custom;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x000582C3 File Offset: 0x000564C3
		public bool WasLastControllerAPointer()
		{
			return ReInput.controllers.GetLastActiveControllerType() == ControllerType.Mouse;
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x000582D4 File Offset: 0x000564D4
		private void ApplyMode(IMirandaInputService.EInputMode mode)
		{
			switch (mode)
			{
			case IMirandaInputService.EInputMode.None:
				this._inputMapsService.DisableAllMaps();
				return;
			case IMirandaInputService.EInputMode.Engagement:
				this._inputMapsService.SetToEngagementMode();
				return;
			case IMirandaInputService.EInputMode.UI:
				this._inputMapsService.SetToUIOnlyMode();
				return;
			case IMirandaInputService.EInputMode.UIWithChat:
				this._inputMapsService.SetToUIWithChatMode();
				return;
			case IMirandaInputService.EInputMode.Gameplay:
				this._inputMapsService.SetToGameplayMode();
				return;
			default:
				throw new ArgumentOutOfRangeException("mode", mode, null);
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0005834A File Offset: 0x0005654A
		private void PeekStackAndApplyMode()
		{
			this._inputMapsService.DisableAllMaps();
			if (this._inputStack.First == null)
			{
				return;
			}
			this.ApplyMode(this._inputStack.Last.Value.RequestedMode);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00058380 File Offset: 0x00056580
		private bool IsHandleInStack(InputModeHandle handle)
		{
			return this._inputStack.Any((InputModeState state) => state.Handle == handle);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x000583B1 File Offset: 0x000565B1
		public void OnStartUp()
		{
			this._inputMapsService.OnStartUp();
			this.ApplyMode(IMirandaInputService.EInputMode.None);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x000583C5 File Offset: 0x000565C5
		public void OnCleanUp()
		{
			this._inputMapsService.OnCleanUp();
			this.LastControllerId = -1;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x000583D9 File Offset: 0x000565D9
		public void OnUpdate(float deltaTime)
		{
			this._inputMapsService.OnUpdate(deltaTime);
			this.UpdateLastUsedControllerId();
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x000583F0 File Offset: 0x000565F0
		private void UpdateLastUsedControllerId()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Controller lastActiveController = ReInput.controllers.GetLastActiveController();
			if (lastActiveController == null)
			{
				return;
			}
			if (this.LastControllerId == lastActiveController.id)
			{
				return;
			}
			this.LastControllerId = lastActiveController.id;
		}

		// Token: 0x04000E5E RID: 3678
		private readonly IInternalInputMapsService _inputMapsService = new InternalInputMapsService();

		// Token: 0x04000E5F RID: 3679
		private readonly LinkedList<InputModeState> _inputStack = new LinkedList<InputModeState>();
	}
}
