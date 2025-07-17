using System;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class ActivateParticle : Interactable
{
	// Token: 0x06000CD7 RID: 3287 RVA: 0x0004A7A3 File Offset: 0x000489A3
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x0004A7AC File Offset: 0x000489AC
	public override void Interact()
	{
		ParticleSystem[] array = this.particles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Play();
		}
	}

	// Token: 0x04000B7D RID: 2941
	public ParticleSystem[] particles;
}
