using System;

namespace Team17.Scripts
{
	// Token: 0x020001EC RID: 492
	public class ItllBeWorthSomethingSomedayStatsProgressiveAchievement : BaseCollectableStatsProgressiveAchievement
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x00055BE3 File Offset: 0x00053DE3
		public override AchievementId AchievementId
		{
			get
			{
				return AchievementId.IT_LL_BE_WORTH_SOMETHING_SOMEDAY;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x00055BE7 File Offset: 0x00053DE7
		public override int TargetValue
		{
			get
			{
				return 200;
			}
		}
	}
}
