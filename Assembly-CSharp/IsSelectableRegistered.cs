using System;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class IsSelectableRegistered : MonoBehaviour
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0004F3E5 File Offset: 0x0004D5E5
	public bool IsRegistered
	{
		get
		{
			return this._isRegistered;
		}
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x0004F3ED File Offset: 0x0004D5ED
	private void Start()
	{
		this._isRegistered = true;
	}

	// Token: 0x04000CB8 RID: 3256
	private bool _isRegistered;
}
