using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000148 RID: 328
public class UIRectResizer : MonoBehaviour
{
	// Token: 0x06000BF6 RID: 3062 RVA: 0x0004522D File Offset: 0x0004342D
	private void Update()
	{
		if (!this.resized)
		{
			this.Resize();
			this.resized = true;
		}
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00045244 File Offset: 0x00043444
	public void Resize()
	{
		if (this.rectToSizeTo != null)
		{
			this.ResizeSelf();
		}
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0004525C File Offset: 0x0004345C
	private void ResizeSelf()
	{
		float width = this.rectToSizeTo.rect.width;
		if (width > this.maxWidth)
		{
			width = this.maxWidth;
			this.rectToSizeTo.sizeDelta = new Vector2(width, this.rectToSizeTo.sizeDelta.y + this.verticalPadding + 6f);
			this.rectToSizeTo.gameObject.GetComponent<ContentSizeFitter>().enabled = false;
		}
		float height = this.rectToSizeTo.rect.height;
		base.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width + this.horizontalPadding, height + this.verticalPadding);
	}

	// Token: 0x04000AAD RID: 2733
	[SerializeField]
	private RectTransform rectToSizeTo;

	// Token: 0x04000AAE RID: 2734
	[SerializeField]
	private float horizontalPadding;

	// Token: 0x04000AAF RID: 2735
	[SerializeField]
	private float verticalPadding;

	// Token: 0x04000AB0 RID: 2736
	[SerializeField]
	private float maxWidth = 1100f;

	// Token: 0x04000AB1 RID: 2737
	private bool resized;
}
