using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[ExecuteAlways]
public class TransformMatchScale : MonoBehaviour
{
	// Token: 0x060000A5 RID: 165 RVA: 0x00004567 File Offset: 0x00002767
	private void LateUpdate()
	{
		if (this.transformToMatch == null)
		{
			return;
		}
		base.transform.localScale = this.transformToMatch.localScale;
	}

	// Token: 0x040000A2 RID: 162
	[SerializeField]
	private Transform transformToMatch;
}
