using System;
using System.Collections.Generic;

// Token: 0x020000BB RID: 187
[Serializable]
public class PlayerPauser : Singleton<PlayerPauser>
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060005C8 RID: 1480 RVA: 0x00020F1C File Offset: 0x0001F11C
	// (remove) Token: 0x060005C9 RID: 1481 RVA: 0x00020F50 File Offset: 0x0001F150
	public static event PlayerPauser._Pause _pause;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060005CA RID: 1482 RVA: 0x00020F84 File Offset: 0x0001F184
	// (remove) Token: 0x060005CB RID: 1483 RVA: 0x00020FB8 File Offset: 0x0001F1B8
	public static event PlayerPauser._Freeze _freeze;

	// Token: 0x060005CC RID: 1484 RVA: 0x00020FEB File Offset: 0x0001F1EB
	public static void ClearAll()
	{
		Singleton<PlayerPauser>.Instance.PauseList.Clear();
		PlayerPauser.UpdatePauseStates();
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00021001 File Offset: 0x0001F201
	public static void Pause(object caller)
	{
		Singleton<PlayerPauser>.Instance.PauseList.Add(new PauseState(PauseOrFreeze.PAUSE, caller));
		PlayerPauser.UpdatePauseStates();
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0002101E File Offset: 0x0001F21E
	public static void Pause()
	{
		if (Singleton<PlayerPauser>.Instance != null)
		{
			Singleton<PlayerPauser>.Instance.PauseList.Add(new PauseState(PauseOrFreeze.PAUSE, "In_Scene_Object"));
			PlayerPauser.UpdatePauseStates();
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002104C File Offset: 0x0001F24C
	public static bool IsPaused()
	{
		if (Singleton<PlayerPauser>.Instance == null || Singleton<PlayerPauser>.Instance.PauseList == null)
		{
			return false;
		}
		return Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState state) => state.Type == PauseOrFreeze.PAUSE) != null;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x000210A5 File Offset: 0x0001F2A5
	public static bool IsFrozen()
	{
		return Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState state) => state.Type == PauseOrFreeze.FREEZE) != null;
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x000210D8 File Offset: 0x0001F2D8
	public static void Freeze(object caller)
	{
		Singleton<PlayerPauser>.Instance.PauseList.Add(new PauseState(PauseOrFreeze.FREEZE, caller));
		PlayerPauser.UpdatePauseStates();
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x000210F5 File Offset: 0x0001F2F5
	public static void Freeze()
	{
		Singleton<PlayerPauser>.Instance.PauseList.Add(new PauseState(PauseOrFreeze.FREEZE, "In_Scene_Object"));
		PlayerPauser.UpdatePauseStates();
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00021118 File Offset: 0x0001F318
	public static void Unpause(object caller)
	{
		PauseState pauseState = Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState state) => state.Type == PauseOrFreeze.PAUSE && state.Source == caller.ToString());
		if (pauseState != null)
		{
			Singleton<PlayerPauser>.Instance.PauseList.Remove(pauseState);
		}
		PlayerPauser.UpdatePauseStates();
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00021168 File Offset: 0x0001F368
	public static void Unpause()
	{
		if (Singleton<PlayerPauser>.Instance != null)
		{
			PauseState pauseState = Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState state) => state.Type == PauseOrFreeze.PAUSE);
			if (pauseState != null)
			{
				Singleton<PlayerPauser>.Instance.PauseList.Remove(pauseState);
			}
			PlayerPauser.UpdatePauseStates();
		}
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x000211CC File Offset: 0x0001F3CC
	public static void ForceUnpause()
	{
		if (Singleton<PlayerPauser>.Instance != null)
		{
			foreach (PauseState pauseState in Singleton<PlayerPauser>.Instance.PauseList.FindAll((PauseState state) => state.Type == PauseOrFreeze.PAUSE))
			{
				if (pauseState != null)
				{
					Singleton<PlayerPauser>.Instance.PauseList.Remove(pauseState);
				}
			}
			PlayerPauser.UpdatePauseStates();
		}
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00021268 File Offset: 0x0001F468
	public static void Unfreeze()
	{
		PauseState pauseState = Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState state) => state.Type == PauseOrFreeze.FREEZE);
		if (pauseState != null)
		{
			Singleton<PlayerPauser>.Instance.PauseList.Remove(pauseState);
		}
		PlayerPauser.UpdatePauseStates();
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x000212C0 File Offset: 0x0001F4C0
	public static void UpdatePauseStates()
	{
		PlayerPauser._Pause pause = PlayerPauser._pause;
		if (pause != null)
		{
			pause(Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState state) => state.Type == PauseOrFreeze.PAUSE) != null);
		}
		PlayerPauser._Freeze freeze = PlayerPauser._freeze;
		if (freeze == null)
		{
			return;
		}
		freeze(Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState state) => state.Type == PauseOrFreeze.FREEZE) != null);
	}

	// Token: 0x04000588 RID: 1416
	public List<PauseState> PauseList = new List<PauseState>();

	// Token: 0x020002DD RID: 733
	// (Invoke) Token: 0x060015FE RID: 5630
	public delegate void _Pause(bool on);

	// Token: 0x020002DE RID: 734
	// (Invoke) Token: 0x06001602 RID: 5634
	public delegate void _Freeze(bool on);
}
