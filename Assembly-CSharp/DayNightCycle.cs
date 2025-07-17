using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000044 RID: 68
public class DayNightCycle : Singleton<DayNightCycle>, IReloadHandler
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000189 RID: 393 RVA: 0x0000A341 File Offset: 0x00008541
	// (set) Token: 0x0600018A RID: 394 RVA: 0x0000A34C File Offset: 0x0000854C
	public DateTime PresentDayPresentTime
	{
		get
		{
			return this._presentDayPresentTime;
		}
		set
		{
			this._presentDayPresentTime = value;
			Singleton<InkController>.Instance.story.variablesState["dayOfWeek"] = this.GetDayOfWeek();
			Singleton<InkController>.Instance.story.variablesState["dayOfMonth"] = this.GetDayOfMonth();
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600018B RID: 395 RVA: 0x0000A39E File Offset: 0x0000859E
	// (set) Token: 0x0600018C RID: 396 RVA: 0x0000A3A6 File Offset: 0x000085A6
	public int DaysSinceStart
	{
		get
		{
			return this._daysSinceStart;
		}
		set
		{
			this._daysSinceStart = value;
		}
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600018D RID: 397 RVA: 0x0000A3B0 File Offset: 0x000085B0
	// (remove) Token: 0x0600018E RID: 398 RVA: 0x0000A3E4 File Offset: 0x000085E4
	public static event DayNightCycle._NightTimeEvents _nightevents;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600018F RID: 399 RVA: 0x0000A418 File Offset: 0x00008618
	// (remove) Token: 0x06000190 RID: 400 RVA: 0x0000A44C File Offset: 0x0000864C
	public static event DayNightCycle._DayTimeEvents _dayevents;

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000191 RID: 401 RVA: 0x0000A47F File Offset: 0x0000867F
	// (set) Token: 0x06000192 RID: 402 RVA: 0x0000A487 File Offset: 0x00008687
	public DayPhase CurrentDayPhase
	{
		get
		{
			return this._currentDayPhase;
		}
		set
		{
			this._currentDayPhase = value;
			Singleton<InkController>.Instance.story.variablesState["dayPhase"] = (int)value;
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000A4AF File Offset: 0x000086AF
	public void InitializeDateTime()
	{
		this.PresentDayPresentTime = new DateTime(2024, 10, 1);
		this.CurrentDayPhase = DayPhase.MORNING;
		UnityEvent dayPhaseUpdated = this.DayPhaseUpdated;
		if (dayPhaseUpdated == null)
		{
			return;
		}
		dayPhaseUpdated.Invoke();
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000A4DB File Offset: 0x000086DB
	public override void AwakeSingleton()
	{
		this._lightingScenarios = base.gameObject.GetComponent<LightingScenarios>();
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000A4EE File Offset: 0x000086EE
	private void Start()
	{
		if (this.TimeUpdated == null)
		{
			this.TimeUpdated = new UnityEvent();
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000A504 File Offset: 0x00008704
	public string GetDayOfMonth()
	{
		return this.PresentDayPresentTime.Day.ToString();
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000A527 File Offset: 0x00008727
	public int GetDaysSinceStart()
	{
		return this._daysSinceStart;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000A530 File Offset: 0x00008730
	public string GetDayOfWeek()
	{
		return this.PresentDayPresentTime.DayOfWeek.ToString();
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000A55C File Offset: 0x0000875C
	public string GetShortDayOfWeek()
	{
		switch (this.PresentDayPresentTime.DayOfWeek)
		{
		case DayOfWeek.Monday:
			return "MONDAY";
		case DayOfWeek.Tuesday:
			return "TUESDAY";
		case DayOfWeek.Wednesday:
			return "WEDNESDAY";
		case DayOfWeek.Thursday:
			return "THURSDAY";
		case DayOfWeek.Friday:
			return "FRIDAY";
		case DayOfWeek.Saturday:
			return "SATURDAY";
		default:
			return "SUNDAY";
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000A5C3 File Offset: 0x000087C3
	public DayPhase GetTime()
	{
		return this.CurrentDayPhase;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000A5CC File Offset: 0x000087CC
	public void SetDateTime(DateTime currentTime, DayPhase currentPhase)
	{
		this.isLoadingGame = true;
		this.PresentDayPresentTime = currentTime;
		this.CurrentDayPhase = currentPhase;
		UnityEvent dayPhaseUpdated = this.DayPhaseUpdated;
		if (dayPhaseUpdated != null)
		{
			dayPhaseUpdated.Invoke();
		}
		UnityEvent timeUpdated = this.TimeUpdated;
		if (timeUpdated != null)
		{
			timeUpdated.Invoke();
		}
		this.ForceSetIncrementTime(currentPhase);
		base.Invoke("FinishLoadingGame", 5f);
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000A627 File Offset: 0x00008827
	private void FinishLoadingGame()
	{
		this.isLoadingGame = false;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000A630 File Offset: 0x00008830
	public void ForceLightingScenario(int timeOfDay)
	{
		this._lightingScenarios.UpdateLighting(timeOfDay, (int)this.CurrentDayPhase);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000A644 File Offset: 0x00008844
	public void IncrementTime()
	{
		int currentDayPhase = (int)this.CurrentDayPhase;
		int num = currentDayPhase + 1;
		if (num > 5)
		{
			num = 5;
			this.UpdateWind();
		}
		this._lightingScenarios.UpdateLighting(num, currentDayPhase);
		this.CurrentDayPhase = (DayPhase)num;
		Singleton<TutorialController>.Instance.CarsTimeCheck();
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.Invoke();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
		Singleton<InkController>.Instance.story.variablesState["dayPhase"] = num;
		this.TimeUpdated.Invoke();
		UnityEvent dayPhaseUpdated = this.DayPhaseUpdated;
		if (dayPhaseUpdated == null)
		{
			return;
		}
		dayPhaseUpdated.Invoke();
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000A6E0 File Offset: 0x000088E0
	public void ReloadLightingUI()
	{
		int currentDayPhase = (int)this.CurrentDayPhase;
		this._lightingScenarios.UpdateLighting(currentDayPhase, currentDayPhase);
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000A710 File Offset: 0x00008910
	public void ForceIncrementTime()
	{
		int currentDayPhase = (int)this.CurrentDayPhase;
		int num = currentDayPhase + 1;
		if (num > 5)
		{
			num = 0;
			this.UpdateWind();
		}
		this._lightingScenarios.UpdateLighting(num, currentDayPhase);
		this.CurrentDayPhase = (DayPhase)num;
		Singleton<TutorialController>.Instance.CarsTimeCheck();
		Singleton<InkController>.Instance.story.variablesState["dayPhase"] = num;
		DayNightCycle.PlayDayMusic(0f);
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.Invoke();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
		this.TimeUpdated.Invoke();
		UnityEvent dayPhaseUpdated = this.DayPhaseUpdated;
		if (dayPhaseUpdated == null)
		{
			return;
		}
		dayPhaseUpdated.Invoke();
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000A7B4 File Offset: 0x000089B4
	public void ForceDecrementTime()
	{
		int currentDayPhase = (int)this.CurrentDayPhase;
		int num = currentDayPhase - 1;
		if (num < 0)
		{
			num = 5;
			this.UpdateWind();
		}
		this._lightingScenarios.UpdateLighting(num, currentDayPhase);
		this.CurrentDayPhase = (DayPhase)num;
		Singleton<TutorialController>.Instance.CarsTimeCheck();
		Singleton<InkController>.Instance.story.variablesState["dayPhase"] = num;
		DayNightCycle.PlayDayMusic(0f);
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.Invoke();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
		this.TimeUpdated.Invoke();
		UnityEvent dayPhaseUpdated = this.DayPhaseUpdated;
		if (dayPhaseUpdated == null)
		{
			return;
		}
		dayPhaseUpdated.Invoke();
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000A858 File Offset: 0x00008A58
	public void ForceSetIncrementTime(DayPhase timeOfDay)
	{
		int currentDayPhase = (int)this.CurrentDayPhase;
		this.CurrentDayPhase = timeOfDay;
		this._lightingScenarios.UpdateLighting((int)timeOfDay, currentDayPhase);
		Singleton<TutorialController>.Instance.CarsTimeCheck();
		Singleton<InkController>.Instance.story.variablesState["dayPhase"] = (int)this.CurrentDayPhase;
		DayNightCycle.PlayDayMusic(0f);
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.Invoke();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
		this.TimeUpdated.Invoke();
		UnityEvent dayPhaseUpdated = this.DayPhaseUpdated;
		if (dayPhaseUpdated == null)
		{
			return;
		}
		dayPhaseUpdated.Invoke();
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000A8F4 File Offset: 0x00008AF4
	public void ForceIncrementDay()
	{
		this.PresentDayPresentTime = this.PresentDayPresentTime.AddDays(1.0);
		this._daysSinceStart++;
		if (VFXEvents.Instance)
		{
			int num = Random.Range(0, 2);
			int num2 = Random.Range(0, 20);
			float num3 = Random.Range(0f, 1f);
			if (num2 > 18)
			{
				num3 = Random.Range(0.5f, 1f);
			}
			else if (num > 0)
			{
				num3 = Random.Range(0.1f, 0.4f);
			}
			VFXEvents.Instance.SetWind(num3);
		}
		Singleton<InkController>.Instance.TrySetVariable("dayOfWeek", this.GetDayOfWeek());
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.Invoke();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000A9C0 File Offset: 0x00008BC0
	public void IncrementDay()
	{
		this.CurrentDayPhase = DayPhase.MORNING;
		this._lightingScenarios.UpdateLighting(0, 0);
		this.UpdateWind();
		DayNightCycle._NightTimeEvents nightevents = DayNightCycle._nightevents;
		if (nightevents != null)
		{
			nightevents(this._daysSinceStart);
		}
		this.PresentDayPresentTime = this.PresentDayPresentTime.AddDays(1.0);
		this._daysSinceStart++;
		DayNightCycle._DayTimeEvents dayevents = DayNightCycle._dayevents;
		if (dayevents != null)
		{
			dayevents(this._daysSinceStart);
		}
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.Invoke();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
		base.StartCoroutine(this.PlayDayMusicDelayed());
		UnityEvent dayPhaseUpdated = this.DayPhaseUpdated;
		if (dayPhaseUpdated != null)
		{
			dayPhaseUpdated.Invoke();
		}
		Singleton<InkController>.Instance.TrySetVariable("dayOfWeek", this.GetDayOfWeek());
		if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && Singleton<Save>.Instance.GetDateStatus("mikey") != RelationshipStatus.Unmet)
		{
			int num = (int)Singleton<InkController>.Instance.story.variablesState["money"];
			num += 5;
			Singleton<InkController>.Instance.story.variablesState["money"] = num;
		}
		Singleton<TutorialController>.Instance.CarsTimeCheck();
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000AAF8 File Offset: 0x00008CF8
	private void UpdateWind()
	{
		Object @object = GameObject.FindWithTag("ExteriorTree");
		float num = Random.Range(0f, 1f);
		if (@object != null)
		{
			foreach (Renderer renderer in base.gameObject.GetComponents<Renderer>())
			{
				if (renderer.sharedMaterial != null && renderer.sharedMaterial.HasProperty("_Wind_Strength"))
				{
					renderer.sharedMaterial.SetFloat("_Wind_Strength", num);
					return;
				}
			}
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000AB78 File Offset: 0x00008D78
	public void finishedDateAsk()
	{
		this.PresentDayPresentTime = this.PresentDayPresentTime.AddDays(1.0);
		this._daysSinceStart++;
		DayNightCycle._DayTimeEvents dayevents = DayNightCycle._dayevents;
		if (dayevents != null)
		{
			dayevents(this._daysSinceStart);
		}
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.Invoke();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000ABE4 File Offset: 0x00008DE4
	public Sprite GetDayPhaseSprite(Sprite currentSprite = null)
	{
		if (this.CurrentDayPhase < DayPhase.MORNING || this.CurrentDayPhase > DayPhase.MIDNIGHT)
		{
			this.CurrentDayPhase = DayPhase.MORNING;
		}
		Sprite sprite;
		switch (this.CurrentDayPhase)
		{
		case DayPhase.MORNING:
			sprite = this._morning;
			break;
		case DayPhase.NOON:
			sprite = this._noon;
			break;
		case DayPhase.AFTERNOON:
			sprite = this._afternoon;
			break;
		case DayPhase.EVENING:
			sprite = this._evening;
			break;
		case DayPhase.NIGHT:
			sprite = this._night;
			break;
		case DayPhase.MIDNIGHT:
			sprite = this._midnight;
			break;
		default:
			sprite = this._morning;
			break;
		}
		return sprite;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000AC70 File Offset: 0x00008E70
	public Sprite GetPreviousDayPhaseSprite()
	{
		DayPhase dayPhase = this.CurrentDayPhase - 1;
		if (dayPhase < DayPhase.MORNING)
		{
			dayPhase = DayPhase.MIDNIGHT;
		}
		Sprite sprite;
		switch (dayPhase)
		{
		case DayPhase.MORNING:
			sprite = this._noon;
			break;
		case DayPhase.NOON:
			sprite = this._afternoon;
			break;
		case DayPhase.AFTERNOON:
			sprite = this._evening;
			break;
		case DayPhase.EVENING:
			sprite = this._night;
			break;
		case DayPhase.NIGHT:
			sprite = this._midnight;
			break;
		case DayPhase.MIDNIGHT:
			sprite = this._morning;
			break;
		default:
			sprite = this._morning;
			break;
		}
		return sprite;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000ACEC File Offset: 0x00008EEC
	public Sprite GetNextDayPhaseSprite()
	{
		Sprite sprite;
		switch (this.CurrentDayPhase)
		{
		case DayPhase.MORNING:
			sprite = this._noon;
			break;
		case DayPhase.NOON:
			sprite = this._afternoon;
			break;
		case DayPhase.AFTERNOON:
			sprite = this._evening;
			break;
		case DayPhase.EVENING:
			sprite = this._night;
			break;
		case DayPhase.NIGHT:
			sprite = this._midnight;
			break;
		case DayPhase.MIDNIGHT:
			sprite = this._morning;
			break;
		default:
			sprite = this._morning;
			break;
		}
		return sprite;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000AD60 File Offset: 0x00008F60
	public void ClearOldDelegates()
	{
		DayNightCycle._dayevents = delegate
		{
		};
		DayNightCycle._nightevents = delegate
		{
		};
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000ADB5 File Offset: 0x00008FB5
	private IEnumerator PlayDayMusicDelayed()
	{
		yield return new WaitForSeconds(3f);
		DayNightCycle.PlayDayMusic(0f);
		yield break;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000ADC0 File Offset: 0x00008FC0
	public static void PlayDayMusic(float fadeTime = 0f)
	{
		bool flag = false;
		Teleporter teleporter = Object.FindFirstObjectByType<Teleporter>();
		if (teleporter != null && teleporter.teleportedIn)
		{
			flag = true;
		}
		if (flag)
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		}
		else if (Singleton<DayNightCycle>.Instance.GetTime() == DayPhase.MORNING)
		{
			Singleton<GameController>.Instance.MorningMusic();
		}
		else if (Singleton<DayNightCycle>.Instance.GetTime() == DayPhase.NOON || Singleton<DayNightCycle>.Instance.GetTime() == DayPhase.EVENING || Singleton<DayNightCycle>.Instance.GetTime() == DayPhase.NIGHT || Singleton<DayNightCycle>.Instance.GetTime() == DayPhase.AFTERNOON)
		{
			Singleton<AudioManager>.Instance.PlayTrack("Overworld_" + Singleton<DayNightCycle>.Instance.GetTime().ToString(), AUDIO_TYPE.MUSIC, true, false, fadeTime, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		}
		else
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		}
		if (AwakenSplashScreen.Instance && AwakenSplashScreen.Instance.isOpen)
		{
			Singleton<AudioManager>.Instance.PauseTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		}
		if (ResultSplashScreen.Instance && ResultSplashScreen.Instance.isOpen)
		{
			Singleton<AudioManager>.Instance.PauseTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000AEEE File Offset: 0x000090EE
	public void ResumeMusic()
	{
		Singleton<AudioManager>.Instance.ResumeTrackGroup(AUDIO_TYPE.MUSIC, false, 9f);
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000AF01 File Offset: 0x00009101
	public void StopMusic()
	{
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 9f);
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000AF13 File Offset: 0x00009113
	public void StopMusicInstant()
	{
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0f);
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x0000AF28 File Offset: 0x00009128
	public void FadeToWhite()
	{
		Singleton<InkController>.Instance.HideBackgroundFrontgroundImages();
		CursorLocker.Unlock();
		PlayerPauser.Pause();
		this.whiteScreenTransition.SetActive(true);
		this.whiteImage = this.whiteScreenTransition.GetComponent<Image>();
		this.transparentColor = new Color(this.whiteImage.color.r, this.whiteImage.color.b, this.whiteImage.color.b, 0f);
		this.solidColor = new Color(this.whiteImage.color.r, this.whiteImage.color.b, this.whiteImage.color.b, 1f);
		this.whiteImage.color = this.transparentColor;
		DOTween.Sequence().Append(this.whiteImage.DOBlendableColor(this.solidColor, 1.5f)).Append(this.whiteImage.DOBlendableColor(this.solidColor, 3f))
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				this.FadeToWhite_Step2();
			});
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000B04C File Offset: 0x0000924C
	private void FadeToWhite_Step2()
	{
		this.whiteImage.color = this.solidColor;
		if (!Locket.IsLocketEnabled())
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_transition_character, AUDIO_TYPE.SFX, false, false, 0f, false, 0f, null, false, SFX_SUBGROUP.UI, false);
		}
		this.FadeToWhite_Step3();
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000B0A0 File Offset: 0x000092A0
	private void FadeToWhite_Step3()
	{
		this.whiteImage.color = this.solidColor;
		DOTween.Sequence().Append(this.whiteImage.DOBlendableColor(this.transparentColor, 1.5f)).OnComplete(delegate
		{
			this.whiteScreenTransition.SetActive(false);
		});
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000B0F0 File Offset: 0x000092F0
	public void GameOver()
	{
		CursorLocker.Unlock();
		PlayerPauser.Pause();
		this.nightBlackScreenTransition.SetActive(true);
		this.blackNightImage = this.nightBlackScreenTransition.GetComponent<Image>();
		this.transparentColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 0f);
		this.solidColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 1f);
		this.blackNightImage.color = this.transparentColor;
		DOTween.Sequence().Append(this.blackNightImage.DOBlendableColor(this.solidColor, 1.5f)).Append(this.blackNightImage.DOBlendableColor(this.solidColor, 3f))
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				this.GameOver2();
			});
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000B208 File Offset: 0x00009408
	public void GameOver2()
	{
		this.nightBlackScreenGameOver.SetActive(true);
		this.ConfirmGoToMainMenu();
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000B21C File Offset: 0x0000941C
	public void ConfirmGoToMainMenu()
	{
		Singleton<AudioManager>.Instance.StopAll(0.5f);
		Object.FindObjectOfType<UIUtilities>().LoadSceneAsyncSingle(SceneConsts.kMainMenu, false);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000B23D File Offset: 0x0000943D
	public void ClearFadeToBlack()
	{
		this.nightBlackScreenTransition.SetActive(false);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000B24B File Offset: 0x0000944B
	public void FadeToBlack()
	{
		this.FadeToBlack(null, 1.5f, 2f, 1f, false);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000B264 File Offset: 0x00009464
	public void FadeToBlack(string newLocation, float waitTime1 = 1.5f, float waitTime2 = 2f, float waitTime3 = 1f, bool exitTalkingUi = false)
	{
		this.isExitingUIOnFadeToBlack = exitTalkingUi;
		PlayerPauser.Pause();
		Singleton<InkController>.Instance.HideBackgroundFrontgroundImages();
		this.nightBlackScreenTransition.SetActive(true);
		this.blackNightImage = this.nightBlackScreenTransition.GetComponent<Image>();
		this.transparentColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 0f);
		this.solidColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 1f);
		this.blackNightImage.color = this.transparentColor;
		DOTween.Sequence().Append(this.blackNightImage.DOBlendableColor(this.solidColor, waitTime1)).Append(this.blackNightImage.DOBlendableColor(this.solidColor, waitTime2))
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				this.FadeToBlack_Step2(newLocation, waitTime3);
			});
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000B3A0 File Offset: 0x000095A0
	private void FadeToBlack_Step2(string newLocation, float waitTime3 = 1f)
	{
		this.blackNightImage.color = this.solidColor;
		if (newLocation != null)
		{
			if (newLocation.Contains(" "))
			{
				List<string> list = newLocation.Split(' ', StringSplitOptions.None).ToList<string>();
				Singleton<GameController>.Instance.talkingUI.ChangeCameraByListedNearest(list);
			}
			else
			{
				Singleton<GameController>.Instance.talkingUI.ChangeCameraByRoom(newLocation);
			}
		}
		base.Invoke("FadeToBlack_Step3", waitTime3);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000B40C File Offset: 0x0000960C
	private void FadeToBlack_Step3()
	{
		this.blackNightImage.color = this.solidColor;
		DOTween.Sequence().Append(this.blackNightImage.DOBlendableColor(this.transparentColor, 1.5f)).OnComplete(delegate
		{
			this.nightBlackScreenTransition.SetActive(false);
			PlayerPauser.Unpause();
			if (this.isExitingUIOnFadeToBlack)
			{
				TalkingUI.Instance.ExitTalkingUI();
			}
		});
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000B45C File Offset: 0x0000965C
	public int Priority()
	{
		return -100;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000B460 File Offset: 0x00009660
	public void LoadState()
	{
		this.ClearOldDelegates();
		Save.SaveData saveData = Save.GetSaveData(false);
		DateTime dayNightCycle_presentDayPresentTime = saveData.dayNightCycle_presentDayPresentTime;
		if (saveData.dayNightCycle_ticks != 0L && saveData.dayNightCycle_ticks != 9223372036854775807L && saveData.dayNightCycle_ticks != 0L)
		{
			dayNightCycle_presentDayPresentTime = new DateTime(saveData.dayNightCycle_ticks);
		}
		this.SetDateTime(dayNightCycle_presentDayPresentTime, saveData.dayNightCycle_currentDayPhase);
		this._daysSinceStart = saveData.dayNightCycle_daysSinceStart;
		if (this._daysSinceStart < 0 || this._daysSinceStart == 0 || this._daysSinceStart == 2147483647)
		{
			this._daysSinceStart = 0;
		}
		this.ReloadLightingUI();
		if (Singleton<Save>.Instance.GetFullTutorialFinished() && !Object.FindObjectOfType<Teleporter>().teleportedIn)
		{
			DayNightCycle.PlayDayMusic(0f);
			return;
		}
		if (Singleton<Save>.Instance.GetTutorialFinished())
		{
			Singleton<AudioManager>.Instance.PlayTrack("mysteriousbox_music", AUDIO_TYPE.MUSIC, true, false, 5f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000B546 File Offset: 0x00009746
	public void SaveState()
	{
		Save.SaveData saveData = Save.GetSaveData(false);
		saveData.dayNightCycle_currentDayPhase = this._currentDayPhase;
		saveData.dayNightCycle_presentDayPresentTime = this._presentDayPresentTime;
		saveData.dayNightCycle_ticks = this._presentDayPresentTime.Ticks;
		saveData.dayNightCycle_daysSinceStart = this._daysSinceStart;
	}

	// Token: 0x04000297 RID: 663
	private int _daysSinceStart;

	// Token: 0x04000298 RID: 664
	private DateTime _presentDayPresentTime;

	// Token: 0x04000299 RID: 665
	public bool isLoadingGame;

	// Token: 0x0400029A RID: 666
	public UnityEvent DayPhaseUpdated;

	// Token: 0x0400029B RID: 667
	public UnityEvent TimeUpdated;

	// Token: 0x0400029E RID: 670
	[SerializeField]
	private DayPhase _currentDayPhase;

	// Token: 0x0400029F RID: 671
	[SerializeField]
	private Sprite _morning;

	// Token: 0x040002A0 RID: 672
	[SerializeField]
	private Sprite _noon;

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	private Sprite _afternoon;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private Sprite _evening;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private Sprite _night;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private Sprite _midnight;

	// Token: 0x040002A5 RID: 677
	private LightingScenarios _lightingScenarios;

	// Token: 0x040002A6 RID: 678
	public GameObject nightBlackScreenTransition;

	// Token: 0x040002A7 RID: 679
	public GameObject whiteScreenTransition;

	// Token: 0x040002A8 RID: 680
	public GameObject nightBlackScreenGameOver;

	// Token: 0x040002A9 RID: 681
	private Image blackNightImage;

	// Token: 0x040002AA RID: 682
	private Image whiteImage;

	// Token: 0x040002AB RID: 683
	private Color transparentColor;

	// Token: 0x040002AC RID: 684
	private Color solidColor;

	// Token: 0x040002AD RID: 685
	private bool isExitingUIOnFadeToBlack;

	// Token: 0x020002A7 RID: 679
	// (Invoke) Token: 0x06001510 RID: 5392
	public delegate void _NightTimeEvents(int i);

	// Token: 0x020002A8 RID: 680
	// (Invoke) Token: 0x06001514 RID: 5396
	public delegate void _DayTimeEvents(int i);
}
