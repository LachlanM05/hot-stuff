using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class BreakerBox : MonoBehaviour
{
	// Token: 0x060003E3 RID: 995 RVA: 0x00018660 File Offset: 0x00016860
	public void PlaySparksSFX()
	{
		Singleton<AudioManager>.Instance.PlayTrack(this.sparks[Random.Range(0, this.sparks.Length)], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
	}

	// Token: 0x040003D7 RID: 983
	[SerializeField]
	private AudioClip[] sparks;
}
