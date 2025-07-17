using System;
using AirFishLab.ScrollingList;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001F8 RID: 504
	public class CircularListPageIndicatorProvider : MonoBehaviour, IScrollListPageIndicatorProvider
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x000560A5 File Offset: 0x000542A5
		public int TotalItems
		{
			get
			{
				return this._list.ListBank.GetContentCount();
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x000560B7 File Offset: 0x000542B7
		public int CurrentItemIndex
		{
			get
			{
				return this._list.GetFocusingContentID() - 1;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x000560C6 File Offset: 0x000542C6
		public int ItemsPerPage
		{
			get
			{
				return this._itemsPerPage;
			}
		}

		// Token: 0x04000E03 RID: 3587
		[SerializeField]
		private CircularScrollingList _list;

		// Token: 0x04000E04 RID: 3588
		[SerializeField]
		private int _itemsPerPage = 5;
	}
}
