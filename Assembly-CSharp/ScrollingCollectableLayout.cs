using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class ScrollingCollectableLayout : CollectableLayout
{
	// Token: 0x060008C3 RID: 2243 RVA: 0x00033D04 File Offset: 0x00031F04
	public override void LayoutElements(List<CollectableView> collectableViews)
	{
		this.currentViews = collectableViews;
		float num = (base.transform as RectTransform).rect.height - (float)this.padding.top - (float)this.padding.bottom;
		float num2 = -(num / 3f + (float)this.padding.top);
		float num3 = -(num / 3f * 2f + (float)this.padding.top);
		float num4 = (float)(this.padding.left + this.stepX);
		using (List<CollectableView>.Enumerator enumerator = this.currentViews.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				RectTransform rectTransform;
				if (enumerator.Current.TryGetComponent<RectTransform>(out rectTransform))
				{
					this.contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(this.stepX * this.currentViews.Count + this.padding.left + this.padding.right) + rectTransform.rect.width / 2f);
					break;
				}
			}
		}
		for (int i = 0; i < this.currentViews.Count; i++)
		{
			this.currentViews[i].transform.SetParent(this.contentRect, false);
			if (this.currentViews[i].Button != null)
			{
				this.currentViews[i].Button.gameObject.AddComponent<ScrollInView>().SetTriggerOnPress(true);
			}
			RectTransform rectTransform2;
			if (this.currentViews[i].TryGetComponent<RectTransform>(out rectTransform2))
			{
				rectTransform2.anchorMin = new Vector2(0f, 1f);
				rectTransform2.anchorMax = new Vector2(0f, 1f);
				rectTransform2.pivot = new Vector2(0.5f, 0.5f);
				Vector2 vector = new Vector2
				{
					x = num4 + (float)(i * this.stepX),
					y = ((i % 2 == 0) ? num3 : num2)
				};
				rectTransform2.anchoredPosition = vector;
				this.currentViews[i].SetSelected(false);
			}
		}
		Vector2 anchoredPosition = this.contentRect.anchoredPosition;
		anchoredPosition.x = 0f;
		this.contentRect.anchoredPosition = anchoredPosition;
		this.currentViews[0].Button.onClick.Invoke();
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00033F84 File Offset: 0x00032184
	public override CollectableView GetInitialCollectable()
	{
		if (this.currentViews.Count > 0)
		{
			return this.currentViews[0];
		}
		return null;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00033FA4 File Offset: 0x000321A4
	public override void ClearViews()
	{
		for (int i = this.currentViews.Count - 1; i >= 0; i--)
		{
			if (this.currentViews[i] != null && this.currentViews[i].Button != null)
			{
				ScrollInView component = this.currentViews[i].Button.gameObject.GetComponent<ScrollInView>();
				if (component != null)
				{
					Object.Destroy(component);
				}
			}
		}
		this.currentViews.Clear();
	}

	// Token: 0x040007FF RID: 2047
	[SerializeField]
	private RectTransform contentRect;

	// Token: 0x04000800 RID: 2048
	[SerializeField]
	private RectOffset padding;

	// Token: 0x04000801 RID: 2049
	[SerializeField]
	private int stepX = 350;
}
