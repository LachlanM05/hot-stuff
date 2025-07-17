using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class FadeInOnEnable : MonoBehaviour
{
	// Token: 0x06000E40 RID: 3648 RVA: 0x0004EB3C File Offset: 0x0004CD3C
	private void Awake()
	{
		if (this.timeToFade <= 0f || (this.group == null && (this.group = base.GetComponent<CanvasGroup>()) == null))
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x0004EB82 File Offset: 0x0004CD82
	private void OnEnable()
	{
		this.currentTime = 0f;
		this.ProcessFade();
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x0004EB95 File Offset: 0x0004CD95
	private void Update()
	{
		if (this.currentTime < this.timeToFade)
		{
			this.currentTime += Time.deltaTime;
			this.ProcessFade();
		}
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x0004EBC0 File Offset: 0x0004CDC0
	private void ProcessFade()
	{
		float num;
		if (this.currentTime < this.timeToFade)
		{
			num = this.currentTime / this.timeToFade;
		}
		else
		{
			num = 1f;
		}
		float num2 = this.startAlpha + (this.endAlpha - this.startAlpha) * num;
		this.group.alpha = num2;
	}

	// Token: 0x04000CA0 RID: 3232
	public CanvasGroup group;

	// Token: 0x04000CA1 RID: 3233
	public float startAlpha;

	// Token: 0x04000CA2 RID: 3234
	public float endAlpha = 1f;

	// Token: 0x04000CA3 RID: 3235
	public float timeToFade = 1f;

	// Token: 0x04000CA4 RID: 3236
	private float currentTime;
}
