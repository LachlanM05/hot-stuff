using System;
using System.Threading.Tasks;
using T17.Services;
using T17.UI;
using Team17.Platform.SaveGame;

namespace T17.Flow.Tasks
{
	// Token: 0x02000257 RID: 599
	public class LoadSaveSlotMetadataTask : BaseGameTask
	{
		// Token: 0x06001385 RID: 4997 RVA: 0x0005CFF7 File Offset: 0x0005B1F7
		public override void Start()
		{
			base.Start();
			this._InitialMetafileSlot = int.MaxValue;
			this._CurrentMetafileSlot = int.MaxValue;
			this._loadTask = Services.SaveGameService.LoadMetadataAsync(int.MaxValue);
			this._deleteTask = null;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0005D034 File Offset: 0x0005B234
		public override void Update(float tTimeDelta)
		{
			if (this._loadTask != null && this._loadTask.IsCompleted)
			{
				SaveGameError result = this._loadTask.Result;
				this._loadTask = null;
				if (this._CurrentMetafileSlot != 2147483647)
				{
					this.HandleRecoverLoadComplete(result);
				}
				else
				{
					this.HandleLoadComplete(result);
				}
			}
			if (this._deleteTask != null && this._deleteTask.IsCompleted)
			{
				this.SetAsComplete(true);
			}
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0005D0A4 File Offset: 0x0005B2A4
		private void HandleLoadComplete(SaveGameError result)
		{
			SaveGameErrorType errorType = result.ErrorType;
			if (errorType <= SaveGameErrorType.FileCorrupted)
			{
				if (errorType != SaveGameErrorType.None)
				{
					if (errorType != SaveGameErrorType.FileCorrupted)
					{
						goto IL_00DB;
					}
					UIDialogManager.Instance.ShowDialog("SaveGame File Error", "The save game file is corrupt. Select Recover to try and recover an earlier backup", "Recover", new Action(this.OnDialogCompleted_RecoverBackup), "", null, "", null, 0, true);
					this._InitialMetafileSlot = (this._CurrentMetafileSlot = ((result.SlotIndex != int.MaxValue) ? result.SlotIndex : 2));
					return;
				}
			}
			else if (errorType != SaveGameErrorType.FileNotFound)
			{
				if (errorType == SaveGameErrorType.OutOfMemory)
				{
					UIDialogManager.Instance.ShowOKDialog("SaveGame File Error", "Out of space. Please free up some space and then try again", new Action(this.OnDialogCompleted_ReturnToIIS), true);
					return;
				}
				if (errorType != SaveGameErrorType.FileVersionTooHigh)
				{
					goto IL_00DB;
				}
				UIDialogManager.Instance.ShowOKDialog("SaveGame File Error", "The save game file is a newer version. Please download the latest version of the game and try again", new Action(this.OnDialogCompleted_ReturnToIIS), true);
				return;
			}
			this.SetAsComplete(true);
			return;
			IL_00DB:
			this._InitialMetafileSlot = (this._CurrentMetafileSlot = ((result.SlotIndex != int.MaxValue) ? result.SlotIndex : 2));
			UIDialogManager.Instance.ShowDialog("SaveGame File Error", "Failed to load the save game file. Select Retry to try again or Recover to try and recover an earlier backup", "Retry", new Action(this.OnDialogCompleted_Retry), "Recover", new Action(this.OnDialogCompleted_RecoverBackup), "", null, 0, true);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0005D1F4 File Offset: 0x0005B3F4
		private void HandleRecoverLoadComplete(SaveGameError result)
		{
			if (result.ErrorType == SaveGameErrorType.None)
			{
				Services.SaveGameService.ForceSaveCurrentCombinedSaveFileAsync();
				UIDialogManager.Instance.ShowOKDialog("SaveGame File Error", "Successfully recovered a backup save file from " + Services.SaveGameService.GetSaveFileTimeStamp().ToString(), delegate
				{
					this.SetAsComplete(true);
				}, true);
				return;
			}
			this.TryRecoverPreviousBackup();
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0005D258 File Offset: 0x0005B458
		private void TryRecoverPreviousBackup()
		{
			this._CurrentMetafileSlot--;
			if (this._CurrentMetafileSlot < 2)
			{
				this._CurrentMetafileSlot = 2 + Services.SaveGameService.GetTotalPlatformSlots() - 1;
			}
			if (this._CurrentMetafileSlot == this._InitialMetafileSlot)
			{
				UIDialogManager.Instance.ShowOKDialog("SaveGame File Error", "Unable to recover a backup of a save game file. All progress will now be reset and an empty save game will be created", new Action(this.OnDialogCompleted_FailedRecoverBackup), true);
				return;
			}
			this._loadTask = Services.SaveGameService.LoadMetadataAsync(this._CurrentMetafileSlot);
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0005D2D7 File Offset: 0x0005B4D7
		private void OnDialogCompleted_ReturnToIIS()
		{
			this.SetAsComplete(false);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005D2E0 File Offset: 0x0005B4E0
		private void OnDialogCompleted_RecoverBackup()
		{
			this.TryRecoverPreviousBackup();
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0005D2E8 File Offset: 0x0005B4E8
		private void OnDialogCompleted_FailedRecoverBackup()
		{
			this._deleteTask = Services.SaveGameService.ClearAllSaveData();
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0005D2FA File Offset: 0x0005B4FA
		private void OnDialogCompleted_Retry()
		{
			this._InitialMetafileSlot = int.MaxValue;
			this._CurrentMetafileSlot = int.MaxValue;
			this._loadTask = Services.SaveGameService.LoadMetadataAsync(int.MaxValue);
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0005D327 File Offset: 0x0005B527
		protected override void SetAsComplete(bool success)
		{
			this._deleteTask = null;
			this._loadTask = null;
			base.SetAsComplete(success);
		}

		// Token: 0x04000F33 RID: 3891
		private const string kDialogHeader = "SaveGame File Error";

		// Token: 0x04000F34 RID: 3892
		private const string kDialogMessage_Corrupt = "The save game file is corrupt. Select Recover to try and recover an earlier backup";

		// Token: 0x04000F35 RID: 3893
		private const string kDialogMessage_Version = "The save game file is a newer version. Please download the latest version of the game and try again";

		// Token: 0x04000F36 RID: 3894
		private const string kDialogMessage_OutOfSpace = "Out of space. Please free up some space and then try again";

		// Token: 0x04000F37 RID: 3895
		private const string kDialogMessage_GeneralError = "Failed to load the save game file. Select Retry to try again or Recover to try and recover an earlier backup";

		// Token: 0x04000F38 RID: 3896
		private const string kDialogMessage_SuccesfullyRecovered = "Successfully recovered a backup save file from ";

		// Token: 0x04000F39 RID: 3897
		private const string kDialogMessage_FailedToRecover = "Unable to recover a backup of a save game file. All progress will now be reset and an empty save game will be created";

		// Token: 0x04000F3A RID: 3898
		private const string kButtonRetry = "Retry";

		// Token: 0x04000F3B RID: 3899
		private const string kButtonRecover = "Recover";

		// Token: 0x04000F3C RID: 3900
		private Task<SaveGameError> _loadTask;

		// Token: 0x04000F3D RID: 3901
		private Task _deleteTask;

		// Token: 0x04000F3E RID: 3902
		private int _InitialMetafileSlot = int.MaxValue;

		// Token: 0x04000F3F RID: 3903
		private int _CurrentMetafileSlot = int.MaxValue;
	}
}
