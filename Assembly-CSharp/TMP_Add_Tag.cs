using System;
using TMPro;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class TMP_Add_Tag : MonoBehaviour
{
	// Token: 0x06000BDC RID: 3036 RVA: 0x000446E1 File Offset: 0x000428E1
	private void Start()
	{
		if (this.textArea)
		{
			this.textArea.characterSpacing = this.characterSpacing;
		}
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x00044704 File Offset: 0x00042904
	public void PrependTagToText(string text)
	{
		if (text == "" || this.input == null || this.prependTag == null)
		{
			return;
		}
		if (text.IndexOf(text) >= 0)
		{
			text = text.Replace(this.prependTag, "");
		}
		this.input.SetTextWithoutNotify(this.prependTag + text);
		this.input.stringPosition = this.input.text.Length;
	}

	// Token: 0x04000A8D RID: 2701
	[SerializeField]
	private TMP_InputField input;

	// Token: 0x04000A8E RID: 2702
	[SerializeField]
	private string prependTag;

	// Token: 0x04000A8F RID: 2703
	[SerializeField]
	private TMP_Text textArea;

	// Token: 0x04000A90 RID: 2704
	[SerializeField]
	private float characterSpacing;
}
