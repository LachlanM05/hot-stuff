using System;
using System.Collections;
using System.IO;
using T17.Services;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200004E RID: 78
public class EarlyBird : MonoBehaviour
{
	// Token: 0x060001F9 RID: 505 RVA: 0x0000BF77 File Offset: 0x0000A177
	private void Start()
	{
		if (!Singleton<DeluxeEditionController>.Instance.IS_EARLY_BIRD_EDITION)
		{
			this.bird.SetActive(false);
			return;
		}
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.CheckToActivate));
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000BFAD File Offset: 0x0000A1AD
	public void CheckToActivate()
	{
		if (Singleton<DayNightCycle>.Instance.CurrentDayPhase == DayPhase.MORNING && Singleton<DeluxeEditionController>.Instance.IS_EARLY_BIRD_EDITION && !Singleton<DayNightCycle>.Instance.isLoadingGame)
		{
			this.PlayEarlyBirdSequence();
			return;
		}
		this.bird.SetActive(false);
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
	public void PlayEarlyBirdSequence()
	{
		if (!Singleton<DeluxeEditionController>.Instance.IS_EARLY_BIRD_EDITION)
		{
			return;
		}
		base.StopAllCoroutines();
		this.bird.SetActive(true);
		this.animator.ResetTrigger("Start");
		this.animator.Play("waiting");
		this.animator.Update(0f);
		this.animator.SetTrigger("Start");
		base.StartCoroutine(this.EarlyBirdCoroutine());
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000C061 File Offset: 0x0000A261
	private IEnumerator EarlyBirdCoroutine()
	{
		yield return new WaitForSeconds(8f);
		this.Sing();
		yield return new WaitForSeconds(5f);
		this.FlyAway();
		yield return new WaitForSeconds(8f);
		this.Done();
		yield break;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000C070 File Offset: 0x0000A270
	private void Sing()
	{
		string text = Path.Combine("Audio", "Sfx", "EarlyBird", "earlybird_" + Random.Range(0, 97).ToString());
		AudioClip audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
		this.previous = audioClip;
		Singleton<AudioManager>.Instance.PlayTrack(audioClip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, this.bird.gameObject, false, SFX_SUBGROUP.FOLEY, false);
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000C0E7 File Offset: 0x0000A2E7
	private void FlyAway()
	{
		Singleton<AudioManager>.Instance.StopTrack(this.previous.name, 7f);
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000C103 File Offset: 0x0000A303
	private void Done()
	{
		this.animator.ResetTrigger("Start");
		this.animator.Play("waiting");
		this.animator.Update(0f);
		this.bird.SetActive(false);
	}

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private GameObject bird;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private AudioClip previous;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	private Animator animator;

	// Token: 0x040002E4 RID: 740
	public const int EARLY_BIRD_COUNT = 97;
}
