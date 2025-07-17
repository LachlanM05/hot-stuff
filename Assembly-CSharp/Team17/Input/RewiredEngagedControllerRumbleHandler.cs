using System;
using Rewired;
using Team17.Common;

namespace Team17.Input
{
	// Token: 0x020001E3 RID: 483
	public class RewiredEngagedControllerRumbleHandler : IControllerDeviceRumbleHandler, IService
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x00055666 File Offset: 0x00053866
		private bool HasEngagedRumbleDevice
		{
			get
			{
				return this._engagedRumbleDevice != null;
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x00055674 File Offset: 0x00053874
		private void OnControllerAddedForUser(ControllerAssignmentChangedEventArgs eventArgs)
		{
			Controller controller = eventArgs.controller;
			if (controller == null)
			{
				return;
			}
			if (eventArgs.player.controllers.GetLastActiveController() == null)
			{
				this.TrySetEngagedRumbleDevice(controller);
			}
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x000556A8 File Offset: 0x000538A8
		public void OnStartUp()
		{
			this._player = ReInput.players.GetPlayer(0);
			this._player.controllers.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.OnLastActiveControllerChangedForDevice));
			this._player.controllers.ControllerAddedEvent += this.OnControllerAddedForUser;
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x000556FE File Offset: 0x000538FE
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00055700 File Offset: 0x00053900
		public void OnCleanUp()
		{
			this.StopDeviceRumble();
			if (this._player != null)
			{
				this._player.controllers.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.OnLastActiveControllerChangedForDevice));
				this._player.controllers.ControllerAddedEvent -= this.OnControllerAddedForUser;
			}
			this._player = null;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0005575A File Offset: 0x0005395A
		public void SetDeviceRumble(float left, float right)
		{
			if (this._engagedRumbleDevice == null)
			{
				return;
			}
			if (!this._rumbling)
			{
				this._rumbling = true;
			}
			this._engagedRumbleDevice.SetVibration(left, right);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00055781 File Offset: 0x00053981
		public void StopDeviceRumble()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this._engagedRumbleDevice == null)
			{
				return;
			}
			if (this._rumbling)
			{
				this._rumbling = false;
				this._engagedRumbleDevice.StopVibration();
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x000557AE File Offset: 0x000539AE
		private void OnLastActiveControllerChangedForDevice(Player player, Controller controller)
		{
			if (this.TrySetEngagedRumbleDevice(controller))
			{
				return;
			}
			this.ClearEngagedRumbleDevice();
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x000557C0 File Offset: 0x000539C0
		private bool TrySetEngagedRumbleDevice(Controller controller)
		{
			Joystick joystick = controller as Joystick;
			if (joystick != null && joystick.supportsVibration)
			{
				this.SetEngagedRumbleDevice(joystick);
				return true;
			}
			return false;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x000557E9 File Offset: 0x000539E9
		private void ClearEngagedRumbleDevice()
		{
			if (this.HasEngagedRumbleDevice)
			{
				this.StopDeviceRumble();
			}
			this._engagedRumbleDevice = null;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00055800 File Offset: 0x00053A00
		private void SetEngagedRumbleDevice(Joystick joystick)
		{
			this.ClearEngagedRumbleDevice();
			this._engagedRumbleDevice = joystick;
		}

		// Token: 0x04000DE5 RID: 3557
		private Player _player;

		// Token: 0x04000DE6 RID: 3558
		private Joystick _engagedRumbleDevice;

		// Token: 0x04000DE7 RID: 3559
		private bool _rumbling;
	}
}
