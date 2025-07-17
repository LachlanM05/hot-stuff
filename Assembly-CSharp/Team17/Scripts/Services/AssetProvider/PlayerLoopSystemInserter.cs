using System;
using System.Text;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Team17.Scripts.Services.AssetProvider
{
	// Token: 0x0200022A RID: 554
	public static class PlayerLoopSystemInserter
	{
		// Token: 0x060011F5 RID: 4597 RVA: 0x00058CB0 File Offset: 0x00056EB0
		public static void InsertAfterAwake(in PlayerLoopSystem system)
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			PlayerLoopSystemInserter.InsertSystem(ref currentPlayerLoop, typeof(Initialization), system, false);
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00058CE4 File Offset: 0x00056EE4
		public static void InsertBeforeUpdate(in PlayerLoopSystem system)
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			PlayerLoopSystemInserter.InsertSystem(ref currentPlayerLoop, typeof(EarlyUpdate), system, true);
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00058D18 File Offset: 0x00056F18
		private static void InsertSystem(ref PlayerLoopSystem playerLoop, Type parentSystem, PlayerLoopSystem newSystem, bool append = true)
		{
			if (playerLoop.subSystemList == null)
			{
				throw new Exception("playerLoop has no subsystem list.");
			}
			int i = 0;
			while (i < playerLoop.subSystemList.Length)
			{
				PlayerLoopSystem playerLoopSystem = playerLoop.subSystemList[i];
				if (playerLoopSystem.type == parentSystem)
				{
					if (playerLoopSystem.subSystemList == null)
					{
						playerLoopSystem.subSystemList = new PlayerLoopSystem[] { newSystem };
						return;
					}
					PlayerLoopSystem[] subSystemList = playerLoopSystem.subSystemList;
					PlayerLoopSystem[] array = new PlayerLoopSystem[subSystemList.Length + 1];
					if (append)
					{
						Array.Copy(subSystemList, array, subSystemList.Length);
						PlayerLoopSystem[] array2 = array;
						array2[array2.Length - 1] = newSystem;
					}
					else
					{
						Array.Copy(subSystemList, 0, array, 1, subSystemList.Length);
						array[0] = newSystem;
					}
					playerLoopSystem.subSystemList = array;
					playerLoop.subSystemList[i] = playerLoopSystem;
					return;
				}
				else
				{
					i++;
				}
			}
			throw new Exception(string.Format("Failed to find parent system: '{0}'", parentSystem));
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00058DF4 File Offset: 0x00056FF4
		public static void RecursivePlayerLoopPrint(PlayerLoopSystem def, StringBuilder sb, int depth)
		{
			if (depth == 0)
			{
				sb.AppendLine("ROOT NODE");
			}
			else if (def.type != null)
			{
				for (int i = 0; i < depth; i++)
				{
					sb.Append("\t");
				}
				sb.AppendLine(def.type.Name);
			}
			if (def.subSystemList != null)
			{
				depth++;
				PlayerLoopSystem[] subSystemList = def.subSystemList;
				for (int j = 0; j < subSystemList.Length; j++)
				{
					PlayerLoopSystemInserter.RecursivePlayerLoopPrint(subSystemList[j], sb, depth);
				}
				depth--;
			}
		}
	}
}
