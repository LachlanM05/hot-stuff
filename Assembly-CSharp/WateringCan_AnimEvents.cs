using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class WateringCan_AnimEvents : MonoBehaviour
{
	// Token: 0x06000550 RID: 1360 RVA: 0x0001F35E File Offset: 0x0001D55E
	public void On()
	{
		this.VFX.SetActive(true);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0001F36C File Offset: 0x0001D56C
	public void Off()
	{
		this.VFX.SetActive(false);
	}

	// Token: 0x04000535 RID: 1333
	[SerializeField]
	private GameObject VFX;
}
