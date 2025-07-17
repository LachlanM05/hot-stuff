using System;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x0200020D RID: 525
	public interface IScrollListPageIndicatorProvider
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06001125 RID: 4389
		int TotalItems { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06001126 RID: 4390
		int CurrentItemIndex { get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06001127 RID: 4391
		int ItemsPerPage { get; }
	}
}
