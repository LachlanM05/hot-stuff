using System;
using Team17.Common;

namespace Team17.Scripts.Input.PlatformButtonLayout
{
	// Token: 0x0200023B RID: 571
	public interface IUIInputService : IService
	{
		// Token: 0x0600124A RID: 4682
		bool IsUISubmitDown();

		// Token: 0x0600124B RID: 4683
		bool IsUISubmitUp();

		// Token: 0x0600124C RID: 4684
		bool IsUICancelDown();

		// Token: 0x0600124D RID: 4685
		bool IsUILeftDown();

		// Token: 0x0600124E RID: 4686
		bool IsUIRightDown();

		// Token: 0x0600124F RID: 4687
		bool IsUIDownDown();

		// Token: 0x06001250 RID: 4688
		bool IsUIUpDown();

		// Token: 0x06001251 RID: 4689
		bool IsUILeftPressed();

		// Token: 0x06001252 RID: 4690
		bool IsUIRightPressed();

		// Token: 0x06001253 RID: 4691
		bool IsUIDownPressed();

		// Token: 0x06001254 RID: 4692
		bool IsUIUpPressed();

		// Token: 0x06001255 RID: 4693
		float GetScrollInput();

		// Token: 0x06001256 RID: 4694
		bool IsUICycleDownDown();

		// Token: 0x06001257 RID: 4695
		bool IsUICycleUpDown();

		// Token: 0x06001258 RID: 4696
		bool IsUIMenuBackDown();

		// Token: 0x06001259 RID: 4697
		bool AnyPopupsOpen();

		// Token: 0x0600125A RID: 4698
		bool AllowUIInteraction();

		// Token: 0x0600125B RID: 4699
		bool HasInputControllerChangedRecently();

		// Token: 0x0600125C RID: 4700
		void CycleDown();

		// Token: 0x0600125D RID: 4701
		void CycleUp();
	}
}
