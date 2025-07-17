using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class AnimateUIScale : MonoBehaviour
{
	// Token: 0x06000E04 RID: 3588 RVA: 0x0004E093 File Offset: 0x0004C293
	private void Awake()
	{
		if (this.uIObject == null)
		{
			base.enabled = false;
			return;
		}
		this.originalScale = this.uIObject.localScale;
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0004E0BC File Offset: 0x0004C2BC
	private void OnEnable()
	{
		this.Reset();
		if (this.animatedFromStart)
		{
			this.StartAnimating();
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x0004E0D2 File Offset: 0x0004C2D2
	private void OnDisable()
	{
		this.Reset();
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x0004E0DA File Offset: 0x0004C2DA
	private void Reset()
	{
		this.startedRefCount = 0;
		this.animTime = 0f;
		if (this.uIObject != null)
		{
			this.uIObject.localScale = this.originalScale;
		}
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x0004E110 File Offset: 0x0004C310
	private void Update()
	{
		if (this.startedRefCount > 0)
		{
			float num = Time.deltaTime * this.timeScale;
			if (num > 0f)
			{
				this.animTime = (this.animTime + num) % 1f;
				this.Scale(this.animTime);
			}
		}
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x0004E15C File Offset: 0x0004C35C
	private void Scale(float time)
	{
		if (this.uIObject != null)
		{
			float num = this.curve.Evaluate(time);
			Vector3 vector = this.originalScale;
			vector.x += num * this.xMultiplier;
			vector.y += num * this.yMultiplier;
			this.uIObject.localScale = vector;
		}
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0004E1C0 File Offset: 0x0004C3C0
	public void StartAnimating()
	{
		int num = this.startedRefCount;
		this.startedRefCount = num + 1;
		if (num == 0)
		{
			this.animTime = 0f;
			this.Scale(0f);
		}
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0004E1F8 File Offset: 0x0004C3F8
	public void StopAnimating()
	{
		if (this.startedRefCount > 0)
		{
			int num = this.startedRefCount - 1;
			this.startedRefCount = num;
			if (num == 0)
			{
				this.Reset();
			}
		}
	}

	// Token: 0x04000C78 RID: 3192
	public AnimationCurve curve = new AnimationCurve();

	// Token: 0x04000C79 RID: 3193
	public RectTransform uIObject;

	// Token: 0x04000C7A RID: 3194
	public float xMultiplier = 1f;

	// Token: 0x04000C7B RID: 3195
	public float yMultiplier = 1f;

	// Token: 0x04000C7C RID: 3196
	public float timeScale = 1f;

	// Token: 0x04000C7D RID: 3197
	public bool animatedFromStart = true;

	// Token: 0x04000C7E RID: 3198
	private Vector3 originalScale = Vector3.one;

	// Token: 0x04000C7F RID: 3199
	private float animTime;

	// Token: 0x04000C80 RID: 3200
	private int startedRefCount;
}
