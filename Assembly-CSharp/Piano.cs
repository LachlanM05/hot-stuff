using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class Piano : MonoBehaviour
{
	// Token: 0x0600056A RID: 1386 RVA: 0x0001F890 File Offset: 0x0001DA90
	public void PianoPlaying()
	{
		Singleton<AudioManager>.Instance.PauseTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0001F8A2 File Offset: 0x0001DAA2
	public void PianoStopped()
	{
		Singleton<AudioManager>.Instance.ResumeTrackGroup(AUDIO_TYPE.MUSIC, false, 0.5f);
	}
}
