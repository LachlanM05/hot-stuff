using System;

namespace Team17.Scripts
{
	// Token: 0x020001EA RID: 490
	public abstract class BaseStatsProgressiveAchievement
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06001068 RID: 4200
		// (set) Token: 0x06001069 RID: 4201
		public abstract int CurrentValue { get; protected set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600106A RID: 4202
		public abstract int TargetValue { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600106B RID: 4203
		public abstract AchievementId AchievementId { get; }

		// Token: 0x0600106C RID: 4204
		public abstract bool RecalculateAchievementProgression(IReadOnlyStatsData statsData);

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x00055BAC File Offset: 0x00053DAC
		public bool CompletedAchievement
		{
			get
			{
				return this.CurrentValue >= this.TargetValue;
			}
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00055BBF File Offset: 0x00053DBF
		public void Reset()
		{
			this.CurrentValue = 0;
		}
	}
}
