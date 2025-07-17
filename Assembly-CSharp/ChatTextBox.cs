using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E8 RID: 232
public class ChatTextBox : MonoBehaviour
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060007AD RID: 1965 RVA: 0x0002B10F File Offset: 0x0002930F
	// (set) Token: 0x060007AC RID: 1964 RVA: 0x0002B106 File Offset: 0x00029306
	public string InkFlowName { get; private set; }

	// Token: 0x060007AE RID: 1966 RVA: 0x0002B118 File Offset: 0x00029318
	public void Init(string text, string inkFlowName, Sprite reaction = null)
	{
		this.Dialogue.text = text;
		this.InkFlowName = inkFlowName;
		if (this.Reaction != null)
		{
			if (reaction != null)
			{
				this.Reaction.gameObject.SetActive(true);
				this.Reaction.sprite = reaction;
				return;
			}
			this.Reaction.gameObject.SetActive(false);
		}
	}

	// Token: 0x040006E4 RID: 1764
	public Image textbox;

	// Token: 0x040006E5 RID: 1765
	public Sprite yousent;

	// Token: 0x040006E6 RID: 1766
	public Sprite theysent;

	// Token: 0x040006E7 RID: 1767
	public TextMeshProUGUI Dialogue;

	// Token: 0x040006E8 RID: 1768
	public Image Reaction;
}
