using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000164 RID: 356
public class EmoteAnimation : MonoBehaviour
{
	// Token: 0x06000CEE RID: 3310 RVA: 0x0004AB86 File Offset: 0x00048D86
	private void Start()
	{
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0004AB88 File Offset: 0x00048D88
	private void Update()
	{
		this.imageRenderer.sprite = this.spriteRenderer.sprite;
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0004ABA0 File Offset: 0x00048DA0
	public void EndEmote()
	{
		if (this.speaker == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		string character = this.speaker.character;
		Singleton<GameController>.Instance.HideEmotes(character);
	}

	// Token: 0x04000BB3 RID: 2995
	[SerializeField]
	private UISpeaker speaker;

	// Token: 0x04000BB4 RID: 2996
	public SpriteRenderer spriteRenderer;

	// Token: 0x04000BB5 RID: 2997
	public Image imageRenderer;
}
