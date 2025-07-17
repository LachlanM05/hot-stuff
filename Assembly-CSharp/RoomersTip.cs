using System;
using TMPro;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class RoomersTip : MonoBehaviour
{
	// Token: 0x06000D0D RID: 3341 RVA: 0x0004B462 File Offset: 0x00049662
	private void Start()
	{
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x0004B464 File Offset: 0x00049664
	private void Update()
	{
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x0004B466 File Offset: 0x00049666
	public void SetUpTip(string title, string info)
	{
		this.TipTitle.text = title.ToUpperInvariant();
		this.TipInfo.text = info;
	}

	// Token: 0x04000BD2 RID: 3026
	public TextMeshProUGUI TipTitle;

	// Token: 0x04000BD3 RID: 3027
	public TextMeshProUGUI TipInfo;
}
