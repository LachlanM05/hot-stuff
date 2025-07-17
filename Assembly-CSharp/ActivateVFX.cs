using System;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x0200015C RID: 348
public class ActivateVFX : MonoBehaviour
{
	// Token: 0x06000CDA RID: 3290 RVA: 0x0004A7E0 File Offset: 0x000489E0
	public void Play()
	{
		this.animator.enabled = true;
		VisualEffect[] array = this.particles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Play();
		}
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0004A818 File Offset: 0x00048A18
	public void Stop()
	{
		this.animator.enabled = false;
		VisualEffect[] array = this.particles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
		}
	}

	// Token: 0x04000B7E RID: 2942
	[SerializeField]
	private Animator animator;

	// Token: 0x04000B7F RID: 2943
	public VisualEffect[] particles;
}
