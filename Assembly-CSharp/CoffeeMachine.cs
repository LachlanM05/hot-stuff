using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x02000066 RID: 102
public class CoffeeMachine : MonoBehaviour
{
	// Token: 0x06000362 RID: 866 RVA: 0x00016256 File Offset: 0x00014456
	private void Awake()
	{
		CoffeeMachine.Instance = this;
		this.pour.Stop();
		this.drain.Stop();
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00016274 File Offset: 0x00014474
	public void StartPour()
	{
		this.pour.Play();
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00016281 File Offset: 0x00014481
	public void StopPour()
	{
		this.pour.Stop();
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001628E File Offset: 0x0001448E
	public void Fill()
	{
		this.drain.Play();
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001629B File Offset: 0x0001449B
	public void DoneFill()
	{
		this.drain.Stop();
	}

	// Token: 0x06000367 RID: 871 RVA: 0x000162A8 File Offset: 0x000144A8
	public void TriggerDate()
	{
		base.StartCoroutine(this.Date());
	}

	// Token: 0x06000368 RID: 872 RVA: 0x000162B7 File Offset: 0x000144B7
	private IEnumerator Date()
	{
		yield return new WaitUntil(() => BetterPlayerControl.Instance.STATE == BetterPlayerControl.PlayerState.CanControl);
		this.genInt.MagicalInteract(true);
		yield return new WaitForSeconds(this.coffeeAnimationDuration);
		this.genInt.EndOfAnimationChecks();
		yield break;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x000162C6 File Offset: 0x000144C6
	public void CoffeeMidnight()
	{
		if (Singleton<DayNightCycle>.Instance.GetTime() == DayPhase.MIDNIGHT)
		{
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.STATE_DRANK_COFFEE);
		}
	}

	// Token: 0x04000364 RID: 868
	public static CoffeeMachine Instance;

	// Token: 0x04000365 RID: 869
	[SerializeField]
	private GenericInteractable genInt;

	// Token: 0x04000366 RID: 870
	[SerializeField]
	private VisualEffect pour;

	// Token: 0x04000367 RID: 871
	[SerializeField]
	private VisualEffect drain;

	// Token: 0x04000368 RID: 872
	[SerializeField]
	private float coffeeAnimationDuration;
}
