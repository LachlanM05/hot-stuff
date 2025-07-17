using System;
using System.Collections.Generic;

// Token: 0x0200008F RID: 143
public class ObjectMover : SubInteractable
{
	// Token: 0x060004EF RID: 1263 RVA: 0x0001D9B3 File Offset: 0x0001BBB3
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0001D9BA File Offset: 0x0001BBBA
	public override void Interact()
	{
		base.Invoke("UpdatePosition", this.WaitInSeconds);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0001D9D0 File Offset: 0x0001BBD0
	private void UpdatePosition()
	{
		foreach (MovingObject movingObject in this.MovingDateables)
		{
			if (movingObject != null)
			{
				movingObject.Object.GetComponent<MovingDateable>().MoveDateable(movingObject.Key, false);
			}
		}
	}

	// Token: 0x040004E7 RID: 1255
	public List<MovingObject> MovingDateables;

	// Token: 0x040004E8 RID: 1256
	public float WaitInSeconds;
}
