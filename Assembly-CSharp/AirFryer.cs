using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class AirFryer : MonoBehaviour
{
	// Token: 0x060003C0 RID: 960 RVA: 0x00017B02 File Offset: 0x00015D02
	private void Awake()
	{
		this.DeactivateVFX();
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00017B0A File Offset: 0x00015D0A
	public void ActivateVFX()
	{
		this.vfx.Play();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00017B17 File Offset: 0x00015D17
	public void DeactivateVFX()
	{
		this.vfx.Stop();
	}

	// Token: 0x040003B7 RID: 951
	[SerializeField]
	private ActivateVFX vfx;
}
