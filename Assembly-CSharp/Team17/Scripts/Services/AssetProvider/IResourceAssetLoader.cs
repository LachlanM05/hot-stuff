using System;
using Team17.Common;
using UnityEngine;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x02000224 RID: 548
	public interface IResourceAssetLoader : IService
	{
		// Token: 0x060011C4 RID: 4548
		T[] LoadResourceAssets<T>(string path) where T : Object;

		// Token: 0x060011C5 RID: 4549
		T LoadResourceAsset<T>(string path, bool errorAsWarning = false) where T : Object;

		// Token: 0x060011C6 RID: 4550
		void UnloadResourceAsset(Object asset);
	}
}
