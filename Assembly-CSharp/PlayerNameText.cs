using System;
using TMPro;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class PlayerNameText : MonoBehaviour
{
	// Token: 0x06000A0F RID: 2575 RVA: 0x0003970C File Offset: 0x0003790C
	private void OnEnable()
	{
		TextMeshProUGUI component = base.GetComponent<TextMeshProUGUI>();
		if (component != null)
		{
			component.text = Singleton<InkController>.Instance.story.variablesState["player_name"].ToString().ToUpperInvariant();
		}
	}
}
