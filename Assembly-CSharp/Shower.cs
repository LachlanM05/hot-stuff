using System;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x02000091 RID: 145
public class Shower : MonoBehaviour
{
	// Token: 0x060004F9 RID: 1273 RVA: 0x0001DE7B File Offset: 0x0001C07B
	private void Awake()
	{
		this.Deactivate();
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0001DE83 File Offset: 0x0001C083
	public void Toggle()
	{
		if (this.showerWater.HasAnySystemAwake())
		{
			this.Deactivate();
			return;
		}
		this.Activate();
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001DEA0 File Offset: 0x0001C0A0
	public void Activate()
	{
		if (this.isActivated)
		{
			this.Deactivate();
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(this.interactable.standardSfx_loop[0], AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, true, SFX_SUBGROUP.FOLEY, false);
		this.showerWater.Play();
		this.interactable.interruptable = true;
		this.isActivated = true;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0001DF0C File Offset: 0x0001C10C
	public void Deactivate()
	{
		Singleton<AudioManager>.Instance.StopTrack(this.interactable.standardSfx_loop[0].name, 0f);
		this.showerWater.Stop();
		this.isActivated = false;
	}

	// Token: 0x040004EC RID: 1260
	[SerializeField]
	private VisualEffect showerWater;

	// Token: 0x040004ED RID: 1261
	[SerializeField]
	private GenericInteractable interactable;

	// Token: 0x040004EE RID: 1262
	private bool isActivated;
}
