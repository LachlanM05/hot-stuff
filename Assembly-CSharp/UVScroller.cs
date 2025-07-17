using System;
using T17.Flow;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B9 RID: 441
[ExecuteInEditMode]
public class UVScroller : MonoBehaviour
{
	// Token: 0x06000EEE RID: 3822 RVA: 0x00051494 File Offset: 0x0004F694
	private void Awake()
	{
		this.ourTransform = base.GetComponent<RectTransform>();
		this.ourRect = this.ourTransform.rect;
		if (!this.ValidateImage())
		{
			base.enabled = false;
			return;
		}
		this.CalculateUVWidth();
		this.CalculateUVX();
		this.UpdateTextureUV();
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x000514E4 File Offset: 0x0004F6E4
	private void Update()
	{
		bool flag = false;
		Rect rect = this.ourTransform.rect;
		if (!rect.Equals(this.ourRect))
		{
			this.ourRect = rect;
			this.CalculateUVWidth();
			flag = true;
		}
		if (this.firstUpdate)
		{
			this.firstUpdate = false;
		}
		else if (SceneTransitionManager.IsFinished())
		{
			UVScroller.s_lastTime += Time.deltaTime;
		}
		flag |= this.CalculateUVX();
		if (flag)
		{
			this.UpdateTextureUV();
		}
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x00051558 File Offset: 0x0004F758
	private bool CalculateUVX()
	{
		if (Application.isPlaying && this.secondsToLoop != 0f)
		{
			while (UVScroller.s_lastTime > this.secondsToLoop)
			{
				UVScroller.s_lastTime -= this.secondsToLoop;
			}
			float num = UVScroller.s_lastTime / this.secondsToLoop;
			if (num >= 1f)
			{
				num -= (float)((int)num);
			}
			if (UVScroller.s_uVRectX != num)
			{
				UVScroller.s_uVRectX = num;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x000515C8 File Offset: 0x0004F7C8
	private bool ValidateImage()
	{
		if ((this.rawImage != null || (this.rawImage = base.GetComponent<RawImage>()) != null) && (this.imageSprite = this.rawImage.texture) != null)
		{
			this.imageApectRatio = (float)this.imageSprite.height / (float)this.imageSprite.width;
			return true;
		}
		return false;
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x00051638 File Offset: 0x0004F838
	private void CalculateUVWidth()
	{
		this.uVRectwidth = this.imageApectRatio / (this.ourRect.height / this.ourRect.width);
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x00051660 File Offset: 0x0004F860
	private void UpdateTextureUV()
	{
		Rect uvRect = this.rawImage.uvRect;
		uvRect.width = this.uVRectwidth;
		uvRect.x = UVScroller.s_uVRectX;
		this.rawImage.uvRect = uvRect;
	}

	// Token: 0x04000D2D RID: 3373
	public RawImage rawImage;

	// Token: 0x04000D2E RID: 3374
	public float secondsToLoop = 5f;

	// Token: 0x04000D2F RID: 3375
	private Texture imageSprite;

	// Token: 0x04000D30 RID: 3376
	private RectTransform ourTransform;

	// Token: 0x04000D31 RID: 3377
	private bool firstUpdate = true;

	// Token: 0x04000D32 RID: 3378
	private Rect ourRect = Rect.zero;

	// Token: 0x04000D33 RID: 3379
	private float imageApectRatio = 1f;

	// Token: 0x04000D34 RID: 3380
	private float uVRectwidth = 1f;

	// Token: 0x04000D35 RID: 3381
	private static float s_uVRectX = -1f;

	// Token: 0x04000D36 RID: 3382
	private static float s_lastTime = 0f;
}
