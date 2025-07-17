using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000052 RID: 82
public class FirstPersonAnimInteraction : Interactable
{
	// Token: 0x06000215 RID: 533 RVA: 0x0000C4B1 File Offset: 0x0000A6B1
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
	public override void Interact()
	{
		this.HeardInteract.Invoke();
		if (!this.isPlaying)
		{
			if (this.tempObject != null)
			{
				this.tempObject.enabled = true;
			}
			this._Animator = base.GetComponent<Animator>();
			if (!(this._Animator == null))
			{
				this._Animator.ResetTrigger("loopEnd");
				this.camLogic(100);
				base.StartCoroutine(this.<Interact>g__waitPlaying|10_0());
				if (this.loadingIn)
				{
					this._Animator.speed = 1000f;
				}
				else
				{
					this._Animator.speed = 1f;
				}
				this._Animator.SetTrigger("animStart");
				this._Animator.CrossFadeInFixedTime(this._Animator.GetNextAnimatorStateInfo(0).fullPathHash, 0f, 0);
				if (!this.interruptable)
				{
					base.StartCoroutine(this.<Interact>g__playAnim|10_1());
				}
			}
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000C5AC File Offset: 0x0000A7AC
	private void Update()
	{
		if (this.interruptable && this.isPlaying && Input.GetMouseButtonDown(0))
		{
			this._Animator.ResetTrigger("animStart");
			this._Animator.SetTrigger("loopEnd");
			base.StartCoroutine(this.<Update>g__waitStopping|11_0());
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000C600 File Offset: 0x0000A800
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

	// Token: 0x0600021A RID: 538 RVA: 0x0000C668 File Offset: 0x0000A868
	[CompilerGenerated]
	private IEnumerator <Interact>g__waitPlaying|10_0()
	{
		yield return new WaitForSeconds(0.1f);
		this.isPlaying = true;
		yield break;
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000C677 File Offset: 0x0000A877
	[CompilerGenerated]
	private IEnumerator <Interact>g__playAnim|10_1()
	{
		if (this.startAudioClips != null && this.startAudioClips.Count > 0 && !this.loadingIn)
		{
			int num = Random.Range(0, this.startAudioClips.Count);
			Singleton<AudioManager>.Instance.PlayTrack(this.startAudioClips[num], AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.NONE, false);
		}
		this.loadingIn = false;
		yield return new WaitForSeconds(this.animLength);
		this.camLogic(10);
		this._Animator.ResetTrigger("animStart");
		this.isPlaying = false;
		yield break;
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000C686 File Offset: 0x0000A886
	[CompilerGenerated]
	private IEnumerator <Update>g__waitStopping|11_0()
	{
		yield return new WaitForSeconds(this.animLength);
		this.isPlaying = false;
		this.camLogic(10);
		yield break;
	}

	// Token: 0x040002F2 RID: 754
	public CinemachineVirtualCamera VirtualCam;

	// Token: 0x040002F3 RID: 755
	public float animLength = 1f;

	// Token: 0x040002F4 RID: 756
	public bool interruptable;

	// Token: 0x040002F5 RID: 757
	private bool isPlaying;

	// Token: 0x040002F6 RID: 758
	private Animator _Animator;

	// Token: 0x040002F7 RID: 759
	public Renderer tempObject;

	// Token: 0x040002F8 RID: 760
	public UnityEvent HeardInteract;

	// Token: 0x040002F9 RID: 761
	public List<AudioClip> startAudioClips;

	// Token: 0x040002FA RID: 762
	public List<AudioClip> endAudioClips;
}
