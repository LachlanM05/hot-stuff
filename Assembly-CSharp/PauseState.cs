using System;

// Token: 0x020000BD RID: 189
[Serializable]
public class PauseState
{
	// Token: 0x060005D9 RID: 1497 RVA: 0x00021361 File Offset: 0x0001F561
	public PauseState(PauseOrFreeze _type, object _source)
	{
		this.Type = _type;
		this.Source = _source.ToString();
	}

	// Token: 0x0400058E RID: 1422
	public PauseOrFreeze Type;

	// Token: 0x0400058F RID: 1423
	public string Source;
}
