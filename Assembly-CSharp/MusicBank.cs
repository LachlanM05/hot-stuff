using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class MusicBank : MonoBehaviour
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x00020ACA File Offset: 0x0001ECCA
	private void Awake()
	{
		MusicBank.Instance = this;
	}

	// Token: 0x04000573 RID: 1395
	public static MusicBank Instance;
}
