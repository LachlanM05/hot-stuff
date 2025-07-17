using System;

// Token: 0x020000A8 RID: 168
public interface IReloadHandler
{
	// Token: 0x06000575 RID: 1397
	int Priority();

	// Token: 0x06000576 RID: 1398
	void LoadState();

	// Token: 0x06000577 RID: 1399
	void SaveState();
}
