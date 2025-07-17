using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000064 RID: 100
public class ClockManager : MonoBehaviour, IReloadHandler
{
	// Token: 0x06000359 RID: 857 RVA: 0x000160FD File Offset: 0x000142FD
	private void Awake()
	{
		ClockManager.instance = this;
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.UpdateClocks));
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00016120 File Offset: 0x00014320
	public void UpdateClocks()
	{
		DayPhase currentDayPhase = Singleton<DayNightCycle>.Instance.CurrentDayPhase;
		if (currentDayPhase == DayPhase.MORNING && Singleton<DeluxeEditionController>.Instance.IS_EARLY_BIRD_EDITION)
		{
			this.GymClock.MoveDateable("MORNING_EARLY_BIRD", false);
			this.KitchenClock.MoveDateable("MORNING_EARLY_BIRD", false);
			return;
		}
		this.GymClock.MoveDateable(currentDayPhase.ToString(), false);
		this.KitchenClock.MoveDateable(currentDayPhase.ToString(), false);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0001619C File Offset: 0x0001439C
	public int Priority()
	{
		return 1100;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x000161A3 File Offset: 0x000143A3
	public void LoadState()
	{
		this.UpdateClocks();
	}

	// Token: 0x0600035D RID: 861 RVA: 0x000161AB File Offset: 0x000143AB
	public void SaveState()
	{
	}

	// Token: 0x0400035C RID: 860
	public static ClockManager instance;

	// Token: 0x0400035D RID: 861
	public MovingDateable GymClock;

	// Token: 0x0400035E RID: 862
	public MovingDateable KitchenClock;
}
