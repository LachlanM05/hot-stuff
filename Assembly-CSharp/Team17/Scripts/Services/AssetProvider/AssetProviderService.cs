using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000227 RID: 551
	public class AssetProviderService : IAssetProviderService, IService
	{
		// Token: 0x060011DB RID: 4571 RVA: 0x00058973 File Offset: 0x00056B73
		public AssetProviderService(IResourceToAddressableProvider resourceAssetOverrideToAddressableReference = null)
		{
			this._addressableAssetLoader = new AddressableAssetLoader();
			this._resourceAssetLoader = new ResourceAssetLoader(this._addressableAssetLoader, resourceAssetOverrideToAddressableReference);
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00058998 File Offset: 0x00056B98
		public void OnStartUp()
		{
			this._resourceAssetLoader.OnStartUp();
			this._addressableAssetLoader.OnStartUp();
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000589B0 File Offset: 0x00056BB0
		public void OnCleanUp()
		{
			this._resourceAssetLoader.OnCleanUp();
			this._addressableAssetLoader.OnCleanUp();
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000589C8 File Offset: 0x00056BC8
		public void OnUpdate(float deltaTime)
		{
			this._resourceAssetLoader.OnUpdate(deltaTime);
			this._addressableAssetLoader.OnUpdate(deltaTime);
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x000589E2 File Offset: 0x00056BE2
		public T[] LoadResourceAssets<T>(string path) where T : Object
		{
			return this._resourceAssetLoader.LoadResourceAssets<T>(path);
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000589F0 File Offset: 0x00056BF0
		public T LoadResourceAsset<T>(string path, bool errorAsWarning) where T : Object
		{
			return this._resourceAssetLoader.LoadResourceAsset<T>(path, errorAsWarning);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000589FF File Offset: 0x00056BFF
		public void UnloadResourceAsset(Object asset)
		{
			this._resourceAssetLoader.UnloadResourceAsset(asset);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00058A0D File Offset: 0x00056C0D
		public T LoadAddressableAsset<T>(AssetReference assetReference) where T : Object
		{
			return this._addressableAssetLoader.LoadAddressableAsset<T>(assetReference);
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00058A1B File Offset: 0x00056C1B
		public void UnloadAddressableAsset(Object asset)
		{
			this._addressableAssetLoader.UnloadAddressableAsset(asset);
		}

		// Token: 0x04000E6C RID: 3692
		private readonly IResourceAssetLoader _resourceAssetLoader;

		// Token: 0x04000E6D RID: 3693
		private readonly IAddressableAssetLoader _addressableAssetLoader;
	}
}
