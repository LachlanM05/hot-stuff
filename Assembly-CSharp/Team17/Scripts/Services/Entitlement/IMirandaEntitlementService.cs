using System;
using Team17.Common;
using Team17.Platform.Entitlements;
using Team17.Platform.User;

namespace Team17.Scripts.Services.Entitlement
{
	// Token: 0x02000220 RID: 544
	public interface IMirandaEntitlementService : IService
	{
		// Token: 0x060011A2 RID: 4514
		int GetProductCount();

		// Token: 0x060011A3 RID: 4515
		bool IsProductOwned(IProductIdProvider productId);

		// Token: 0x060011A4 RID: 4516
		void RegisterListener(IEntitlementListener listener);

		// Token: 0x060011A5 RID: 4517
		void SetUser(IUser user);

		// Token: 0x060011A6 RID: 4518
		bool ShowStore(IAppIdProvider appId, IProductIdProvider productId);

		// Token: 0x060011A7 RID: 4519
		bool ShowStore(IAppIdProvider appId);

		// Token: 0x060011A8 RID: 4520
		bool ShowStore();

		// Token: 0x060011A9 RID: 4521
		bool ShowStore(ProductDefinition product);

		// Token: 0x060011AA RID: 4522
		void UnregisterListener(IEntitlementListener listener);
	}
}
