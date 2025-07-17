using System;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x02000097 RID: 151
public class Toilet : MonoBehaviour
{
	// Token: 0x06000531 RID: 1329 RVA: 0x0001EB00 File Offset: 0x0001CD00
	public void Start()
	{
		this.vfx.Stop();
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0001EB10 File Offset: 0x0001CD10
	public void TryActivateToilet()
	{
		if (this.vfx.HasAnySystemAwake())
		{
			return;
		}
		if (this.interactable.stateLock)
		{
			return;
		}
		this.vfx_object.SetActive(true);
		this.vfx.Play();
		if (this.flushClip != null)
		{
			Singleton<AudioManager>.Instance.StopTrack(this.flushClip.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(this.flushClip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
	}

	// Token: 0x0400051B RID: 1307
	[SerializeField]
	private GameObject vfx_object;

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	private VisualEffect vfx;

	// Token: 0x0400051D RID: 1309
	[SerializeField]
	private GenericInteractable interactable;

	// Token: 0x0400051E RID: 1310
	[SerializeField]
	private AudioClip flushClip;
}
