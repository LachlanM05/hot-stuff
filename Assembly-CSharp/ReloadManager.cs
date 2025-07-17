using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000AB RID: 171
public class ReloadManager : Singleton<ReloadManager>
{
	// Token: 0x0600057A RID: 1402 RVA: 0x0001F9E4 File Offset: 0x0001DBE4
	public void Start()
	{
		List<IReloadHandler> list = new List<IReloadHandler>();
		List<IResetHandler> list2 = new List<IResetHandler>();
		foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
		{
			list.AddRange(gameObject.GetComponentsInChildren<IReloadHandler>(true));
			list2.AddRange(gameObject.GetComponentsInChildren<IResetHandler>(true));
		}
		IOrderedEnumerable<IReloadHandler> orderedEnumerable = list.OrderBy((IReloadHandler x) => x.Priority());
		if (Singleton<Save>.Instance != null)
		{
			foreach (IReloadHandler reloadHandler in orderedEnumerable)
			{
				Save.onGameSave += reloadHandler.SaveState;
				Save.afterGameLoad += reloadHandler.LoadState;
			}
			foreach (IResetHandler resetHandler in list2)
			{
				Save.afterGameLoadReset += resetHandler.ResetState;
			}
		}
		this.resetsInScene = list2;
		this.reloadsInScene = list;
		this.canContinue = true;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0001FB30 File Offset: 0x0001DD30
	public void LateUpdate()
	{
		if (this.canContinue)
		{
			this.canContinue = false;
			Save.InvokeResetEvent();
			Save.InvokeLoadEvent();
			this.ApplySaveDataToObjs();
		}
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001FB54 File Offset: 0x0001DD54
	private void ApplySaveDataToObjs()
	{
		Object[] array = Object.FindObjectsOfType(typeof(InteractableObj), true);
		for (int i = 0; i < array.Length; i++)
		{
			((InteractableObj)array[i]).LoadSaveData();
		}
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0001FB90 File Offset: 0x0001DD90
	public void OnDestroy()
	{
		if (this.reloadsInScene != null)
		{
			for (int i = 0; i < this.reloadsInScene.Count; i++)
			{
				IReloadHandler reloadHandler = this.reloadsInScene[i];
				if (reloadHandler != null)
				{
					Save.onGameSave -= reloadHandler.SaveState;
					Save.afterGameLoad -= reloadHandler.LoadState;
				}
			}
		}
		if (this.resetsInScene != null)
		{
			for (int j = 0; j < this.resetsInScene.Count; j++)
			{
				IResetHandler resetHandler = this.resetsInScene[j];
				if (resetHandler != null)
				{
					Save.afterGameLoadReset -= resetHandler.ResetState;
				}
			}
		}
	}

	// Token: 0x04000546 RID: 1350
	private bool canContinue;

	// Token: 0x04000547 RID: 1351
	public List<IReloadHandler> reloadsInScene;

	// Token: 0x04000548 RID: 1352
	public List<IResetHandler> resetsInScene;
}
