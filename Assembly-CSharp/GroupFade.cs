using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class GroupFade : MonoBehaviour
{
	// Token: 0x06000E55 RID: 3669 RVA: 0x0004F1D8 File Offset: 0x0004D3D8
	private void Awake()
	{
		if (GroupFade.s_Times.Count == 0)
		{
			for (int i = 0; i < 100; i++)
			{
				GroupFade.s_Times.Add(0f);
			}
		}
		if (this.group == null && (this.group = base.GetComponent<CanvasGroup>()) == null)
		{
			base.enabled = false;
		}
		else if (this.staticIndex >= 100 || this.staticIndex < -1)
		{
			base.enabled = false;
		}
		if (this.resetTimer && this.staticIndex != -1)
		{
			GroupFade.s_Times[this.staticIndex] = 0f;
		}
		this.ProcessFade();
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0004F284 File Offset: 0x0004D484
	private void Update()
	{
		if (this.staticIndex != -1)
		{
			List<float> list = GroupFade.s_Times;
			int num = this.staticIndex;
			list[num] += Time.deltaTime;
		}
		else
		{
			this.localTimer += Time.deltaTime;
		}
		this.ProcessFade();
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0004F2D8 File Offset: 0x0004D4D8
	private void ProcessFade()
	{
		float num;
		if (this.staticIndex != -1)
		{
			num = GroupFade.s_Times[this.staticIndex];
		}
		else
		{
			num = this.localTimer;
		}
		if (num >= this.delayBeforeStart + this.timeToFade)
		{
			this.group.alpha = this.TranslateAlpha(1f);
			base.enabled = false;
			if (this.deactivateOnComplete)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else if (num < this.delayBeforeStart)
		{
			float num2 = this.TranslateAlpha(0f);
			if (this.group.alpha != num2)
			{
				this.group.alpha = num2;
				return;
			}
		}
		else
		{
			float num3 = this.TranslateAlpha((num - this.delayBeforeStart) / this.timeToFade);
			if (this.group.alpha != num3)
			{
				this.group.alpha = num3;
			}
		}
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x0004F3AA File Offset: 0x0004D5AA
	private float TranslateAlpha(float alpha)
	{
		if (this.typeOfFade == GroupFade.FadeType.ToZero)
		{
			return 1f - alpha;
		}
		return alpha;
	}

	// Token: 0x04000CAE RID: 3246
	private const int c_MaxTimes = 100;

	// Token: 0x04000CAF RID: 3247
	public CanvasGroup group;

	// Token: 0x04000CB0 RID: 3248
	[Tooltip("Do you want the timer reset each time")]
	public bool resetTimer;

	// Token: 0x04000CB1 RID: 3249
	[Tooltip("Do you want the gameObject reset on completion")]
	public bool deactivateOnComplete;

	// Token: 0x04000CB2 RID: 3250
	public GroupFade.FadeType typeOfFade;

	// Token: 0x04000CB3 RID: 3251
	public float delayBeforeStart;

	// Token: 0x04000CB4 RID: 3252
	public float timeToFade = 0.5f;

	// Token: 0x04000CB5 RID: 3253
	[Tooltip("-1 for non static or between 0 - 100 for a static index to use for the timer. Useful if you want something to start fading in one scene and continue in another")]
	public int staticIndex = -1;

	// Token: 0x04000CB6 RID: 3254
	private static List<float> s_Times = new List<float>(100);

	// Token: 0x04000CB7 RID: 3255
	private float localTimer;

	// Token: 0x02000384 RID: 900
	public enum FadeType
	{
		// Token: 0x040013DB RID: 5083
		ToZero,
		// Token: 0x040013DC RID: 5084
		ToOne
	}
}
