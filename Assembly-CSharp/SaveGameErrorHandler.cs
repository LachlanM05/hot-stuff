using System;
using T17.Services;
using T17.UI;
using Team17.Platform.SaveGame;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class SaveGameErrorHandler : MonoBehaviour
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000D9D RID: 3485 RVA: 0x0004CCD4 File Offset: 0x0004AED4
	// (remove) Token: 0x06000D9E RID: 3486 RVA: 0x0004CD08 File Offset: 0x0004AF08
	public static event Action<int> OnRetrySave;

	// Token: 0x06000D9F RID: 3487 RVA: 0x0004CD3B File Offset: 0x0004AF3B
	private void Start()
	{
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x0004CD40 File Offset: 0x0004AF40
	private void Initialise()
	{
		if (!this._isInitialised && Services.SaveGameService != null)
		{
			Services.SaveGameService.OnSaveCompleted += this.OnSaveCompleted;
			Save.onGameSaveCompleted += this.OnSaveCompleted;
			this._didPauseGame = false;
			this._isInitialised = true;
		}
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x0004CD91 File Offset: 0x0004AF91
	private void Update()
	{
		if (!this._isInitialised)
		{
			this.Initialise();
		}
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x0004CDA1 File Offset: 0x0004AFA1
	private void OnDestroy()
	{
		if (Services.SaveGameService != null)
		{
			Services.SaveGameService.OnSaveCompleted -= this.OnSaveCompleted;
		}
		Save.onGameSaveCompleted -= this.OnSaveCompleted;
		this._isInitialised = false;
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x0004CDD8 File Offset: 0x0004AFD8
	private void OnSaveCompleted(SaveGameError result)
	{
		if (Services.SaveGameService.IsSlotAGameplaySlot(result.SlotIndex))
		{
			this._lastErrorSlotindex = Services.SaveGameService.LastSavedGameplaySlotIndex;
			bool flag = this._lastErrorSlotindex < Save.MAX_AUTOSAVE_SLOTS;
			this._didPauseGame = false;
			SaveGameErrorType errorType = result.ErrorType;
			if (errorType != SaveGameErrorType.None)
			{
				if (errorType == SaveGameErrorType.OutOfMemory)
				{
					if (flag)
					{
						this.PauseGame();
						if (this._currentErrorDialogHandle == 0U)
						{
							this._currentErrorDialogHandle = UIDialogManager.Instance.ShowDialog("File Error", "Out of space while trying to save. Please free up some space and select on Continue to continue without saving", "Continue", new Action(this.OnContinueClicked), "", null, "", null, 0, true);
							return;
						}
					}
					else if (this._currentErrorDialogHandle == 0U)
					{
						this._currentErrorDialogHandle = UIDialogManager.Instance.ShowDialog("File Error", "Out of space while trying to save. Please free up some space and then select Retry to try again or select Continue to continue without saving", "Retry", new Action(this.OnRetryClicked), "Continue", new Action(this.OnContinueClicked), "", null, 0, true);
						return;
					}
				}
				else if (flag)
				{
					this.PauseGame();
					if (this._currentErrorDialogHandle == 0U)
					{
						this._currentErrorDialogHandle = UIDialogManager.Instance.ShowDialog("File Error", "Failed to save. Select Continue to continue without saving", "Continue", new Action(this.OnContinueClicked), "", null, "", null, 0, true);
						return;
					}
				}
				else if (this._currentErrorDialogHandle == 0U)
				{
					this._currentErrorDialogHandle = UIDialogManager.Instance.ShowDialog("File Error", "Failed to save. Select Retry to try again or on Continue to continue without saving", "Retry", new Action(this.OnRetryClicked), "Continue", new Action(this.OnContinueClicked), "", null, 0, true);
				}
			}
		}
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x0004CF6E File Offset: 0x0004B16E
	private void PauseGame()
	{
		if (!PlayerPauser.IsPaused())
		{
			PlayerPauser.Pause();
			this._didPauseGame = true;
		}
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x0004CF83 File Offset: 0x0004B183
	private void UnpauseGame()
	{
		if (this._didPauseGame)
		{
			PlayerPauser.Unpause();
			this._didPauseGame = false;
		}
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x0004CF99 File Offset: 0x0004B199
	private void OnContinueClicked()
	{
		this._currentErrorDialogHandle = 0U;
		this.UnpauseGame();
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x0004CFA8 File Offset: 0x0004B1A8
	private void OnRetryClicked()
	{
		this._currentErrorDialogHandle = 0U;
		Action<int> onRetrySave = SaveGameErrorHandler.OnRetrySave;
		if (onRetrySave == null)
		{
			return;
		}
		onRetrySave(this._lastErrorSlotindex);
	}

	// Token: 0x04000C3C RID: 3132
	private const string dialogTitle = "File Error";

	// Token: 0x04000C3D RID: 3133
	private const string errorMessage_DiskSpace = "Out of space while trying to save. Please free up some space and then select Retry to try again or select Continue to continue without saving";

	// Token: 0x04000C3E RID: 3134
	private const string errorMessage_General = "Failed to save. Select Retry to try again or on Continue to continue without saving";

	// Token: 0x04000C3F RID: 3135
	private const string errorMessage_DiskSpaceNoRetry = "Out of space while trying to save. Please free up some space and select on Continue to continue without saving";

	// Token: 0x04000C40 RID: 3136
	private const string errorMessage_GeneralNoRetry = "Failed to save. Select Continue to continue without saving";

	// Token: 0x04000C41 RID: 3137
	private const string buttonRetry = "Retry";

	// Token: 0x04000C42 RID: 3138
	private const string buttonContinue = "Continue";

	// Token: 0x04000C43 RID: 3139
	private bool _isInitialised;

	// Token: 0x04000C44 RID: 3140
	private int _lastErrorSlotindex = -1;

	// Token: 0x04000C45 RID: 3141
	private bool _didPauseGame;

	// Token: 0x04000C46 RID: 3142
	private uint _currentErrorDialogHandle;
}
