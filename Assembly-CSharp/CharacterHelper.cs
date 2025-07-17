using System;
using System.Collections.Generic;
using System.IO;
using T17.Services;

// Token: 0x02000032 RID: 50
public class CharacterHelper : Singleton<CharacterHelper>
{
	// Token: 0x06000138 RID: 312 RVA: 0x00009330 File Offset: 0x00007530
	public override void AwakeSingleton()
	{
		string text = "CharacterUtility/";
		string text2 = Path.Combine("CharacterUtility", "Aliases");
		CharacterUtility[] array = Services.AssetProviderService.LoadResourceAssets<CharacterUtility>(text);
		CharacterAlias[] array2 = Services.AssetProviderService.LoadResourceAssets<CharacterAlias>(text2);
		StringToCharacterUtilityMap stringToCharacterUtilityMap = new StringToCharacterUtilityMap();
		HashSet<string> hashSet = new HashSet<string>();
		Dictionary<string, CharacterUtility> dictionary = new Dictionary<string, CharacterUtility>();
		Dictionary<string, CharacterAlias> dictionary2 = new Dictionary<string, CharacterAlias>();
		foreach (CharacterUtility characterUtility in array)
		{
			hashSet.Add(characterUtility.internalName);
			stringToCharacterUtilityMap.Add(characterUtility.internalName, characterUtility);
		}
		foreach (CharacterAlias characterAlias in array2)
		{
			dictionary2.Add(characterAlias._internalName, characterAlias);
		}
		foreach (string text3 in stringToCharacterUtilityMap.Keys)
		{
			dictionary.Add(text3, stringToCharacterUtilityMap[text3]);
		}
		this._characters = new NameResolver(hashSet, dictionary2, dictionary);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00009450 File Offset: 0x00007650
	public void Start()
	{
	}

	// Token: 0x04000147 RID: 327
	public NameResolver _characters;
}
