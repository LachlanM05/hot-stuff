using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000142 RID: 322
public class TimebarTimeIcon : MonoBehaviour, IReloadHandler
{
	// Token: 0x06000BD3 RID: 3027 RVA: 0x000445AE File Offset: 0x000427AE
	public void Start()
	{
		Singleton<GameController>.Instance.UpdateHUDForTimeChange.AddListener(new UnityAction(this.UpdateCurrentDayPhase));
		this.UpdateDayPhaseIcon();
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x000445D1 File Offset: 0x000427D1
	private void OnEnable()
	{
		this.UpdateDayPhaseIcon();
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x000445D9 File Offset: 0x000427D9
	public void UpdateCurrentDayPhase()
	{
		base.Invoke("SetCurrentDayPhaseSpritesForAnimation2", 4f);
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x000445EC File Offset: 0x000427EC
	private void SetCurrentDayPhaseSpritesForAnimation2()
	{
		this.DayPhaseIcon.sprite = Singleton<DayNightCycle>.Instance.GetPreviousDayPhaseSprite();
		switch (Singleton<DayNightCycle>.Instance.CurrentDayPhase)
		{
		case DayPhase.MORNING:
			this.dayPhaseAnimation.Play("T6");
			break;
		case DayPhase.NOON:
			this.dayPhaseAnimation.Play("T1");
			break;
		case DayPhase.AFTERNOON:
			this.dayPhaseAnimation.Play("T2");
			break;
		case DayPhase.EVENING:
			this.dayPhaseAnimation.Play("T3");
			break;
		case DayPhase.NIGHT:
			this.dayPhaseAnimation.Play("T4");
			break;
		case DayPhase.MIDNIGHT:
			this.dayPhaseAnimation.Play("T5");
			break;
		}
		this.SaveState();
		this.initialized = true;
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x000446B0 File Offset: 0x000428B0
	private void UpdateDayPhaseIcon()
	{
		this.DayPhaseIcon.sprite = Singleton<DayNightCycle>.Instance.GetDayPhaseSprite(null);
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x000446C8 File Offset: 0x000428C8
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x000446CF File Offset: 0x000428CF
	public void LoadState()
	{
		this.UpdateCurrentDayPhase();
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x000446D7 File Offset: 0x000428D7
	public void SaveState()
	{
	}

	// Token: 0x04000A8A RID: 2698
	public Image DayPhaseIcon;

	// Token: 0x04000A8B RID: 2699
	public Animator dayPhaseAnimation;

	// Token: 0x04000A8C RID: 2700
	private bool initialized;
}
