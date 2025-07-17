using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000120 RID: 288
public class PhoneBackgroundSwitcher : MonoBehaviour
{
	// Token: 0x060009B4 RID: 2484 RVA: 0x00037BB0 File Offset: 0x00035DB0
	public void NextBackground()
	{
		this.currentBackground++;
		if (this.currentBackground == this.backgrounds.Length)
		{
			this.currentBackground = 0;
		}
		this.SetBackgroundImages();
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00037BDD File Offset: 0x00035DDD
	public void PreviousBackground()
	{
		this.currentBackground--;
		if (this.currentBackground < 0)
		{
			this.currentBackground = this.backgrounds.Length - 1;
		}
		this.SetBackgroundImages();
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00037C0C File Offset: 0x00035E0C
	public void SetBackgroundImages()
	{
		this.image.sprite = this.backgrounds[this.currentBackground];
		this.sidewaysImage.sprite = this.backgrounds[this.currentBackground];
		this.MaskImage.sprite = this.backgrounds[this.currentBackground];
		Singleton<Save>.Instance.SetPhoneBackgroundIndex(this.currentBackground);
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00037C71 File Offset: 0x00035E71
	public void LoadBackgroundOnPhoneOpen()
	{
		if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			this.currentBackground = Singleton<Save>.Instance.GetPhoneBackgroundIndex();
		}
		else
		{
			this.currentBackground = 0;
		}
		this.SetBackgroundImages();
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00037C9E File Offset: 0x00035E9E
	public void SetBackgroundImageColor(Color endValue, float duration, TweenCallback onComplete)
	{
		this.sidewaysImage.color = endValue;
		this.image.DOColor(endValue, duration).OnComplete(onComplete);
	}

	// Token: 0x040008F2 RID: 2290
	[Header("Settings")]
	public bool isAvailable = true;

	// Token: 0x040008F3 RID: 2291
	public int currentBackground;

	// Token: 0x040008F4 RID: 2292
	[Header("Backgrounds")]
	[SerializeField]
	private Sprite[] backgrounds;

	// Token: 0x040008F5 RID: 2293
	[Header("References")]
	[SerializeField]
	private Image image;

	// Token: 0x040008F6 RID: 2294
	public Image sidewaysImage;

	// Token: 0x040008F7 RID: 2295
	[SerializeField]
	private Image MaskImage;
}
