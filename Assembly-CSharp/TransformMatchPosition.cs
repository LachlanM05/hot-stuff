using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[ExecuteAlways]
public class TransformMatchPosition : MonoBehaviour
{
	// Token: 0x060000A1 RID: 161 RVA: 0x00004509 File Offset: 0x00002709
	private void LateUpdate()
	{
		if (this.transformToMatch == null)
		{
			return;
		}
		base.transform.position = this.transformToMatch.position;
	}

	// Token: 0x040000A0 RID: 160
	[SerializeField]
	private Transform transformToMatch;
}
