using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000169 RID: 361
public class RoomersButton : MonoBehaviour, ISelectHandler, IEventSystemHandler, IPointerEnterHandler, IDeselectHandler
{
	// Token: 0x06000CFE RID: 3326 RVA: 0x0004AFC0 File Offset: 0x000491C0
	private void Start()
	{
		base.gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(this.ButtonClicked));
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x0004AFE3 File Offset: 0x000491E3
	private void Update()
	{
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x0004AFE8 File Offset: 0x000491E8
	public void OnPointerEnter(PointerEventData ped)
	{
		if (base.transform.GetComponent<Selectable>().interactable)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x0004B02D File Offset: 0x0004922D
	public void ButtonClicked()
	{
		if (this.roomersInfo != null)
		{
			Roomers.Instance.switchEntryIndex(this.roomersEntryButton.index, true);
			ScrollInView component = base.GetComponent<ScrollInView>();
			if (component == null)
			{
				return;
			}
			component.ManualScrollToSelf();
		}
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x0004B063 File Offset: 0x00049263
	public void OnSelect(BaseEventData eventData)
	{
		if (this.roomersInfo != null)
		{
			this.AnimateSelected();
			Roomers.Instance.switchEntryIndex(this.roomersEntryButton.index, false);
		}
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x0004B08F File Offset: 0x0004928F
	public void OnDeselect(BaseEventData eventData)
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x0004B0B0 File Offset: 0x000492B0
	public void AnimateSelected()
	{
		base.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x0004B0D1 File Offset: 0x000492D1
	public void AnimateDeselected()
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x04000BC8 RID: 3016
	public RoomersEntryButton roomersEntryButton;

	// Token: 0x04000BC9 RID: 3017
	public RoomersInfo roomersInfo;
}
