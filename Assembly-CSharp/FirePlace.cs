using System;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class FirePlace : MonoBehaviour
{
	// Token: 0x06000467 RID: 1127 RVA: 0x0001AC7E File Offset: 0x00018E7E
	private void Start()
	{
		this.turnedOn = false;
		this.FireLight.SetActive(false);
		this.FireVfx.SetActive(false);
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.sfx_fireplace.name, 0f);
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0001ACC0 File Offset: 0x00018EC0
	public void Interact()
	{
		if (Singleton<Dateviators>.Instance.Equipped)
		{
			if (this.turnedOn)
			{
				this.turnedOn = false;
				this.FireLight.SetActive(false);
				this.FireVfx.SetActive(false);
				this.FireAudio.Stop();
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.sfx_fireplace.name, 0f);
				return;
			}
			this.turnedOn = true;
			this.FireLight.SetActive(true);
			this.FireVfx.SetActive(true);
			this.FireAudio.Play();
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.sfx_fireplace, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
	}

	// Token: 0x04000466 RID: 1126
	public GameObject FireLight;

	// Token: 0x04000467 RID: 1127
	public GameObject FireVfx;

	// Token: 0x04000468 RID: 1128
	public AudioSource FireAudio;

	// Token: 0x04000469 RID: 1129
	private bool turnedOn;
}
