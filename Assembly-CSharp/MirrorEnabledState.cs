using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class MirrorEnabledState : MonoBehaviour
{
	// Token: 0x06000E7C RID: 3708 RVA: 0x0004FC85 File Offset: 0x0004DE85
	private void OnEnable()
	{
		if (this.target != null)
		{
			this.target.SetActive(!this.disableWhenEnabled);
		}
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0004FCA9 File Offset: 0x0004DEA9
	private void OnDisable()
	{
		if (this.target != null)
		{
			this.target.SetActive(this.disableWhenEnabled);
		}
	}

	// Token: 0x04000CE3 RID: 3299
	public GameObject target;

	// Token: 0x04000CE4 RID: 3300
	public bool disableWhenEnabled;
}
