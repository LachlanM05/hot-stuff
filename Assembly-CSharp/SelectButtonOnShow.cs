using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class SelectButtonOnShow : MonoBehaviour
{
	// Token: 0x06000AB9 RID: 2745 RVA: 0x0003D93C File Offset: 0x0003BB3C
	private void OnEnable()
	{
		ControllerMenuUI.SetCurrentlySelected(this.button, ControllerMenuUI.Direction.Down, false, false);
	}

	// Token: 0x040009BB RID: 2491
	[SerializeField]
	private GameObject button;
}
