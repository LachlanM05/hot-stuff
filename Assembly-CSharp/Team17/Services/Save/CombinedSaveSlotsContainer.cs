using System;
using System.Buffers;
using System.Collections.Generic;
using SaveExtensions;
using Team17.Common;
using UnityEngine;

namespace Team17.Services.Save
{
	// Token: 0x020001CF RID: 463
	public class CombinedSaveSlotsContainer
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x0005363C File Offset: 0x0005183C
		// (set) Token: 0x06000F59 RID: 3929 RVA: 0x00053644 File Offset: 0x00051844
		public bool HasLoadedData { get; private set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0005364D File Offset: 0x0005184D
		public int NumberOfSlots
		{
			get
			{
				return this._SlotData.Length;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x00053657 File Offset: 0x00051857
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x0005365F File Offset: 0x0005185F
		public uint SlotDataVersion { get; private set; }

		// Token: 0x06000F5D RID: 3933 RVA: 0x00053668 File Offset: 0x00051868
		public bool IsSlotValid(int slotIndex)
		{
			return this.HasLoadedData && slotIndex < this._SlotData.Length && this._SlotData[slotIndex] != null && this._SlotData[slotIndex].Count > 0;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0005369C File Offset: 0x0005189C
		public void SetSlotData(int slotIndex, string jsonData)
		{
			if (slotIndex >= this._SlotData.Length)
			{
				return;
			}
			byte[] array = Compression.CompressString(jsonData);
			this._SlotData[slotIndex] = new FastList<byte>(array);
			this.HasLoadedData = true;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x000536D4 File Offset: 0x000518D4
		public string GetSlotData(int slotIndex)
		{
			if (slotIndex >= this._SlotData.Length)
			{
				return string.Empty;
			}
			if (this._SlotData[slotIndex] == null)
			{
				return string.Empty;
			}
			return Compression.DecompressString(this._SlotData[slotIndex]._items, this._SlotData[slotIndex].Count);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00053721 File Offset: 0x00051921
		public void DeleteSlotData(int slotIndex)
		{
			if (slotIndex >= this._SlotData.Length)
			{
				return;
			}
			this._SlotData[slotIndex] = new FastList<byte>();
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0005373C File Offset: 0x0005193C
		public ArraySegment<char> ToSerializedCharArray()
		{
			this.SlotDataVersion = Save.DataVersion.Latest;
			int num = this.CalculateListsSizeAsBase64();
			char[] array = ArrayPool<char>.Shared.Rent(Math.Max(16384000, num));
			try
			{
				int num2 = 0;
				for (int i = 0; i < this._SlotData.Length; i++)
				{
					if (i > 0)
					{
						array[num2++] = '|';
					}
					if (this._SlotData[i] == null)
					{
						this._SlotData[i] = new FastList<byte>();
					}
					num2 += this._SlotData[i].ToBase64Span(array, num2).Length;
				}
				return new ArraySegment<char>(array, 0, num2);
			}
			catch (Exception ex)
			{
				T17Debug.LogError(string.Format("[CombinedSaveSlotsContainer] Exception during serialization - {0}", ex));
			}
			return Array.Empty<char>();
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00053804 File Offset: 0x00051A04
		private int CalculateListsSizeAsBase64()
		{
			int num = 0;
			for (int i = 0; i < this._SlotData.Length; i++)
			{
				if (this._SlotData[i] != null)
				{
					num += this._SlotData[i].CalculateBase64Size();
				}
				if (i < this._SlotData.Length - 1)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00053852 File Offset: 0x00051A52
		public bool FromSerialized(JsonSegment jsonSegment, uint resultVersion)
		{
			if (resultVersion < 3U)
			{
				return this.FromSerializedString(jsonSegment.JsonString, resultVersion);
			}
			return this.FromSerializedCharArray(jsonSegment.JsonBuffer, resultVersion);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00053874 File Offset: 0x00051A74
		public unsafe bool FromSerializedCharArray(ArraySegment<char> serializedData, uint version)
		{
			this.ClearAllData();
			this.SlotDataVersion = version;
			try
			{
				if (version < 3U)
				{
					throw new ArgumentException("Invalid version for buffer decoding");
				}
				ReadOnlySpan<char> readOnlySpan = serializedData;
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					if (*readOnlySpan[i] == 124)
					{
						this.ProcessBuffer(readOnlySpan.Slice(num2, i - num2), num, version);
						num2 = i + 1;
						num++;
						if (num >= Save.MAX_SAVE_SLOTS)
						{
							break;
						}
					}
				}
				if (num2 < readOnlySpan.Length && num < Save.MAX_SAVE_SLOTS)
				{
					this.ProcessBuffer(readOnlySpan.Slice(num2), num, version);
				}
				this.HasLoadedData = true;
			}
			catch (Exception ex)
			{
				T17Debug.LogError(string.Format("[CombinedSaveSlotsContainer] Exception parsing json savedata - {0}", ex));
			}
			return this.HasLoadedData;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00053940 File Offset: 0x00051B40
		public bool FromSerializedString(string serialisedData, uint version)
		{
			this.ClearAllData();
			this.SlotDataVersion = version;
			try
			{
				if (version >= 3U)
				{
					throw new ArgumentException("Invalid version for string decoding");
				}
				CombinedSegmentData combinedSegmentData = JsonUtility.FromJson<CombinedSegmentData>(serialisedData);
				int num = 0;
				int num2 = combinedSegmentData.buffers.Length;
				while (num < num2 && num < Save.MAX_SAVE_SLOTS)
				{
					this._SlotData[num] = combinedSegmentData.buffers[num];
					num++;
				}
				this.HasLoadedData = true;
			}
			catch (Exception ex)
			{
				T17Debug.LogError(string.Format("[CombinedSaveSlotsContainer] Exception parsing json savedata - {0}", ex));
			}
			return this.HasLoadedData;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x000539D0 File Offset: 0x00051BD0
		private void ProcessBuffer(ReadOnlySpan<char> bufferSpan, int index, uint version)
		{
			if (version < 3U)
			{
				throw new ArgumentException("Invalid version for buffer decoding");
			}
			this._SlotData[index] = FastListExtensions.FromBase64Span(bufferSpan);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x000539EF File Offset: 0x00051BEF
		public void ClearAllData()
		{
			this._SlotData = new FastList<byte>[Save.MAX_SAVE_SLOTS];
			this.HasLoadedData = false;
		}

		// Token: 0x04000DA2 RID: 3490
		private FastList<byte>[] _SlotData = new FastList<byte>[Save.MAX_SAVE_SLOTS];
	}
}
