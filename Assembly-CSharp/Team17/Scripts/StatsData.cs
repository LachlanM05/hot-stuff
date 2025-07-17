using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Team17.Scripts
{
	// Token: 0x020001F1 RID: 497
	[JsonObject(MemberSerialization.OptIn)]
	public class StatsData : IReadOnlyStatsData
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x00055E3F File Offset: 0x0005403F
		public int TotalUnlockedCollectables
		{
			get
			{
				return this.collectablesUnlocked.Count;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00055E4C File Offset: 0x0005404C
		public int TotalBaseGameUnlockedCollectables
		{
			get
			{
				return this.collectablesUnlocked.Where(new Func<string, bool>(AchievementConstants.IsBaseGameCollectable)).Count<string>();
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00055E6A File Offset: 0x0005406A
		public bool OnUnlockedCollectable(string collectableKeyName)
		{
			if (this.IsCollectableUnlocked(collectableKeyName))
			{
				return false;
			}
			this.collectablesUnlocked.Add(collectableKeyName);
			return true;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00055E84 File Offset: 0x00054084
		public bool IsCollectableUnlocked(string collectableKeyName)
		{
			return this.collectablesUnlocked.Contains(collectableKeyName);
		}

		// Token: 0x04000DF8 RID: 3576
		[JsonProperty]
		private List<string> collectablesUnlocked = new List<string>();
	}
}
