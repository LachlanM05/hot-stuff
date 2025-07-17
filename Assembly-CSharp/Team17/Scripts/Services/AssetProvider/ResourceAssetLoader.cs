using System;
using System.Collections.Generic;
using Team17.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000225 RID: 549
	public class ResourceAssetLoader : IResourceAssetLoader, IService
	{
		// Token: 0x060011C7 RID: 4551 RVA: 0x00058654 File Offset: 0x00056854
		public ResourceAssetLoader(IAddressableAssetLoader addressableAssetLoader, IResourceToAddressableProvider resourceAssetOverrideToAddressableReference = null)
		{
			this._addressableAssetLoader = addressableAssetLoader;
			this._resourceAssetOverrideToAddressableReference = resourceAssetOverrideToAddressableReference ?? new EmptyResourceToAddressableProvider();
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x000586A4 File Offset: 0x000568A4
		public void OnStartUp()
		{
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x000586A6 File Offset: 0x000568A6
		public void OnCleanUp()
		{
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000586A8 File Offset: 0x000568A8
		public void OnUpdate(float deltaTime)
		{
			this._timedResourceUnloader.ProcessQueue();
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000586B8 File Offset: 0x000568B8
		public T LoadResourceAsset<T>(string path, bool errorAsWarning = false) where T : Object
		{
			AssetReference assetReference;
			if (this._resourceAssetOverrideToAddressableReference.TryGetAddressableOverrideForResourceAssetPath(path, out assetReference))
			{
				T t = this._addressableAssetLoader.LoadAddressableAsset<T>(assetReference);
				this._addressableLoadedAssets.Add(t);
				return t;
			}
			T t2 = Resources.Load<T>(path);
			if (t2 == null)
			{
				if (!errorAsWarning)
				{
					T17Debug.LogError("[ASSET] No asset to resource load at '" + path + "'");
				}
				return default(T);
			}
			this.IncreaseResourceLoadedAssetRefCount(t2);
			return t2;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00058739 File Offset: 0x00056939
		public void UnloadResourceAsset(Object asset)
		{
			if (this.IsAssetAddressableLoaded(asset))
			{
				this._addressableLoadedAssets.Remove(asset);
				this._addressableAssetLoader.UnloadAddressableAsset(asset);
				return;
			}
			this.DecreaseResourceLoadedAssetRefCount(asset);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00058768 File Offset: 0x00056968
		public T[] LoadResourceAssets<T>(string path) where T : Object
		{
			T[] array = Resources.LoadAll<T>(path);
			foreach (T t in array)
			{
				this.IncreaseResourceLoadedAssetRefCount(t);
			}
			return array;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x000587A1 File Offset: 0x000569A1
		private void IncreaseResourceLoadedAssetRefCount(Object loadedAsset)
		{
			this.EnsureResourceIsNotQueuedForUnloading(loadedAsset);
			this._resourceLoadedAssetsReferenceCounter.IncreaseResourceLoadedAssetRefCount(loadedAsset);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000587B6 File Offset: 0x000569B6
		private void DecreaseResourceLoadedAssetRefCount(Object loadedAsset)
		{
			if (!this._resourceLoadedAssetsReferenceCounter.DecreaseResourceLoadedAssetRefCount(loadedAsset))
			{
				return;
			}
			this.QueueResourceForUnloading(loadedAsset);
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x000587CE File Offset: 0x000569CE
		private void EnsureResourceIsNotQueuedForUnloading(Object loadedAsset)
		{
			if (!this._timedResourceUnloader.IsQueuedForUnloading(loadedAsset))
			{
				return;
			}
			this._timedResourceUnloader.Remove(loadedAsset);
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x000587EB File Offset: 0x000569EB
		private void QueueResourceForUnloading(Object loadedAsset)
		{
			this._timedResourceUnloader.QueueForUnloading(loadedAsset);
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000587F9 File Offset: 0x000569F9
		private bool IsAssetAddressableLoaded(Object loadedAsset)
		{
			return this._addressableLoadedAssets.Contains(loadedAsset);
		}

		// Token: 0x04000E65 RID: 3685
		private static readonly TimeSpan UnloadDelay = TimeSpan.FromSeconds(1.0);

		// Token: 0x04000E66 RID: 3686
		private readonly IResourceToAddressableProvider _resourceAssetOverrideToAddressableReference;

		// Token: 0x04000E67 RID: 3687
		private readonly IAddressableAssetLoader _addressableAssetLoader;

		// Token: 0x04000E68 RID: 3688
		private readonly List<object> _addressableLoadedAssets = new List<object>();

		// Token: 0x04000E69 RID: 3689
		private readonly UnityObjectReferenceCounter _resourceLoadedAssetsReferenceCounter = new UnityObjectReferenceCounter();

		// Token: 0x04000E6A RID: 3690
		private readonly TimedResourceUnloader _timedResourceUnloader = new TimedResourceUnloader(ResourceAssetLoader.UnloadDelay);
	}
}
