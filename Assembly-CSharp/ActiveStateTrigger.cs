using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000195 RID: 405
public class ActiveStateTrigger : MonoBehaviour
{
	// Token: 0x06000DF8 RID: 3576 RVA: 0x0004DD72 File Offset: 0x0004BF72
	private void OnEnable()
	{
		if (this.trigger == ActiveStateTrigger.TriggerOn.Enabled || (!this.triggeredBefore && this.trigger == ActiveStateTrigger.TriggerOn.First_Enabled))
		{
			this.PerformAction();
		}
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x0004DD93 File Offset: 0x0004BF93
	private void OnDisable()
	{
		if (this.trigger == ActiveStateTrigger.TriggerOn.Disabled || (!this.triggeredBefore && this.trigger == ActiveStateTrigger.TriggerOn.First_Disabled))
		{
			this.PerformAction();
		}
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x0004DDB5 File Offset: 0x0004BFB5
	private void PerformAction()
	{
		this.triggeredBefore = true;
		UnityEvent unityEvent = this.actionToTake;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04000C6C RID: 3180
	public ActiveStateTrigger.TriggerOn trigger = ActiveStateTrigger.TriggerOn.Enabled;

	// Token: 0x04000C6D RID: 3181
	public UnityEvent actionToTake;

	// Token: 0x04000C6E RID: 3182
	private bool triggeredBefore;

	// Token: 0x0200037D RID: 893
	public enum TriggerOn
	{
		// Token: 0x040013C1 RID: 5057
		First_Enabled,
		// Token: 0x040013C2 RID: 5058
		Enabled,
		// Token: 0x040013C3 RID: 5059
		First_Disabled,
		// Token: 0x040013C4 RID: 5060
		Disabled
	}
}
