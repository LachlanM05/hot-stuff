using System;

namespace Team17.Services.Activities
{
	// Token: 0x020001CA RID: 458
	public interface IActivitesServiceImpl
	{
		// Token: 0x06000F29 RID: 3881
		void StartUp();

		// Token: 0x06000F2A RID: 3882
		void ShutDown();

		// Token: 0x06000F2B RID: 3883
		void Update(float deltaTime);

		// Token: 0x06000F2C RID: 3884
		bool IsPlatformRelevant(ActivityDefinition activityDef);

		// Token: 0x06000F2D RID: 3885
		bool IsPlatformSupported();

		// Token: 0x06000F2E RID: 3886
		void ActivityStart(ActivityDefinition activityDef, ActivityResult resultCallback = null);

		// Token: 0x06000F2F RID: 3887
		void ActivityEnd(ActivityDefinition activityDef, ActivityOutcome outcome, ActivityResult resultCallback = null);

		// Token: 0x06000F30 RID: 3888
		void ActivityEnd(string activityId, ActivityOutcome outcome, ActivityResult resultCallback = null);

		// Token: 0x06000F31 RID: 3889
		void ActivityResume(string activityId, string[] activeTaskIds, string[] completeTaskIds, ActivityResult resultCallback = null);

		// Token: 0x06000F32 RID: 3890
		void ActivityTerminate();

		// Token: 0x06000F33 RID: 3891
		void ChangeActivityAvailability(string activityId, ActivityDefinition[] available, ActivityDefinition[] unavailable, ActivityResult resultCallback = null);

		// Token: 0x06000F34 RID: 3892
		void RegisterActivitySequencedEvent(ActivityRequestedEvent onActivityRequested);
	}
}
