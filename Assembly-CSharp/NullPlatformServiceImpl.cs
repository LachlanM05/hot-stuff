using System;
using Team17.Scripts.Platforms.Enums;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class NullPlatformServiceImpl : IPlatformServiceImpl
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000D5D RID: 3421 RVA: 0x0004C823 File Offset: 0x0004AA23
	public IRateLimiter SaveOperationRateLimiter
	{
		get
		{
			return RateLimiter.Unlimited;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000D5E RID: 3422 RVA: 0x0004C82A File Offset: 0x0004AA2A
	public EPlatformFlags PlatformFlags
	{
		get
		{
			return EPlatformFlags.None;
		}
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0004C82D File Offset: 0x0004AA2D
	public GraphicsQualityLevel GetCurrentGraphicsQualityLevel()
	{
		return (GraphicsQualityLevel)QualitySettings.GetQualityLevel();
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x0004C834 File Offset: 0x0004AA34
	public NullPlatformServiceImpl()
	{
		this.m_IsInitialised = true;
	}

	// Token: 0x06000D61 RID: 3425 RVA: 0x0004C843 File Offset: 0x0004AA43
	bool IPlatformServiceImpl.IsInitialised()
	{
		return this.m_IsInitialised;
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x0004C84B File Offset: 0x0004AA4B
	void IPlatformServiceImpl.OnStartUp()
	{
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x0004C84D File Offset: 0x0004AA4D
	void IPlatformServiceImpl.OnUpdate(float deltaTime)
	{
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x0004C84F File Offset: 0x0004AA4F
	void IPlatformServiceImpl.OnCleanUp()
	{
	}

	// Token: 0x04000C2D RID: 3117
	private bool m_IsInitialised;
}
