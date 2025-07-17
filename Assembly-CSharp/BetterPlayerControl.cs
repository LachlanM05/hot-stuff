using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Rewired;
using T17.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// Token: 0x0200002A RID: 42
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BetterPlayerControl : MonoBehaviour, IReloadHandler
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x060000EF RID: 239 RVA: 0x000065E6 File Offset: 0x000047E6
	public bool crouchUnlocked
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000065E9 File Offset: 0x000047E9
	private void OnValidate()
	{
		this._rigidbody = base.GetComponent<Rigidbody>();
		this._collider = base.GetComponent<Collider>();
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00006604 File Offset: 0x00004804
	private void Awake()
	{
		if (BetterPlayerControl.Instance != null && BetterPlayerControl.Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		BetterPlayerControl.Instance = this;
		this._rigidbody = base.GetComponent<Rigidbody>();
		this._collider = base.GetComponent<Collider>();
		this.baseSpeed = this.speed;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00006660 File Offset: 0x00004860
	private void OnEnable()
	{
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00006664 File Offset: 0x00004864
	private void OnDisable()
	{
		if (this._camera.GetComponent<Camera>() != null)
		{
			Debug.LogWarning("Setting all Camera.targetTexture fields to null");
			foreach (Camera camera in base.GetComponentsInChildren<Camera>())
			{
				Debug.LogWarning("Found camera '" + camera.name + "' - resetting targetTexture");
				camera.targetTexture = null;
			}
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000066C8 File Offset: 0x000048C8
	private void Start()
	{
		this.ppv.profile.TryGet<DepthOfField>(out this.dof);
		PlayerPauser._pause += this.TogglePause;
		this.player = ReInput.players.GetPlayer(0);
		this.center = new Quaternion(0f, 0f, 0f, 1f);
		this.storedSpeed = this.speed;
		this.isGameEndingOn = false;
		this.demoEnded = false;
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			this.demoEndTime = DateTime.UtcNow.AddMinutes((double)DeluxeEditionController.DEMO_LIMIT_MINUTES).Ticks;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00006770 File Offset: 0x00004970
	public bool IsPlayerFloor(GameObject go)
	{
		RaycastHit raycastHit;
		return Physics.Raycast(new Ray(base.transform.position + Vector3.up * this.heightdist, Vector3.down), out raycastHit, this.heightdist * 2f) && raycastHit.collider.gameObject == go;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000067D0 File Offset: 0x000049D0
	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000067DC File Offset: 0x000049DC
	public void SpeedBoost()
	{
		this.speedBoost = true;
		this.storedSpeed *= 1.25f;
		this.baseSpeed = this.storedSpeed;
		if (this.crouched)
		{
			this.speed = this.storedSpeed - this.speedReduction;
		}
		else
		{
			this.speed = this.baseSpeed;
		}
		Singleton<Save>.Instance.SetPlayerSpeedFast();
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006841 File Offset: 0x00004A41
	public void ResetSpeed()
	{
		this.speed = this.baseSpeed;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00006850 File Offset: 0x00004A50
	[ContextMenu("Shrink Player")]
	public void ShrinkPlayerInternal()
	{
		this.shrinkAmount += 0.1f;
		this.cameraOffsetNode.transform.DOComplete(true);
		float num = Mathf.Clamp(this.shrinkAmount, 0f, 1f);
		num *= -1f;
		if (this.crouched)
		{
			num -= (this.colliderHeightNormal - this.colliderHeightCrouched) / this.displacementCoefficient;
		}
		this.cameraOffsetNode.transform.DOLocalMoveY(num, 0.2f, false).SetEase(Ease.InSine);
		Singleton<Save>.Instance.SetShrinkAmount(this.shrinkAmount);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000068EC File Offset: 0x00004AEC
	public void ShrinkPlayer()
	{
		bool flag;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("bug_shrink"), out flag);
		if (flag)
		{
			this.ShrinkPlayerInternal();
			return;
		}
		this.cameraOffsetNode.transform.DOComplete(true);
		float num = 0f;
		if (this.crouched)
		{
			num -= (this.colliderHeightNormal - this.colliderHeightCrouched) / this.displacementCoefficient;
		}
		this.cameraOffsetNode.transform.DOLocalMoveY(0f + num, 0.2f, false).SetEase(Ease.InSine);
		this.shrinkAmount = 0f;
		Singleton<Save>.Instance.SetShrinkAmount(0f);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x0000698F File Offset: 0x00004B8F
	private void DateNarrator()
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized("narrator_date") != RelationshipStatus.Realized)
		{
			Singleton<PhoneManager>.Instance.StartChatWithSelectedDateable("narrator_date");
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x000069B4 File Offset: 0x00004BB4
	private void Update()
	{
		BetterPlayerControl.<>c__DisplayClass76_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
		}
		if (DeluxeEditionController.IS_DEMO_EDITION && DateTime.UtcNow.Ticks > this.demoEndTime)
		{
			this.demoEnded = true;
		}
		if (!PlayerPauser.IsPaused() && !PlayerPauser.IsFrozen() && this.STATE != BetterPlayerControl.PlayerState.CantControl && !this.isGameEndingOn)
		{
			if (this.player.GetButtonDown("Interact"))
			{
				CursorLocker.Lock();
			}
		}
		else
		{
			Singleton<Dateviators>.Instance.DisableReticle();
		}
		if (Singleton<GameController>.Instance.viewState == VIEW_STATE.TALKING)
		{
			if (this.player.GetButtonDown("Message Log"))
			{
				Singleton<MessageLogManager>.Instance.SetActive(!Singleton<MessageLogManager>.Instance.isactive);
			}
			if (this.STATE == BetterPlayerControl.PlayerState.CanControl)
			{
				this.STATE = BetterPlayerControl.PlayerState.CantControl;
			}
			if (this.player.GetButtonSinglePressUp(13) && Singleton<InkController>.Instance.GetLastKnownPath() != null && Singleton<InkController>.Instance.GetLastKnownPath().StartsWith("fpn_tutorial.fpn_tutorial_door"))
			{
				Singleton<GameController>.Instance.ReturnToHouse(null);
				base.Invoke("DateNarrator", 1f);
			}
		}
		else
		{
			Singleton<MessageLogManager>.Instance.SetActive(false);
		}
		CS$<>8__locals1.collider = this._collider as CapsuleCollider;
		Vector3 localPosition = this.cameraOffsetNode.transform.localPosition;
		CS$<>8__locals1.heightDisplacement = (this.colliderHeightNormal - this.colliderHeightCrouched) / this.displacementCoefficient + this.shrinkAmount;
		CS$<>8__locals1.colliderCenter = CS$<>8__locals1.collider.center;
		switch (this.STATE)
		{
		case BetterPlayerControl.PlayerState.CanControl:
			if (DeluxeEditionController.IS_DEMO_EDITION && this.demoEnded && !this.demoEndedPopupShown)
			{
				this.demoEndedPopupShown = true;
				UnityEvent unityEvent = new UnityEvent();
				unityEvent.AddListener(new UnityAction(Singleton<PhoneManager>.Instance.GoToDemoMenu));
				Singleton<Popup>.Instance.CreatePopup("Date Everything", "Unfortunately all good dates must come to an end. Please wish list or tell your friends about your experience!", unityEvent, false);
			}
			if ((this.player.GetButtonDown(5) || this.player.GetButtonDown(29)) && !this.isGameEndingOn && !Singleton<PhoneManager>.Instance.IsPhoneAnimating() && Singleton<PhoneManager>.Instance.CanCloseCommApp() && !Singleton<PhoneManager>.Instance.BlockPhoneOpening)
			{
				if (!Singleton<PhoneManager>.Instance.IsPhoneMenuOpened())
				{
					Singleton<PhoneManager>.Instance.OpenPhoneAsync(null);
					UnityEvent<string> buttonPressed = this.ButtonPressed;
					if (buttonPressed != null)
					{
						buttonPressed.Invoke("Phone");
					}
				}
				else
				{
					Singleton<PhoneManager>.Instance.ClosePhoneAsync(null, false);
					UnityEvent<string> buttonPressed2 = this.ButtonPressed;
					if (buttonPressed2 != null)
					{
						buttonPressed2.Invoke("Phone");
					}
				}
			}
			if (!Singleton<PhoneManager>.Instance.openRequested && !Singleton<PhoneManager>.Instance.forceCloseRequested)
			{
				if (this.player.GetButtonDown(52) && !Singleton<Dateviators>.Instance.Equipped && !this.isGameEndingOn && this.mouse0holdtime == 0f && !Singleton<PhoneManager>.Instance.IsPhoneAnimating())
				{
					Singleton<Dateviators>.Instance.Equip();
					this.mouse0holdtime = 0f;
				}
				else if (this.player.GetButtonDown(52) && Singleton<Dateviators>.Instance.Equipped && !this.isGameEndingOn && this.mouse0holdtime == 0f && !Singleton<PhoneManager>.Instance.IsPhoneAnimating())
				{
					Singleton<Dateviators>.Instance.Dequip();
					this.beamsSupressed = false;
					this.mouse0holdtime = 0f;
				}
			}
			if (Singleton<Dateviators>.Instance.Equipped && !Singleton<Dateviators>.Instance.PostProcessOn)
			{
				Singleton<Dateviators>.Instance.ForcePostProcessVolumeTo1();
			}
			if (this.crouchUnlocked && !this.isGameEndingOn)
			{
				if (this.player.GetButtonDown(33))
				{
					this.crouchToggleInputRequestedState = !this.crouchToggleInputRequestedState;
				}
				if (this.crouched && this.IsInputRequestingUnCrouch() && this.<Update>g__CanUncrouch|76_1(ref CS$<>8__locals1))
				{
					this.<Update>g__Uncrouch|76_2(ref CS$<>8__locals1);
				}
				else if (!this.crouched && this.IsInputRequestingCrouch())
				{
					this.<Update>g__Crouch|76_0(ref CS$<>8__locals1);
				}
			}
			break;
		case BetterPlayerControl.PlayerState.CantMove:
		{
			Popup instance = Singleton<Popup>.Instance;
			bool flag = instance.IsPopupOpen() && !instance.canAwakenTbc && !Singleton<Dateviators>.Instance.CanConsumeCharge();
			PhoneManager instance2 = Singleton<PhoneManager>.Instance;
			bool flag2 = instance2.IsPhoneAnimating();
			if (instance2.IsPhoneMenuOpened() && !flag2 && !this.isGameEndingOn && !flag && !FrontDoor.Instance.inNarratorConversation)
			{
				if (this.player.GetButtonDown(52) && Singleton<Save>.Instance.GetDateStatus("skylar_specs") != RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatusRealized("skylar_specs") != RelationshipStatus.Realized && !this.isGameEndingOn && !Singleton<PhoneManager>.Instance.openRequested && !Singleton<PhoneManager>.Instance.forceCloseRequested && !Singleton<PhoneManager>.Instance.IsPhoneAppOpened() && this.mouse0holdtime == 0f)
				{
					Singleton<PhoneManager>.Instance.ToggleGlasses();
				}
				if (Singleton<Dateviators>.Instance.Equipped)
				{
					this.beamsSupressed = false;
					Singleton<Dateviators>.Instance.ShowReticle();
					float num = Mathf.InverseLerp(0f, 2f, this.mouse0holdtime - this.timeToStartStream);
					this.HandleChargingUp(num, null, true);
					this.HandleStopChargingUp(null, true);
					this.HandleChargingUpCompleted(num, null, true);
				}
				if (this.player.GetButton(13) && !CinematicBars.IsCinematicBarsOn())
				{
					this.mouse0holdtime += Time.deltaTime;
					return;
				}
				this.mouse0holdtime = 0f;
				return;
			}
			else
			{
				if (!FrontDoor.Instance.inNarratorConversation)
				{
					this.mouse0holdtime = 0f;
					Singleton<Dateviators>.Instance.DisableReticle();
					return;
				}
				this.beamsSupressed = false;
				Singleton<Dateviators>.Instance.ShowReticle();
				float num2 = Mathf.InverseLerp(0f, 2f, this.mouse0holdtime - this.timeToStartStream);
				this.HandleChargingUp(num2, null, false);
				this.HandleStopChargingUp(null, false);
				this.HandleChargingUpCompleted(num2, null, false);
				if (this.player.GetButton(13) && !CinematicBars.IsCinematicBarsOn())
				{
					this.mouse0holdtime += Time.deltaTime;
					return;
				}
				this.mouse0holdtime = 0f;
				return;
			}
			break;
		}
		case BetterPlayerControl.PlayerState.CantControl:
			if (Singleton<Dateviators>.Instance.Equipped)
			{
				this.StopBeamSounds();
			}
			this.mouse0holdtime = 0f;
			return;
		}
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
			this.move = Vector3.zero;
		}
		else
		{
			int @int = Services.GameSettings.GetInt("invertYAxis", 0);
			int num3 = 1;
			if (@int == 1)
			{
				num3 = -1;
			}
			this.move = new Vector3(this.player.GetAxis(3), 0f, this.player.GetAxis(4));
			this.look = new Vector3(this.player.GetAxis(9), 0f, this.player.GetAxis(10) * (float)num3);
			if (this.look == Vector3.zero)
			{
				this.camerarotAcceleration = ((!Services.InputService.WasLastControllerAPointer()) ? 0.15f : 1f);
			}
			else
			{
				this.camerarotAcceleration += Time.deltaTime * 2f;
				this.camerarotAcceleration = Mathf.Clamp(this.camerarotAcceleration, 0f, 1f);
			}
			this.look *= this.camerarotAcceleration;
		}
		bool flag3 = false;
		InteractableObj interactableObj = null;
		float num4 = this.mouse0holdtime;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(this._camera.transform.position - this._camera.transform.forward * 0.25f, this._camera.transform.forward), out raycastHit, float.PositiveInfinity, ~this.dateviatorIgnores) && this.STATE == BetterPlayerControl.PlayerState.CanControl)
		{
			this.dof.focusDistance.Override(Vector3.Distance(raycastHit.point, this._camera.transform.position));
			InteractableObj interactableObj2;
			if (raycastHit.transform.TryGetComponent<InteractableObj>(out interactableObj2) && (!Singleton<Dateviators>.Instance.IsEquipped || !interactableObj2.IsRealized))
			{
				interactableObj = interactableObj2;
				if (Vector3.Distance(raycastHit.collider.ClosestPointOnBounds(this._camera.transform.position), this._camera.transform.position) < interactableObj.InteractionRadius)
				{
					flag3 = true;
					if (interactableObj2.inkFileName != "doug_dread")
					{
						Singleton<Dateviators>.Instance.ShowReticle();
					}
				}
				if (interactableObj != null && interactableObj.AlternateInteractions.Count > 0 && flag3)
				{
					if (this.lastExamined != null && this.lastExamined.AlternateInteractions.Count > 0 && interactableObj.AlternateInteractions.Count > 0)
					{
						ObjectExamine objectExamine;
						if (this.lastExamined.AlternateInteractions[0] != null && interactableObj.AlternateInteractions[0] != null && this.lastExamined.AlternateInteractions[0].TryGetComponent<ObjectExamine>(out objectExamine) && this.lastExamined.AlternateInteractions[0].GetComponent<ObjectExamine>() != interactableObj.AlternateInteractions[0].GetComponent<ObjectExamine>())
						{
							Singleton<ExamineController>.Instance.HideExamine();
							this.mouse0holdtime = 0f;
							if (this.player.GetButton(13) && Singleton<Dateviators>.Instance.Equipped)
							{
								Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateviator_character_rise, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, SFX_SUBGROUP.UI, false);
							}
						}
					}
					else
					{
						Singleton<ExamineController>.Instance.HideExamine();
						this.mouse0holdtime = 0f;
					}
				}
			}
			else
			{
				if (this.lastExamined != null)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dateviator_character_rise.name, 0.1f);
					this.lastExamined = null;
					this.mouse0holdtime = 0f;
				}
				if (Singleton<ExamineController>.Instance.isShown)
				{
					Singleton<ExamineController>.Instance.HideExamine();
				}
				if (this.STATE == BetterPlayerControl.PlayerState.CanControl)
				{
					Singleton<Dateviators>.Instance.HideReticle();
				}
			}
			Singleton<DougLogic>.Instance.SetLookingAtWall(raycastHit.transform.GetComponent<DougScript>() != null);
			this.lastExamined = interactableObj;
		}
		else
		{
			if (this.lastExamined != null)
			{
				this.lastExamined = null;
				this.mouse0holdtime = 0f;
				Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dateviator_character_rise.name, 0.1f);
			}
			Singleton<ExamineController>.Instance.HideExamine();
			Singleton<DougLogic>.Instance.SetLookingAtWall(false);
			if (this.STATE == BetterPlayerControl.PlayerState.CanControl)
			{
				Singleton<Dateviators>.Instance.HideReticle();
			}
		}
		if (Singleton<Dateviators>.Instance.Equipped && this.STATE == BetterPlayerControl.PlayerState.CanControl && !this.isGameEndingOn)
		{
			float num5 = Mathf.InverseLerp(0f, 2f, this.mouse0holdtime - this.timeToStartStream);
			this.HandleChargingUp(num5, interactableObj, false);
			this.HandleStopChargingUp(interactableObj, false);
			this.HandleChargingUpCompleted(num5, interactableObj, false);
			if (this.player.GetButtonDown(12))
			{
				Singleton<InteractableManager>.Instance.Interact(true, true);
			}
		}
		else
		{
			if (this.stream.isPlaying && !FrontDoor.Instance.inNarratorConversation)
			{
				VFXEvents.Instance.ResetOutline();
				Singleton<PhoneManager>.Instance.ResetOutline();
				this.StopBeamSounds();
				this.mouse0holdtime = 0f;
				this.stream.Stop();
			}
			if (this.fullheart.isPlaying)
			{
				this.fullheart.Stop();
			}
			if (this.player.GetButtonDown(12) && this.STATE == BetterPlayerControl.PlayerState.CanControl && !this.isGameEndingOn && flag3)
			{
				Singleton<InteractableManager>.Instance.Interact(false, true);
				UnityEvent<string> buttonPressed3 = this.ButtonPressed;
				if (buttonPressed3 != null)
				{
					buttonPressed3.Invoke("Interact");
				}
			}
		}
		if (!flag3 && this.STATE == BetterPlayerControl.PlayerState.CanControl)
		{
			Singleton<Dateviators>.Instance.HideReticle();
		}
		if (this.player.GetButtonDown(18) && this.STATE == BetterPlayerControl.PlayerState.CanControl && !this.isGameEndingOn)
		{
			if (Singleton<ExamineController>.Instance.isShown)
			{
				Singleton<ExamineController>.Instance.HideExamine();
			}
			else if (Singleton<InteractableManager>.Instance.activeObject && flag3)
			{
				ObjectExamine objectExamine2;
				Singleton<InteractableManager>.Instance.activeObject.TryGetComponent<ObjectExamine>(out objectExamine2);
				if (objectExamine2 == null)
				{
					InteractableObj interactableObj3;
					Singleton<InteractableManager>.Instance.activeObject.TryGetComponent<InteractableObj>(out interactableObj3);
					if (interactableObj3 != null && interactableObj3.AlternateInteractions.Count > 0)
					{
						interactableObj3.gameObject.TryGetComponent<ObjectExamine>(out objectExamine2);
						if (objectExamine2 == null)
						{
							interactableObj3.AlternateInteractions[0].gameObject.TryGetComponent<ObjectExamine>(out objectExamine2);
						}
						this.lastExamined = interactableObj3;
					}
				}
				if (objectExamine2 != null)
				{
					Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_examine.name, 0.1f);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_examine, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, SFX_SUBGROUP.UI, false);
					objectExamine2.ShowExamine();
					UnityEvent<string> buttonPressed4 = this.ButtonPressed;
					if (buttonPressed4 != null)
					{
						buttonPressed4.Invoke("Examine");
					}
				}
			}
		}
		Singleton<InteractableManager>.Instance.UpdatePlayerValues(num4 > 0f, flag3);
		if (interactableObj != Singleton<InteractableManager>.Instance.activeObject)
		{
			Singleton<InteractableManager>.Instance.UpdatePlayerTarget(interactableObj);
		}
		if (!Singleton<Dateviators>.Instance.Equipped && this.STATE != BetterPlayerControl.PlayerState.CanControl)
		{
			return;
		}
		if (this.player.GetButton(13) && ((raycastHit.transform != null && !this.isGameEndingOn) || FrontDoor.Instance.inNarratorConversation) && !CinematicBars.IsCinematicBarsOn())
		{
			this.mouse0holdtime += Time.deltaTime;
			InteractableObj interactableObj4;
			if (raycastHit.transform.TryGetComponent<InteractableObj>(out interactableObj4) && !interactableObj4.IsRealized)
			{
				interactableObj4.DoMagicWiggle();
				if (interactableObj4.linkedWigglers.Count > 0 && interactableObj4.linkedWigglers != null)
				{
					interactableObj4.linkedWigglers.ForEach(delegate(InteractableObj wiggler)
					{
						if (wiggler != null)
						{
							wiggler.DoMagicWiggle();
						}
					});
					return;
				}
			}
		}
		else
		{
			this.mouse0holdtime = 0f;
			this.beamsSupressed = false;
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000077EC File Offset: 0x000059EC
	private void HandleChargingUp(float normalizedFillAmount, InteractableObj hitInteractable, bool isPhone)
	{
		if (!isPhone && FrontDoor.Instance.inNarratorConversation && (hitInteractable == null || !Singleton<InteractableManager>.Instance.IsPlayerInRange))
		{
			normalizedFillAmount = 0f;
		}
		if (hitInteractable != null && (hitInteractable.inkFileName == "doug_dread" || (Singleton<Dateviators>.Instance.IsEquipped && hitInteractable.IsRealized)))
		{
			normalizedFillAmount = 0f;
			this.mouse0holdtime = 0f;
			Singleton<Dateviators>.Instance.HideReticle();
		}
		this.fillupim.fillAmount = normalizedFillAmount;
		this.fillupim.color = ((normalizedFillAmount > 1f) ? new Color(31f, 83f, 100f) : Color.white);
		if (this.mouse0holdtime > 0f && this.player.GetButton(13) && (!this.beamsSupressed || FrontDoor.Instance.inNarratorConversation))
		{
			float num = Mathf.InverseLerp(0f, 1f, normalizedFillAmount);
			if (isPhone)
			{
				Singleton<PhoneManager>.Instance.UpdateOutline(num);
			}
			else
			{
				VFXEvents.Instance.UpdateOutline(num);
			}
			UnityEvent<string> buttonPressed = this.ButtonPressed;
			if (buttonPressed != null)
			{
				buttonPressed.Invoke("Interact");
			}
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateviator_beam, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, true, SFX_SUBGROUP.UI, false);
		}
		if (Services.GameSettings.GetInt("oneClickInteract") == 1 && this.player.GetButtonDown(13) && !CinematicBars.IsCinematicBarsOn())
		{
			this.fullheart.Play();
			if (!isPhone)
			{
				Singleton<InteractableManager>.Instance.Interact(false, false);
				VFXEvents.Instance.ResetOutline();
			}
			Singleton<PhoneManager>.Instance.ResetOutline();
		}
		if (this.mouse0holdtime > this.timeToStartStream)
		{
			if (normalizedFillAmount < 1f)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateviator_heart_rise, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
			if ((hitInteractable != null || isPhone || FrontDoor.Instance.inNarratorConversation) && ((normalizedFillAmount > 0f && normalizedFillAmount < 1f) || FrontDoor.Instance.inNarratorConversation))
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateviator_character_rise, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
			if (!this.playingStream && !FrontDoor.Instance.inNarratorConversation)
			{
				this.playingStream = true;
				ParticleSystem.MainModule main = this.stream.main;
				if (!isPhone && hitInteractable != null && hitInteractable.internalCharacterName != null && Singleton<Save>.Instance.GetDateStatusRealized(hitInteractable.internalCharacterName) == RelationshipStatus.Realized)
				{
					main.maxParticles = 500;
				}
				else if (isPhone)
				{
					bool flag = Singleton<PhoneManager>.Instance.IsCreditsScreenOn();
					bool flag2 = Singleton<ChatMaster>.Instance.IsWrkspceOpened();
					bool flag3 = Singleton<Popup>.Instance.IsPopupOpen() && Singleton<Popup>.Instance.canAwakenTbc;
					if (flag && Singleton<Save>.Instance.GetDateStatusRealized("sassy") == RelationshipStatus.Realized)
					{
						main.maxParticles = 500;
					}
					else if (flag2 && Singleton<Save>.Instance.GetDateStatusRealized("willi") == RelationshipStatus.Realized)
					{
						main.maxParticles = 500;
					}
					else if (flag3 && Singleton<Save>.Instance.GetDateStatusRealized("tbc") == RelationshipStatus.Realized)
					{
						main.maxParticles = 500;
					}
					else
					{
						main.maxParticles = 1000;
					}
				}
				else if (Singleton<Dateviators>.Instance.CanConsumeCharge())
				{
					main.maxParticles = 1000;
				}
				else
				{
					main.maxParticles = 1;
				}
				this.stream.Play();
			}
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00007B70 File Offset: 0x00005D70
	private void HandleStopChargingUp(InteractableObj hitInteractable, bool isPhone)
	{
		if (this.player.GetButtonUp(13))
		{
			this.StopBeamSounds();
			if (hitInteractable != null || isPhone || FrontDoor.Instance.inNarratorConversation)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.dateviator_beam_cancel_focused, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
			else
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.dateviator_beam_cancel, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
			this.stream.Stop();
			this.playingStream = false;
			this.mouse0holdtime = 0f;
			VFXEvents.Instance.ResetOutline();
			Singleton<PhoneManager>.Instance.ResetOutline();
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00007C2C File Offset: 0x00005E2C
	private void HandleChargingUpCompleted(float normalizedFillAmount, InteractableObj hitInteractable, bool isPhone)
	{
		bool flag = false;
		MenuComponent menuComponent = null;
		bool flag2 = Singleton<Dateviators>.Instance.CanConsumeCharge();
		if (Services.GameSettings.GetInt("oneClickInteract") == 1 && this.player.GetButtonDown(13) && !CinematicBars.IsCinematicBarsOn())
		{
			flag = true;
		}
		if (this.player.GetButton(13) && normalizedFillAmount >= 1f)
		{
			if (hitInteractable != null)
			{
				flag = Singleton<InteractableManager>.Instance.IsPlayerInRange;
			}
			if (isPhone)
			{
				menuComponent = Singleton<CanvasUIManager>.Instance.FindMenuComponent("Phone Menu");
				flag = !(menuComponent == null) && menuComponent.GetComponent<PauseScreen>().CheckAwakenInPhone(true);
				if (!flag && !flag2)
				{
					flag = true;
				}
			}
			if (FrontDoor.Instance.inNarratorConversation)
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.StopBeamSounds();
			this.stream.Stop();
			this.fullheart.Play();
			this.playingStream = false;
			VFXEvents.Instance.ResetOutline();
			Singleton<PhoneManager>.Instance.ResetOutline();
			if (isPhone && !flag2)
			{
				Singleton<Popup>.Instance.CreatePopup("Oops", "You need to use a charge to awaken this object, but the Dateviators are out of battery. Sleep to recharge the battery!", false);
			}
			else if (Singleton<PhoneManager>.Instance.IsPhoneMenuOpened())
			{
				if (menuComponent == null)
				{
					menuComponent = Singleton<CanvasUIManager>.Instance.FindMenuComponent("Phone Menu");
				}
				if (menuComponent != null)
				{
					menuComponent.GetComponent<PauseScreen>().CheckAwakenInPhone(false);
				}
			}
			else
			{
				if (hitInteractable != null)
				{
					Singleton<InteractableManager>.Instance.Interact(false, false);
				}
				if (FrontDoor.Instance.inNarratorConversation)
				{
					Singleton<InkController>.Instance.JumpToKnot("fpn_tutorial.narrator_date");
					TalkingUI.Instance.ContinuePressed();
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_transition_character, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.STINGER, false);
					Singleton<GameController>.Instance.SetDoNotPostProcessOnReturn();
					FrontDoor.Instance.inNarratorConversation = false;
				}
			}
			this.mouse0holdtime = 0f;
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00007DFC File Offset: 0x00005FFC
	public void StopBeamSounds()
	{
		this.beamsSupressed = true;
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dateviator_heart_rise.name, 0.1f);
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dateviator_character_rise.name, 0.1f);
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.dateviator_beam_cancel_focused.name, 0.1f);
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.dateviator_beam_cancel.name, 0.1f);
		Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_dateviator_beam.name, 0.1f);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00007EA6 File Offset: 0x000060A6
	public void ChangePlayerState(BetterPlayerControl.PlayerState newState)
	{
		this.STATE = newState;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00007EB0 File Offset: 0x000060B0
	private void FixedUpdate()
	{
		if (this.STATE != BetterPlayerControl.PlayerState.CanControl)
		{
			this.stepAnims.SetBool("Walking", false);
			this._rigidbody.isKinematic = true;
			if (this.stream.isPlaying)
			{
				this.stream.Stop();
			}
			if (this.fullheart.isPlaying)
			{
				this.fullheart.Stop();
			}
			return;
		}
		base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
		this._rigidbody.isKinematic = false;
		this._rigidbody.velocity = Vector3.zero;
		bool flag = this.move.magnitude >= 0.2f;
		this.stepAnims.SetBool("Walking", flag && this.grounded);
		Vector3 vector = base.transform.forward * this.move.z + base.transform.right * this.move.x;
		vector = vector.normalized * Mathf.Clamp(vector.magnitude, 0f, 1f);
		Vector3 vector2;
		if (flag)
		{
			this._timemoving += Time.fixedDeltaTime;
			this._timedeceling = 0f;
			vector2 = this.speed * vector;
		}
		else
		{
			this._timedeceling += Time.fixedDeltaTime;
			this._timemoving = 0f;
			vector2 = this._rigidbody.velocity * this.decceleration.Evaluate(this._timedeceling);
		}
		this.camerarotspeed = Services.GameSettings.GetFloat("cameraSENSITIVITY", 1f) * this.camerarotmultiplier;
		this._rigidbody.DORotate(base.GetComponent<Rigidbody>().rotation.eulerAngles + new Vector3(0f, this.look.x, 0f) * this.camerarotspeed * Time.fixedDeltaTime, this.easingTime, RotateMode.Fast).SetEase(this.easingFunction);
		Quaternion quaternion = Quaternion.AngleAxis(this.look.z * this.camerarotspeed * Time.fixedDeltaTime, -Vector3.right);
		Quaternion quaternion2 = this._camera.transform.localRotation * quaternion;
		if (Quaternion.Angle(this.center, quaternion2) < this.maxrot)
		{
			this._camera.transform.DOLocalRotateQuaternion(quaternion2, this.easingTime).SetEase(this.easingFunction);
		}
		float num = this._rigidbody.velocity.y;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(base.transform.position + Vector3.up * this.heightdist, Vector3.down), out raycastHit, this.heightdist * 2f))
		{
			this.grounded = true;
			num = (raycastHit.point.y - base.transform.position.y) * this.shiftforce;
		}
		else
		{
			this.grounded = false;
			num = -50f;
		}
		vector2 = new Vector3(vector2.x, num, vector2.z);
		this._rigidbody.velocity = Vector3.Lerp(this._rigidbody.velocity, vector2, 0.5f);
		this._currentSpeed = this._rigidbody.velocity.magnitude;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x0000823F File Offset: 0x0000643F
	public void SetCameraRotation(Quaternion quat, float easingTime, Ease easingFunction)
	{
		this._camera.transform.DOLocalRotateQuaternion(quat, easingTime).SetEase(easingFunction);
	}

	// Token: 0x06000104 RID: 260 RVA: 0x0000825A File Offset: 0x0000645A
	public void TogglePause(bool paused)
	{
		if (paused)
		{
			this.STATE = BetterPlayerControl.PlayerState.CantMove;
			return;
		}
		this.STATE = BetterPlayerControl.PlayerState.CanControl;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00008270 File Offset: 0x00006470
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(base.transform.position + Vector3.up * this.heightdist, base.transform.position + Vector3.down * this.heightdist);
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000082CC File Offset: 0x000064CC
	private bool IsInputRequestingUnCrouch()
	{
		BetterPlayerControl.InputMode inputMode = this.crouchInputMode;
		if (inputMode == BetterPlayerControl.InputMode.Toggle)
		{
			return !this.crouchToggleInputRequestedState;
		}
		if (inputMode != BetterPlayerControl.InputMode.Hold)
		{
			throw new ArgumentOutOfRangeException();
		}
		return !this.player.GetButton(33);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x0000830C File Offset: 0x0000650C
	private bool IsInputRequestingCrouch()
	{
		if (this.shrinkAmount > (this.colliderHeightNormal - this.colliderHeightCrouched) / this.displacementCoefficient)
		{
			return false;
		}
		BetterPlayerControl.InputMode inputMode = this.crouchInputMode;
		if (inputMode == BetterPlayerControl.InputMode.Toggle)
		{
			return this.crouchToggleInputRequestedState;
		}
		if (inputMode != BetterPlayerControl.InputMode.Hold)
		{
			throw new ArgumentOutOfRangeException();
		}
		return this.player.GetButton(33);
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00008364 File Offset: 0x00006564
	public void Move(Vector3 newPosition, Quaternion newOrientation)
	{
		this._rigidbody.velocity = Vector3.zero;
		bool isKinematic = this._rigidbody.isKinematic;
		this._rigidbody.isKinematic = true;
		this._rigidbody.position = newPosition;
		this._rigidbody.rotation = newOrientation;
		this._rigidbody.isKinematic = isKinematic;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x000083BD File Offset: 0x000065BD
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x000083C4 File Offset: 0x000065C4
	public void LoadState()
	{
		this.ShrinkPlayer();
		if (Singleton<Save>.Instance.speedBoost)
		{
			this.SpeedBoost();
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x000083DE File Offset: 0x000065DE
	public void SaveState()
	{
		if (this.speedBoost)
		{
			Singleton<Save>.Instance.SetPlayerSpeedFast();
		}
		else
		{
			Singleton<Save>.Instance.SetPlayerSpeedNormal();
		}
		Singleton<Save>.Instance.SetShrinkAmount(this.shrinkAmount);
	}

	// Token: 0x0600010D RID: 269 RVA: 0x000084D0 File Offset: 0x000066D0
	[CompilerGenerated]
	private void <Update>g__Crouch|76_0(ref BetterPlayerControl.<>c__DisplayClass76_0 A_1)
	{
		this.cameraOffsetNode.transform.DOComplete(true);
		A_1.collider.height = this.colliderHeightCrouched;
		A_1.collider.center = new Vector3(A_1.colliderCenter.x, A_1.colliderCenter.y - A_1.heightDisplacement, A_1.colliderCenter.z);
		A_1.heightDisplacement -= this.shrinkAmount;
		this.cameraOffsetNode.transform.DOLocalMoveY(-A_1.heightDisplacement, 0.2f, false).SetEase(Ease.InSine);
		this.crouched = true;
		this.speed = this.storedSpeed - this.speedReduction;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x0000858C File Offset: 0x0000678C
	[CompilerGenerated]
	private bool <Update>g__CanUncrouch|76_1(ref BetterPlayerControl.<>c__DisplayClass76_0 A_1)
	{
		return !Physics.Raycast(base.transform.TransformPoint(A_1.colliderCenter + Vector3.up * A_1.heightDisplacement), Vector3.up, this.colliderHeightNormal / 1.5f);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x000085DC File Offset: 0x000067DC
	[CompilerGenerated]
	private void <Update>g__Uncrouch|76_2(ref BetterPlayerControl.<>c__DisplayClass76_0 A_1)
	{
		this.cameraOffsetNode.transform.DOComplete(true);
		A_1.collider.height = this.colliderHeightNormal;
		A_1.collider.center = new Vector3(A_1.colliderCenter.x, A_1.colliderCenter.y + A_1.heightDisplacement, A_1.colliderCenter.z);
		this.cameraOffsetNode.transform.DOLocalMoveY(0f - this.shrinkAmount, 0.2f, false).SetEase(Ease.InSine);
		this.crouched = false;
		this.speed = this.storedSpeed;
	}

	// Token: 0x040000E9 RID: 233
	public static BetterPlayerControl Instance;

	// Token: 0x040000EA RID: 234
	public bool isGameEndingOn;

	// Token: 0x040000EB RID: 235
	public BetterPlayerControl.PlayerState STATE;

	// Token: 0x040000EC RID: 236
	[Header("Settings")]
	public AnimationCurve acceleration;

	// Token: 0x040000ED RID: 237
	public AnimationCurve decceleration;

	// Token: 0x040000EE RID: 238
	public float _currentSpeed;

	// Token: 0x040000EF RID: 239
	public float speed;

	// Token: 0x040000F0 RID: 240
	private float baseSpeed;

	// Token: 0x040000F1 RID: 241
	[Header("Interactable")]
	public ParticleSystem stream;

	// Token: 0x040000F2 RID: 242
	public ParticleSystem fullheart;

	// Token: 0x040000F3 RID: 243
	public Image fillupim;

	// Token: 0x040000F4 RID: 244
	[SerializeField]
	private InteractableObj lastExamined;

	// Token: 0x040000F5 RID: 245
	[Header("GroundClamp")]
	public float heightdist = 0.25f;

	// Token: 0x040000F6 RID: 246
	public float shiftforce = 2f;

	// Token: 0x040000F7 RID: 247
	public float shrinkAmount;

	// Token: 0x040000F8 RID: 248
	private Rigidbody _rigidbody;

	// Token: 0x040000F9 RID: 249
	private Collider _collider;

	// Token: 0x040000FA RID: 250
	[Header("Camera")]
	public Animator stepAnims;

	// Token: 0x040000FB RID: 251
	public GameObject _camera;

	// Token: 0x040000FC RID: 252
	public GameObject cameraOffsetNode;

	// Token: 0x040000FD RID: 253
	public Volume ppv;

	// Token: 0x040000FE RID: 254
	private DepthOfField dof;

	// Token: 0x040000FF RID: 255
	public float interactionradius = 5f;

	// Token: 0x04000100 RID: 256
	public float camerarotspeed;

	// Token: 0x04000101 RID: 257
	public float camerarotAcceleration = 1f;

	// Token: 0x04000102 RID: 258
	public float camerarotmultiplier = 50f;

	// Token: 0x04000103 RID: 259
	public float maxrot = 70f;

	// Token: 0x04000104 RID: 260
	public float minrot = -70f;

	// Token: 0x04000105 RID: 261
	public float easingTime = 0.03f;

	// Token: 0x04000106 RID: 262
	public Ease easingFunction = Ease.InOutSine;

	// Token: 0x04000107 RID: 263
	public LayerMask dateviatorIgnores;

	// Token: 0x04000108 RID: 264
	private float _timemoving;

	// Token: 0x04000109 RID: 265
	private float _timedeceling;

	// Token: 0x0400010A RID: 266
	private Vector3 look;

	// Token: 0x0400010B RID: 267
	private Vector3 move;

	// Token: 0x0400010C RID: 268
	private Vector3 lookControllerValue;

	// Token: 0x0400010D RID: 269
	private float roty;

	// Token: 0x0400010E RID: 270
	private Quaternion center;

	// Token: 0x0400010F RID: 271
	private float mouse0holdtime;

	// Token: 0x04000110 RID: 272
	private bool grounded;

	// Token: 0x04000111 RID: 273
	private float timecharging;

	// Token: 0x04000112 RID: 274
	private bool beamsSupressed;

	// Token: 0x04000113 RID: 275
	private bool speedBoost;

	// Token: 0x04000114 RID: 276
	[Header("Crouch")]
	[SerializeField]
	public bool crouched;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	private BetterPlayerControl.InputMode crouchInputMode;

	// Token: 0x04000116 RID: 278
	[SerializeField]
	private float speedReduction = 2f;

	// Token: 0x04000117 RID: 279
	private float storedSpeed;

	// Token: 0x04000118 RID: 280
	[SerializeField]
	[Range(1f, 2f)]
	private float displacementCoefficient = 2f;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	private float colliderHeightNormal = 2.5f;

	// Token: 0x0400011A RID: 282
	[SerializeField]
	[Range(1f, 2.5f)]
	private float colliderHeightCrouched = 1f;

	// Token: 0x0400011B RID: 283
	private long demoEndTime;

	// Token: 0x0400011C RID: 284
	private bool demoEnded;

	// Token: 0x0400011D RID: 285
	private bool demoEndedPopupShown;

	// Token: 0x0400011E RID: 286
	private const string CAMERA_SENSITIVITY = "cameraSENSITIVITY";

	// Token: 0x0400011F RID: 287
	public UnityEvent<string> ButtonPressed = new UnityEvent<string>();

	// Token: 0x04000120 RID: 288
	[Header("Rewired")]
	private string playerID = "0";

	// Token: 0x04000121 RID: 289
	private Player player;

	// Token: 0x04000122 RID: 290
	private bool playingStream;

	// Token: 0x04000123 RID: 291
	private float timeToStartStream = 0.25f;

	// Token: 0x04000124 RID: 292
	private bool crouchToggleInputRequestedState;

	// Token: 0x0200029F RID: 671
	public enum PlayerState
	{
		// Token: 0x0400107E RID: 4222
		CanControl,
		// Token: 0x0400107F RID: 4223
		CantMove,
		// Token: 0x04001080 RID: 4224
		CantControl
	}

	// Token: 0x020002A0 RID: 672
	public enum InputMode
	{
		// Token: 0x04001082 RID: 4226
		Toggle,
		// Token: 0x04001083 RID: 4227
		Hold
	}
}
