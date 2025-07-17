using System;
using System.Collections;
using QRCoder;
using QRCoder.Unity;
using T17.Services;
using Team17.Common;
using Team17.Scripts.Services.Input;
using Team17.Scripts.UI_Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000112 RID: 274
public class Locket : MonoBehaviour
{
	// Token: 0x0600095B RID: 2395 RVA: 0x00036248 File Offset: 0x00034448
	private void Awake()
	{
		if (Locket.s_Instance == null)
		{
			Locket.s_Instance = this;
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0003625D File Offset: 0x0003445D
	private void OnDestroy()
	{
		if (this == Locket.s_Instance)
		{
			Locket.s_Instance = null;
		}
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00036270 File Offset: 0x00034470
	public void OnEnable()
	{
		Locket.locketEnabled = true;
		CursorLocker.Unlock();
		this._curScene = 0;
		Locket._currScene = this._curScene;
		this.nextButtonBinding = this.nextButton.GetComponent<ControllerButtonComponent>();
		this.SetActiveScene();
		this.backButton.SetActive(false);
		this.SetCharacterPortrait();
		this.SetEndStats();
		this.SetDatableStatus();
		this.popupShown = false;
		if (this.backLegend != null)
		{
			this.backLegend.OnISOkToDisplayGlyth += this.ShouldWeDisplayBackGlyph;
		}
		if (this.nextLegend != null)
		{
			this.nextLegend.OnISOkToDisplayGlyth += this.ShouldWeDisplayNextGlyph;
		}
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00036320 File Offset: 0x00034520
	private void OnDisable()
	{
		Locket.locketEnabled = false;
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = null;
		if (this.backLegend != null)
		{
			this.backLegend.OnISOkToDisplayGlyth -= this.ShouldWeDisplayBackGlyph;
		}
		if (this.nextLegend != null)
		{
			this.nextLegend.OnISOkToDisplayGlyth -= this.ShouldWeDisplayNextGlyph;
		}
		CursorLocker.Lock();
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0003639C File Offset: 0x0003459C
	private void SetActiveScene()
	{
		if (this._curScene < 0)
		{
			this._curScene = 0;
		}
		this.scenes[this._curScene].SetActive(true);
		if (this._curScene == 0)
		{
			MenuComponent component = this.scenes[this._curScene].transform.GetChild(0).GetComponent<MenuComponent>();
			if (component != null)
			{
				component.gameObject.SetActive(true);
			}
			CanvasGroup component2 = this.scenes[this._curScene].transform.GetChild(0).GetComponent<CanvasGroup>();
			if (component2 != null)
			{
				component2.alpha = 0f;
			}
		}
		if (this._curScene == 5)
		{
			MenuComponent menuComponent = null;
			if (this.scenes[this._curScene].transform.childCount > 0)
			{
				Transform transform = this.scenes[this._curScene].transform.GetChild(0).transform;
				if (transform.childCount > 0)
				{
					menuComponent = transform.GetChild(0).GetComponent<MenuComponent>();
				}
			}
			if (menuComponent != null)
			{
				menuComponent.gameObject.SetActive(true);
			}
		}
		Locket._currScene = this._curScene;
		GameObject gameObject;
		if (Locket._currScene == 2)
		{
			gameObject = this.scrollRect.verticalScrollbar.gameObject;
			this.scrollRect.verticalNormalizedPosition = 1f;
		}
		else
		{
			gameObject = this.nextButton;
		}
		ControllerMenuUI.SetCurrentlySelected(gameObject, ControllerMenuUI.Direction.Down, false, false);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x000364F0 File Offset: 0x000346F0
	public void Initialize(string characterInternalName)
	{
		if (Singleton<CharacterHelper>.Instance._characters.IsNameInSet(characterInternalName))
		{
			this.UpdateCommittedCharacterImage(characterInternalName, E_General_Poses.neutral, E_Facial_Expressions.neutral);
		}
		GameObject[] array = this.scenes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.SetActiveScene();
		try
		{
			Singleton<Save>.Instance.GetPlayerName();
			Singleton<Save>.Instance.GetPlayerGender();
			Singleton<Save>.Instance.GetPlayerTown();
			Singleton<Save>.Instance.GetPlayTimeInMillis();
			Singleton<Save>.Instance.GetAllDateStatus();
		}
		catch (Exception ex)
		{
			T17Debug.LogError("Error requesting QR code: " + ex.Message);
		}
		if (this.canvas)
		{
			this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
		}
		if (this.menuCamera)
		{
			this.menuCamera.gameObject.SetActive(true);
		}
		this.DisableAudioListener();
		if (!PlayerPauser.IsPaused())
		{
			PlayerPauser.Pause();
		}
		Singleton<TutorialController>.Instance.HideCars();
		ControllerMenuUI.SetCurrentlySelected(this.nextButton, ControllerMenuUI.Direction.Down, false, false);
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00036600 File Offset: 0x00034800
	private void CreateQrCode(string datesStatus, string playerName, int playerGender, string playerTown, long playTime)
	{
		Texture2D graphic = new UnityQRCode(new QRCodeGenerator().CreateQrCode(string.Concat(new string[]
		{
			"https://qr.dateeverything.com/?dates=",
			datesStatus,
			"&name=",
			playerName,
			"&gender=",
			playerGender.ToString(),
			"&town=",
			playerTown,
			"&time=",
			playTime.ToString()
		}), QRCodeGenerator.ECCLevel.Q, false, false, QRCodeGenerator.EciMode.Default, -1)).GetGraphic(20);
		Sprite sprite = Sprite.Create(graphic, new Rect(0f, 0f, (float)graphic.width, (float)graphic.height), new Vector2(0.5f, 0.5f), 100f);
		this.qrCodeImage.sprite = sprite;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x000366C3 File Offset: 0x000348C3
	public void Next()
	{
		if (Singleton<ScreenFader>.Instance.CurrentFadeType != ScreenFader.FadeType.none)
		{
			return;
		}
		if (Singleton<Popup>.Instance.IsPopupOpen())
		{
			return;
		}
		base.StartCoroutine(this.GotoNextScene(true));
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x000366F0 File Offset: 0x000348F0
	private void GoToMainMenu()
	{
		Singleton<Popup>.Instance.ClosePopup();
		BetterPlayerControl.Instance.isGameEndingOn = false;
		UIUtilities uiutilities = Object.FindObjectOfType<UIUtilities>();
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.SPECIAL, 4f);
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 5f);
		uiutilities.LoadSceneAsyncSingle(SceneConsts.kMainMenu, false);
		base.gameObject.SetActive(false);
		this.RestoreAudioListener();
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00036754 File Offset: 0x00034954
	private void InternalNext()
	{
		IMirandaInputService.EInputMode currentMode = Services.InputService.CurrentMode;
		if (this._curScene == this.scenes.Length - 1)
		{
			this.GoToMainMenu();
			return;
		}
		this._curScene++;
		Locket._currScene = this._curScene;
		GameObject gameObject = this.scenes[this._curScene - 1];
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		this.SetActiveScene();
		this.UpdateButtons();
		if (this._curScene == this.scenes.Length - 1)
		{
			Singleton<AudioManager>.Instance.StopTrack("locket", 3f);
			if (!this.popupShown)
			{
				this.popupShown = true;
				UnityEvent unityEvent = new UnityEvent();
				unityEvent.AddListener(new UnityAction(this.FocusOnSaveScreen));
				if (!Singleton<Save>.Instance.GetNewGamePlus())
				{
					Singleton<Popup>.Instance.CreatePopup("Thank you for playing Date Everything!", "You can consider this ending your canon story... or continue playing to see if you can find something better! The choice is yours!", unityEvent, false);
					return;
				}
				UnityEvent unityEvent2 = new UnityEvent();
				unityEvent2.AddListener(new UnityAction(this.GoToMainMenu));
				if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
				{
					Singleton<Popup>.Instance.CreatePopup("Thank you for playing Date Everything!", "You have unlocked New Game+! This allows you to start a new game with all progress reset - except for Collectables - giving you a chance to collect all 409! New Game+ is shown on the Save/Load screen by a locket icon. Would you like to start New Game+ now? Your progress will be saved.", unityEvent, unityEvent2, false);
					return;
				}
				Singleton<Popup>.Instance.CreatePopup("Thank you for playing Date Everything!", "You have unlocked New Game+! This allows you to start a new game with all progress reset - except for Collectables - giving you a chance to collect all 404! New Game+ is shown on the Save/Load screen by a locket icon. Would you like to start New Game+ now? Your progress will be saved.", unityEvent, unityEvent2, false);
				return;
			}
			else
			{
				this.FocusOnSaveScreen();
			}
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00036895 File Offset: 0x00034A95
	public void Back()
	{
		if (Singleton<ScreenFader>.Instance.CurrentFadeType == ScreenFader.FadeType.none)
		{
			base.StartCoroutine(this.GotoNextScene(false));
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x000368B1 File Offset: 0x00034AB1
	public void InternalBack()
	{
		this._curScene--;
		Locket._currScene = this._curScene;
		this.SetActiveScene();
		this.scenes[this._curScene + 1].SetActive(false);
		this.UpdateButtons();
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x000368ED File Offset: 0x00034AED
	private void FocusOnSaveScreen()
	{
		this.saveScreen.SelectBackButton();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x000368FA File Offset: 0x00034AFA
	private void UpdateButtons()
	{
		this.backButton.SetActive(this._curScene > 0);
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00036910 File Offset: 0x00034B10
	private void SetCharacterPortrait()
	{
		string text = Singleton<InkController>.Instance.story.variablesState["committed_name"].ToString();
		if (!string.IsNullOrEmpty(text))
		{
			this.UpdateCommittedCharacterImage(text, E_General_Poses.neutral, E_Facial_Expressions.neutral);
		}
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00036950 File Offset: 0x00034B50
	private void ClearCommittedCharacterImage()
	{
		Sprite sprite = this.committedCharImg.sprite;
		if (sprite == null)
		{
			return;
		}
		if (this._characterUtility != null)
		{
			CharacterUtility.ReturnCharacterSprite(sprite);
			this._characterUtility = null;
		}
		this.committedCharImg.sprite = null;
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0003699A File Offset: 0x00034B9A
	private void UpdateCommittedCharacterImage(string committedName, E_General_Poses pose, E_Facial_Expressions expression)
	{
		this.ClearCommittedCharacterImage();
		this._characterUtility = Singleton<CharacterHelper>.Instance._characters[committedName];
		this.committedCharImg.sprite = this._characterUtility.GetSpriteFromPoseExpression(pose, expression, false, false);
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x000369D4 File Offset: 0x00034BD4
	private void SetEndStats()
	{
		this.smartsText.text = Singleton<SpecStatMain>.Instance.GetStatAdjective("smarts");
		this.poiseText.text = Singleton<SpecStatMain>.Instance.GetStatAdjective("poise");
		this.empathyText.text = Singleton<SpecStatMain>.Instance.GetStatAdjective("empathy");
		this.charmText.text = Singleton<SpecStatMain>.Instance.GetStatAdjective("charm");
		this.sassText.text = Singleton<SpecStatMain>.Instance.GetStatAdjective("sass");
		int num = Singleton<Save>.Instance.AvailableTotalDatables();
		this.datablesText.text = string.Format("{0}/{1}", Singleton<Save>.Instance.AvailableTotalMetDatables(), num);
		this.friendsText.text = string.Format("{0}/{1}", Singleton<Save>.Instance.AvailableTotalFriendEndings(), num);
		this.loveText.text = string.Format("{0}/{1}", Singleton<Save>.Instance.AvailableTotalLoveEndings(), num);
		this.hateText.text = string.Format("{0}/{1}", Singleton<Save>.Instance.AvailableTotalHateEndings(), num);
		this.collectablesText.text = string.Format("{0}/{1}", Singleton<Save>.Instance.GetTotalUnlockedCollectables(true), Singleton<Save>.Instance.GetTotalCollectables(Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION));
		TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)Singleton<Save>.Instance.GetPlayTimeInMillis());
		string text = string.Format("{0:00}:{1:D2}:{2:D2}", timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
		this.totalPlaytimeText.text = text;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00036BA0 File Offset: 0x00034DA0
	private void SetDatableStatus()
	{
		foreach (object obj in this.scrollRect.content.transform)
		{
			Object.Destroy(((Transform)obj).gameObject);
		}
		for (int i = 0; i < Singleton<Save>.Instance.AvailableTotalDatables(); i++)
		{
			DateADexEntry entry = DateADex.Instance.GetEntry(i);
			if ((!(entry.internalName == DeluxeEditionController.DELUXE_CHARACTER_1) && !(entry.internalName == DeluxeEditionController.DELUXE_CHARACTER_2)) || Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
			{
				LocketListDisplay locketListDisplay = Object.Instantiate<LocketListDisplay>(this.listItemPrefab, this.scrollRect.content);
				string text = "N/A";
				switch (Singleton<Save>.Instance.GetDateStatus(i))
				{
				case RelationshipStatus.Hate:
					text = "Hate";
					break;
				case RelationshipStatus.Single:
					text = "";
					break;
				case RelationshipStatus.Love:
					text = "Love";
					break;
				case RelationshipStatus.Friend:
					text = "Friend";
					break;
				}
				locketListDisplay.statText.text = text;
				if (text == "N/A")
				{
					locketListDisplay.nameText.text = "???";
				}
				else
				{
					locketListDisplay.nameText.text = entry.CharName;
				}
			}
		}
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00036D10 File Offset: 0x00034F10
	private void DisableAudioListener()
	{
		if (this.menuCamera != null)
		{
			AudioListener component = this.menuCamera.gameObject.GetComponent<AudioListener>();
			if (component != null)
			{
				this._cachedAudioListenerEnabled = component.enabled;
				component.enabled = false;
			}
		}
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00036D58 File Offset: 0x00034F58
	private void RestoreAudioListener()
	{
		if (this.menuCamera != null)
		{
			AudioListener component = this.menuCamera.gameObject.GetComponent<AudioListener>();
			if (component != null)
			{
				component.enabled = this._cachedAudioListenerEnabled;
			}
		}
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00036D99 File Offset: 0x00034F99
	public static bool IsLocketEnabled()
	{
		return Locket.locketEnabled;
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x00036DA0 File Offset: 0x00034FA0
	public static int GetCurrScene()
	{
		return Locket._currScene;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00036DA7 File Offset: 0x00034FA7
	public IEnumerator GotoNextScene(bool forwards)
	{
		bool doFade = false;
		if (forwards && (Locket._currScene == 0 || this._curScene == this.scenes.Length - 1))
		{
			doFade = true;
		}
		if (!forwards && this._curScene == 1)
		{
			doFade = true;
		}
		if (doFade)
		{
			Singleton<ScreenFader>.Instance.FadeToWhite(this.SceneTransitionFadeLength);
			while (Singleton<ScreenFader>.Instance.CurrentFadeType != ScreenFader.FadeType.none)
			{
				yield return null;
			}
		}
		if (forwards)
		{
			this.InternalNext();
		}
		else
		{
			this.InternalBack();
		}
		if (doFade)
		{
			Singleton<ScreenFader>.Instance.FadeIn(this.SceneTransitionFadeLength);
			while (Singleton<ScreenFader>.Instance.CurrentFadeType != ScreenFader.FadeType.none)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00036DBD File Offset: 0x00034FBD
	private void ShouldWeDisplayBackGlyph(ControllerGlyphComponent.ResultEvent result)
	{
		if (Locket._currScene == 0)
		{
			result.result = false;
		}
		if (Locket._currScene == this.scenes.Length - 1)
		{
			result.result = false;
		}
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00036DE5 File Offset: 0x00034FE5
	private void ShouldWeDisplayNextGlyph(ControllerGlyphComponent.ResultEvent result)
	{
		if (Locket._currScene == this.scenes.Length - 1)
		{
			result.result = false;
		}
	}

	// Token: 0x04000896 RID: 2198
	[SerializeField]
	private GameObject backButton;

	// Token: 0x04000897 RID: 2199
	[SerializeField]
	private GameObject nextButton;

	// Token: 0x04000898 RID: 2200
	[SerializeField]
	private GameObject[] scenes;

	// Token: 0x04000899 RID: 2201
	[SerializeField]
	private LocketListDisplay listItemPrefab;

	// Token: 0x0400089A RID: 2202
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x0400089B RID: 2203
	[SerializeField]
	private Image committedCharImg;

	// Token: 0x0400089C RID: 2204
	[SerializeField]
	private Image qrCodeImage;

	// Token: 0x0400089D RID: 2205
	[SerializeField]
	private Camera menuCamera;

	// Token: 0x0400089E RID: 2206
	[SerializeField]
	private Canvas canvas;

	// Token: 0x0400089F RID: 2207
	[SerializeField]
	private ControllerGlyphComponent backLegend;

	// Token: 0x040008A0 RID: 2208
	[SerializeField]
	private ControllerGlyphComponent nextLegend;

	// Token: 0x040008A1 RID: 2209
	[SerializeField]
	private SaveScreenManager saveScreen;

	// Token: 0x040008A2 RID: 2210
	private ControllerButtonComponent nextButtonBinding;

	// Token: 0x040008A3 RID: 2211
	private int _curScene;

	// Token: 0x040008A4 RID: 2212
	[Header("Stats")]
	[SerializeField]
	private TextMeshProUGUI smartsText;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	private TextMeshProUGUI poiseText;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private TextMeshProUGUI empathyText;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	private TextMeshProUGUI charmText;

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private TextMeshProUGUI sassText;

	// Token: 0x040008A9 RID: 2217
	[SerializeField]
	private TextMeshProUGUI datablesText;

	// Token: 0x040008AA RID: 2218
	[SerializeField]
	private TextMeshProUGUI friendsText;

	// Token: 0x040008AB RID: 2219
	[SerializeField]
	private TextMeshProUGUI loveText;

	// Token: 0x040008AC RID: 2220
	[SerializeField]
	private TextMeshProUGUI hateText;

	// Token: 0x040008AD RID: 2221
	[SerializeField]
	private TextMeshProUGUI collectablesText;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private TextMeshProUGUI totalPlaytimeText;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private float SceneTransitionFadeLength = 0.3f;

	// Token: 0x040008B0 RID: 2224
	private CharacterUtility _characterUtility;

	// Token: 0x040008B1 RID: 2225
	private InputModeHandle _inputModeHandle;

	// Token: 0x040008B2 RID: 2226
	private bool _cachedAudioListenerEnabled;

	// Token: 0x040008B3 RID: 2227
	private bool popupShown;

	// Token: 0x040008B4 RID: 2228
	private static bool locketEnabled = false;

	// Token: 0x040008B5 RID: 2229
	public static int _currScene = -1;

	// Token: 0x040008B6 RID: 2230
	public static Locket s_Instance = null;
}
