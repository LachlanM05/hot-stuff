using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CB RID: 203
[CreateAssetMenu(fileName = "SPEC Stat", menuName = "ScriptableObjects/SPECstat", order = 6)]
[Serializable]
public class SpecStats : ScriptableObject
{
	// Token: 0x060006B2 RID: 1714 RVA: 0x000274F1 File Offset: 0x000256F1
	public SpecStats(List<Stat> stats)
	{
		this.Stats = stats;
	}

	// Token: 0x040005FA RID: 1530
	public List<Stat> Stats;
}
