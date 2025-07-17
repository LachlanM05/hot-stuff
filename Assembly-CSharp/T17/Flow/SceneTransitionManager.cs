using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Date_Everything.Scripts.Ink;
using T17.Services;
using Team17.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace T17.Flow
{
	// Token: 0x02000252 RID: 594
	public class SceneTransitionManager : MonoBehaviour
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600135E RID: 4958 RVA: 0x0005C9A8 File Offset: 0x0005ABA8
		// (remove) Token: 0x0600135F RID: 4959 RVA: 0x0005C9DC File Offset: 0x0005ABDC
		public static event UnityAction<string> sceneActivated;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001360 RID: 4960 RVA: 0x0005CA10 File Offset: 0x0005AC10
		// (remove) Token: 0x06001361 RID: 4961 RVA: 0x0005CA44 File Offset: 0x0005AC44
		public static event UnityAction<string> finished;

		// Token: 0x06001362 RID: 4962 RVA: 0x0005CA77 File Offset: 0x0005AC77
		public static bool IsInitialised()
		{
			return SceneTransitionManager.s_Instance != null;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0005CA84 File Offset: 0x0005AC84
		public static bool IsFinished()
		{
			return SceneTransitionManager.s_Instance != null && SceneTransitionManager.s_Instance.transitionStage == null;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0005CAA4 File Offset: 0x0005ACA4
		private void Awake()
		{
			bool flag = this.loadingScenes.Length != 0;
			if (SceneTransitionManager.s_Instance != null)
			{
				flag = false;
			}
			else
			{
				int num = this.loadingScenes.Length - 1;
				while (num >= 0 && flag)
				{
					if (string.IsNullOrWhiteSpace(this.loadingScenes[num]))
					{
						T17Debug.LogError("SceneTransitionManager has at least one blank entry in the 'Loading Scenes' list");
						flag = false;
					}
					num--;
				}
			}
			if (flag)
			{
				Object.DontDestroyOnLoad(base.gameObject);
				SceneTransitionManager.s_Instance = this;
				return;
			}
			Object.Destroy(this);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0005CB1B File Offset: 0x0005AD1B
		public static bool TransitionToScene(string sceneName)
		{
			if (SceneTransitionManager.s_Instance == null)
			{
				T17Debug.LogError("SceneTransitionManager has not been created");
				return false;
			}
			return SceneTransitionManager.s_Instance.TransitionToSceneInternal(sceneName);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0005CB44 File Offset: 0x0005AD44
		private bool TransitionToSceneInternal(string sceneName)
		{
			if (this.transitionStage != null && !string.IsNullOrEmpty(this.desinationSceneName))
			{
				T17Debug.LogError("Can not transition scenes while transition manger is busy");
				return false;
			}
			this.desinationSceneName = sceneName;
			if (!string.IsNullOrWhiteSpace(this.desinationSceneName) && SceneManager.GetSceneByName(this.desinationSceneName).IsValid())
			{
				return false;
			}
			if (this.transitionStage == null)
			{
				this.loadingSceneName = this.loadingScenes[Random.Range(0, this.loadingScenes.Length)];
				if (SceneManager.GetSceneByName(this.loadingSceneName).IsValid())
				{
					T17Debug.LogError("The loading scene (" + this.loadingSceneName + ") is already loaded");
					return false;
				}
				this.transitionStage = new SceneTransitionManager.TransitionStage(this.Load_LoadingScene);
			}
			return true;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0005CC04 File Offset: 0x0005AE04
		private void Update()
		{
			if (this.transitionStage != null)
			{
				this.transitionStage();
			}
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0005CC19 File Offset: 0x0005AE19
		private void Load_LoadingScene()
		{
			this.HideCursor();
			this.EnableLoadingSpeedup(true);
			this.EnableBoostMode(true);
			Addressables.LoadSceneAsync(this.loadingSceneName, LoadSceneMode.Single, true, 100);
			this.transitionStage = new SceneTransitionManager.TransitionStage(this.Wait_LoadingSceneLoaded);
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0005CC54 File Offset: 0x0005AE54
		private void Wait_LoadingSceneLoaded()
		{
			if (string.CompareOrdinal(SceneManager.GetActiveScene().name, this.loadingSceneName) == 0)
			{
				GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
				if (rootGameObjects.Length != 1)
				{
					throw new Exception(string.Concat(new string[]
					{
						"The loading scene (",
						this.loadingSceneName,
						") should only have one root object (",
						rootGameObjects.Length.ToString(),
						")"
					}));
				}
				this.loadingSceneRoot = rootGameObjects[0];
				Object.DontDestroyOnLoad(this.loadingSceneRoot);
				this.loadingSceneTransitioner = this.loadingSceneRoot.GetComponent<LoadingSceneTransitioner>();
				SceneTransitionManager.loadSceneTimer.Reset();
				SceneTransitionManager.loadSceneTimer.Start();
				this.transitionStage = new SceneTransitionManager.TransitionStage(this.Unloading_UnusedAssets);
			}
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0005CD1D File Offset: 0x0005AF1D
		private void Unloading_UnusedAssets()
		{
			this.EnableLoadingSpeedup(false);
			this.taskObject = SceneTransitionManager.UnloadUnusedAssets();
			this.transitionStage = new SceneTransitionManager.TransitionStage(this.Waiting_Unloading_UnusedAssets);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0005CD43 File Offset: 0x0005AF43
		private void Waiting_Unloading_UnusedAssets()
		{
			if (this.taskObject.IsCompleted)
			{
				this.taskObject = null;
				this.transitionStage = new SceneTransitionManager.TransitionStage(this.Waiting_Loading_Ink_Story);
			}
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0005CD6B File Offset: 0x0005AF6B
		private void Waiting_Loading_Ink_Story()
		{
			if (Singleton<InkStoryProvider>.Instance.IsStoryAvailable())
			{
				this.taskObject = null;
				this.transitionStage = new SceneTransitionManager.TransitionStage(this.Loading_DestinationScene);
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0005CD92 File Offset: 0x0005AF92
		private void Loading_DestinationScene()
		{
			if (!string.IsNullOrWhiteSpace(this.desinationSceneName))
			{
				this.EnableLoadingSpeedup(true);
				this.taskObject = SceneTransitionManager.LoadScene(this.desinationSceneName, LoadSceneMode.Single);
				this.transitionStage = new SceneTransitionManager.TransitionStage(this.Waiting_DestinationScene);
			}
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0005CDCC File Offset: 0x0005AFCC
		private void Waiting_DestinationScene()
		{
			if (this.taskObject.IsCompleted)
			{
				this.taskObject = null;
				this.transitionStage = new SceneTransitionManager.TransitionStage(this.CleanUpLoading);
				if (this.desinationSceneName == SceneConsts.kGameScene && this.loadingSceneTransitioner != null)
				{
					this.loadingSceneTransitioner.SetupPostLoadSceneState();
				}
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0005CE2C File Offset: 0x0005B02C
		private void CleanUpLoading()
		{
			this.EnableBoostMode(false);
			this.EnableLoadingSpeedup(false);
			this.transitionStage = null;
			if (SceneTransitionManager.finished != null)
			{
				SceneTransitionManager.finished(this.desinationSceneName);
			}
			if (this.loadingSceneRoot != null)
			{
				if (this.desinationSceneName == SceneConsts.kGameScene && this.loadingSceneTransitioner != null)
				{
					this.loadingSceneTransitioner.Transition();
					return;
				}
				Object.Destroy(this.loadingSceneRoot);
			}
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0005CEAA File Offset: 0x0005B0AA
		private void EnableLoadingSpeedup(bool enableSpeedup)
		{
			if (enableSpeedup)
			{
				Application.backgroundLoadingPriority = ThreadPriority.High;
				return;
			}
			Application.backgroundLoadingPriority = ThreadPriority.Normal;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0005CEBC File Offset: 0x0005B0BC
		private void EnableBoostMode(bool enableBoost)
		{
			Services.PlatformService.EnableBurstMode(enableBoost);
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0005CECC File Offset: 0x0005B0CC
		private static async Task UnloadUnusedAssets()
		{
			await Task.Yield();
			AsyncOperation asyncOp = Resources.UnloadUnusedAssets();
			while (!asyncOp.isDone)
			{
				await Task.Yield();
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0005CF08 File Offset: 0x0005B108
		private static async Task LoadScene(string sceneName, LoadSceneMode mode)
		{
			if (!false)
			{
				AsyncOperationHandle<SceneInstance> asyncLoad = Addressables.LoadSceneAsync(sceneName, mode, false, 100);
				while (asyncLoad.Status == AsyncOperationStatus.None)
				{
					await Task.Yield();
				}
				while (SceneTransitionManager.loadSceneTimer.ElapsedMilliseconds > 1000L && SceneTransitionManager.loadSceneTimer.ElapsedMilliseconds < 6000L)
				{
					await Task.Yield();
				}
				await Task.Yield();
				asyncLoad.Result.ActivateAsync();
				asyncLoad = default(AsyncOperationHandle<SceneInstance>);
			}
			else
			{
				AsyncOperationHandle<SceneInstance> asyncLoad = Addressables.LoadSceneAsync(sceneName, mode, true, 100);
				while (!asyncLoad.IsDone)
				{
					await Task.Yield();
				}
				asyncLoad = default(AsyncOperationHandle<SceneInstance>);
			}
			if (SceneTransitionManager.sceneActivated != null)
			{
				SceneTransitionManager.sceneActivated(sceneName);
			}
			SceneTransitionManager.loadSceneTimer.Stop();
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0005CF53 File Offset: 0x0005B153
		private void HideCursor()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x04000F1C RID: 3868
		[Tooltip("A list of loading screens that can be used. Scene will be randomly selected from list")]
		public string[] loadingScenes = new string[] { SceneConsts.kLoadingScene };

		// Token: 0x04000F1F RID: 3871
		private static SceneTransitionManager s_Instance = null;

		// Token: 0x04000F20 RID: 3872
		private string loadingSceneName = "";

		// Token: 0x04000F21 RID: 3873
		private string desinationSceneName = "";

		// Token: 0x04000F22 RID: 3874
		private GameObject loadingSceneRoot;

		// Token: 0x04000F23 RID: 3875
		private static Stopwatch loadSceneTimer = new Stopwatch();

		// Token: 0x04000F24 RID: 3876
		private SceneTransitionManager.TransitionStage transitionStage;

		// Token: 0x04000F25 RID: 3877
		private Task taskObject;

		// Token: 0x04000F26 RID: 3878
		private LoadingSceneTransitioner loadingSceneTransitioner;

		// Token: 0x04000F27 RID: 3879
		private const long minTimeWait = 1000L;

		// Token: 0x04000F28 RID: 3880
		private const long maxTimeWait = 6000L;

		// Token: 0x020003CC RID: 972
		// (Invoke) Token: 0x06001896 RID: 6294
		private delegate void TransitionStage();

		// Token: 0x020003CD RID: 973
		// (Invoke) Token: 0x0600189A RID: 6298
		private delegate void OnSceneActivated(string sceneName);
	}
}
