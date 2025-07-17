using System;
using Steamworks;
using Team17.Platform.Entitlements;
using Team17.Platform.Entitlements.Steam;

namespace Team17
{
	// Token: 0x020001C4 RID: 452
	public class AppIdDefinition : ISteamAppIdProvider, IAppIdProvider
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x00052792 File Offset: 0x00050992
		public AppId_t TitleId
		{
			get
			{
				return SteamManager.ApplicationId;
			}
		}
	}
}
