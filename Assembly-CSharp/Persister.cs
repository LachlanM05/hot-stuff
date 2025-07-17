using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class Persister : Singleton<Persister>
{
	// Token: 0x060009AF RID: 2479 RVA: 0x00037B2F File Offset: 0x00035D2F
	public override void AwakeSingleton()
	{
		base.AwakeSingleton();
		Object.DontDestroyOnLoad(this);
	}
}
