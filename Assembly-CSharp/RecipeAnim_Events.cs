using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class RecipeAnim_Events : MonoBehaviour
{
	// Token: 0x060005E4 RID: 1508 RVA: 0x000216EB File Offset: 0x0001F8EB
	public void Hide()
	{
		Singleton<RecipeAnim>.Instance.stopanim();
	}
}
