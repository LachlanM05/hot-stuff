using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x02000182 RID: 386
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0004C957 File Offset: 0x0004AB57
	public static AppId_t ApplicationId
	{
		get
		{
			return new AppId_t(2201320U);
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0004C963 File Offset: 0x0004AB63
	private static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000D80 RID: 3456 RVA: 0x0004C987 File Offset: 0x0004AB87
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x0004C993 File Offset: 0x0004AB93
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x0004C99C File Offset: 0x0004AB9C
	public static void EnsureRunningThroughSteam()
	{
		try
		{
			if (SteamAPI.RestartAppIfNecessary(SteamManager.ApplicationId))
			{
				Debug.LogError("[SteamManager] Game is not running through steam. Relaunching through Steam");
				Application.Quit();
			}
		}
		catch (DllNotFoundException ex)
		{
			string text = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null));
			Application.Quit();
		}
		catch (Exception ex3)
		{
			string text2 = "[Steamworks.NET] Exception thrown in RestartAppIfNecessary : ";
			Exception ex4 = ex3;
			Debug.LogError(text2 + ((ex4 != null) ? ex4.ToString() : null));
			Application.Quit();
		}
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x0004CA2C File Offset: 0x0004AC2C
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		SteamManager.EnsureRunningThroughSteam();
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			Application.Quit();
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x0004CAC8 File Offset: 0x0004ACC8
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x0004CB16 File Offset: 0x0004AD16
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0004CB3A File Offset: 0x0004AD3A
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04000C2F RID: 3119
	private static SteamManager s_instance;

	// Token: 0x04000C30 RID: 3120
	private static bool s_EverInitialized;

	// Token: 0x04000C31 RID: 3121
	private bool m_bInitialized;

	// Token: 0x04000C32 RID: 3122
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
