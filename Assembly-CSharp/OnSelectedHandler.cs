using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200011C RID: 284
public class OnSelectedHandler : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x0600099C RID: 2460 RVA: 0x00037654 File Offset: 0x00035854
	public void OnSelect(BaseEventData eventData)
	{
		for (int i = 0; i < this.onSelectEvents.Count; i++)
		{
			this.onSelectEvents[i].Invoke(eventData);
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0003768C File Offset: 0x0003588C
	public void OnDeselect(BaseEventData eventData)
	{
		for (int i = 0; i < this.onDeselectEvents.Count; i++)
		{
			this.onDeselectEvents[i].Invoke(eventData);
		}
	}

	// Token: 0x040008DF RID: 2271
	public List<OnSelectedHandler.TriggerEvent> onSelectEvents;

	// Token: 0x040008E0 RID: 2272
	public List<OnSelectedHandler.TriggerEvent> onDeselectEvents;

	// Token: 0x02000324 RID: 804
	[Serializable]
	public class TriggerEvent : UnityEvent<BaseEventData>
	{
	}
}
