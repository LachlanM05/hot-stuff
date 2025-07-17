using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class SineOscillateScale : MonoBehaviour
{
	// Token: 0x0600009E RID: 158 RVA: 0x00004480 File Offset: 0x00002680
	private void Start()
	{
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00004484 File Offset: 0x00002684
	private void Update()
	{
		base.transform.localScale = Vector3.Lerp(this.minScale, this.maxScale, (Mathf.Sin(Time.realtimeSinceStartup + this.offset) + 1f) / 2f * this.speedMultiplier);
	}

	// Token: 0x0400009C RID: 156
	public Vector3 maxScale = Vector3.one;

	// Token: 0x0400009D RID: 157
	public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x0400009E RID: 158
	public float offset;

	// Token: 0x0400009F RID: 159
	public float speedMultiplier = 1f;
}
