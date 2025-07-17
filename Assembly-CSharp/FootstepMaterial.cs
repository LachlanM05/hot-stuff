using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class FootstepMaterial : MonoBehaviour
{
	// Token: 0x06000221 RID: 545 RVA: 0x0000C7B6 File Offset: 0x0000A9B6
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Player")
		{
			return;
		}
		if (!BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			return;
		}
		FootStep.instance.ChangeFoostepCategory(this.matType);
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000C7F3 File Offset: 0x0000A9F3
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag != "Player")
		{
			return;
		}
		if (!BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			return;
		}
		FootStep.instance.ChangeFoostepCategory(this.matType);
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000C830 File Offset: 0x0000AA30
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag != "Player")
		{
			return;
		}
		if (!BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			return;
		}
		FootStep.instance.ChangeFoostepCategory(this.matType);
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000C86D File Offset: 0x0000AA6D
	private void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.tag != "Player")
		{
			return;
		}
		if (!BetterPlayerControl.Instance.IsPlayerFloor(base.gameObject))
		{
			return;
		}
		FootStep.instance.ChangeFoostepCategory(this.matType);
	}

	// Token: 0x04000307 RID: 775
	[SerializeField]
	private FootStep.FootstepMaterial matType;
}
