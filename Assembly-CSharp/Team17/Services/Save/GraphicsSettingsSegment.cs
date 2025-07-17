using System;

namespace Team17.Services.Save
{
	// Token: 0x020001D2 RID: 466
	public class GraphicsSettingsSegment : JsonSegment
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x00053CB1 File Offset: 0x00051EB1
		// (set) Token: 0x06000F80 RID: 3968 RVA: 0x00053CB9 File Offset: 0x00051EB9
		public override uint Version { get; set; } = GraphicsSettingsProvider.DataVersion.Latest;

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x00053CC2 File Offset: 0x00051EC2
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x00053CCA File Offset: 0x00051ECA
		public override string SegmentName { get; set; } = "GraphicsSettings";

		// Token: 0x06000F83 RID: 3971 RVA: 0x00053CD3 File Offset: 0x00051ED3
		protected override bool IsBase64VersionOrHigher(uint fileSaveVersion)
		{
			return fileSaveVersion >= 1U;
		}
	}
}
