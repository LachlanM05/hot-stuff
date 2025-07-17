using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B4 RID: 180
public class MorningRoutine : Singleton<MorningRoutine>, IReloadHandler
{
	// Token: 0x06000599 RID: 1433 RVA: 0x000202C2 File Offset: 0x0001E4C2
	public void Start()
	{
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x000202C4 File Offset: 0x0001E4C4
	public void StartMorningRoutine(int day)
	{
		this.ExecuteMorningRoutine(day);
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x000202CD File Offset: 0x0001E4CD
	public void openworkspace()
	{
		Singleton<ComputerManager>.Instance.SwapToCanopyMenu();
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x000202D9 File Offset: 0x0001E4D9
	public void ExecuteMorningRoutine(int day)
	{
		this.startofcutscene.Invoke();
		this.endofcutscene.Invoke();
		PlayerPauser.Unfreeze();
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x000202F6 File Offset: 0x0001E4F6
	public IEnumerator WaitingForFinishedMorning()
	{
		yield return new WaitUntil(() => Singleton<ComputerManager>.Instance.ReadyToStart());
		yield return new WaitUntil(() => Singleton<ComputerManager>.Instance.ReadyToExit());
		UnityEvent unityEvent = new UnityEvent();
		unityEvent.AddListener(new UnityAction(Singleton<ComputerManager>.Instance.ExitOutOfCanopyMenu));
		Singleton<TutorialController>.Instance.SetTutorialText("Select the top right button to leave work.", false);
		if (Singleton<Save>.Instance.GetTutorialFinished())
		{
			Singleton<Popup>.Instance.CreatePopup("Finished", "You are finished with your tasks!", unityEvent, true);
		}
		else
		{
			Singleton<Popup>.Instance.CreatePopup("AD", "FEELING LONELY ? I KNOW I DO AT TIMES <br>TRY VALDIVIAN'S NEWEST PRODUCT, THE <br>DATEVIATORS <br>YOU CAN SEE THE WORLD IN A WHOLE NEW WAY <br>SIGN UP FOR BETA TESTING TODAY", unityEvent, true);
		}
		yield break;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x000202FE File Offset: 0x0001E4FE
	public void setPlayerLookAt(GameObject g)
	{
		if (BetterPlayerControl.Instance != null)
		{
			BetterPlayerControl.Instance.transform.LookAt(g.transform);
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00020324 File Offset: 0x0001E524
	public void setplayerpos(GameObject g, bool updateRotation = false)
	{
		if (BetterPlayerControl.Instance != null)
		{
			BetterPlayerControl.Instance.transform.position = g.transform.position;
			if (updateRotation)
			{
				BetterPlayerControl.Instance.transform.rotation = g.transform.rotation;
			}
		}
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00020375 File Offset: 0x0001E575
	public void setplayerpos(GameObject g)
	{
		if (BetterPlayerControl.Instance != null)
		{
			BetterPlayerControl.Instance.transform.position = g.transform.position;
		}
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0002039E File Offset: 0x0001E59E
	public void LoadState()
	{
		DayNightCycle._dayevents += this.StartMorningRoutine;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x000203B1 File Offset: 0x0001E5B1
	public void SaveState()
	{
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x000203B3 File Offset: 0x0001E5B3
	public int Priority()
	{
		return 100;
	}

	// Token: 0x0400056A RID: 1386
	public UnityEvent startofcutscene;

	// Token: 0x0400056B RID: 1387
	public UnityEvent endofcutscene;
}
