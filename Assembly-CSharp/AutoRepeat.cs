using System;
using T17.Services;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class AutoRepeat : MonoBehaviour
{
	// Token: 0x06000E1D RID: 3613 RVA: 0x0004E47B File Offset: 0x0004C67B
	private void Awake()
	{
		this.ResetAutoRepeat();
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0004E484 File Offset: 0x0004C684
	private void Update()
	{
		if (!this.IsButtonPressed())
		{
			this.ResetAutoRepeat();
			return;
		}
		switch (this.state)
		{
		case AutoRepeat.AutoRepeatState.Idle:
			this.timeLeft = this.timeBeforeRepeat;
			this.state = AutoRepeat.AutoRepeatState.WaitingToStart;
			return;
		case AutoRepeat.AutoRepeatState.WaitingToStart:
			this.timeLeft -= Time.deltaTime;
			if (this.timeLeft <= 0f)
			{
				this.timeLeft = this.timeBetweenRepeat;
				this.state = AutoRepeat.AutoRepeatState.WaitingToRepeat;
				this.repeated = true;
				return;
			}
			break;
		case AutoRepeat.AutoRepeatState.WaitingToRepeat:
			this.timeLeft -= Time.deltaTime;
			if (this.timeLeft <= 0f)
			{
				this.timeLeft = this.timeBetweenRepeat;
				this.repeated = true;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0004E53C File Offset: 0x0004C73C
	private bool IsButtonPressed()
	{
		bool flag = false;
		switch (this.inputDirection)
		{
		case ControllerMenuUI.Direction.Up:
			flag = Services.UIInputService.IsUIUpPressed();
			break;
		case ControllerMenuUI.Direction.Down:
			flag = Services.UIInputService.IsUIDownPressed();
			break;
		case ControllerMenuUI.Direction.Right:
			flag = Services.UIInputService.IsUIRightPressed();
			break;
		case ControllerMenuUI.Direction.Left:
			flag = Services.UIInputService.IsUILeftPressed();
			break;
		}
		return flag;
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0004E59D File Offset: 0x0004C79D
	public bool HasDirectionRepeated()
	{
		if (this.state == AutoRepeat.AutoRepeatState.WaitingToRepeat && this.repeated)
		{
			this.repeated = false;
			return true;
		}
		return false;
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0004E5BA File Offset: 0x0004C7BA
	private void ResetAutoRepeat()
	{
		this.state = AutoRepeat.AutoRepeatState.Idle;
		this.repeated = false;
	}

	// Token: 0x04000C8A RID: 3210
	public ControllerMenuUI.Direction inputDirection = ControllerMenuUI.Direction.Down;

	// Token: 0x04000C8B RID: 3211
	public float timeBeforeRepeat = 0.6f;

	// Token: 0x04000C8C RID: 3212
	public float timeBetweenRepeat = 0.15f;

	// Token: 0x04000C8D RID: 3213
	private float timeLeft;

	// Token: 0x04000C8E RID: 3214
	private bool repeated;

	// Token: 0x04000C8F RID: 3215
	private AutoRepeat.AutoRepeatState state;

	// Token: 0x02000380 RID: 896
	private enum AutoRepeatState
	{
		// Token: 0x040013CA RID: 5066
		Idle,
		// Token: 0x040013CB RID: 5067
		WaitingToStart,
		// Token: 0x040013CC RID: 5068
		WaitingToRepeat
	}
}
