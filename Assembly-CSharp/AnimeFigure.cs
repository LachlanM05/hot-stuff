using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class AnimeFigure : MonoBehaviour
{
	// Token: 0x060000AC RID: 172 RVA: 0x000046C8 File Offset: 0x000028C8
	private void Start()
	{
		this._rdr = base.GetComponent<Renderer>();
		this._cld = base.GetComponent<Collider>();
		Singleton<InkController>.Instance.story.ObserveVariable("sam_hime_gift", delegate(string varName, object newValue)
		{
			this.SetVisibility((bool)newValue);
		});
		this.SetVisibility(false);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00004714 File Offset: 0x00002914
	public void SetVisibility(bool vis)
	{
		this._rdr.enabled = vis;
		this._cld.enabled = vis;
	}

	// Token: 0x040000A8 RID: 168
	private Renderer _rdr;

	// Token: 0x040000A9 RID: 169
	private Collider _cld;
}
