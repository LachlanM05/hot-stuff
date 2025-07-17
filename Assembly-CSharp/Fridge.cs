using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class Fridge : MonoBehaviour
{
	// Token: 0x06000372 RID: 882 RVA: 0x000163B3 File Offset: 0x000145B3
	private void Start()
	{
	}

	// Token: 0x06000373 RID: 883 RVA: 0x000163B5 File Offset: 0x000145B5
	public void BeginHowl()
	{
		this.rightDoor.stateLock = true;
		this.leftDoor.stateLock = true;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x000163CF File Offset: 0x000145CF
	public void EndHowl()
	{
		this.rightDoor.stateLock = false;
		this.leftDoor.stateLock = false;
	}

	// Token: 0x0400036F RID: 879
	[SerializeField]
	private Door leftDoor;

	// Token: 0x04000370 RID: 880
	[SerializeField]
	private Door rightDoor;
}
