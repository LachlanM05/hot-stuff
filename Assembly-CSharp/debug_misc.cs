using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class debug_misc : Interactable
{
	// Token: 0x060001D0 RID: 464 RVA: 0x0000B7D4 File Offset: 0x000099D4
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000B7DC File Offset: 0x000099DC
	public override void Interact()
	{
		AssetCleaner[] array = this.assetCleaners;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ToggleClean();
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000B806 File Offset: 0x00009A06
	private void Start()
	{
		this.assetCleaners = (AssetCleaner[])Object.FindObjectsOfType(typeof(AssetCleaner));
	}

	// Token: 0x040002B5 RID: 693
	private AssetCleaner[] assetCleaners;
}
