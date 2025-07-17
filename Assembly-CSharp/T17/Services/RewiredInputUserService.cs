using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rewired;
using Team17.Common;
using Team17.Input;
using Team17.Platform.EngagementService;
using Team17.Platform.UserService;

namespace T17.Services
{
	// Token: 0x02000244 RID: 580
	public class RewiredInputUserService<TDeviceId, TDeviceIdProvider> : IInputUserService, IService where TDeviceId : IDeviceId where TDeviceIdProvider : IDeviceIdProvider<Controller, TDeviceId>, new()
	{
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060012C2 RID: 4802 RVA: 0x0005ABC0 File Offset: 0x00058DC0
		// (remove) Token: 0x060012C3 RID: 4803 RVA: 0x0005ABF8 File Offset: 0x00058DF8
		public event DeviceRemovedFromUserCallback OnDeviceRemovedFromUser;

		// Token: 0x060012C4 RID: 4804 RVA: 0x0005AC2D File Offset: 0x00058E2D
		public RewiredInputUserService(TDeviceIdProvider deviceIdProvider, IUserService userService)
		{
			this._deviceIdProvider = deviceIdProvider;
			this._userService = userService;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0005AC43 File Offset: 0x00058E43
		public bool AddUser(int userIndex)
		{
			this._user.isActive = true;
			return true;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0005AC54 File Offset: 0x00058E54
		public void AssignDeviceToUser(IDeviceId deviceId, int userIndex = 0)
		{
			if (!RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return;
			}
			Controller deviceFromId = this._deviceIdProvider.GetDeviceFromId((TDeviceId)((object)deviceId));
			if (deviceFromId == null)
			{
				return;
			}
			this.RewiredPlayer.isPlaying = true;
			this.AssignControllerToPlayer(this.RewiredPlayer, deviceFromId);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0005ACA0 File Offset: 0x00058EA0
		private async void OnControllerChanged(Controller controller)
		{
			await Task.Yield();
			if (this.IsPrimaryUserEngaged() && this.ShouldAssignAllControllersToPrimaryUser())
			{
				this._user.engagedController = controller;
				this.AssignControllerToPlayer(this.RewiredPlayer, controller);
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0005ACDF File Offset: 0x00058EDF
		private bool IsPrimaryUserEngaged()
		{
			return Services.EngagementService.State == EngagementState.Idle;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0005ACEE File Offset: 0x00058EEE
		private bool ShouldAssignAllControllersToPrimaryUser()
		{
			return true;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0005ACF4 File Offset: 0x00058EF4
		public bool GetEngagingDevice(out IDeviceId deviceId)
		{
			if (this.IsAnyControllerEngaging())
			{
				Controller lastActiveController = ReInput.controllers.GetLastActiveController();
				deviceId = this._deviceIdProvider.GetDeviceId(lastActiveController);
				return this.IsDeviceSupported(deviceId) && this.IsPrimaryUser(deviceId);
			}
			IDeviceId deviceId2;
			if (Services.AutoEngagingDeviceService.TryGetAutoEngagingController(out deviceId2) && this.IsDeviceSupported(deviceId2) && this.IsPrimaryUser(deviceId2))
			{
				deviceId = deviceId2;
				return true;
			}
			deviceId = null;
			return false;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0005AD6C File Offset: 0x00058F6C
		public bool IsDeviceEngaged(IDeviceId deviceId)
		{
			if (!RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return false;
			}
			if (!this._user.isActive)
			{
				return false;
			}
			Controller deviceFromId = this._deviceIdProvider.GetDeviceFromId((TDeviceId)((object)deviceId));
			return deviceFromId != null && deviceFromId.type != ControllerType.Mouse && this._user.engagedController == deviceFromId;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0005ADC7 File Offset: 0x00058FC7
		public bool IsValidUser(int userIndex)
		{
			return userIndex == 0 && this._user.isActive;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0005ADD9 File Offset: 0x00058FD9
		public void OnCleanUp()
		{
			this.DeregisterControllerConnectionEvents();
			this.DeregisterEngagementEvents();
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0005ADE7 File Offset: 0x00058FE7
		public void OnStartUp()
		{
			if (RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				ReInput.configuration.autoAssignJoysticks = false;
				this.RegisterControllerConnectionEvents();
				this.RegisterEngagementEvents();
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0005AE07 File Offset: 0x00059007
		public void OnUpdate(float deltaTime)
		{
			if (!this._hasInitialisedInitialPlayer && ReInput.isReady)
			{
				if (RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
				{
					this.AssignAllControllersToPlayer(this.RewiredSystemPlayer);
				}
				else
				{
					this.AssignAllControllersToPlayer(this.RewiredPlayer);
				}
				this._hasInitialisedInitialPlayer = true;
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0005AE40 File Offset: 0x00059040
		public void RemoveAllDevicesFromUser(int userIndex = 0)
		{
			if (!RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return;
			}
			IDeviceId deviceId = null;
			if (this._user.engagedController != null)
			{
				deviceId = this._deviceIdProvider.GetDeviceId(this._user.engagedController);
			}
			foreach (Controller controller in new List<Controller>(this.RewiredPlayer.controllers.Controllers))
			{
				this.AssignControllerToSystemPlayer(controller);
			}
			if (deviceId != null)
			{
				DeviceRemovedFromUserCallback onDeviceRemovedFromUser = this.OnDeviceRemovedFromUser;
				if (onDeviceRemovedFromUser != null)
				{
					onDeviceRemovedFromUser(deviceId, userIndex);
				}
			}
			this.RewiredPlayer.isPlaying = false;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0005AF00 File Offset: 0x00059100
		public void RemoveAllUsers()
		{
			this.RemoveUser(0);
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0005AF0C File Offset: 0x0005910C
		public void RemoveDeviceFromUser(IDeviceId deviceId, int userIndex = 0)
		{
			if (!RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return;
			}
			Controller deviceFromId = this._deviceIdProvider.GetDeviceFromId((TDeviceId)((object)deviceId));
			if (deviceFromId == null)
			{
				return;
			}
			this.AssignControllerToSystemPlayer(deviceFromId);
			DeviceRemovedFromUserCallback onDeviceRemovedFromUser = this.OnDeviceRemovedFromUser;
			if (onDeviceRemovedFromUser == null)
			{
				return;
			}
			onDeviceRemovedFromUser(deviceId, userIndex);
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0005AF56 File Offset: 0x00059156
		public bool RemoveUser(int userIndex)
		{
			this._user.isActive = false;
			this.RewiredPlayer.isPlaying = false;
			return true;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0005AF71 File Offset: 0x00059171
		public Controller GetLastActiveController()
		{
			if (RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return ReInput.controllers.GetLastActiveController(ControllerType.Joystick);
			}
			return ReInput.controllers.GetLastActiveController();
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0005AF90 File Offset: 0x00059190
		public IDeviceId GetPrimaryController()
		{
			if (this._user.engagedController != null)
			{
				return this._deviceIdProvider.GetDeviceId(this._user.engagedController);
			}
			Controller lastActiveController = this.GetLastActiveController();
			if (lastActiveController == null)
			{
				return null;
			}
			return this._deviceIdProvider.GetDeviceId(lastActiveController);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0005AFEF File Offset: 0x000591EF
		private bool IsDeviceSupported(IDeviceId deviceId)
		{
			return typeof(TDeviceId) == typeof(NullDeviceId) || deviceId is TDeviceId;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0005B018 File Offset: 0x00059218
		private bool IsPrimaryUser(IDeviceId deviceId)
		{
			IPlatformPrimaryUserDeviceAssociation platformPrimaryUserDeviceAssociation = this._userService.PlatformPrimaryUserDeviceAssociation;
			return platformPrimaryUserDeviceAssociation == null || platformPrimaryUserDeviceAssociation.IsDeviceAssociatedWithPlatformPrimaryUser(deviceId);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0005B040 File Offset: 0x00059240
		private bool IsAnyControllerEngaging()
		{
			if (this.HasAnyJoysticksNotAssignedToSystem())
			{
				this.AssignAllControllersToPlayer(this.RewiredSystemPlayer);
			}
			bool flag = RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsPlayerEngagingWithAction(this.RewiredSystemPlayer, 19);
			bool flag2 = RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsPlayerEngagingWithAction(this.RewiredPlayer, 19);
			return flag || flag2;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0005B07E File Offset: 0x0005927E
		private static bool IsPlayerEngagingWithAction(Player player, int action)
		{
			return player.GetButtonDown(action);
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0005B087 File Offset: 0x00059287
		private static bool IsPlayerEngagingWithAny(Player player)
		{
			return player.GetAnyButtonDown();
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0005B08F File Offset: 0x0005928F
		private void AssignControllerToSystemPlayer(Controller controller)
		{
			if (!RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return;
			}
			if (this._user.engagedController == controller)
			{
				this._user.engagedController = null;
				this.RewiredPlayer.isPlaying = false;
			}
			this.AssignControllerToPlayer(this.RewiredSystemPlayer, controller);
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0005B0CC File Offset: 0x000592CC
		private void AssignAllControllersToPlayer(Player rewiredPlayer)
		{
			foreach (Controller controller in ReInput.controllers.Controllers)
			{
				this.AssignControllerToPlayer(rewiredPlayer, controller);
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0005B120 File Offset: 0x00059320
		private void AssignControllerToPlayer(Player rewiredPlayer, Controller controller)
		{
			rewiredPlayer.controllers.AddController(controller, true);
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0005B12F File Offset: 0x0005932F
		private static bool IsStrictWithEngagement()
		{
			return true;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0005B134 File Offset: 0x00059334
		private bool HasAnyJoysticksNotAssignedToSystem()
		{
			int joystickCount = this.RewiredSystemPlayer.controllers.joystickCount;
			int joystickCount2 = ReInput.controllers.joystickCount;
			return joystickCount < joystickCount2;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0005B15F File Offset: 0x0005935F
		private void RegisterControllerConnectionEvents()
		{
			ReInput.ControllerConnectedEvent += this.OnControllerConnected;
			ReInput.ControllerPreDisconnectEvent += this.OnControllerPreDisconnected;
			ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.OnControllerChanged));
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0005B19C File Offset: 0x0005939C
		private void DeregisterControllerConnectionEvents()
		{
			if (ReInput.isReady)
			{
				ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
				ReInput.ControllerPreDisconnectEvent -= this.OnControllerPreDisconnected;
				ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.OnControllerChanged));
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0005B1E8 File Offset: 0x000593E8
		private void RegisterEngagementEvents()
		{
			if (Services.EngagementService != null)
			{
				Services.EngagementService.OnPrimaryUserEngagedEvent += this.OnPrimaryUserEngaged;
			}
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0005B207 File Offset: 0x00059407
		private void DeregisterEngagementEvents()
		{
			if (Services.EngagementService != null)
			{
				Services.EngagementService.OnPrimaryUserEngagedEvent -= this.OnPrimaryUserEngaged;
			}
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0005B226 File Offset: 0x00059426
		private void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (!RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return;
			}
			this.AssignControllerToSystemPlayer(args.controller);
			this.OnControllerChanged(args.controller);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0005B248 File Offset: 0x00059448
		private static bool AreMultipleControllersSupported()
		{
			return true;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0005B24C File Offset: 0x0005944C
		private void OnControllerPreDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!RewiredInputUserService<TDeviceId, TDeviceIdProvider>.IsStrictWithEngagement())
			{
				return;
			}
			TDeviceId deviceId = this._deviceIdProvider.GetDeviceId(args.controller);
			if (this.IsDeviceEngaged(deviceId))
			{
				if (RewiredInputUserService<TDeviceId, TDeviceIdProvider>.AreMultipleControllersSupported())
				{
					this.RemoveAllDevicesFromUser(0);
					this._user.engagedController = null;
					return;
				}
				this.RemoveDeviceFromUser(deviceId, 0);
			}
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0005B2AF File Offset: 0x000594AF
		private void OnPrimaryUserEngaged()
		{
			if (RewiredInputUserService<TDeviceId, TDeviceIdProvider>.AreMultipleControllersSupported())
			{
				this.AssignAllControllersToPlayer(this.RewiredPlayer);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0005B2C4 File Offset: 0x000594C4
		private Player RewiredPlayer
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x0005B2D1 File Offset: 0x000594D1
		private Player RewiredSystemPlayer
		{
			get
			{
				return ReInput.players.SystemPlayer;
			}
		}

		// Token: 0x04000EC9 RID: 3785
		private TDeviceIdProvider _deviceIdProvider;

		// Token: 0x04000ECA RID: 3786
		private IUserService _userService;

		// Token: 0x04000ECB RID: 3787
		private RewiredInputUserService<TDeviceId, TDeviceIdProvider>.User _user;

		// Token: 0x04000ECC RID: 3788
		private bool _hasInitialisedInitialPlayer;

		// Token: 0x020003C6 RID: 966
		private struct User
		{
			// Token: 0x040014E8 RID: 5352
			public bool isActive;

			// Token: 0x040014E9 RID: 5353
			public Controller engagedController;
		}
	}
}
