using System;
using System.Collections.Generic;
using Date_Everything.Animation.UI;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class InteractionBlockAnimatorBehaviour : StateMachineBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600001D RID: 29 RVA: 0x000028DF File Offset: 0x00000ADF
	public static bool AnyMenusAnimating
	{
		get
		{
			return InteractionBlockAnimatorBehaviour._animators.Count > 0;
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000028EE File Offset: 0x00000AEE
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		InteractionBlockAnimatorBehaviour.AddBlocker(animator);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000028F6 File Offset: 0x00000AF6
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (InteractionBlockAnimatorBehaviour.IsBlocking(animator) && InteractionBlockAnimatorBehaviour.HasAnimationFinishedButUnableToTransition(stateInfo))
		{
			InteractionBlockAnimatorBehaviour.QueueRemoveBlocker(animator, this._delayToUnblockInSeconds);
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002914 File Offset: 0x00000B14
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!InteractionBlockAnimatorBehaviour.IsBlocking(animator))
		{
			return;
		}
		InteractionBlockAnimatorBehaviour.QueueRemoveBlocker(animator, this._delayToUnblockInSeconds);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000292B File Offset: 0x00000B2B
	private static bool HasAnimationFinishedButUnableToTransition(AnimatorStateInfo stateInfo)
	{
		return stateInfo.normalizedTime >= 1f && !stateInfo.loop;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002948 File Offset: 0x00000B48
	public static void AddBlocker(Animator animator)
	{
		InteractionBlockAnimatorBehaviour._animators.Add(animator);
		UIAnimatorBlockHelper component = animator.gameObject.GetComponent<UIAnimatorBlockHelper>();
		if (component != null)
		{
			component.RemoveOngoingQueuedRequest();
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000297C File Offset: 0x00000B7C
	private static void QueueRemoveBlocker(Animator animator, float delayInSeconds)
	{
		UIAnimatorBlockHelper uianimatorBlockHelper = animator.gameObject.GetComponent<UIAnimatorBlockHelper>();
		if (uianimatorBlockHelper == null)
		{
			uianimatorBlockHelper = animator.gameObject.AddComponent<UIAnimatorBlockHelper>();
		}
		uianimatorBlockHelper.QueueRemoveBlocker(delayInSeconds);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000029B1 File Offset: 0x00000BB1
	private static bool IsBlocking(Animator animator)
	{
		return InteractionBlockAnimatorBehaviour._animators.Contains(animator);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000029BE File Offset: 0x00000BBE
	public static void RemoveBlocker(Animator animator)
	{
		InteractionBlockAnimatorBehaviour._animators.Remove(animator);
	}

	// Token: 0x0400001D RID: 29
	private static List<Animator> _animators = new List<Animator>();

	// Token: 0x0400001E RID: 30
	[SerializeField]
	private float _delayToUnblockInSeconds = 0.5f;
}
