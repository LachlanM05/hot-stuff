using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000041 RID: 65
public class DarknessVisibility : MonoBehaviour
{
	// Token: 0x0600015E RID: 350 RVA: 0x00009830 File Offset: 0x00007A30
	private void Start()
	{
		this.rdr = base.gameObject.GetComponent<Renderer>();
		this.cldr = base.gameObject.GetComponent<Collider>();
		if (this.Lights != null)
		{
			this.Lights.LightUpdated.AddListener(new UnityAction(this.ToggleShadow));
			Singleton<DayNightCycle>.Instance.TimeUpdated.AddListener(new UnityAction(this.ToggleShadow));
			this.ToggleShadow();
			return;
		}
		T17Debug.LogError("In DarknessVisibility, Lightswitch is not set up");
	}

	// Token: 0x0600015F RID: 351 RVA: 0x000098B8 File Offset: 0x00007AB8
	private void ToggleShadow()
	{
		bool flag = this.Lights.lightsOn;
		bool flag2 = this.TimeOfDay == Singleton<DayNightCycle>.Instance.CurrentDayPhase;
		if (this.UseTimeOfDay && !flag2)
		{
			flag = false;
		}
		this.rdr.enabled = flag;
		this.cldr.enabled = flag;
	}

	// Token: 0x04000261 RID: 609
	public Lights_Inter Lights;

	// Token: 0x04000262 RID: 610
	private Renderer rdr;

	// Token: 0x04000263 RID: 611
	private Collider cldr;

	// Token: 0x04000264 RID: 612
	public bool UseTimeOfDay;

	// Token: 0x04000265 RID: 613
	public DayPhase TimeOfDay;
}
