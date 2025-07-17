using System;

namespace CowberryStudios.ProjectAI
{
	// Token: 0x02000261 RID: 609
	public class Dialogue
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x0005F5B7 File Offset: 0x0005D7B7
		public string Character { get; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x0005F5BF File Offset: 0x0005D7BF
		public string Text { get; }

		// Token: 0x060013CD RID: 5069 RVA: 0x0005F5C7 File Offset: 0x0005D7C7
		public Dialogue(string character, string text)
		{
			this.Character = character;
			this.Text = text;
		}
	}
}
