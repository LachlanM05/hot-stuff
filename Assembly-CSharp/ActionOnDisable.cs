using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class ActionOnDisable : MonoBehaviour
{
	// Token: 0x06000DF4 RID: 3572 RVA: 0x0004DD30 File Offset: 0x0004BF30
	private void Start()
	{
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x0004DD32 File Offset: 0x0004BF32
	private void Update()
	{
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x0004DD34 File Offset: 0x0004BF34
	private void OnDisable()
	{
		if (base.enabled && (!this.ignoreHierarchy || !base.gameObject.activeSelf))
		{
			Action action = this.onDisable;
			if (action == null)
			{
				return;
			}
			action();
		}
	}

	// Token: 0x04000C6A RID: 3178
	[Tooltip("Only trigger action if gameObject itself becomes inactive, not if it's inactive due to hierarchy")]
	public bool ignoreHierarchy = true;

	// Token: 0x04000C6B RID: 3179
	public Action onDisable;
}
