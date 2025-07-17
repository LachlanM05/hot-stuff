using System;
using TMPro;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class InitControlItem : MonoBehaviour
{
	// Token: 0x06000955 RID: 2389 RVA: 0x000360B6 File Offset: 0x000342B6
	public void Initialize(string action, string map)
	{
		this.Action.text = action + ":";
		this.Map.text = map;
	}

	// Token: 0x04000890 RID: 2192
	public TextMeshProUGUI Action;

	// Token: 0x04000891 RID: 2193
	public TextMeshProUGUI Map;
}
