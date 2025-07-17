using System;
using System.Threading.Tasks;
using T17.Services;
using Team17.Platform.SaveGame;

// Token: 0x0200018A RID: 394
public class GameSettingsProvider : SettingsProvider
{
	// Token: 0x06000DAD RID: 3501 RVA: 0x0004D038 File Offset: 0x0004B238
	protected override async Task<SaveGameError> SerialiseData()
	{
		string text = this.ToJson();
		return await Services.SaveGameService.SaveSettingsDataAsync(text);
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x0004D07C File Offset: 0x0004B27C
	protected override async Task<SaveGameErrorType> DeserialiseData()
	{
		SaveResult<string> saveResult = await Services.SaveGameService.LoadSettingsDataAsync();
		SaveGameErrorType saveGameErrorType = saveResult.Error;
		if (saveResult.Error == SaveGameErrorType.None)
		{
			if (saveResult.Version > GameSettingsProvider.DataVersion.Latest && saveResult.Version != 4294967295U)
			{
				this.SetToDefaultSettings();
				saveGameErrorType = SaveGameErrorType.FileVersionTooHigh;
			}
			else
			{
				uint version = saveResult.Version;
				uint latest = GameSettingsProvider.DataVersion.Latest;
				if (!this.FromJson(saveResult.Result))
				{
					this.SetToDefaultSettings();
					saveGameErrorType = SaveGameErrorType.Broken;
				}
				else
				{
					saveGameErrorType = SaveGameErrorType.None;
				}
			}
		}
		else if (saveResult.Error == SaveGameErrorType.FileNotFound)
		{
			this.SetToDefaultSettings();
			saveGameErrorType = SaveGameErrorType.None;
		}
		else
		{
			this.SetToDefaultSettings();
		}
		return saveGameErrorType;
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x0004D0BF File Offset: 0x0004B2BF
	private void SetToDefaultSettings()
	{
		this._settingsData.Clear();
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x0004D0CC File Offset: 0x0004B2CC
	public override void Load(Action<SaveGameErrorType> onCompltionCallback = null)
	{
		base.Load(delegate(SaveGameErrorType result)
		{
			if (result == SaveGameErrorType.None)
			{
				Services.StatsService.LoadAndProcessStatsData();
			}
			else
			{
				Services.StatsService.CreateNewStatsData();
			}
			Action<SaveGameErrorType> onCompltionCallback2 = onCompltionCallback;
			if (onCompltionCallback2 == null)
			{
				return;
			}
			onCompltionCallback2(result);
		});
	}

	// Token: 0x0200036F RID: 879
	public static class DataVersion
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060017F1 RID: 6129 RVA: 0x0006CB19 File Offset: 0x0006AD19
		public static uint Latest
		{
			get
			{
				return 1U;
			}
		}

		// Token: 0x04001383 RID: 4995
		private const int Initial = 0;

		// Token: 0x04001384 RID: 4996
		public const int JsonAsBase64 = 1;
	}
}
