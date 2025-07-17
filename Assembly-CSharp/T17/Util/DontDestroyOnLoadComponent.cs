using System;
using UnityEngine;

namespace T17.Util
{
	// Token: 0x02000247 RID: 583
	public class DontDestroyOnLoadComponent : MonoBehaviour
	{
		// Token: 0x06001311 RID: 4881 RVA: 0x0005B86C File Offset: 0x00059A6C
		private void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
