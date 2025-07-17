using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
[Serializable]
public class Stat
{
	// Token: 0x060006B3 RID: 1715 RVA: 0x00027500 File Offset: 0x00025700
	public Stat(string name, string description, Sprite image)
	{
		this.Name = name;
		this.Description = description;
		this.Image = image;
	}

	// Token: 0x040005FB RID: 1531
	public string Name;

	// Token: 0x040005FC RID: 1532
	public string Description;

	// Token: 0x040005FD RID: 1533
	public Sprite Image;
}
