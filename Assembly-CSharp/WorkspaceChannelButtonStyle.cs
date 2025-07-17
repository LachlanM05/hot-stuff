using System;
using TMPro;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class WorkspaceChannelButtonStyle : MonoBehaviour
{
	// Token: 0x06000CC7 RID: 3271 RVA: 0x0004A3EE File Offset: 0x000485EE
	public void ToggleSelected(bool isSelected)
	{
		this.backing.SetActive(isSelected);
		this.text.color = (isSelected ? this.textColorOn : this.textColorOff);
	}

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private GameObject backing;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private Color textColorOn;

	// Token: 0x04000B78 RID: 2936
	[SerializeField]
	private Color textColorOff;
}
