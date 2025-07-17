using System;

namespace Team17.Scripts
{
	// Token: 0x020001EB RID: 491
	public class CollectionCompleteStatsProgressiveAchievement : BaseCollectableStatsProgressiveAchievement
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x00055BD0 File Offset: 0x00053DD0
		public override AchievementId AchievementId
		{
			get
			{
				return AchievementId.COLLECTION_COMPLETE;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x00055BD4 File Offset: 0x00053DD4
		public override int TargetValue
		{
			get
			{
				return 404;
			}
		}
	}
}
