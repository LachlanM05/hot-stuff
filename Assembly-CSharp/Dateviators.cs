using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x02000042 RID: 66
public class Dateviators : Singleton<Dateviators>, IReloadHandler
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000161 RID: 353 RVA: 0x00009911 File Offset: 0x00007B11
	public bool IsEquipped
	{
		get
		{
			return this.Equipped;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000162 RID: 354 RVA: 0x00009919 File Offset: 0x00007B19
	public bool IsEquippingOrEquipped
	{
		get
		{
			return this.Equipped | this.inProgress;
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00009928 File Offset: 0x00007B28
	public int GetCurrentCharges()
	{
		return this._currentCharges;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00009930 File Offset: 0x00007B30
	public void Start()
	{
		Shader.SetGlobalColor("OutlineColor", this._colorOff);
		this.UnequippedHUD.SetActive(true);
		this.EquippedHUD.SetActive(false);
		this.hudTimeOfDayIcon.SetActive(true);
		Singleton<GameController>.Instance.UpdateHUD.AddListener(new UnityAction(this.UpdateBatteryUI));
		this.UpdateBatteryUI();
		this.equippedReticleCanvas = this.equippedReticle.gameObject.GetComponent<Canvas>();
		this.equippedReticleCanvasFill = this.equippedReticleFill.gameObject.GetComponent<Canvas>();
	}

	// Token: 0x06000165 RID: 357 RVA: 0x000099C0 File Offset: 0x00007BC0
	public void Equip()
	{
		Bed bed = Object.FindObjectOfType<Bed>();
		if (this.Equipped || Singleton<Save>.Instance.GetDateStatus("skylar_specs") == RelationshipStatus.Unmet || DateADex.Instance.startedEnding || Singleton<Popup>.Instance.IsPopupOpen() || (bed != null && bed.playingAnimation) || Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1000_DATEVIATORS_GONE) || Singleton<PhoneManager>.Instance.IsPhoneAnimating())
		{
			return;
		}
		base.StartCoroutine(this.InvokeTriggerGlassesOn());
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO) && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_TALKED_TO_SKYLAR_DAY_TWO))
		{
			if (Singleton<PhoneManager>.Instance.phoneOpened)
			{
				Singleton<PhoneManager>.Instance.ClosePhoneAsync(new SimpleAnimController.AnimFinishedEvent(this.StartSkylarChat), false);
				return;
			}
			this.StartSkylarChat();
		}
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00009A8C File Offset: 0x00007C8C
	private void StartSkylarChat()
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized("skylar_specs") == RelationshipStatus.Realized)
		{
			return;
		}
		Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_TALKED_TO_SKYLAR_DAY_TWO);
		Singleton<GameController>.Instance.ForceDialogue("skylar_specs.day_two_skylar", null, true, false);
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00009AC2 File Offset: 0x00007CC2
	public void EquipOverride()
	{
		this.noSound = true;
		UnityEvent equipOverridden = this.EquipOverridden;
		if (equipOverridden != null)
		{
			equipOverridden.Invoke();
		}
		this.glassesHUDButton.TriggerTransitionOn();
		base.StartCoroutine(this.InvokeTriggerGlassesOn());
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00009AF4 File Offset: 0x00007CF4
	public void InstantGlassesOn()
	{
		base.StopAllCoroutines();
		this.noSound = true;
		this.ForcePostProcessVolumeTo1();
		this.inProgress = false;
		this.hudAnimations.Play("HudGlassesOn");
		this.TriggerGlassesOn();
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00009B26 File Offset: 0x00007D26
	public void InstantGlassesOff()
	{
		base.StopAllCoroutines();
		this.noSound = true;
		this.ForcePostProcessVolumeTo0();
		this.inProgress = false;
		this.hudAnimations.Play("HudGlassesOff");
		this.TriggerGlassesOff();
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00009B58 File Offset: 0x00007D58
	public void ResetTriggers()
	{
		this.hudAnimations.ResetTrigger("GlassesOn");
		this.hudAnimations.ResetTrigger("GlassesOff");
		this.hudAnimations.ResetTrigger("DialogueEnter");
		this.hudAnimations.ResetTrigger("DialogueExit");
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00009BA5 File Offset: 0x00007DA5
	private IEnumerator InvokeTriggerGlassesOn()
	{
		UnityEvent transitionOn = this.TransitionOn;
		if (transitionOn != null)
		{
			transitionOn.Invoke();
		}
		yield return null;
		if (this.inProgress)
		{
			yield break;
		}
		this.inProgress = true;
		base.StopCoroutine(this.InvokeTriggerGlassesOff());
		this.hudAnimations.ResetTrigger("GlassesOn");
		this.hudAnimations.ResetTrigger("GlassesOff");
		yield return null;
		this.hudAnimations.SetBool("DateviatorsOff", false);
		yield return new WaitUntil(() => !this.hudAnimations.IsInTransition(0));
		this.hudAnimations.SetTrigger("GlassesOn");
		yield return new WaitUntil(() => !this.hudAnimations.IsInTransition(0));
		this.TriggerGlassesOn();
		this.inProgress = false;
		yield break;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00009BB4 File Offset: 0x00007DB4
	private IEnumerator InvokeTriggerGlassesOff()
	{
		UnityEvent transitionOff = this.TransitionOff;
		if (transitionOff != null)
		{
			transitionOff.Invoke();
		}
		yield return null;
		if (this.inProgress)
		{
			yield break;
		}
		this.inProgress = true;
		base.StopCoroutine(this.InvokeTriggerGlassesOn());
		this.hudAnimations.ResetTrigger("GlassesOn");
		this.hudAnimations.ResetTrigger("GlassesOff");
		yield return null;
		this.hudAnimations.SetBool("DateviatorsOff", true);
		yield return new WaitUntil(() => !this.hudAnimations.IsInTransition(0));
		this.hudAnimations.SetTrigger("GlassesOff");
		yield return new WaitUntil(() => !this.hudAnimations.IsInTransition(0));
		this.TriggerGlassesOff();
		this.inProgress = false;
		yield break;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00009BC3 File Offset: 0x00007DC3
	public void ForcePostProcessVolumeTo1()
	{
		this.PostProcessOn = true;
		this.PostProcess2.SetActive(true);
		this.PostProcess1.weight = 1f;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00009BE8 File Offset: 0x00007DE8
	public void ForcePostProcessVolumeTo0()
	{
		this.PostProcessOn = false;
		this.PostProcess1.weight = 0f;
		this.PostProcess2.SetActive(false);
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00009C10 File Offset: 0x00007E10
	private void TriggerGlassesOn()
	{
		Shader.SetGlobalColor("OutlineColor", this._colorOn);
		this.PostProcess2.SetActive(true);
		this.PostProcess1.weight = 1f;
		this.Equipped = true;
		this.UnequippedHUD.SetActive(false);
		this.EquippedHUD.SetActive(true);
		UnityEvent equippedDateviators = this.EquippedDateviators;
		if (equippedDateviators != null)
		{
			equippedDateviators.Invoke();
		}
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
		BetterPlayerControl.Instance.StopBeamSounds();
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dateviators_on.name, 0f);
		if (!this.noSound)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateviators_on, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		this.noSound = false;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00009CE8 File Offset: 0x00007EE8
	private void TriggerGlassesOff()
	{
		Shader.SetGlobalColor("OutlineColor", this._colorOff);
		this.PostProcess1.weight = 0f;
		this.PostProcess2.SetActive(false);
		this.Equipped = false;
		this.UnequippedHUD.SetActive(true);
		this.EquippedHUD.SetActive(false);
		UnityEvent unequippedDateviators = this.UnequippedDateviators;
		if (unequippedDateviators != null)
		{
			unequippedDateviators.Invoke();
		}
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dateviators_off.name, 0f);
		if (!this.noSound)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateviators_off, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		this.noSound = false;
		BetterPlayerControl.Instance.StopBeamSounds();
		Singleton<GameController>.Instance.UpdateHUD.Invoke();
		Singleton<InteractableManager>.Instance.StopPulse();
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00009DC7 File Offset: 0x00007FC7
	private void TriggerNewPhoneMessageInHud(int unreadMessages)
	{
		if (unreadMessages > 0)
		{
			this.phoneHUDButton.SetAlert();
			return;
		}
		this.phoneHUDButton.SetDefault();
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00009DE4 File Offset: 0x00007FE4
	public void ClearNewPhoneMessageInHud()
	{
		this.phoneHUDButton.SetDefault();
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00009DF1 File Offset: 0x00007FF1
	public void Dequip()
	{
		if (!this.Equipped)
		{
			return;
		}
		base.StartCoroutine(this.InvokeTriggerGlassesOff());
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00009E09 File Offset: 0x00008009
	public void DequipOverride()
	{
		this.noSound = true;
		base.StartCoroutine(this.InvokeTriggerGlassesOff());
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00009E20 File Offset: 0x00008020
	public void StartHud()
	{
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1_WENT_TO_WORK) && !Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1000_DATEVIATORS_GONE))
		{
			this.hudAnimations.SetTrigger("StartHud");
			return;
		}
		this.hudAnimations.SetTrigger("StartHudNoPhone");
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00009E70 File Offset: 0x00008070
	public void StartAddPhoneAnimation()
	{
		this.hudAnimations.SetTrigger("AddPhone");
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00009E84 File Offset: 0x00008084
	public void LoadState()
	{
		this._currentCharges = Save.GetSaveData(false).dateviators_currentCharges;
		this.UpdateBatteryUI();
		if (this.Equipped)
		{
			this.ForcePostProcessVolumeTo1();
		}
		else
		{
			this.ForcePostProcessVolumeTo0();
		}
		if (Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_0_ANIMATIONS))
		{
			this.StartHud();
		}
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00009ED5 File Offset: 0x000080D5
	public void OnDestroy()
	{
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00009ED7 File Offset: 0x000080D7
	public void SaveState()
	{
		Save.GetSaveData(false).dateviators_currentCharges = this._currentCharges;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00009EEC File Offset: 0x000080EC
	public bool CanConsumeCharge()
	{
		bool flag = false;
		if (this._currentCharges > 0)
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00009F07 File Offset: 0x00008107
	public bool ConsumeCharge()
	{
		bool flag = this.CanConsumeCharge();
		if (flag)
		{
			this._currentCharges--;
			this.UpdateBatteryUI();
		}
		return flag;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00009F26 File Offset: 0x00008126
	public void ResetCharges()
	{
		this._currentCharges = 5;
		this.UpdateBatteryUI();
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00009F38 File Offset: 0x00008138
	public void UpdateBatteryUI()
	{
		for (int i = 0; i < 5; i++)
		{
			if (i + 1 <= this._currentCharges)
			{
				this.UnequippedBatteryPips[i].sprite = this.BatteryPipUnequipped;
			}
			else
			{
				this.UnequippedBatteryPips[i].sprite = this.BatteryPipUnequippedEmpty;
			}
		}
		for (int j = 0; j < 5; j++)
		{
			if (j + 1 <= this._currentCharges)
			{
				this.EquippedBatteryPips[j].sprite = this.BatteryPipEquipped;
			}
			else
			{
				this.EquippedBatteryPips[j].sprite = this.BatteryPipEquippedEmpty;
			}
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00009FC3 File Offset: 0x000081C3
	public void UpdatePhoneAlertDisplay(int unreadMessages)
	{
		this.phoneAlertUnequipped.gameObject.SetActive(unreadMessages > 0);
		TextMeshProUGUI componentInChildren = this.phoneAlertUnequipped.GetComponentInChildren<TextMeshProUGUI>();
		if (componentInChildren != null)
		{
			componentInChildren.SetText(unreadMessages.ToString(), true);
		}
		if (unreadMessages > 0)
		{
			this.TriggerNewPhoneMessageInHud(unreadMessages);
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000A002 File Offset: 0x00008202
	public int Priority()
	{
		return 0;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000A005 File Offset: 0x00008205
	public void DisableReticle()
	{
		this.equippedReticle.enabled = false;
		this.unequippedReticle.enabled = false;
		this.equippedReticleFill.enabled = false;
		this.unequippedReticleFill.enabled = false;
		this.SetReticleCavasSorting();
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000A040 File Offset: 0x00008240
	public void HideReticle()
	{
		this.unequippedReticle.color = new Color(0.7f, 0.7f, 0.7f, 1f);
		this.equippedReticle.color = Color.gray;
		this.unequippedReticle.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
		this.equippedReticle.sprite = this.heartSpriteSmall;
		this.equippedReticleFill.sprite = this.fillSpriteSmall;
		this.equippedReticle.enabled = true;
		this.unequippedReticle.enabled = true;
		this.equippedReticleFill.enabled = true;
		this.unequippedReticleFill.enabled = true;
		this.SetReticleCavasSorting();
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000A100 File Offset: 0x00008300
	public void ShowReticle()
	{
		this.unequippedReticle.color = Color.white;
		this.equippedReticle.color = Color.magenta;
		this.unequippedReticle.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
		this.equippedReticle.sprite = this.heartSprite;
		this.equippedReticleFill.sprite = this.fillSprite;
		this.equippedReticle.enabled = true;
		this.unequippedReticle.enabled = true;
		this.equippedReticleFill.enabled = true;
		this.unequippedReticleFill.enabled = true;
		this.SetReticleCavasSorting();
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000A1AC File Offset: 0x000083AC
	private void SetReticleCavasSorting()
	{
		PhoneManager instance = Singleton<PhoneManager>.Instance;
		bool flag = false;
		if (instance.isActiveAndEnabled || instance.IsPhoneAnimating())
		{
			flag = Singleton<Save>.Instance.GetDateStatus("phoenicia_phone") == RelationshipStatus.Unmet || this.equippedReticleFill.fillAmount > 0f;
		}
		if (flag != this.equippedReticleCanvas.overrideSorting)
		{
			if (flag)
			{
				this.equippedReticleCanvas.overrideSorting = true;
				this.equippedReticleCanvas.sortingOrder = 150;
				this.equippedReticleCanvasFill.overrideSorting = true;
				this.equippedReticleCanvasFill.sortingOrder = 149;
				return;
			}
			this.equippedReticleCanvas.sortingOrder = 100;
			this.equippedReticleCanvas.overrideSorting = false;
			this.equippedReticleCanvasFill.sortingOrder = 100;
			this.equippedReticleCanvasFill.overrideSorting = false;
		}
	}

	// Token: 0x04000266 RID: 614
	public bool Equipped;

	// Token: 0x04000267 RID: 615
	public bool PostProcessOn;

	// Token: 0x04000268 RID: 616
	[Header("Visuals")]
	private Color _colorOn = new Color(1f, 0.54f, 0.8f);

	// Token: 0x04000269 RID: 617
	private Color _colorOff = new Color(0.2f, 0.53f, 0.92f);

	// Token: 0x0400026A RID: 618
	public Volume PostProcess1;

	// Token: 0x0400026B RID: 619
	public GameObject PostProcess2;

	// Token: 0x0400026C RID: 620
	[Header("UI")]
	public GameObject EquippedHUD;

	// Token: 0x0400026D RID: 621
	public GameObject UnequippedHUD;

	// Token: 0x0400026E RID: 622
	public GameObject hudTimeOfDayIcon;

	// Token: 0x0400026F RID: 623
	public Image[] EquippedBatteryPips;

	// Token: 0x04000270 RID: 624
	public Image[] UnequippedBatteryPips;

	// Token: 0x04000271 RID: 625
	public Sprite BatteryPipUnequipped;

	// Token: 0x04000272 RID: 626
	public Sprite BatteryPipUnequippedEmpty;

	// Token: 0x04000273 RID: 627
	public Sprite BatteryPipEquipped;

	// Token: 0x04000274 RID: 628
	public Sprite BatteryPipEquippedEmpty;

	// Token: 0x04000275 RID: 629
	public GameObject InteractionMenu;

	// Token: 0x04000276 RID: 630
	public const int MAX_CHARGES = 5;

	// Token: 0x04000277 RID: 631
	private int _currentCharges = 5;

	// Token: 0x04000278 RID: 632
	public Animator hudAnimations;

	// Token: 0x04000279 RID: 633
	public UnityEvent EquippedDateviators = new UnityEvent();

	// Token: 0x0400027A RID: 634
	public UnityEvent UnequippedDateviators = new UnityEvent();

	// Token: 0x0400027B RID: 635
	public UnityEvent TransitionOff = new UnityEvent();

	// Token: 0x0400027C RID: 636
	public UnityEvent TransitionOn = new UnityEvent();

	// Token: 0x0400027D RID: 637
	public UnityEvent EquipOverridden = new UnityEvent();

	// Token: 0x0400027E RID: 638
	public bool inProgress;

	// Token: 0x0400027F RID: 639
	[SerializeField]
	private Image phoneAlertUnequipped;

	// Token: 0x04000280 RID: 640
	[SerializeField]
	private Image equippedReticle;

	// Token: 0x04000281 RID: 641
	[SerializeField]
	private Image unequippedReticle;

	// Token: 0x04000282 RID: 642
	[SerializeField]
	private Image equippedReticleFill;

	// Token: 0x04000283 RID: 643
	[SerializeField]
	private Image unequippedReticleFill;

	// Token: 0x04000284 RID: 644
	[SerializeField]
	private Sprite heartSprite;

	// Token: 0x04000285 RID: 645
	[SerializeField]
	private Sprite fillSprite;

	// Token: 0x04000286 RID: 646
	[SerializeField]
	private Sprite heartSpriteSmall;

	// Token: 0x04000287 RID: 647
	[SerializeField]
	private Sprite fillSpriteSmall;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	private UIButtonAnimation phoneHUDButton;

	// Token: 0x04000289 RID: 649
	[SerializeField]
	private UIButtonAnimation glassesHUDButton;

	// Token: 0x0400028A RID: 650
	private bool noSound;

	// Token: 0x0400028B RID: 651
	private Canvas equippedReticleCanvas;

	// Token: 0x0400028C RID: 652
	private Canvas equippedReticleCanvasFill;
}
