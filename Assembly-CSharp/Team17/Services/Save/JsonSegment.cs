using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using Team17.Common;
using Team17.Platform.SaveGame;

namespace Team17.Services.Save
{
	// Token: 0x020001D3 RID: 467
	public abstract class JsonSegment : ISaveSegment
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x00053CFA File Offset: 0x00051EFA
		// (set) Token: 0x06000F86 RID: 3974 RVA: 0x00053D02 File Offset: 0x00051F02
		public FastList<byte> Buffer { get; set; } = new FastList<byte>();

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000F87 RID: 3975
		// (set) Token: 0x06000F88 RID: 3976
		public abstract uint Version { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000F89 RID: 3977
		// (set) Token: 0x06000F8A RID: 3978
		public abstract string SegmentName { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00053D0B File Offset: 0x00051F0B
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x00053D13 File Offset: 0x00051F13
		public uint DeserialisedVersion { get; protected set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x00053D1C File Offset: 0x00051F1C
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x00053D24 File Offset: 0x00051F24
		public string JsonString
		{
			get
			{
				return this._jsonString;
			}
			private set
			{
				this.ClearJson();
				this._jsonString = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x00053D33 File Offset: 0x00051F33
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x00053D3B File Offset: 0x00051F3B
		public ArraySegment<char> JsonBuffer
		{
			get
			{
				return this._jsonBuffer;
			}
			private set
			{
				this.ClearJson();
				this._jsonBuffer = value;
			}
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00053D4A File Offset: 0x00051F4A
		public void SetDataToSave(string json)
		{
			this.JsonString = json;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00053D53 File Offset: 0x00051F53
		public void SetDataToSave(ArraySegment<char> jsonBuffer)
		{
			this.JsonBuffer = jsonBuffer;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00053D5C File Offset: 0x00051F5C
		public void ClearJson()
		{
			this._jsonString = string.Empty;
			this._jsonBuffer = Array.Empty<char>();
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00053D7C File Offset: 0x00051F7C
		public virtual void Serialise()
		{
			using (FastListByteStream fastListByteStream = new FastListByteStream(this.Buffer))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fastListByteStream))
				{
					try
					{
						if (!string.IsNullOrEmpty(this.JsonString))
						{
							binaryWriter.Write(true);
							binaryWriter.Write(this.JsonString);
						}
						else
						{
							binaryWriter.Write(false);
							binaryWriter.Write(this._jsonBuffer.Count);
							binaryWriter.Write(this.JsonBuffer.Array, this.JsonBuffer.Offset, this.JsonBuffer.Count);
						}
					}
					catch (Exception ex)
					{
						T17Debug.LogError("Exception serialising JsonSegment : " + ex.ToString());
					}
					finally
					{
						if (this.JsonBuffer != null && this.JsonBuffer.Count > 0)
						{
							ArrayPool<char>.Shared.Return(this.JsonBuffer.Array, false);
						}
						this.JsonBuffer = Array.Empty<char>();
						this.JsonString = string.Empty;
					}
				}
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00053EC8 File Offset: 0x000520C8
		public virtual void Deserialise(uint fileSaveVersion)
		{
			if (this.Buffer == null || this.Buffer.Count == 0)
			{
				this.JsonString = string.Empty;
				this.JsonBuffer = Array.Empty<char>();
				return;
			}
			using (FastListByteStream fastListByteStream = new FastListByteStream(this.Buffer))
			{
				using (BinaryReader binaryReader = new BinaryReader(fastListByteStream))
				{
					try
					{
						if (!this.IsBase64VersionOrHigher(fileSaveVersion))
						{
							this.JsonString = binaryReader.ReadString();
						}
						else
						{
							this.ClearJson();
							if (binaryReader.ReadBoolean())
							{
								this.JsonString = binaryReader.ReadString();
							}
							else
							{
								int num = binaryReader.ReadInt32();
								this.JsonBuffer = new ArraySegment<char>(binaryReader.ReadChars(num));
							}
						}
						this.DeserialisedVersion = fileSaveVersion;
					}
					catch (Exception ex)
					{
						T17Debug.LogError("Exception deserialising JsonSegment : " + ex.ToString());
					}
				}
			}
		}

		// Token: 0x06000F96 RID: 3990
		protected abstract bool IsBase64VersionOrHigher(uint fileSaveVersion);

		// Token: 0x04000DA9 RID: 3497
		private ArraySegment<char> _jsonBuffer = Array.Empty<char>();

		// Token: 0x04000DAA RID: 3498
		private string _jsonString = string.Empty;
	}
}
