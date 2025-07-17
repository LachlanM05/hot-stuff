using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class Curtain : MonoBehaviour
{
	// Token: 0x0600040F RID: 1039 RVA: 0x00019428 File Offset: 0x00017628
	public void ToggleCurtain()
	{
		this.isOpen = !this.isOpen;
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (this.isOpen)
		{
			component.size = this.colliderOpenSize;
			component.center = this.colliderOpenCenter;
			return;
		}
		component.size = this.colliderCloseSize;
		component.center = this.colliderCloseCenter;
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0001948C File Offset: 0x0001768C
	private void Start()
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		this.colliderCloseSize = component.size;
		this.colliderCloseCenter = component.center;
	}

	// Token: 0x0400040D RID: 1037
	public Vector3 colliderOpenCenter;

	// Token: 0x0400040E RID: 1038
	public Vector3 colliderOpenSize;

	// Token: 0x0400040F RID: 1039
	private Vector3 colliderCloseCenter;

	// Token: 0x04000410 RID: 1040
	private Vector3 colliderCloseSize;

	// Token: 0x04000411 RID: 1041
	private bool isOpen;
}
