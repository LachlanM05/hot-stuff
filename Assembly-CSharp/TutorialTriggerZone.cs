using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class TutorialTriggerZone : MonoBehaviour
{
	// Token: 0x06000769 RID: 1897 RVA: 0x00029F98 File Offset: 0x00028198
	private void OnTriggerEnter(Collider other)
	{
		if (this.eventType == TutorialTriggerZone.EventType.TriggerDrone)
		{
			Singleton<TutorialController>.Instance.DeliverGiftBox();
		}
	}

	// Token: 0x040006BA RID: 1722
	[SerializeField]
	public TutorialTriggerZone.EventType eventType;

	// Token: 0x020002FE RID: 766
	public enum EventType
	{
		// Token: 0x040011F4 RID: 4596
		TriggerDrone
	}
}
