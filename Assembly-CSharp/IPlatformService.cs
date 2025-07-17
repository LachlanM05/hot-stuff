using System;
using Team17.Common;
using Team17.Scripts.Platforms.Enums;

// Token: 0x02000180 RID: 384
public interface IPlatformService : IService
{
	// Token: 0x06000D70 RID: 3440
	bool IsInitialised();

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000D71 RID: 3441
	IRateLimiter SaveOperationRateLimiter { get; }

	// Token: 0x06000D72 RID: 3442
	bool ShouldSkipBootFlow();

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000D73 RID: 3443
	EPlatformFlags PlatformFlags { get; }

	// Token: 0x06000D74 RID: 3444
	bool HasFlag(EPlatformFlags flags);

	// Token: 0x06000D75 RID: 3445
	void EnableBurstMode(bool enable);

	// Token: 0x06000D76 RID: 3446
	GraphicsQualityLevel GetCurrentGraphicsQualityLevel();
}
