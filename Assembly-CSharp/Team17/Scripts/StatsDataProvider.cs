using System;
using Newtonsoft.Json;
using T17.Services;
using Team17.Common;

namespace Team17.Scripts
{
	// Token: 0x020001F3 RID: 499
	public class StatsDataProvider : IStatsSaveProvider
	{
		// Token: 0x06001093 RID: 4243 RVA: 0x00055EA5 File Offset: 0x000540A5
		public StatsData CreateStatsData()
		{
			return new StatsData();
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00055EAC File Offset: 0x000540AC
		public StatsData GetOrCreateStatsData()
		{
			string @string = Services.GameSettings.GetString("global_game_stats");
			if (string.IsNullOrEmpty(@string))
			{
				return this.CreateStatsData();
			}
			return JsonConvert.DeserializeObject<StatsData>(@string);
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00055EE0 File Offset: 0x000540E0
		public bool SaveStats(StatsData statsData)
		{
			bool flag;
			try
			{
				string text = JsonConvert.SerializeObject(statsData);
				Services.GameSettings.SetString("global_game_stats", text);
				Services.GameSettings.Save(null, false);
				flag = true;
			}
			catch (JsonException ex)
			{
				T17Debug.LogError("[STATS] Failed to serialize data - " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x04000DF9 RID: 3577
		private const string StatsKey = "global_game_stats";
	}
}
