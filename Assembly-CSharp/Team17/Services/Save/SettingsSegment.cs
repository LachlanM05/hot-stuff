using System;

namespace Team17.Services.Save
{
	// Token: 0x020001D6 RID: 470
	public class SettingsSegment : JsonSegment
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00054051 File Offset: 0x00052251
		// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x00054059 File Offset: 0x00052259
		public override uint Version { get; set; } = GameSettingsProvider.DataVersion.Latest;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x00054062 File Offset: 0x00052262
		// (set) Token: 0x06000FA2 RID: 4002 RVA: 0x0005406A File Offset: 0x0005226A
		public override string SegmentName { get; set; } = "Settings";

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00054073 File Offset: 0x00052273
		protected override bool IsBase64VersionOrHigher(uint fileSaveVersion)
		{
			return fileSaveVersion >= 1U;
		}
	}
}
