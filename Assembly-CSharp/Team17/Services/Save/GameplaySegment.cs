using System;

namespace Team17.Services.Save
{
	// Token: 0x020001D1 RID: 465
	public class GameplaySegment : JsonSegment
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x00053C68 File Offset: 0x00051E68
		// (set) Token: 0x06000F7A RID: 3962 RVA: 0x00053C70 File Offset: 0x00051E70
		public override uint Version { get; set; } = Save.DataVersion.Latest;

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00053C79 File Offset: 0x00051E79
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x00053C81 File Offset: 0x00051E81
		public override string SegmentName { get; set; } = "Gameplay";

		// Token: 0x06000F7D RID: 3965 RVA: 0x00053C8A File Offset: 0x00051E8A
		protected override bool IsBase64VersionOrHigher(uint fileSaveVersion)
		{
			return fileSaveVersion >= 3U;
		}
	}
}
