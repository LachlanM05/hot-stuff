using System;
using System.Collections.Generic;
using Team17.Common;

namespace Team17.Scripts.Services.Input
{
	// Token: 0x0200021C RID: 540
	public interface IMirandaInputService : IService
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001181 RID: 4481
		// (remove) Token: 0x06001182 RID: 4482
		event Action OnStackChanged;

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06001183 RID: 4483
		IReadOnlyCollection<InputModeState> CurrentInputModeStack { get; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06001184 RID: 4484
		int LastControllerId { get; }

		// Token: 0x06001185 RID: 4485
		InputModeHandle PushMode(IMirandaInputService.EInputMode mode, object requester);

		// Token: 0x06001186 RID: 4486
		void PopHandle(InputModeHandle handle);

		// Token: 0x06001187 RID: 4487
		bool IsValidHandle(InputModeHandle inputModeHandle);

		// Token: 0x06001188 RID: 4488
		bool IsLastActiveInputController();

		// Token: 0x06001189 RID: 4489
		bool WasLastControllerAPointer();

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600118A RID: 4490
		IMirandaInputService.EInputMode CurrentMode { get; }

		// Token: 0x04000E58 RID: 3672
		public const int InvalidControllerId = -1;

		// Token: 0x020003BE RID: 958
		public enum EInputMode
		{
			// Token: 0x040014DA RID: 5338
			None,
			// Token: 0x040014DB RID: 5339
			Engagement,
			// Token: 0x040014DC RID: 5340
			UI,
			// Token: 0x040014DD RID: 5341
			UIWithChat,
			// Token: 0x040014DE RID: 5342
			Gameplay
		}
	}
}
