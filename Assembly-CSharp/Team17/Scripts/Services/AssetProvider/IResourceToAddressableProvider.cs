using System;
using UnityEngine.AddressableAssets;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x0200022C RID: 556
	public interface IResourceToAddressableProvider
	{
		// Token: 0x060011FB RID: 4603
		bool TryGetAddressableOverrideForResourceAssetPath(string path, out AssetReference assetReference);
	}
}
