using System;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class TimedSetActiveState : MonoBehaviour
{
	// Token: 0x06000D33 RID: 3379 RVA: 0x0004BBE4 File Offset: 0x00049DE4
	private void OnEnable()
	{
		if (this.Object == null || this.delay < 0f)
		{
			base.enabled = false;
			return;
		}
		this.timeElapsed = 0f;
		this.finished = false;
		this.Object.SetActive(!this.newEnabledState);
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x0004BC3C File Offset: 0x00049E3C
	private void Update()
	{
		if (!this.finished)
		{
			this.timeElapsed += Time.deltaTime;
			if (this.timeElapsed >= this.delay)
			{
				this.Object.SetActive(this.newEnabledState);
				this.finished = true;
			}
		}
	}

	// Token: 0x04000BEF RID: 3055
	[Tooltip("How many seconds before enabling GameObject")]
	[SerializeField]
	private float delay = 2f;

	// Token: 0x04000BF0 RID: 3056
	[SerializeField]
	private bool newEnabledState;

	// Token: 0x04000BF1 RID: 3057
	[SerializeField]
	private GameObject Object;

	// Token: 0x04000BF2 RID: 3058
	private float timeElapsed;

	// Token: 0x04000BF3 RID: 3059
	private bool finished;
}
