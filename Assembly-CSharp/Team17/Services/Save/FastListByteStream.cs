using System;
using System.Collections.Generic;
using System.IO;

namespace Team17.Services.Save
{
	// Token: 0x020001D0 RID: 464
	public class FastListByteStream : Stream
	{
		// Token: 0x06000F69 RID: 3945 RVA: 0x00053A20 File Offset: 0x00051C20
		public FastListByteStream(FastList<byte> list)
		{
			this._list = list;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x00053A2F File Offset: 0x00051C2F
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x00053A32 File Offset: 0x00051C32
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00053A35 File Offset: 0x00051C35
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x00053A38 File Offset: 0x00051C38
		public override long Length
		{
			get
			{
				return (long)this._list.Count;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x00053A46 File Offset: 0x00051C46
		// (set) Token: 0x06000F6F RID: 3951 RVA: 0x00053A4E File Offset: 0x00051C4E
		public override long Position { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x00053A57 File Offset: 0x00051C57
		private bool IsPositionAtEndOfList
		{
			get
			{
				return this.Position == this.Length;
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00053A67 File Offset: 0x00051C67
		public override void Flush()
		{
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00053A6C File Offset: 0x00051C6C
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this.CalculateSafeReadCount(count);
			this._list.CopyTo((int)this.Position, buffer, offset, num);
			this.Position += (long)num;
			return num;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00053AA6 File Offset: 0x00051CA6
		private int CalculateSafeReadCount(int desiredCount)
		{
			return Math.Min((int)(this.Length - this.Position), desiredCount);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00053ABC File Offset: 0x00051CBC
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this.Position + offset;
				break;
			case SeekOrigin.End:
				num = this.Length - offset;
				break;
			default:
				num = this.Position;
				break;
			}
			this.Position = num;
			return this.Position;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00053B0B File Offset: 0x00051D0B
		public void EnsureCapacity(int capacity)
		{
			if (this._list.Capacity >= capacity)
			{
				return;
			}
			this._list.Capacity = capacity;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00053B28 File Offset: 0x00051D28
		public override void SetLength(long value)
		{
			this._list.Capacity = (int)value;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00053B38 File Offset: 0x00051D38
		public override void Write(byte[] srcBuffer, int offset, int count)
		{
			if (this.IsPositionAtEndOfList)
			{
				if (offset == 0 && count == srcBuffer.Length)
				{
					this._list.AddRange(srcBuffer);
				}
				else
				{
					ArraySegment<byte> arraySegment = new ArraySegment<byte>(srcBuffer, offset, count);
					this._list.AddRange(arraySegment);
				}
				this.Position = this.Length;
				return;
			}
			byte[] items = this._list._items;
			long num = 0L;
			long num2 = this.Position;
			long num3 = (long)offset;
			while (num < (long)count)
			{
				if (this.Position >= this.Length)
				{
					int num4 = srcBuffer.Length - (int)num3;
					ArraySegment<byte> arraySegment2 = new ArraySegment<byte>(srcBuffer, (int)num3, num4);
					this._list.AddRange(arraySegment2);
					this.Position = this.Length;
					return;
				}
				checked
				{
					items[(int)((IntPtr)num2)] = srcBuffer[(int)((IntPtr)num3)];
				}
				num2 += 1L;
				num3 += 1L;
				long position = this.Position;
				this.Position = position + 1L;
				num += 1L;
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00053C20 File Offset: 0x00051E20
		private void WriteByteToList(byte data)
		{
			if (this.IsPositionAtEndOfList)
			{
				this._list.Add(data);
			}
			else
			{
				this._list._items[(int)(checked((IntPtr)this.Position))] = data;
			}
			long num = this.Position + 1L;
			this.Position = num;
		}

		// Token: 0x04000DA3 RID: 3491
		private readonly FastList<byte> _list;
	}
}
