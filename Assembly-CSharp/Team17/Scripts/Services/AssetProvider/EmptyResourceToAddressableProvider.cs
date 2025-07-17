using System;
using UnityEngine.AddressableAssets;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x0200022B RID: 555
	public class EmptyResourceToAddressableProvider : IResourceToAddressableProvider
	{
		// Token: 0x060011F9 RID: 4601 RVA: 0x00058E7F File Offset: 0x0005707F
		public bool TryGetAddressableOverrideForResourceAssetPath(string path, out AssetReference assetReference)
		{
			assetReference = null;
			return false;
		}
	}
}
