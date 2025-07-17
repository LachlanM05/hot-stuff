using System;
using UnityEngine;

// Token: 0x0200018E RID: 398
[CreateAssetMenu(fileName = "Assets/Team17/Data/Activities/NewActivity", menuName = "Activities/Create Activity Definition", order = 0)]
[Serializable]
public class ActivityDefinition : ScriptableObject, IComparable
{
	// Token: 0x06000DCA RID: 3530 RVA: 0x0004D524 File Offset: 0x0004B724
	public string SelectId()
	{
		if (!string.IsNullOrEmpty(this.taskId))
		{
			return this.taskId;
		}
		return this.activityId;
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x0004D540 File Offset: 0x0004B740
	public int Compare(ActivityDefinition x, ActivityDefinition y)
	{
		if (x.activityType == ActivityDefinition.ContinueType.neutral)
		{
			if (y.activityType == ActivityDefinition.ContinueType.neutral)
			{
				return 0;
			}
			return -1;
		}
		else
		{
			if (y.activityType == ActivityDefinition.ContinueType.neutral)
			{
				return 1;
			}
			if (x.activityType > y.activityType)
			{
				return -1;
			}
			if (x.activityType < y.activityType)
			{
				return 1;
			}
			if (x.activityType != y.activityType)
			{
				return 0;
			}
			if (x.requiredAmount > y.requiredAmount)
			{
				return -1;
			}
			if (x.requiredAmount < y.requiredAmount)
			{
				return 1;
			}
			return 0;
		}
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0004D5BC File Offset: 0x0004B7BC
	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		ActivityDefinition activityDefinition = obj as ActivityDefinition;
		if (activityDefinition == null)
		{
			throw new ArgumentException("Object is not an ActivityDefinition");
		}
		if (this.activityType == ActivityDefinition.ContinueType.neutral)
		{
			if (activityDefinition.activityType == this.activityType)
			{
				return 0;
			}
			return 1;
		}
		else
		{
			if (activityDefinition.activityType == ActivityDefinition.ContinueType.neutral)
			{
				return -1;
			}
			if (activityDefinition.activityType > this.activityType)
			{
				return -1;
			}
			if (activityDefinition.activityType < this.activityType)
			{
				return 1;
			}
			if (activityDefinition.requiredAmount > this.requiredAmount)
			{
				return -1;
			}
			if (activityDefinition.requiredAmount < this.requiredAmount)
			{
				return 1;
			}
			return 0;
		}
	}

	// Token: 0x04000C4D RID: 3149
	public string activityId = string.Empty;

	// Token: 0x04000C4E RID: 3150
	public string taskId = string.Empty;

	// Token: 0x04000C4F RID: 3151
	public string displayText = string.Empty;

	// Token: 0x04000C50 RID: 3152
	public bool showOnPS5 = true;

	// Token: 0x04000C51 RID: 3153
	public bool isFinalTask;

	// Token: 0x04000C52 RID: 3154
	public bool newGamePlus;

	// Token: 0x04000C53 RID: 3155
	public ActivityDefinition.ContinueType activityType;

	// Token: 0x04000C54 RID: 3156
	public int requiredAmount;

	// Token: 0x0200037A RID: 890
	public enum ContinueType
	{
		// Token: 0x040013AE RID: 5038
		neutral,
		// Token: 0x040013AF RID: 5039
		awakened,
		// Token: 0x040013B0 RID: 5040
		realized,
		// Token: 0x040013B1 RID: 5041
		collectables
	}
}
