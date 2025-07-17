using System;
using System.Collections;
using System.IO;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000084 RID: 132
public class FrontDoor : Interactable
{
	// Token: 0x0600046E RID: 1134 RVA: 0x0001AE65 File Offset: 0x00019065
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001AE6C File Offset: 0x0001906C
	private void Start()
	{
		if (FrontDoor.Instance == null)
		{
			FrontDoor.Instance = this;
		}
		UnityEvent dialogueExit = Singleton<GameController>.Instance.DialogueExit;
		if (dialogueExit == null)
		{
			return;
		}
		dialogueExit.AddListener(delegate
		{
			this.inNarratorConversation = false;
		});
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001AEA1 File Offset: 0x000190A1
	private void OnEnable()
	{
		FrontDoor.Instance = this;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001AEAC File Offset: 0x000190AC
	public override void Interact()
	{
		if (this.locked || this.stateLock)
		{
			if (this.lockedSound != null)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.lockedSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			}
			return;
		}
		if (Singleton<Save>.Instance.GetDateStatus("dorian") == RelationshipStatus.Love && Singleton<Save>.Instance.GetDateStatusRealized("dorian") != RelationshipStatus.Realized && Singleton<Dateviators>.Instance.Equipped)
		{
			string text = Path.Combine("VoiceOver", "magical_love", "dorian");
			if (this.loveClipOverride == null)
			{
				Singleton<AudioManager>.Instance.PlayTrack(text, AUDIO_TYPE.SFX, false, false, 0.5f, true, 1f, base.gameObject, false, null, SFX_SUBGROUP.FOLEY, null, false);
			}
			else
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.loveClipOverride, AUDIO_TYPE.SFX, false, false, 0.1f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			}
		}
		if (!Singleton<Dateviators>.Instance.Equipped && Singleton<Save>.Instance.GetDateStatus("skylar_specs") != RelationshipStatus.Unmet && Singleton<Save>.Instance.SafetyCheckOnGameLoad())
		{
			if (base.GetComponent<InteractableObj>() == null)
			{
				base.gameObject.AddComponent<InteractableObj>();
			}
			base.GetComponent<InteractableObj>().inkFileName = "fpn_tutorial.fpn_front_door_sorter";
			Singleton<GameController>.Instance.ForceSelectObj(base.GetComponent<InteractableObj>(), null);
			this.inNarratorConversation = true;
		}
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001B00F File Offset: 0x0001920F
	public void ConfigureOpenDoor()
	{
		Singleton<GameController>.Instance.SetDelegateMethodWhenChatEnds(new GameController.DelegateAfterChatEndsEvents(this.OpenFrontDoor));
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0001B027 File Offset: 0x00019227
	public void SwapToHelicopterAnim()
	{
		this.animator.runtimeAnimatorController = this.helicopterController;
		this.usingHelicopter = true;
		this.helicopterAudioSource = this.helicopter.GetComponentInChildren<AudioSource>();
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001B054 File Offset: 0x00019254
	public void OpenFrontDoor()
	{
		this.inNarratorConversation = false;
		this.stateLock = true;
		Singleton<GameController>.Instance.TriggerEndGame();
		this.animator.SetTrigger("standardAnimStart");
		this.CamLogic(100);
		if (this.usingHelicopter)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.openSoundHelicopter, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		else
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.openSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		Singleton<DayNightCycle>.Instance.StopMusic();
		CinematicBars.Show(-1f);
		base.StartCoroutine(this.ResetTrigger());
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0001B10A File Offset: 0x0001930A
	private IEnumerator ResetTrigger()
	{
		yield return null;
		Singleton<GameController>.Instance.ForceShowHUD();
		if (this.animator != null)
		{
			while (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
			{
				if (this.animator.speed <= 0f)
				{
					break;
				}
				Singleton<GameController>.Instance.ForceShowHUD();
				yield return null;
			}
		}
		else
		{
			yield return new WaitWhile(() => this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f && this.animator.speed > 0f);
		}
		if (BetterPlayerControl.Instance.isGameEndingOn)
		{
			this.animator.speed = 0f;
		}
		if (!this.loadingIn)
		{
			UnityEvent interactEnded = this.InteractEnded;
			if (interactEnded != null)
			{
				interactEnded.Invoke();
			}
		}
		this.loadingIn = false;
		if (Singleton<Save>.Instance.GetDateStatus("betty") != RelationshipStatus.Unmet)
		{
			Singleton<InkController>.Instance.story.variablesState["tutorial"] = "done";
		}
		CinematicBars.Hide(0.01f);
		yield return new WaitWhile(() => CinematicBars.CurrentShowState == CinematicBars.ShowState.OnScreen);
		Singleton<GameController>.Instance.ForceHideHUD();
		if (base.GetComponent<InteractableObj>() == null)
		{
			base.gameObject.AddComponent<InteractableObj>();
		}
		base.GetComponent<InteractableObj>().inkFileName = "epilogue_final.epilogue_sorter";
		Singleton<GameController>.Instance.ForceSelectObj(base.GetComponent<InteractableObj>(), null);
		Singleton<Dateviators>.Instance.Dequip();
		if (!BetterPlayerControl.Instance.isGameEndingOn)
		{
			this.CamLogic(-1);
		}
		else
		{
			this.CamLogic(50);
		}
		yield break;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001B119 File Offset: 0x00019319
	public void PauseAnimator()
	{
		this.animator.speed = 0f;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0001B12B File Offset: 0x0001932B
	public void FadeHelicopterSound()
	{
		if (this.helicopterAudioSource)
		{
			this.helicopterAudioSource.DOFade(0f, 0.5f);
		}
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0001B150 File Offset: 0x00019350
	public void TriggerHelicopter()
	{
		this.helicopter.SetTrigger("FlyDown");
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0001B162 File Offset: 0x00019362
	public void TriggerHelicopterFX()
	{
		this.helicopterPS1.Play();
		this.helicopterPS2.Play();
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001B17A File Offset: 0x0001937A
	public void StopHelicopterFX()
	{
		this.helicopterPS1.Stop();
		this.helicopterPS2.Stop();
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0001B194 File Offset: 0x00019394
	public void CamLogic(int priority)
	{
		if (this.cam != null)
		{
			if (priority > 0)
			{
				BetterPlayerControl.Instance.ChangePlayerState(this.playerStateWhileAnimating);
			}
			else
			{
				BetterPlayerControl.Instance.ChangePlayerState(this.playerStateBeforeAnimating);
			}
			CinemachineBlendDefinition cinemachineBlendDefinition = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f);
			CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this.cam);
			if (cinemachineBrain != null)
			{
				cinemachineBrain.m_DefaultBlend = cinemachineBlendDefinition;
			}
			this.cam.Priority = priority;
			return;
		}
	}

	// Token: 0x0400046D RID: 1133
	public static FrontDoor Instance;

	// Token: 0x0400046E RID: 1134
	public bool inNarratorConversation;

	// Token: 0x0400046F RID: 1135
	public bool locked = true;

	// Token: 0x04000470 RID: 1136
	public CinemachineVirtualCamera cam;

	// Token: 0x04000471 RID: 1137
	public Animator animator;

	// Token: 0x04000472 RID: 1138
	public AnimatorOverrideController helicopterController;

	// Token: 0x04000473 RID: 1139
	[SerializeField]
	[Tooltip("Some objects will steal control from the player.")]
	private BetterPlayerControl.PlayerState playerStateWhileAnimating;

	// Token: 0x04000474 RID: 1140
	private BetterPlayerControl.PlayerState playerStateBeforeAnimating;

	// Token: 0x04000475 RID: 1141
	public UnityEvent InteractEnded;

	// Token: 0x04000476 RID: 1142
	public AudioClip lockedSound;

	// Token: 0x04000477 RID: 1143
	public AudioClip openSound;

	// Token: 0x04000478 RID: 1144
	public AudioClip openSoundHelicopter;

	// Token: 0x04000479 RID: 1145
	public AudioClip loveClipOverride;

	// Token: 0x0400047A RID: 1146
	public Animator helicopter;

	// Token: 0x0400047B RID: 1147
	public ParticleSystem helicopterPS1;

	// Token: 0x0400047C RID: 1148
	public ParticleSystem helicopterPS2;

	// Token: 0x0400047D RID: 1149
	private AudioSource helicopterAudioSource;

	// Token: 0x0400047E RID: 1150
	private bool usingHelicopter;

	// Token: 0x0400047F RID: 1151
	[SerializeField]
	public InteractableObj interactableObj;
}
