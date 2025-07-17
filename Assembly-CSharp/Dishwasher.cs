using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class Dishwasher : MonoBehaviour
{
	// Token: 0x06000412 RID: 1042 RVA: 0x000194C8 File Offset: 0x000176C8
	private int GetRelationshipEnding()
	{
		RelationshipStatus dateStatus = Singleton<Save>.Instance.GetDateStatus(this.inkNode);
		if (dateStatus == RelationshipStatus.Love)
		{
			return 2;
		}
		if (dateStatus != RelationshipStatus.Friend)
		{
			return -1;
		}
		return 3;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x000194F8 File Offset: 0x000176F8
	public void InteractTray()
	{
		if (this.inProgress)
		{
			return;
		}
		base.StopAllCoroutines();
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			base.StartCoroutine(this.Interact("Tray"));
			return;
		}
		base.StartCoroutine(this.Interact("Magical"));
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00019548 File Offset: 0x00017748
	public void InteractDoor()
	{
		if (this.inProgress)
		{
			return;
		}
		base.StopAllCoroutines();
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			base.StartCoroutine(this.Interact("Door"));
			return;
		}
		base.StartCoroutine(this.Interact("Magical"));
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00019595 File Offset: 0x00017795
	public IEnumerator Interact(string source = "")
	{
		this.animator.enabled = true;
		this.inProgress = true;
		this.ResetTrayTrigger();
		this.ResetDoorTrigger();
		if (source == "Tray" && !Singleton<Dateviators>.Instance.Equipped)
		{
			this.animator.SetTrigger("trayTrigger");
		}
		else if (source == "Door" && !Singleton<Dateviators>.Instance.Equipped)
		{
			if (this.doorOpen && this.trayOut)
			{
				this.animator.SetTrigger("animReset");
				yield return new WaitUntil(() => this.animator.IsInTransition(0) || this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
			}
			else if (this.doorOpen && !this.trayOut)
			{
				this.animator.SetTrigger("doorTrigger");
				yield return new WaitUntil(() => this.animator.IsInTransition(0) || this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
			}
			else if (!this.doorOpen)
			{
				this.animator.SetTrigger("doorTrigger");
				yield return new WaitUntil(() => !this.animator.IsInTransition(0) && this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
			}
		}
		else if (Singleton<Dateviators>.Instance.Equipped && (this.GetRelationshipEnding() == 2 || this.GetRelationshipEnding() == 3))
		{
			base.StartCoroutine(this.TransitionToMagical());
			yield break;
		}
		this.PlaySounds(source);
		yield return new WaitUntil(() => this.animator.IsInTransition(0) || this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
		this.inProgress = false;
		yield break;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x000195AB File Offset: 0x000177AB
	private IEnumerator TransitionToMagical()
	{
		this.dishwasherInteractable.stateLock = true;
		this.inProgress = true;
		this.animator.SetTrigger("animReset");
		yield return new WaitUntil(() => !this.trayOut && !this.doorOpen);
		this.ResetTrayTrigger();
		this.ResetDoorTrigger();
		this.animator.SetInteger("ending", this.GetRelationshipEnding());
		yield return new WaitUntil(() => !this.animator.IsInTransition(0));
		if (this.GetRelationshipEnding() == 2)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.magical_1_Sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			Singleton<AudioManager>.Instance.PlayTrack(this.loveClipOverride, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
		}
		else if (this.GetRelationshipEnding() == 3)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.magical_2_Sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
			Singleton<AudioManager>.Instance.PlayTrack(this.friendClipOverride, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
		}
		this.animator.SetTrigger("magicalAnimStart");
		yield return new WaitUntil(() => this.animator.IsInTransition(0) || this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
		this.animator.ResetTrigger("magicalAnimStart");
		this.inProgress = false;
		yield return null;
		yield break;
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x000195BA File Offset: 0x000177BA
	public void SetDoorOpen()
	{
		this.doorOpen = true;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x000195C3 File Offset: 0x000177C3
	public void SetDoorClosed()
	{
		this.doorOpen = false;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x000195CC File Offset: 0x000177CC
	public void SetTrayOut()
	{
		this.trayOut = true;
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x000195D5 File Offset: 0x000177D5
	public void SetTrayIn()
	{
		this.trayOut = false;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x000195E0 File Offset: 0x000177E0
	public void PlayDoorCloseSound()
	{
		Singleton<AudioManager>.Instance.PlayTrack(this.doorClose_Sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00019614 File Offset: 0x00017814
	private void PlaySounds(string source)
	{
		if (source == "Tray")
		{
			if (this.trayOut)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.traysClose_Sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
			}
			else if (!this.trayOut)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.traysOpen_Sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
			}
		}
		if (source == "Door")
		{
			if (this.doorOpen && this.trayOut)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.traysClose_Sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
				return;
			}
			if (!this.doorOpen)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.doorOpen_Sfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.NONE, false);
			}
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00019702 File Offset: 0x00017902
	public void ResetDoorTrigger()
	{
		this.animator.ResetTrigger("doorTrigger");
		this.animator.ResetTrigger("animReset");
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00019724 File Offset: 0x00017924
	public void ResetTrayTrigger()
	{
		this.animator.ResetTrigger("trayTrigger");
		this.animator.ResetTrigger("animReset");
	}

	// Token: 0x04000412 RID: 1042
	[Header("Status")]
	[SerializeField]
	private bool trayOut;

	// Token: 0x04000413 RID: 1043
	[SerializeField]
	private bool doorOpen;

	// Token: 0x04000414 RID: 1044
	[SerializeField]
	private bool inProgress;

	// Token: 0x04000415 RID: 1045
	[SerializeField]
	private string inkNode = "";

	// Token: 0x04000416 RID: 1046
	[Header("References")]
	[SerializeField]
	private GenericInteractable dishwasherInteractable;

	// Token: 0x04000417 RID: 1047
	public AudioClip friendClipOverride;

	// Token: 0x04000418 RID: 1048
	public AudioClip loveClipOverride;

	// Token: 0x04000419 RID: 1049
	[Header("Animation")]
	[SerializeField]
	private Animator animator;

	// Token: 0x0400041A RID: 1050
	[Header("Audio")]
	public AudioClip doorOpen_Sfx;

	// Token: 0x0400041B RID: 1051
	public AudioClip doorClose_Sfx;

	// Token: 0x0400041C RID: 1052
	public AudioClip traysOpen_Sfx;

	// Token: 0x0400041D RID: 1053
	public AudioClip traysClose_Sfx;

	// Token: 0x0400041E RID: 1054
	public AudioClip magical_1_Sfx;

	// Token: 0x0400041F RID: 1055
	public AudioClip magical_2_Sfx;
}
