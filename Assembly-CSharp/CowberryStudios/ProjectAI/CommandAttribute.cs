using System;

namespace CowberryStudios.ProjectAI
{
	// Token: 0x0200025E RID: 606
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandAttribute : Attribute
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060013B3 RID: 5043 RVA: 0x0005F442 File Offset: 0x0005D642
		public CommandID Command { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0005F44A File Offset: 0x0005D64A
		// (set) Token: 0x060013B5 RID: 5045 RVA: 0x0005F452 File Offset: 0x0005D652
		public int GreaterThan { get; set; } = -1;

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0005F45B File Offset: 0x0005D65B
		// (set) Token: 0x060013B7 RID: 5047 RVA: 0x0005F463 File Offset: 0x0005D663
		public int LessThan { get; set; } = -1;

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0005F46C File Offset: 0x0005D66C
		// (set) Token: 0x060013B9 RID: 5049 RVA: 0x0005F474 File Offset: 0x0005D674
		public int GreaterThanOrEqual { get; set; } = -1;

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x0005F47D File Offset: 0x0005D67D
		// (set) Token: 0x060013BB RID: 5051 RVA: 0x0005F485 File Offset: 0x0005D685
		public int LessThanOrEqual { get; set; } = -1;

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0005F48E File Offset: 0x0005D68E
		// (set) Token: 0x060013BD RID: 5053 RVA: 0x0005F496 File Offset: 0x0005D696
		public int Exactly { get; set; } = -1;

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0005F49F File Offset: 0x0005D69F
		// (set) Token: 0x060013BF RID: 5055 RVA: 0x0005F4A7 File Offset: 0x0005D6A7
		public int Not { get; set; } = -1;

		// Token: 0x060013C0 RID: 5056 RVA: 0x0005F4B0 File Offset: 0x0005D6B0
		public CommandAttribute(string commandAlias)
		{
			this.Command = new CommandID(commandAlias);
		}
	}
}
