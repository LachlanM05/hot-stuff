using System;

namespace CowberryStudios.ProjectAI
{
	// Token: 0x0200025F RID: 607
	public static class Util
	{
		// Token: 0x060013C1 RID: 5057 RVA: 0x0005F4F0 File Offset: 0x0005D6F0
		public static uint FNVHash(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return 0U;
			}
			uint num = 2166136261U;
			for (int i = 0; i < input.Length; i++)
			{
				num ^= (uint)input[i];
				num *= 16777619U;
			}
			return num;
		}
	}
}
