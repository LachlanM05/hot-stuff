using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class SimpleSlideAnimation : MonoBehaviour
{
	// Token: 0x0600009B RID: 155 RVA: 0x00004328 File Offset: 0x00002528
	private void Start()
	{
		base.gameObject.transform.position = new Vector3((float)this.initialX, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
		this.currentAnimationTime = 0f;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00004388 File Offset: 0x00002588
	private void Update()
	{
		this.currentAnimationTime += Time.deltaTime;
		float num = Mathf.Lerp((float)this.initialX, (float)this.finalX, this.currentAnimationTime / (float)this.animationTimeInSeconds);
		base.gameObject.transform.localPosition = new Vector3(num, base.gameObject.transform.localPosition.y, base.gameObject.transform.localPosition.z);
		if (base.gameObject.transform.localPosition.x <= (float)this.finalX)
		{
			base.gameObject.transform.localPosition = new Vector3((float)this.initialX, base.gameObject.transform.localPosition.y, base.gameObject.transform.localPosition.z);
			this.currentAnimationTime = 0f;
		}
	}

	// Token: 0x04000098 RID: 152
	public int initialX;

	// Token: 0x04000099 RID: 153
	public int finalX;

	// Token: 0x0400009A RID: 154
	public int animationTimeInSeconds;

	// Token: 0x0400009B RID: 155
	private float currentAnimationTime;
}
