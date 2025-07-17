using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x0200022D RID: 557
	[CreateAssetMenu(menuName = "Team17/ResourceToAddressableOverrideProvider", fileName = "ResourceToAddressableOverrideProvider", order = 0)]
	public class ResourceToAddressableOverrideProvider : ScriptableObject, IResourceToAddressableProvider
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x00058E8D File Offset: 0x0005708D
		public bool TryGetAddressableOverrideForResourceAssetPath(string path, out AssetReference assetReference)
		{
			return this._overrides.TryGetValue(ResourceToAddressableOverrideProvider.NormalizePath(path), out assetReference);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00058EA1 File Offset: 0x000570A1
		private static string NormalizePath(string path)
		{
			return path.Replace("\\", "/").ToLowerInvariant();
		}

		// Token: 0x04000E71 RID: 3697
		[SerializeField]
		private Object _addressableResourcesFolder;

		// Token: 0x04000E72 RID: 3698
		[SerializeField]
		private ResourceToAddressableOverrideProvider.AssetReferenceDictionary _overrides = new ResourceToAddressableOverrideProvider.AssetReferenceDictionary();

		// Token: 0x020003C3 RID: 963
		[Serializable]
		private sealed class AssetReferenceDictionary : global::UnityEngine.Rendering.SerializedDictionary<string, AssetReference>
		{
		}
	}
}
