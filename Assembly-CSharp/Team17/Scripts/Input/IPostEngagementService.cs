using System;
using Team17.Common;

namespace Team17.Scripts.Input
{
	// Token: 0x02000237 RID: 567
	public interface IPostEngagementService : IService
	{
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001221 RID: 4641
		// (remove) Token: 0x06001222 RID: 4642
		event Action OnHandlingLostUserEvent;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001223 RID: 4643
		// (remove) Token: 0x06001224 RID: 4644
		event Action OnHandlingLostControllerEvent;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001225 RID: 4645
		// (remove) Token: 0x06001226 RID: 4646
		event Action OnCompletedHandlingLostEvent;

		// Token: 0x06001227 RID: 4647
		void EnsureUserAndControllerEngaged(bool controllerLostEvent);
	}
}
