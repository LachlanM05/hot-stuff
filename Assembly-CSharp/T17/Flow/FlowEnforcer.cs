using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace T17.Flow
{
	// Token: 0x0200024A RID: 586
	public class FlowEnforcer : MonoBehaviour
	{
		// Token: 0x06001323 RID: 4899 RVA: 0x0005BE49 File Offset: 0x0005A049
		private void Awake()
		{
			if (!SceneTransitionManager.IsInitialised())
			{
				this.DeactivateAndDestroyAlllGameObjects();
				return;
			}
			base.enabled = false;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0005BE60 File Offset: 0x0005A060
		private void OnEnable()
		{
			IdentManager.EnableForceSkip();
			SceneManager.LoadScene(this.sceneToLoad, LoadSceneMode.Single);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0005BE74 File Offset: 0x0005A074
		private void DeactivateAndDestroyAlllGameObjects()
		{
			int instanceID = base.gameObject.GetInstanceID();
			GameObject[] array = Object.FindObjectsOfType<GameObject>();
			for (int i = array.Length - 1; i >= 0; i--)
			{
				if (array[i].GetInstanceID() != instanceID)
				{
					Object.Destroy(array[i]);
					array[i].SetActive(false);
				}
			}
		}

		// Token: 0x04000F02 RID: 3842
		public string sceneToLoad = "Boot";
	}
}
