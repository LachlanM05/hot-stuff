using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000154 RID: 340
public class DesktopTabButton : MonoBehaviour
{
	// Token: 0x06000CA6 RID: 3238 RVA: 0x00049158 File Offset: 0x00047358
	public void ToggleState(bool isOn)
	{
		foreach (Graphic graphic in this.primaryElements)
		{
			graphic.color = (isOn ? this.styleOn.primaryColor : this.styleOff.primaryColor);
		}
		foreach (Graphic graphic2 in this.secondaryElements)
		{
			graphic2.color = (isOn ? this.styleOn.secondaryColor : this.styleOff.secondaryColor);
		}
	}

	// Token: 0x04000B5F RID: 2911
	[SerializeField]
	private DesktopTabButton.TabStyle styleOn;

	// Token: 0x04000B60 RID: 2912
	[SerializeField]
	private DesktopTabButton.TabStyle styleOff;

	// Token: 0x04000B61 RID: 2913
	[SerializeField]
	private List<Graphic> primaryElements;

	// Token: 0x04000B62 RID: 2914
	[SerializeField]
	private List<Graphic> secondaryElements;

	// Token: 0x0200035D RID: 861
	[Serializable]
	public class TabStyle
	{
		// Token: 0x04001344 RID: 4932
		public Color primaryColor;

		// Token: 0x04001345 RID: 4933
		public Color secondaryColor;
	}
}
