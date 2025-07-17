using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class CollectableLayout : MonoBehaviour
{
	// Token: 0x060007EC RID: 2028 RVA: 0x0002D2A0 File Offset: 0x0002B4A0
	public virtual void LayoutElements(List<CollectableView> collectableViews)
	{
		int count = collectableViews.Count;
		int count2 = this.itemRoots.Count;
		this.currentViews = collectableViews;
		int num = 0;
		while (num < this.itemRoots.Count && num < this.currentViews.Count)
		{
			this.currentViews[num].transform.SetParent(this.itemRoots[num], false);
			this.currentViews[num].AlignWithParent();
			this.currentViews[num].SetSelected(false);
			num++;
		}
		this.currentViews[0].Button.onClick.Invoke();
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0002D34C File Offset: 0x0002B54C
	public void DeselectViews()
	{
		foreach (CollectableView collectableView in this.currentViews)
		{
			collectableView.SetSelected(false);
		}
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0002D3A0 File Offset: 0x0002B5A0
	public List<CollectableView> GetViews()
	{
		return this.currentViews;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0002D3A8 File Offset: 0x0002B5A8
	public virtual void ClearViews()
	{
		this.currentViews.Clear();
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0002D3B5 File Offset: 0x0002B5B5
	public virtual CollectableView GetInitialCollectable()
	{
		if (this.itemRoots.Count > 0)
		{
			return this.currentViews[0];
		}
		return null;
	}

	// Token: 0x0400071D RID: 1821
	[SerializeField]
	protected List<Transform> itemRoots;

	// Token: 0x0400071E RID: 1822
	protected List<CollectableView> currentViews;
}
