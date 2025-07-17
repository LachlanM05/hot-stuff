using System;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class CloseOpenAnimLogic : MonoBehaviour
{
	// Token: 0x06000CFA RID: 3322 RVA: 0x0004AEC8 File Offset: 0x000490C8
	private void Start()
	{
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x0004AECA File Offset: 0x000490CA
	private void Update()
	{
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x0004AECC File Offset: 0x000490CC
	public void Interact()
	{
		if (Singleton<Dateviators>.Instance.Equipped && (Singleton<Save>.Instance.GetDateStatus(this.dateable) == RelationshipStatus.Love || Singleton<Save>.Instance.GetDateStatus(this.dateable) == RelationshipStatus.Realized))
		{
			if (!this.clean)
			{
				this.ironAnimator.SetBool("magicalAnimMessyStart", true);
			}
			else
			{
				this.ironAnimator.SetBool("magicalAnimStart", true);
			}
			Singleton<AudioManager>.Instance.PlayTrack(this.magicalSfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		if (this.ironAnimator.GetBool("opened"))
		{
			this.ironAnimator.SetBool("opened", false);
		}
		else
		{
			this.ironAnimator.SetBool("opened", true);
		}
		Singleton<AudioManager>.Instance.PlayTrack(this.standardSfx, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x04000BC3 RID: 3011
	public Animator ironAnimator;

	// Token: 0x04000BC4 RID: 3012
	public string standardSfx;

	// Token: 0x04000BC5 RID: 3013
	public string magicalSfx;

	// Token: 0x04000BC6 RID: 3014
	public bool clean;

	// Token: 0x04000BC7 RID: 3015
	public string dateable;
}
