using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class LabeledBool : PropertyAttribute
{
	// Token: 0x06000589 RID: 1417 RVA: 0x0001FE0D File Offset: 0x0001E00D
	public LabeledBool(string labelWhenTrue, string labelWhenFalse)
	{
		this.labelWhenTrue = labelWhenTrue;
		this.labelWhenFalse = labelWhenFalse;
	}

	// Token: 0x0400054D RID: 1357
	public string labelWhenTrue;

	// Token: 0x0400054E RID: 1358
	public string labelWhenFalse;
}
