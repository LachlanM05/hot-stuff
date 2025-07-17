using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B0 RID: 432
public class ScreenFader : Singleton<ScreenFader>
{
	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000E99 RID: 3737 RVA: 0x0005012B File Offset: 0x0004E32B
	// (set) Token: 0x06000E9A RID: 3738 RVA: 0x00050133 File Offset: 0x0004E333
	public float Alpha { get; set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000E9C RID: 3740 RVA: 0x00050145 File Offset: 0x0004E345
	// (set) Token: 0x06000E9B RID: 3739 RVA: 0x0005013C File Offset: 0x0004E33C
	public ScreenFader.FadeType CurrentFadeType { get; private set; }

	// Token: 0x06000E9D RID: 3741 RVA: 0x0005014D File Offset: 0x0004E34D
	public void FadeToWhite(float time = 1f)
	{
		this.FadeToColor(Color.white, time);
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x0005015B File Offset: 0x0004E35B
	public void FadeToBlack(float time = 1f)
	{
		this.FadeToColor(Color.black, time);
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x0005016C File Offset: 0x0004E36C
	public void FadeToColor(Color c, float time)
	{
		base.gameObject.SetActive(true);
		this.Alpha = 0f;
		c.a = 0f;
		this._fadeDuration = time;
		this.FullScreenImage.color = c;
		this._startTime = Time.realtimeSinceStartup;
		this.CurrentFadeType = ScreenFader.FadeType.FadeToColor;
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x000501C1 File Offset: 0x0004E3C1
	public void FadeIn(float time)
	{
		base.gameObject.SetActive(true);
		this.Alpha = 1f;
		this._fadeDuration = time;
		this._startTime = Time.realtimeSinceStartup;
		this.CurrentFadeType = ScreenFader.FadeType.FadeToScreen;
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x000501F3 File Offset: 0x0004E3F3
	public void InstantBlack()
	{
		base.gameObject.SetActive(true);
		this.Alpha = 1f;
		this.FullScreenImage.color = Color.black;
		this.SetFullscreenAlphaValue();
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x00050222 File Offset: 0x0004E422
	public void InstantFadeIn()
	{
		this.Alpha = 0f;
		this.SetFullscreenAlphaValue();
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x00050235 File Offset: 0x0004E435
	public void OnEnable()
	{
		if (this.FullScreenImage)
		{
			this.FullScreenImage.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x00050255 File Offset: 0x0004E455
	public void OnDisable()
	{
		this.Alpha = 0f;
		this.SetFullscreenAlphaValue();
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x00050268 File Offset: 0x0004E468
	public void Update()
	{
		if (this.CurrentFadeType == ScreenFader.FadeType.FadeToColor)
		{
			this.Alpha = (Time.realtimeSinceStartup - this._startTime) / this._fadeDuration;
			if (this.Alpha >= 1f)
			{
				this.Alpha = 1f;
				this.CurrentFadeType = ScreenFader.FadeType.none;
			}
		}
		else if (this.CurrentFadeType == ScreenFader.FadeType.FadeToScreen)
		{
			this.Alpha = 1f - (Time.realtimeSinceStartup - this._startTime) / this._fadeDuration;
			if (this.Alpha <= 0f)
			{
				this.Reset();
			}
		}
		if (this.Alpha != this.FullScreenImage.color.a)
		{
			this.SetFullscreenAlphaValue();
		}
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x00050314 File Offset: 0x0004E514
	private void SetFullscreenAlphaValue()
	{
		Color color = this.FullScreenImage.color;
		color.a = this.Alpha;
		this.FullScreenImage.color = color;
		if (this.Alpha == 0f)
		{
			this.Reset();
			return;
		}
		if (!this.FullScreenImage.gameObject.activeSelf)
		{
			this.FullScreenImage.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x00050380 File Offset: 0x0004E580
	private void Reset()
	{
		Color black = Color.black;
		black.a = 0f;
		this.FullScreenImage.color = black;
		this.Alpha = 0f;
		this.FullScreenImage.gameObject.SetActive(false);
		this.CurrentFadeType = ScreenFader.FadeType.none;
	}

	// Token: 0x04000CF5 RID: 3317
	[SerializeField]
	private Image FullScreenImage;

	// Token: 0x04000CF7 RID: 3319
	private float _startTime = -1f;

	// Token: 0x04000CF8 RID: 3320
	private float _fadeDuration = 1f;

	// Token: 0x02000389 RID: 905
	public enum FadeType
	{
		// Token: 0x040013EE RID: 5102
		none,
		// Token: 0x040013EF RID: 5103
		FadeToColor,
		// Token: 0x040013F0 RID: 5104
		FadeToScreen
	}
}
