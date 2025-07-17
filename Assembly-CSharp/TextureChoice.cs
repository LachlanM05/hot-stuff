using System;
using T17.Services;
using UnityEngine;

// Token: 0x020000CD RID: 205
[CreateAssetMenu(fileName = "TextureChoice", menuName = "ScriptableObjects/TextureChoice", order = 2)]
public class TextureChoice : CustomizationChoice
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x00027520 File Offset: 0x00025720
	public override void Set(GameObject target)
	{
		Material[] materials = target.GetComponent<MeshRenderer>().materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetTexture("_MainTex", this.ChoiceTexture);
		}
		foreach (TextureChoice textureChoice in Services.AssetProviderService.LoadResourceAssets<TextureChoice>("Character Creation Assets"))
		{
			if (textureChoice.TargetBodyPart == this.TargetBodyPart)
			{
				textureChoice.IsSet = false;
			}
		}
		this.IsSet = true;
	}

	// Token: 0x040005FE RID: 1534
	public Texture ChoiceTexture;
}
