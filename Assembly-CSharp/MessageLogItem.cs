using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class MessageLogItem : MonoBehaviour
{
	// Token: 0x0600098C RID: 2444 RVA: 0x00037198 File Offset: 0x00035398
	public void Init(MessageType type, string text, [CanBeNull] AudioClip Audio)
	{
		if (type == MessageType.Event)
		{
			Object.Instantiate<GameObject>(this.Header, base.transform).GetComponent<TextMeshProUGUI>().text = text;
			return;
		}
		Object.Instantiate<GameObject>(this.Dialogue, base.transform).GetComponent<TextMeshProUGUI>().text = text;
		if (Audio != null)
		{
			Object.Instantiate<GameObject>(this.SoundClip, base.transform).GetComponent<AudioSource>().clip = Audio;
		}
	}

	// Token: 0x040008D0 RID: 2256
	public GameObject SoundClip;

	// Token: 0x040008D1 RID: 2257
	public GameObject Dialogue;

	// Token: 0x040008D2 RID: 2258
	public GameObject Header;
}
