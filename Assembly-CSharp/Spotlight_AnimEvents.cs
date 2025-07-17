using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D8 RID: 216
public class Spotlight_AnimEvents : MonoBehaviour
{
	// Token: 0x06000711 RID: 1809 RVA: 0x00027DB1 File Offset: 0x00025FB1
	private void Awake()
	{
		UnityEvent dialogueExit = Singleton<GameController>.Instance.DialogueExit;
		if (dialogueExit == null)
		{
			return;
		}
		dialogueExit.AddListener(new UnityAction(this.EndSpotlight));
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00027DD3 File Offset: 0x00025FD3
	public void EndSpotlight()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00027DE1 File Offset: 0x00025FE1
	private void OnDisable()
	{
		this.EndSpotlight();
	}
}
