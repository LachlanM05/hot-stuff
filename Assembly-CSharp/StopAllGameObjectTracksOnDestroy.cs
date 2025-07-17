using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class StopAllGameObjectTracksOnDestroy : MonoBehaviour
{
	// Token: 0x06000715 RID: 1813 RVA: 0x00027DF4 File Offset: 0x00025FF4
	private void OnDestroy()
	{
		AudioManager instance = Singleton<AudioManager>.Instance;
		if (instance)
		{
			instance.StopAllTracksForGameObject(base.gameObject);
		}
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00027E1B File Offset: 0x0002601B
	public static void EnsureHasComponent(GameObject gameObject)
	{
		if (gameObject.GetComponent<StopAllGameObjectTracksOnDestroy>() != null)
		{
			return;
		}
		gameObject.AddComponent<StopAllGameObjectTracksOnDestroy>();
	}
}
