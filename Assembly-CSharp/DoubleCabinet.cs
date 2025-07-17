using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class DoubleCabinet : MonoBehaviour
{
	// Token: 0x0600043C RID: 1084 RVA: 0x0001A278 File Offset: 0x00018478
	public void OpenDoors()
	{
		if (!this.leftDoor.open)
		{
			this.leftDoor.OpenDoor(this.leftDoor.interactedPosition, 1f);
		}
		if (!this.rightDoor.open)
		{
			this.rightDoor.OpenDoor(this.rightDoor.interactedPosition, 1f);
		}
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x0001A2D5 File Offset: 0x000184D5
	public void CloseDoors()
	{
		if (this.leftDoor.open)
		{
			this.leftDoor.CloseDoor(1f);
		}
		if (this.rightDoor.open)
		{
			this.rightDoor.CloseDoor(1f);
		}
	}

	// Token: 0x04000441 RID: 1089
	[SerializeField]
	private Door leftDoor;

	// Token: 0x04000442 RID: 1090
	[SerializeField]
	private Door rightDoor;
}
