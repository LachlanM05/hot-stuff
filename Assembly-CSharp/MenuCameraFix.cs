using System;
using Rewired;
using Rewired.Integration.UnityUI;
using Team17.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Token: 0x020000B3 RID: 179
public class MenuCameraFix : MonoBehaviour
{
	// Token: 0x06000594 RID: 1428 RVA: 0x00020194 File Offset: 0x0001E394
	private void Start()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		Transform transform = base.transform.Find("MainMenu");
		if (transform != null)
		{
			this.gameMainMenu = transform.gameObject;
			return;
		}
		T17Debug.LogError("Count not find MainMenu, has it been renamed");
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x000201E3 File Offset: 0x0001E3E3
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.EnsureMenuCameraFunctionality();
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x000201EB File Offset: 0x0001E3EB
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x00020200 File Offset: 0x0001E400
	public void EnsureMenuCameraFunctionality()
	{
		bool flag = string.CompareOrdinal(SceneManager.GetActiveScene().name, SceneConsts.kMainMenu) == 0;
		this.Canvas.renderMode = (flag ? RenderMode.ScreenSpaceCamera : RenderMode.ScreenSpaceOverlay);
		this.MenuCamera.gameObject.SetActive(flag);
		if (flag)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("EventSystem");
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("RewiredInputManager");
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("NewGameButton");
			gameObject.GetComponent<EventSystem>().firstSelectedGameObject = gameObject3;
			gameObject.GetComponent<RewiredStandaloneInputModule>().RewiredInputManager = gameObject2.GetComponent<InputManager>();
			Singleton<PhoneManager>.Instance.ClosePhoneAsync(null, false);
			PlayerPauser.Unpause();
			CursorLocker.Unlock();
		}
		if (this.gameMainMenu != null)
		{
			this.gameMainMenu.SetActive(flag);
		}
	}

	// Token: 0x04000567 RID: 1383
	[Header("Menu Checks")]
	public Camera MenuCamera;

	// Token: 0x04000568 RID: 1384
	public Canvas Canvas;

	// Token: 0x04000569 RID: 1385
	private GameObject gameMainMenu;
}
