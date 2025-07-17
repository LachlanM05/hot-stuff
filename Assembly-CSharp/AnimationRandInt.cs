using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class AnimationRandInt : StateMachineBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x0000208E File Offset: 0x0000028E
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetInteger(this.m_intName, Random.Range(this.min_inclusive, this.max_exclusive));
	}

	// Token: 0x04000003 RID: 3
	[SerializeField]
	private string m_intName;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	private int min_inclusive;

	// Token: 0x04000005 RID: 5
	[SerializeField]
	private int max_exclusive;
}
