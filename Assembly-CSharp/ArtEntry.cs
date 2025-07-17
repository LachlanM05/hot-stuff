using System;
using UnityEngine.AddressableAssets;

// Token: 0x02000105 RID: 261
[Serializable]
public class ArtEntry
{
	// Token: 0x060008DA RID: 2266 RVA: 0x00034417 File Offset: 0x00032617
	public ArtEntry(int number, string title, AssetReferenceSprite spriteReference)
	{
		this.number = number;
		this.title = title;
		this.spriteReference = spriteReference;
	}

	// Token: 0x0400081B RID: 2075
	public int number;

	// Token: 0x0400081C RID: 2076
	public string title;

	// Token: 0x0400081D RID: 2077
	public AssetReferenceSprite spriteReference;
}
