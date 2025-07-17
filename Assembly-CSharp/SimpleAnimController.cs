using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class SimpleAnimController : MonoBehaviour
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000EC6 RID: 3782 RVA: 0x00050B50 File Offset: 0x0004ED50
	// (remove) Token: 0x06000EC7 RID: 3783 RVA: 0x00050B88 File Offset: 0x0004ED88
	private event SimpleAnimController.AnimFinishedEvent animFinishedEvent;

	// Token: 0x06000EC8 RID: 3784 RVA: 0x00050BC0 File Offset: 0x0004EDC0
	protected void Awake()
	{
		if (this.animator != null || (this.animator = base.GetComponent<Animator>()) != null)
		{
			this.valid = true;
		}
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x00050BF9 File Offset: 0x0004EDF9
	private void OnDisable()
	{
		this.animFinishedEvent = null;
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x00050C02 File Offset: 0x0004EE02
	public void RegisterForAnimFinished(SimpleAnimController.AnimFinishedEvent finishedEvent)
	{
		this.animFinishedEvent += finishedEvent;
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00050C0B File Offset: 0x0004EE0B
	public void UnregisterForAnimFinished(SimpleAnimController.AnimFinishedEvent finishedEvent)
	{
		if (this.animFinishedEvent != null)
		{
			this.animFinishedEvent -= finishedEvent;
		}
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x00050C1C File Offset: 0x0004EE1C
	private void LateUpdate()
	{
		this.isAnimating = this.CheckIfAnimating();
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x00050C2A File Offset: 0x0004EE2A
	public bool IsAnimating()
	{
		return this.isAnimating;
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x00050C32 File Offset: 0x0004EE32
	public void SetAnimTrigger(string _trigger)
	{
		bool flag = this.isAnimating;
		if (this.valid)
		{
			this.currentTrigger = _trigger;
			this.animator.SetTrigger(this.currentTrigger);
			this.isAnimating = true;
		}
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x00050C62 File Offset: 0x0004EE62
	public void ResetAnimTrigger(string _trigger)
	{
		if (this.valid)
		{
			this.animator.ResetTrigger(_trigger);
			if (string.CompareOrdinal(this.currentTrigger, _trigger) == 0)
			{
				this.currentTrigger = "";
				this.isAnimating = this.CheckIfAnimating();
			}
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00050CA0 File Offset: 0x0004EEA0
	private bool CheckIfAnimating()
	{
		bool flag = false;
		if (this.valid)
		{
			if (!string.IsNullOrEmpty(this.currentTrigger))
			{
				if (this.animator.GetBool(this.currentTrigger))
				{
					flag = true;
				}
				else
				{
					this.currentTrigger = "";
				}
			}
			if (!flag)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
				float num = Mathf.Min(currentAnimatorStateInfo.normalizedTime, 1f);
				if (!this.notifyJustBeforeEnd)
				{
					flag = num < 1f;
				}
				else
				{
					flag = currentAnimatorStateInfo.length * (1f - num) > 0.033333335f;
				}
			}
		}
		if (this.isAnimating && !flag)
		{
			SimpleAnimController.AnimFinishedEvent animFinishedEvent = this.animFinishedEvent;
			if (animFinishedEvent != null)
			{
				animFinishedEvent();
			}
			this.animFinishedEvent = null;
		}
		return flag;
	}

	// Token: 0x04000D0C RID: 3340
	[Tooltip("If no animator is specified it will use the one attached to the GameObject")]
	public Animator animator;

	// Token: 0x04000D0D RID: 3341
	[Tooltip("If you require to be notified of the anim ending just before it does then set this flag")]
	public bool notifyJustBeforeEnd;

	// Token: 0x04000D0F RID: 3343
	private bool valid;

	// Token: 0x04000D10 RID: 3344
	private bool isAnimating;

	// Token: 0x04000D11 RID: 3345
	private string currentTrigger = "";

	// Token: 0x04000D12 RID: 3346
	private const float c_Frame = 0.033333335f;

	// Token: 0x0200038D RID: 909
	// (Invoke) Token: 0x06001820 RID: 6176
	public delegate void AnimFinishedEvent();
}
