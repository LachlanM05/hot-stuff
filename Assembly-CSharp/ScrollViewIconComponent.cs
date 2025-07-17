using System;
using Rewired;
using T17.Services;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class ScrollViewIconComponent : MonoBehaviour
{
	// Token: 0x06000EBF RID: 3775 RVA: 0x00050A08 File Offset: 0x0004EC08
	private void Awake()
	{
		this.InitializeData();
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x00050A10 File Offset: 0x0004EC10
	private void InitializeData()
	{
		if (this.iconObject != null)
		{
			this.iconRect = this.iconObject.GetComponent<RectTransform>();
			if (this.iconRect != null)
			{
				this.iconHeight = this.iconRect.rect.height;
			}
		}
		if (this.scrollbarRect != null)
		{
			this.scrollbarInitialOffset = this.scrollbarRect.offsetMin.y;
		}
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x00050A87 File Offset: 0x0004EC87
	private void OnEnable()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.DetectControllerChange));
		this.UpdateScrollBarDimensions(Services.InputService.IsLastActiveInputController());
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x00050AB7 File Offset: 0x0004ECB7
	private void OnDisable()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.DetectControllerChange));
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00050AD8 File Offset: 0x0004ECD8
	private void UpdateScrollBarDimensions(bool controller)
	{
		if (this.scrollbarRect != null)
		{
			float num;
			if (controller)
			{
				num = this.padding + this.iconHeight;
			}
			else
			{
				num = 0f;
			}
			this.scrollbarRect.offsetMin = new Vector2(this.scrollbarRect.offsetMin.x, this.scrollbarInitialOffset + num);
		}
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x00050B34 File Offset: 0x0004ED34
	private void DetectControllerChange(Controller controller)
	{
		this.UpdateScrollBarDimensions(Services.InputService.IsLastActiveInputController());
	}

	// Token: 0x04000D06 RID: 3334
	[SerializeField]
	private GameObject iconObject;

	// Token: 0x04000D07 RID: 3335
	[SerializeField]
	private RectTransform scrollbarRect;

	// Token: 0x04000D08 RID: 3336
	private RectTransform iconRect;

	// Token: 0x04000D09 RID: 3337
	private float scrollbarInitialOffset;

	// Token: 0x04000D0A RID: 3338
	private float iconHeight;

	// Token: 0x04000D0B RID: 3339
	[SerializeField]
	private float padding;
}
