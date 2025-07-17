using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class DresserWall : MonoBehaviour, IReloadHandler
{
	// Token: 0x0600044C RID: 1100 RVA: 0x0001A652 File Offset: 0x00018852
	public void MakeWallInvisible()
	{
		this.collider.enabled = false;
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0001A660 File Offset: 0x00018860
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0001A668 File Offset: 0x00018868
	public void LoadState()
	{
		bool flag;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("leave_house_hint"), out flag);
		if (flag)
		{
			this.MakeWallInvisible();
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0001A695 File Offset: 0x00018895
	public void SaveState()
	{
	}

	// Token: 0x04000450 RID: 1104
	public BoxCollider collider;
}
