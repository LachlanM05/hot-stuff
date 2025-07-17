using System;
using System.Threading.Tasks;
using T17.Services;
using Team17.Platform.SaveGame;

// Token: 0x0200018B RID: 395
public class GraphicsSettingsProvider : SettingsProvider
{
	// Token: 0x06000DB2 RID: 3506 RVA: 0x0004D100 File Offset: 0x0004B300
	protected override async Task<SaveGameError> SerialiseData()
	{
		string text = this.ToJson();
		return await Services.SaveGameService.SaveGraphicsSettingsDataAsync(text);
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x0004D144 File Offset: 0x0004B344
	protected override async Task<SaveGameErrorType> DeserialiseData()
	{
		SaveResult<string> saveResult = await Services.SaveGameService.LoadGraphicsSettingsDataAsync();
		SaveGameErrorType saveGameErrorType = saveResult.Error;
		if (saveResult.Error == SaveGameErrorType.None)
		{
			if (saveResult.Version > GraphicsSettingsProvider.DataVersion.Latest && saveResult.Version != 4294967295U)
			{
				this.SetToDefaultSettings();
				saveGameErrorType = SaveGameErrorType.FileVersionTooHigh;
			}
			else
			{
				uint version = saveResult.Version;
				uint latest = GraphicsSettingsProvider.DataVersion.Latest;
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

	// Token: 0x06000DB4 RID: 3508 RVA: 0x0004D187 File Offset: 0x0004B387
	private void SetToDefaultSettings()
	{
		this._settingsData.Clear();
	}

	// Token: 0x02000373 RID: 883
	public static class DataVersion
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x0006CD72 File Offset: 0x0006AF72
		public static uint Latest
		{
			get
			{
				return 1U;
			}
		}

		// Token: 0x0400138E RID: 5006
		private const int Initial = 0;

		// Token: 0x0400138F RID: 5007
		public const int JsonAsBase64 = 1;
	}
}
