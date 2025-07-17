using System;
using Team17.Common;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class MainMenuAmbience : MonoBehaviour
{
	// Token: 0x06000591 RID: 1425 RVA: 0x00020100 File Offset: 0x0001E300
	public void StartAmbience()
	{
		if (this.ambience == null)
		{
			T17Debug.LogError("No ambience clip assigned to MainMenuAmbience.");
			return;
		}
		this._playingTrackName = this.ambience.name;
		Singleton<AudioManager>.Instance.PlayTrack(this.ambience, AUDIO_TYPE.SFX, false, false, 4f, true, 1f, null, true, SFX_SUBGROUP.AMBIENT, false);
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00020159 File Offset: 0x0001E359
	public void StopAmbience()
	{
		if (string.IsNullOrEmpty(this._playingTrackName))
		{
			return;
		}
		Singleton<AudioManager>.Instance.StopTrack(this._playingTrackName, 3f);
	}

	// Token: 0x04000565 RID: 1381
	[SerializeField]
	private AudioClip ambience;

	// Token: 0x04000566 RID: 1382
	private string _playingTrackName = string.Empty;
}
