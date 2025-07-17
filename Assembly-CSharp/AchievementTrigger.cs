using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class AchievementTrigger : MonoBehaviour
{
	// Token: 0x06000040 RID: 64 RVA: 0x0000331F File Offset: 0x0000151F
	private void Start()
	{
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003321 File Offset: 0x00001521
	public void TriggerAchievement()
	{
		Singleton<AchievementController>.Instance.CheckPlaythrough(this.AchievementId);
	}

	// Token: 0x04000066 RID: 102
	public AchievementId AchievementId;
}
