using System;
using T17.Services;
using Team17.Common;
using Team17.Scripts.Services.ApplicationStatus;
using UnityEngine;

namespace Team17.Scripts.Services
{
	// Token: 0x02000215 RID: 533
	public class GameSuspendService : IGameSuspendService, IService
	{
		// Token: 0x06001152 RID: 4434 RVA: 0x00057C20 File Offset: 0x00055E20
		public void OnStartUp()
		{
			Services.ApplicationStatusPlatformService.OnApplicationStatusChangedEvent += this.OnApplicationStatusChanged;
			Services.PostEngagementService.OnCompletedHandlingLostEvent += this.OnFoundUserOrController;
			Services.PostEngagementService.OnHandlingLostUserEvent += this.OnLostUserOrController;
			Services.PostEngagementService.OnHandlingLostControllerEvent += this.OnLostUserOrController;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00057C88 File Offset: 0x00055E88
		public void OnCleanUp()
		{
			if (this._gameSuspended)
			{
				this.ResumeSuspendGame();
			}
			this._gameSuspendRequestRefCount = 0;
			this._gameSuspended = false;
			Services.ApplicationStatusPlatformService.OnApplicationStatusChangedEvent -= this.OnApplicationStatusChanged;
			Services.PostEngagementService.OnCompletedHandlingLostEvent -= this.OnFoundUserOrController;
			Services.PostEngagementService.OnHandlingLostUserEvent -= this.OnLostUserOrController;
			Services.PostEngagementService.OnHandlingLostControllerEvent -= this.OnLostUserOrController;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00057D09 File Offset: 0x00055F09
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00057D0B File Offset: 0x00055F0B
		public bool IsGameSuspended()
		{
			return this._gameSuspended;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00057D13 File Offset: 0x00055F13
		private void OnApplicationStatusChanged(EApplicationStatus applicationStatus)
		{
			switch (applicationStatus)
			{
			case EApplicationStatus.FocusGained:
				this.DecreaseSuspendRefCount();
				return;
			case EApplicationStatus.FocusLost:
				this.IncreaseSuspendRefCount();
				return;
			case EApplicationStatus.Suspended:
				this.IncreaseSuspendRefCount();
				return;
			case EApplicationStatus.ResumedFromSuspend:
				this.DecreaseSuspendRefCount();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00057D47 File Offset: 0x00055F47
		private void OnLostUserOrController()
		{
			this.IncreaseSuspendRefCount();
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00057D4F File Offset: 0x00055F4F
		private void OnFoundUserOrController()
		{
			this.DecreaseSuspendRefCount();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00057D57 File Offset: 0x00055F57
		private void IncreaseSuspendRefCount()
		{
			this._gameSuspendRequestRefCount++;
			this.OnSuspendRefCountChanged();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00057D6D File Offset: 0x00055F6D
		private void DecreaseSuspendRefCount()
		{
			this._gameSuspendRequestRefCount--;
			this.OnSuspendRefCountChanged();
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00057D84 File Offset: 0x00055F84
		private void OnSuspendRefCountChanged()
		{
			if (this._gameSuspendRequestRefCount < 0)
			{
				this._gameSuspendRequestRefCount = 0;
			}
			if (!this._gameSuspended && this._gameSuspendRequestRefCount > 0)
			{
				this.SuspendGame();
				return;
			}
			if (this._gameSuspended && this._gameSuspendRequestRefCount == 0)
			{
				this.ResumeSuspendGame();
				return;
			}
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00057DD0 File Offset: 0x00055FD0
		private void SuspendGame()
		{
			Time.timeScale = 0f;
			this._gameSuspended = true;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00057DE3 File Offset: 0x00055FE3
		private void ResumeSuspendGame()
		{
			Time.timeScale = 1f;
			this._gameSuspended = false;
		}

		// Token: 0x04000E51 RID: 3665
		private const float kSuspendTimeScale = 0f;

		// Token: 0x04000E52 RID: 3666
		private const float kNormalTimeScale = 1f;

		// Token: 0x04000E53 RID: 3667
		private int _gameSuspendRequestRefCount;

		// Token: 0x04000E54 RID: 3668
		private bool _gameSuspended;
	}
}
