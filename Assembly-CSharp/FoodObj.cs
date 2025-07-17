using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000083 RID: 131
public class FoodObj : MonoBehaviour
{
	// Token: 0x0600046A RID: 1130 RVA: 0x0001AD8A File Offset: 0x00018F8A
	private void Awake()
	{
		this.eatenToday = false;
		Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.Reenable));
		this.Reenable();
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0001ADB4 File Offset: 0x00018FB4
	public void Reenable()
	{
		if (Singleton<DayNightCycle>.Instance.CurrentDayPhase == DayPhase.MORNING)
		{
			this.eatenToday = false;
			this.interactable.stateLock = false;
			this.interactable.blockMagical = false;
			for (int i = 0; i < this.objects.Length; i++)
			{
				this.objects[i].SetActive(true);
			}
		}
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001AE10 File Offset: 0x00019010
	public void Disable()
	{
		this.eatenToday = true;
		this.interactable.stateLock = true;
		this.interactable.blockMagical = true;
		for (int i = 0; i < this.objects.Length; i++)
		{
			this.objects[i].SetActive(false);
		}
	}

	// Token: 0x0400046A RID: 1130
	[SerializeField]
	private GenericInteractable interactable;

	// Token: 0x0400046B RID: 1131
	[SerializeField]
	private GameObject[] objects;

	// Token: 0x0400046C RID: 1132
	[SerializeField]
	public bool eatenToday;
}
