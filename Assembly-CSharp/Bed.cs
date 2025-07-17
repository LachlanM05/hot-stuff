using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000072 RID: 114
public class Bed : Interactable
{
	// Token: 0x060003CE RID: 974 RVA: 0x00017E66 File Offset: 0x00016066
	public override string noderequired()
	{
		return "";
	}

	// Token: 0x060003CF RID: 975 RVA: 0x00017E6D File Offset: 0x0001606D
	public void Start()
	{
		this.playerControl = Object.FindObjectOfType<BetterPlayerControl>();
		this.interactable.interruptable = true;
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x00017E88 File Offset: 0x00016088
	public override void Interact()
	{
		if (BetterPlayerControl.Instance.STATE == BetterPlayerControl.PlayerState.CantControl)
		{
			return;
		}
		if (!Singleton<TutorialController>.Instance.CanGoToBed())
		{
			Singleton<Popup>.Instance.CreatePopup("", "It's not time for sleep yet...", true);
			return;
		}
		if (Singleton<Dateviators>.Instance.CanConsumeCharge() && !Singleton<Save>.Instance.GetTutorialThresholdState(Bed.BED_CAN_SLEEP_ANYTIME))
		{
			Singleton<Popup>.Instance.CreatePopup("", "It's not time for sleep yet...", true);
			return;
		}
		if (Singleton<Dateviators>.Instance.CanConsumeCharge())
		{
			UnityEvent unityEvent = new UnityEvent();
			unityEvent.AddListener(new UnityAction(this.FinishChecking));
			UnityEvent unityEvent2 = new UnityEvent();
			Singleton<Popup>.Instance.CreatePopup("Sure?", "Are you sure you want to sleep? You have charges remaining", unityEvent, unityEvent2, true);
			return;
		}
		this.FinishChecking();
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00017F40 File Offset: 0x00016140
	public void HadCoffee()
	{
		if (Singleton<DayNightCycle>.Instance.CurrentDayPhase == DayPhase.MIDNIGHT)
		{
			Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.STATE_DRANK_COFFEE);
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00017F60 File Offset: 0x00016160
	public void Sleep()
	{
		if (this.playingAnimation)
		{
			this.interactable.SpeedUpAnimation();
		}
		GhostController ghostController = Object.FindFirstObjectByType<GhostController>();
		if (ghostController != null)
		{
			ghostController.CheckTime();
		}
		Singleton<Save>.Instance.SetTutorialFinished(true);
		MovingDateable.MoveDateable("MovingFrontDoor", "default", true);
		Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.FIVE_PER_CENT_FOR_NOTHING);
		int nextAnimation = this.GetNextAnimation();
		Singleton<MorningRoutine>.Instance.setplayerpos(this.playerControl.gameObject);
		this.playingAnimation = true;
		this.SetBedroomCurtains(false);
		if (nextAnimation != 0)
		{
			if (nextAnimation == 1)
			{
				this.GoToBedAnimation1_Step1();
			}
		}
		else
		{
			this.GoToBedAnimation0_Step1();
		}
		PlayerPauser.Pause();
		CursorLocker.Lock();
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00018008 File Offset: 0x00016208
	private int GetNextAnimation()
	{
		return 0;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001800C File Offset: 0x0001620C
	private void GoToBedAnimation0_Step1()
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_sleep, AUDIO_TYPE.SFX, true, true, 0f, false, 1f, null, false, SFX_SUBGROUP.STINGER, false);
		this.nightBlackScreenTransition.SetActive(true);
		this.blackNightImage = this.nightBlackScreenTransition.GetComponent<Image>();
		this.transparentColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 0f);
		this.solidColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 1f);
		this.blackNightImage.color = this.transparentColor;
		DOTween.Sequence().Append(this.blackNightImage.DOBlendableColor(this.solidColor, 1.2f)).SetRelative<Sequence>()
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				base.Invoke("GoToBedAnimation0_Step2", 2f);
			});
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001812A File Offset: 0x0001632A
	private void GoToBedAnimation0_Step2()
	{
		this.CheckForNightmareEvent(new GameController.DelegateAfterChatEndsEvents(this.CompleteGoToBedAnimation));
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00018140 File Offset: 0x00016340
	private void GoToBedAnimation1_Step1()
	{
		Vector3 position = base.transform.position;
		position.y = this.playerControl.transform.position.y;
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_sleep, AUDIO_TYPE.SFX, true, true, 0f, false, 1f, null, false, SFX_SUBGROUP.STINGER, false);
		this.nightBlackScreenTransition.SetActive(true);
		this.blackNightImage = this.nightBlackScreenTransition.GetComponent<Image>();
		this.transparentColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 0f);
		this.solidColor = new Color(this.blackNightImage.color.r, this.blackNightImage.color.b, this.blackNightImage.color.b, 1f);
		this.blackNightImage.color = this.transparentColor;
		DOTween.Sequence().Append(this.playerControl.transform.DOLookAt(position, 1f, AxisConstraint.None, null)).Append(this.playerControl.transform.DOMove(position, 1.5f, false))
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				this.GoToBedAnimation1_Step2();
			});
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x000182A8 File Offset: 0x000164A8
	private void GoToBedAnimation1_Step2()
	{
		base.transform.position.y = this.playerControl.transform.position.y - this.offsetY;
		DOTween.Sequence().Append(this.playerControl.transform.DOBlendableLocalRotateBy(new Vector3(0f, 0f, 45f), 2f, RotateMode.Fast)).Append(this.blackNightImage.DOBlendableColor(this.solidColor, 1.2f))
			.Append(this.playerControl.transform.DOBlendableLocalRotateBy(new Vector3(0f, 0f, -45f), 0.1f, RotateMode.Fast))
			.SetRelative<Sequence>()
			.SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				this.CheckForNightmareEvent(new GameController.DelegateAfterChatEndsEvents(this.CompleteGoToBedAnimation));
			});
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00018380 File Offset: 0x00016580
	private void CheckForNightmareEvent(GameController.DelegateAfterChatEndsEvents methodNextAnimation)
	{
		this.methodToCallAfterMidnight = methodNextAnimation;
		int num = Random.Range(1, 100);
		if (Singleton<Save>.Instance.GetDateStatusRealized("nightmare") != RelationshipStatus.Realized && Singleton<Save>.Instance.GetFullTutorialFinished() && (num > 95 || Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.STATE_DRANK_COFFEE)) && Singleton<Save>.Instance.GetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO))
		{
			Singleton<Save>.Instance.RemoveTutorialThresholdState(TutorialController.STATE_DRANK_COFFEE);
			Singleton<GameController>.Instance.SelectObj(this.nightBlackScreenTransition.GetComponent<InteractableObj>(), true, new GameController.DelegateAfterChatEndsEvents(this.GoToNextDay), true, false, false);
			Singleton<GameController>.Instance.SetDoNotPostProcessOnReturn();
			return;
		}
		this.GoToNextDay();
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00018428 File Offset: 0x00016628
	private void GoToNextDay()
	{
		Singleton<DayNightCycle>.Instance.IncrementDay();
		MovingDateable.MoveDateable("MovingFrontDoor", "default", true);
		MovingDateable.MoveDateable("Computer", "after", true);
		Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_3_WOKE_UP_DAY_TWO);
		Singleton<TutorialController>.Instance.SetTutorialText(null, false);
		this.methodToCallAfterMidnight();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00018488 File Offset: 0x00016688
	private void CompleteGoToBedAnimation()
	{
		this.playerControl.Move(this.PlayerAwakeLocation.position, Quaternion.Euler(0f, this.PlayerAwakeLocation.rotation.eulerAngles.y, 0f));
		BetterPlayerControl.Instance._camera.transform.localRotation = Quaternion.Euler(this.PlayerAwakeLocation.rotation.eulerAngles.x, 0f, 0f);
		this.blackNightImage.color = this.solidColor;
		DOTween.Sequence().Append(this.blackNightImage.DOBlendableColor(this.transparentColor, 1.5f)).OnComplete(delegate
		{
			this.SetBedroomCurtains(true);
			PlayerPauser.Unpause();
			CursorLocker.Lock();
			this.nightBlackScreenTransition.SetActive(false);
			this.playingAnimation = false;
		});
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00018550 File Offset: 0x00016750
	public void FinishChecking()
	{
		if (!Singleton<ComputerManager>.Instance.ThiscordReadyToExit())
		{
			Singleton<Popup>.Instance.CreatePopup("THISCORD", "You have unread Thiscord messages. Make sure you check them before going to sleep!", true);
			return;
		}
		if (!Singleton<ComputerManager>.Instance.ReadyToExit())
		{
			Singleton<Popup>.Instance.CreatePopup("WRKSPCE", "You have unread Wrkspce messages. Make sure you check them before going to sleep!", true);
			return;
		}
		this.Sleep();
	}

	// Token: 0x060003DC RID: 988 RVA: 0x000185A7 File Offset: 0x000167A7
	private void SetBedroomCurtains(bool open)
	{
		if (this.BedroomCurtains.interactedWithState != open)
		{
			this.BedroomCurtains.Interact(true, false);
			this.BedroomCurtains.ToggleInteractedWith(BetterPlayerControl.Instance.transform.position, false);
		}
	}

	// Token: 0x040003C8 RID: 968
	public static string BED_CAN_SLEEP_ANYTIME = "BedCanSleepAnytime";

	// Token: 0x040003C9 RID: 969
	public BetterPlayerControl playerControl;

	// Token: 0x040003CA RID: 970
	public GameObject rightSideDoor;

	// Token: 0x040003CB RID: 971
	public float movementTime = 10f;

	// Token: 0x040003CC RID: 972
	public float offsetY = 10f;

	// Token: 0x040003CD RID: 973
	public GameObject nightBlackScreenTransition;

	// Token: 0x040003CE RID: 974
	public bool playingAnimation;

	// Token: 0x040003CF RID: 975
	public Transform PlayerAwakeLocation;

	// Token: 0x040003D0 RID: 976
	private Color transparentColor;

	// Token: 0x040003D1 RID: 977
	private Color solidColor;

	// Token: 0x040003D2 RID: 978
	private Image blackNightImage;

	// Token: 0x040003D3 RID: 979
	private Image whiteImage;

	// Token: 0x040003D4 RID: 980
	private GameController.DelegateAfterChatEndsEvents methodToCallAfterMidnight;

	// Token: 0x040003D5 RID: 981
	[SerializeField]
	private GenericInteractable interactable;

	// Token: 0x040003D6 RID: 982
	[SerializeField]
	private GenericInteractable BedroomCurtains;
}
