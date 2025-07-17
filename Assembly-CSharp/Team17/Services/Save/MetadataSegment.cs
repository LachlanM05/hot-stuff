using System;

namespace Team17.Services.Save
{
	// Token: 0x020001D4 RID: 468
	public class MetadataSegment : JsonSegment
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00053FF2 File Offset: 0x000521F2
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x00053FFA File Offset: 0x000521FA
		public override uint Version { get; set; } = SaveSlotMetadata.Version.Latest;

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x00054003 File Offset: 0x00052203
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x0005400B File Offset: 0x0005220B
		public override string SegmentName { get; set; } = "Metadata";

		// Token: 0x06000F9C RID: 3996 RVA: 0x00054014 File Offset: 0x00052214
		protected override bool IsBase64VersionOrHigher(uint fileSaveVersion)
		{
			return fileSaveVersion >= 2U;
		}
	}
}
