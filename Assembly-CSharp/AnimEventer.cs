using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E2 RID: 226
public class AnimEventer : MonoBehaviour
{
	// Token: 0x0600077C RID: 1916 RVA: 0x0002A35B File Offset: 0x0002855B
	public void callevent()
	{
		this.EventToCall.Invoke();
	}

	// Token: 0x040006C2 RID: 1730
	public UnityEvent EventToCall;
}
