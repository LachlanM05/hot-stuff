using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000141 RID: 321
public class Timebar : MonoBehaviour, IReloadHandler
{
	// Token: 0x06000BCA RID: 3018 RVA: 0x00044486 File Offset: 0x00042686
	public void Start()
	{
		Singleton<GameController>.Instance.UpdateHUD.AddListener(new UnityAction(this.UpdateTimebar));
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.AddListener(new UnityAction(this.UpdateCurrentDayPhase));
		this.UpdateTimebar();
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x000444C4 File Offset: 0x000426C4
	public void UpdateCurrentDayPhase()
	{
		base.Invoke("SetCurrentDayPhaseSpritesForAnimation2", 4f);
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x000444D8 File Offset: 0x000426D8
	private void SetCurrentDayPhaseSpritesForAnimation2()
	{
		switch (Singleton<DayNightCycle>.Instance.CurrentDayPhase)
		{
		case DayPhase.MORNING:
			this.animatorState = "NewState";
			break;
		case DayPhase.AFTERNOON:
			this.animatorState = "T3";
			break;
		case DayPhase.EVENING:
			this.animatorState = "T4";
			break;
		case DayPhase.NIGHT:
			this.animatorState = "T5";
			break;
		case DayPhase.MIDNIGHT:
			this.animatorState = "T6";
			break;
		}
		this.SaveState();
		this.initialized = true;
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x0004455C File Offset: 0x0004275C
	public void UpdateTimebar()
	{
		this.DayNameShort.text = Singleton<DayNightCycle>.Instance.GetShortDayOfWeek().ToUpperInvariant();
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00044578 File Offset: 0x00042778
	private void SetCurrentDayPhaseSprites()
	{
		this.hudAnimations.SetTrigger("NewDay2");
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x0004458A File Offset: 0x0004278A
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00044591 File Offset: 0x00042791
	public void LoadState()
	{
		this.UpdateCurrentDayPhase();
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00044599 File Offset: 0x00042799
	public void SaveState()
	{
	}

	// Token: 0x04000A86 RID: 2694
	public TextMeshProUGUI DayNameShort;

	// Token: 0x04000A87 RID: 2695
	private bool initialized;

	// Token: 0x04000A88 RID: 2696
	public Animator hudAnimations;

	// Token: 0x04000A89 RID: 2697
	[SerializeField]
	private string animatorState = "NewState";
}
