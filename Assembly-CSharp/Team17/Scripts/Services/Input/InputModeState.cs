using System;

namespace Team17.Scripts.Services.Input
{
	// Token: 0x0200021E RID: 542
	public class InputModeState
	{
		// Token: 0x0600118E RID: 4494 RVA: 0x00058118 File Offset: 0x00056318
		public InputModeState(IMirandaInputService.EInputMode requestedMode, InputModeHandle handle, object owner)
		{
			this.RequestedMode = requestedMode;
			this.Handle = handle;
			this.Owner = owner;
		}

		// Token: 0x04000E59 RID: 3673
		public readonly InputModeHandle Handle;

		// Token: 0x04000E5A RID: 3674
		public readonly IMirandaInputService.EInputMode RequestedMode;

		// Token: 0x04000E5B RID: 3675
		public readonly object Owner;
	}
}
