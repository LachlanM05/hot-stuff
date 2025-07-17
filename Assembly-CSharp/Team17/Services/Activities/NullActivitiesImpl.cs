using System;

namespace Team17.Services.Activities
{
	// Token: 0x020001CC RID: 460
	public class NullActivitiesImpl : IActivitesServiceImpl
	{
		// Token: 0x06000F48 RID: 3912 RVA: 0x000535CD File Offset: 0x000517CD
		public void StartUp()
		{
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x000535CF File Offset: 0x000517CF
		public void ShutDown()
		{
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000535D1 File Offset: 0x000517D1
		public void Update(float deltaTime)
		{
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000535D3 File Offset: 0x000517D3
		public bool IsPlatformRelevant(ActivityDefinition activityDef)
		{
			return false;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000535D6 File Offset: 0x000517D6
		public bool IsPlatformSupported()
		{
			return false;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000535D9 File Offset: 0x000517D9
		public void ActivityStart(ActivityDefinition activityDef, ActivityResult resultCallback = null)
		{
			if (resultCallback != null)
			{
				resultCallback(activityDef, true);
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x000535E6 File Offset: 0x000517E6
		public void ActivityEnd(ActivityDefinition activityDef, ActivityOutcome outcome, ActivityResult resultCallback = null)
		{
			if (resultCallback != null)
			{
				resultCallback(activityDef, true);
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000535F3 File Offset: 0x000517F3
		public void ActivityEnd(string activityDef, ActivityOutcome outcome, ActivityResult resultCallback = null)
		{
			if (resultCallback != null)
			{
				resultCallback(null, true);
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00053600 File Offset: 0x00051800
		public void ActivityResume(string activityDef, string[] activeTaskIds, string[] completeTaskIds, ActivityResult resultCallback = null)
		{
			if (resultCallback != null)
			{
				resultCallback(null, true);
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0005360F File Offset: 0x0005180F
		public void ActivityTerminate()
		{
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00053611 File Offset: 0x00051811
		public void ChangeActivityAvailability(string activityId, ActivityDefinition[] available, ActivityDefinition[] unavailable, ActivityResult resultCallback = null)
		{
			if (resultCallback != null)
			{
				resultCallback(null, true);
			}
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00053620 File Offset: 0x00051820
		public void ChangeActivityAvailability(ActivityDefinition.ContinueType activityType, int headCount = -1, ActivityResult resultCallback = null)
		{
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00053622 File Offset: 0x00051822
		public void RegisterActivitySequencedEvent(ActivityRequestedEvent onActivityRequested)
		{
		}
	}
}
