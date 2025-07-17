using System;
using T17.Services;
using UnityEngine;

// Token: 0x020000C9 RID: 201
[CreateAssetMenu(fileName = "MeshChoice", menuName = "ScriptableObjects/MeshChoice", order = 0)]
public class MeshChoice : CustomizationChoice
{
	// Token: 0x060006AD RID: 1709 RVA: 0x000273C0 File Offset: 0x000255C0
	public override void Set(GameObject target)
	{
		target.GetComponent<MeshFilter>().mesh = this.ChoiceMesh;
		foreach (MeshChoice meshChoice in Services.AssetProviderService.LoadResourceAssets<MeshChoice>("Character Creation Assets"))
		{
			if (meshChoice.TargetBodyPart == this.TargetBodyPart)
			{
				meshChoice.IsSet = false;
			}
		}
		this.IsSet = true;
	}

	// Token: 0x040005F1 RID: 1521
	public Mesh ChoiceMesh;
}
