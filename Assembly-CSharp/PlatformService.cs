using System;
using Team17.Common;
using Team17.Scripts.Platforms.Enums;

// Token: 0x0200017F RID: 383
public class PlatformService<TPlatformServiceImpl> : IPlatformService, IService where TPlatformServiceImpl : IPlatformServiceImpl, new()
{
	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000D65 RID: 3429 RVA: 0x0004C851 File Offset: 0x0004AA51
	public IRateLimiter SaveOperationRateLimiter
	{
		get
		{
			return this.m_Impl.SaveOperationRateLimiter;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000D66 RID: 3430 RVA: 0x0004C864 File Offset: 0x0004AA64
	public EPlatformFlags PlatformFlags
	{
		get
		{
			return this.m_Impl.PlatformFlags;
		}
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x0004C877 File Offset: 0x0004AA77
	public bool HasFlag(EPlatformFlags flag)
	{
		return this.PlatformFlags.HasFlag(flag);
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x0004C88F File Offset: 0x0004AA8F
	public bool IsInitialised()
	{
		return this.m_Impl.IsInitialised();
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0004C8A4 File Offset: 0x0004AAA4
	public bool ShouldSkipBootFlow()
	{
		ISkipBootFlowImpl skipBootFlowImpl = this.m_Impl as ISkipBootFlowImpl;
		return skipBootFlowImpl != null && skipBootFlowImpl.ShouldSkipBootFlow();
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x0004C8CD File Offset: 0x0004AACD
	public PlatformService()
	{
		this.m_Impl = new TPlatformServiceImpl();
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x0004C8E0 File Offset: 0x0004AAE0
	void IService.OnStartUp()
	{
		this.m_Impl.OnStartUp();
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x0004C8F3 File Offset: 0x0004AAF3
	void IService.OnUpdate(float deltaTime)
	{
		this.m_Impl.OnUpdate(deltaTime);
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x0004C907 File Offset: 0x0004AB07
	void IService.OnCleanUp()
	{
		this.m_Impl.OnCleanUp();
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x0004C91C File Offset: 0x0004AB1C
	public void EnableBurstMode(bool enable)
	{
		IBurstModeImpl burstModeImpl = this.m_Impl as IBurstModeImpl;
		if (burstModeImpl != null)
		{
			burstModeImpl.EnableBurstMode(enable);
		}
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x0004C944 File Offset: 0x0004AB44
	public GraphicsQualityLevel GetCurrentGraphicsQualityLevel()
	{
		return this.m_Impl.GetCurrentGraphicsQualityLevel();
	}

	// Token: 0x04000C2E RID: 3118
	private TPlatformServiceImpl m_Impl;
}
