using System;
using Team17.Common;
using Team17.Platform.EngagementService;
using Team17.Platform.Entitlements;
using Team17.Platform.Entitlements.Steam;
using Team17.Platform.User;
using Team17.Platform.User.Steam;
using Team17.Platform.UserService;

namespace Team17.Scripts.Services.Entitlement
{
	// Token: 0x02000221 RID: 545
	public class MirandaEntitlementService : IMirandaEntitlementService, IService
	{
		// Token: 0x060011AB RID: 4523 RVA: 0x00058454 File Offset: 0x00056654
		public MirandaEntitlementService(IEngagementService EngagementService, IUserService UserService, ProductDefinition[] allProductDefinitions)
		{
			this._engagementService = EngagementService;
			this._userService = UserService;
			this.m_allProducts = allProductDefinitions;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00058474 File Offset: 0x00056674
		public void OnStartUp()
		{
			this.m_AppId = new AppIdDefinition();
			this._engagementService.OnPrimaryUserEngagedEvent += this.OnPrimaryUserEngaged;
			IUser user;
			if (this._userService.TryGetPrimaryUser(out user))
			{
				this.SetupOnStart(user);
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000584B9 File Offset: 0x000566B9
		public void OnUpdate(float deltaTime)
		{
			if (this._impl != null)
			{
				this._impl.OnUpdate(deltaTime);
			}
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x000584CF File Offset: 0x000566CF
		public void OnCleanUp()
		{
			this._engagementService.OnPrimaryUserEngagedEvent -= this.OnPrimaryUserEngaged;
			if (this._impl != null)
			{
				this._impl.OnCleanUp();
			}
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000584FC File Offset: 0x000566FC
		private void OnPrimaryUserEngaged()
		{
			IUser user;
			if (this._userService.TryGetPrimaryUser(out user))
			{
				this.SetupOnStart(user);
			}
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0005851F File Offset: 0x0005671F
		public void SetupOnStart(IUser primaryUser)
		{
			if (this._impl == null)
			{
				this._impl = new EntitlementsService<SteamEntitlementsImpl, SteamUser, ISteamProductIdProvider, ISteamAppIdProvider>();
			}
			this.SetUser(primaryUser);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0005853B File Offset: 0x0005673B
		public int GetProductCount()
		{
			return this._impl.GetProductCount();
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00058548 File Offset: 0x00056748
		public bool IsProductOwned(IProductIdProvider productId)
		{
			return this._impl.IsProductOwned(productId);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00058556 File Offset: 0x00056756
		public void RegisterListener(IEntitlementListener listener)
		{
			this._impl.RegisterListener(listener);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00058564 File Offset: 0x00056764
		public void SetUser(IUser user)
		{
			this._impl.SetUser(user);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00058572 File Offset: 0x00056772
		public bool ShowStore(IAppIdProvider appId, IProductIdProvider productId)
		{
			return this._impl.ShowStore(appId, productId);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00058581 File Offset: 0x00056781
		public bool ShowStore(IAppIdProvider appId)
		{
			return this._impl.ShowStore(appId);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0005858F File Offset: 0x0005678F
		public bool ShowStore()
		{
			return this.ShowStore(this.m_AppId);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0005859D File Offset: 0x0005679D
		public bool ShowStore(ProductDefinition product)
		{
			return this.ShowStore(this.m_AppId, product);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x000585AC File Offset: 0x000567AC
		public void UnregisterListener(IEntitlementListener listener)
		{
			this._impl.UnregisterListener(listener);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x000585BA File Offset: 0x000567BA
		public bool IsPreOrderOwned()
		{
			return this.IsProductOwned(this.m_allProducts[0]);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x000585CA File Offset: 0x000567CA
		public bool IsDexluxeOwned()
		{
			return this.IsProductOwned(this.m_allProducts[1]);
		}

		// Token: 0x04000E60 RID: 3680
		private IEngagementService _engagementService;

		// Token: 0x04000E61 RID: 3681
		private IUserService _userService;

		// Token: 0x04000E62 RID: 3682
		private IEntitlementsService _impl;

		// Token: 0x04000E63 RID: 3683
		private AppIdDefinition m_AppId;

		// Token: 0x04000E64 RID: 3684
		private ProductDefinition[] m_allProducts;
	}
}
