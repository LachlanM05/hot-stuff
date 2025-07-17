using System;
using System.Collections.Generic;
using System.Linq;
using CowberryStudios.ProjectAI;
using T17.Services;
using Team17.Scripts.Services.Input;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000128 RID: 296
public class ResultSplashScreen : MonoBehaviour
{
	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0003A169 File Offset: 0x00038369
	// (set) Token: 0x06000A30 RID: 2608 RVA: 0x0003A170 File Offset: 0x00038370
	public static ResultSplashScreen Instance { get; private set; }

	// Token: 0x06000A31 RID: 2609 RVA: 0x0003A178 File Offset: 0x00038378
	private void Awake()
	{
		if (ResultSplashScreen.Instance == null)
		{
			ResultSplashScreen.Instance = this;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0003A199 File Offset: 0x00038399
	private void OnDestroy()
	{
		if (this == ResultSplashScreen.Instance)
		{
			ResultSplashScreen.Instance = null;
		}
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0003A1A9 File Offset: 0x000383A9
	private void Update()
	{
		if (Singleton<PlayerPauser>.Instance.PauseList.Find((PauseState pause) => pause.Source == this.ToString()) == null)
		{
			PlayerPauser.Pause(this);
		}
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0003A1D0 File Offset: 0x000383D0
	public void Initialize(DateADexEntry entry, RelationshipStatus status, string internalName, string charNumber, string charName, string charObj)
	{
		this._timeLastOpened = Time.time;
		this._inputModeHandle = Services.InputService.PushMode(IMirandaInputService.EInputMode.UI, this);
		this.isOpen = true;
		if (entry != null)
		{
			this._titleBanner.SetContent(entry);
			if (Singleton<CharacterHelper>.Instance._characters.IsNameInSet(entry.internalName))
			{
				this._characterUtility = Singleton<CharacterHelper>.Instance._characters[entry.internalName];
				this._orignalSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.neutral, false, false);
				this._character.sprite = this._orignalSprite;
			}
		}
		else
		{
			this._titleBanner.UpdateCustomDisplayContent(status, charNumber, charName, charObj);
			if (internalName == "timmy")
			{
				if (status == RelationshipStatus.Realized)
				{
					this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.timmy, E_Facial_Expressions.neutral, "tim", false);
				}
				else
				{
					this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.nya, E_Facial_Expressions.neutral, "tim", false);
				}
			}
			else if (internalName == "jonwick")
			{
				this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.sulk, E_Facial_Expressions.neutral, "scandalabra", false);
			}
			else if (internalName == "front")
			{
				this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.front, E_Facial_Expressions.neutral, "dorian", false);
			}
			else if (internalName == "tiny")
			{
				this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.tiny, E_Facial_Expressions.neutral, "dorian", false);
			}
			else if (internalName == "trap")
			{
				this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.trap, E_Facial_Expressions.neutral, "dorian", false);
			}
			else if (internalName == "back")
			{
				this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.back, E_Facial_Expressions.neutral, "dorian", false);
			}
			else
			{
				this._orignalSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.neutral, internalName, false);
			}
			this._character.sprite = this._orignalSprite;
		}
		this._animator.SetTrigger(status.ToString());
		Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.EMOTE, 0.3f);
		Singleton<AudioManager>.Instance.PauseTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		if (this._characterUtility != null)
		{
			switch (status)
			{
			case RelationshipStatus.Hate:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_hate, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (this._characterUtility.internalName == "lucinda")
				{
					string variable = Singleton<InkController>.Instance.GetVariable("lucinda_final");
					if (variable == "lust")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.lust, E_Facial_Expressions.tsundere, false, false);
					}
					else if (variable == "loving")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.hate, E_Facial_Expressions.crosstsundere, false, false);
					}
					else if (variable == "limitless")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.friend, E_Facial_Expressions.tsundere, false, false);
					}
					else if (variable == "lucid")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.love, E_Facial_Expressions.tsundere, false, false);
					}
					else
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.tsundere, false, false);
					}
				}
				else if (this._characterUtility.internalName == "connie" && charName == "luna")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.folded, E_Facial_Expressions.tsundere, false, false);
				}
				else if (this._characterUtility.internalName == "connie")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.gun, E_Facial_Expressions.shout, false, false);
				}
				else
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.hate, E_Facial_Expressions.angry, false, false);
				}
				break;
			case RelationshipStatus.Single:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_friend, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				break;
			case RelationshipStatus.Love:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_love, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (this._characterUtility.internalName == "shadow")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.skips, E_Facial_Expressions.flirt, false, false);
				}
				else if (this._characterUtility.internalName == "wallace")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.hate, E_Facial_Expressions.happy, false, false);
				}
				else if (this._characterUtility.internalName == "connie" && charName == "luna")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.folded, E_Facial_Expressions.smirk, false, false);
				}
				else if (this._characterUtility.internalName == "connie")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.love, E_Facial_Expressions.joy, false, false);
				}
				else if (this._characterUtility.internalName == "lucinda")
				{
					string variable2 = Singleton<InkController>.Instance.GetVariable("lucinda_final");
					if (variable2 == "lust")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.lust, E_Facial_Expressions.joy, false, false);
					}
					else if (variable2 == "loving")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.hate, E_Facial_Expressions.flirt, false, false);
					}
					else if (variable2 == "limitless")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.friend, E_Facial_Expressions.smirk, false, false);
					}
					else if (variable2 == "lucid")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.love, E_Facial_Expressions.sad, false, false);
					}
					else
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.happy, false, false);
					}
				}
				else
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.love, E_Facial_Expressions.flirt, false, false);
				}
				break;
			case RelationshipStatus.Friend:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_friend, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (this._characterUtility.internalName == "shadow")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.skips, E_Facial_Expressions.happy, false, false);
				}
				else if (this._characterUtility.internalName == "dorian")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.joy, false, false);
				}
				else if (this._characterUtility.internalName == "connie" && charName == "luna")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.gun, E_Facial_Expressions.happy, false, false);
				}
				else if (this._characterUtility.internalName == "connie")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.folded, E_Facial_Expressions.smirk, false, false);
				}
				else if (this._characterUtility.internalName == "lucinda")
				{
					string variable3 = Singleton<InkController>.Instance.GetVariable("lucinda_final");
					if (variable3 == "lust")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.lust, E_Facial_Expressions.happy, false, false);
					}
					else if (variable3 == "loving")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.hate, E_Facial_Expressions.joy, false, false);
					}
					else if (variable3 == "limitless")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.friend, E_Facial_Expressions.joy, false, false);
					}
					else if (variable3 == "lucid")
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.love, E_Facial_Expressions.smirk, false, false);
					}
					else
					{
						this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.joy, false, false);
					}
				}
				else
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.friend, E_Facial_Expressions.joy, false, false);
				}
				break;
			case RelationshipStatus.Realized:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_realized, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (this._characterUtility.internalName == "sinclaire" && (bool)Singleton<InkController>.Instance.story.variablesState["sinclaire_turtle"])
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.turtle, E_Facial_Expressions.happy, false, false);
				}
				else if (this._characterUtility.internalName == "jonwick")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.wick, E_Facial_Expressions.think, false, false);
				}
				else if (this._characterUtility.internalName == "rebel")
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.realized, E_Facial_Expressions.think, false, false);
				}
				else
				{
					this._relationshipStatusSprite = this._characterUtility.GetSpriteFromPoseExpression(E_General_Poses.realized, E_Facial_Expressions.joy, false, false);
				}
				break;
			}
		}
		else
		{
			switch (status)
			{
			case RelationshipStatus.Hate:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_hate, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (internalName == "jonwick")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.sulk, E_Facial_Expressions.neutral, "scandalabra", false);
				}
				else if (internalName == "timmy")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.feral, E_Facial_Expressions.tsundere, "tim", false);
				}
				else if (internalName == "front")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.front, E_Facial_Expressions.angry, "dorian", false);
				}
				else if (internalName == "tiny")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.tiny, E_Facial_Expressions.angry, "dorian", false);
				}
				else if (internalName == "trap")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.trap, E_Facial_Expressions.angry, "dorian", false);
				}
				else if (internalName == "back")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.back, E_Facial_Expressions.neutral, "dorian", false);
				}
				else
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.hate, E_Facial_Expressions.angry, internalName, false);
				}
				break;
			case RelationshipStatus.Single:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_friend, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				break;
			case RelationshipStatus.Love:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_love, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (internalName == "jonwick")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.sulk, E_Facial_Expressions.neutral, "scandalabra", false);
				}
				else if (internalName == "timmy")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.nya, E_Facial_Expressions.flirt, "tim", false);
				}
				else if (internalName == "front")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.front, E_Facial_Expressions.flirt, "dorian", false);
				}
				else if (internalName == "tiny")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.tiny, E_Facial_Expressions.flirt, "dorian", false);
				}
				else if (internalName == "trap")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.trap, E_Facial_Expressions.flirt, "dorian", false);
				}
				else if (internalName == "back")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.back, E_Facial_Expressions.neutral, "dorian", false);
				}
				else
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.love, E_Facial_Expressions.flirt, internalName, false);
				}
				break;
			case RelationshipStatus.Friend:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_friend, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (internalName == "jonwick")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.sulk, E_Facial_Expressions.neutral, "scandalabra", false);
				}
				else if (internalName == "timmy")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.nya, E_Facial_Expressions.joy, "tim", false);
				}
				else if (internalName == "front")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.front, E_Facial_Expressions.joy, "dorian", false);
				}
				else if (internalName == "tiny")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.tiny, E_Facial_Expressions.joy, "dorian", false);
				}
				else if (internalName == "trap")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.trap, E_Facial_Expressions.joy, "dorian", false);
				}
				else if (internalName == "back")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.back, E_Facial_Expressions.neutral, "dorian", false);
				}
				else
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.friend, E_Facial_Expressions.joy, internalName, false);
				}
				break;
			case RelationshipStatus.Realized:
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_ending_realized, AUDIO_TYPE.SFX, false, false, 0f, true, 0f, null, false, SFX_SUBGROUP.STINGER, false);
				if (internalName == "sinclaire" && (bool)Singleton<InkController>.Instance.story.variablesState["sinclaire_turtle"])
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.turtle, E_Facial_Expressions.happy, "sinclaire", false);
				}
				else if (internalName == "jonwick")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.wick, E_Facial_Expressions.think, "scandalabra", false);
				}
				else if (internalName == "rebel")
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.realized, E_Facial_Expressions.think, "rebel", false);
				}
				else
				{
					this._relationshipStatusSprite = CharacterUtility.GetSpriteFileFromPoseExpression(E_General_Poses.realized, E_Facial_Expressions.joy, internalName, false);
				}
				break;
			}
		}
		Button componentInChildren = base.GetComponentInChildren<Button>();
		if (componentInChildren != null)
		{
			this._cachedCurrentlySelected = Singleton<ControllerMenuUI>.Instance.currentlySelected;
			ControllerMenuUI.SetCurrentlySelected(componentInChildren.gameObject, ControllerMenuUI.Direction.Down, false, false);
		}
		CursorLocker.Unlock();
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0003AF4C File Offset: 0x0003914C
	public void UpdateCharacterExpression()
	{
		if (this._relationshipStatusSprite != null)
		{
			this._character.sprite = this._relationshipStatusSprite;
		}
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x0003AF6D File Offset: 0x0003916D
	private bool HasBeenOpenMinimumTime()
	{
		return this._timeLastOpened + this._minTimeMustOpenFor < Time.time;
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0003AF83 File Offset: 0x00039183
	public void TryClose()
	{
		if (!this.HasBeenOpenMinimumTime())
		{
			return;
		}
		this.Close();
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0003AF94 File Offset: 0x00039194
	public void Close()
	{
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.Dispose();
		}
		this._inputModeHandle = null;
		this.ReturnAllCharacterImages();
		PlayerPauser.Unpause(this);
		base.gameObject.SetActive(false);
		TalkingUI.Instance.UnpauseCoroutine("ResultSplashScreen");
		if (this._cachedCurrentlySelected != null)
		{
			ControllerMenuUI.SetCurrentlySelected(this._cachedCurrentlySelected, ControllerMenuUI.Direction.Down, false, false);
		}
		if (Singleton<InkController>.Instance.forceGoHome)
		{
			PlayerPauser.Unpause();
			Singleton<InkController>.Instance.forceGoHome = false;
		}
		CursorLocker.Lock();
		this.isOpen = false;
		Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.STINGER, 0f);
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0f);
		if ((!Singleton<GameController>.Instance.talkingUI.autoPlayNextVo && !Singleton<SpecStatMain>.Instance.visible) || Singleton<InkController>.Instance.forceGoHome)
		{
			Singleton<AudioManager>.Instance.ResumeTrackGroup(AUDIO_TYPE.MUSIC, false, 1f);
			Singleton<GameController>.Instance.talkingUI.autoPlayNextVo = true;
			if (!Singleton<InkController>.Instance.forceGoHome)
			{
				ParsedTag nextAudioVo = Singleton<GameController>.Instance.talkingUI.nextAudioVo;
				if (nextAudioVo.properties != null)
				{
					string[] array = nextAudioVo.properties.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					if (array.Count<string>() > 0)
					{
						CommandID commandID = new CommandID(nextAudioVo.name);
						IEnumerable<string> enumerable = array.Select((string p) => p.Trim().ToLowerInvariant());
						InkCommand inkCommand = new InkCommand(commandID, enumerable);
						Singleton<InkController>.Instance.binding.Invoke(inkCommand);
					}
				}
			}
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x0003B118 File Offset: 0x00039318
	private void ReturnAllCharacterImages()
	{
		if (this._characterUtility == null)
		{
			return;
		}
		if (this._relationshipStatusSprite != null)
		{
			CharacterUtility.ReturnCharacterSprite(this._relationshipStatusSprite);
			this._relationshipStatusSprite = null;
		}
		if (this._orignalSprite != null)
		{
			CharacterUtility.ReturnCharacterSprite(this._orignalSprite);
			this._orignalSprite = null;
		}
		this._characterUtility = null;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0003B17B File Offset: 0x0003937B
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

	// Token: 0x04000954 RID: 2388
	[SerializeField]
	private DexEntryButton _titleBanner;

	// Token: 0x04000955 RID: 2389
	[SerializeField]
	private Animator _animator;

	// Token: 0x04000956 RID: 2390
	[SerializeField]
	private Image _character;

	// Token: 0x04000957 RID: 2391
	[SerializeField]
	private float _minTimeMustOpenFor = 2f;

	// Token: 0x04000958 RID: 2392
	public bool isOpen;

	// Token: 0x04000959 RID: 2393
	private Sprite _relationshipStatusSprite;

	// Token: 0x0400095A RID: 2394
	private GameObject _cachedCurrentlySelected;

	// Token: 0x0400095B RID: 2395
	private CharacterUtility _characterUtility;

	// Token: 0x0400095C RID: 2396
	private Sprite _orignalSprite;

	// Token: 0x0400095D RID: 2397
	private InputModeHandle _inputModeHandle;

	// Token: 0x0400095E RID: 2398
	private float _timeLastOpened = float.MinValue;
}
