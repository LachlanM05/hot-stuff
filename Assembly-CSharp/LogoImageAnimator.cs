using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A8 RID: 424
public class LogoImageAnimator : MonoBehaviour
{
	// Token: 0x06000E78 RID: 3704 RVA: 0x0004FBA6 File Offset: 0x0004DDA6
	private void Awake()
	{
		this.SetCurrentSprite(LogoImageAnimator.currentIndex);
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0004FBB4 File Offset: 0x0004DDB4
	private void Update()
	{
		if (this.loopingType != LogoImageAnimator.LoopingType.Stop)
		{
			LogoImageAnimator.timeToChange -= Mathf.Min(Time.deltaTime, 0.033333335f);
			if (LogoImageAnimator.timeToChange < 0f)
			{
				LogoImageAnimator.timeToChange += this.timeBetweenFrames;
				this.SetCurrentSprite(LogoImageAnimator.currentIndex + 1);
				if (this.loopingType == LogoImageAnimator.LoopingType.PlayOnce && LogoImageAnimator.currentIndex == 0)
				{
					this.loopingType = LogoImageAnimator.LoopingType.Stop;
				}
			}
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0004FC25 File Offset: 0x0004DE25
	private void SetCurrentSprite(int index)
	{
		if (this.Sprites.Length == 0)
		{
			base.enabled = false;
			return;
		}
		if (index >= this.Sprites.Length)
		{
			index = 0;
		}
		this.image.sprite = this.Sprites[index];
		LogoImageAnimator.currentIndex = index;
	}

	// Token: 0x04000CDC RID: 3292
	public Image image;

	// Token: 0x04000CDD RID: 3293
	public LogoImageAnimator.LoopingType loopingType = LogoImageAnimator.LoopingType.PlayOnce;

	// Token: 0x04000CDE RID: 3294
	public float timeBetweenFrames = 0.25f;

	// Token: 0x04000CDF RID: 3295
	public Sprite[] Sprites = new Sprite[0];

	// Token: 0x04000CE0 RID: 3296
	private static int currentIndex;

	// Token: 0x04000CE1 RID: 3297
	private static float timeToChange;

	// Token: 0x04000CE2 RID: 3298
	private const float c_Frame = 0.033333335f;

	// Token: 0x02000386 RID: 902
	public enum LoopingType
	{
		// Token: 0x040013E3 RID: 5091
		Continuous,
		// Token: 0x040013E4 RID: 5092
		PlayOnce,
		// Token: 0x040013E5 RID: 5093
		Stop
	}
}
