using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000086 RID: 134
public class GhostController : Interactable, IReloadHandler
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001C930 File Offset: 0x0001AB30
	private bool disappearForever
	{
		get
		{
			return Singleton<Save>.Instance.GetDateStatusRealized(this.ghostObj.InternalName()) == RelationshipStatus.Realized;
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0001C94A File Offset: 0x0001AB4A
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0001C954 File Offset: 0x0001AB54
	private void Start()
	{
		base.gameObject.SetActive(true);
		this.GhostEnable(true);
		this.CheckTime();
		this.CheckForDisable();
		this.ExitHandler();
		if (!this.disappearForever)
		{
			Singleton<DayNightCycle>.Instance.TimeUpdated.AddListener(new UnityAction(this.CheckTime));
			Singleton<DayNightCycle>.Instance.DayPhaseUpdated.AddListener(new UnityAction(this.CheckTime));
			Singleton<DayNightCycle>.Instance.TimeUpdated.AddListener(new UnityAction(this.CheckForDisable));
			Singleton<GameController>.Instance.DialogueExit.AddListener(new UnityAction(this.CheckForDisable));
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0001CA07 File Offset: 0x0001AC07
	private void Update()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.disappearForever)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0001CA2C File Offset: 0x0001AC2C
	public override void Interact()
	{
		if (GhostController.IsSeanceComplete())
		{
			return;
		}
		Singleton<GameController>.Instance.DialogueExit.AddListener(new UnityAction(this.ExitHandler));
		Singleton<GameController>.Instance.SetDoNotPostProcessOnReturn();
		Singleton<GameController>.Instance.SelectObj(this.ghostObj, true, new GameController.DelegateAfterChatEndsEvents(this.ExitHandler), true, false, false);
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0001CA87 File Offset: 0x0001AC87
	private void CheckForDisable()
	{
		if (!GhostController.IsSeanceComplete() && Singleton<Save>.Instance.GetDateableTalkedTo(this.ghostObj.InternalName()) == Singleton<DayNightCycle>.Instance.GetDaysSinceStart())
		{
			this.orb.SetActive(false);
			return;
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0001CABE File Offset: 0x0001ACBE
	private void AttemptGhostEnable()
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized(this.ghostObj.InternalName()) == RelationshipStatus.Realized)
		{
			this.GhostEnable(false);
			return;
		}
		this.GhostEnable(true);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0001CAE8 File Offset: 0x0001ACE8
	public void GhostEnable(bool input)
	{
		if (input)
		{
			this.orb.SetActive(input);
			this.ps.emission.enabled = input;
			if (input)
			{
				this.ps.Play();
				return;
			}
		}
		else
		{
			this.ps.emission.enabled = input;
			this.orb.SetActive(input);
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0001CB47 File Offset: 0x0001AD47
	public void SetShownAtAllTimes(bool isShownAtAllTimes)
	{
		this.IsShownAtAllTimes = isShownAtAllTimes;
		if (this.IsShownAtAllTimes)
		{
			this.AttemptGhostEnable();
			return;
		}
		this.GhostEnable(false);
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0001CB68 File Offset: 0x0001AD68
	private void ExitHandler()
	{
		this.GhostEnable(false);
		if (GhostController.IsSeanceComplete())
		{
			this.ghostObj.AlternateInteractions[0] = this.genInt;
			return;
		}
		Singleton<GameController>.Instance.DialogueExit.RemoveListener(new UnityAction(this.ExitHandler));
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0001CBB8 File Offset: 0x0001ADB8
	public void CheckTime()
	{
		if (this.disappearForever)
		{
			this.GhostEnable(false);
		}
		if (Singleton<DayNightCycle>.Instance.CurrentDayPhase == DayPhase.MIDNIGHT && !GhostController.IsSeanceComplete())
		{
			this.AttemptGhostEnable();
		}
		else if (GhostController.IsSeanceComplete())
		{
			this.AttemptGhostEnable();
		}
		else
		{
			this.GhostEnable(false);
		}
		if (GhostController.IsSeanceComplete())
		{
			this.ghostObj.AlternateInteractions[0] = this.genInt;
		}
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0001CC24 File Offset: 0x0001AE24
	public void DisableAfterConversarion()
	{
		if (GhostController.IsSeanceComplete())
		{
			this.AttemptGhostEnable();
			this.ghostObj.AlternateInteractions[0] = this.genInt;
			return;
		}
		this.GhostEnable(false);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0001CC52 File Offset: 0x0001AE52
	public static bool IsSeanceComplete()
	{
		return Singleton<InkController>.Instance.GetVariable("zoey_seance_ready") == "complete";
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0001CC6D File Offset: 0x0001AE6D
	public void LoadState()
	{
		this.IsShownAtAllTimes = Singleton<Save>.Instance.GetInteractableState("Ghost_IsShownAtAllTimes");
		this.orb.SetActive(false);
		this.ExitHandler();
		this.CheckTime();
		this.CheckForDisable();
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001CCA2 File Offset: 0x0001AEA2
	public void SaveState()
	{
		Singleton<Save>.Instance.SetInteractableState("Ghost_IsShownAtAllTimes", this.IsShownAtAllTimes);
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0001CCB9 File Offset: 0x0001AEB9
	public int Priority()
	{
		return 3000;
	}

	// Token: 0x040004AE RID: 1198
	public ParticleSystem ps;

	// Token: 0x040004AF RID: 1199
	public GameObject orb;

	// Token: 0x040004B0 RID: 1200
	public bool IsShownAtAllTimes;

	// Token: 0x040004B1 RID: 1201
	public InteractableObj ghostObj;

	// Token: 0x040004B2 RID: 1202
	public GenericInteractable genInt;
}
