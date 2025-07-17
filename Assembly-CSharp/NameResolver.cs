using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020000B8 RID: 184
public class NameResolver : IEnumerable<string>, IEnumerable
{
	// Token: 0x060005B5 RID: 1461 RVA: 0x00020ADA File Offset: 0x0001ECDA
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00020AE2 File Offset: 0x0001ECE2
	public IEnumerator<string> GetEnumerator()
	{
		return this._internalNames.GetEnumerator();
	}

	// Token: 0x17000019 RID: 25
	public CharacterUtility this[string internalName]
	{
		get
		{
			if (this._internalNames.Contains(internalName))
			{
				return this._internalNameToCharacterUtility[internalName];
			}
			return null;
		}
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00020B12 File Offset: 0x0001ED12
	public bool IsNameInSet(string name)
	{
		return this._internalNames.Contains(name);
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00020B20 File Offset: 0x0001ED20
	public bool IsGlobalNameInSet(string name)
	{
		return this._globalNameSet.Contains(name);
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00020B30 File Offset: 0x0001ED30
	public string GetInternalName(string alias)
	{
		if (alias == "volt")
		{
			return "eddie";
		}
		alias = alias.ToLowerInvariant();
		if (this.IsNameInSet(alias))
		{
			return alias;
		}
		string text;
		if (!this._aliasToInternalName.TryGetValue(alias, out text))
		{
			return alias;
		}
		return text;
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00020B78 File Offset: 0x0001ED78
	public string GetGlobalName(string internalName)
	{
		internalName = internalName.ToLowerInvariant();
		if (this.IsGlobalNameInSet(internalName))
		{
			return internalName;
		}
		foreach (string text in this._aliasToInternalName.Keys)
		{
			if (this._aliasToInternalName[text] == internalName)
			{
				return text;
			}
		}
		return internalName;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00020BF8 File Offset: 0x0001EDF8
	public NameResolver(HashSet<string> internalNames, Dictionary<string, CharacterUtility> internalNameToCharacterUtility)
	{
		this._globalNameSet = new HashSet<string>(internalNames);
		this._internalNames = internalNames;
		this._internalNameToCharacterUtility = internalNameToCharacterUtility;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x00020C1C File Offset: 0x0001EE1C
	public NameResolver(HashSet<string> internalNames, Dictionary<string, CharacterAlias> internalNameToCharacterAlias, Dictionary<string, CharacterUtility> internalNameToCharacterUtility)
	{
		this._globalNameSet = new HashSet<string>(internalNames);
		this._internalNames = internalNames;
		this._internalNameToCharacterAlias = internalNameToCharacterAlias;
		this._aliasToInternalName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		foreach (string text in this._internalNames)
		{
			if (this._internalNameToCharacterAlias.ContainsKey(text))
			{
				foreach (string text2 in this._internalNameToCharacterAlias[text]._aliasNames)
				{
					string text3 = text2.ToLowerInvariant();
					if (this._globalNameSet.Contains(text3))
					{
						if (text != text3)
						{
						}
					}
					else
					{
						this._globalNameSet.Add(text3);
						this._aliasToInternalName.Add(text3, text);
					}
				}
			}
		}
		this._internalNameToCharacterUtility = internalNameToCharacterUtility;
	}

	// Token: 0x04000574 RID: 1396
	private HashSet<string> _globalNameSet;

	// Token: 0x04000575 RID: 1397
	private HashSet<string> _internalNames;

	// Token: 0x04000576 RID: 1398
	private Dictionary<string, CharacterAlias> _internalNameToCharacterAlias;

	// Token: 0x04000577 RID: 1399
	private Dictionary<string, CharacterUtility> _internalNameToCharacterUtility;

	// Token: 0x04000578 RID: 1400
	private Dictionary<string, string> _aliasToInternalName;
}
