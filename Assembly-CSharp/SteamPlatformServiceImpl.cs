using System;
using Steamworks;
using Team17.Scripts.Platforms.Enums;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class SteamPlatformServiceImpl : IPlatformServiceImpl
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0004CB52 File Offset: 0x0004AD52
	public IRateLimiter SaveOperationRateLimiter
	{
		get
		{
			return RateLimiter.Unlimited;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0004CB59 File Offset: 0x0004AD59
	// (set) Token: 0x06000D8A RID: 3466 RVA: 0x0004CB61 File Offset: 0x0004AD61
	public EPlatformFlags PlatformFlags { get; private set; }

	// Token: 0x06000D8B RID: 3467 RVA: 0x0004CB6A File Offset: 0x0004AD6A
	public GraphicsQualityLevel GetCurrentGraphicsQualityLevel()
	{
		return (GraphicsQualityLevel)QualitySettings.GetQualityLevel();
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0004CB79 File Offset: 0x0004AD79
	bool IPlatformServiceImpl.IsInitialised()
	{
		return SteamManager.Initialized;
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0004CB80 File Offset: 0x0004AD80
	void IPlatformServiceImpl.OnStartUp()
	{
		if (SteamUtils.IsSteamInBigPictureMode())
		{
			this.PlatformFlags |= EPlatformFlags.ForceFullScreen;
			Screen.fullScreen = true;
		}
		if (SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.PlatformFlags |= EPlatformFlags.IsSmallScreen;
			this.PlatformFlags |= EPlatformFlags.ApplyGraphicalOptimisations;
			return;
		}
		this.PlatformFlags |= EPlatformFlags.AllowResolutionChanges;
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0004CBDB File Offset: 0x0004ADDB
	void IPlatformServiceImpl.OnUpdate(float deltaTime)
	{
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x0004CBDD File Offset: 0x0004ADDD
	void IPlatformServiceImpl.OnCleanUp()
	{
	}
}
