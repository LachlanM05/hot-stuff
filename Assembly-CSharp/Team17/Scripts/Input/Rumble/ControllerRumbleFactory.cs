using System;
using System.Collections.Generic;
using Team17.Input;

namespace Team17.Scripts.Input.Rumble
{
	// Token: 0x0200023E RID: 574
	public class ControllerRumbleFactory : IControllerRumbleFactory
	{
		// Token: 0x06001281 RID: 4737 RVA: 0x00059BC0 File Offset: 0x00057DC0
		public ControllerRumbleFactory(int initialSize = 8)
		{
			for (int i = 0; i < initialSize; i++)
			{
				this._freeControllerRumbles.Enqueue(new ControllerRumble());
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00059BFC File Offset: 0x00057DFC
		public ControllerRumble RequestControllerRumble()
		{
			ControllerRumble controllerRumble;
			if (!this._freeControllerRumbles.TryDequeue(out controllerRumble))
			{
				return new ControllerRumble();
			}
			return controllerRumble;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00059C1F File Offset: 0x00057E1F
		public void ReturnControllerRumble(ControllerRumble rumble)
		{
			rumble.Clear();
			this._freeControllerRumbles.Enqueue(rumble);
		}

		// Token: 0x04000E9A RID: 3738
		private const int kDefaultInitialQueueSize = 8;

		// Token: 0x04000E9B RID: 3739
		private readonly Queue<ControllerRumble> _freeControllerRumbles = new Queue<ControllerRumble>();
	}
}
