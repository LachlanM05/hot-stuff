using System;

namespace Team17.Scripts
{
	// Token: 0x020001F0 RID: 496
	public interface IReadOnlyStatsData
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06001088 RID: 4232
		int TotalUnlockedCollectables { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06001089 RID: 4233
		int TotalBaseGameUnlockedCollectables { get; }

		// Token: 0x0600108A RID: 4234
		bool IsCollectableUnlocked(string internalName);
	}
}
