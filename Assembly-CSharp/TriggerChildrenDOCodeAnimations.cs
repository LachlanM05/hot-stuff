using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class TriggerChildrenDOCodeAnimations : MonoBehaviour
{
	// Token: 0x060000A7 RID: 167 RVA: 0x00004598 File Offset: 0x00002798
	private void Init()
	{
		if (base.transform.childCount == 0)
		{
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			this.animations.Add(base.transform.GetChild(i).GetComponent<DoCodeAnimation>());
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000045E5 File Offset: 0x000027E5
	private void Start()
	{
		this.Init();
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000045F0 File Offset: 0x000027F0
	private void OnEnable()
	{
		if (this.animations == null)
		{
			this.Init();
		}
		int num = 0;
		float num2 = 0f;
		foreach (DoCodeAnimation doCodeAnimation in this.animations)
		{
			base.StartCoroutine(this.AnimateChild(num2, doCodeAnimation));
			if (this.speedUpOnIteration)
			{
				if (num == 0)
				{
					num2 += 1f;
				}
				else
				{
					num2 += 1f - (float)num * this.growth;
				}
			}
			else
			{
				num2 += 1f;
			}
			num++;
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00004698 File Offset: 0x00002898
	private IEnumerator AnimateChild(float i, DoCodeAnimation animation)
	{
		yield return new WaitForSeconds(this.delayBetweenChildren * i + this.initialDelayBeforeAnimating);
		if (animation.gameObject.activeInHierarchy)
		{
			animation.Trigger();
		}
		yield break;
	}

	// Token: 0x040000A3 RID: 163
	[SerializeField]
	private float delayBetweenChildren;

	// Token: 0x040000A4 RID: 164
	[SerializeField]
	private float initialDelayBeforeAnimating;

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	private bool speedUpOnIteration;

	// Token: 0x040000A6 RID: 166
	[SerializeField]
	private float growth;

	// Token: 0x040000A7 RID: 167
	private List<DoCodeAnimation> animations = new List<DoCodeAnimation>();
}
