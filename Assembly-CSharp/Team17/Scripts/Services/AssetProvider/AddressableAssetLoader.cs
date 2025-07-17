using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000223 RID: 547
	public class AddressableAssetLoader : IAddressableAssetLoader, IService
	{
		// Token: 0x060011BE RID: 4542 RVA: 0x000585DC File Offset: 0x000567DC
		public T LoadAddressableAsset<T>(AssetReference assetReference) where T : Object
		{
			AssetProviderExtensions.EnsureCanLoadAssets();
			AsyncOperationHandle<T> asyncOperationHandle = Addressables.LoadAssetAsync<T>(assetReference);
			asyncOperationHandle.WaitForCompletion();
			if (asyncOperationHandle.Status == AsyncOperationStatus.Failed)
			{
				string text = ((assetReference != null) ? assetReference.AssetGUID : string.Empty);
				T17Debug.LogError("Failed to load asset " + text + " from addressable asset bundle");
				return default(T);
			}
			return asyncOperationHandle.Result;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0005863E File Offset: 0x0005683E
		public void UnloadAddressableAsset(Object asset)
		{
			Addressables.Release<Object>(asset);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00058646 File Offset: 0x00056846
		public void OnStartUp()
		{
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00058648 File Offset: 0x00056848
		public void OnCleanUp()
		{
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0005864A File Offset: 0x0005684A
		public void OnUpdate(float deltaTime)
		{
		}
	}
}
