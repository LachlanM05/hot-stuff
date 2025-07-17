using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200016C RID: 364
public class ScrollOption : MonoBehaviour
{
	// Token: 0x06000D11 RID: 3345 RVA: 0x0004B48D File Offset: 0x0004968D
	private void Start()
	{
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0004B48F File Offset: 0x0004968F
	private void Update()
	{
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x0004B494 File Offset: 0x00049694
	public void ScrollPosition()
	{
		if (this.ScrollContent != null && this.ScrollRect != null && this.myRectTransform != null)
		{
			this.ScrollContent.anchoredPosition = this.ScrollRect.transform.InverseTransformPoint(this.ScrollContent.position) - this.ScrollRect.transform.InverseTransformPoint(this.myRectTransform.position);
		}
	}

	// Token: 0x04000BD4 RID: 3028
	public ScrollRect ScrollRect;

	// Token: 0x04000BD5 RID: 3029
	public RectTransform ScrollContent;

	// Token: 0x04000BD6 RID: 3030
	public RectTransform myRectTransform;
}
