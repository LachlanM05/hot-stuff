using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class MessageLogManager : Singleton<MessageLogManager>
{
	// Token: 0x0600098E RID: 2446 RVA: 0x0003720E File Offset: 0x0003540E
	public void Additem(MessageType type, string text, [CanBeNull] AudioClip Audio)
	{
		Object.Instantiate<GameObject>(this.messageItem, this.TransformParent.transform).GetComponent<MessageLogItem>().Init(type, text, Audio);
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00037233 File Offset: 0x00035433
	public void SetActive(bool on)
	{
		if (on)
		{
			this.isactive = true;
			this.LogParent.SetActive(true);
			return;
		}
		this.isactive = false;
		this.LogParent.SetActive(false);
	}

	// Token: 0x040008D3 RID: 2259
	public GameObject messageItem;

	// Token: 0x040008D4 RID: 2260
	public GameObject TransformParent;

	// Token: 0x040008D5 RID: 2261
	public GameObject LogParent;

	// Token: 0x040008D6 RID: 2262
	public bool isactive;
}
