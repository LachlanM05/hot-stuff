using System;

namespace Team17.Scripts
{
	// Token: 0x020001E9 RID: 489
	public abstract class BaseCollectableStatsProgressiveAchievement : BaseStatsProgressiveAchievement
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x00055B6E File Offset: 0x00053D6E
		// (set) Token: 0x06001065 RID: 4197 RVA: 0x00055B76 File Offset: 0x00053D76
		public override int CurrentValue { get; protected set; }

		// Token: 0x06001066 RID: 4198 RVA: 0x00055B80 File Offset: 0x00053D80
		public override bool RecalculateAchievementProgression(IReadOnlyStatsData statsData)
		{
			int totalBaseGameUnlockedCollectables = statsData.TotalBaseGameUnlockedCollectables;
			bool flag = totalBaseGameUnlockedCollectables > this.CurrentValue;
			this.CurrentValue = totalBaseGameUnlockedCollectables;
			return flag;
		}
	}
}
