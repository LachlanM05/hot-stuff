using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013D RID: 317
public class StageManager
{
	// Token: 0x06000B4F RID: 2895 RVA: 0x0004081C File Offset: 0x0003EA1C
	public static string ParsePositionNames(string positionName)
	{
		if (positionName == "right_center")
		{
			return "rightcenter";
		}
		if (positionName == "left_center")
		{
			return "leftcenter";
		}
		return positionName;
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00040848 File Offset: 0x0003EA48
	public Dictionary<string, string> GetSaveData()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (string text in this.speakPositionToSpeakerPortrait.Keys)
		{
			List<UISpeaker> list = this.speakPositionToSpeakerPortrait[text];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null && !dictionary.ContainsKey(list[i].character))
				{
					dictionary.Add(list[i].character, text);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x000408FC File Offset: 0x0003EAFC
	public List<string> GetCurrentSpeakerList()
	{
		return this.enteredCharacterSet.ToList<string>();
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0004090C File Offset: 0x0003EB0C
	public StageManager(UISpeaker[] _speakers, Transform _speakerPositions, Image _bgimage, GameObject _foregroundImage)
	{
		this.bgImg = _bgimage;
		this.foregroundImg = _foregroundImage;
		this.internalNameToSpeakingCharacter = new Dictionary<string, SpeakingCharacter>(StringComparer.OrdinalIgnoreCase);
		this.enteredCharacterSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		this.speakPositionToSpeakerPortrait = new Dictionary<string, List<UISpeaker>>(StringComparer.OrdinalIgnoreCase);
		foreach (string text in StageManager.speakPositionNames)
		{
			this.speakPositionToSpeakerPortrait.Add(text, new List<UISpeaker>());
		}
		this.speakerPositions = _speakerPositions;
		this.speakerPortraits = _speakers;
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x00040A08 File Offset: 0x0003EC08
	public void SetupStageFromSave(Dictionary<string, string> characterToPos)
	{
		foreach (string text in characterToPos.Keys)
		{
			string text2 = characterToPos[text];
			this.CheckAndAddCharacter(text, "offscreenleft", text2, false);
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00040A6C File Offset: 0x0003EC6C
	public void SetBackgroundImage(Sprite spriteImage)
	{
		if (spriteImage == null)
		{
			this.HideBackgroundImage();
			return;
		}
		this.bgImg.gameObject.SetActive(true);
		Image component = this.bgImg.transform.GetChild(0).GetComponent<Image>();
		component.DOFade(0f, 0f);
		component.DOFade(1f, 0.2f);
		component.sprite = spriteImage;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00040AD8 File Offset: 0x0003ECD8
	public void HideBackgroundImage()
	{
		this.bgImg.transform.GetChild(0).GetComponent<Image>().DOFade(0f, 0.2f)
			.OnComplete(delegate
			{
				this.bgImg.gameObject.SetActive(false);
			});
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00040B14 File Offset: 0x0003ED14
	public void SetForegroundImage(Sprite spriteImage)
	{
		if (this.foregroundImg == null)
		{
			return;
		}
		this.foregroundImg.gameObject.SetActive(true);
		if (this.foregroundImg.transform.childCount == 0)
		{
			return;
		}
		Transform child = this.foregroundImg.transform.GetChild(0);
		if (child == null)
		{
			return;
		}
		Image component = child.GetComponent<Image>();
		if (component == null)
		{
			return;
		}
		component.sprite = spriteImage;
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00040B88 File Offset: 0x0003ED88
	public void HideForegroundImage()
	{
		if (this.foregroundImg == null)
		{
			return;
		}
		this.foregroundImg.gameObject.SetActive(false);
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00040BAA File Offset: 0x0003EDAA
	public float GetStagePosition(string pos)
	{
		if (!StageManager.speakPositionNames.Contains(pos))
		{
			return 1470f;
		}
		return this.speakerPositions.Find(pos).localPosition.x;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00040BD5 File Offset: 0x0003EDD5
	public float GetStagePositionForNewCharacter(out string toPosition)
	{
		this.TryGetToPositionForNewCharacter(out toPosition, false, false);
		return this.GetStagePosition(toPosition);
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x00040BE9 File Offset: 0x0003EDE9
	public void CheckAndAddCharacter(string _name, string currentStagePosition, string toStagePosition, bool isSlow = false)
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized(_name) == RelationshipStatus.Realized)
		{
			this.CheckAndAddCharacter(_name, currentStagePosition, toStagePosition, E_General_Poses.realized, E_Facial_Expressions.realized, isSlow);
			return;
		}
		this.CheckAndAddCharacter(_name, currentStagePosition, toStagePosition, E_General_Poses.neutral, E_Facial_Expressions.neutral, isSlow);
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00040C18 File Offset: 0x0003EE18
	public void CheckAndAddCharacter(string _name, string currentStagePosition, string toStagePosition, E_General_Poses e_pose, E_Facial_Expressions e_expr, bool isSlow = false)
	{
		string text;
		if (!this.GetInternalName(_name, out text))
		{
			string.IsNullOrEmpty(text.Trim());
			return;
		}
		this.Log("CheckAndAddCharacter " + text);
		UISpeaker uispeaker = null;
		if (!this.internalNameToSpeakingCharacter.ContainsKey(text))
		{
			foreach (UISpeaker uispeaker2 in this.speakerPortraits)
			{
				if (!uispeaker2.isActiveOnStage && !uispeaker2.isExiting)
				{
					uispeaker = uispeaker2;
					break;
				}
			}
			if (uispeaker != null)
			{
				SpeakingCharacter speakingCharacter = this.InitNewSpeakingCharacter(text, uispeaker);
				speakingCharacter.SetUIPortraitSprite(e_pose, e_expr);
				uispeaker.SetData(toStagePosition, this.GetStagePosition(currentStagePosition), this.GetStagePosition(toStagePosition), true, isSlow);
				if (!this.internalNameToSpeakingCharacter.ContainsKey(text))
				{
					this.internalNameToSpeakingCharacter.Add(text, speakingCharacter);
				}
				this.speakPositionToSpeakerPortrait[toStagePosition].Add(uispeaker);
				if (!this.enteredCharacterSet.Contains(text))
				{
					this.enteredCharacterSet.Add(text);
				}
			}
		}
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00040D14 File Offset: 0x0003EF14
	private SpeakingCharacter InitNewSpeakingCharacter(string _inName, UISpeaker speakerPortrait)
	{
		string text;
		if (!Singleton<CharacterHelper>.Instance._characters.IsNameInSet(_inName))
		{
			text = Singleton<CharacterHelper>.Instance._characters.GetInternalName(_inName);
		}
		else
		{
			text = _inName;
		}
		if (Singleton<CharacterHelper>.Instance._characters.Contains(text))
		{
			string inkName = Singleton<CharacterHelper>.Instance._characters[text].inkName;
			return new SpeakingCharacter(text, inkName, speakerPortrait);
		}
		return new SpeakingCharacter(text, text, speakerPortrait);
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x00040D84 File Offset: 0x0003EF84
	public void GetFromPositionForNewCharacter(out string FromPosition)
	{
		switch (this.GetNumberOfOccupiedPositions())
		{
		case 0:
			FromPosition = "offscreenleft";
			return;
		case 1:
			FromPosition = "offscreenleft";
			return;
		case 2:
			FromPosition = "offscreenright";
			return;
		case 3:
			FromPosition = "offscreenleft";
			return;
		case 4:
			FromPosition = "offscreenright";
			return;
		case 5:
			FromPosition = "offscreenleft";
			return;
		case 6:
			FromPosition = "offscreenright";
			return;
		default:
			FromPosition = "offscreenright";
			return;
		}
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00040DFB File Offset: 0x0003EFFB
	public bool GetInternalName(string _characterName, out string internalName)
	{
		internalName = "";
		if (!Singleton<CharacterHelper>.Instance._characters.IsNameInSet(_characterName))
		{
			internalName = Singleton<CharacterHelper>.Instance._characters.GetInternalName(_characterName);
		}
		else
		{
			internalName = _characterName;
		}
		return true;
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00040E2E File Offset: 0x0003F02E
	public bool TryGetSpeakingCharacter(string _characterName, out SpeakingCharacter speakingChara)
	{
		speakingChara = this.GetSpeakingCharacter(_characterName);
		return speakingChara != null;
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00040E40 File Offset: 0x0003F040
	public SpeakingCharacter GetSpeakingCharacter(string _characterName)
	{
		if (this.enteredCharacterSet.Count <= 0)
		{
			return null;
		}
		string text;
		if (!this.GetInternalName(_characterName, out text) && string.IsNullOrEmpty(text.Trim()))
		{
			return null;
		}
		SpeakingCharacter speakingCharacter;
		if (!this.internalNameToSpeakingCharacter.TryGetValue(text, out speakingCharacter))
		{
			return null;
		}
		return speakingCharacter;
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00040E8C File Offset: 0x0003F08C
	public void MoveCharacterToPosition(string _characterName, string _toPosition)
	{
		SpeakingCharacter speakingCharacter;
		if (!this.TryGetSpeakingCharacter(_characterName, out speakingCharacter))
		{
			return;
		}
		StageManager.speakPositionNames.Contains(_toPosition, StringComparer.OrdinalIgnoreCase);
		speakingCharacter.SetUIPortraitMovePosition(this.GetStagePosition(_toPosition));
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00040EC4 File Offset: 0x0003F0C4
	public void SetFacingDirection(string _characterName, UISpeaker.E_Direction direction)
	{
		SpeakingCharacter speakingCharacter;
		if (!this.TryGetSpeakingCharacter(_characterName, out speakingCharacter))
		{
			return;
		}
		speakingCharacter.SetUIPortraitDirection(direction);
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00040EE4 File Offset: 0x0003F0E4
	public void ResetEmotes()
	{
		if (this.activeEmotes != null)
		{
			foreach (GameObject gameObject in this.activeEmotes)
			{
				gameObject.gameObject.SetActive(false);
			}
			this.activeEmotes.Clear();
		}
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x00040F50 File Offset: 0x0003F150
	public void SetCharacterPoseExpression(string _characterName, E_General_Poses pose, E_Facial_Expressions expression)
	{
		SpeakingCharacter speakingCharacter;
		if (!this.TryGetSpeakingCharacter(_characterName, out speakingCharacter))
		{
			if (_characterName == "narrator")
			{
				return;
			}
			if (!this.internalNameToSpeakingCharacter.ContainsKey(_characterName))
			{
				this.OnCharacterEnter(_characterName, pose, expression);
				return;
			}
		}
		else
		{
			if (speakingCharacter.lastPose != pose)
			{
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_switch_pose, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			}
			this.SetCharacterPoseExpression(speakingCharacter, pose, expression);
		}
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x00040FC8 File Offset: 0x0003F1C8
	public void SetCharacterExpression(string _characterName, E_Facial_Expressions expression)
	{
		SpeakingCharacter speakingCharacter;
		if (!this.TryGetSpeakingCharacter(_characterName, out speakingCharacter))
		{
			return;
		}
		this.SetCharacterExpression(speakingCharacter, expression);
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00040FE9 File Offset: 0x0003F1E9
	public void SetCharacterPoseExpression(SpeakingCharacter currentSpeaking, E_General_Poses pose, E_Facial_Expressions expression)
	{
		currentSpeaking.SetUIPortraitPoseExpression(pose, expression);
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00040FF3 File Offset: 0x0003F1F3
	public void SetCharacterExpression(SpeakingCharacter currentSpeaking, E_Facial_Expressions expression)
	{
		currentSpeaking.SetUIPortraitExpression(expression);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00040FFC File Offset: 0x0003F1FC
	public void SetCharacterPose(SpeakingCharacter currentSpeaking, E_General_Poses pose)
	{
		currentSpeaking.SetUIPortraitPose(pose);
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00041008 File Offset: 0x0003F208
	public void SetCharacterPose(string _characterName, E_General_Poses pose)
	{
		SpeakingCharacter speakingCharacter;
		if (!this.TryGetSpeakingCharacter(_characterName, out speakingCharacter))
		{
			return;
		}
		this.SetCharacterPose(speakingCharacter, pose);
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0004102C File Offset: 0x0003F22C
	public bool TryGetToPositionForNewCharacter(out string ToPosition, bool overrideCrowding = false, bool forceIsCollectableShowing = false)
	{
		ToPosition = "";
		switch (this.GetNumberOfOccupiedPositions())
		{
		case 0:
			ToPosition = "center";
			break;
		case 1:
			ToPosition = "left";
			break;
		case 2:
			ToPosition = "right";
			break;
		case 3:
			ToPosition = "leftcenter";
			break;
		case 4:
			ToPosition = "rightcenter";
			break;
		default:
			if (this.enteredCharacterSet.Count % 2 == 0)
			{
				ToPosition = "right";
			}
			else
			{
				ToPosition = "left";
			}
			break;
		}
		this.Log(string.Format("TryGetToPositionForNewCharacter num occupied pos={0} Destination Pos='{1}'", this.GetNumberOfOccupiedPositions(), ToPosition));
		return true;
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x000410D0 File Offset: 0x0003F2D0
	public bool TryGetToPositionForNewCharacter(string _tryPos, out string ToPosition, bool overrideCrowding = false)
	{
		if (this.enteredCharacterSet.Count <= 0)
		{
			ToPosition = _tryPos;
			return true;
		}
		List<UISpeaker> list = this.speakPositionToSpeakerPortrait[_tryPos];
		if (list == null)
		{
			ToPosition = _tryPos;
			return true;
		}
		bool flag = false;
		for (int i = 0; i < list.Count; i++)
		{
			flag |= list[i].isActiveOnStage;
		}
		if (!flag)
		{
			ToPosition = _tryPos;
			return true;
		}
		if (overrideCrowding)
		{
			ToPosition = _tryPos;
			return true;
		}
		ToPosition = "";
		return false;
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x00041140 File Offset: 0x0003F340
	public bool TryGetStagePositionsForNewCharacter(string _tryPos, out string FromPosition, out string ToPosition, bool overrideCrowding = false)
	{
		this.GetFromPositionForNewCharacter(out FromPosition);
		return this.TryGetToPositionForNewCharacter(_tryPos, out ToPosition, overrideCrowding) || overrideCrowding || this.TryGetToPositionForNewCharacter(out ToPosition, false, false);
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0004117C File Offset: 0x0003F37C
	public bool TryGetStagePositionsForNewCharacter(out string FromPosition, out string ToPosition, bool overrideCrowding = false)
	{
		this.GetFromPositionForNewCharacter(out FromPosition);
		return this.TryGetToPositionForNewCharacter(out ToPosition, overrideCrowding, false);
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00041194 File Offset: 0x0003F394
	public void OnCharacterEnter(string internalName, bool isSlow)
	{
		this.Log("OnCharacterEnter '" + internalName + "'");
		if (!this.enteredCharacterSet.Contains(internalName))
		{
			this.enteredCharacterSet.Add(internalName);
			string text = "";
			string text2 = "";
			if (!this.TryGetStagePositionsForNewCharacter(out text, out text2, false))
			{
				return;
			}
			this.CheckAndAddCharacter(internalName, text, text2, isSlow);
			this.PlayCharacterEnter(isSlow);
		}
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00041200 File Offset: 0x0003F400
	public void OnPlayerSpeaking(string internalName)
	{
		SpeakingCharacter speakingCharacter;
		if (!this.TryGetSpeakingCharacter(Singleton<CharacterHelper>.Instance._characters.GetInternalName(internalName), out speakingCharacter))
		{
			return;
		}
		if (this._currentSpeaker != speakingCharacter)
		{
			foreach (KeyValuePair<string, List<UISpeaker>> keyValuePair in this.speakPositionToSpeakerPortrait)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					if (keyValuePair.Value[i] != null)
					{
						keyValuePair.Value[i].OnNotSpeaking();
					}
				}
			}
			speakingCharacter.UIPortrait.OnSpeaking();
			this._currentSpeaker = speakingCharacter;
		}
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x000412C0 File Offset: 0x0003F4C0
	private void PlayCharacterEnter(bool isSlow)
	{
		if (isSlow)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_enter_slow, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_enter_fast, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0004131D File Offset: 0x0003F51D
	public void OnCharacterEnter(string internalName, E_General_Poses pose)
	{
		this.OnCharacterEnter(internalName, pose, E_Facial_Expressions.neutral);
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x00041328 File Offset: 0x0003F528
	public void OnCharacterEnter(string internalName, E_General_Poses pose, string toPosition)
	{
		this.OnCharacterEnter(internalName, pose, E_Facial_Expressions.neutral, toPosition);
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x00041334 File Offset: 0x0003F534
	public void OnCharacterEnter(string internalName, E_General_Poses pose, E_Facial_Expressions expression)
	{
		this.Log("OnCharacterEnter '" + internalName + "'");
		if (!this.enteredCharacterSet.Contains(internalName))
		{
			this.enteredCharacterSet.Add(internalName);
		}
		string text;
		if (!this.TryGetToPositionForNewCharacter(out text, false, false))
		{
			return;
		}
		this.OnCharacterEnter(internalName, pose, expression, text);
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0004138C File Offset: 0x0003F58C
	public void OnCharacterEnter(string internalName, E_General_Poses pose, E_Facial_Expressions expression, string toPosition)
	{
		if (!this.enteredCharacterSet.Contains(internalName))
		{
			this.enteredCharacterSet.Add(internalName);
		}
		this.Log("OnCharacterEnter '" + internalName + "'");
		string text = "";
		string text2 = "";
		if (!this.TryGetStagePositionsForNewCharacter(toPosition, out text, out text2, true))
		{
			return;
		}
		this.CheckAndAddCharacter(internalName, text, text2, pose, expression, false);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x000413F2 File Offset: 0x0003F5F2
	public bool IsAnEnteredCharacter(string internalName)
	{
		return this.enteredCharacterSet.Contains(internalName);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x00041400 File Offset: 0x0003F600
	public void OnCharacterEnter(string internalName, string toPosition, bool isSlow)
	{
		if (!this.enteredCharacterSet.Contains(internalName))
		{
			this.Log("OnCharacterEnter '" + internalName + "'");
			this.enteredCharacterSet.Add(internalName);
			string text = "";
			string text2 = "";
			if (!this.TryGetStagePositionsForNewCharacter(toPosition, out text, out text2, true))
			{
				return;
			}
			this.TryGetStagePositionsForNewCharacter(toPosition, out text, out text2, true);
			this.CheckAndAddCharacter(internalName, text, text2, isSlow);
			this.PlayCharacterEnter(isSlow);
		}
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x00041478 File Offset: 0x0003F678
	public void OnCharacterExit(string _name)
	{
		this.Log("OnCharacterExit '" + _name + "'");
		string text;
		if (!this.GetInternalName(_name, out text))
		{
			text = _name;
		}
		if (this.internalNameToSpeakingCharacter.ContainsKey(text))
		{
			SpeakingCharacter speakingCharacter = this.internalNameToSpeakingCharacter[text];
			this.HideEmotes(text);
			this.enteredCharacterSet.Remove(text);
			this.speakPositionToSpeakerPortrait[speakingCharacter.UIPortrait.targetPosName].Remove(speakingCharacter.UIPortrait);
			this.internalNameToSpeakingCharacter.Remove(text);
			speakingCharacter.UIPortrait.StartExit();
			this.RepositionEnteredCharactersAfterCharacterLeft(speakingCharacter);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x00041544 File Offset: 0x0003F744
	public void ClearData()
	{
		this.enteredCharacterSet.Clear();
		this.internalNameToSpeakingCharacter.Clear();
		foreach (KeyValuePair<string, List<UISpeaker>> keyValuePair in this.speakPositionToSpeakerPortrait)
		{
			keyValuePair.Value.Clear();
		}
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x000415B4 File Offset: 0x0003F7B4
	public void OnReturnToHome()
	{
		foreach (UISpeaker uispeaker in this.speakerPortraits)
		{
			uispeaker.SetTargetPosition(this.GetStagePosition("offscreenleft"));
			uispeaker.StartExit();
		}
		this.enteredCharacterSet.Clear();
		this.ResetEmotes();
		this.internalNameToSpeakingCharacter = new Dictionary<string, SpeakingCharacter>(StringComparer.OrdinalIgnoreCase);
		foreach (KeyValuePair<string, List<UISpeaker>> keyValuePair in this.speakPositionToSpeakerPortrait)
		{
			keyValuePair.Value.Clear();
		}
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0004165C File Offset: 0x0003F85C
	public void ShowEmote(string characterName, EmoteReaction emoteReaction, EmoteHeight height)
	{
		UISpeaker[] array = this.speakerPortraits;
		for (int i = 0; i < array.Length; i++)
		{
			UISpeaker speakerPortrait = array[i];
			if (((characterName == "volt" && speakerPortrait.character == characterName) || speakerPortrait.character == Singleton<CharacterHelper>.Instance._characters.GetInternalName(characterName)) && speakerPortrait.isActiveOnStage)
			{
				switch (emoteReaction)
				{
				case EmoteReaction.disgust:
					this.SetGameObjectActive(speakerPortrait.disgustEmote, true);
					speakerPortrait.disgustEmotePS.Play();
					break;
				case EmoteReaction.angry:
					this.SetGameObjectActive(speakerPortrait.angryEmote, true);
					speakerPortrait.angryEmotePS.Play();
					break;
				case EmoteReaction.love:
					this.SetGameObjectActive(speakerPortrait.loveEmote, true);
					speakerPortrait.loveEmotePS.Play();
					break;
				case EmoteReaction.love1:
					this.SetGameObjectActive(speakerPortrait.love2Emote, true);
					speakerPortrait.love2EmotePS.Play();
					break;
				case EmoteReaction.love2:
					this.SetGameObjectActive(speakerPortrait.love3Emote, true);
					speakerPortrait.love3EmotePS.Play();
					break;
				case EmoteReaction.sweat:
					this.SetGameObjectActive(speakerPortrait.sweatEmote, true);
					speakerPortrait.sweatEmotePS.Play();
					break;
				case EmoteReaction.joy:
					this.SetGameObjectActive(speakerPortrait.joyEmote, true);
					speakerPortrait.joyEmotePS.Play();
					break;
				case EmoteReaction.bad:
					this.SetGameObjectActive(speakerPortrait.badEmote, true);
					speakerPortrait.badEmotePS.Play();
					break;
				case EmoteReaction.sleep:
					this.SetGameObjectActive(speakerPortrait.sleepEmote, true);
					speakerPortrait.sleepEmotePS.Play();
					break;
				case EmoteReaction.sing:
					this.SetGameObjectActive(speakerPortrait.singEmote, true);
					speakerPortrait.singEmotePS.Play();
					break;
				case EmoteReaction.violent:
					this.SetGameObjectActive(speakerPortrait.violentEmote, true);
					speakerPortrait.violentEmotePS.Play();
					break;
				case EmoteReaction.good:
					this.SetGameObjectActive(speakerPortrait.goodEmote, true);
					speakerPortrait.goodEmotePS.Play();
					break;
				case EmoteReaction.resolve:
					this.SetGameObjectActive(speakerPortrait.resolveEmote, true);
					speakerPortrait.resolveEmotePS.Play();
					break;
				case EmoteReaction.stop:
					this.HideEmotes(characterName);
					break;
				case EmoteReaction.shake:
				{
					speakerPortrait.transform.DOComplete(true);
					float baseX = speakerPortrait.transform.localPosition.x;
					TweenCallback <>9__4;
					TweenCallback <>9__3;
					TweenCallback <>9__2;
					TweenCallback <>9__1;
					speakerPortrait.transform.DOLocalMoveX(baseX - 100f, 0.15f, false).SetEase(Ease.InQuad).OnComplete(delegate
					{
						TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = speakerPortrait.transform.DOLocalMoveX(baseX + 100f, 0.25f, false).SetEase(Ease.InQuad);
						TweenCallback tweenCallback;
						if ((tweenCallback = <>9__1) == null)
						{
							tweenCallback = (<>9__1 = delegate
							{
								TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore2 = speakerPortrait.transform.DOLocalMoveX(baseX - 100f, 0.25f, false).SetEase(Ease.InQuad);
								TweenCallback tweenCallback2;
								if ((tweenCallback2 = <>9__2) == null)
								{
									tweenCallback2 = (<>9__2 = delegate
									{
										TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore3 = speakerPortrait.transform.DOLocalMoveX(baseX + 100f, 0.25f, false).SetEase(Ease.InQuad);
										TweenCallback tweenCallback3;
										if ((tweenCallback3 = <>9__3) == null)
										{
											tweenCallback3 = (<>9__3 = delegate
											{
												TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore4 = speakerPortrait.transform.DOLocalMoveX(baseX - 100f, 0.25f, false).SetEase(Ease.InQuad);
												TweenCallback tweenCallback4;
												if ((tweenCallback4 = <>9__4) == null)
												{
													tweenCallback4 = (<>9__4 = delegate
													{
														speakerPortrait.transform.DOLocalMoveX(baseX, 0.5f, false).SetEase(Ease.OutBack);
													});
												}
												tweenerCore4.OnComplete(tweenCallback4);
											});
										}
										tweenerCore3.OnComplete(tweenCallback3);
									});
								}
								tweenerCore2.OnComplete(tweenCallback2);
							});
						}
						tweenerCore.OnComplete(tweenCallback);
					});
					break;
				}
				}
				switch (height)
				{
				case EmoteHeight.bottom:
					speakerPortrait.emotes.localPosition = new Vector3(speakerPortrait.emotes.localPosition.x, -100f, speakerPortrait.emotes.localPosition.z);
					break;
				case EmoteHeight.center:
					speakerPortrait.emotes.localPosition = new Vector3(speakerPortrait.emotes.localPosition.x, speakerPortrait.emotes.localPosition.y, speakerPortrait.emotes.localPosition.z);
					break;
				case EmoteHeight.top:
					speakerPortrait.emotes.localPosition = new Vector3(speakerPortrait.emotes.localPosition.x, 100f, speakerPortrait.emotes.localPosition.z);
					break;
				default:
					speakerPortrait.emotes.localPosition = new Vector3(speakerPortrait.emotes.localPosition.x, 0f, speakerPortrait.emotes.localPosition.z);
					break;
				}
				if (emoteReaction != EmoteReaction.shake)
				{
					speakerPortrait.StartEmoteHop();
				}
			}
		}
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x00041AEC File Offset: 0x0003FCEC
	public void HideEmotes(string characterName)
	{
		foreach (UISpeaker uispeaker in this.speakerPortraits)
		{
			if (uispeaker.character == Singleton<CharacterHelper>.Instance._characters.GetInternalName(characterName) && uispeaker.isActiveOnStage)
			{
				this.SetGameObjectActive(uispeaker.angryEmote, false);
				this.SetGameObjectActive(uispeaker.joyEmote, false);
				this.SetGameObjectActive(uispeaker.disgustEmote, false);
				this.SetGameObjectActive(uispeaker.loveEmote, false);
				this.SetGameObjectActive(uispeaker.sweatEmote, false);
				this.SetGameObjectActive(uispeaker.love1Emote, false);
				this.SetGameObjectActive(uispeaker.love2Emote, false);
				this.SetGameObjectActive(uispeaker.love3Emote, false);
				this.SetGameObjectActive(uispeaker.resolveEmote, false);
				this.SetGameObjectActive(uispeaker.violentEmote, false);
				this.SetGameObjectActive(uispeaker.goodEmote, false);
				this.SetGameObjectActive(uispeaker.badEmote, false);
				this.SetGameObjectActive(uispeaker.sleepEmote, false);
				this.SetGameObjectActive(uispeaker.singEmote, false);
			}
		}
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x00041BFC File Offset: 0x0003FDFC
	public void HideEmote(string characterName, EmoteReaction emote)
	{
		foreach (UISpeaker uispeaker in this.speakerPortraits)
		{
			if (uispeaker.character == Singleton<CharacterHelper>.Instance._characters.GetInternalName(characterName) && uispeaker.isActiveOnStage)
			{
				switch (emote)
				{
				case EmoteReaction.disgust:
					uispeaker.disgustEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.angry:
					uispeaker.angryEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.love:
					uispeaker.loveEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.love1:
					uispeaker.love2EmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.love2:
					uispeaker.love3EmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.sweat:
					uispeaker.sweatEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.joy:
					uispeaker.joyEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.bad:
					uispeaker.badEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.sleep:
					uispeaker.sleepEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.sing:
					uispeaker.singEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.violent:
					uispeaker.violentEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.good:
					uispeaker.goodEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.resolve:
					uispeaker.resolveEmotePS.gameObject.SetActive(false);
					break;
				case EmoteReaction.stop:
					this.HideEmotes(characterName);
					break;
				}
			}
		}
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x00041DA6 File Offset: 0x0003FFA6
	private void SetGameObjectActive(GameObject gameObject, bool isActive)
	{
		if (gameObject != null)
		{
			gameObject.SetActive(isActive);
			this.activeEmotes.Add(gameObject);
		}
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x00041DC4 File Offset: 0x0003FFC4
	public void RepositionEnteredCharactersForNewCharacter()
	{
		this.Log(string.Format("RepositionEnteredCharactersForNewCharacter num occupied pos={0}", this.GetNumberOfOccupiedPositions()));
		switch (this.GetNumberOfOccupiedPositions())
		{
		case 0:
		case 3:
		case 4:
		case 5:
			break;
		case 1:
			this.MoveEnteredCharacterToPosition("center", "right");
			return;
		case 2:
			this.MoveEnteredCharacterToPosition("right", "center");
			break;
		default:
			return;
		}
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00041E33 File Offset: 0x00040033
	public void RepositionEnteredCharactersAfterCharacterLeft(SpeakingCharacter characterThatJustLeft)
	{
		this.RepositionEnteredCharactersAfterCharacterLeft(characterThatJustLeft.UIPortrait.targetPosName, characterThatJustLeft.internalName);
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00041E4C File Offset: 0x0004004C
	private void RepositionEnteredCharactersAfterCharacterLeft(string exitedCharacterTargetPosName, string exitedCharacterInternalName)
	{
		this.Log("Moving entered characters after character '" + exitedCharacterInternalName + "' left from position " + exitedCharacterTargetPosName);
		switch (this.GetNumberOfOccupiedPositions())
		{
		case 1:
		{
			bool flag = false;
			using (Dictionary<string, SpeakingCharacter>.Enumerator enumerator = this.internalNameToSpeakingCharacter.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, SpeakingCharacter> keyValuePair = enumerator.Current;
					if (keyValuePair.Value != null && keyValuePair.Value.UIPortrait.isActiveOnStage && !flag)
					{
						this.MoveEnteredCharacterToPosition(keyValuePair.Value.UIPortrait.targetPosName, "center");
						flag = true;
					}
				}
				return;
			}
			break;
		}
		case 2:
			break;
		case 3:
			if (exitedCharacterTargetPosName == "left")
			{
				this.MoveEnteredCharacterToPosition("leftcenter", "left");
				return;
			}
			if (exitedCharacterTargetPosName == "center")
			{
				this.MoveEnteredCharacterToPosition("left", "center");
				this.MoveEnteredCharacterToPosition("leftcenter", "left");
				return;
			}
			if (!(exitedCharacterTargetPosName == "right"))
			{
				return;
			}
			this.MoveEnteredCharacterToPosition("center", "right");
			this.MoveEnteredCharacterToPosition("left", "center");
			this.MoveEnteredCharacterToPosition("leftcenter", "left");
			return;
		case 4:
			if (exitedCharacterTargetPosName == "leftcenter")
			{
				this.MoveEnteredCharacterToPosition("left", "leftcenter");
				this.MoveEnteredCharacterToPosition("center", "left");
				this.MoveEnteredCharacterToPosition("right", "center");
				this.MoveEnteredCharacterToPosition("rightcenter", "right");
				return;
			}
			if (exitedCharacterTargetPosName == "left")
			{
				this.MoveEnteredCharacterToPosition("center", "left");
				this.MoveEnteredCharacterToPosition("right", "center");
				this.MoveEnteredCharacterToPosition("rightcenter", "right");
				return;
			}
			if (exitedCharacterTargetPosName == "center")
			{
				this.MoveEnteredCharacterToPosition("right", "center");
				this.MoveEnteredCharacterToPosition("rightcenter", "right");
				return;
			}
			if (!(exitedCharacterTargetPosName == "right"))
			{
				return;
			}
			this.MoveEnteredCharacterToPosition("rightcenter", "right");
			return;
		default:
			return;
		}
		if (exitedCharacterTargetPosName == "left")
		{
			this.MoveEnteredCharacterToPosition("center", "left");
			return;
		}
		if (!(exitedCharacterTargetPosName == "right"))
		{
			return;
		}
		this.MoveEnteredCharacterToPosition("center", "right");
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x000420B8 File Offset: 0x000402B8
	private void MoveEnteredCharacterToPosition(string fromPosition, string toPosition)
	{
		if (this.speakPositionToSpeakerPortrait[fromPosition].Count == 0)
		{
			DateADex instance = DateADex.Instance;
			if (instance.IsCollectableShowingOrHiding && instance.LastCollectableStagePosition == fromPosition)
			{
				this.Log("Move collectable from " + fromPosition + " => " + toPosition);
				instance.MoveCollectableToPosition(toPosition, this.GetStagePosition(toPosition));
			}
			return;
		}
		if (fromPosition != toPosition)
		{
			List<UISpeaker> list = this.speakPositionToSpeakerPortrait[fromPosition];
			for (int i = 0; i < list.Count; i++)
			{
				UISpeaker uispeaker = list[i];
				this.Log(string.Concat(new string[] { "Move character '", uispeaker.character, "' from '", fromPosition, "' => '", toPosition, "'" }));
				this.speakPositionToSpeakerPortrait[toPosition].Add(uispeaker);
				foreach (KeyValuePair<string, SpeakingCharacter> keyValuePair in this.internalNameToSpeakingCharacter)
				{
					SpeakingCharacter value = keyValuePair.Value;
					if (value.UIPortrait == uispeaker)
					{
						value.SetUIPortraitPosition(this.GetStagePosition(toPosition));
						value.UIPortrait.SetTargetPositionName(toPosition);
						break;
					}
				}
			}
			this.speakPositionToSpeakerPortrait[fromPosition].Clear();
		}
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00042228 File Offset: 0x00040428
	public void RepositionEnteredCharactersAfterCollectableReleased()
	{
		string lastCollectableStagePosition = DateADex.Instance.LastCollectableStagePosition;
		this.RepositionEnteredCharactersAfterCharacterLeft(lastCollectableStagePosition, "Collectable");
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0004224C File Offset: 0x0004044C
	private int GetNumberOfOccupiedPositions()
	{
		int num = 0;
		foreach (KeyValuePair<string, List<UISpeaker>> keyValuePair in this.speakPositionToSpeakerPortrait)
		{
			if (keyValuePair.Value.Count > 0)
			{
				num++;
			}
		}
		if (DateADex.Instance.IsCollectableShowingOrHiding)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x000422C0 File Offset: 0x000404C0
	public void Log(string message)
	{
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x000422C4 File Offset: 0x000404C4
	[Conditional("STAGE_MANAGER_LOGGING")]
	private void LogInternal(string message)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[STAGE MANAGER]           Current active characters:");
		foreach (KeyValuePair<string, List<UISpeaker>> keyValuePair in this.speakPositionToSpeakerPortrait)
		{
			if (keyValuePair.Value.Count > 0)
			{
				foreach (UISpeaker uispeaker in keyValuePair.Value)
				{
					stringBuilder.Append("  ");
					stringBuilder.Append(uispeaker.character);
					stringBuilder.Append(" at ");
					stringBuilder.Append(keyValuePair.Key);
				}
			}
		}
		if (DateADex.Instance.IsCollectableShowingOrHiding)
		{
			stringBuilder.Append("  Collectable at ");
			stringBuilder.Append(DateADex.Instance.LastCollectableStagePosition);
		}
	}

	// Token: 0x04000A40 RID: 2624
	private Image bgImg;

	// Token: 0x04000A41 RID: 2625
	private GameObject foregroundImg;

	// Token: 0x04000A42 RID: 2626
	[SerializeField]
	private Dictionary<string, SpeakingCharacter> internalNameToSpeakingCharacter;

	// Token: 0x04000A43 RID: 2627
	private HashSet<string> enteredCharacterSet;

	// Token: 0x04000A44 RID: 2628
	private UISpeaker[] speakerPortraits;

	// Token: 0x04000A45 RID: 2629
	private List<GameObject> activeEmotes = new List<GameObject>();

	// Token: 0x04000A46 RID: 2630
	public static HashSet<string> speakPositionNames = new HashSet<string> { "offscreenright", "rightcenter", "right", "offscreenleft", "left", "leftcenter", "center" };

	// Token: 0x04000A47 RID: 2631
	private List<string> OrderToCheck = new List<string> { "center", "leftcenter", "rightcenter", "right", "left" };

	// Token: 0x04000A48 RID: 2632
	public Transform speakerPositions;

	// Token: 0x04000A49 RID: 2633
	public Dictionary<string, List<UISpeaker>> speakPositionToSpeakerPortrait;

	// Token: 0x04000A4A RID: 2634
	private SpeakingCharacter _currentSpeaker;
}
