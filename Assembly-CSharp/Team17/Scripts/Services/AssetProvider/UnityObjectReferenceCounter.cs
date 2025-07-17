using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x0200022F RID: 559
	public class UnityObjectReferenceCounter
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x00059028 File Offset: 0x00057228
		public int AssetCount
		{
			get
			{
				return this._resourceLoadedAssetsRefCount.Count;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x00059035 File Offset: 0x00057235
		public IReadOnlyDictionary<Object, int> LoadedAssetReferenceCount
		{
			get
			{
				return this._resourceLoadedAssetsRefCount;
			}
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00059040 File Offset: 0x00057240
		public int GetReferenceCountForAsset(Object asset)
		{
			int num;
			if (!this._resourceLoadedAssetsRefCount.TryGetValue(asset, out num))
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00059060 File Offset: 0x00057260
		public void IncreaseResourceLoadedAssetRefCount(Object loadedAsset)
		{
			int num;
			if (!this._resourceLoadedAssetsRefCount.TryGetValue(loadedAsset, out num))
			{
				num = 0;
			}
			this._resourceLoadedAssetsRefCount[loadedAsset] = num + 1;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00059090 File Offset: 0x00057290
		public bool DecreaseResourceLoadedAssetRefCount(Object loadedAsset)
		{
			int num;
			if (!this._resourceLoadedAssetsRefCount.TryGetValue(loadedAsset, out num))
			{
				return false;
			}
			if (num - 1 > 0)
			{
				this._resourceLoadedAssetsRefCount[loadedAsset] = num - 1;
				return false;
			}
			this._resourceLoadedAssetsRefCount.Remove(loadedAsset);
			return true;
		}

		// Token: 0x04000E76 RID: 3702
		private readonly Dictionary<Object, int> _resourceLoadedAssetsRefCount = new Dictionary<Object, int>();
	}
}
