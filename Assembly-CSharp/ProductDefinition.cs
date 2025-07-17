using System;
using Steamworks;
using Team17.Platform.Entitlements;
using Team17.Platform.Entitlements.Steam;
using UnityEngine;

// Token: 0x0200018F RID: 399
[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Entitlements/Product", order = 100)]
public class ProductDefinition : ScriptableObject, ISteamProductIdProvider, IProductIdProvider
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0004D67E File Offset: 0x0004B87E
	AppId_t ISteamProductIdProvider.Id
	{
		get
		{
			return new AppId_t(this.m_SteamId);
		}
	}

	// Token: 0x04000C55 RID: 3157
	[SerializeField]
	private string m_NullId;

	// Token: 0x04000C56 RID: 3158
	[SerializeField]
	private string m_SwitchId;

	// Token: 0x04000C57 RID: 3159
	[SerializeField]
	private string m_GameCoreId;

	// Token: 0x04000C58 RID: 3160
	[SerializeField]
	private string[] m_PS5Ids;

	// Token: 0x04000C59 RID: 3161
	[SerializeField]
	private uint m_SteamId;
}
