using System;
using T17.Flow;
using Team17.Common;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class LoadScene : MonoBehaviour
{
	// Token: 0x06000D1D RID: 3357 RVA: 0x0004B5D5 File Offset: 0x000497D5
	private void Start()
	{
		if (this.whenToLoad == LoadScene.LoadWhen.Auto)
		{
			this.loadSceneRequested = true;
		}
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0004B5E6 File Offset: 0x000497E6
	private void Update()
	{
		if (this.loadSceneRequested)
		{
			this.TriggerLoadScene();
		}
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0004B5F6 File Offset: 0x000497F6
	public void RequestLoadScene(bool immediate)
	{
		if (immediate)
		{
			this.TriggerLoadScene();
			return;
		}
		this.loadSceneRequested = true;
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x0004B60C File Offset: 0x0004980C
	public void TriggerLoadScene()
	{
		if (!this.alreadyTriggered)
		{
			this.alreadyTriggered = true;
			this.loadSceneRequested = false;
			base.enabled = false;
			if (string.IsNullOrWhiteSpace(this.sceneToLoad))
			{
				T17Debug.LogError("Requested empty scene to load");
				return;
			}
			if (!SceneTransitionManager.TransitionToScene(this.sceneToLoad))
			{
				T17Debug.LogError("Requested load scene (" + this.sceneToLoad + ") Failed");
			}
		}
	}

	// Token: 0x04000BD9 RID: 3033
	[SerializeField]
	private string sceneToLoad = "";

	// Token: 0x04000BDA RID: 3034
	[SerializeField]
	private LoadScene.LoadWhen whenToLoad;

	// Token: 0x04000BDB RID: 3035
	private bool alreadyTriggered;

	// Token: 0x04000BDC RID: 3036
	private bool loadSceneRequested;

	// Token: 0x02000365 RID: 869
	private enum LoadWhen
	{
		// Token: 0x04001359 RID: 4953
		Auto,
		// Token: 0x0400135A RID: 4954
		WhenTriggered
	}
}
