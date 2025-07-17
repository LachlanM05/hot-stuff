using System;
using System.Collections;
using T17.Flow;
using T17.Services;
using Team17.Common;
using Team17.Scripts.Services.Input;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200014A RID: 330
public class UIUtilities : MonoBehaviour
{
	// Token: 0x06000C17 RID: 3095 RVA: 0x00045EAB File Offset: 0x000440AB
	private static bool GetLoadingScreen()
	{
		if (UIUtilities._loadingScreen == null)
		{
			UIUtilities._loadingScreen = GameObject.FindWithTag(UIUtilities.LOADING_SCREEN_OBJ_NAME);
		}
		return UIUtilities._loadingScreen;
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00045ED4 File Offset: 0x000440D4
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		UIUtilities._loadingScreen = GameObject.FindWithTag(UIUtilities.LOADING_SCREEN_OBJ_NAME);
		if (!(UIUtilities._loadingScreen == null))
		{
			UIUtilities._loadingScreen.SetActive(false);
			this._slider = UIUtilities._loadingScreen.GetComponentInChildren<Slider>();
			this._progressText = UIUtilities._loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
		}
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00045F33 File Offset: 0x00044133
	public void LoadScene(int sceneIndex)
	{
		throw new Exception("Loading scenes by scene Index is not supported");
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00045F3F File Offset: 0x0004413F
	public void LoadSceneAdditive(int sceneIndex)
	{
		throw new Exception("Loading scenes by scene Index is not supported");
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00045F4B File Offset: 0x0004414B
	public void QuitGame()
	{
		Application.Quit();
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00045F52 File Offset: 0x00044152
	public void LoadSceneAsyncSingle(string sceneName, bool forceLoad = false)
	{
		base.StartCoroutine(this.LoadAsynchronously(sceneName, LoadSceneMode.Single, forceLoad));
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00045F64 File Offset: 0x00044164
	public void LoadSceneAsyncSingle(int sceneIndex)
	{
		throw new Exception("Loading scenes by scene Index is not supported");
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00045F70 File Offset: 0x00044170
	public void LoadSceneAsyncAdditive(string sceneName)
	{
		base.StartCoroutine(this.LoadAsynchronously(sceneName, LoadSceneMode.Additive, false));
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00045F82 File Offset: 0x00044182
	public void LoadSceneAsyncAdditive(int sceneIndex)
	{
		throw new Exception("Loading scenes by scene Index is not supported");
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00045F8E File Offset: 0x0004418E
	private IEnumerator LoadAsynchronously(string sceneName, LoadSceneMode loadSceneMode, bool forceLoad = false)
	{
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 1f);
		if (SceneTransitionManager.IsInitialised() && !forceLoad)
		{
			SceneTransitionManager.sceneActivated += this.OnSceneActviated;
			SceneTransitionManager.finished += this.OnSceneTransitionFinished;
			SceneTransitionManager.TransitionToScene(sceneName);
			while (!SceneTransitionManager.IsFinished())
			{
				yield return null;
			}
		}
		else
		{
			this.ShowLoadingScreen(true);
			yield return new WaitForSeconds(1.5f);
			SceneManager.sceneLoaded += UIUtilities.OnSceneLoaded;
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
			while (!operation.isDone)
			{
				this.LoadProgress(operation);
				yield return null;
			}
			yield return new WaitForSeconds(1.5f);
			Scene sceneByName = SceneManager.GetSceneByName(sceneName);
			Singleton<CanvasUIManager>.Instance.isInGame = string.CompareOrdinal(sceneName, SceneConsts.kGameScene) == 0;
			SceneManager.SetActiveScene(sceneByName);
			this.ShowLoadingScreen(false);
			operation = null;
		}
		yield break;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00045FB2 File Offset: 0x000441B2
	private IEnumerator LoadAsynchronously(int sceneIndex, LoadSceneMode loadSceneMode)
	{
		throw new Exception("Loading scenes by scene Index is not supported");
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00045FBE File Offset: 0x000441BE
	public void UnloadSceneAsync(string sceneName)
	{
		base.StartCoroutine(this.UnloadAsynchronously(sceneName));
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00045FCE File Offset: 0x000441CE
	public void UnloadSceneAsync(int sceneIndex)
	{
		throw new Exception("Unoading scenes by scene Index is not supported");
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00045FDA File Offset: 0x000441DA
	private IEnumerator UnloadAsynchronously(string sceneName)
	{
		this.ShowLoadingScreen(true);
		yield return new WaitForSeconds(0.01f);
		SceneManager.sceneLoaded += UIUtilities.OnSceneLoaded;
		SceneManager.UnloadSceneAsync(sceneName);
		this.ShowLoadingScreen(false);
		yield break;
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00045FF0 File Offset: 0x000441F0
	private IEnumerator UnloadAsynchronously(int sceneIndex)
	{
		throw new Exception("Unloading scenes by scene Index is not supported");
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00045FFC File Offset: 0x000441FC
	public void PlayMusic(string track)
	{
		base.StartCoroutine(this.PlayMusicNextFrame(track));
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0004600C File Offset: 0x0004420C
	private IEnumerator PlayMusicNextFrame(string track)
	{
		yield return null;
		try
		{
			Singleton<AudioManager>.Instance.PlayTrack(track, AUDIO_TYPE.MUSIC, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			yield break;
		}
		catch (Exception ex)
		{
			T17Debug.LogError("Error playing music." + ex.Message);
			yield break;
		}
		yield break;
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0004601B File Offset: 0x0004421B
	public void FadeOutTrack(string track)
	{
		Singleton<AudioManager>.Instance.FadeTrackOut(track, 1f);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0004602D File Offset: 0x0004422D
	public void FadeInTrack(string track)
	{
		Singleton<AudioManager>.Instance.FadeTrackIn(track, 1f);
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0004603F File Offset: 0x0004423F
	public void SetMusicVolume(float volume)
	{
		Singleton<AudioManager>.Instance.SetTrackGroupVolume(AUDIO_TYPE.MUSIC, volume);
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x00046050 File Offset: 0x00044250
	public void SetMusicFrequency(float frequency)
	{
		if (string.CompareOrdinal(SceneManager.GetActiveScene().name, SceneConsts.kMainMenu) != 0)
		{
			Singleton<AudioManager>.Instance.SetMusicFrequency(frequency, true, 1.5f);
			return;
		}
		Singleton<AudioManager>.Instance.SetMusicFrequency(1f, false, 0f);
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x0004609D File Offset: 0x0004429D
	public void ResetMusicFrequency(float frequency)
	{
		Singleton<AudioManager>.Instance.SetMusicFrequency(frequency, true, 0.25f);
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x000460B0 File Offset: 0x000442B0
	public void SetMusicResonance(float resonance)
	{
		Singleton<AudioManager>.Instance.SetMusicResonance(resonance);
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x000460BD File Offset: 0x000442BD
	private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (UIUtilities.GetLoadingScreen())
		{
			UIUtilities._loadingScreen.SetActive(false);
		}
		SceneManager.sceneLoaded -= UIUtilities.OnSceneLoaded;
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x000460E2 File Offset: 0x000442E2
	private void OnSceneActviated(string sceneName)
	{
		Singleton<CanvasUIManager>.Instance.isInGame = string.CompareOrdinal(sceneName, SceneConsts.kGameScene) == 0;
		UIUtilities.HandleInputModeForNewScene();
		SceneTransitionManager.sceneActivated -= this.OnSceneActviated;
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00046112 File Offset: 0x00044312
	private static void HandleInputModeForNewScene()
	{
		InputModeHandle inputModeHandleForLastScene = UIUtilities._inputModeHandleForLastScene;
		if (inputModeHandleForLastScene != null)
		{
			inputModeHandleForLastScene.Dispose();
		}
		UIUtilities._inputModeHandleForLastScene = null;
		if (Singleton<CanvasUIManager>.Instance.isInGame)
		{
			UIUtilities._inputModeHandleForLastScene = Services.InputService.PushMode(IMirandaInputService.EInputMode.Gameplay, typeof(UIUtilities));
		}
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00046150 File Offset: 0x00044350
	private void OnSceneTransitionFinished(string sceneName)
	{
		SceneTransitionManager.finished -= this.OnSceneTransitionFinished;
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00046163 File Offset: 0x00044363
	public void LoadProgress(AsyncOperation operation)
	{
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x00046165 File Offset: 0x00044365
	public void ShowLoadingScreen(bool show)
	{
		UIUtilities.GetLoadingScreen();
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0004616D File Offset: 0x0004436D
	public MainMenuSubScreen GetMainMenuSubscreen()
	{
		if (UIUtilities._saveScreen == null)
		{
			UIUtilities._saveScreen = GameObject.FindGameObjectWithTag(UIUtilities.SAVE_SCREEN_OBJ_NAME);
		}
		return UIUtilities._saveScreen.GetComponent<MainMenuSubScreen>();
	}

	// Token: 0x04000AE8 RID: 2792
	private static string LOADING_SCREEN_OBJ_NAME = "LoadingScreen";

	// Token: 0x04000AE9 RID: 2793
	private static string SAVE_SCREEN_OBJ_NAME = "SaveScreen";

	// Token: 0x04000AEA RID: 2794
	private static string MAIN_SCREEN_OBJ_NAME = "ThirdPersonGreybox";

	// Token: 0x04000AEB RID: 2795
	private static GameObject _loadingScreen;

	// Token: 0x04000AEC RID: 2796
	private static GameObject _saveScreen;

	// Token: 0x04000AED RID: 2797
	private Slider _slider;

	// Token: 0x04000AEE RID: 2798
	private TextMeshProUGUI _progressText;

	// Token: 0x04000AEF RID: 2799
	private static InputModeHandle _inputModeHandleForLastScene = null;
}
