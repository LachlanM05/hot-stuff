using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000222 RID: 546
	public interface IAddressableAssetLoader : IService
	{
		// Token: 0x060011BC RID: 4540
		T LoadAddressableAsset<T>(AssetReference assetReference) where T : Object;

		// Token: 0x060011BD RID: 4541
		void UnloadAddressableAsset(Object asset);
	}
}
