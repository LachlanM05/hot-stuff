using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class DustBunny : MonoBehaviour
{
	// Token: 0x060001F6 RID: 502 RVA: 0x0000BF3B File Offset: 0x0000A13B
	private void Start()
	{
		this._rdr = base.GetComponent<Renderer>();
		this._cld = base.GetComponent<Collider>();
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000BF55 File Offset: 0x0000A155
	public void SetVisibility(bool vis)
	{
		this._rdr.enabled = vis;
		this._cld.enabled = vis;
	}

	// Token: 0x040002DF RID: 735
	private Renderer _rdr;

	// Token: 0x040002E0 RID: 736
	private Collider _cld;
}
