using System;
using System.Collections.Generic;

// Token: 0x0200005F RID: 95
public class Localisation : Singleton<Localisation>
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000349 RID: 841 RVA: 0x00015966 File Offset: 0x00013B66
	// (set) Token: 0x0600034A RID: 842 RVA: 0x0001596E File Offset: 0x00013B6E
	public TextLanguage activeLanguage { get; protected set; }

	// Token: 0x0600034B RID: 843 RVA: 0x00015977 File Offset: 0x00013B77
	public Localisation()
	{
		this.activeLanguage = TextLanguage.English;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00015998 File Offset: 0x00013B98
	public void LoadCSV(string data)
	{
		this.localisedText = new Dictionary<TextLanguage, Dictionary<string, string>>();
		string[] array = data.Split("\n", StringSplitOptions.None);
		foreach (TextLanguage textLanguage in this.languages)
		{
			this.localisedText[textLanguage] = new Dictionary<string, string>();
		}
		for (int j = 1; j < array.Length; j++)
		{
			string[] array3 = array[j].Split("\t", StringSplitOptions.None);
			for (int k = 0; k < array3.Length; k++)
			{
				this.localisedText[this.languages[k]][array3[0]] = array3[k + 1];
			}
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00015A41 File Offset: 0x00013C41
	public void SetLanguage(TextLanguage language)
	{
		this.activeLanguage = language;
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00015A4A File Offset: 0x00013C4A
	public string Get(string id, TextLanguage language)
	{
		return this.localisedText[language][id];
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00015A5E File Offset: 0x00013C5E
	public string Get(string id)
	{
		return this.Get(id, this.activeLanguage);
	}

	// Token: 0x0400034D RID: 845
	public TextLanguage[] languages = new TextLanguage[]
	{
		TextLanguage.English,
		TextLanguage.Japanese
	};

	// Token: 0x0400034F RID: 847
	protected Dictionary<TextLanguage, Dictionary<string, string>> localisedText;
}
