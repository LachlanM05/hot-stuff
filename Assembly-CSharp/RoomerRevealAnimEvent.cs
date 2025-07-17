using System;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class RoomerRevealAnimEvent : MonoBehaviour
{
	// Token: 0x06000A49 RID: 2633 RVA: 0x0003B439 File Offset: 0x00039639
	public void OnAnimatorComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0003B446 File Offset: 0x00039646
	public void OnDisable()
	{
		this.OnAnimatorComplete();
	}
}
