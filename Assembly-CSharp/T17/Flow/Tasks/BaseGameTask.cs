using System;

namespace T17.Flow.Tasks
{
	// Token: 0x02000256 RID: 598
	public abstract class BaseGameTask : IGameTask
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x0005CFA5 File Offset: 0x0005B1A5
		public TaskStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x0005CFAD File Offset: 0x0005B1AD
		public TaskResult Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x0005CFB5 File Offset: 0x0005B1B5
		public bool IsComplete
		{
			get
			{
				return this._status == TaskStatus.Complete;
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0005CFC0 File Offset: 0x0005B1C0
		public virtual void Start()
		{
			this._status = TaskStatus.InProgress;
			this._result = TaskResult.NotAvailableYet;
		}

		// Token: 0x06001381 RID: 4993
		public abstract void Update(float tTimeDelta);

		// Token: 0x06001382 RID: 4994 RVA: 0x0005CFD0 File Offset: 0x0005B1D0
		public virtual void End()
		{
			this._status = TaskStatus.NotStarted;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0005CFD9 File Offset: 0x0005B1D9
		protected virtual void SetAsComplete(bool success)
		{
			this._status = TaskStatus.Complete;
			this._result = (success ? TaskResult.Success : TaskResult.Failure);
		}

		// Token: 0x04000F31 RID: 3889
		protected TaskStatus _status;

		// Token: 0x04000F32 RID: 3890
		protected TaskResult _result;
	}
}
