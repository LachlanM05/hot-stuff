using System;
using Team17.Common;

namespace Team17.Scripts.Services
{
	// Token: 0x02000218 RID: 536
	internal interface IInternalInputMapsService : IService
	{
		// Token: 0x0600116A RID: 4458
		void SetToEngagementMode();

		// Token: 0x0600116B RID: 4459
		void SetToGameplayMode();

		// Token: 0x0600116C RID: 4460
		void SetToUIOnlyMode();

		// Token: 0x0600116D RID: 4461
		void SetToUIWithChatMode();

		// Token: 0x0600116E RID: 4462
		void DisableAllMaps();
	}
}
