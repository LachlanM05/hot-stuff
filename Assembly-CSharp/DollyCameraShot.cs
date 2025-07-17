using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class DollyCameraShot : MonoBehaviour
{
	// Token: 0x06000427 RID: 1063 RVA: 0x000198AC File Offset: 0x00017AAC
	public void Interact()
	{
		if (!this.isPlaying)
		{
			this._Animator = base.GetComponent<Animator>();
			if (!(this._Animator == null))
			{
				this._Animator.ResetTrigger("loopEnd");
				this.camLogic(100);
				base.StartCoroutine(this.<Interact>g__waitPlaying|5_0());
				this._Animator.speed = 1f;
				this._Animator.SetTrigger("animStart");
				this._Animator.CrossFadeInFixedTime(this._Animator.GetNextAnimatorStateInfo(0).fullPathHash, 0f, 0);
				if (!this.interruptable)
				{
					base.StartCoroutine(this.<Interact>g__playAnim|5_1());
				}
			}
		}
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00019960 File Offset: 0x00017B60
	private void Update()
	{
		if (this.interruptable && this.isPlaying && Input.GetMouseButtonDown(0))
		{
			this._Animator.ResetTrigger("animStart");
			this._Animator.SetTrigger("loopEnd");
			base.StartCoroutine(this.<Update>g__waitStopping|6_0());
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000199B4 File Offset: 0x00017BB4
	public void camLogic(int priority)
	{
		if (this.VirtualCam != null)
		{
			CinemachineBlendDefinition cinemachineBlendDefinition = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f);
			CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this.VirtualCam);
			if (cinemachineBrain != null)
			{
				cinemachineBrain.m_DefaultBlend = cinemachineBlendDefinition;
				this.VirtualCam.Priority = priority;
			}
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00019A1C File Offset: 0x00017C1C
	[CompilerGenerated]
	private IEnumerator <Interact>g__waitPlaying|5_0()
	{
		yield return new WaitForSeconds(0.1f);
		this.isPlaying = true;
		yield break;
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00019A2B File Offset: 0x00017C2B
	[CompilerGenerated]
	private IEnumerator <Interact>g__playAnim|5_1()
	{
		yield return new WaitForSeconds(this.animLength);
		this.camLogic(10);
		this._Animator.ResetTrigger("animStart");
		this.isPlaying = false;
		yield break;
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00019A3A File Offset: 0x00017C3A
	[CompilerGenerated]
	private IEnumerator <Update>g__waitStopping|6_0()
	{
		yield return new WaitForSeconds(this.animLength);
		this.isPlaying = false;
		this.camLogic(10);
		yield break;
	}

	// Token: 0x04000420 RID: 1056
	public CinemachineVirtualCamera VirtualCam;

	// Token: 0x04000421 RID: 1057
	public float animLength = 1f;

	// Token: 0x04000422 RID: 1058
	public bool interruptable;

	// Token: 0x04000423 RID: 1059
	private bool isPlaying;

	// Token: 0x04000424 RID: 1060
	private Animator _Animator;
}
