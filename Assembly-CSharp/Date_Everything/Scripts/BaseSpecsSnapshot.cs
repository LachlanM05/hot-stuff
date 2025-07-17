using System;
using Newtonsoft.Json;

namespace Date_Everything.Scripts
{
	// Token: 0x02000268 RID: 616
	[Serializable]
	public class BaseSpecsSnapshot
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0006051B File Offset: 0x0005E71B
		// (set) Token: 0x060013EF RID: 5103 RVA: 0x00060523 File Offset: 0x0005E723
		[JsonProperty]
		public int Smarts { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x0006052C File Offset: 0x0005E72C
		// (set) Token: 0x060013F1 RID: 5105 RVA: 0x00060534 File Offset: 0x0005E734
		[JsonProperty]
		public int Poise { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x0006053D File Offset: 0x0005E73D
		// (set) Token: 0x060013F3 RID: 5107 RVA: 0x00060545 File Offset: 0x0005E745
		[JsonProperty]
		public int Empathy { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x0006054E File Offset: 0x0005E74E
		// (set) Token: 0x060013F5 RID: 5109 RVA: 0x00060556 File Offset: 0x0005E756
		[JsonProperty]
		public int Charm { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x0006055F File Offset: 0x0005E75F
		// (set) Token: 0x060013F7 RID: 5111 RVA: 0x00060567 File Offset: 0x0005E767
		[JsonProperty]
		public int Sass { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00060570 File Offset: 0x0005E770
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x00060578 File Offset: 0x0005E778
		[JsonProperty]
		public int SmartsDlc { get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x00060581 File Offset: 0x0005E781
		// (set) Token: 0x060013FB RID: 5115 RVA: 0x00060589 File Offset: 0x0005E789
		[JsonProperty]
		public int PoiseDlc { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x00060592 File Offset: 0x0005E792
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x0006059A File Offset: 0x0005E79A
		[JsonProperty]
		public int EmpathyDlc { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x000605A3 File Offset: 0x0005E7A3
		// (set) Token: 0x060013FF RID: 5119 RVA: 0x000605AB File Offset: 0x0005E7AB
		[JsonProperty]
		public int CharmDlc { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x000605B4 File Offset: 0x0005E7B4
		// (set) Token: 0x06001401 RID: 5121 RVA: 0x000605BC File Offset: 0x0005E7BC
		[JsonProperty]
		public int SassDlc { get; set; }

		// Token: 0x06001402 RID: 5122 RVA: 0x000605C5 File Offset: 0x0005E7C5
		public BaseSpecsSnapshot()
		{
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x000605D0 File Offset: 0x0005E7D0
		public BaseSpecsSnapshot(int smarts, int poise, int empathy, int charm, int sass, int smartsDlc, int poiseDlc, int empathyDlc, int charmDlc, int sassDlc)
		{
			this.Smarts = smarts;
			this.Poise = poise;
			this.Empathy = empathy;
			this.Charm = charm;
			this.Sass = sass;
			this.SmartsDlc = smartsDlc;
			this.PoiseDlc = poiseDlc;
			this.EmpathyDlc = empathyDlc;
			this.CharmDlc = charmDlc;
			this.SassDlc = sassDlc;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00060630 File Offset: 0x0005E830
		public static BaseSpecsSnapshot CreateFromCurrentStory()
		{
			return new BaseSpecsSnapshot(BaseSpecsSnapshot.GetBaseSpecLevel("smarts"), BaseSpecsSnapshot.GetBaseSpecLevel("poise"), BaseSpecsSnapshot.GetBaseSpecLevel("empathy"), BaseSpecsSnapshot.GetBaseSpecLevel("charm"), BaseSpecsSnapshot.GetBaseSpecLevel("sass"), BaseSpecsSnapshot.GetDlcSpecLevel("smarts_dlc"), BaseSpecsSnapshot.GetDlcSpecLevel("poise_dlc"), BaseSpecsSnapshot.GetDlcSpecLevel("empathy_dlc"), BaseSpecsSnapshot.GetDlcSpecLevel("charm_dlc"), BaseSpecsSnapshot.GetDlcSpecLevel("sass_dlc"));
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000606A8 File Offset: 0x0005E8A8
		public static void ApplySnapshot(BaseSpecsSnapshot snapshot)
		{
			BaseSpecsSnapshot.SetBaseSpecLevel("smarts", snapshot.Smarts);
			BaseSpecsSnapshot.SetBaseSpecLevel("poise", snapshot.Poise);
			BaseSpecsSnapshot.SetBaseSpecLevel("empathy", snapshot.Empathy);
			BaseSpecsSnapshot.SetBaseSpecLevel("charm", snapshot.Charm);
			BaseSpecsSnapshot.SetBaseSpecLevel("sass", snapshot.Sass);
			BaseSpecsSnapshot.SetDlcSpecLevel("smarts_dlc", snapshot.SmartsDlc);
			BaseSpecsSnapshot.SetDlcSpecLevel("poise_dlc", snapshot.PoiseDlc);
			BaseSpecsSnapshot.SetDlcSpecLevel("empathy_dlc", snapshot.EmpathyDlc);
			BaseSpecsSnapshot.SetDlcSpecLevel("charm_dlc", snapshot.CharmDlc);
			BaseSpecsSnapshot.SetDlcSpecLevel("sass_dlc", snapshot.SassDlc);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00060755 File Offset: 0x0005E955
		public static void SetBaseSpecLevel(string stat, int level)
		{
			Singleton<InkController>.Instance.story.variablesState[stat.ToLowerInvariant().Trim()] = level;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0006077C File Offset: 0x0005E97C
		public static void SetDlcSpecLevel(string stat, int level)
		{
			Singleton<InkController>.Instance.story.variablesState[stat.ToLowerInvariant().Trim()] = level;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000607A3 File Offset: 0x0005E9A3
		public static int GetBaseSpecLevel(string stat)
		{
			return (int)Singleton<InkController>.Instance.story.variablesState[stat.ToLowerInvariant().Trim()];
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x000607CC File Offset: 0x0005E9CC
		public static int GetDlcSpecLevel(string stat)
		{
			string text = Singleton<InkController>.Instance.story.variablesState[stat.ToLowerInvariant().Trim()].ToString();
			int num = 0;
			int.TryParse(text, out num);
			return num;
		}
	}
}
