using System;
using System.Linq;

namespace Team17.Scripts
{
	// Token: 0x020001E8 RID: 488
	public static class AchievementConstants
	{
		// Token: 0x06001061 RID: 4193 RVA: 0x00055AEA File Offset: 0x00053CEA
		public static bool IsBaseGameCollectable(string statKeyName)
		{
			return !AchievementConstants.DeluxeEditionCollectables.Contains(statKeyName);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00055AFC File Offset: 0x00053CFC
		public static string ConvertToStatKey(string internalName, int collectableNumber)
		{
			return string.Format("{0};{1}", internalName, collectableNumber);
		}

		// Token: 0x04000DEE RID: 3566
		public const int TotalBaseGameCollectables = 404;

		// Token: 0x04000DEF RID: 3567
		public const int ItllBeWorthSomethingRequiredBaseCollectables = 200;

		// Token: 0x04000DF0 RID: 3568
		public const int LoreThanYouBargainingForRequiredBaseCollectables = 50;

		// Token: 0x04000DF1 RID: 3569
		private static readonly string[] DeluxeEditionCollectables = new string[]
		{
			AchievementConstants.ConvertToStatKey("mikey", 1),
			AchievementConstants.ConvertToStatKey("mikey", 2),
			AchievementConstants.ConvertToStatKey("mikey", 3),
			AchievementConstants.ConvertToStatKey("mikey", 4),
			AchievementConstants.ConvertToStatKey("skylar", 4)
		};
	}
}
