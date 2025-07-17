using System;
using System.IO;
using T17.Services;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class VoiceOverCharacter : MonoBehaviour
{
	// Token: 0x06000C56 RID: 3158 RVA: 0x00046A0D File Offset: 0x00044C0D
	public void Awake()
	{
		this.audioSource = base.gameObject.GetComponent<AudioSource>();
		if (this.trigger == VoiceOverCharacter.TriggerVoice.WaitingForFinish)
		{
			this.trigger = VoiceOverCharacter.TriggerVoice.Manually;
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00046A30 File Offset: 0x00044C30
	private void OnDisable()
	{
		if (this.audioSource.isPlaying)
		{
			this.audioSource.Stop();
			this.audioSource.clip = null;
			this.UnloadAudio();
		}
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00046A5C File Offset: 0x00044C5C
	public void Update()
	{
		switch (this.trigger)
		{
		case VoiceOverCharacter.TriggerVoice.Manually:
			break;
		case VoiceOverCharacter.TriggerVoice.Auto:
			if (this.delay > 0f)
			{
				this.PlayVoiceOver(this.delay);
				return;
			}
			this.PlayVoiceOverNoDelay();
			return;
		case VoiceOverCharacter.TriggerVoice.WaitingForFinish:
			if (!this.audioSource.isPlaying)
			{
				this.UnloadAudio();
				this.trigger = VoiceOverCharacter.TriggerVoice.Manually;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00046AC0 File Offset: 0x00044CC0
	public void PlayVoiceOver(float delay = 0.5f)
	{
		int num = Random.Range(0, 104);
		string text = Path.Combine(new string[]
		{
			"Audio",
			"Sfx",
			"VoiceOver",
			this.folder,
			num.ToString()
		});
		this.loadedAudio = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
		if (this.loadedAudio != null)
		{
			this.audioSource.clip = this.loadedAudio;
			this.audioSource.PlayDelayed(delay);
			this.trigger = VoiceOverCharacter.TriggerVoice.WaitingForFinish;
			return;
		}
		this.trigger = VoiceOverCharacter.TriggerVoice.Manually;
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00046B5C File Offset: 0x00044D5C
	public void PlayVoiceOverNoDelay()
	{
		int num = Random.Range(0, 104);
		string text = Path.Combine(new string[]
		{
			"Audio",
			"Sfx",
			"VoiceOver",
			this.folder,
			num.ToString()
		});
		AudioClip audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
		if (audioClip != null)
		{
			base.gameObject.GetComponent<AudioSource>().clip = audioClip;
			base.gameObject.GetComponent<AudioSource>().Play();
			this.trigger = VoiceOverCharacter.TriggerVoice.WaitingForFinish;
			return;
		}
		this.trigger = VoiceOverCharacter.TriggerVoice.Manually;
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00046BEF File Offset: 0x00044DEF
	private void UnloadAudio()
	{
		if (this.loadedAudio != null)
		{
			Services.AssetProviderService.UnloadResourceAsset(this.loadedAudio);
			this.loadedAudio = null;
		}
	}

	// Token: 0x04000B0B RID: 2827
	public string folder;

	// Token: 0x04000B0C RID: 2828
	public VoiceOverCharacter.TriggerVoice trigger;

	// Token: 0x04000B0D RID: 2829
	public float delay = 0.5f;

	// Token: 0x04000B0E RID: 2830
	private AudioSource audioSource;

	// Token: 0x04000B0F RID: 2831
	private AudioClip loadedAudio;

	// Token: 0x0200035A RID: 858
	public enum TriggerVoice
	{
		// Token: 0x04001338 RID: 4920
		Manually,
		// Token: 0x04001339 RID: 4921
		Auto,
		// Token: 0x0400133A RID: 4922
		WaitingForFinish
	}
}
