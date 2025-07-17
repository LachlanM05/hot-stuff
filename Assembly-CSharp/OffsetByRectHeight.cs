using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class OffsetByRectHeight : MonoBehaviour
{
	// Token: 0x06000998 RID: 2456 RVA: 0x0003759C File Offset: 0x0003579C
	private void Awake()
	{
		this.rect = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x000375AC File Offset: 0x000357AC
	private void LateUpdate()
	{
		float num = this.map(this.referenceRect.sizeDelta.y, this.heightRange.x, this.heightRange.y, this.xPosRange.x, this.xPosRange.y);
		if (Mathf.Abs(this.rect.anchoredPosition.x - num) > 0.1f)
		{
			Vector2 anchoredPosition = this.rect.anchoredPosition;
			anchoredPosition.x = num;
			this.rect.anchoredPosition = anchoredPosition;
		}
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0003763A File Offset: 0x0003583A
	private float map(float value, float istart, float istop, float ostart, float ostop)
	{
		return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
	}

	// Token: 0x040008DB RID: 2267
	private RectTransform rect;

	// Token: 0x040008DC RID: 2268
	public RectTransform referenceRect;

	// Token: 0x040008DD RID: 2269
	public Vector2 heightRange;

	// Token: 0x040008DE RID: 2270
	public Vector2 xPosRange;
}
