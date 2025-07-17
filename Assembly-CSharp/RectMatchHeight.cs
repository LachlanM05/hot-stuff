using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class RectMatchHeight : MonoBehaviour
{
	// Token: 0x06000A2C RID: 2604 RVA: 0x0003A006 File Offset: 0x00038206
	private void Awake()
	{
		this.rect = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0003A014 File Offset: 0x00038214
	private void LateUpdate()
	{
		if ((Mathf.Abs(this.rect.sizeDelta.x - this.rectToMatch.sizeDelta.x) > 0.001f && this.maxSize.x > 0f) || (Mathf.Abs(this.rect.sizeDelta.y - this.rectToMatch.sizeDelta.y) > 0.001f && this.maxSize.y > 0f))
		{
			Vector2 sizeDelta = this.rect.sizeDelta;
			sizeDelta.x = ((this.maxSize.x > 0f) ? (Mathf.Clamp(this.rectToMatch.sizeDelta.x, 0f, this.maxSize.x) + this.padding.x) : sizeDelta.x);
			sizeDelta.y = ((this.maxSize.y > 0f) ? (Mathf.Clamp(this.rectToMatch.sizeDelta.y, 0f, this.maxSize.y) + this.padding.y) : sizeDelta.y);
			this.rect.sizeDelta = sizeDelta;
		}
	}

	// Token: 0x0400094F RID: 2383
	private RectTransform rect;

	// Token: 0x04000950 RID: 2384
	public RectTransform rectToMatch;

	// Token: 0x04000951 RID: 2385
	public Vector2 maxSize;

	// Token: 0x04000952 RID: 2386
	public Vector2 padding;
}
