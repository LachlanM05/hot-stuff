using System;
using T17.Services;

namespace Team17.Scripts.Services.Input
{
	// Token: 0x0200021D RID: 541
	public class InputModeHandle : IDisposable
	{
		// Token: 0x0600118B RID: 4491 RVA: 0x000580EE File Offset: 0x000562EE
		public void SafeDispose()
		{
			if (Services.InputService.IsValidHandle(this))
			{
				this.Dispose();
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00058103 File Offset: 0x00056303
		public void Dispose()
		{
			Services.InputService.PopHandle(this);
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00058110 File Offset: 0x00056310
		internal InputModeHandle()
		{
		}
	}
}
