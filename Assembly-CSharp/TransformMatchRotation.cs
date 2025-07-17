using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[ExecuteAlways]
public class TransformMatchRotation : MonoBehaviour
{
	// Token: 0x060000A3 RID: 163 RVA: 0x00004538 File Offset: 0x00002738
	private void LateUpdate()
	{
		if (this.transformToMatch == null)
		{
			return;
		}
		base.transform.rotation = this.transformToMatch.rotation;
	}

	// Token: 0x040000A1 RID: 161
	[SerializeField]
	private Transform transformToMatch;
}
