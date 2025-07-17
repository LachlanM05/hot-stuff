using System;
using System.Collections.Generic;
using Team17.Common;
using UnityEngine;

namespace Team17.Services.Activities
{
	// Token: 0x020001CB RID: 459
	public class ActivitiesService : MonoBehaviour, IService
	{
		// Token: 0x06000F35 RID: 3893 RVA: 0x00052F4B File Offset: 0x0005114B
		public void OnValidate()
		{
			this.SortActivityTable();
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00052F53 File Offset: 0x00051153
		public void OnCleanUp()
		{
			if (this._impl != null)
			{
				this._impl.ShutDown();
				this._impl = null;
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00052F6F File Offset: 0x0005116F
		private void SortActivityTable()
		{
			this._activityDefinitions.Sort();
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00052F7C File Offset: 0x0005117C
		public void OnStartUp()
		{
			if (this._impl == null)
			{
				this.CreateActivityServiceImpl();
			}
			if (this._impl != null && this._impl.IsPlatformSupported())
			{
				this._impl.StartUp();
				this._impl.RegisterActivitySequencedEvent(new ActivityRequestedEvent(this.OnActivityActivationRequested));
			}
			this.ResetToDefault();
			this._activityId = string.Empty;
			this._currentlyActive = null;
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00052FE8 File Offset: 0x000511E8
		public void OnUpdate(float deltaTime)
		{
			if (this._impl != null && this._impl.IsPlatformSupported())
			{
				this._impl.Update(deltaTime);
				if (Singleton<Save>.Instance != null && Save.GetSaveData(true) != null)
				{
					Save instance = Singleton<Save>.Instance;
					this.UpdateActivities(instance.GetNewGamePlus(), instance.AvailableTotalMetDatables(), instance.AvailableTotalRealizedDatables());
				}
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00053049 File Offset: 0x00051249
		public string GetCurrentActivityDisplayString()
		{
			if (this._currentlyActive != null)
			{
				return this._currentlyActive.displayText;
			}
			return string.Empty;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0005306A File Offset: 0x0005126A
		public string GetPendingRequestedActivityId()
		{
			return this._requestedActivity;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00053072 File Offset: 0x00051272
		public void ClearPendingActivity()
		{
			this._requestedActivity = null;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0005307B File Offset: 0x0005127B
		public bool IsCurrentActivity(string activityId)
		{
			return !string.IsNullOrEmpty(activityId) && this._activityId != null && activityId.Equals(this._activityId);
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0005309C File Offset: 0x0005129C
		public void StartTask(ActivityDefinition activityDef)
		{
			List<ActivityDefinition> list = new List<ActivityDefinition>();
			List<string> list2 = new List<string>();
			activityDef != null;
			if (activityDef != this._currentlyActive)
			{
				if (this._impl != null && this._impl.IsPlatformRelevant(activityDef))
				{
					this._impl.ActivityTerminate();
					if (activityDef != null)
					{
						foreach (ActivityDefinition activityDefinition in this._activityDefinitions)
						{
							if (activityDefinition.activityId == activityDef.activityId)
							{
								list.Add(activityDefinition);
								if (activityDefinition.requiredAmount < activityDef.requiredAmount)
								{
									list2.Add(activityDefinition.taskId);
								}
							}
						}
						if (list.Count > 0)
						{
							this._impl.ChangeActivityAvailability(activityDef.activityId, list.ToArray(), null, new ActivityResult(this.OnActivityAvailabilityChangeComplete));
							if (list2.Count > 0)
							{
								this._impl.ActivityResume(activityDef.activityId, new string[] { activityDef.taskId }, list2.ToArray(), new ActivityResult(this.OnActivityStartComplete));
							}
							else
							{
								this._impl.ActivityStart(activityDef, new ActivityResult(this.OnActivityStartComplete));
							}
						}
					}
				}
				if (this._requestedActivity != null && activityDef != null && this._requestedActivity == activityDef.activityId)
				{
					this.ClearPendingActivity();
				}
				if (activityDef == null)
				{
					this._activityId = null;
				}
				else
				{
					this._activityId = activityDef.activityId;
				}
				this._currentlyActive = activityDef;
				return;
			}
			if (activityDef != null)
			{
				foreach (ActivityDefinition activityDefinition2 in this._activityDefinitions)
				{
					if (activityDefinition2.activityId == activityDef.activityId)
					{
						list.Add(activityDefinition2);
						if (activityDefinition2.requiredAmount < activityDef.requiredAmount)
						{
							list2.Add(activityDefinition2.taskId);
						}
					}
				}
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x000532C4 File Offset: 0x000514C4
		public void ResetToDefault()
		{
			if (this._impl.IsPlatformSupported())
			{
				this._impl.ActivityTerminate();
			}
			this._currentlyActive = null;
			this.LastSavedAwakenings = -1;
			this.LastSavedRealizes = -1;
			this.inNGPlus = false;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x000532FC File Offset: 0x000514FC
		private void OnActivityEndComplete(ActivityDefinition activityDef, bool succeeded)
		{
			if (activityDef != null && !succeeded)
			{
				T17Debug.LogError(string.Concat(new string[] { "ActivityService: Activity '", activityDef.activityId, ":", activityDef.taskId, "' failed to end" }));
			}
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00053350 File Offset: 0x00051550
		private void OnActivityStartComplete(ActivityDefinition activityDef, bool succeeded)
		{
			if (activityDef != null && !succeeded)
			{
				T17Debug.LogError(string.Concat(new string[] { "ActivityService: Activity '", activityDef.activityId, ":", activityDef.taskId, "' failed to start" }));
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000533A4 File Offset: 0x000515A4
		private void OnActivityAvailabilityChangeComplete(ActivityDefinition activityDef, bool succeeded)
		{
			if (activityDef != null)
			{
				if (!succeeded)
				{
					T17Debug.LogError(string.Concat(new string[] { "[ActivitiesService] Activity '", activityDef.activityId, ":", activityDef.taskId, "' availability change failed in the platform subsystem" }));
					return;
				}
			}
			else if (!succeeded)
			{
				T17Debug.LogError("[ActivitiesService] Activity (No Def) availability change failed in the platform subsystem");
			}
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00053405 File Offset: 0x00051605
		private void CreateActivityServiceImpl()
		{
			this._impl = new NullActivitiesImpl();
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00053412 File Offset: 0x00051612
		private void OnActivityActivationRequested(string activityId)
		{
			this._requestedActivity = activityId;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0005341C File Offset: 0x0005161C
		public void UpdateActivities(bool newGamePlus, int totalAwakenings, int totalRealizes)
		{
			if (totalAwakenings == this.LastSavedAwakenings && totalRealizes == this.LastSavedRealizes && newGamePlus == this.inNGPlus)
			{
				return;
			}
			this.LastSavedAwakenings = totalAwakenings;
			this.LastSavedRealizes = totalRealizes;
			this.inNGPlus = newGamePlus;
			ActivityDefinition activityDefinition = null;
			foreach (ActivityDefinition activityDefinition2 in this._activityDefinitions)
			{
				bool flag = false;
				switch (activityDefinition2.activityType)
				{
				case ActivityDefinition.ContinueType.neutral:
					if (totalAwakenings > 50 && totalRealizes > 50 && !newGamePlus)
					{
						activityDefinition = activityDefinition2;
						flag = true;
					}
					break;
				case ActivityDefinition.ContinueType.awakened:
					if (!newGamePlus)
					{
						if (totalAwakenings >= activityDefinition2.requiredAmount)
						{
							if (activityDefinition2.isFinalTask && this._currentlyActive == activityDefinition2)
							{
								this._impl.ActivityEnd(activityDefinition2.activityId, ActivityOutcome.completed, new ActivityResult(this.OnActivityEndComplete));
							}
						}
						else
						{
							activityDefinition = activityDefinition2;
							flag = true;
						}
					}
					break;
				case ActivityDefinition.ContinueType.realized:
					if (!newGamePlus && totalAwakenings >= 50)
					{
						if (totalRealizes >= activityDefinition2.requiredAmount)
						{
							if (activityDefinition2.isFinalTask && this._currentlyActive == activityDefinition2)
							{
								this._impl.ActivityEnd(activityDefinition2.activityId, ActivityOutcome.completed, new ActivityResult(this.OnActivityEndComplete));
							}
						}
						else
						{
							activityDefinition = activityDefinition2;
							flag = true;
						}
					}
					break;
				case ActivityDefinition.ContinueType.collectables:
					if (newGamePlus)
					{
						activityDefinition = activityDefinition2;
						flag = true;
					}
					break;
				}
				if (flag)
				{
					break;
				}
			}
			if (activityDefinition != null && this._currentlyActive != activityDefinition)
			{
				this.StartTask(activityDefinition);
			}
		}

		// Token: 0x04000D95 RID: 3477
		public static int InvalidIndex = -1;

		// Token: 0x04000D96 RID: 3478
		private IActivitesServiceImpl _impl;

		// Token: 0x04000D97 RID: 3479
		[SerializeField]
		private List<ActivityDefinition> _activityDefinitions = new List<ActivityDefinition>();

		// Token: 0x04000D98 RID: 3480
		private string _activityId;

		// Token: 0x04000D99 RID: 3481
		private ActivityDefinition _currentlyActive;

		// Token: 0x04000D9A RID: 3482
		private int LastSavedAwakenings = -1;

		// Token: 0x04000D9B RID: 3483
		private int LastSavedRealizes = -1;

		// Token: 0x04000D9C RID: 3484
		private bool inNGPlus;

		// Token: 0x04000D9D RID: 3485
		private string _requestedActivity;
	}
}
