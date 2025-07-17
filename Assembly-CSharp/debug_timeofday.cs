using System;

// Token: 0x02000047 RID: 71
public class debug_timeofday : Interactable
{
	// Token: 0x060001D4 RID: 468 RVA: 0x0000B82A File Offset: 0x00009A2A
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000B831 File Offset: 0x00009A31
	public override void Interact()
	{
		Singleton<Dateviators>.Instance.ConsumeCharge();
		Singleton<DayNightCycle>.Instance.IncrementTime();
	}
}
