using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200011F RID: 287
public class PhoneAppManager : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	// Token: 0x060009B1 RID: 2481 RVA: 0x00037B48 File Offset: 0x00035D48
	public void UpdateApp(bool IsGlassesEquipped)
	{
		Image componentInChildren = base.gameObject.GetComponentInChildren<Image>();
		if (IsGlassesEquipped && this.spriteOn != null)
		{
			componentInChildren.sprite = this.spriteOn;
			return;
		}
		componentInChildren.sprite = this.spriteOff;
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00037B8B File Offset: 0x00035D8B
	public void OnSelect(BaseEventData eventData)
	{
		Singleton<PhoneManager>.Instance.ToggleSelectedItem();
		base.GetComponent<Animator>().SetBool("selected", true);
	}

	// Token: 0x040008F0 RID: 2288
	[SerializeField]
	private Sprite spriteOff;

	// Token: 0x040008F1 RID: 2289
	[SerializeField]
	private Sprite spriteOn;
}
