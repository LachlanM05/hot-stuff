using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class Faucet_AnimEvents : MonoBehaviour
{
	// Token: 0x0600036F RID: 879 RVA: 0x00016391 File Offset: 0x00014591
	public void StopSink()
	{
		this.faucet.StopVFX();
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0001639E File Offset: 0x0001459E
	public void StartSink()
	{
		this.faucet.StartVFX();
	}

	// Token: 0x0400036E RID: 878
	[SerializeField]
	private Faucet faucet;
}
