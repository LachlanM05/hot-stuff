using System;

// Token: 0x02000063 RID: 99
public struct ParsedTag
{
	// Token: 0x06000357 RID: 855 RVA: 0x00016090 File Offset: 0x00014290
	public ParsedTag(string input)
	{
		int num = input.IndexOf(" ");
		if (num >= 0)
		{
			this.name = input.Substring(0, num);
			this.properties = input.Substring(num + 1, input.Length - num - 1);
			return;
		}
		this.name = input.Trim().ToLowerInvariant();
		this.properties = null;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x000160ED File Offset: 0x000142ED
	public bool HasProperties()
	{
		return !string.IsNullOrEmpty(this.properties);
	}

	// Token: 0x0400035A RID: 858
	public string name;

	// Token: 0x0400035B RID: 859
	public string properties;
}
