using System;
using System.Collections;
using T17.Services;
using Team17.Scripts.Services.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E3 RID: 227
public class AwakenSplashScreen : MonoBehaviour
{
	// Token: 0x0600077E RID: 1918 RVA: 0x0002A370 File Offset: 0x00028570
	private void Awake()
	{
		if (AwakenSplashScreen.Instance == null)
		{
			AwakenSplashScreen.Instance = this;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0002A391 File Offset: 0x00028591
	private void OnDestroy()
	{
		if (this == AwakenSplashScreen.Instance)
		{
			AwakenSplashScreen.Instance = null;
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0002A3A4 File Offset: 0x000285A4
	public void Initialize(DateADexEntry entry, string internalName, string charName, bool forceCustomCharName, E_General_Poses PoseToUse = E_General_Poses.neutral, E_Facial_Expressions ExpressionToUse = E_Facial_Expressions.neutral)
	{
		this._inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.UI, this);
		this._Animator = base.GetComponent<Animator>();
		this._Animator.Update(0f);
		this._Animator.ResetTrigger("close");
		Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.STINGER, 0.5f);
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.5f);
		Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.EMOTE, 0.5f);
		Singleton<AudioManager>.Instance.PauseTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_dateable_awakened, AUDIO_TYPE.SFX, true, false, 0f, false, 1f, null, false, SFX_SUBGROUP.STINGER, false);
		this.characterName.text = ((entry != null) ? entry.CharName : charName);
		if (forceCustomCharName)
		{
			this.characterName.text = charName;
		}
		if (entry != null && !forceCustomCharName)
		{
			this._characterUtility = Singleton<CharacterHelper>.Instance._characters[entry.internalName];
			this._character.sprite = this._characterUtility.GetSpriteFromPoseExpression(PoseToUse, ExpressionToUse, false, false);
		}
		else
		{
			this._character.sprite = CharacterUtility.GetSpriteFileFromPoseExpression(PoseToUse, ExpressionToUse, internalName, false);
		}
		base.gameObject.SetActive(true);
		this._nextButton.gameObject.SetActive(true);
		this.isOpen = true;
		CursorLocker.Unlock();
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0002A4FB File Offset: 0x000286FB
	private void Update()
	{
		if (this._nextButton.gameObject.activeInHierarchy)
		{
			ControllerMenuUI.SetCurrentlySelected(this._nextButton.gameObject, ControllerMenuUI.Direction.Down, false, false);
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0002A524 File Offset: 0x00028724
	public void Close()
	{
		this._nextButton.gameObject.SetActive(false);
		this._Animator.Update(1000f);
		this._Animator.SetTrigger("close");
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_transition_house, AUDIO_TYPE.SFX, false, false, 0.5f, false, 1f, null, false, SFX_SUBGROUP.NONE, false);
		CursorLocker.Lock();
		base.StartCoroutine(this.CloseAfterAnimation());
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0002A59A File Offset: 0x0002879A
	private IEnumerator CloseAfterAnimation()
	{
		yield return new WaitForSeconds(2f);
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = null;
		if (this._characterUtility != null)
		{
			CharacterUtility.ReturnCharacterSprite(this._character.sprite);
			this._characterUtility = null;
		}
		TalkingUI.Instance.UnpauseCoroutine("AwakenSplashScreen");
		this.isOpen = false;
		base.gameObject.SetActive(false);
		Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.EMOTE, 0.5f);
		Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.STINGER, 0f);
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0f);
		Singleton<AudioManager>.Instance.ResumeTrackGroup(AUDIO_TYPE.MUSIC, false, 1f);
		yield break;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0002A5A9 File Offset: 0x000287A9
	private void OnDisable()
	{
		PlayerPauser.Unpause(this);
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = null;
	}

	// Token: 0x040006C3 RID: 1731
	public static AwakenSplashScreen Instance;

	// Token: 0x040006C4 RID: 1732
	[SerializeField]
	private Image _character;

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	private TMP_Text characterName;

	// Token: 0x040006C6 RID: 1734
	[SerializeField]
	private float delayTillClose = 5f;

	// Token: 0x040006C7 RID: 1735
	[SerializeField]
	private Button _nextButton;

	// Token: 0x040006C8 RID: 1736
	public bool isOpen;

	// Token: 0x040006C9 RID: 1737
	private CharacterUtility _characterUtility;

	// Token: 0x040006CA RID: 1738
	private InputModeHandle _inputModeHandle;

	// Token: 0x040006CB RID: 1739
	private Animator _Animator;
}
