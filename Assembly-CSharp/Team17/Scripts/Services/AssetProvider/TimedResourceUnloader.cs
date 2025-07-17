using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x0200022E RID: 558
	public class TimedResourceUnloader
	{
		// Token: 0x060011FF RID: 4607 RVA: 0x00058ECB File Offset: 0x000570CB
		public TimedResourceUnloader(TimeSpan unloadDelay)
		{
			this._unloadDelay = unloadDelay;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00058EF0 File Offset: 0x000570F0
		public void QueueForUnloading(Object loadedAsset)
		{
			if (this.IsQueuedForUnloading(loadedAsset))
			{
				return;
			}
			this._unloadRequests.Add(loadedAsset, DateTime.UtcNow + this._unloadDelay);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00058F18 File Offset: 0x00057118
		public void ProcessQueue()
		{
			this.FillCacheWithExpiredDeleteRequests();
			this.UnloadAllAssetsInUnloadCache();
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00058F26 File Offset: 0x00057126
		public bool IsQueuedForUnloading(Object loadedAsset)
		{
			return this._unloadRequests.ContainsKey(loadedAsset);
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00058F34 File Offset: 0x00057134
		public void Remove(Object loadedAsset)
		{
			this._unloadRequests.Remove(loadedAsset);
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00058F44 File Offset: 0x00057144
		private void FillCacheWithExpiredDeleteRequests()
		{
			DateTime utcNow = DateTime.UtcNow;
			foreach (KeyValuePair<Object, DateTime> keyValuePair in this._unloadRequests)
			{
				Object @object;
				DateTime dateTime;
				keyValuePair.Deconstruct(out @object, out dateTime);
				Object object2 = @object;
				if (dateTime < utcNow)
				{
					this._unloadCache.Add(object2);
				}
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00058FBC File Offset: 0x000571BC
		private void UnloadAllAssetsInUnloadCache()
		{
			foreach (Object @object in this._unloadCache)
			{
				this._unloadRequests.Remove(@object);
				Resources.UnloadAsset(@object);
			}
			this._unloadCache.Clear();
		}

		// Token: 0x04000E73 RID: 3699
		private readonly TimeSpan _unloadDelay;

		// Token: 0x04000E74 RID: 3700
		private readonly Dictionary<Object, DateTime> _unloadRequests = new Dictionary<Object, DateTime>();

		// Token: 0x04000E75 RID: 3701
		private readonly List<Object> _unloadCache = new List<Object>();
	}
}
