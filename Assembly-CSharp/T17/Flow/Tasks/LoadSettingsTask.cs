using System;
using System.Runtime.CompilerServices;
using T17.Services;
using T17.UI;
using Team17.Platform.SaveGame;

namespace T17.Flow.Tasks
{
	// Token: 0x02000258 RID: 600
	public class LoadSettingsTask : BaseGameTask
	{
		// Token: 0x06001391 RID: 5009 RVA: 0x0005D365 File Offset: 0x0005B565
		public override void Start()
		{
			base.Start();
			this._currentPhase = LoadSettingsTask.Phase.Initialise;
			this._supressErrorMessages = false;
			this.MoveToNextPhase();
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0005D381 File Offset: 0x0005B581
		public override void Update(float timeDelta)
		{
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0005D383 File Offset: 0x0005B583
		private void OnLoadGameSettingsCompleted(bool result)
		{
			Services.GraphicsSettings.Load(delegate(SaveGameErrorType result)
			{
				this.SetAsComplete(result == SaveGameErrorType.None);
			});
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0005D39C File Offset: 0x0005B59C
		private void HandleLoadComplete(SaveGameErrorType result)
		{
			if (result <= SaveGameErrorType.FileCorrupted)
			{
				if (result != SaveGameErrorType.None)
				{
					if (result != SaveGameErrorType.FileCorrupted)
					{
						goto IL_0077;
					}
					if (!this._supressErrorMessages)
					{
						UIDialogManager.Instance.ShowOKDialog("Settings file load Error", "The settings file is corrupt. Select OK to continue with the default settings", new Action(this.OnDialogCompleted_Continue), true);
						return;
					}
					this.OnDialogCompleted_Continue();
					return;
				}
			}
			else if (result != SaveGameErrorType.FileNotFound)
			{
				if (result != SaveGameErrorType.FileVersionTooHigh)
				{
					goto IL_0077;
				}
				UIDialogManager.Instance.ShowOKDialog("Settings file load Error", "The settings file is a newer version. Please download the latest version of the game and try again", new Action(this.OnDialogCompleted_ReturnToIIS), true);
				return;
			}
			this.MoveToNextPhase();
			return;
			IL_0077:
			if (!this._supressErrorMessages)
			{
				UIDialogManager.Instance.ShowDialog("Settings file load Error", "Failed to load settings file. Select Retry to try again or Continue to continue with the default settings", "Retry", new Action(this.OnDialogCompleted_Retry), "Continue", new Action(this.OnDialogCompleted_Continue), "", null, 0, true);
				return;
			}
			this.OnDialogCompleted_Continue();
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0005D470 File Offset: 0x0005B670
		private void MoveToNextPhase()
		{
			switch (this._currentPhase)
			{
			case LoadSettingsTask.Phase.Initialise:
				this._supressErrorMessages = false;
				this._currentPhase = LoadSettingsTask.Phase.LoadingGameSettings;
				Services.GameSettings.Load(new Action<SaveGameErrorType>(this.HandleLoadComplete));
				return;
			case LoadSettingsTask.Phase.LoadingGameSettings:
				this._currentPhase = LoadSettingsTask.Phase.LoadingGraphicsSettings;
				Services.GraphicsSettings.Load(new Action<SaveGameErrorType>(this.HandleLoadComplete));
				return;
			case LoadSettingsTask.Phase.LoadingGraphicsSettings:
				SettingsMenu.ForceResolutionToSavedValue();
				SettingsMenu.ForceQualityToSavedValue();
				this._currentPhase = LoadSettingsTask.Phase.Completed;
				this.SetAsComplete(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0005D4F4 File Offset: 0x0005B6F4
		private void OnDialogCompleted_Continue()
		{
			this._supressErrorMessages = true;
			if (this._currentPhase == LoadSettingsTask.Phase.LoadingGameSettings)
			{
				Services.GameSettings.Save(new Action<SaveGameError>(this.<OnDialogCompleted_Continue>g__OnSaveComplete|14_0), true);
				return;
			}
			if (this._currentPhase == LoadSettingsTask.Phase.LoadingGraphicsSettings)
			{
				Services.GraphicsSettings.Save(new Action<SaveGameError>(this.<OnDialogCompleted_Continue>g__OnSaveComplete|14_0), true);
				return;
			}
			this.MoveToNextPhase();
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0005D550 File Offset: 0x0005B750
		private void OnDialogCompleted_ReturnToIIS()
		{
			this.SetAsComplete(false);
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0005D55C File Offset: 0x0005B75C
		private void OnDialogCompleted_Retry()
		{
			LoadSettingsTask.Phase currentPhase = this._currentPhase;
			if (currentPhase <= LoadSettingsTask.Phase.LoadingGameSettings)
			{
				Services.GameSettings.Load(new Action<SaveGameErrorType>(this.HandleLoadComplete));
				return;
			}
			if (currentPhase != LoadSettingsTask.Phase.LoadingGraphicsSettings)
			{
				this.MoveToNextPhase();
				return;
			}
			Services.GraphicsSettings.Load(new Action<SaveGameErrorType>(this.HandleLoadComplete));
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0005D5C5 File Offset: 0x0005B7C5
		[CompilerGenerated]
		private void <OnDialogCompleted_Continue>g__OnSaveComplete|14_0(SaveGameError result)
		{
			this.MoveToNextPhase();
		}

		// Token: 0x04000F40 RID: 3904
		private const string kDialogHeader = "Settings file load Error";

		// Token: 0x04000F41 RID: 3905
		private const string kDialogMessage_Corrupt = "The settings file is corrupt. Select OK to continue with the default settings";

		// Token: 0x04000F42 RID: 3906
		private const string kDialogMessage_Version = "The settings file is a newer version. Please download the latest version of the game and try again";

		// Token: 0x04000F43 RID: 3907
		private const string kDialogMessage_GeneralError = "Failed to load settings file. Select Retry to try again or Continue to continue with the default settings";

		// Token: 0x04000F44 RID: 3908
		private const string kButtonRetry = "Retry";

		// Token: 0x04000F45 RID: 3909
		private const string kButtonContinue = "Continue";

		// Token: 0x04000F46 RID: 3910
		private LoadSettingsTask.Phase _currentPhase;

		// Token: 0x04000F47 RID: 3911
		private bool _supressErrorMessages;

		// Token: 0x020003D0 RID: 976
		private enum Phase
		{
			// Token: 0x04001506 RID: 5382
			Initialise,
			// Token: 0x04001507 RID: 5383
			LoadingGameSettings,
			// Token: 0x04001508 RID: 5384
			LoadingGraphicsSettings,
			// Token: 0x04001509 RID: 5385
			Completed
		}
	}
}
