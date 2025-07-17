using System;

namespace Team17.Scripts
{
	// Token: 0x020001ED RID: 493
	public class LoreThanYouBargainingForStatsProgressiveAchievement : BaseCollectableStatsProgressiveAchievement
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x00055BF6 File Offset: 0x00053DF6
		public override AchievementId AchievementId
		{
			get
			{
				return AchievementId.LORE_THAN_YOU_BARGAINING_FOR;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x00055BFA File Offset: 0x00053DFA
		public override int TargetValue
		{
			get
			{
				return 50;
			}
		}
	}
}
