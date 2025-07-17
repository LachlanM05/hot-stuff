using System;
using T17.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001BB RID: 443
public class UIButtonSoundEvent : MonoBehaviour, ISelectHandler, IEventSystemHandler, IPointerEnterHandler, IPointerDownHandler
{
	// Token: 0x06000EFA RID: 3834 RVA: 0x0005182C File Offset: 0x0004FA2C
	private void Awake()
	{
		if (this.button == null)
		{
			this.button = base.gameObject.GetComponent<Button>();
		}
		if (this.button != null)
		{
			this.button.onClick.AddListener(new UnityAction(this.ButtonClicked));
		}
		this.initialized = true;
		if (this.phoneMenuOption)
		{
			this.alreadyPhoneMenu = true;
		}
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x00051898 File Offset: 0x0004FA98
	private void OnEnable()
	{
		this.CheckPhoneStatus();
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x000518A0 File Offset: 0x0004FAA0
	private void CheckPhoneStatus()
	{
		if (Singleton<PhoneManager>.Instance != null && Singleton<PhoneManager>.Instance.phoneOpened)
		{
			this.phoneMenuOption = true;
			return;
		}
		if (Singleton<PhoneManager>.Instance != null && !Singleton<PhoneManager>.Instance.phoneOpened && !this.alreadyPhoneMenu)
		{
			this.phoneMenuOption = false;
		}
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x000518F6 File Offset: 0x0004FAF6
	public void OnPointerEnter()
	{
		this.OnPointerEnter(null);
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x00051900 File Offset: 0x0004FB00
	public void OnPointerEnter(PointerEventData ped)
	{
		this.CheckPhoneStatus();
		if (Services.InputService.IsLastActiveInputController() || Services.UIInputService.HasInputControllerChangedRecently())
		{
			return;
		}
		if (this.ShouldPlaySFX())
		{
			if (this.selectOnPointerEnter)
			{
				ControllerMenuUI.SetCurrentlySelected(base.gameObject, ControllerMenuUI.Direction.Down, false, true);
			}
			if (this.doNotPlayPointerEnter)
			{
				return;
			}
			if (!this.dialogueOptionSpecCheck)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_option_specs_scroll.name, 1f);
			}
			if (this.specsOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_specs_menu_scroll.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_specs_menu_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dateadexOption)
			{
				if (DateADex.Instance._scrollingList.movedUp && !Singleton<AudioManager>.Instance.IsPlayingTrack(SFXBank.Instance.ui_date_a_dex_scroll_up.name))
				{
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_date_a_dex_scroll_up, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					DateADex.Instance._scrollingList.movedUp = false;
					return;
				}
				if (DateADex.Instance._scrollingList.movedDown && !Singleton<AudioManager>.Instance.IsPlayingTrack(SFXBank.Instance.ui_date_a_dex_scroll_down.name))
				{
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_date_a_dex_scroll_down, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					DateADex.Instance._scrollingList.movedDown = false;
					return;
				}
			}
			else
			{
				if (this.phoneMenuOption)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.dialogueOptionSpecCheck)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_option_specs_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_option_specs_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, true, SFX_SUBGROUP.UI, false);
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.workOption && this.dialogueOption)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.workOption && !this.dialogueOption && !this.phoneMenuOption)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.workOption && !this.dialogueOption && this.phoneMenuOption)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.dialogueOption || this.dialogueOptionNegative)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (!this.dialogueBox)
				{
					if (Singleton<AudioManager>.Instance.GetTrack("UI_Dialogue_Cont") != null)
					{
						return;
					}
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				else
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				}
			}
		}
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x00051D8E File Offset: 0x0004FF8E
	public void OnPointerDown(PointerEventData ped)
	{
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x00051D90 File Offset: 0x0004FF90
	public void OnSelect(BaseEventData eventData)
	{
		this.CheckPhoneStatus();
		if (this.ShouldPlaySFX())
		{
			if (this.doNotPlayOnSelect)
			{
				return;
			}
			if (!this.dialogueOptionSpecCheck)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_option_specs_scroll.name, 1f);
			}
			if (this.specsOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_specs_menu_scroll.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_specs_menu_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dateadexOption)
			{
				if (DateADex.Instance._scrollingList.movedUp && !Singleton<AudioManager>.Instance.IsPlayingTrack(SFXBank.Instance.ui_date_a_dex_scroll_up.name))
				{
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_date_a_dex_scroll_up, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					DateADex.Instance._scrollingList.movedUp = false;
					return;
				}
				if (DateADex.Instance._scrollingList.movedDown && !Singleton<AudioManager>.Instance.IsPlayingTrack(SFXBank.Instance.ui_date_a_dex_scroll_down.name))
				{
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_date_a_dex_scroll_down, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					DateADex.Instance._scrollingList.movedDown = false;
					return;
				}
			}
			else
			{
				if (this.phoneMenuOption)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.dialogueOptionSpecCheck)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_option_specs_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_option_specs_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, true, SFX_SUBGROUP.UI, false);
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.dialogueOption || this.dialogueOptionNegative)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.workOption && !this.phoneMenuOption)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				if (this.workOption && this.phoneMenuOption)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_scroll.name, 0f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
					return;
				}
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_scroll.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
		}
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x00052125 File Offset: 0x00050325
	public void ButtonClicked()
	{
		this.CheckPhoneStatus();
		if (this.ShouldPlaySFX())
		{
			if (this.doNotPlayButtonClick)
			{
				return;
			}
			this.ForcePlayClicked();
		}
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x00052144 File Offset: 0x00050344
	public void ForcePlayClicked()
	{
		if (Singleton<AudioManager>.Instance != null)
		{
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_option_specs_scroll.name, 1f);
			if (this.backMenuOption && !this.phoneMenuOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_exit.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.backMenuOption && this.phoneMenuOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_exit.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.specsOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_specs_menu_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_specs_menu_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.mainMenuOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.phoneMenuOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dateadexOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dialogueOptionSpecCheckInactive)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_exit.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dialogueOptionSpecCheck)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_option_specs_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_option_specs_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dialogueOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_option_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_option_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.workOption && !this.phoneMenuOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_canopy_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_canopy_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.workOption && this.phoneMenuOption)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_phone_menu_select.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_phone_menu_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dialogueOptionNegative)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_exit.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			if (this.dialogueBox)
			{
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dialogue_confirm.name, 0f);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dialogue_confirm, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
				return;
			}
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_select.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_select, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x000525CC File Offset: 0x000507CC
	private void OnValidate()
	{
		if (this.button == null)
		{
			this.button = base.gameObject.GetComponent<Button>();
		}
		if (this.button != null && this.button.gameObject.name.Contains("Back"))
		{
			this.playOptions = UIButtonSoundEvent.PlayOptions.InteractableAndActiveOrInactive;
		}
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x0005262C File Offset: 0x0005082C
	private bool ShouldPlaySFX()
	{
		if (!this.initialized)
		{
			return false;
		}
		if (Singleton<AudioManager>.Instance == null)
		{
			return false;
		}
		if (AwakenSplashScreen.Instance != null && AwakenSplashScreen.Instance.isOpen)
		{
			return false;
		}
		bool flag = false;
		Selectable component = base.transform.GetComponent<Selectable>();
		if (component != null)
		{
			flag = component.interactable;
		}
		switch (this.playOptions)
		{
		case UIButtonSoundEvent.PlayOptions.InteractableAndActiveOrInactive:
			return flag;
		case UIButtonSoundEvent.PlayOptions.Always:
			return true;
		}
		return flag && base.gameObject.activeInHierarchy;
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x000526BC File Offset: 0x000508BC
	public void SetAsDialogOption(bool negativeAction)
	{
		this.specsOption = false;
		this.mainMenuOption = false;
		this.phoneMenuOption = false;
		this.dialogueOptionSpecCheck = false;
		this.dialogueOptionSpecCheckInactive = false;
		this.dialogueBox = false;
		if (negativeAction)
		{
			this.backMenuOption = true;
		}
		else
		{
			this.backMenuOption = false;
		}
		this.playOptions = UIButtonSoundEvent.PlayOptions.InteractableAndActiveOrInactive;
	}

	// Token: 0x04000D3A RID: 3386
	public bool backMenuOption;

	// Token: 0x04000D3B RID: 3387
	public bool specsOption;

	// Token: 0x04000D3C RID: 3388
	public bool mainMenuOption;

	// Token: 0x04000D3D RID: 3389
	public bool phoneMenuOption;

	// Token: 0x04000D3E RID: 3390
	public bool dateadexOption;

	// Token: 0x04000D3F RID: 3391
	public bool dialogueOption;

	// Token: 0x04000D40 RID: 3392
	public bool workOption;

	// Token: 0x04000D41 RID: 3393
	public bool dialogueOptionSpecCheck;

	// Token: 0x04000D42 RID: 3394
	public bool dialogueOptionSpecCheckInactive;

	// Token: 0x04000D43 RID: 3395
	public bool dialogueBox;

	// Token: 0x04000D44 RID: 3396
	public bool dialogueOptionNegative;

	// Token: 0x04000D45 RID: 3397
	public UIButtonSoundEvent.PlayOptions playOptions;

	// Token: 0x04000D46 RID: 3398
	[SerializeField]
	private Button button;

	// Token: 0x04000D47 RID: 3399
	public bool doNotPlayPointerEnter;

	// Token: 0x04000D48 RID: 3400
	public bool doNotPlayButtonClick;

	// Token: 0x04000D49 RID: 3401
	public bool doNotPlayOnSelect;

	// Token: 0x04000D4A RID: 3402
	public bool selectOnPointerEnter = true;

	// Token: 0x04000D4B RID: 3403
	private bool initialized;

	// Token: 0x04000D4C RID: 3404
	private bool alreadyPhoneMenu;

	// Token: 0x0200038E RID: 910
	public enum PlayOptions
	{
		// Token: 0x04001403 RID: 5123
		InteractableAndActive,
		// Token: 0x04001404 RID: 5124
		InteractableAndActiveOrInactive,
		// Token: 0x04001405 RID: 5125
		Always
	}
}
