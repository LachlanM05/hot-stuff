using System;
using T17.Services;
using Team17.Common;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x0200020E RID: 526
	public class ScrollListPageIndicator : MonoBehaviour
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00057563 File Offset: 0x00055763
		private int PageDots
		{
			get
			{
				return this._pageDots.Length;
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0005756D File Offset: 0x0005576D
		private void Start()
		{
			this._provider = base.GetComponent<IScrollListPageIndicatorProvider>();
			this.RefreshView();
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00057581 File Offset: 0x00055781
		private void OnValidate()
		{
			this._pageDots = base.GetComponentsInChildren<ScrollListPageIndicatorDot>();
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00057590 File Offset: 0x00055790
		private int TotalPages()
		{
			ScrollListPageIndicator.ListType listType = this.listType;
			if (listType == ScrollListPageIndicator.ListType.HeavyMiddle)
			{
				return Mathf.CeilToInt((float)this._totalItems / (float)this._itemsPerPage);
			}
			if (listType != ScrollListPageIndicator.ListType.SimpleLinear)
			{
				T17Debug.LogError("ScrollListPageIndicator.CurrentPageIndex is not set to a valid type (" + this.listType.ToString() + ")");
				return 0;
			}
			return this.PageDots;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000575F0 File Offset: 0x000557F0
		private int CurrentPageIndex()
		{
			ScrollListPageIndicator.ListType listType = this.listType;
			if (listType == ScrollListPageIndicator.ListType.HeavyMiddle)
			{
				return (this._currentItemIndex + 1) / this._itemsPerPage;
			}
			if (listType != ScrollListPageIndicator.ListType.SimpleLinear)
			{
				T17Debug.LogError("ScrollListPageIndicator.TotalPages is not set to a valid type (" + this.listType.ToString() + ")");
				return 0;
			}
			return (int)(((float)this._currentItemIndex + 1f) / ((float)this._totalItems / (float)this.activePageDots));
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00057662 File Offset: 0x00055862
		private void RefreshView()
		{
			this._currentItemIndex = this._provider.CurrentItemIndex;
			this._totalItems = this._provider.TotalItems;
			this._itemsPerPage = this._provider.ItemsPerPage;
			this.UpdateUI();
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0005769D File Offset: 0x0005589D
		private void Update()
		{
			if (this.IsDirty())
			{
				this.RefreshView();
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x000576AD File Offset: 0x000558AD
		public void PreviousPage()
		{
			Services.UIInputService.CycleUp();
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x000576B9 File Offset: 0x000558B9
		public void NextPage()
		{
			Services.UIInputService.CycleDown();
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x000576C8 File Offset: 0x000558C8
		private void UpdateUI()
		{
			ScrollListPageIndicator.ListType listType = this.listType;
			if (listType == ScrollListPageIndicator.ListType.HeavyMiddle)
			{
				this.UpdateUI_HeavyMiddle();
				return;
			}
			if (listType != ScrollListPageIndicator.ListType.SimpleLinear)
			{
				return;
			}
			this.UpdateUI_SimpleLinear();
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000576F4 File Offset: 0x000558F4
		private void UpdateUI_HeavyMiddle()
		{
			this.EnsureDotsEnabled(this.TotalPages());
			int num = this.CurrentPageIndex();
			if (this._lastShownPageIndex != num)
			{
				this._lastShownPageIndex = num;
				int num2 = this.CalculatePageIndexToDotIndex(this._lastShownPageIndex);
				ScrollListPageIndicatorDot scrollListPageIndicatorDot;
				if (this._pageDots.Length > num2)
				{
					scrollListPageIndicatorDot = this._pageDots[num2];
				}
				else
				{
					scrollListPageIndicatorDot = this._pageDots[this._pageDots.Length - 1];
				}
				this.UpdateDotUI(scrollListPageIndicatorDot);
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00057760 File Offset: 0x00055960
		private void UpdateUI_SimpleLinear()
		{
			this.EnsureDotsEnabled(this.TotalPages());
			int num = this.CurrentPageIndex();
			if (this._lastShownPageIndex != num)
			{
				this._lastShownPageIndex = num;
				int lastShownPageIndex = this._lastShownPageIndex;
				ScrollListPageIndicatorDot scrollListPageIndicatorDot;
				if (this._pageDots.Length > lastShownPageIndex)
				{
					scrollListPageIndicatorDot = this._pageDots[lastShownPageIndex];
				}
				else
				{
					scrollListPageIndicatorDot = this._pageDots[this._pageDots.Length - 1];
				}
				this.UpdateDotUI(scrollListPageIndicatorDot);
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000577C6 File Offset: 0x000559C6
		private void UpdateDotUI(ScrollListPageIndicatorDot newDot)
		{
			ScrollListPageIndicatorDot currentOpenDot = this._currentOpenDot;
			if (currentOpenDot != null)
			{
				currentOpenDot.OnLeftThisPage();
			}
			this._currentOpenDot = newDot;
			this._currentOpenDot.OnOpenedThisPage();
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000577EC File Offset: 0x000559EC
		private int CalculatePageIndexToDotIndex(int pageIndex)
		{
			int num = this.TotalPages();
			if (num <= this.activePageDots)
			{
				return pageIndex;
			}
			int num2 = this.activePageDots / 2;
			if (pageIndex < num2)
			{
				return pageIndex;
			}
			if (pageIndex > num - num2)
			{
				return this.activePageDots - (num - pageIndex);
			}
			if (this.activePageDots % 2 != 0)
			{
				return num2;
			}
			if (pageIndex < num / 2)
			{
				return num2 - 1;
			}
			return num2;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00057848 File Offset: 0x00055A48
		private bool IsDirty()
		{
			return this._currentItemIndex != this._provider.CurrentItemIndex || this._totalItems != this._provider.TotalItems || this._itemsPerPage != this._provider.ItemsPerPage;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00057898 File Offset: 0x00055A98
		private void EnsureDotsEnabled(int amount)
		{
			this.activePageDots = this._pageDots.Length;
			for (int i = 0; i < this._pageDots.Length; i++)
			{
				if (i < amount)
				{
					this._pageDots[i].Show();
				}
				else
				{
					this._pageDots[i].Hide();
					this.activePageDots--;
				}
			}
		}

		// Token: 0x04000E39 RID: 3641
		private const int kInvalid = -1;

		// Token: 0x04000E3A RID: 3642
		private IScrollListPageIndicatorProvider _provider;

		// Token: 0x04000E3B RID: 3643
		[SerializeField]
		private ScrollListPageIndicatorDot[] _pageDots;

		// Token: 0x04000E3C RID: 3644
		[SerializeField]
		private ScrollListPageIndicator.ListType listType;

		// Token: 0x04000E3D RID: 3645
		private int activePageDots;

		// Token: 0x04000E3E RID: 3646
		private int _currentItemIndex = -1;

		// Token: 0x04000E3F RID: 3647
		private int _totalItems;

		// Token: 0x04000E40 RID: 3648
		private int _itemsPerPage;

		// Token: 0x04000E41 RID: 3649
		private int _lastShownPageIndex = -1;

		// Token: 0x04000E42 RID: 3650
		private ScrollListPageIndicatorDot _currentOpenDot;

		// Token: 0x020003BC RID: 956
		public enum ListType
		{
			// Token: 0x040014D1 RID: 5329
			HeavyMiddle,
			// Token: 0x040014D2 RID: 5330
			SimpleLinear
		}
	}
}
