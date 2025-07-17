using System;
using System.Threading.Tasks;
using Team17.Common;
using Team17.Platform.SaveGame;

namespace Team17.Services.Save
{
	// Token: 0x020001DA RID: 474
	public interface IMirandaSaveGameService : IService
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000FCD RID: 4045
		// (remove) Token: 0x06000FCE RID: 4046
		event Action OnSaveBegin;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000FCF RID: 4047
		// (remove) Token: 0x06000FD0 RID: 4048
		event Action<SaveGameError> OnSaveCompleted;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000FD1 RID: 4049
		// (remove) Token: 0x06000FD2 RID: 4050
		event Action<int> OnSlotUpdated;

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000FD3 RID: 4051
		bool IsBusy { get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000FD4 RID: 4052
		int LastSavedGameplaySlotIndex { get; }

		// Token: 0x06000FD5 RID: 4053
		Task<SaveResult<string>> LoadSettingsDataAsync();

		// Token: 0x06000FD6 RID: 4054
		Task<SaveGameError> SaveSettingsDataAsync(string json);

		// Token: 0x06000FD7 RID: 4055
		Task<SaveResult<string>> LoadGraphicsSettingsDataAsync();

		// Token: 0x06000FD8 RID: 4056
		Task<SaveGameError> SaveGraphicsSettingsDataAsync(string json);

		// Token: 0x06000FD9 RID: 4057
		Task<SaveGameError> LoadMetadataAsync(int slotIndex = 2147483647);

		// Token: 0x06000FDA RID: 4058
		SaveSlotMetadata GetSlotInfo(int slotNumber);

		// Token: 0x06000FDB RID: 4059
		SaveSlotMetadata GetLatestSlotInfo();

		// Token: 0x06000FDC RID: 4060
		int GetNumberSaveSlotsInUse();

		// Token: 0x06000FDD RID: 4061
		DateTime GetSaveFileTimeStamp();

		// Token: 0x06000FDE RID: 4062
		bool IsSlotAGameplaySlot(int slotIndex);

		// Token: 0x06000FDF RID: 4063
		Task<SaveResult<bool>> DoesGameplaySlotExistAsync(int slotIndex);

		// Token: 0x06000FE0 RID: 4064
		Task<SaveResult<string>> LoadGameplayDataAsync(int slotIndex);

		// Token: 0x06000FE1 RID: 4065
		Task<SaveGameError> SaveGameplayDataAsync(int slotIndex, Save.SaveData saveData);

		// Token: 0x06000FE2 RID: 4066
		Task<SaveGameError> DeleteGameplayDataAsync(int slotIndex);

		// Token: 0x06000FE3 RID: 4067
		Task ClearAllSaveData();

		// Token: 0x06000FE4 RID: 4068
		Task<SaveGameError> ForceSaveCurrentCombinedSaveFileAsync();

		// Token: 0x06000FE5 RID: 4069
		int GetTotalPlatformSlots();

		// Token: 0x04000DC2 RID: 3522
		public const int kSettingsSlotIndex = 0;

		// Token: 0x04000DC3 RID: 3523
		public const int kGraphicsSettingsSlotIndex = 1;

		// Token: 0x04000DC4 RID: 3524
		public const int kGameplaySlotIndex = 2;

		// Token: 0x04000DC5 RID: 3525
		public const int kMaxNumCombinedSaveGameSlots = 3;

		// Token: 0x04000DC6 RID: 3526
		public const int kLatestCombinedSave = 2147483647;
	}
}
