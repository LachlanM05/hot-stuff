using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000050 RID: 80
[Serializable]
public class command
{
	// Token: 0x0600020E RID: 526 RVA: 0x0000C35C File Offset: 0x0000A55C
	public command(Texture2D i)
	{
		this.icon = i;
		this.ueve = new UnityEvent();
	}

	// Token: 0x040002E7 RID: 743
	public Texture2D icon;

	// Token: 0x040002E8 RID: 744
	public UnityEvent ueve;
}
