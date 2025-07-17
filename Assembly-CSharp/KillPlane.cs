using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class KillPlane : MonoBehaviour
{
	// Token: 0x06000585 RID: 1413 RVA: 0x0001FDA8 File Offset: 0x0001DFA8
	private void Awake()
	{
		KillPlane.Instance = this;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0001FDB0 File Offset: 0x0001DFB0
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			this.ResetPlayer();
		}
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0001FDCA File Offset: 0x0001DFCA
	public void ResetPlayer()
	{
		BetterPlayerControl.Instance.ResetSpeed();
		BetterPlayerControl.Instance.transform.position = this.playerPosition;
		BetterPlayerControl.Instance.transform.rotation = Quaternion.Euler(this.playerRotation);
	}

	// Token: 0x0400054A RID: 1354
	public static KillPlane Instance;

	// Token: 0x0400054B RID: 1355
	public Vector3 playerPosition;

	// Token: 0x0400054C RID: 1356
	public Vector3 playerRotation;
}
