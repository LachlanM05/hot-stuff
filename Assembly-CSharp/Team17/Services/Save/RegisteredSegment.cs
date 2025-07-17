using System;
using Team17.Platform.SaveGame;

namespace Team17.Services.Save
{
	// Token: 0x020001D5 RID: 469
	public class RegisteredSegment<T> where T : ISaveSegment
	{
		// Token: 0x06000F9E RID: 3998 RVA: 0x0005403B File Offset: 0x0005223B
		public RegisteredSegment(T segment, int id)
		{
			this.Segment = segment;
			this.Id = id;
		}

		// Token: 0x04000DAF RID: 3503
		public readonly T Segment;

		// Token: 0x04000DB0 RID: 3504
		public readonly int Id;
	}
}
