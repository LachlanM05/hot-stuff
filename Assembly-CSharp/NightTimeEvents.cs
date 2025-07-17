using System;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class NightTimeEvents : MonoBehaviour, IReloadHandler
{
	// Token: 0x060005BE RID: 1470 RVA: 0x00020D2C File Offset: 0x0001EF2C
	public void Start()
	{
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00020D30 File Offset: 0x0001EF30
	public void nighttimeevents(int day)
	{
		if (this.eveningdialogues.Length <= day)
		{
			return;
		}
		if (this.eveningdialogues[day].Length != 0)
		{
			string text = this.eveningdialogues[day];
			if (text.ToLowerInvariant().Contains("canopy"))
			{
				Singleton<ChatMaster>.Instance.StartChat(text, ChatType.Canopy, false, true);
			}
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00020D81 File Offset: 0x0001EF81
	public int Priority()
	{
		return 100;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00020D85 File Offset: 0x0001EF85
	public void LoadState()
	{
		DayNightCycle._nightevents += this.nighttimeevents;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00020D98 File Offset: 0x0001EF98
	public void SaveState()
	{
	}

	// Token: 0x04000579 RID: 1401
	public string[] eveningdialogues;
}
