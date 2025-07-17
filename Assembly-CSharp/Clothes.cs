using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000065 RID: 101
public class Clothes : MonoBehaviour
{
	// Token: 0x0600035F RID: 863 RVA: 0x000161B5 File Offset: 0x000143B5
	private void Awake()
	{
		Singleton<GameController>.Instance.DialogueExit.AddListener(new UnityAction(this.UpdateClothes));
	}

	// Token: 0x06000360 RID: 864 RVA: 0x000161D4 File Offset: 0x000143D4
	public void UpdateClothes()
	{
		if (this.interactable)
		{
			if (Singleton<InkController>.Instance.GetVariable("clarence_transform") == "dirk")
			{
				this.interactable.loveClipOverride = this.dirkLoveClip;
				this.interactable.friendClipOverride = this.dirkFriendClip;
				return;
			}
			this.interactable.loveClipOverride = this.clarenceLoveClip;
			this.interactable.friendClipOverride = this.clarenceFriendClip;
		}
	}

	// Token: 0x0400035F RID: 863
	[SerializeField]
	private GenericInteractable interactable;

	// Token: 0x04000360 RID: 864
	[SerializeField]
	private AudioClip dirkFriendClip;

	// Token: 0x04000361 RID: 865
	[SerializeField]
	private AudioClip dirkLoveClip;

	// Token: 0x04000362 RID: 866
	[SerializeField]
	private AudioClip clarenceFriendClip;

	// Token: 0x04000363 RID: 867
	[SerializeField]
	private AudioClip clarenceLoveClip;
}
