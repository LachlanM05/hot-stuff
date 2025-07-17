using System;

namespace T17.Flow.Tasks
{
	// Token: 0x02000255 RID: 597
	public interface IGameTask
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06001377 RID: 4983
		TaskStatus Status { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06001378 RID: 4984
		TaskResult Result { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06001379 RID: 4985
		bool IsComplete { get; }

		// Token: 0x0600137A RID: 4986
		void Start();

		// Token: 0x0600137B RID: 4987
		void Update(float tTimeDelta);

		// Token: 0x0600137C RID: 4988
		void End();
	}
}
