using System;
using Team17.Common;

namespace Team17.Services
{
	// Token: 0x020001C5 RID: 453
	public interface IIconTextMarkupService : IService
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000F0B RID: 3851
		// (remove) Token: 0x06000F0C RID: 3852
		event Action RefreshedIconsEvent;

		// Token: 0x06000F0D RID: 3853
		string ReplaceMirandaMarkupWithTMPSpriteMarkup(string text, out bool stringConverted);

		// Token: 0x06000F0E RID: 3854
		string GetTMPSpriteTag(string inputAction, bool addBrackets = true);

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000F0F RID: 3855
		bool HasCachedControllerMaps { get; }
	}
}
