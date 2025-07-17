using System;

namespace Team17.Scripts.Services
{
	// Token: 0x0200021A RID: 538
	public class DefaultMaxSaveSlotsProvider : IMaxSaveSlotsProvider
	{
		// Token: 0x0600117E RID: 4478 RVA: 0x000580E2 File Offset: 0x000562E2
		public int GetMaxSaveSlots()
		{
			return 100;
		}
	}
}
