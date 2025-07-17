using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C4 RID: 196
[CreateAssetMenu(fileName = "CharacterAlias", menuName = "ScriptableObjects/CharacterAlias", order = 13)]
[Serializable]
public class CharacterAlias : ScriptableObject
{
	// Token: 0x06000696 RID: 1686 RVA: 0x000245EA File Offset: 0x000227EA
	public CharacterAlias()
	{
		this._internalName = "";
		this._aliasNames = new SerializedNameSet();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00024608 File Offset: 0x00022808
	public void DataInitialize(string internalName, HashSet<string> names)
	{
		this._internalName = internalName;
		this._aliasNames.CopyFrom(names);
	}

	// Token: 0x040005D5 RID: 1493
	[SerializeField]
	public string _internalName;

	// Token: 0x040005D6 RID: 1494
	[SerializeField]
	public SerializedNameSet _aliasNames;
}
