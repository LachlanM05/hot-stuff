using System;
using System.Diagnostics;
using System.Linq;
using Team17.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000228 RID: 552
	public class DebugAssetProviderServiceDecorator : IAssetProviderService, IService
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00058A29 File Offset: 0x00056C29
		public UnityObjectReferenceCounter ResourcesAssetRefCounter
		{
			get
			{
				return this._resourcesAssetRefCounter;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x00058A31 File Offset: 0x00056C31
		public UnityObjectReferenceCounter AddressableAssetRefCounter
		{
			get
			{
				return this._addressableAssetRefCounter;
			}
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00058A39 File Offset: 0x00056C39
		public DebugAssetProviderServiceDecorator(IAssetProviderService actualServiceImpl)
		{
			this._actualServiceImpl = actualServiceImpl;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00058A5E File Offset: 0x00056C5E
		public void OnStartUp()
		{
			this._actualServiceImpl.OnStartUp();
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00058A6B File Offset: 0x00056C6B
		public void OnCleanUp()
		{
			this._actualServiceImpl.OnCleanUp();
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00058A78 File Offset: 0x00056C78
		public void OnUpdate(float deltaTime)
		{
			this._actualServiceImpl.OnUpdate(deltaTime);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00058A88 File Offset: 0x00056C88
		public T LoadResourceAsset<T>(string path, bool errorAsWarning = false) where T : Object
		{
			T t2;
			using (new DebugAssetProviderServiceDecorator.TimeTrackLogger("[ASSET] Loading asset '" + path + "'"))
			{
				T t = this._actualServiceImpl.LoadResourceAsset<T>(path, errorAsWarning);
				if (t != null)
				{
					this._resourcesAssetRefCounter.IncreaseResourceLoadedAssetRefCount(t);
				}
				t2 = t;
			}
			return t2;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00058AF8 File Offset: 0x00056CF8
		public T[] LoadResourceAssets<T>(string path) where T : Object
		{
			T[] array2;
			using (new DebugAssetProviderServiceDecorator.TimeTrackLogger("[ASSET] Loading assets at '" + path + "'"))
			{
				T[] array = this._actualServiceImpl.LoadResourceAssets<T>(path);
				foreach (T t in array)
				{
					this._resourcesAssetRefCounter.IncreaseResourceLoadedAssetRefCount(t);
				}
				array2 = array;
			}
			return array2;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00058B74 File Offset: 0x00056D74
		public void UnloadResourceAsset(Object asset)
		{
			using (new DebugAssetProviderServiceDecorator.TimeTrackLogger(string.Format("[ASSET] Unloading asset '{0}'", asset)))
			{
				this._resourcesAssetRefCounter.DecreaseResourceLoadedAssetRefCount(asset);
				this._actualServiceImpl.UnloadResourceAsset(asset);
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00058BC8 File Offset: 0x00056DC8
		public T LoadAddressableAsset<T>(AssetReference assetReference) where T : Object
		{
			T t2;
			using (new DebugAssetProviderServiceDecorator.TimeTrackLogger(string.Format("[ASSET] Loading addressable asset '{0}'", assetReference)))
			{
				T t = this._actualServiceImpl.LoadAddressableAsset<T>(assetReference);
				this._addressableAssetRefCounter.IncreaseResourceLoadedAssetRefCount(t);
				t2 = t;
			}
			return t2;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00058C24 File Offset: 0x00056E24
		public void UnloadAddressableAsset(Object asset)
		{
			using (new DebugAssetProviderServiceDecorator.TimeTrackLogger(string.Format("[ASSET] Unloading addressable asset '{0}'", asset)))
			{
				this._addressableAssetRefCounter.DecreaseResourceLoadedAssetRefCount(asset);
				this._actualServiceImpl.UnloadAddressableAsset(asset);
			}
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00058C78 File Offset: 0x00056E78
		public int GetTypeCount<T>() where T : Object
		{
			return this.ResourcesAssetRefCounter.LoadedAssetReferenceCount.Keys.OfType<T>().Count<T>() + this.AddressableAssetRefCounter.LoadedAssetReferenceCount.Keys.OfType<T>().Count<T>();
		}

		// Token: 0x04000E6E RID: 3694
		private readonly IAssetProviderService _actualServiceImpl;

		// Token: 0x04000E6F RID: 3695
		private UnityObjectReferenceCounter _addressableAssetRefCounter = new UnityObjectReferenceCounter();

		// Token: 0x04000E70 RID: 3696
		private UnityObjectReferenceCounter _resourcesAssetRefCounter = new UnityObjectReferenceCounter();

		// Token: 0x020003C2 RID: 962
		private class TimeTrackLogger : IDisposable
		{
			// Token: 0x06001883 RID: 6275 RVA: 0x00070D41 File Offset: 0x0006EF41
			public TimeTrackLogger(string messagePrefix)
			{
				this._messagePrefix = messagePrefix;
				this._stopwatch = Stopwatch.StartNew();
			}

			// Token: 0x06001884 RID: 6276 RVA: 0x00070D5B File Offset: 0x0006EF5B
			public void Dispose()
			{
				this._stopwatch.Stop();
			}

			// Token: 0x040014E4 RID: 5348
			private readonly string _messagePrefix;

			// Token: 0x040014E5 RID: 5349
			private readonly Stopwatch _stopwatch;
		}
	}
}
