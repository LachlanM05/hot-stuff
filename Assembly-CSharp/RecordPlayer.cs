using System;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class RecordPlayer : MonoBehaviour
{
	// Token: 0x0600056D RID: 1389 RVA: 0x0001F8BD File Offset: 0x0001DABD
	private int GetRelationshipEnding()
	{
		return (int)Singleton<Save>.Instance.GetDateStatus(this.inkNode);
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0001F8CF File Offset: 0x0001DACF
	public void UpdateEnding()
	{
		this.animator.SetInteger("ending", this.GetRelationshipEnding());
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0001F8E8 File Offset: 0x0001DAE8
	public void PlaySound()
	{
		int relationshipEnding = this.GetRelationshipEnding();
		if (relationshipEnding > 1 && relationshipEnding < 4)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.danceClip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, true, SFX_SUBGROUP.FOLEY, false);
			return;
		}
		if (relationshipEnding <= 1)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.normalClip, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, true, SFX_SUBGROUP.FOLEY, false);
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0001F958 File Offset: 0x0001DB58
	public void StopSound()
	{
		int relationshipEnding = this.GetRelationshipEnding();
		if (relationshipEnding > 1 && relationshipEnding < 4)
		{
			Singleton<AudioManager>.Instance.StopTrack(this.danceClip.name, 0f);
			return;
		}
		if (relationshipEnding <= 1)
		{
			Singleton<AudioManager>.Instance.StopTrack(this.normalClip.name, 0f);
		}
	}

	// Token: 0x04000542 RID: 1346
	[Header("Status")]
	[SerializeField]
	private string inkNode = "";

	// Token: 0x04000543 RID: 1347
	[SerializeField]
	private AudioClip danceClip;

	// Token: 0x04000544 RID: 1348
	[SerializeField]
	private AudioClip normalClip;

	// Token: 0x04000545 RID: 1349
	[SerializeField]
	private Animator animator;
}
