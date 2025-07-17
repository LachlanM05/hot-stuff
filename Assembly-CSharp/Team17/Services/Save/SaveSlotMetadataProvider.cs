using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Team17.Common;
using Team17.Platform.SaveGame;

namespace Team17.Services.Save
{
	// Token: 0x020001D9 RID: 473
	public class SaveSlotMetadataProvider
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0005441F File Offset: 0x0005261F
		public int Count
		{
			get
			{
				return this._saveSlotMetadata._slots.Count;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00054431 File Offset: 0x00052631
		public DateTime TimeStamp
		{
			get
			{
				return this._saveSlotMetadata.TimeStamp;
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00054440 File Offset: 0x00052640
		public void SetSlotData(int slotNumber, Save.SaveData data)
		{
			SaveSlotMetadata saveSlotMetadata;
			if (this._saveSlotMetadata._slots.TryGetValue(slotNumber, out saveSlotMetadata))
			{
				saveSlotMetadata.Set(slotNumber, data);
				this._saveSlotMetadata._slots[slotNumber] = saveSlotMetadata;
				return;
			}
			SaveSlotMetadata saveSlotMetadata2 = new SaveSlotMetadata(slotNumber, data);
			this._saveSlotMetadata._slots.Add(slotNumber, saveSlotMetadata2);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00054497 File Offset: 0x00052697
		public void SetSlotData(int slotNumber, SaveSlotMetadata metadata)
		{
			if (this._saveSlotMetadata._slots.ContainsKey(slotNumber))
			{
				this._saveSlotMetadata._slots[slotNumber] = metadata;
				return;
			}
			this._saveSlotMetadata._slots.Add(slotNumber, metadata);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x000544D4 File Offset: 0x000526D4
		public SaveSlotMetadata GetSlotData(int slotNumber)
		{
			SaveSlotMetadata saveSlotMetadata;
			if (this._saveSlotMetadata._slots.TryGetValue(slotNumber, out saveSlotMetadata))
			{
				return saveSlotMetadata;
			}
			return null;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x000544FC File Offset: 0x000526FC
		public SaveSlotMetadata GetLatestSlotData()
		{
			if (this._saveSlotMetadata._slots.Count == 0)
			{
				return null;
			}
			SaveSlotMetadata saveSlotMetadata = null;
			DateTime? dateTime = null;
			foreach (SaveSlotMetadata saveSlotMetadata2 in this._saveSlotMetadata._slots.Values)
			{
				if (saveSlotMetadata2.SlotNumber < Save.MAX_SAVE_SLOTS)
				{
					DateTime dateTime2;
					DateTime.TryParse(saveSlotMetadata2.TimeStamp, out dateTime2);
					if (dateTime == null || DateTime.Compare(dateTime2, dateTime.Value) > 0)
					{
						saveSlotMetadata = saveSlotMetadata2;
						dateTime = new DateTime?(dateTime2);
					}
				}
			}
			return saveSlotMetadata;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x000545B0 File Offset: 0x000527B0
		public void RemoveSlotData(int slotNumber)
		{
			if (this._saveSlotMetadata._slots.ContainsKey(slotNumber))
			{
				this._saveSlotMetadata._slots.Remove(slotNumber);
			}
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x000545D7 File Offset: 0x000527D7
		public string Serialise()
		{
			this._saveSlotMetadata.TimeStamp = DateTime.Now;
			return JsonConvert.SerializeObject(this._saveSlotMetadata);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x000545F4 File Offset: 0x000527F4
		public SaveGameErrorType Deserialise(SaveResult<string> loadMetaDataResult)
		{
			try
			{
				if (loadMetaDataResult.Error != SaveGameErrorType.None)
				{
					return loadMetaDataResult.Error;
				}
				if (loadMetaDataResult.Version > SaveSlotMetadata.Version.Latest && loadMetaDataResult.Version != 4294967295U)
				{
					this._saveSlotMetadata = new SaveMetadataInfo();
					return SaveGameErrorType.FileVersionTooHigh;
				}
				if (string.IsNullOrEmpty(loadMetaDataResult.Result))
				{
					T17Debug.LogError("[SaveSlotMetadataProvider] failed to deserialise the meta data - data is null or empty");
					this._saveSlotMetadata = new SaveMetadataInfo();
					return SaveGameErrorType.Broken;
				}
				uint version = loadMetaDataResult.Version;
				uint latest = SaveSlotMetadata.Version.Latest;
				this._saveSlotMetadata = JsonConvert.DeserializeObject<SaveMetadataInfo>(loadMetaDataResult.Result);
			}
			catch (Exception ex)
			{
				T17Debug.LogError("[SaveSlotMetadata] Exception trying to deserialise the save slot metadata. error=" + ex.Message);
				this._saveSlotMetadata = new SaveMetadataInfo();
				return SaveGameErrorType.Broken;
			}
			return SaveGameErrorType.None;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x000546BC File Offset: 0x000528BC
		public void RebuildMetadata(CombinedSaveSlotsContainer combinedSaveSlots)
		{
			this._saveSlotMetadata._slots.Clear();
			int i = 0;
			int numberOfSlots = combinedSaveSlots.NumberOfSlots;
			while (i < numberOfSlots)
			{
				string slotData = combinedSaveSlots.GetSlotData(i);
				if (!string.IsNullOrEmpty(slotData))
				{
					Save.SaveData saveData = Save.GetSaveData(slotData, combinedSaveSlots.SlotDataVersion);
					SaveSlotMetadata saveSlotMetadata = new SaveSlotMetadata(i, saveData);
					this._saveSlotMetadata._slots.Add(i, saveSlotMetadata);
				}
				i++;
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00054725 File Offset: 0x00052925
		public void ClearAll()
		{
			this._saveSlotMetadata._slots.Clear();
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00054738 File Offset: 0x00052938
		public async Task<bool> RebuildMetadata(int segmentGameplayFirstSlotIndex, List<EnumeratedSlotInfo> allSlots, List<EnumeratedSlotInfo> gameplaySlots, IMirandaSaveGameService saveGameService)
		{
			bool flag;
			if (allSlots == null || allSlots.Count == 0)
			{
				flag = false;
			}
			else
			{
				List<int> list = new List<int>();
				foreach (KeyValuePair<int, SaveSlotMetadata> keyValuePair in this._saveSlotMetadata._slots)
				{
					bool flag2 = false;
					for (int i = 0; i < allSlots.Count; i++)
					{
						if (allSlots[i].SlotIndex == keyValuePair.Key + segmentGameplayFirstSlotIndex)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						list.Add(keyValuePair.Key);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					this._saveSlotMetadata._slots.Remove(list[j]);
				}
				foreach (EnumeratedSlotInfo enumeratedSlotInfo in allSlots)
				{
					if (gameplaySlots.Contains(enumeratedSlotInfo))
					{
						int gameSlotNumber = enumeratedSlotInfo.SlotIndex - segmentGameplayFirstSlotIndex;
						if (!this._saveSlotMetadata._slots.ContainsKey(gameSlotNumber))
						{
							SaveSlotMetadata metadataInfo = new SaveSlotMetadata(gameSlotNumber, null);
							SaveResult<string> saveResult = await saveGameService.LoadGameplayDataAsync(gameSlotNumber);
							if (saveResult.Error == SaveGameErrorType.None)
							{
								metadataInfo.Set(gameSlotNumber, Save.GetSaveData(saveResult.Result, saveResult.Version));
							}
							this._saveSlotMetadata._slots.Add(gameSlotNumber, metadataInfo);
							metadataInfo = null;
						}
					}
				}
				List<EnumeratedSlotInfo>.Enumerator enumerator2 = default(List<EnumeratedSlotInfo>.Enumerator);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0005479C File Offset: 0x0005299C
		public bool ValidateMetadata(List<EnumeratedSlotInfo> allSlots, List<EnumeratedSlotInfo> gameplaySlots, int segmentFirstGameplaySlotIndex)
		{
			if (this._saveSlotMetadata._slots == null || this._saveSlotMetadata._slots.Count == 0)
			{
				return false;
			}
			if (this._saveSlotMetadata._slots.Count != gameplaySlots.Count)
			{
				return false;
			}
			for (int i = 0; i < allSlots.Count; i++)
			{
				EnumeratedSlotInfo enumeratedSlotInfo = allSlots[i];
				if (gameplaySlots.Contains(enumeratedSlotInfo) && !this._saveSlotMetadata._slots.ContainsKey(enumeratedSlotInfo.SlotIndex - segmentFirstGameplaySlotIndex))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000DC0 RID: 3520
		private const string kUnknownSlotName = "";

		// Token: 0x04000DC1 RID: 3521
		private SaveMetadataInfo _saveSlotMetadata = new SaveMetadataInfo();
	}
}
