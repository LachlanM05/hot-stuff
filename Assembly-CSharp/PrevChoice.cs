using System;

// Token: 0x02000124 RID: 292
public class PrevChoice
{
	// Token: 0x06000A22 RID: 2594 RVA: 0x00039F12 File Offset: 0x00038112
	private PrevChoice()
	{
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00039F1A File Offset: 0x0003811A
	public PrevChoice(bool lcc, string ps)
	{
		this.lastChoiceContinue = lcc;
		this.prevState = ps;
	}

	// Token: 0x04000946 RID: 2374
	public bool lastChoiceContinue;

	// Token: 0x04000947 RID: 2375
	public string prevState;

	// Token: 0x04000948 RID: 2376
	public string parentInkNode;
}
