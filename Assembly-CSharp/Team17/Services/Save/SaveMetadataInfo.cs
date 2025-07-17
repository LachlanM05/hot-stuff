using System;
using System.Collections.Generic;

namespace Team17.Services.Save
{
	// Token: 0x020001D8 RID: 472
	[Serializable]
	public class SaveMetadataInfo
	{
		// Token: 0x04000DBE RID: 3518
		public DateTime TimeStamp = DateTime.Now;

		// Token: 0x04000DBF RID: 3519
		public Dictionary<int, SaveSlotMetadata> _slots = new Dictionary<int, SaveSlotMetadata>();
	}
}
