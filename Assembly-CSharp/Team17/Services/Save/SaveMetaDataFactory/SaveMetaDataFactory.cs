using System;
using Team17.Platform.SaveGame;

namespace Team17.Services.Save.SaveMetaDataFactory
{
	// Token: 0x020001DD RID: 477
	public class SaveMetaDataFactory : ISaveMetaDataFactory
	{
		// Token: 0x0600101C RID: 4124 RVA: 0x00055328 File Offset: 0x00053528
		private static string GetSettingsTitle()
		{
			return SaveMetaDataFactory.GetSlotTitle("Settings Data");
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00055334 File Offset: 0x00053534
		private static string GetSettingsDescription()
		{
			return SaveMetaDataFactory.GetSlotDescription("Settings Data");
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00055340 File Offset: 0x00053540
		private static string GetGraphicsSettingsTitle()
		{
			return SaveMetaDataFactory.GetSlotTitle("Graphics Settings Data");
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0005534C File Offset: 0x0005354C
		private static string GetGraphicsSettingsDescription()
		{
			return SaveMetaDataFactory.GetSlotDescription("Graphics Settings Data");
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00055358 File Offset: 0x00053558
		private static string GetGameplayTitle()
		{
			return SaveMetaDataFactory.GetSlotTitle("Gameplay Data");
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00055364 File Offset: 0x00053564
		private static string GetGameplayDescription()
		{
			return SaveMetaDataFactory.GetSlotDescription("Gameplay Data");
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00055370 File Offset: 0x00053570
		public virtual SaveMetaData Create()
		{
			SaveMetaData saveMetaData = new SaveMetaData
			{
				m_MaxSaveSizeBytes = 104857600UL,
				m_SaveTitle = "Date Everything",
				m_SaveDescription = "Date Everything savedata",
				m_SlotTitles = new string[Save.MAX_SAVE_SLOTS + 2],
				m_SlotDescriptions = new string[Save.MAX_SAVE_SLOTS + 2]
			};
			saveMetaData.m_SlotTitles[0] = SaveMetaDataFactory.GetSettingsTitle();
			saveMetaData.m_SlotTitles[1] = SaveMetaDataFactory.GetGraphicsSettingsTitle();
			saveMetaData.m_SlotTitles[2] = SaveMetaDataFactory.GetGameplayTitle();
			saveMetaData.m_SlotDescriptions[0] = SaveMetaDataFactory.GetSettingsDescription();
			saveMetaData.m_SlotDescriptions[1] = SaveMetaDataFactory.GetGraphicsSettingsDescription();
			saveMetaData.m_SlotDescriptions[2] = SaveMetaDataFactory.GetGameplayDescription();
			string gameplayTitle = SaveMetaDataFactory.GetGameplayTitle();
			string gameplayDescription = SaveMetaDataFactory.GetGameplayDescription();
			for (int i = 2; i < 2 + Save.MAX_SAVE_SLOTS; i++)
			{
				saveMetaData.m_SlotTitles[i] = gameplayTitle;
				saveMetaData.m_SlotDescriptions[i] = gameplayDescription;
			}
			return saveMetaData;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00055448 File Offset: 0x00053648
		private static string GetSlotTitle(string slot)
		{
			return SaveMetaDataFactory.GetLocalization(slot);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00055450 File Offset: 0x00053650
		private static string GetSlotDescription(string slot)
		{
			return SaveMetaDataFactory.GetLocalization(slot);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00055458 File Offset: 0x00053658
		private static string GetLocalization(string key)
		{
			return key;
		}

		// Token: 0x04000DD7 RID: 3543
		private const string Settings = "Settings Data";

		// Token: 0x04000DD8 RID: 3544
		private const string Gameplay = "Gameplay Data";

		// Token: 0x04000DD9 RID: 3545
		private const string GraphicsSettings = "Graphics Settings Data";
	}
}
