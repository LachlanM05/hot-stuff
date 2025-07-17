using System;

namespace Team17.Scripts.Platforms.Enums
{
	// Token: 0x02000235 RID: 565
	[Flags]
	public enum EPlatformFlags
	{
		// Token: 0x04000E7E RID: 3710
		None = 0,
		// Token: 0x04000E7F RID: 3711
		IsSmallScreen = 1,
		// Token: 0x04000E80 RID: 3712
		LimitedPlatformSaveSlots = 2,
		// Token: 0x04000E81 RID: 3713
		ApplyGraphicalOptimisations = 4,
		// Token: 0x04000E82 RID: 3714
		ForceFullScreen = 8,
		// Token: 0x04000E83 RID: 3715
		AllowResolutionChanges = 16,
		// Token: 0x04000E84 RID: 3716
		PlatformHandlesControllerDisconnects = 32,
		// Token: 0x04000E85 RID: 3717
		DoesntSupportFileTimestamps = 64
	}
}
