using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using T17.Input;
using Team17.Common;
using Team17.Input;
using Team17.Input.Unity;
using Team17.Performance;
using Team17.Platform.Achievements;
using Team17.Platform.Achievements.Steam;
using Team17.Platform.EngagementService;
using Team17.Platform.OnScreenKeyboard;
using Team17.Platform.OnScreenKeyboard.Steam;
using Team17.Platform.SaveGame.Steam;
using Team17.Platform.User.Steam;
using Team17.Platform.UserService;
using Team17.Platform.UserService.Steam;
using Team17.Scripts;
using Team17.Scripts.Input;
using Team17.Scripts.Input.PlatformButtonLayout;
using Team17.Scripts.Input.Rumble;
using Team17.Scripts.Services;
using Team17.Scripts.Services.ApplicationStatus;
using Team17.Scripts.Services.AssetProvider;
using Team17.Scripts.Services.Entitlement;
using Team17.Scripts.Services.Input;
using Team17.Scripts.Services.Suspend;
using Team17.Scripts.UI_Components;
using Team17.Services;
using Team17.Services.Activities;
using Team17.Services.Save;
using UnityEngine;

namespace T17.Services
{
	// Token: 0x02000245 RID: 581
	public class Services : MonoBehaviour
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x0005B2DD File Offset: 0x000594DD
		public static IUserService UserService
		{
			get
			{
				return Services.m_UserService;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x0005B2E4 File Offset: 0x000594E4
		public static IInputUserService InputUserService
		{
			get
			{
				return Services.m_InputUserService;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x0005B2EB File Offset: 0x000594EB
		public static IDeviceEngagementValidator DeviceEngagementValidator
		{
			get
			{
				return Services.m_DeviceEngagementValidator;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x0005B2F2 File Offset: 0x000594F2
		public static IAutoEngagingDeviceService AutoEngagingDeviceService
		{
			get
			{
				return Services.m_AutoEngagingDeviceService;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x0005B2F9 File Offset: 0x000594F9
		public static IMirandaInputService InputService
		{
			get
			{
				return Services.m_InputService;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x0005B300 File Offset: 0x00059500
		public static IEngagementService EngagementService
		{
			get
			{
				return Services.m_EngagementService;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0005B307 File Offset: 0x00059507
		public static IMirandaSaveGameService SaveGameService
		{
			get
			{
				return Services.m_SaveGameService;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0005B30E File Offset: 0x0005950E
		public static GameSettingsProvider GameSettings
		{
			get
			{
				return Services.m_GameSettingsProvider;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x0005B315 File Offset: 0x00059515
		public static GraphicsSettingsProvider GraphicsSettings
		{
			get
			{
				return Services.m_GraphicsSettingsProvider;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0005B31C File Offset: 0x0005951C
		public static IIconTextMarkupService IconTextMarkupService
		{
			get
			{
				return Services.m_IconTextMarkupService;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x0005B323 File Offset: 0x00059523
		public static IUIInputService UIInputService
		{
			get
			{
				return Services.m_UIInputService;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x0005B32A File Offset: 0x0005952A
		public static IPostEngagementService PostEngagementService
		{
			get
			{
				return Services.m_PostEngagementService;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x0005B331 File Offset: 0x00059531
		public static IControllerRumbleService ControllerRumbleService
		{
			get
			{
				return Services.m_ControllerRumbleService;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x0005B338 File Offset: 0x00059538
		public static IMirandaEntitlementService Entitlements
		{
			get
			{
				return Services.m_EntitlementsService;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0005B33F File Offset: 0x0005953F
		public static IOnScreenKeyboardService OnScreenKeyboard
		{
			get
			{
				return Services.m_OnScreenKeyboardService;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0005B346 File Offset: 0x00059546
		public static IApplicationStatusPlatformService ApplicationStatusPlatformService
		{
			get
			{
				return Services.m_ApplicationStatusPlatformService;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0005B34D File Offset: 0x0005954D
		public static IPerformanceService PerformanceService
		{
			get
			{
				return Services.m_PerformanceService;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0005B354 File Offset: 0x00059554
		public static IGameSuspendService GameSuspendService
		{
			get
			{
				return Services.m_GameSuspendService;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x0005B35B File Offset: 0x0005955B
		public static IAssetProviderService AssetProviderService
		{
			get
			{
				return Services.m_AssetProviderService;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0005B362 File Offset: 0x00059562
		public static IAchievementsService PlatformAchievementsService
		{
			get
			{
				return Services.m_PlatformAchievementService;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x0005B369 File Offset: 0x00059569
		public static IStatsService StatsService
		{
			get
			{
				return Services.m_StatsService;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0005B370 File Offset: 0x00059570
		public static ActivitiesService ActivitiesService
		{
			get
			{
				return Services.m_ActivitiesService;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x0005B377 File Offset: 0x00059577
		public static IPlatformService PlatformService
		{
			get
			{
				return Services.m_PlatformService;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0005B37E File Offset: 0x0005957E
		public static ResolutionProviderService ResolutionProviderService
		{
			get
			{
				return Services.m_ResolutionProviderService;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x0005B385 File Offset: 0x00059585
		public static IMaxSaveSlotsProvider MaxSaveSlotsProvider
		{
			get
			{
				return Services.m_MaxSaveSlotProvider;
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0005B38C File Offset: 0x0005958C
		private void Awake()
		{
			TaskScheduler.UnobservedTaskException += this.OnTaskSchedulerOnUnobservedTaskException;
			if (Services.s_Instance != null)
			{
				Object.Destroy(this);
				return;
			}
			Services.s_Instance = this;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0005B3B9 File Offset: 0x000595B9
		private void OnTaskSchedulerOnUnobservedTaskException(object _, UnobservedTaskExceptionEventArgs e)
		{
			Debug.LogException(e.Exception);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0005B3C8 File Offset: 0x000595C8
		private void Start()
		{
			Application.runInBackground = false;
			T17Debug.SetLogger(new MirandaT17Logger());
			Services.m_GameSettingsProvider = new GameSettingsProvider();
			Services.m_GraphicsSettingsProvider = new GraphicsSettingsProvider();
			Services.m_ApplicationStatusPlatformService = new ApplicationStatusPlatformService();
			Services.m_GameSuspendService = new GameSuspendService();
			Services.m_PlatformService = new PlatformService<SteamPlatformServiceImpl>();
			Services.m_UserService = new UserService<SteamUserServiceImpl, SteamUser, NullDeviceId>(4);
			Services.m_InputUserService = new RewiredInputUserService<NullDeviceId, RewiredDeviceIDProviderNull>(new RewiredDeviceIDProviderNull(), Services.m_UserService);
			Services.m_AutoEngagingDeviceService = new AutoEngagingDeviceService();
			Services.m_DeviceEngagementValidator = new NullDeviceEngagementValidator();
			Services.m_EngagementService = new EngagementService(Services.m_UserService, Services.m_InputUserService, Services.m_DeviceEngagementValidator);
			Services.m_SaveGameService = new MirandaSaveGameService<SteamSaveGameIO>();
			Services.m_IconTextMarkupService = base.GetComponentInChildren<IconTextMarkupService>();
			Services.m_UIInputService = new UIInputService();
			Services.m_InputService = new MirandaInputService();
			Services.m_PostEngagementService = new PostEngagementService();
			Services.m_ControllerRumbleService = new ControllerRumbleService(new ControllerRumbleFactory(8));
			Services.m_EntitlementsService = new MirandaEntitlementService(Services.m_EngagementService, Services.m_UserService, this.allProductDefinitions);
			Services.m_OnScreenKeyboardService = new OnScreenKeyboardService<SteamOnScreenKeyboardImpl>();
			Services.m_PerformanceService = this.CreatePerformanceService();
			Services.m_AssetProviderService = new AssetProviderService(this.m_ResourceToAddressableProvider);
			Services.m_PlatformAchievementService = new AchievementsService<SteamAchievementsImpl, SteamUser, ISteamAchievementID, ISteamProgressID>();
			Services.m_StatsService = new MirandaStatsService();
			Services.m_ResolutionProviderService = base.GetComponent<ResolutionProviderService>();
			Services.m_ActivitiesService = base.GetComponentInChildren<ActivitiesService>();
			this.m_Services.Add(Services.m_ApplicationStatusPlatformService);
			this.m_Services.Add(new UnityApplicationFocusListenerService());
			this.m_Services.Add(new EntitlementsRefreshOnResumeService());
			this.m_Services.Add(Services.m_GameSuspendService);
			this.m_Services.Add(Services.m_PlatformService);
			this.m_Services.Add(Services.m_UserService);
			this.m_Services.Add(Services.m_InputUserService);
			this.m_Services.Add(Services.m_AutoEngagingDeviceService);
			this.m_Services.Add(Services.m_EngagementService);
			this.m_Services.Add(Services.m_SaveGameService);
			this.m_Services.Add(Services.m_IconTextMarkupService);
			this.m_Services.Add(Services.m_UIInputService);
			this.m_Services.Add(Services.m_InputService);
			this.m_Services.Add(Services.m_PostEngagementService);
			this.m_Services.Add(Services.m_ControllerRumbleService);
			this.m_Services.Add(Services.m_EntitlementsService);
			this.m_Services.Add(Services.m_OnScreenKeyboardService);
			this.m_Services.Add(Services.m_PerformanceService);
			this.m_Services.Add(Services.m_AssetProviderService);
			this.m_Services.Add(Services.m_PlatformAchievementService);
			this.m_Services.Add(Services.m_StatsService);
			this.m_Services.Add(Services.m_ActivitiesService);
			this.m_Services.Add(Services.m_ResolutionProviderService);
			int i = 0;
			int count = this.m_Services.Count;
			while (i < count)
			{
				this.m_Services[i].OnStartUp();
				i++;
			}
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0005B6A8 File Offset: 0x000598A8
		private IPerformanceService CreatePerformanceService()
		{
			return new MockPerformanceService();
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0005B6B0 File Offset: 0x000598B0
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			int i = 0;
			int count = this.m_Services.Count;
			while (i < count)
			{
				this.m_Services[i].OnUpdate(deltaTime);
				i++;
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0005B6ED File Offset: 0x000598ED
		private void OnDisable()
		{
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0005B6F0 File Offset: 0x000598F0
		private void OnDestroy()
		{
			TaskScheduler.UnobservedTaskException -= this.OnTaskSchedulerOnUnobservedTaskException;
			int i = 0;
			int count = this.m_Services.Count;
			while (i < count)
			{
				this.m_Services[i].OnCleanUp();
				i++;
			}
			this.m_Services.Clear();
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0005B744 File Offset: 0x00059944
		private void OnApplicationFocus(bool hasFocus)
		{
			try
			{
				IControllerRumbleService controllerRumbleService = Services.m_ControllerRumbleService;
				if (controllerRumbleService != null)
				{
					controllerRumbleService.OnApplicationFocus(hasFocus);
				}
				if (hasFocus && Services.m_UserService.IsPrimaryUserEngaged)
				{
					SettingsMenu.ForceResolutionToSavedValue();
				}
			}
			catch (Exception ex)
			{
				T17Debug.LogError("Services:OnApplicationFocus(" + hasFocus.ToString() + ") ERROR: " + ex.Message);
			}
		}

		// Token: 0x04000ECD RID: 3789
		private const short m_MaxUserCount = 4;

		// Token: 0x04000ECE RID: 3790
		private const short m_MaxDeviceCountPerUser = 16;

		// Token: 0x04000ECF RID: 3791
		private static IPlatformService m_PlatformService;

		// Token: 0x04000ED0 RID: 3792
		private static IUserService m_UserService;

		// Token: 0x04000ED1 RID: 3793
		private static IInputUserService m_InputUserService;

		// Token: 0x04000ED2 RID: 3794
		private static IDeviceEngagementValidator m_DeviceEngagementValidator;

		// Token: 0x04000ED3 RID: 3795
		private static IAutoEngagingDeviceService m_AutoEngagingDeviceService;

		// Token: 0x04000ED4 RID: 3796
		private static IEngagementService m_EngagementService;

		// Token: 0x04000ED5 RID: 3797
		private static IMirandaSaveGameService m_SaveGameService;

		// Token: 0x04000ED6 RID: 3798
		private static GameSettingsProvider m_GameSettingsProvider;

		// Token: 0x04000ED7 RID: 3799
		private static GraphicsSettingsProvider m_GraphicsSettingsProvider;

		// Token: 0x04000ED8 RID: 3800
		private static IconTextMarkupService m_IconTextMarkupService;

		// Token: 0x04000ED9 RID: 3801
		private static IUIInputService m_UIInputService;

		// Token: 0x04000EDA RID: 3802
		private static IMirandaInputService m_InputService;

		// Token: 0x04000EDB RID: 3803
		private static IPostEngagementService m_PostEngagementService;

		// Token: 0x04000EDC RID: 3804
		private static IControllerRumbleService m_ControllerRumbleService;

		// Token: 0x04000EDD RID: 3805
		private static IMirandaEntitlementService m_EntitlementsService;

		// Token: 0x04000EDE RID: 3806
		private static IApplicationStatusPlatformService m_ApplicationStatusPlatformService;

		// Token: 0x04000EDF RID: 3807
		private static IGameSuspendService m_GameSuspendService;

		// Token: 0x04000EE0 RID: 3808
		private static IOnScreenKeyboardService m_OnScreenKeyboardService;

		// Token: 0x04000EE1 RID: 3809
		private static IAssetProviderService m_AssetProviderService;

		// Token: 0x04000EE2 RID: 3810
		private static IPerformanceService m_PerformanceService;

		// Token: 0x04000EE3 RID: 3811
		private static IAchievementsService m_PlatformAchievementService;

		// Token: 0x04000EE4 RID: 3812
		private static IStatsService m_StatsService;

		// Token: 0x04000EE5 RID: 3813
		private static ActivitiesService m_ActivitiesService;

		// Token: 0x04000EE6 RID: 3814
		private static ResolutionProviderService m_ResolutionProviderService;

		// Token: 0x04000EE7 RID: 3815
		private static readonly IMaxSaveSlotsProvider m_MaxSaveSlotProvider = new DefaultMaxSaveSlotsProvider();

		// Token: 0x04000EE8 RID: 3816
		private List<IService> m_Services = new List<IService>();

		// Token: 0x04000EE9 RID: 3817
		private static Services s_Instance = null;

		// Token: 0x04000EEA RID: 3818
		[SerializeField]
		private ResourceToAddressableOverrideProvider m_ResourceToAddressableProvider;

		// Token: 0x04000EEB RID: 3819
		[SerializeField]
		private ProductDefinition[] allProductDefinitions;
	}
}
