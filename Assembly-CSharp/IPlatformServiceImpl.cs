using System;
using Team17.Scripts.Platforms.Enums;

// Token: 0x02000181 RID: 385
public interface IPlatformServiceImpl
{
	// Token: 0x06000D77 RID: 3447
	bool IsInitialised();

	// Token: 0x06000D78 RID: 3448
	void OnStartUp();

	// Token: 0x06000D79 RID: 3449
	void OnCleanUp();

	// Token: 0x06000D7A RID: 3450
	void OnUpdate(float deltaTime);

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000D7B RID: 3451
	IRateLimiter SaveOperationRateLimiter { get; }

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000D7C RID: 3452
	EPlatformFlags PlatformFlags { get; }

	// Token: 0x06000D7D RID: 3453
	GraphicsQualityLevel GetCurrentGraphicsQualityLevel();
}
