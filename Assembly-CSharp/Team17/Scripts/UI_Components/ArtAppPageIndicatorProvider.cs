using System;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001F5 RID: 501
	public class ArtAppPageIndicatorProvider : MonoBehaviour, IScrollListPageIndicatorProvider
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x00055F5D File Offset: 0x0005415D
		public int TotalItems
		{
			get
			{
				return this._artPlayer.TotalEntries;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x00055F6A File Offset: 0x0005416A
		public int CurrentItemIndex
		{
			get
			{
				return this._artPlayer.CurrentArtIndex;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x00055F77 File Offset: 0x00054177
		public int ItemsPerPage
		{
			get
			{
				return this._itemsPerPage;
			}
		}

		// Token: 0x04000DFB RID: 3579
		[SerializeField]
		private ArtPlayer _artPlayer;

		// Token: 0x04000DFC RID: 3580
		[SerializeField]
		private int _itemsPerPage = 5;
	}
}
