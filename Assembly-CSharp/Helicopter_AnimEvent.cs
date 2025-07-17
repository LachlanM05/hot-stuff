using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class Helicopter_AnimEvent : MonoBehaviour
{
	// Token: 0x06000376 RID: 886 RVA: 0x000163F1 File Offset: 0x000145F1
	public void PauseAnimator()
	{
		this.animator.speed = 0f;
	}

	// Token: 0x04000371 RID: 881
	[SerializeField]
	private Animator animator;
}
