using System;
using T17.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000206 RID: 518
	public class PressOnControllerSelectComponent : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x060010DC RID: 4316 RVA: 0x00056714 File Offset: 0x00054914
		public void OnSelect(BaseEventData eventData)
		{
			if (!Services.InputService.IsLastActiveInputController())
			{
				return;
			}
			Button component = base.GetComponent<Button>();
			if (component != null && component.gameObject.activeInHierarchy && component.interactable)
			{
				Button.ButtonClickedEvent onClick = component.onClick;
				if (onClick == null)
				{
					return;
				}
				onClick.Invoke();
			}
		}
	}
}
