using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class LoopedSound : MonoBehaviour
{
	// Token: 0x0600055A RID: 1370 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
	public void PlaySound()
	{
		Singleton<AudioManager>.Instance.StopTrack(this.loopedSound.name, 0f);
		Singleton<AudioManager>.Instance.PlayTrack(this.loopedSound, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
	}

	// Token: 0x0400053B RID: 1339
	[SerializeField]
	private AudioClip loopedSound;
}
