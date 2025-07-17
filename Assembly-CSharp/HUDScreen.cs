using System;
using Rewired;

// Token: 0x0200010F RID: 271
public class HUDScreen : MenuComponent
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x00036098 File Offset: 0x00034298
	public override bool NeedsInput
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0003609B File Offset: 0x0003429B
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
	}

	// Token: 0x0400088F RID: 2191
	private Player player;
}
