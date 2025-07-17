using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200000B RID: 11
public class AnimationSetVariable : StateMachineBehaviour
{
	// Token: 0x0600001B RID: 27 RVA: 0x00002858 File Offset: 0x00000A58
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		int num;
		if (int.TryParse(Singleton<InkController>.Instance.GetVariable(this.m_inkVar), out num))
		{
			animator.SetInteger(this.m_varName, num);
		}
		bool flag;
		if (bool.TryParse(Singleton<InkController>.Instance.GetVariable(this.m_inkVar), out flag))
		{
			animator.SetBool(this.m_varName, flag);
		}
		float num2;
		if (float.TryParse(Singleton<InkController>.Instance.GetVariable(this.m_inkVar), out num2))
		{
			animator.SetFloat(this.m_varName, num2);
		}
	}

	// Token: 0x0400001A RID: 26
	[SerializeField]
	private string m_inkVar;

	// Token: 0x0400001B RID: 27
	[FormerlySerializedAs("m_intName")]
	[SerializeField]
	private string m_varName;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	private AnimationSetVariable.VAR_TYPE m_animVarType;

	// Token: 0x02000272 RID: 626
	private enum VAR_TYPE
	{
		// Token: 0x04000F8A RID: 3978
		INT,
		// Token: 0x04000F8B RID: 3979
		FLOAT,
		// Token: 0x04000F8C RID: 3980
		BOOL
	}
}
