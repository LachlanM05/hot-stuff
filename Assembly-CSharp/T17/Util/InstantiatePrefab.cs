using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace T17.Util
{
	// Token: 0x02000248 RID: 584
	public class InstantiatePrefab : MonoBehaviour
	{
		// Token: 0x06001313 RID: 4883 RVA: 0x0005B884 File Offset: 0x00059A84
		private void Start()
		{
			for (int i = this._prefabs.Length - 1; i >= 0; i--)
			{
				if (this._prefabs[i] != null && !this.IsPrefabAlreadyInstantiated(this._prefabs[i].AssetGUID))
				{
					GameObject gameObject = this._prefabs[i].InstantiateAsync(Vector3.zero, Quaternion.identity, null).WaitForCompletion();
					if (gameObject != null)
					{
						InstantiatePrefab.PrefabTracking prefabTracking = new InstantiatePrefab.PrefabTracking(this._prefabs[i].AssetGUID, gameObject);
						InstantiatePrefab.instantiatedPrefabs.Add(prefabTracking);
					}
				}
			}
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0005B910 File Offset: 0x00059B10
		private bool IsPrefabAlreadyInstantiated(string assetGuid)
		{
			for (int i = InstantiatePrefab.instantiatedPrefabs.Count - 1; i >= 0; i--)
			{
				if (InstantiatePrefab.instantiatedPrefabs[i].assetGuid == assetGuid)
				{
					if (InstantiatePrefab.instantiatedPrefabs[i].instantiatedObject != null)
					{
						return true;
					}
					InstantiatePrefab.instantiatedPrefabs.RemoveAt(i);
				}
			}
			return false;
		}

		// Token: 0x04000EED RID: 3821
		[SerializeField]
		private AssetReferenceGameObject[] _prefabs = Array.Empty<AssetReferenceGameObject>();

		// Token: 0x04000EEE RID: 3822
		private static List<InstantiatePrefab.PrefabTracking> instantiatedPrefabs = new List<InstantiatePrefab.PrefabTracking>(8);

		// Token: 0x020003C8 RID: 968
		private struct PrefabTracking
		{
			// Token: 0x0600188E RID: 6286 RVA: 0x00070E86 File Offset: 0x0006F086
			public PrefabTracking(string _assetGuid, GameObject _instantiatedObject)
			{
				this.assetGuid = _assetGuid;
				this.instantiatedObject = _instantiatedObject;
			}

			// Token: 0x040014EF RID: 5359
			public string assetGuid;

			// Token: 0x040014F0 RID: 5360
			public GameObject instantiatedObject;
		}
	}
}
