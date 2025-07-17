using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x02000085 RID: 133
public class GenericInteractable : Interactable
{
	// Token: 0x0600047F RID: 1151 RVA: 0x0001B264 File Offset: 0x00019464
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001B26B File Offset: 0x0001946B
	public void BlockMagical()
	{
		this.blockMagical = true;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0001B274 File Offset: 0x00019474
	public bool GetState()
	{
		return this.state;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0001B27C File Offset: 0x0001947C
	private void OnValidate()
	{
		this.Validate();
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0001B284 File Offset: 0x00019484
	private void Validate()
	{
		if (this.interactableObj == null)
		{
			this.interactableObj = base.GetComponentInChildren<InteractableObj>();
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0001B2A0 File Offset: 0x000194A0
	public override void Awake()
	{
		base.Awake();
		this.Validate();
		if (this.animator == null)
		{
			this.animator = base.GetComponent<Animator>();
		}
		Animator animator;
		if (!this.animator && base.transform.parent != null && base.transform.parent.TryGetComponent<Animator>(out animator))
		{
			this.animator = animator;
		}
		if (this.animator)
		{
			this.animator.keepAnimatorStateOnDisable = true;
		}
		this.loadingIn = false;
		this.standardSfx_activate.RemoveAll((AudioClip clip) => clip == null);
		this.standardSfx_deactivate.RemoveAll((AudioClip clip) => clip == null);
		this.magicalSfx_activate.RemoveAll((AudioClip clip) => clip == null);
		this.magicalSfx_deactivate.RemoveAll((AudioClip clip) => clip == null);
		this.interruptable = false;
		if (!this.maintainAnimator)
		{
			if (this.animator)
			{
				this.animator.enabled = false;
			}
			if (this.alternateAnimator)
			{
				this.alternateAnimator.enabled = false;
			}
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0001B41C File Offset: 0x0001961C
	public override void Interact()
	{
		if (this.interactableObj != null && Singleton<Save>.Instance.GetDateStatusRealized(this.interactableObj.InternalName()) == RelationshipStatus.Realized && !this.realizedOverride && Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		this.interactableObj == null;
		if (this.animator)
		{
			this.animator.enabled = true;
			if (this.interactableObj != null)
			{
				this.animator.SetInteger("ending", (int)Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()));
			}
		}
		if (this.alternateAnimator)
		{
			this.alternateAnimator.enabled = true;
			if (this.interactableObj != null)
			{
				this.alternateAnimator.SetInteger("ending", (int)Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()));
			}
		}
		if (this.stateLock && !this.interruptable)
		{
			return;
		}
		if (Singleton<Dateviators>.Instance.Equipped && this.blockMagical)
		{
			return;
		}
		if (!this.interruptable && this.maintainAnimator && !this.AreAllAnimatorsFinished())
		{
			return;
		}
		if (this.interruptable && this.autoResets && this.currentlyPlaying == GenericInteractable.AnimType.Standard)
		{
			base.StopAllCoroutines();
			this.ResetTriggers();
			this.EndOfAnimationChecks();
		}
		if (this.interruptable && this.autoResetsMagical && this.currentlyPlaying == GenericInteractable.AnimType.Magical)
		{
			base.StopAllCoroutines();
			this.ResetTriggers();
			this.EndOfAnimationChecks();
		}
		else if (this.interruptable)
		{
			base.StopAllCoroutines();
			this.ResetTriggers();
			this.EndOfAnimationChecks();
		}
		base.StartCoroutine(this.InteractCoroutine());
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0001B5C8 File Offset: 0x000197C8
	public override void OnLoadNoInteract()
	{
		if (this.animator != null || this.currentAnimator != null || this.alternateAnimator != null)
		{
			if (this.interactedWithState && !this.autoResets && (this.currentlyPlaying == GenericInteractable.AnimType.Standard || this.currentlyPlaying == GenericInteractable.AnimType.Neither))
			{
				this.state = true;
			}
			if (this.interactedWithState && !this.autoResetsMagical && this.currentlyPlaying == GenericInteractable.AnimType.Magical)
			{
				this.state = true;
			}
			this.ResetTriggers();
		}
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0001B64D File Offset: 0x0001984D
	public void ForceInteractIfNotInteracted()
	{
		if (!this.interactedWithState)
		{
			base.StartCoroutine(this.InteractCoroutine());
			this.ToggleInteractedWith(BetterPlayerControl.Instance.transform.position, false);
		}
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001B67A File Offset: 0x0001987A
	private IEnumerator InteractCoroutine()
	{
		if (this.animator)
		{
			this.animator.enabled = true;
			while (this.animator.IsInTransition(0))
			{
				yield return null;
			}
		}
		if (this.alternateAnimator && this.usingAlternate)
		{
			this.alternateAnimator.enabled = true;
			while (this.alternateAnimator.IsInTransition(0))
			{
				yield return null;
			}
		}
		while (this.stateLock)
		{
			yield return null;
		}
		UnityEvent preInteract = this.PreInteract;
		if (preInteract != null)
		{
			preInteract.Invoke();
		}
		yield return null;
		this.Interact(true, false);
		yield break;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0001B689 File Offset: 0x00019889
	public void SetCameraVars(GenericInteractable.AnimType type, BetterPlayerControl.PlayerState stateWhileAnim)
	{
		this.playerStateWhileAnimating = stateWhileAnim;
		this.whichUsesCam = type;
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0001B699 File Offset: 0x00019899
	private bool AreAllAnimatorsFinished()
	{
		return (!this.animator || GenericInteractable.IsAnimatorIdle(this.animator)) && (!this.alternateAnimator || GenericInteractable.IsAnimatorIdle(this.alternateAnimator));
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001B6D4 File Offset: 0x000198D4
	public void Interact(bool playSound = true, bool forceMagicalInteraction = false)
	{
		if (this.stateLock || !BetterPlayerControl.Instance)
		{
			return;
		}
		this.animator;
		if (this.animator)
		{
			this.animator.enabled = true;
		}
		if (this.alternateAnimator && this.usingAlternate)
		{
			this.alternateAnimator.enabled = true;
		}
		if (BetterPlayerControl.Instance.STATE != this.playerStateWhileAnimating && this.playerStateWhileAnimating != BetterPlayerControl.PlayerState.CanControl && !this.loadingIn)
		{
			this.playerStateBeforeAnimating = BetterPlayerControl.PlayerState.CanControl;
		}
		if ((this.state || this.interactedWithState) && base.gameObject.GetComponent<SubInteractable>() != null)
		{
			base.gameObject.GetComponent<SubInteractable>().Interact();
		}
		if (Singleton<Dateviators>.Instance.Equipped && !this.blockMagical && !this.loadingIn && (Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Friend || Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Love || Singleton<GameController>.Instance.debugMagical || forceMagicalInteraction))
		{
			if (Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Friend && !this.blockMagicalFriendLine && this.allowMagicalLines)
			{
				string text = Path.Combine("VoiceOver", "magical_friend", this.interactableObj.InternalName());
				if (this.friendClipOverride == null)
				{
					Singleton<AudioManager>.Instance.PlayTrack(text, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
				}
				else
				{
					Singleton<AudioManager>.Instance.PlayTrack(this.friendClipOverride, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
				}
			}
			else if (Singleton<Save>.Instance.GetDateStatus(this.interactableObj.InternalName()) == RelationshipStatus.Love && !this.blockMagicalLoveLine && this.allowMagicalLines)
			{
				string text2 = Path.Combine("VoiceOver", "magical_love", this.interactableObj.InternalName());
				if (this.loveClipOverride == null)
				{
					Singleton<AudioManager>.Instance.PlayTrack(text2, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, true);
				}
				else
				{
					Singleton<AudioManager>.Instance.PlayTrack(this.loveClipOverride, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
				}
			}
			this.state = !this.state;
			this.MagicalInteract(playSound);
			if (this.animator)
			{
				this.animator.SetBool("isActive", this.state);
			}
			if (this.alternateAnimator && this.usingAlternate)
			{
				this.alternateAnimator.SetBool("isActive", this.state);
			}
		}
		else if (!Singleton<Dateviators>.Instance.Equipped)
		{
			if (this.state && this.Name == "Rug")
			{
				playSound = false;
			}
			else
			{
				this.state = !this.state;
			}
			this.StandardInteract(playSound);
			if (this.animator)
			{
				this.animator.SetBool("isActive", this.state);
			}
			if (this.alternateAnimator && this.usingAlternate)
			{
				this.alternateAnimator.SetBool("isActive", this.state);
			}
		}
		else
		{
			UnityEvent interactBlocked = this.InteractBlocked;
			if (interactBlocked == null)
			{
				return;
			}
			interactBlocked.Invoke();
			return;
		}
		base.StartCoroutine(this.ResetTrigger(playSound));
		if (!this.alternateAnimator && !this.animator)
		{
			this.ResetTriggers();
			this.OnAnimationReset();
			UnityEvent interactEnded = this.InteractEnded;
			if (interactEnded == null)
			{
				return;
			}
			interactEnded.Invoke();
		}
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0001BAA8 File Offset: 0x00019CA8
	private IEnumerator ResetTrigger(bool playSound)
	{
		float origAnimSpeed = 1f;
		if (this.currentAnimator)
		{
			origAnimSpeed = this.currentAnimator.speed;
		}
		if (this.loadingIn && this.currentAnimator != null)
		{
			this.currentAnimator.speed = 50f;
		}
		UnityEvent interactRestartCoroutineBegun = this.InteractRestartCoroutineBegun;
		if (interactRestartCoroutineBegun != null)
		{
			interactRestartCoroutineBegun.Invoke();
		}
		if (!this.loadingIn && playSound)
		{
			this.PlaySounds();
		}
		if (this.currentAnimator != null)
		{
			if (!this.currentAnimator.IsInTransition(0))
			{
				yield return null;
			}
			yield return new WaitUntil(() => !this.currentAnimator.IsInTransition(0));
			yield return new WaitUntil(() => this.currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f);
			UnityEvent interactMidAnimation = this.InteractMidAnimation;
			if (interactMidAnimation != null)
			{
				interactMidAnimation.Invoke();
			}
			yield return new WaitUntil(() => this.currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
		}
		if (this.currentAnimator != null && this.currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && this.interruptable)
		{
			this.currentAnimator.Play(0, 0, 0f);
		}
		if (this.currentAnimator != null)
		{
			yield return new WaitUntil(() => this.currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f);
		}
		if (this.currentAnimator != null)
		{
			yield return new WaitUntil(() => this.currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= this.animationEventOffset || this.currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime == 1f);
		}
		if (this.currentAnimator != null)
		{
			yield return new WaitWhile(() => this.currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);
		}
		if (this.currentAnimator != null)
		{
			yield return new WaitUntil(() => !this.currentAnimator.IsInTransition(0));
			if (this.currentAnimator.speed != origAnimSpeed)
			{
				this.currentAnimator.speed = origAnimSpeed;
			}
		}
		if (this.autoResets && this.currentlyPlaying == GenericInteractable.AnimType.Standard)
		{
			this.OnAnimationReset();
		}
		if (this.autoResetsMagical && this.currentlyPlaying == GenericInteractable.AnimType.Magical)
		{
			this.OnAnimationReset();
		}
		bool loadingIn = this.loadingIn;
		this.EndEvents();
		this.EndOfAnimationChecks();
		if (loadingIn && this.interactedWithState && !this.autoResets && (this.currentlyPlaying == GenericInteractable.AnimType.Standard || this.currentlyPlaying == GenericInteractable.AnimType.Neither))
		{
			this.state = true;
		}
		if (loadingIn && this.interactedWithState && !this.autoResetsMagical && this.currentlyPlaying == GenericInteractable.AnimType.Magical)
		{
			this.state = true;
		}
		this.ResetTriggers();
		this.CamLogic(-1);
		yield break;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0001BAC0 File Offset: 0x00019CC0
	private static bool IsAnimatorIdle(Animator animator)
	{
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animator.IsInTransition(0);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001BAF1 File Offset: 0x00019CF1
	public void SpeedUpAnimation()
	{
		this.currentAnimator.speed = 10000f;
		this.StopSounds();
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0001BB09 File Offset: 0x00019D09
	public override void ToggleInteractedWith(Vector3 positionWhenInteracting, bool loading = false)
	{
		this.interactedWithState = !this.interactedWithState;
		this.interactedPosition = positionWhenInteracting;
		if (loading)
		{
			UnityEvent toggledInteractedWith = this.ToggledInteractedWith;
			if (toggledInteractedWith == null)
			{
				return;
			}
			toggledInteractedWith.Invoke();
		}
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001BB34 File Offset: 0x00019D34
	public void EndOfAnimationChecks()
	{
		this.loadingIn = false;
		this.CamLogic(-1);
		this.stateLock = false;
		this.ReturnControl();
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0001BB54 File Offset: 0x00019D54
	private void EndEvents()
	{
		if (!this.loadingIn)
		{
			if (this.currentlyPlaying == GenericInteractable.AnimType.Magical)
			{
				UnityEvent interactEndedMagical = this.InteractEndedMagical;
				if (interactEndedMagical != null)
				{
					interactEndedMagical.Invoke();
				}
			}
			else if (this.currentlyPlaying == GenericInteractable.AnimType.Standard)
			{
				UnityEvent interactEndedStandard = this.InteractEndedStandard;
				if (interactEndedStandard != null)
				{
					interactEndedStandard.Invoke();
				}
			}
			else
			{
				UnityEvent interactEnded = this.InteractEnded;
				if (interactEnded != null)
				{
					interactEnded.Invoke();
				}
			}
		}
		if (this.animator && !this.maintainAnimator)
		{
			this.animator.enabled = false;
		}
		if (this.alternateAnimator && !this.maintainAnimator)
		{
			this.alternateAnimator.enabled = false;
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0001BBF4 File Offset: 0x00019DF4
	public void StandardInteract(bool playSound = true)
	{
		this.currentlyPlaying = GenericInteractable.AnimType.Standard;
		bool flag = false;
		UnityEvent interactStarted = this.InteractStarted;
		if (interactStarted != null)
		{
			interactStarted.Invoke();
		}
		if (this.animator)
		{
			if (!this.interruptable)
			{
				this.stateLock = true;
			}
			if (this.alternateAnimType == GenericInteractable.AnimType.Standard && this.alternateAnimator)
			{
				this.currentAnimator = this.alternateAnimator;
				this.usingAlternate = true;
				this.alternateAnimator.SetTrigger("standardAnimStart");
			}
			else
			{
				this.currentAnimator = this.animator;
				this.usingAlternate = false;
				this.animator.SetTrigger("standardAnimStart");
			}
			if (this.whichUsesCam == GenericInteractable.AnimType.Both || this.whichUsesCam == GenericInteractable.AnimType.Standard)
			{
				flag = true;
			}
		}
		if (!this.loadingIn && flag)
		{
			this.CamLogic(100);
		}
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0001BCC4 File Offset: 0x00019EC4
	public void MagicalInteract(bool playSound = true)
	{
		if (this.loadingIn)
		{
			return;
		}
		this.currentlyPlaying = GenericInteractable.AnimType.Magical;
		bool flag = false;
		UnityEvent interactStartedMagical = this.InteractStartedMagical;
		if (interactStartedMagical != null)
		{
			interactStartedMagical.Invoke();
		}
		if (this.animator)
		{
			this.animator.enabled = true;
			if (!this.interruptable)
			{
				this.stateLock = true;
			}
			if (this.alternateAnimType == GenericInteractable.AnimType.Magical && this.alternateAnimator)
			{
				this.currentAnimator = this.alternateAnimator;
				this.usingAlternate = true;
				this.alternateAnimator.SetTrigger("magicalAnimStart");
			}
			else
			{
				this.currentAnimator = this.animator;
				this.usingAlternate = false;
				this.animator.SetTrigger("magicalAnimStart");
			}
			if (this.whichUsesCam == GenericInteractable.AnimType.Both || this.whichUsesCam == GenericInteractable.AnimType.Magical)
			{
				flag = true;
			}
		}
		if (!this.loadingIn && flag)
		{
			this.CamLogic(100);
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0001BDA4 File Offset: 0x00019FA4
	public void PlayLoopedSound()
	{
		if (this.loadingIn)
		{
			return;
		}
		if (this.standardSfx_loop == null)
		{
			return;
		}
		if (this.standardSfx_loop.Count == 0)
		{
			return;
		}
		if (Singleton<AudioManager>.Instance.GetTrack(this.standardSfx_loop[0].name) == null)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.standardSfx_loop[0], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, true, SFX_SUBGROUP.FOLEY, false);
		}
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0001BE24 File Offset: 0x0001A024
	public void StopLoopedSound()
	{
		if (this.loadingIn)
		{
			return;
		}
		if (this.standardSfx_loop == null)
		{
			return;
		}
		if (this.standardSfx_loop.Count == 0)
		{
			return;
		}
		Singleton<AudioManager>.Instance.StopTrack(this.standardSfx_loop[0].name, 0f);
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0001BE74 File Offset: 0x0001A074
	public void PlaySounds()
	{
		if (this.loadingIn)
		{
			return;
		}
		if (!this.animator && !this.alternateAnimator)
		{
			return;
		}
		if (this.currentlyPlaying == GenericInteractable.AnimType.Standard)
		{
			this.PlayStandardSound();
			return;
		}
		if (this.currentlyPlaying == GenericInteractable.AnimType.Magical)
		{
			this.PlayMagicalSound();
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0001BEC4 File Offset: 0x0001A0C4
	public void PlayMagicalSound()
	{
		if (this.state)
		{
			if (this.magicalSfx_activate.Count > 0)
			{
				int num = Random.Range(0, this.magicalSfx_activate.Count);
				if (this.magicalSfx_activate[this.prevMagActIndex] != null)
				{
					Singleton<AudioManager>.Instance.StopTrack(this.magicalSfx_activate[this.prevMagActIndex].name, 0f);
				}
				Singleton<AudioManager>.Instance.PlayTrack(this.magicalSfx_activate[num], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
				this.prevMagActIndex = num;
				return;
			}
		}
		else if (this.magicalSfx_deactivate.Count > 0)
		{
			int num = Random.Range(0, this.magicalSfx_deactivate.Count);
			if (this.magicalSfx_loop != null && this.magicalSfx_loop.Count > 0 && this.magicalSfx_loop[0] != null)
			{
				Singleton<AudioManager>.Instance.StopTrack(this.magicalSfx_loop[0].name, 0f);
			}
			if (this.magicalSfx_deactivate[this.prevMagDeactIndex] != null)
			{
				Singleton<AudioManager>.Instance.StopTrack(this.magicalSfx_deactivate[this.prevMagDeactIndex].name, 0f);
			}
			Singleton<AudioManager>.Instance.PlayTrack(this.magicalSfx_deactivate[num], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			this.prevMagDeactIndex = num;
		}
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0001C051 File Offset: 0x0001A251
	protected void OnDestroy()
	{
		this.StopSounds();
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0001C05C File Offset: 0x0001A25C
	public void PlayStandardSound()
	{
		if (this.state)
		{
			if (this.standardSfx_activate.Count > 0)
			{
				int num = Random.Range(0, this.standardSfx_activate.Count);
				Singleton<AudioManager>.Instance.StopTrack(this.standardSfx_activate[this.prevStdActIndex].name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(this.standardSfx_activate[num], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
				this.prevStdActIndex = num;
				return;
			}
		}
		else if (this.standardSfx_deactivate.Count > 0)
		{
			int num = Random.Range(0, this.standardSfx_deactivate.Count);
			if (this.standardSfx_loop != null && this.standardSfx_loop.Count > 0)
			{
				Singleton<AudioManager>.Instance.StopTrack(this.standardSfx_loop[0].name, 0f);
			}
			Singleton<AudioManager>.Instance.StopTrack(this.standardSfx_deactivate[this.prevStdDeactIndex].name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(this.standardSfx_deactivate[num], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			this.prevStdDeactIndex = num;
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0001C1A0 File Offset: 0x0001A3A0
	public void StopSounds()
	{
		this.StopSounds(this.standardSfx_activate);
		this.StopSounds(this.standardSfx_deactivate);
		this.StopSounds(this.standardSfx_loop);
		this.StopSounds(this.magicalSfx_activate);
		this.StopSounds(this.magicalSfx_deactivate);
		this.StopSounds(this.magicalSfx_loop);
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0001C1F8 File Offset: 0x0001A3F8
	private void StopSounds(IReadOnlyCollection<AudioClip> sounds)
	{
		if (Singleton<AudioManager>.Instance != null)
		{
			foreach (AudioClip audioClip in sounds)
			{
				if (audioClip != null)
				{
					Singleton<AudioManager>.Instance.StopTrack(audioClip.name, 0f);
				}
			}
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0001C264 File Offset: 0x0001A464
	public void ResetTriggers()
	{
		if (this.alternateAnimator && this.alternateAnimType == GenericInteractable.AnimType.Magical)
		{
			this.alternateAnimator.SetBool("isActive", this.state);
			this.alternateAnimator.ResetTrigger("magicalAnimStart");
			this.alternateAnimator.ResetTrigger("animReset");
		}
		else if (this.animator)
		{
			this.animator.SetBool("isActive", this.state);
			this.animator.ResetTrigger("magicalAnimStart");
			this.animator.ResetTrigger("animReset");
		}
		if (this.alternateAnimator && this.alternateAnimType == GenericInteractable.AnimType.Standard)
		{
			this.alternateAnimator.SetBool("isActive", this.state);
			this.alternateAnimator.ResetTrigger("standardAnimStart");
			this.alternateAnimator.ResetTrigger("animReset");
			return;
		}
		if (this.animator)
		{
			this.animator.SetBool("isActive", this.state);
			this.animator.ResetTrigger("standardAnimStart");
			this.animator.ResetTrigger("animReset");
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0001C391 File Offset: 0x0001A591
	public void OnAnimationReset()
	{
		this.state = false;
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0001C39C File Offset: 0x0001A59C
	public void ReturnControl()
	{
		if (this.playerStateWhileAnimating == BetterPlayerControl.PlayerState.CanControl)
		{
			return;
		}
		BetterPlayerControl.Instance.ChangePlayerState(this.playerStateBeforeAnimating);
		this.playerStateBeforeAnimating = BetterPlayerControl.PlayerState.CanControl;
		if (this.showCinematicBarsWhenAnimatingCamera && (this.whichUsesCam == GenericInteractable.AnimType.Both || this.whichUsesCam == GenericInteractable.AnimType.Magical))
		{
			CinematicBars.Hide(-1f);
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0001C3EC File Offset: 0x0001A5EC
	public void TurnOffAnimator()
	{
		this.animator.enabled = false;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001C3FC File Offset: 0x0001A5FC
	public void CamLogic(int priority)
	{
		if (this.loadingIn)
		{
			this.VirtualCam.Priority = -1;
			return;
		}
		if (this.VirtualCam != null && (this.whichUsesCam == this.currentlyPlaying || this.whichUsesCam == GenericInteractable.AnimType.Both))
		{
			if (priority > 0)
			{
				BetterPlayerControl.Instance.ChangePlayerState(this.playerStateWhileAnimating);
				Singleton<CanvasUIManager>.Instance.Hide();
				if (this.showCinematicBarsWhenAnimatingCamera)
				{
					CinematicBars.Show(-1f);
				}
			}
			else
			{
				Singleton<CanvasUIManager>.Instance.Show();
				if (this.showCinematicBarsWhenAnimatingCamera)
				{
					CinematicBars.Hide(-1f);
				}
			}
			CinemachineBlendDefinition cinemachineBlendDefinition = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f);
			CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this.VirtualCam);
			if (cinemachineBrain != null)
			{
				cinemachineBrain.m_DefaultBlend = cinemachineBlendDefinition;
			}
			this.VirtualCam.Priority = priority;
			return;
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001C4D4 File Offset: 0x0001A6D4
	public static string GetMissingData()
	{
		StringBuilder stringBuilder = new StringBuilder();
		GenericInteractable[] array = Object.FindObjectsOfType<GenericInteractable>(true);
		InteractableObj[] array2 = Object.FindObjectsOfType<InteractableObj>(true);
		stringBuilder.Append("GenericInteractable\n================================\n");
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			if (array[i].interactableObj == null)
			{
				stringBuilder.Append(GenericInteractable.GetPath(array[i].gameObject) + "\n");
			}
			i++;
		}
		stringBuilder.Append("\nInteractableObj\n================================\n");
		int j = 0;
		int num2 = array2.Length;
		while (j < num2)
		{
			if (array2[j].CleanEvent == null)
			{
				stringBuilder.Append(GenericInteractable.GetPath(array2[j].gameObject) + "\n");
			}
			j++;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0001C594 File Offset: 0x0001A794
	private static string GetPath(GameObject obj)
	{
		string text = "/" + obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = "/" + obj.name + text;
		}
		return text;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0001C5EC File Offset: 0x0001A7EC
	private void OutputAnimators(string title = "")
	{
		if (!string.IsNullOrEmpty(title))
		{
			title = "[" + title + "] ";
		}
		if (this.animator)
		{
			this.OutputAnimState(this.animator, title + "Main");
		}
		if (this.alternateAnimator)
		{
			this.OutputAnimState(this.alternateAnimator, title + "Alternate");
		}
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0001C65C File Offset: 0x0001A85C
	private void OutputAnimState(Animator anim, string title)
	{
		string text = string.Format("Anim: {0}\nLayer Count: {1}\n", title, anim.layerCount);
		for (int i = 0; i < anim.layerCount; i++)
		{
			AnimatorStateInfo currentAnimatorStateInfo = anim.GetCurrentAnimatorStateInfo(i);
			AnimatorTransitionInfo animatorTransitionInfo = anim.GetAnimatorTransitionInfo(i);
			text += string.Format("{0} Layer name = {1}\n{2}: IsInTransition = {3}\n{4} state normalizedTime = {5}\n{6} Trans normal time = {7}\n\n", new object[]
			{
				1,
				anim.GetLayerName(i),
				i,
				anim.IsInTransition(i),
				i,
				currentAnimatorStateInfo.normalizedTime,
				1,
				animatorTransitionInfo.normalizedTime
			});
		}
	}

	// Token: 0x04000480 RID: 1152
	[FormerlySerializedAs("active")]
	[Space(5f)]
	[Header("Status")]
	[SerializeField]
	[LabeledBool("Active", "Inactive")]
	public bool state;

	// Token: 0x04000481 RID: 1153
	[SerializeField]
	private bool maintainAnimator;

	// Token: 0x04000482 RID: 1154
	[LabeledBool("Clean", "Not Clean")]
	public bool clean;

	// Token: 0x04000483 RID: 1155
	[SerializeField]
	[Tooltip("Some objects will steal control from the player.")]
	private BetterPlayerControl.PlayerState playerStateWhileAnimating;

	// Token: 0x04000484 RID: 1156
	private BetterPlayerControl.PlayerState playerStateBeforeAnimating;

	// Token: 0x04000485 RID: 1157
	[SerializeField]
	public bool allowMagicalLines = true;

	// Token: 0x04000486 RID: 1158
	[SerializeField]
	private bool realizedOverride;

	// Token: 0x04000487 RID: 1159
	[FormerlySerializedAs("standardSfx_active")]
	[Space(5f)]
	[Header("Audio")]
	public int prevStdActIndex;

	// Token: 0x04000488 RID: 1160
	public int prevStdDeactIndex;

	// Token: 0x04000489 RID: 1161
	public int prevMagActIndex;

	// Token: 0x0400048A RID: 1162
	public int prevMagDeactIndex;

	// Token: 0x0400048B RID: 1163
	public AudioClip friendClipOverride;

	// Token: 0x0400048C RID: 1164
	public AudioClip loveClipOverride;

	// Token: 0x0400048D RID: 1165
	public List<AudioClip> standardSfx_activate = new List<AudioClip>();

	// Token: 0x0400048E RID: 1166
	public List<AudioClip> standardSfx_deactivate = new List<AudioClip>();

	// Token: 0x0400048F RID: 1167
	public List<AudioClip> standardSfx_loop = new List<AudioClip>();

	// Token: 0x04000490 RID: 1168
	public List<AudioClip> magicalSfx_activate = new List<AudioClip>();

	// Token: 0x04000491 RID: 1169
	public List<AudioClip> magicalSfx_deactivate = new List<AudioClip>();

	// Token: 0x04000492 RID: 1170
	public List<AudioClip> magicalSfx_loop = new List<AudioClip>();

	// Token: 0x04000493 RID: 1171
	[Space(5f)]
	[Header("Animation")]
	[SerializeField]
	[Tooltip("Mark this true if the animation plays fully and resets the object without player intervention. This is NOT true for things like cabinet doors which stay open, etc.")]
	private bool autoResets = true;

	// Token: 0x04000494 RID: 1172
	[SerializeField]
	[Tooltip("Mark this true if the animation plays fully and resets the object without player intervention. This is NOT true for things like cabinet doors which stay open, etc.")]
	private bool autoResetsMagical = true;

	// Token: 0x04000495 RID: 1173
	[SerializeField]
	private bool usingAlternate;

	// Token: 0x04000496 RID: 1174
	[SerializeField]
	[Tooltip("Can player interrupt animation while playing.")]
	public bool interruptable = true;

	// Token: 0x04000497 RID: 1175
	[SerializeField]
	[Tooltip("Can player interrupt animation while playing.")]
	public bool blockMagical;

	// Token: 0x04000498 RID: 1176
	[SerializeField]
	private float animationEventOffset = 0.99f;

	// Token: 0x04000499 RID: 1177
	public CinemachineVirtualCamera VirtualCam;

	// Token: 0x0400049A RID: 1178
	[SerializeField]
	private Animator currentAnimator;

	// Token: 0x0400049B RID: 1179
	[SerializeField]
	public Animator animator;

	// Token: 0x0400049C RID: 1180
	[SerializeField]
	private GenericInteractable.AnimType alternateAnimType = GenericInteractable.AnimType.Neither;

	// Token: 0x0400049D RID: 1181
	[SerializeField]
	public GenericInteractable.AnimType whichUsesCam = GenericInteractable.AnimType.Neither;

	// Token: 0x0400049E RID: 1182
	[SerializeField]
	private GenericInteractable.AnimType currentlyPlaying = GenericInteractable.AnimType.Neither;

	// Token: 0x0400049F RID: 1183
	[SerializeField]
	private bool showCinematicBarsWhenAnimatingCamera = true;

	// Token: 0x040004A0 RID: 1184
	public Animator alternateAnimator;

	// Token: 0x040004A1 RID: 1185
	private InteractableObj interactableObj;

	// Token: 0x040004A2 RID: 1186
	[Header("Events")]
	public UnityEvent PreInteract = new UnityEvent();

	// Token: 0x040004A3 RID: 1187
	public UnityEvent InteractStartedMagical = new UnityEvent();

	// Token: 0x040004A4 RID: 1188
	public UnityEvent InteractStarted = new UnityEvent();

	// Token: 0x040004A5 RID: 1189
	public UnityEvent InteractEndedStandard = new UnityEvent();

	// Token: 0x040004A6 RID: 1190
	public UnityEvent InteractEndedMagical = new UnityEvent();

	// Token: 0x040004A7 RID: 1191
	public UnityEvent InteractEnded = new UnityEvent();

	// Token: 0x040004A8 RID: 1192
	public UnityEvent InteractBlocked = new UnityEvent();

	// Token: 0x040004A9 RID: 1193
	public UnityEvent InteractRestartCoroutineBegun = new UnityEvent();

	// Token: 0x040004AA RID: 1194
	public UnityEvent InteractMidAnimation = new UnityEvent();

	// Token: 0x040004AB RID: 1195
	public UnityEvent ToggledInteractedWith;

	// Token: 0x040004AC RID: 1196
	[SerializeField]
	private bool blockMagicalFriendLine;

	// Token: 0x040004AD RID: 1197
	[SerializeField]
	private bool blockMagicalLoveLine;

	// Token: 0x020002CE RID: 718
	public enum AnimType
	{
		// Token: 0x04001119 RID: 4377
		Magical,
		// Token: 0x0400111A RID: 4378
		Standard,
		// Token: 0x0400111B RID: 4379
		Both,
		// Token: 0x0400111C RID: 4380
		Neither
	}
}
