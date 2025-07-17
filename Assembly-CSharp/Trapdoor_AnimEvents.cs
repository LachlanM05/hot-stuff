using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class Trapdoor_AnimEvents : MonoBehaviour
{
	// Token: 0x06000537 RID: 1335 RVA: 0x0001EBDF File Offset: 0x0001CDDF
	public void Opened()
	{
		this.ladder.stateLock = false;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0001EBED File Offset: 0x0001CDED
	public void Closed()
	{
		this.ladder.stateLock = true;
	}

	// Token: 0x04000521 RID: 1313
	[SerializeField]
	private GenericInteractable ladder;
}
