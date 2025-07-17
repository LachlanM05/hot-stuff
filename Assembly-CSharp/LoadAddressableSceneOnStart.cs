using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

// Token: 0x0200016F RID: 367
public class LoadAddressableSceneOnStart : MonoBehaviour
{
	// Token: 0x06000D19 RID: 3353 RVA: 0x0004B5A5 File Offset: 0x000497A5
	private void Start()
	{
		Addressables.LoadSceneAsync(this.sceneToLoad, LoadSceneMode.Single, true, 100);
	}

	// Token: 0x04000BD8 RID: 3032
	[SerializeField]
	private string sceneToLoad = SceneConsts.kIdentScene;
}
