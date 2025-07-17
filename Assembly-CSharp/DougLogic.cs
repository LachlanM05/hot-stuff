using System;
using Rewired;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class DougLogic : Singleton<DougLogic>
{
	// Token: 0x060001EF RID: 495 RVA: 0x0000BD80 File Offset: 0x00009F80
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.timer = this.DougTime;
		this.player.AddInputEventDelegate(new Action<InputActionEventData>(this.TimerReset), UpdateLoopType.Update, InputActionEventType.ButtonPressed);
		this.player.AddInputEventDelegate(new Action<InputActionEventData>(this.TimerReset), UpdateLoopType.Update, InputActionEventType.AxisActive);
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000BDE0 File Offset: 0x00009FE0
	private void Update()
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized("doug") == RelationshipStatus.Realized)
		{
			base.enabled = false;
		}
		if (BetterPlayerControl.Instance.STATE != BetterPlayerControl.PlayerState.CanControl || PlayerPauser.IsPaused() || PlayerPauser.IsFrozen() || !Singleton<Dateviators>.Instance.Equipped || !base.enabled)
		{
			return;
		}
		this.UpdateTimer();
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000BE3C File Offset: 0x0000A03C
	private void UpdateTimer()
	{
		if (this.lookingAtWall)
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{
				if (!this.dougAwakened)
				{
					this.DougTime = 10f;
				}
				this.timer = this.DougTime;
				if (Singleton<Dateviators>.Instance.CanConsumeCharge())
				{
					this.TalkToDoug();
				}
			}
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000BEA1 File Offset: 0x0000A0A1
	public void TimerReset(InputActionEventData data)
	{
		this.timer = this.DougTime;
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000BEAF File Offset: 0x0000A0AF
	public void SetLookingAtWall(bool looking)
	{
		this.lookingAtWall = looking;
		if (!this.lookingAtWall)
		{
			this.timer = this.DougTime;
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000BECC File Offset: 0x0000A0CC
	public void TalkToDoug()
	{
		if (!this.TalkedTo && Singleton<Save>.Instance.GetFullTutorialFinished() && Singleton<Save>.Instance.GetDateStatusRealized("doug") != RelationshipStatus.Realized)
		{
			this.TalkedTo = true;
			Singleton<Save>.Instance.MeetDatableIfUnmet("doug");
			Singleton<GameController>.Instance.ForceDialogue("doug_dread", null, false, true);
		}
	}

	// Token: 0x040002D9 RID: 729
	public float DougTime = 30f;

	// Token: 0x040002DA RID: 730
	public bool dougAwakened;

	// Token: 0x040002DB RID: 731
	public bool TalkedTo;

	// Token: 0x040002DC RID: 732
	public bool lookingAtWall;

	// Token: 0x040002DD RID: 733
	private Player player;

	// Token: 0x040002DE RID: 734
	private float timer;
}
