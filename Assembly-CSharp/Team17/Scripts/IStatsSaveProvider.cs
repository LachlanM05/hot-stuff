using System;

namespace Team17.Scripts
{
	// Token: 0x020001F2 RID: 498
	public interface IStatsSaveProvider
	{
		// Token: 0x06001090 RID: 4240
		StatsData CreateStatsData();

		// Token: 0x06001091 RID: 4241
		StatsData GetOrCreateStatsData();

		// Token: 0x06001092 RID: 4242
		bool SaveStats(StatsData statsData);
	}
}
