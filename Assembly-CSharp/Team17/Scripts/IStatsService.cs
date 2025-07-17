using System;
using Team17.Common;

namespace Team17.Scripts
{
	// Token: 0x020001EE RID: 494
	public interface IStatsService : IService
	{
		// Token: 0x06001079 RID: 4217
		void OnUnlockedCollectable(string internalName, int collectableNumber);

		// Token: 0x0600107A RID: 4218
		void LoadAndProcessStatsData();

		// Token: 0x0600107B RID: 4219
		void CreateNewStatsData();
	}
}
