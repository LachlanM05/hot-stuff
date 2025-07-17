using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class IroningBoard : MonoBehaviour
{
	// Token: 0x060004D0 RID: 1232 RVA: 0x0001D137 File Offset: 0x0001B337
	public void Close()
	{
		if (!this.ironDoor.open)
		{
			return;
		}
		this.ironDoor.CloseDoor(1f);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0001D157 File Offset: 0x0001B357
	public void Open()
	{
		if (this.ironDoor.open)
		{
			return;
		}
		this.ironDoor.OpenDoor(this.ironDoor.interactedPosition, 1f);
	}

	// Token: 0x040004CC RID: 1228
	[SerializeField]
	private Door ironDoor;
}
