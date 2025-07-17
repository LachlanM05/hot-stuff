using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000229 RID: 553
	public interface IAssetProviderService : IService
	{
		// Token: 0x060011F0 RID: 4592
		T[] LoadResourceAssets<T>(string path) where T : Object;

		// Token: 0x060011F1 RID: 4593
		T LoadResourceAsset<T>(string path, bool errorAsWarning = false) where T : Object;

		// Token: 0x060011F2 RID: 4594
		void UnloadResourceAsset(Object asset);

		// Token: 0x060011F3 RID: 4595
		T LoadAddressableAsset<T>(AssetReference assetReference) where T : Object;

		// Token: 0x060011F4 RID: 4596
		void UnloadAddressableAsset(Object asset);
	}
}
