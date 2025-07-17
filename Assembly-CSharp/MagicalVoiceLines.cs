using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x0200008C RID: 140
public class MagicalVoiceLines : MonoBehaviour
{
	// Token: 0x060004DB RID: 1243 RVA: 0x0001D679 File Offset: 0x0001B879
	private void Start()
	{
		if (this.interactable)
		{
			GenericInteractable genericInteractable = this.interactable;
			if (genericInteractable == null)
			{
				return;
			}
			UnityEvent interactStartedMagical = genericInteractable.InteractStartedMagical;
			if (interactStartedMagical == null)
			{
				return;
			}
			interactStartedMagical.AddListener(new UnityAction(this.SetAnimatorEnding));
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0001D6B0 File Offset: 0x0001B8B0
	private void SetAnimatorEnding()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.interactable.animator)
		{
			this.interactable.animator.SetInteger("ending", this.GetRelationshipEnding());
		}
		if (this.interactable.alternateAnimator)
		{
			this.interactable.alternateAnimator.SetInteger("ending", this.GetRelationshipEnding());
		}
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0001D725 File Offset: 0x0001B925
	private int GetRelationshipEnding()
	{
		return (int)Singleton<Save>.Instance.GetDateStatus(this.inkNode);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0001D738 File Offset: 0x0001B938
	public void PlayDialogue()
	{
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		if (this.GetRelationshipEnding() == 3 || this.friendVOOverride)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.friend_VO, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
			return;
		}
		if (this.GetRelationshipEnding() == 2 || this.loveVOOverride)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.love_VO, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
		}
	}

	// Token: 0x040004DA RID: 1242
	[Header("Status")]
	[SerializeField]
	private string inkNode = "";

	// Token: 0x040004DB RID: 1243
	[SerializeField]
	private bool loveVOOverride;

	// Token: 0x040004DC RID: 1244
	[SerializeField]
	private bool friendVOOverride;

	// Token: 0x040004DD RID: 1245
	[FormerlySerializedAs("magical_1_Sfx")]
	[Header("Audio")]
	public AudioClip friend_VO;

	// Token: 0x040004DE RID: 1246
	[FormerlySerializedAs("magical_2_Sfx")]
	public AudioClip love_VO;

	// Token: 0x040004DF RID: 1247
	[SerializeField]
	private GenericInteractable interactable;
}
