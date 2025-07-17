using System;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.LowLevel;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000226 RID: 550
	public static class AssetProviderExtensions
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x0005881C File Offset: 0x00056A1C
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x00058823 File Offset: 0x00056A23
		public static bool CanDoAsyncAssetLoading { get; private set; }

		// Token: 0x060011D6 RID: 4566 RVA: 0x0005882C File Offset: 0x00056A2C
		public static T LoadResourceAsset<T>(this IAssetProviderService assetProviderService, string path, T currentLoaded) where T : Object
		{
			T t = assetProviderService.LoadResourceAsset<T>(path, false);
			if (currentLoaded != null && t != currentLoaded)
			{
				assetProviderService.UnloadResourceAsset(currentLoaded);
			}
			return t;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0005886C File Offset: 0x00056A6C
		public static T LoadAddressableAsset<T>(this IAssetProviderService assetProviderService, AssetReference assetReference, T currentLoaded) where T : Object
		{
			T t = assetProviderService.LoadAddressableAsset<T>(assetReference);
			if (currentLoaded != null && t != currentLoaded)
			{
				assetProviderService.UnloadAddressableAsset(currentLoaded);
			}
			return t;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000588AA File Offset: 0x00056AAA
		public static void EnsureCanLoadAssets()
		{
			if (!AssetProviderExtensions.CanDoAsyncAssetLoading)
			{
				throw new UnityException("Attempting to synchronously load assets in Awake, Start, or OnSceneLoad. This is not supported and will cause a hang in builds. You must do this after, or in, the first Update instead.");
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000588C0 File Offset: 0x00056AC0
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void SetupPlayerLoop()
		{
			PlayerLoopSystem playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(AssetProviderService);
			playerLoopSystem.updateDelegate = delegate
			{
				AssetProviderExtensions.SetAllowAsyncLoading(false);
			};
			PlayerLoopSystemInserter.InsertAfterAwake(in playerLoopSystem);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(AssetProviderService);
			playerLoopSystem.updateDelegate = delegate
			{
				AssetProviderExtensions.SetAllowAsyncLoading(true);
			};
			PlayerLoopSystemInserter.InsertBeforeUpdate(in playerLoopSystem);
			StringBuilder stringBuilder = new StringBuilder();
			PlayerLoopSystemInserter.RecursivePlayerLoopPrint(PlayerLoop.GetCurrentPlayerLoop(), stringBuilder, 0);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0005896B File Offset: 0x00056B6B
		private static void SetAllowAsyncLoading(bool enabled)
		{
			AssetProviderExtensions.CanDoAsyncAssetLoading = enabled;
		}
	}
}
