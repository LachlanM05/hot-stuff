using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using T17.Services;
using Team17.Common;
using Team17.Platform.SaveGame;
using Team17.Platform.User;
using Team17.Scripts.Platforms.Enums;
using Team17.Services.Save.SaveMetaDataFactory;
using UnityEngine;

namespace Team17.Services.Save
{
	// Token: 0x020001DB RID: 475
	public class MirandaSaveGameService<TPlatformIOImpl> : IMirandaSaveGameService, IService where TPlatformIOImpl : IPlatformSaveGameIO, new()
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000FE6 RID: 4070 RVA: 0x00054838 File Offset: 0x00052A38
		// (remove) Token: 0x06000FE7 RID: 4071 RVA: 0x00054870 File Offset: 0x00052A70
		public event Action OnSaveBegin;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000FE8 RID: 4072 RVA: 0x000548A8 File Offset: 0x00052AA8
		// (remove) Token: 0x06000FE9 RID: 4073 RVA: 0x000548E0 File Offset: 0x00052AE0
		public event Action<SaveGameError> OnSaveCompleted;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000FEA RID: 4074 RVA: 0x00054918 File Offset: 0x00052B18
		// (remove) Token: 0x06000FEB RID: 4075 RVA: 0x00054950 File Offset: 0x00052B50
		public event Action<int> OnSlotUpdated;

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00054985 File Offset: 0x00052B85
		public bool IsBusy
		{
			get
			{
				return this._currentOperation > (MirandaSaveGameService<TPlatformIOImpl>.Operation)0;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x00054990 File Offset: 0x00052B90
		public int LastSavedGameplaySlotIndex
		{
			get
			{
				return this._lastSavedGameplaySlotIndex;
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00054998 File Offset: 0x00052B98
		public MirandaSaveGameService()
		{
			this._impl = new SaveGameService<TPlatformIOImpl>();
			this._metadataFactory = new SteamSaveMetaDataFactory();
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x000549F0 File Offset: 0x00052BF0
		public void OnStartUp()
		{
			Services.EngagementService.OnPrimaryUserEngagedEvent += this.OnPrimaryUserEngaged;
			IUser user;
			if (Services.UserService.TryGetPrimaryUser(out user))
			{
				this.InitialiseSaveSystem(user);
			}
			this._impl.OnStartUp();
			this.RegisterSaveSegment<SettingsSegment>(out this._settings);
			this.RegisterSaveSegment<GraphicsSettingsSegment>(out this._graphicsSettings);
			this.RegisterSaveSegment<MetadataSegment>(out this._metadata);
			this.RegisterSaveSegment<GameplaySegment>(out this._gameplay);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00054A63 File Offset: 0x00052C63
		public void OnCleanUp()
		{
			Services.EngagementService.OnPrimaryUserEngagedEvent -= this.OnPrimaryUserEngaged;
			this._impl.OnCleanUp();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00054A86 File Offset: 0x00052C86
		public void OnUpdate(float deltaTime)
		{
			this._impl.OnUpdate(deltaTime);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00054A94 File Offset: 0x00052C94
		public async Task<SaveResult<string>> LoadSettingsDataAsync()
		{
			return await this.LoadAsync(0, this._settings.Id, this._settings.Segment);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00054AD8 File Offset: 0x00052CD8
		public async Task<SaveGameError> SaveSettingsDataAsync(string json)
		{
			this._settings.Segment.SetDataToSave(json);
			return await this.SaveAsync(0, this._settings.Id, this._settings.Segment);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00054B24 File Offset: 0x00052D24
		public async Task<SaveResult<string>> LoadGraphicsSettingsDataAsync()
		{
			await Task.Yield();
			string @string = PlayerPrefs.GetString("MGSettings");
			SaveGameErrorType saveGameErrorType;
			if (string.IsNullOrWhiteSpace(@string))
			{
				saveGameErrorType = SaveGameErrorType.FileNotFound;
			}
			else
			{
				saveGameErrorType = SaveGameErrorType.None;
			}
			return new SaveResult<string>(in saveGameErrorType, in @string, GraphicsSettingsProvider.DataVersion.Latest);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00054B60 File Offset: 0x00052D60
		public async Task<SaveGameError> SaveGraphicsSettingsDataAsync(string json)
		{
			await Task.Yield();
			PlayerPrefs.SetString("MGSettings", json);
			return SaveGameError.None;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00054BA4 File Offset: 0x00052DA4
		public async Task<SaveGameError> LoadMetadataAsync(int slotIndex = 2147483647)
		{
			this.SetOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.loading);
			this._enumeratedSlotsResult.Clear();
			if (slotIndex == 2147483647)
			{
				int num = await this.GetLatestSlotIndex();
				slotIndex = num;
			}
			SaveGameErrorType saveGameErrorType;
			if (slotIndex != 2147483647)
			{
				SaveResult<string> saveResult = await this.LoadAsync(slotIndex, this._metadata.Id, this._metadata.Segment);
				this.SetOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.loading);
				if (saveResult.Error == SaveGameErrorType.None)
				{
					saveGameErrorType = this._saveSlotsMetadata.Deserialise(saveResult);
				}
				else
				{
					saveGameErrorType = saveResult.Error;
				}
			}
			else
			{
				saveGameErrorType = SaveGameErrorType.NotFound;
			}
			SaveGameError saveGameError;
			if (saveGameErrorType == SaveGameErrorType.FileVersionTooHigh)
			{
				this.ClearOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.loading);
				saveGameError = new SaveGameError(saveGameErrorType, slotIndex, -1, 0UL);
			}
			else
			{
				this._currentCombinedSaveGameSlot = slotIndex;
				if (this._currentCombinedSaveGameSlot == 2147483647 || saveGameErrorType != SaveGameErrorType.None)
				{
					if (this._currentCombinedSaveGameSlot == 2147483647 || this._currentCombinedSaveGameSlot < 2)
					{
						this._currentCombinedSaveGameSlot = (slotIndex = 2);
					}
					saveGameErrorType = await this.LoadCombinedSaveAndRebuildMetadata();
				}
				else
				{
					saveGameErrorType = await this.LoadCombinedSaveSlots();
				}
				if (saveGameErrorType == SaveGameErrorType.None)
				{
					this.AdvanceCurrentCombinedSaveSlot();
				}
				this.ClearOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.loading);
				saveGameError = new SaveGameError(saveGameErrorType, slotIndex, -1, 0UL);
			}
			return saveGameError;
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00054BF0 File Offset: 0x00052DF0
		private async Task<int> GetLatestSlotIndex()
		{
			this._enumeratedSlotsResult.Clear();
			await this.EnumerateSlotsAsync();
			EnumeratedSlotInfo enumeratedSlotInfo = null;
			for (int i = 0; i < this._enumeratedSlotsResult.Count; i++)
			{
				EnumeratedSlotInfo enumeratedSlotInfo2 = this._enumeratedSlotsResult[i];
				if (this.IsSlotAGameplaySlot(enumeratedSlotInfo2.SlotIndex))
				{
					if (enumeratedSlotInfo == null)
					{
						enumeratedSlotInfo = enumeratedSlotInfo2;
					}
					else if (DateTime.Compare(enumeratedSlotInfo2.LastAccessTime, enumeratedSlotInfo.LastAccessTime) > 0)
					{
						enumeratedSlotInfo = enumeratedSlotInfo2;
					}
				}
			}
			int num;
			if (enumeratedSlotInfo == null)
			{
				num = int.MaxValue;
			}
			else
			{
				num = enumeratedSlotInfo.SlotIndex;
			}
			return num;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00054C34 File Offset: 0x00052E34
		private async Task<SaveGameErrorType> LoadCombinedSaveAndRebuildMetadata()
		{
			this._saveSlotsMetadata.ClearAll();
			SaveGameErrorType saveGameErrorType = await this.LoadCombinedSaveSlots();
			if (saveGameErrorType == SaveGameErrorType.None)
			{
				this._saveSlotsMetadata.RebuildMetadata(this._combinedSaveSlots);
				saveGameErrorType = (await this.SaveMetaData()).ErrorType;
			}
			return saveGameErrorType;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00054C77 File Offset: 0x00052E77
		public SaveSlotMetadata GetSlotInfo(int slotNumber)
		{
			return this._saveSlotsMetadata.GetSlotData(slotNumber);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00054C85 File Offset: 0x00052E85
		public SaveSlotMetadata GetLatestSlotInfo()
		{
			return this._saveSlotsMetadata.GetLatestSlotData();
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00054C92 File Offset: 0x00052E92
		public int GetNumberSaveSlotsInUse()
		{
			return this._saveSlotsMetadata.Count;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00054C9F File Offset: 0x00052E9F
		public DateTime GetSaveFileTimeStamp()
		{
			return this._saveSlotsMetadata.TimeStamp;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00054CAC File Offset: 0x00052EAC
		public async Task<SaveGameError> SetSlotMetadataAsync(int saveSlotId, Save.SaveData slotData)
		{
			this._saveSlotsMetadata.SetSlotData(saveSlotId, slotData);
			SaveGameError saveGameError = await this.SaveMetaData();
			Action<int> onSlotUpdated = this.OnSlotUpdated;
			if (onSlotUpdated != null)
			{
				onSlotUpdated(saveSlotId);
			}
			return saveGameError;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00054D00 File Offset: 0x00052F00
		protected async Task<SaveGameError> SaveMetaData()
		{
			this._metadata.Segment.SetDataToSave(this._saveSlotsMetadata.Serialise());
			return await this.SaveAsync(this._currentCombinedSaveGameSlot, this._metadata.Id, this._metadata.Segment);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00054D44 File Offset: 0x00052F44
		public async Task<SaveResult<bool>> DoesGameplaySlotExistAsync(int slotIndex)
		{
			SaveResult<bool> saveResult;
			if (this._combinedSaveSlots.IsSlotValid(slotIndex))
			{
				bool flag = true;
				saveResult = SaveResult.FromResult<bool>(in flag, uint.MaxValue);
			}
			else
			{
				bool flag = false;
				saveResult = SaveResult.FromResult<bool>(in flag, uint.MaxValue);
			}
			return saveResult;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00054D90 File Offset: 0x00052F90
		public async Task<SaveResult<string>> LoadGameplayDataAsync(int slotIndex)
		{
			string slotData = this._combinedSaveSlots.GetSlotData(slotIndex);
			return SaveResult.FromResult<string>(in slotData, this._gameplay.Segment.DeserialisedVersion);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00054DDC File Offset: 0x00052FDC
		public async Task<SaveGameError> SaveGameplayDataAsync(int slotIndex, Save.SaveData saveData)
		{
			string text = saveData.SaveToString();
			this._combinedSaveSlots.SetSlotData(slotIndex, text);
			this._saveSlotsMetadata.SetSlotData(slotIndex, saveData);
			this._gameplay.Segment.SetDataToSave(this._combinedSaveSlots.ToSerializedCharArray());
			this._metadata.Segment.SetDataToSave(this._saveSlotsMetadata.Serialise());
			this._lastSavedGameplaySlotIndex = slotIndex;
			int[] array = new int[]
			{
				this._gameplay.Id,
				this._metadata.Id
			};
			SaveGameError saveGameError = await this.SaveBatchedAsync(this._currentCombinedSaveGameSlot, array);
			Action<int> onSlotUpdated = this.OnSlotUpdated;
			if (onSlotUpdated != null)
			{
				onSlotUpdated(slotIndex);
			}
			this.AdvanceCurrentCombinedSaveSlot();
			return saveGameError;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00054E30 File Offset: 0x00053030
		public async Task<SaveGameError> DeleteGameplayDataAsync(int slotIndex)
		{
			this._combinedSaveSlots.DeleteSlotData(slotIndex);
			this._gameplay.Segment.SetDataToSave(this._combinedSaveSlots.ToSerializedCharArray());
			this._lastSavedGameplaySlotIndex = slotIndex;
			SaveGameError saveGameError = await this.SaveAsync(this._currentCombinedSaveGameSlot, this._gameplay.Id, this._gameplay.Segment);
			SaveGameError gameplaySaveResult = saveGameError;
			this._saveSlotsMetadata.RemoveSlotData(slotIndex);
			await this.SaveMetaData();
			Action<int> onSlotUpdated = this.OnSlotUpdated;
			if (onSlotUpdated != null)
			{
				onSlotUpdated(slotIndex);
			}
			return gameplaySaveResult;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00054E7C File Offset: 0x0005307C
		private async Task<SaveResult<string>> LoadAsync(int slotIndex, int segmentId, JsonSegment segment)
		{
			this.SetOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.loading);
			TaskAwaiter<SaveResult<bool>> taskAwaiter = this.SlotExistsAsync(slotIndex).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<SaveResult<bool>> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<SaveResult<bool>>);
			}
			SaveResult<string> saveResult;
			if (!taskAwaiter.GetResult().Result)
			{
				SaveGameErrorType saveGameErrorType = SaveGameErrorType.FileNotFound;
				saveResult = SaveResult.FromError<string>(in saveGameErrorType);
			}
			else
			{
				await this.WaitUntilSaveIONotBusy();
				SaveGameError saveGameError = await this.OpenSlotAsync(slotIndex, false);
				SaveResult<string> saveResult2;
				if (!this.IsSuccess<string>(saveGameError, out saveResult2))
				{
					saveResult = saveResult2;
				}
				else if (!this._impl.Load(segmentId) && !this.IsSuccess<string>(this._impl.GetError(), out saveResult2))
				{
					saveResult = saveResult2;
				}
				else
				{
					this._impl.CloseSlot();
					await this.WaitUntilSaveIONotBusy();
					if (!this.IsSuccess<string>(this._impl.GetError(), out saveResult2))
					{
						saveResult = saveResult2;
					}
					else
					{
						this.ClearOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.loading);
						string jsonString = segment.JsonString;
						saveResult = SaveResult.FromResult<string>(in jsonString, segment.DeserialisedVersion);
					}
				}
			}
			return saveResult;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00054ED8 File Offset: 0x000530D8
		public async Task<SaveGameError> SaveAsync(int slotIndex, int segmentId, JsonSegment segment)
		{
			this.SetOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.saving);
			Action onSaveBegin = this.OnSaveBegin;
			if (onSaveBegin != null)
			{
				onSaveBegin();
			}
			await this.WaitUntilSaveIONotBusy();
			SaveGameError saveGameError = await this.OpenSlotAsync(slotIndex, true);
			SaveGameError saveGameError2;
			if (saveGameError.ErrorType != SaveGameErrorType.None)
			{
				saveGameError2 = saveGameError;
			}
			else
			{
				if (!this._impl.Save(segmentId))
				{
					saveGameError = this._impl.GetError();
					if (saveGameError.ErrorType != SaveGameErrorType.None)
					{
						this._impl.CloseSlot();
						return saveGameError;
					}
				}
				this._impl.CloseSlot();
				await this.WaitUntilSaveIONotBusy();
				GC.Collect();
				saveGameError = this._impl.GetError();
				this.ClearOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.saving);
				Action<SaveGameError> onSaveCompleted = this.OnSaveCompleted;
				if (onSaveCompleted != null)
				{
					onSaveCompleted(saveGameError);
				}
				saveGameError2 = saveGameError;
			}
			return saveGameError2;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00054F2C File Offset: 0x0005312C
		public async Task<SaveGameError> SaveBatchedAsync(int slotIndex, int[] segmentIds)
		{
			this.SetOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.saving);
			Action onSaveBegin = this.OnSaveBegin;
			if (onSaveBegin != null)
			{
				onSaveBegin();
			}
			await this.WaitUntilSaveIONotBusy();
			SaveGameError result = await this.OpenSlotAsync(slotIndex, true);
			SaveGameError saveGameError;
			if (result.ErrorType != SaveGameErrorType.None)
			{
				saveGameError = result;
			}
			else
			{
				for (int i = 0; i < segmentIds.Length; i++)
				{
					if (!this._impl.Save(segmentIds[i]))
					{
						result = this._impl.GetError();
						if (result.ErrorType != SaveGameErrorType.None)
						{
							this._impl.CloseSlot();
							return result;
						}
					}
				}
				this._impl.CloseSlot();
				await this.WaitUntilSaveIONotBusy();
				result = this._impl.GetError();
				this.ClearOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.saving);
				if (result.ErrorType == SaveGameErrorType.FileCorrupted)
				{
					SaveGameErrorType errorType = (await this.DeleteSlotAsync(slotIndex)).ErrorType;
				}
				Action<SaveGameError> onSaveCompleted = this.OnSaveCompleted;
				if (onSaveCompleted != null)
				{
					onSaveCompleted(result);
				}
				saveGameError = result;
			}
			return saveGameError;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00054F80 File Offset: 0x00053180
		public async Task<SaveGameError> DeleteSlotAsync(int slotIndex)
		{
			this.SetOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.deleting);
			await this.WaitUntilSaveIONotBusy();
			this._impl.DeleteSlot(slotIndex);
			await this.WaitUntilSaveIONotBusy();
			this.ClearOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation.deleting);
			return this._impl.GetError();
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00054FCC File Offset: 0x000531CC
		public async Task<SaveResult<bool>> SlotExistsAsync(int slotIndex)
		{
			await this.WaitUntilSaveIONotBusy();
			SaveResult<bool> saveResult;
			if (!this._impl.SlotExists(slotIndex))
			{
				bool flag = false;
				saveResult = SaveResult.FromResult<bool>(in flag, uint.MaxValue);
			}
			else
			{
				await this.WaitUntilSaveIONotBusy();
				SaveGameError error = this._impl.GetError();
				SaveGameErrorType errorType = error.ErrorType;
				if (errorType != SaveGameErrorType.None)
				{
					if (errorType != SaveGameErrorType.FileNotFound && errorType != SaveGameErrorType.NotFound)
					{
						SaveGameErrorType errorType2 = error.ErrorType;
						saveResult = SaveResult.FromError<bool>(in errorType2);
					}
					else
					{
						bool flag = false;
						saveResult = SaveResult.FromResult<bool>(in flag, uint.MaxValue);
					}
				}
				else
				{
					bool flag = true;
					saveResult = SaveResult.FromResult<bool>(in flag, uint.MaxValue);
				}
			}
			return saveResult;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00055018 File Offset: 0x00053218
		private async Task<SaveGameError> EnumerateSlotsAsync()
		{
			SaveGameError saveGameError;
			if (!this._impl.EnumerateSlots(ref this._enumeratedSlotsResult))
			{
				saveGameError = new SaveGameError(SaveGameErrorType.Busy, -1, -1, 0UL);
			}
			else
			{
				await this.WaitUntilSaveIONotBusy();
				if (Services.PlatformService.HasFlag(EPlatformFlags.DoesntSupportFileTimestamps))
				{
					int i = 0;
					int slotIndex = 2;
					while (i < 3)
					{
						int j = 0;
						while (j < this._enumeratedSlotsResult.Count)
						{
							if (this._enumeratedSlotsResult[j].SlotIndex == slotIndex)
							{
								TaskAwaiter<SaveGameError> taskAwaiter = this.OpenSlotAsync(slotIndex, false).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									await taskAwaiter;
									TaskAwaiter<SaveGameError> taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<SaveGameError>);
								}
								if (taskAwaiter.GetResult().ErrorType == SaveGameErrorType.None)
								{
									this._impl.CloseSlot();
									await this.WaitUntilSaveIONotBusy();
									DateTime headerTimestamp = this._impl.GetHeaderTimestamp();
									this._enumeratedSlotsResult[j].LastAccessTime = headerTimestamp;
									break;
								}
								break;
							}
							else
							{
								j++;
							}
						}
						i++;
						slotIndex++;
					}
				}
				foreach (EnumeratedSlotInfo enumeratedSlotInfo in this._enumeratedSlotsResult)
				{
				}
				saveGameError = this._impl.GetGlobalError();
			}
			return saveGameError;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0005505C File Offset: 0x0005325C
		private async Task<SaveGameError> OpenSlotAsync(int slotIndex, bool forWrite)
		{
			while (!this._impl.OpenSlot(slotIndex, forWrite))
			{
				SaveGameError error = this._impl.GetError();
				if (error.ErrorType != SaveGameErrorType.None)
				{
					return error;
				}
				await Task.Yield();
			}
			return this._impl.GetError();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x000550B0 File Offset: 0x000532B0
		private async Task WaitUntilSaveIONotBusy()
		{
			while (this._impl.IsBusy())
			{
				await Task.Yield();
			}
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x000550F4 File Offset: 0x000532F4
		private void RegisterSaveSegment<T>(out RegisteredSegment<T> registerdSegment) where T : ISaveSegment, new()
		{
			T t = new T();
			int num = this._impl.RegisterSegment<T>(t);
			registerdSegment = new RegisteredSegment<T>(t, num);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00055120 File Offset: 0x00053320
		private bool IsSuccess<T>(SaveGameError result, out SaveResult<T> saveResult)
		{
			if (result.ErrorType == SaveGameErrorType.None)
			{
				saveResult = default(SaveResult<T>);
				return true;
			}
			SaveGameErrorType errorType = result.ErrorType;
			saveResult = SaveResult.FromError<T>(in errorType);
			return false;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00055158 File Offset: 0x00053358
		private void OnPrimaryUserEngaged()
		{
			IUser user;
			if (Services.UserService.TryGetPrimaryUser(out user))
			{
				this.InitialiseSaveSystem(user);
			}
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0005517A File Offset: 0x0005337A
		private void InitialiseSaveSystem(IUser user)
		{
			this._impl.Initialise(user, this._metadataFactory.Create());
			this.ClearAllOperationFlags();
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00055199 File Offset: 0x00053399
		private void SetOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation flag)
		{
			this._currentOperation |= flag;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x000551A9 File Offset: 0x000533A9
		private void ClearOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation flag)
		{
			this._currentOperation &= ~flag;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000551BA File Offset: 0x000533BA
		private void ClearAllOperationFlags()
		{
			this._currentOperation = (MirandaSaveGameService<TPlatformIOImpl>.Operation)0;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000551C3 File Offset: 0x000533C3
		private bool HasOperationFlag(MirandaSaveGameService<TPlatformIOImpl>.Operation flag)
		{
			return this._currentOperation.HasFlag(flag);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000551DB File Offset: 0x000533DB
		public bool IsSlotAGameplaySlot(int slotIndex)
		{
			return slotIndex >= 2 && slotIndex < Save.MAX_SAVE_SLOTS + 2;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000551F0 File Offset: 0x000533F0
		private async Task<SaveGameErrorType> LoadCombinedSaveSlots()
		{
			SaveResult<string> saveResult = await this.LoadAsync(this._currentCombinedSaveGameSlot, this._gameplay.Id, this._gameplay.Segment);
			SaveGameErrorType saveGameErrorType;
			if (!saveResult.IsSuccess())
			{
				this._gameplay.Segment.ClearJson();
				saveGameErrorType = saveResult.Error;
			}
			else if (saveResult.Version > this._gameplay.Segment.Version)
			{
				this._gameplay.Segment.ClearJson();
				saveGameErrorType = SaveGameErrorType.FileVersionTooHigh;
			}
			else
			{
				uint version = saveResult.Version;
				uint version2 = this._gameplay.Segment.Version;
				bool flag = this._combinedSaveSlots.FromSerialized(this._gameplay.Segment, saveResult.Version);
				this._gameplay.Segment.ClearJson();
				saveGameErrorType = ((!flag) ? SaveGameErrorType.Broken : SaveGameErrorType.None);
			}
			return saveGameErrorType;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00055233 File Offset: 0x00053433
		private void AdvanceCurrentCombinedSaveSlot()
		{
			this._currentCombinedSaveGameSlot++;
			if (this._currentCombinedSaveGameSlot >= 2 + this.GetTotalPlatformSlots())
			{
				this._currentCombinedSaveGameSlot = 2;
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0005525C File Offset: 0x0005345C
		public async Task ClearAllSaveData()
		{
			this.ClearAllOperationFlags();
			this._combinedSaveSlots.ClearAllData();
			this._saveSlotsMetadata.ClearAll();
			int i = 2;
			int count = 2 + this.GetTotalPlatformSlots();
			while (i < count)
			{
				await this.DeleteSlotAsync(i);
				i++;
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x000552A0 File Offset: 0x000534A0
		public async Task<SaveGameError> ForceSaveCurrentCombinedSaveFileAsync()
		{
			this._gameplay.Segment.SetDataToSave(this._combinedSaveSlots.ToSerializedCharArray());
			this._lastSavedGameplaySlotIndex = -1;
			SaveGameError saveGameError = await this.SaveAsync(this._currentCombinedSaveGameSlot, this._gameplay.Id, this._gameplay.Segment);
			if (saveGameError.ErrorType == SaveGameErrorType.None)
			{
				saveGameError = await this.SaveMetaData();
			}
			return saveGameError;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x000552E3 File Offset: 0x000534E3
		protected SaveResult<T> ForceErrorForTesting<T>(SaveResult<T> saveResult, SaveGameErrorType errorType, uint version = 4294967295U)
		{
			return new SaveResult<T>(in errorType, in saveResult.Result, (version == uint.MaxValue) ? saveResult.Version : version);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00055300 File Offset: 0x00053500
		protected SaveResult<T> ForceVersionForTesting<T>(SaveResult<T> saveResult, uint version)
		{
			return new SaveResult<T>(in saveResult.Error, in saveResult.Result, version);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00055316 File Offset: 0x00053516
		public int GetTotalPlatformSlots()
		{
			if (Services.PlatformService.HasFlag(EPlatformFlags.LimitedPlatformSaveSlots))
			{
				return 1;
			}
			return 3;
		}

		// Token: 0x04000DCA RID: 3530
		private ISaveGameService _impl;

		// Token: 0x04000DCB RID: 3531
		private ISaveMetaDataFactory _metadataFactory;

		// Token: 0x04000DCC RID: 3532
		private RegisteredSegment<SettingsSegment> _settings;

		// Token: 0x04000DCD RID: 3533
		private RegisteredSegment<GraphicsSettingsSegment> _graphicsSettings;

		// Token: 0x04000DCE RID: 3534
		private RegisteredSegment<MetadataSegment> _metadata;

		// Token: 0x04000DCF RID: 3535
		private RegisteredSegment<GameplaySegment> _gameplay;

		// Token: 0x04000DD0 RID: 3536
		private SaveSlotMetadataProvider _saveSlotsMetadata = new SaveSlotMetadataProvider();

		// Token: 0x04000DD1 RID: 3537
		private CombinedSaveSlotsContainer _combinedSaveSlots = new CombinedSaveSlotsContainer();

		// Token: 0x04000DD2 RID: 3538
		private MirandaSaveGameService<TPlatformIOImpl>.Operation _currentOperation;

		// Token: 0x04000DD3 RID: 3539
		private List<EnumeratedSlotInfo> _enumeratedSlotsResult = new List<EnumeratedSlotInfo>();

		// Token: 0x04000DD4 RID: 3540
		private int _currentCombinedSaveGameSlot = 2;

		// Token: 0x04000DD5 RID: 3541
		private int _lastSavedGameplaySlotIndex = -1;

		// Token: 0x04000DD6 RID: 3542
		private const string playerPrefKey = "MGSettings";

		// Token: 0x0200039D RID: 925
		[Flags]
		public enum Operation
		{
			// Token: 0x04001440 RID: 5184
			loading = 1,
			// Token: 0x04001441 RID: 5185
			saving = 2,
			// Token: 0x04001442 RID: 5186
			deleting = 4
		}
	}
}
