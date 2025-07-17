using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class ExteriorAnimal_AnimEvents : MonoBehaviour
{
	// Token: 0x06000013 RID: 19 RVA: 0x00002383 File Offset: 0x00000583
	public void Stop()
	{
		this.ext.EndCurrent();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002390 File Offset: 0x00000590
	public void Play()
	{
		this.ext.Play();
	}

	// Token: 0x04000010 RID: 16
	[SerializeField]
	private ExteriorAnimal ext;
}
