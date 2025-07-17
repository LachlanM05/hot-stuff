using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class SkeletonKey : MonoBehaviour
{
	// Token: 0x060004FE RID: 1278 RVA: 0x0001DF4D File Offset: 0x0001C14D
	public void MagicalEnded()
	{
		MovingDateable.MoveDateable("MovingKeyBox", "pocket", true);
		Save.AutoSaveGame();
	}
}
