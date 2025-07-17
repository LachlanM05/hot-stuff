using System;
using System.Collections.Generic;
using T17.Services;
using Team17.Common;
using Team17.Platform.Achievements;
using Team17.Platform.User;

namespace Team17.Scripts
{
	// Token: 0x020001EF RID: 495
	public class MirandaStatsService : IStatsService, IService
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x00055C06 File Offset: 0x00053E06
		public IReadOnlyStatsData StatsData
		{
			get
			{
				return this._statsData;
			}
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00055C0E File Offset: 0x00053E0E
		public void OnStartUp()
		{
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00055C10 File Offset: 0x00053E10
		public void OnCleanUp()
		{
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00055C12 File Offset: 0x00053E12
		public void OnUpdate(float deltaTime)
		{
			this.SaveStatsIfDirty();
			if (Services.UserService.IsPrimaryUserEngaged && this._queueUpdateAchievements)
			{
				this.TryUpdateAchievements();
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00055C34 File Offset: 0x00053E34
		public void OnUnlockedCollectable(string internalName, int collectableNumber)
		{
			string text = AchievementConstants.ConvertToStatKey(internalName, collectableNumber);
			if (this._statsData.OnUnlockedCollectable(text))
			{
				this.SetStatsDirty();
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00055C5D File Offset: 0x00053E5D
		public void LoadAndProcessStatsData()
		{
			this._statsData = this._statsSaveProvider.GetOrCreateStatsData();
			this.ProcessStatsData();
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00055C76 File Offset: 0x00053E76
		public void CreateNewStatsData()
		{
			this._statsData = this._statsSaveProvider.CreateStatsData();
			this.ProcessStatsData();
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00055C90 File Offset: 0x00053E90
		private void ProcessStatsData()
		{
			foreach (BaseStatsProgressiveAchievement baseStatsProgressiveAchievement in this._statAchievements)
			{
				baseStatsProgressiveAchievement.Reset();
			}
			this.TryUpdateAchievements();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00055CE8 File Offset: 0x00053EE8
		private void SaveStatsIfDirty()
		{
			if (!this._statsDirty)
			{
				return;
			}
			bool flag = this._statsSaveProvider.SaveStats(this._statsData);
			this._statsDirty = !flag;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00055D1A File Offset: 0x00053F1A
		private void SetStatsDirty()
		{
			this._statsDirty = true;
			this.TryUpdateAchievements();
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00055D2C File Offset: 0x00053F2C
		private void TryUpdateAchievements()
		{
			IUser user;
			if (!Services.UserService.TryGetPrimaryUser(out user))
			{
				T17Debug.LogError("[ACHIEVEMENTS] Failed to check achievements because primary user could not be found.");
				return;
			}
			AchievementController instance = Singleton<AchievementController>.Instance;
			IAchievementsService platformAchievementsService = Services.PlatformAchievementsService;
			if (instance == null || platformAchievementsService == null)
			{
				this._queueUpdateAchievements = true;
				return;
			}
			this._queueUpdateAchievements = false;
			foreach (BaseStatsProgressiveAchievement baseStatsProgressiveAchievement in this._statAchievements)
			{
				if (baseStatsProgressiveAchievement.RecalculateAchievementProgression(this._statsData))
				{
					Achievement achievement = instance.GetAchievement(baseStatsProgressiveAchievement.AchievementId);
					platformAchievementsService.SetProgress(user, achievement, baseStatsProgressiveAchievement.CurrentValue, baseStatsProgressiveAchievement.TargetValue);
					if (baseStatsProgressiveAchievement.CompletedAchievement)
					{
						platformAchievementsService.Unlock(user, achievement);
					}
				}
			}
		}

		// Token: 0x04000DF3 RID: 3571
		private readonly IStatsSaveProvider _statsSaveProvider = new StatsDataProvider();

		// Token: 0x04000DF4 RID: 3572
		private bool _statsDirty;

		// Token: 0x04000DF5 RID: 3573
		private bool _queueUpdateAchievements;

		// Token: 0x04000DF6 RID: 3574
		private StatsData _statsData;

		// Token: 0x04000DF7 RID: 3575
		private readonly List<BaseStatsProgressiveAchievement> _statAchievements = new List<BaseStatsProgressiveAchievement>
		{
			new LoreThanYouBargainingForStatsProgressiveAchievement(),
			new ItllBeWorthSomethingSomedayStatsProgressiveAchievement(),
			new CollectionCompleteStatsProgressiveAchievement()
		};
	}
}
