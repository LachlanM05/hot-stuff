using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class AnimationFeeder : MonoBehaviour
{
	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0004E279 File Offset: 0x0004C479
	// (set) Token: 0x06000E0E RID: 3598 RVA: 0x0004E281 File Offset: 0x0004C481
	public float Alpha { get; set; }

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000E0F RID: 3599 RVA: 0x0004E28A File Offset: 0x0004C48A
	// (set) Token: 0x06000E10 RID: 3600 RVA: 0x0004E292 File Offset: 0x0004C492
	public float ScreenFadeAlpha { get; set; }

	// Token: 0x06000E11 RID: 3601 RVA: 0x0004E29B File Offset: 0x0004C49B
	private void Start()
	{
		this.addedCameraNumber = 0;
		this.currentAddedCameraNumber = 0;
		this.currentLightSetting = 0;
		this.addedLightSetting = 0;
		this.startedTutorial = false;
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0004E2C0 File Offset: 0x0004C4C0
	private void Update()
	{
		if (this.ScreenFadeAlpha != this.cachedAlpha)
		{
			this.cachedAlpha = this.ScreenFadeAlpha;
			Singleton<ScreenFader>.Instance.Alpha = this.ScreenFadeAlpha;
		}
		if (this.addedCameraNumber != this.currentAddedCameraNumber)
		{
			this.currentAddedCameraNumber = this.addedCameraNumber;
			if (Singleton<TutorialController>.Instance != null)
			{
				Singleton<TutorialController>.Instance.OnIntroCameraCut(this.currentAddedCameraNumber);
			}
		}
		if (this.addedLightSetting != this.currentLightSetting)
		{
			this.currentLightSetting = this.addedLightSetting;
			this.OnLightSetting();
		}
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x0004E34E File Offset: 0x0004C54E
	public void OnIntroChangeTimeOfDay(int timeOfDay)
	{
		Singleton<DayNightCycle>.Instance.ForceLightingScenario(timeOfDay);
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x0004E35C File Offset: 0x0004C55C
	public void OnIntroCameraCut(int cameraNumber)
	{
		if (Singleton<TutorialController>.Instance != null && !this.startedTutorial)
		{
			this.startedTutorial = true;
			Singleton<TutorialController>.Instance.inCinematic = true;
			Singleton<TutorialController>.Instance.OnIntroCameraCut(0);
			Singleton<AudioManager>.Instance.PlayTrack("opening_cinematic_audio", AUDIO_TYPE.SPECIAL, false, false, 0f, true, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		}
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x0004E3BF File Offset: 0x0004C5BF
	public void OnLightSetting()
	{
		if (Singleton<TutorialController>.Instance != null)
		{
			Singleton<TutorialController>.Instance.SetLightScenario(this.currentLightSetting);
		}
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x0004E3DE File Offset: 0x0004C5DE
	public void OnIntroAnimationFinished()
	{
		if (Singleton<TutorialController>.Instance != null)
		{
			Singleton<TutorialController>.Instance.EndInitialAnimation();
		}
	}

	// Token: 0x04000C83 RID: 3203
	private float cachedAlpha = float.MaxValue;

	// Token: 0x04000C84 RID: 3204
	[SerializeField]
	public int addedCameraNumber;

	// Token: 0x04000C85 RID: 3205
	private int currentAddedCameraNumber;

	// Token: 0x04000C86 RID: 3206
	[SerializeField]
	public int addedLightSetting;

	// Token: 0x04000C87 RID: 3207
	private int currentLightSetting;

	// Token: 0x04000C88 RID: 3208
	private bool startedTutorial;
}
