using System;
using T17.Services;
using UnityEngine;

// Token: 0x020000C6 RID: 198
[CreateAssetMenu(fileName = "ColorChoice", menuName = "ScriptableObjects/ColorChoice", order = 1)]
public class ColorChoice : CustomizationChoice
{
	// Token: 0x060006A4 RID: 1700 RVA: 0x00027268 File Offset: 0x00025468
	public override void Set(GameObject target)
	{
		target.GetComponent<Material>().color = this.ChoiceColor;
		foreach (ColorChoice colorChoice in Services.AssetProviderService.LoadResourceAssets<ColorChoice>("Character Creation Assets"))
		{
			if (colorChoice.TargetBodyPart == this.TargetBodyPart)
			{
				colorChoice.IsSet = false;
			}
		}
		this.IsSet = true;
	}

	// Token: 0x040005DC RID: 1500
	public Color ChoiceColor;
}
