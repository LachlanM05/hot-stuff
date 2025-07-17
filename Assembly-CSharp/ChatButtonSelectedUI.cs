using System;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class ChatButtonSelectedUI : MonoBehaviour
{
	// Token: 0x06000C65 RID: 3173 RVA: 0x00046F6C File Offset: 0x0004516C
	public void OnSelected(bool select)
	{
		this._selectedUI.SetActive(select);
	}

	// Token: 0x04000B1A RID: 2842
	[SerializeField]
	private GameObject _selectedUI;
}
