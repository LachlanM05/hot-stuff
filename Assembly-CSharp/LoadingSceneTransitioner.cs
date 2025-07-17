using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020001A7 RID: 423
public class LoadingSceneTransitioner : MonoBehaviour
{
	// Token: 0x06000E6F RID: 3695 RVA: 0x0004FA12 File Offset: 0x0004DC12
	private void Start()
	{
		this._currentState = LoadingSceneTransitioner.State.Idle;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x0004FA2C File Offset: 0x0004DC2C
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x0004FA40 File Offset: 0x0004DC40
	private void Update()
	{
		switch (this._currentState)
		{
		case LoadingSceneTransitioner.State.Idle:
		case LoadingSceneTransitioner.State.Completed:
			break;
		case LoadingSceneTransitioner.State.PostLoadSceneWait:
			if (Time.realtimeSinceStartup > this._transitionEndTime)
			{
				this._transitionEndTime = Time.realtimeSinceStartup + this.FadeDuration;
				this._currentState = LoadingSceneTransitioner.State.FadingOut;
				return;
			}
			break;
		case LoadingSceneTransitioner.State.FadingOut:
			if (this.LoadingScreenCanvasGroup != null)
			{
				float num = (this._transitionEndTime - Time.realtimeSinceStartup) / this.FadeDuration;
				if (num <= 0f)
				{
					this.LoadingScreenCanvasGroup.alpha = 0f;
					this.CompleteTransition();
					return;
				}
				this.LoadingScreenCanvasGroup.alpha = num;
				return;
			}
			else
			{
				this.CompleteTransition();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x0004FAE6 File Offset: 0x0004DCE6
	public void Transition()
	{
		this.SetupPostLoadSceneState();
		this._transitionEndTime = Time.realtimeSinceStartup + this.TransitionLength;
		this._currentState = LoadingSceneTransitioner.State.PostLoadSceneWait;
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x0004FB07 File Offset: 0x0004DD07
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x0004FB09 File Offset: 0x0004DD09
	public void SetupPostLoadSceneState()
	{
		this.LoadingScreenCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		this.ClearImageMaterial(this.LoadingScreenBackgroundLight);
		this.ClearImageMaterial(this.LoadingScreenLogo);
		Object.Destroy(this.LoadingScreenCamera);
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x0004FB3C File Offset: 0x0004DD3C
	private void ClearImageMaterial(GameObject gameObject)
	{
		if (gameObject != null)
		{
			Image component = gameObject.GetComponent<Image>();
			if (component != null)
			{
				component.material = null;
			}
		}
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x0004FB69 File Offset: 0x0004DD69
	private void CompleteTransition()
	{
		this._currentState = LoadingSceneTransitioner.State.Completed;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000CD2 RID: 3282
	private const float kMAxSceneLoadWaitLength = 10f;

	// Token: 0x04000CD3 RID: 3283
	[SerializeField]
	private CanvasGroup LoadingScreenCanvasGroup;

	// Token: 0x04000CD4 RID: 3284
	[SerializeField]
	private Canvas LoadingScreenCanvas;

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	private float FadeDuration = 1f;

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	private float TransitionLength = 2f;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	private GameObject LoadingScreenBackgroundLight;

	// Token: 0x04000CD8 RID: 3288
	[SerializeField]
	private GameObject LoadingScreenLogo;

	// Token: 0x04000CD9 RID: 3289
	[SerializeField]
	private GameObject LoadingScreenCamera;

	// Token: 0x04000CDA RID: 3290
	private float _transitionEndTime = -1f;

	// Token: 0x04000CDB RID: 3291
	private LoadingSceneTransitioner.State _currentState;

	// Token: 0x02000385 RID: 901
	private enum State
	{
		// Token: 0x040013DE RID: 5086
		Idle,
		// Token: 0x040013DF RID: 5087
		PostLoadSceneWait,
		// Token: 0x040013E0 RID: 5088
		FadingOut,
		// Token: 0x040013E1 RID: 5089
		Completed
	}
}
