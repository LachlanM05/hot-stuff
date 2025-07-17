using System;
using TMPro;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class BuildVersion : MonoBehaviour
{
	// Token: 0x06000786 RID: 1926 RVA: 0x0002A5DC File Offset: 0x000287DC
	private void Awake()
	{
		TextMeshProUGUI textMeshProUGUI;
		if (base.TryGetComponent<TextMeshProUGUI>(out textMeshProUGUI))
		{
			textMeshProUGUI.text = Application.version;
		}
	}
}
