using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class WallaceWall : MonoBehaviour
{
	// Token: 0x0600054C RID: 1356 RVA: 0x0001F23C File Offset: 0x0001D43C
	public void CheckForLines()
	{
		if (!Singleton<Dateviators>.Instance.Equipped)
		{
			return;
		}
		bool flag;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("wallace_true_friend"), out flag);
		if (flag)
		{
			this._interactable.friendClipOverride = this.wallaceExtraFriend;
			this._interactable.loveClipOverride = this.wallaceExtraFriend;
			return;
		}
		this._interactable.friendClipOverride = null;
		this._interactable.loveClipOverride = null;
	}

	// Token: 0x04000530 RID: 1328
	[SerializeField]
	private GenericInteractable _interactable;

	// Token: 0x04000531 RID: 1329
	[SerializeField]
	private AudioClip wallaceExtraFriend;
}
