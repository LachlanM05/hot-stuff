using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CowberryStudios.ProjectAI;
using Rewired;
using T17.Services;
using Team17.Common;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class DialogueTagManager
{
	// Token: 0x06000268 RID: 616 RVA: 0x0000DE2F File Offset: 0x0000C02F
	public DialogueTagManager(StageManager _stageManagerRef)
	{
		this.stageManagerRef = _stageManagerRef;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000DE3E File Offset: 0x0000C03E
	public void SetBinding(InkBinding binding)
	{
		binding.Bind(this);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000DE47 File Offset: 0x0000C047
	public void ResetBinding(InkBinding binding)
	{
		binding.Unbind(this);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000DE50 File Offset: 0x0000C050
	[Command("setglobalint", Exactly = 2)]
	public void SetGlobalVariableInt(InkCommand command)
	{
		PlayerPrefs.SetInt(command[0], int.Parse(command[1]));
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000DE6A File Offset: 0x0000C06A
	[Command("getglobalint", Exactly = 1)]
	public void GetGlobalVariableInt(InkCommand command)
	{
		Singleton<InkController>.Instance.story.variablesState[command[0]] = PlayerPrefs.GetInt(command[0]);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000DE98 File Offset: 0x0000C098
	[Command("setglobalstring", Exactly = 2)]
	public void SetGlobalVariableString(InkCommand command)
	{
		PlayerPrefs.SetString(command[0], command[1]);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000DEAD File Offset: 0x0000C0AD
	[Command("getglobalstring", Exactly = 1)]
	public void GetGlobalVariableString(InkCommand command)
	{
		Singleton<InkController>.Instance.story.variablesState[command[0]] = PlayerPrefs.GetString(command[0]);
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000DED6 File Offset: 0x0000C0D6
	[Command("setglobalbool", Exactly = 1)]
	public void SetGlobalVariableBool(InkCommand command)
	{
		PlayerPrefs.SetString(command[0], command[1]);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000DEEB File Offset: 0x0000C0EB
	[Command("getglobalbool", Exactly = 1)]
	public void GetGlobalVariableBool(InkCommand command)
	{
		Singleton<InkController>.Instance.story.variablesState[command[0]] = PlayerPrefs.GetString(command[0]).ToLowerInvariant() == "true";
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000DF28 File Offset: 0x0000C128
	public bool TryGetInternalName(string nameToCheck, out string internalName)
	{
		string text = nameToCheck.ToLowerInvariant().Trim();
		if (string.IsNullOrEmpty(text))
		{
			internalName = "";
			return false;
		}
		if (!Singleton<CharacterHelper>.Instance._characters.IsNameInSet(text))
		{
			internalName = Singleton<CharacterHelper>.Instance._characters.GetInternalName(text);
			return !(internalName == text);
		}
		internalName = text;
		return true;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000DF88 File Offset: 0x0000C188
	public string GetRandomInternalName()
	{
		List<string> list = Singleton<CharacterHelper>.Instance._characters.ToList<string>();
		int num = Random.Range(0, list.Count);
		return list[num];
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000DFBC File Offset: 0x0000C1BC
	[Command("forcesleep")]
	public void ForceSleep(InkCommand command)
	{
		Singleton<GameController>.Instance.StartStateTransition(VIEW_STATE.HOUSE, false);
		GameObject gameObject = GameObject.FindWithTag("Bed");
		if (gameObject != null)
		{
			gameObject.GetComponent<Bed>().Sleep();
		}
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
	[Command("force_sleep_skip_tutorial")]
	public void ForceSleepSkipTutorial(InkCommand command)
	{
		Singleton<GameController>.Instance.StartStateTransition(VIEW_STATE.HOUSE, false);
		GameObject gameObject = GameObject.FindWithTag("Bed");
		if (gameObject != null)
		{
			gameObject.GetComponent<Bed>().Sleep();
		}
		if (Singleton<Dateviators>.Instance.IsEquipped)
		{
			Singleton<Dateviators>.Instance.Dequip();
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000E044 File Offset: 0x0000C244
	[Command("AUDIO_NAME", Exactly = 1)]
	public void SetAudioFromName(InkCommand command)
	{
		if (!Singleton<AudioManager>.Instance.recentVOFound)
		{
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_text_writing.name, 0f);
		}
		string text = "";
		string[] array = Singleton<InkController>.Instance.GetInkKnotLoaded().Split(".", StringSplitOptions.None);
		if (array.Count<string>() > 0)
		{
			text = array[0];
		}
		string text2 = Singleton<CharacterHelper>.Instance._characters.GetInternalName(text);
		if (command[0].StartsWith("reggie_"))
		{
			text2 = "reggie";
		}
		else if (text2.Contains("epilogue_final"))
		{
			text2 = text2.Replace("epilogue_final", "epilogue");
		}
		else if (text2.Contains("cf_cf"))
		{
			text2 = text2.Replace("cf_cf", "cf");
		}
		string text3 = command[0];
		string text4 = Path.Combine("english", text2, text3);
		Singleton<AudioManager>.Instance.PlayTrack(text4, AUDIO_TYPE.DIALOGUE, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
		if (!Singleton<AudioManager>.Instance.recentVOFound && Singleton<GameController>.Instance.GetCurrentActiveDatable().KnotName().Contains("sassy"))
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_text_writing, AUDIO_TYPE.DIALOGUE, false, false, 0f, false, 1f, null, true, SFX_SUBGROUP.NONE, false);
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000E194 File Offset: 0x0000C394
	[Command("AUDIO_SFX", Exactly = 1)]
	[Command("SFX", Exactly = 1)]
	public void SetAudioSFXFromName(InkCommand command)
	{
		string text = "";
		string[] array = Singleton<InkController>.Instance.GetInkKnotLoaded().Split(".", StringSplitOptions.None);
		if (array.Count<string>() > 0)
		{
			text = array[0];
		}
		Singleton<CharacterHelper>.Instance._characters.GetInternalName(text);
		command[0].StartsWith("reggie_");
		string text2 = Path.Combine("Dialogue_SFX", command[0]);
		Singleton<AudioManager>.Instance.PlayTrack(text2, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.EMOTE, null, false);
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000E220 File Offset: 0x0000C420
	[Command("AUDIO_SFX_LOOP_ON", Exactly = 1)]
	[Command("SFX_LOOP_ON", Exactly = 1)]
	public void SetAudioSFXLoopFromName(InkCommand command)
	{
		string text = "";
		string[] array = Singleton<InkController>.Instance.GetInkKnotLoaded().Split(".", StringSplitOptions.None);
		if (array.Count<string>() > 0)
		{
			text = array[0];
		}
		Singleton<CharacterHelper>.Instance._characters.GetInternalName(text);
		command[0].StartsWith("reggie_");
		string text2 = Path.Combine("Dialogue_SFX", command[0]);
		Singleton<AudioManager>.Instance.PlayTrack(text2, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, true, null, SFX_SUBGROUP.EMOTE, null, false);
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000E2AC File Offset: 0x0000C4AC
	[Command("AUDIO_SFX_LOOP_OFF", Exactly = 1)]
	[Command("SFX_LOOP_OFF", Exactly = 1)]
	public void EndAudioSFXLoopFromName(InkCommand command)
	{
		string text = "";
		string[] array = Singleton<InkController>.Instance.GetInkKnotLoaded().Split(".", StringSplitOptions.None);
		if (array.Count<string>() > 0)
		{
			text = array[0];
		}
		Singleton<CharacterHelper>.Instance._characters.GetInternalName(text);
		command[0].StartsWith("reggie_");
		string text2 = Path.Combine("Dialogue_SFX", command[0]);
		Singleton<AudioManager>.Instance.StopTrack(text2, 0.25f);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000E328 File Offset: 0x0000C528
	[Command("MUSIC", GreaterThanOrEqual = 1)]
	public void SetMusicFromNameDefault(InkCommand command)
	{
		if (command.Count == 2)
		{
			Singleton<AudioManager>.Instance.PlayTrackWithIntro(command[1], command[0]);
			return;
		}
		if (command[0] == "realize")
		{
			Singleton<AudioManager>.Instance.PlayTrack("mysteriousbox_music", AUDIO_TYPE.MUSIC, true, false, 3f, false, 1f, null, true, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		if (command[0].Contains("bodhi_song") || command[0].Contains("miranda_ballad") || command[0].Contains("keyes_concerto"))
		{
			Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.SPECIAL, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		if (command[0].Contains("epilogue_ending"))
		{
			Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.MUSIC, true, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.STINGER, null, false);
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.MUSIC, true, false, 0f, false, 1f, null, true, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000E450 File Offset: 0x0000C650
	[Command("MUSIC_NO_LOOP", GreaterThanOrEqual = 1)]
	public void SetMusicFromNameDefaultNoLoop(InkCommand command)
	{
		if (command.Count == 2)
		{
			Singleton<AudioManager>.Instance.PlayTrackWithIntro(command[1], command[0]);
			return;
		}
		if (command[0].Contains("bodhi_song") || command[0].Contains("miranda_ballad") || command[0].Contains("keyes_concerto"))
		{
			Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.SPECIAL, true, false, 0.5f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.MUSIC, true, false, 0.5f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000E508 File Offset: 0x0000C708
	[Command("MUSIC_FADE_FAST", GreaterThanOrEqual = 0)]
	public void SetMusicFromName(InkCommand command)
	{
		if (command.Count == 0)
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.SPECIAL, 0.5f);
			return;
		}
		if (command.Count == 2)
		{
			Singleton<AudioManager>.Instance.PlayTrackWithIntro(command[1], command[0]);
			return;
		}
		if (command[0].Contains("bodhi_song") || command[0].Contains("miranda_ballad") || command[0].Contains("keyes_concerto"))
		{
			Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.SPECIAL, true, false, 0.1f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.MUSIC, true, false, 0.1f, false, 1f, null, true, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
	[Command("MUSIC_FADE_SLOW", GreaterThanOrEqual = 0)]
	public void SetMusicFromNameSlow(InkCommand command)
	{
		if (command.Count == 0)
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 1.5f);
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.SPECIAL, 1.5f);
			return;
		}
		if (command.Count == 2)
		{
			Singleton<AudioManager>.Instance.PlayTrackWithIntro(command[1], command[0]);
			return;
		}
		if (command[0].Contains("bodhi_song") || command[0].Contains("miranda_ballad") || command[0].Contains("keyes_concerto"))
		{
			Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.SPECIAL, true, false, 1.5f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.MUSIC, true, false, 1.5f, false, 1f, null, true, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
	[Command("MUSIC_FADE_MEDIUM ", GreaterThanOrEqual = 0)]
	public void SetMusicFromNameMedium(InkCommand command)
	{
		if (command.Count == 0)
		{
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 1f);
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.SPECIAL, 1f);
			return;
		}
		if (command.Count == 2)
		{
			Singleton<AudioManager>.Instance.PlayTrackWithIntro(command[1], command[0]);
			return;
		}
		if (command[0].Contains("bodhi_song") || command[0].Contains("miranda_ballad") || command[0].Contains("keyes_concerto"))
		{
			Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.SPECIAL, true, false, 1f, false, 1f, null, false, null, SFX_SUBGROUP.NONE, null, false);
			return;
		}
		Singleton<AudioManager>.Instance.PlayTrack(command[0], AUDIO_TYPE.MUSIC, true, false, 1f, false, 1f, null, true, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
	[Command("EXIT", Exactly = 1)]
	public void CharacterExit(InkCommand command)
	{
		string text = "";
		this.TryGetInternalName(command[0], out text);
		this.stageManagerRef.OnCharacterExit(text);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_exit, AUDIO_TYPE.SFX, false, false, 0f, false, 0f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000E800 File Offset: 0x0000CA00
	[Command("ENTER_POSE", GreaterThanOrEqual = 2)]
	public void CharacterEnterPose(InkCommand command)
	{
		bool flag = true;
		E_General_Poses e_General_Poses = E_General_Poses.neutral;
		bool flag2 = false;
		string text = "";
		string text2;
		if (this.TryGetInternalName(command[0], out text2))
		{
			string text3 = command[1];
			if (text3 == "human")
			{
				text3 = "realized";
			}
			Enum.TryParse<E_General_Poses>(text3, true, out e_General_Poses);
			if (command.Count > 2)
			{
				string text4 = StageManager.ParsePositionNames(command[2].ToLowerInvariant().Trim());
				if (StageManager.speakPositionNames.Contains(text4))
				{
					flag2 = true;
					text = text4;
					flag = false;
				}
			}
		}
		this.stageManagerRef.Log("CharacterEnterPose '" + text2 + "'");
		if (!this.stageManagerRef.IsAnEnteredCharacter(text2) && flag)
		{
			this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		}
		if (flag2)
		{
			this.stageManagerRef.OnCharacterEnter(text2, e_General_Poses, text);
		}
		else
		{
			this.stageManagerRef.OnCharacterEnter(text2, e_General_Poses);
		}
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_enter_fast, AUDIO_TYPE.SFX, false, false, 0f, false, 0f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0000E90C File Offset: 0x0000CB0C
	[Command("ENTER_EXPR", GreaterThanOrEqual = 3)]
	public void CharacterEnterExpression(InkCommand command)
	{
		bool flag = true;
		E_General_Poses e_General_Poses = E_General_Poses.neutral;
		E_Facial_Expressions e_Facial_Expressions = E_Facial_Expressions.neutral;
		bool flag2 = false;
		string text = "";
		string text2;
		if (this.TryGetInternalName(command[0], out text2))
		{
			string text3 = command[1];
			if (text3 == "human")
			{
				text3 = "realized";
			}
			Enum.TryParse<E_General_Poses>(text3, true, out e_General_Poses);
			Enum.TryParse<E_Facial_Expressions>(command[2], true, out e_Facial_Expressions);
			if (command.Count > 3)
			{
				string text4 = StageManager.ParsePositionNames(command[3].ToLowerInvariant().Trim());
				if (StageManager.speakPositionNames.Contains(text4))
				{
					flag2 = true;
					text = text4;
					flag = false;
				}
			}
		}
		if (!this.stageManagerRef.IsAnEnteredCharacter(text2) && flag)
		{
			this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		}
		if (flag2)
		{
			this.stageManagerRef.OnCharacterEnter(text2, e_General_Poses, e_Facial_Expressions, text);
		}
		else
		{
			this.stageManagerRef.OnCharacterEnter(text2, e_General_Poses, e_Facial_Expressions);
		}
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_enter_fast, AUDIO_TYPE.SFX, false, false, 0f, false, 0f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000EA14 File Offset: 0x0000CC14
	[Command("ENTER", GreaterThanOrEqual = 1)]
	public void CharacterEnter(InkCommand command)
	{
		this.TreatEnterCharacter(command, false);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_enter_fast, AUDIO_TYPE.SFX, false, false, 0f, false, 0f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000EA50 File Offset: 0x0000CC50
	[Command("ENTER_SLOW", GreaterThanOrEqual = 1)]
	public void CharacterEnterSlow(InkCommand command)
	{
		this.TreatEnterCharacter(command, true);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_enter_slow, AUDIO_TYPE.SFX, false, false, 0f, false, 0f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000EA8C File Offset: 0x0000CC8C
	private void TreatEnterCharacter(InkCommand command, bool isSlow)
	{
		string text = "";
		bool flag = true;
		string text2;
		if (!this.TryGetInternalName(command[0], out text2))
		{
			if (command.Count > 1)
			{
				string text3 = StageManager.ParsePositionNames(command[1].ToLowerInvariant().Trim());
				if (StageManager.speakPositionNames.Contains(text3))
				{
					text = text3;
					flag = false;
				}
			}
			this.stageManagerRef.Log("Enter Character '" + text2 + "'");
			if (!this.stageManagerRef.IsAnEnteredCharacter(text2) && flag)
			{
				this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.stageManagerRef.OnCharacterEnter(text2, text, isSlow);
				return;
			}
			this.stageManagerRef.OnCharacterEnter(text2, isSlow);
			return;
		}
		else
		{
			if (command.Count > 1)
			{
				string text4 = StageManager.ParsePositionNames(command[1].ToLowerInvariant().Trim());
				if (StageManager.speakPositionNames.Contains(text4))
				{
					text = text4;
					flag = false;
				}
			}
			this.stageManagerRef.Log("Enter Character '" + text2 + "' ");
			if (!this.stageManagerRef.IsAnEnteredCharacter(text2) && flag)
			{
				this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.stageManagerRef.OnCharacterEnter(text2, text, isSlow);
				return;
			}
			this.stageManagerRef.OnCharacterEnter(text2, isSlow);
			return;
		}
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
	[Command("MOVE", GreaterThanOrEqual = 1)]
	public void CharacterMove(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		string text2 = StageManager.ParsePositionNames(command[0].Replace("to:", ""));
		if (command.Count == 2)
		{
			this.TryGetInternalName(command[0], out text);
			text2 = command[1].Replace("to:", "");
		}
		text2 = StageManager.ParsePositionNames(text2.ToLowerInvariant().Trim());
		this.stageManagerRef.Log(string.Concat(new string[] { "CharacterMove (manual) '", text, "' -> '", text2, "'" }));
		if (StageManager.speakPositionNames.Contains(text2))
		{
			this.stageManagerRef.MoveCharacterToPosition(text, text2);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_dateable_move, AUDIO_TYPE.SFX, false, false, 0f, false, 0f, null, false, SFX_SUBGROUP.UI, false);
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x0000ECC8 File Offset: 0x0000CEC8
	[Command("FACE", GreaterThanOrEqual = 1)]
	public void CharacterFace(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (command.Count == 1)
		{
			UISpeaker.E_Direction e_Direction;
			if (Enum.TryParse<UISpeaker.E_Direction>(command[0].Trim().ToLowerInvariant(), true, out e_Direction))
			{
				this.stageManagerRef.SetFacingDirection(text, e_Direction);
				return;
			}
		}
		else if (command.Count == 2)
		{
			this.TryGetInternalName(command[0], out text);
			UISpeaker.E_Direction e_Direction2;
			if (Enum.TryParse<UISpeaker.E_Direction>(command[1].Trim().ToLowerInvariant(), true, out e_Direction2))
			{
				this.stageManagerRef.SetFacingDirection(text, e_Direction2);
			}
		}
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000ED58 File Offset: 0x0000CF58
	[Command("COLLECTABLE", GreaterThanOrEqual = 1)]
	public void UnlockCollectable(InkCommand command)
	{
		this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (!this.TryGetInternalName(command[0], out text))
		{
			return;
		}
		this.stageManagerRef.Log("Unlock collectable " + text);
		int num;
		if (int.TryParse(command[1].Trim(), out num))
		{
			DateADex.Instance.UnlockCollectable(text, num, false, false, true);
		}
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000EDCC File Offset: 0x0000CFCC
	[Command("COLLECTABLE_DONT_MOVE", GreaterThanOrEqual = 1)]
	public void UnlockCollectableDontMove(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (!this.TryGetInternalName(command[0], out text))
		{
			return;
		}
		int num;
		if (int.TryParse(command[1].Trim(), out num))
		{
			DateADex.Instance.UnlockCollectable(text, num, false, false, false);
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0000EE20 File Offset: 0x0000D020
	[Command("COLLECTABLE_HIDDEN", GreaterThanOrEqual = 1)]
	public void UnlockCollectableHidden(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (!this.TryGetInternalName(command[0], out text))
		{
			return;
		}
		int num;
		if (int.TryParse(command[1].Trim(), out num))
		{
			DateADex.Instance.UnlockCollectable(text, num, true, false, true);
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x0000EE74 File Offset: 0x0000D074
	[Command("COLLECTABLE_SCREEN_ONLY", GreaterThanOrEqual = 1)]
	public void UnlockCollectableScreenOnly(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		if (!this.TryGetInternalName(command[0], out text))
		{
			return;
		}
		int num;
		if (int.TryParse(command[1].Trim(), out num))
		{
			DateADex.Instance.UnlockCollectable(text, num, false, true, true);
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000EED4 File Offset: 0x0000D0D4
	[Command("COLLECTABLE_TIME", Exactly = 3)]
	public void UnlockCollectableTime(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		int num;
		int num2;
		if (this.TryGetInternalName(command[0], out text) && int.TryParse(command[1].Trim(), out num) && int.TryParse(command[2].Trim(), out num2))
		{
			DateADex.Instance.UnlockCollectable(text, num, num2);
		}
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000EF44 File Offset: 0x0000D144
	[Command("COLLECTABLE_HOLD", Exactly = 2)]
	public void UnlockCollectableHold(InkCommand command)
	{
		if (Singleton<GameController>.Instance.viewState != VIEW_STATE.TALKING)
		{
			return;
		}
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		if (!this.TryGetInternalName(command[0], out text))
		{
			return;
		}
		this.stageManagerRef.Log("Unlock Collectable hold " + text + " ");
		int num;
		if (int.TryParse(command[1].Trim(), out num))
		{
			string text2;
			this.stageManagerRef.TryGetToPositionForNewCharacter(out text2, false, false);
			this.stageManagerRef.GetStagePosition(text2);
			DateADex.Instance.UnlockCollectableHold(text, num, false, false);
		}
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
	[Command("COLLECTABLE_SCREEN_ONLY_HOLD", Exactly = 2)]
	public void UnlockCollectableScreenOnlyHold(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		if (!this.TryGetInternalName(command[0], out text))
		{
			return;
		}
		int num;
		if (int.TryParse(command[1].Trim(), out num))
		{
			DateADex.Instance.UnlockCollectableHold(text, num, false, true);
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000F045 File Offset: 0x0000D245
	[Command("COLLECTABLE_RELEASE", GreaterThanOrEqual = 0)]
	[Command("COLLECTABLE_SCREEN_ONLY_RELEASE", GreaterThanOrEqual = 0)]
	public void UnlockCollectableRelease(InkCommand command)
	{
		this.stageManagerRef.Log("Unlock collectable Release");
		DateADex.Instance.UnlockCollectableRelease();
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000F064 File Offset: 0x0000D264
	[Command("MAGIC_COLLECTABLE_HOLD", Exactly = 1)]
	public void UnlockMagicCollectableHold(InkCommand command)
	{
		string randomInternalName = this.GetRandomInternalName();
		this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		int num;
		if (int.TryParse(command[0].Trim(), out num))
		{
			DateADex.Instance.UnlockCollectableHold(randomInternalName, num, false, false);
		}
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
	[Command("EXPRESSION", Exactly = 2)]
	[Command("EXPR", Exactly = 2)]
	public void SetCharacterExpression(InkCommand command)
	{
		E_Facial_Expressions e_Facial_Expressions;
		if (Enum.TryParse<E_Facial_Expressions>(command[1], true, out e_Facial_Expressions))
		{
			this.stageManagerRef.SetCharacterExpression(command[0], e_Facial_Expressions);
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000F0DC File Offset: 0x0000D2DC
	[Command("POSE", GreaterThanOrEqual = 2)]
	[Command("POSE_EXPR", GreaterThanOrEqual = 2)]
	[Command("POSE_EXPRESSION", GreaterThanOrEqual = 2)]
	[Command("POSEEXPRESSION", GreaterThanOrEqual = 2)]
	public void SetCharacterPoseExpression(InkCommand command)
	{
		string text = "";
		this.TryGetInternalName(command[0], out text);
		if (command[0] == "clarence")
		{
			text = "clarence";
		}
		string text2 = command[1];
		if (text2 == "human")
		{
			text2 = "realized";
		}
		E_General_Poses e_General_Poses;
		if (!Enum.TryParse<E_General_Poses>(text2, true, out e_General_Poses))
		{
			e_General_Poses = E_General_Poses.neutral;
		}
		string text3 = "";
		if (command.Count > 2)
		{
			text3 = command[2];
		}
		E_Facial_Expressions e_Facial_Expressions;
		if (!Enum.TryParse<E_Facial_Expressions>(text3, true, out e_Facial_Expressions))
		{
			e_Facial_Expressions = E_Facial_Expressions.neutral;
		}
		if (!this.stageManagerRef.IsAnEnteredCharacter(text))
		{
			this.stageManagerRef.Log(string.Format("Bringing character on via pose character {0} : {1}", text, e_Facial_Expressions));
			this.stageManagerRef.RepositionEnteredCharactersForNewCharacter();
		}
		this.stageManagerRef.SetCharacterPoseExpression(text, e_General_Poses, e_Facial_Expressions);
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000F1AC File Offset: 0x0000D3AC
	[Command("BACKGROUND", Exactly = 1)]
	public void SetBackgroundSprite(InkCommand command)
	{
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Postcards", command[0]), false);
		if (sprite)
		{
			this.stageManagerRef.SetBackgroundImage(sprite);
			Singleton<DayNightCycle>.Instance.ClearFadeToBlack();
			return;
		}
		sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Backgrounds", command[0]), false);
		if (sprite)
		{
			this.stageManagerRef.SetBackgroundImage(sprite);
			Singleton<DayNightCycle>.Instance.ClearFadeToBlack();
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000F23A File Offset: 0x0000D43A
	[Command("BACKGROUND_REMOVE", Exactly = 0)]
	public void RemoveBackgroundSprite(InkCommand command)
	{
		this.stageManagerRef.SetBackgroundImage(null);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000F248 File Offset: 0x0000D448
	[Command("FOREGROUND", Exactly = 1)]
	public void SetForegroundSprite(InkCommand command)
	{
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Foregrounds", command[0]), false);
		if (sprite)
		{
			this.stageManagerRef.SetForegroundImage(sprite);
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0000F28C File Offset: 0x0000D48C
	[Command("SPOTLIGHT_ON", GreaterThanOrEqual = 0)]
	public void SpotlightOn(InkCommand command)
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_spotlight, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
		TalkingUI.Instance.ShowSpotlight();
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000F2C9 File Offset: 0x0000D4C9
	[Command("SPOTLIGHT_OFF", GreaterThanOrEqual = 0)]
	public void SpotlightOff(InkCommand command)
	{
		TalkingUI.Instance.HideSpotlight();
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000F2D8 File Offset: 0x0000D4D8
	[Command("ENDING", GreaterThanOrEqual = 1)]
	public void UnlockEnding(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		string text2 = command[0].Trim();
		if (command.Count > 1)
		{
			text = command[1].Trim();
		}
		RelationshipStatus relationshipStatus = DateADex.ParseRelationshipStatus(text2);
		if (relationshipStatus == RelationshipStatus.Single && command.Count > 1)
		{
			text = command[0].Trim();
			relationshipStatus = DateADex.ParseRelationshipStatus(command[1].Trim());
		}
		this.TreatUnlockEnding(text, relationshipStatus, false);
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000F354 File Offset: 0x0000D554
	[Command("ENDING_LUNA", GreaterThanOrEqual = 1)]
	public void UnlockEndingForLuna(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		string text2 = command[0].Trim();
		if (command.Count > 1)
		{
			text = command[1].Trim();
		}
		RelationshipStatus relationshipStatus = DateADex.ParseRelationshipStatus(text2);
		if (relationshipStatus == RelationshipStatus.Single && command.Count > 1)
		{
			text = command[0].Trim();
			relationshipStatus = DateADex.ParseRelationshipStatus(command[1].Trim());
		}
		this.TreatUnlockEnding(text, relationshipStatus, true);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000F3D0 File Offset: 0x0000D5D0
	private void TreatUnlockEnding(string internalName, RelationshipStatus relationshipStatus, bool skipStatusChange)
	{
		Singleton<AudioManager>.Instance.PauseTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		Singleton<GameController>.Instance.characterEndings.Add(internalName);
		bool flag = false;
		if (internalName == "reggie2")
		{
			flag = true;
			internalName = "reggie";
		}
		DateADex.Instance.UnlockEnding(internalName, relationshipStatus, false, skipStatusChange);
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.2f);
		if (relationshipStatus == RelationshipStatus.Hate)
		{
			string text = Path.Combine("VoiceOver", "hate", internalName);
			if (internalName == "lucinda")
			{
				string variable = Singleton<InkController>.Instance.GetVariable("lucinda_final");
				if (variable == "lust")
				{
					text = Path.Combine("VoiceOver", "hate", "lucinda_lust");
				}
				else if (variable == "loving")
				{
					text = Path.Combine("VoiceOver", "hate", "lucinda_loving");
				}
				else if (variable == "limitless")
				{
					text = Path.Combine("VoiceOver", "hate", "lucinda_limitless");
				}
				else if (variable == "lucid")
				{
					text = Path.Combine("VoiceOver", "hate", "lucinda_lucid");
				}
			}
			else if (internalName == "front" || internalName == "trap" || internalName == "tiny" || internalName == "back")
			{
				text = Path.Combine("VoiceOver", "hate", "dorian");
			}
			Singleton<AudioManager>.Instance.PlayTrackAfterWait(text, AUDIO_TYPE.SFX, false, false, 0.5f, true, 1f, 4.5f);
		}
		else if (relationshipStatus == RelationshipStatus.Love)
		{
			string text2;
			if (flag)
			{
				text2 = Path.Combine("VoiceOver", "love", "reggie2");
			}
			else if (internalName == "lucinda")
			{
				string variable2 = Singleton<InkController>.Instance.GetVariable("lucinda_final");
				if (variable2 == "lust")
				{
					text2 = Path.Combine("VoiceOver", "love", "lucinda_lust");
				}
				else if (variable2 == "loving")
				{
					text2 = Path.Combine("VoiceOver", "love", "lucinda_loving");
				}
				else if (variable2 == "limitless")
				{
					text2 = Path.Combine("VoiceOver", "love", "lucinda_limitless");
				}
				else if (variable2 == "lucid")
				{
					text2 = Path.Combine("VoiceOver", "love", "lucinda_lucid");
				}
				else
				{
					text2 = Path.Combine("VoiceOver", "love", internalName);
				}
			}
			else if (internalName == "front" || internalName == "trap" || internalName == "tiny" || internalName == "back")
			{
				text2 = Path.Combine("VoiceOver", "love", "dorian");
			}
			else
			{
				text2 = Path.Combine("VoiceOver", "love", internalName);
			}
			Singleton<AudioManager>.Instance.PlayTrackAfterWait(text2, AUDIO_TYPE.SFX, false, false, 0.5f, true, 1f, 6f);
		}
		else if (relationshipStatus == RelationshipStatus.Friend)
		{
			string text3 = Path.Combine("VoiceOver", "friend", internalName);
			if (internalName == "lucinda")
			{
				string variable3 = Singleton<InkController>.Instance.GetVariable("lucinda_final");
				if (variable3 == "lust")
				{
					text3 = Path.Combine("VoiceOver", "friend", "lucinda_lavish2");
				}
				else if (variable3 == "loving")
				{
					text3 = Path.Combine("VoiceOver", "friend", "lucinda_lavish2");
				}
				else if (variable3 == "limitless")
				{
					text3 = Path.Combine("VoiceOver", "friend", "lucinda_lavish2");
				}
				else if (variable3 == "lucid")
				{
					text3 = Path.Combine("VoiceOver", "friend", "lucinda_lucid");
				}
			}
			else if (internalName == "front" || internalName == "trap" || internalName == "tiny" || internalName == "back")
			{
				text3 = Path.Combine("VoiceOver", "friend", "dorian");
			}
			Singleton<AudioManager>.Instance.PlayTrackAfterWait(text3, AUDIO_TYPE.SFX, false, false, 0.5f, true, 1f, 4.5f);
		}
		if (relationshipStatus == RelationshipStatus.Hate && Singleton<Save>.Instance.GetDateStatus("reggie_rejection") == RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatusRealized("reggie_rejection") != RelationshipStatus.Realized)
		{
			Singleton<PhoneManager>.Instance.StartChatWithDateable("reggie_rejection.hate_sorter");
		}
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000F860 File Offset: 0x0000DA60
	[Command("NAME", GreaterThanOrEqual = 1)]
	public void CharacterTemporaryAwakening(InkCommand command)
	{
		string text = command[0].Trim();
		Singleton<Save>.Instance.TemporaryUnlockAwakening(text);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000F888 File Offset: 0x0000DA88
	[Command("AWAKENED", GreaterThanOrEqual = 0)]
	[Command("AWAKEN", GreaterThanOrEqual = 0)]
	public void Awaken(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (command.Count > 0)
		{
			text = command[0].Trim();
		}
		if (Singleton<InkController>.Instance.story.variablesState.Contains(text + "_awakened"))
		{
			Singleton<InkController>.Instance.story.variablesState[text + "_awakened"] = true;
		}
		if ((text == "front" || text == "tiny" || text == "trap" || text == "back") && Singleton<InkController>.Instance.story.variablesState.Contains("dorian_awakened"))
		{
			Singleton<InkController>.Instance.story.variablesState["dorian_awakened"] = true;
		}
		string text2 = Path.Combine("VoiceOver", "awaken", text);
		DateADex.Instance.UnlockAwakening(text, RelationshipStatus.Single);
		if (text == "front")
		{
			text2 = Path.Combine("VoiceOver", "awaken", "dorian_front");
		}
		else if (text == "tiny")
		{
			text2 = Path.Combine("VoiceOver", "awaken", "dorian_tiny");
		}
		else if (text == "trap")
		{
			text2 = Path.Combine("VoiceOver", "awaken", "dorian_trap");
		}
		else if (text == "back")
		{
			text2 = Path.Combine("VoiceOver", "awaken", "dorian_back");
		}
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.1f);
		Singleton<AudioManager>.Instance.PauseTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		Singleton<AudioManager>.Instance.PlayTrackAfterWait(text2, AUDIO_TYPE.SFX, false, false, 0.3f, true, 1f, 3f);
		DateADex.Instance.UpdateDateStatusInkVariables();
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000FA64 File Offset: 0x0000DC64
	[Command("REALIZED", GreaterThanOrEqual = 0)]
	[Command("REALIZE", GreaterThanOrEqual = 0)]
	[Command("REALISED", GreaterThanOrEqual = 0)]
	[Command("REALISE", GreaterThanOrEqual = 0)]
	public void Realize(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (command.Count > 0)
		{
			text = command[0].Trim();
		}
		if (text == "dirk" && Singleton<InkController>.Instance.GetVariable("harper_dirk") == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
		{
			text = "clarence";
		}
		else if (text == "clarence" && (Singleton<InkController>.Instance.GetVariable("harper_dirk") != "healthy" || Singleton<InkController>.Instance.GetVariable("clarence_transform") == "dirk"))
		{
			text = "dirk";
		}
		string text2 = Path.Combine("VoiceOver", "realize", text);
		if (text == "celia_florence")
		{
			Singleton<GameController>.Instance.characterEndings.Add("celiaflorence");
			text2 = Path.Combine("VoiceOver", "realize", "florence");
		}
		else if (text == "abel_dasha")
		{
			Singleton<GameController>.Instance.characterEndings.Add("abeldasha");
			text2 = Path.Combine("VoiceOver", "realize", "abel");
		}
		else if (text == "artt_doug")
		{
			Singleton<GameController>.Instance.characterEndings.Add("arttdoug");
			text2 = Path.Combine("VoiceOver", "realize", "artt");
		}
		else if (text == "eddie_volt")
		{
			Singleton<GameController>.Instance.characterEndings.Add("eddievolt");
			text2 = Path.Combine("VoiceOver", "realize", "eddie");
		}
		else if (text == "dirk_harper")
		{
			Singleton<GameController>.Instance.characterEndings.Add("harperdirk");
			text2 = Path.Combine("VoiceOver", "realize", "dirk");
		}
		else if (text == "tina_tony")
		{
			Singleton<GameController>.Instance.characterEndings.Add("tinatony");
			text2 = Path.Combine("VoiceOver", "realize", "tina");
		}
		else if (text == "washford_drysdale")
		{
			Singleton<GameController>.Instance.characterEndings.Add("drysdalewashford");
			text2 = Path.Combine("VoiceOver", "realize", "drysdale");
		}
		else if (text == "skips")
		{
			Singleton<GameController>.Instance.characterEndings.Add("shadow");
			text2 = Path.Combine("VoiceOver", "realize", "shadow");
		}
		else if (text == "timmy")
		{
			Singleton<GameController>.Instance.characterEndings.Add("timmy");
			text2 = Path.Combine("VoiceOver", "realize", "timmy");
		}
		else if (text == "jonwick")
		{
			Singleton<GameController>.Instance.characterEndings.Add("jonwick");
			text2 = Path.Combine("VoiceOver", "realize", "jonwick");
		}
		else if (text == "clarence")
		{
			Singleton<GameController>.Instance.characterEndings.Add("dirk");
			text2 = Path.Combine("VoiceOver", "realize", "dirk");
		}
		else if (text == "lucinda")
		{
			string variable = Singleton<InkController>.Instance.GetVariable("lucinda_final");
			if (variable == "lust")
			{
				text2 = Path.Combine("VoiceOver", "realize", "lucinda_lust");
				Singleton<GameController>.Instance.characterEndings.Add("lucinda_lust");
			}
			else if (variable == "loving")
			{
				text2 = Path.Combine("VoiceOver", "realize", "lucinda_loving");
				Singleton<GameController>.Instance.characterEndings.Add("lucinda_loving");
			}
			else if (variable == "limitless")
			{
				text2 = Path.Combine("VoiceOver", "realize", "lucinda_limitless");
				Singleton<GameController>.Instance.characterEndings.Add("lucinda_limitless");
			}
			else if (variable == "lucid")
			{
				text2 = Path.Combine("VoiceOver", "realize", "lucinda_lucid");
				Singleton<GameController>.Instance.characterEndings.Add("lucinda_lucid");
			}
			else
			{
				text2 = Path.Combine("VoiceOver", "realize", "lucinda");
				Singleton<GameController>.Instance.characterEndings.Add("lucinda_lust");
			}
		}
		else if (text == "front" || text == "trap" || text == "tiny" || text == "back")
		{
			Singleton<GameController>.Instance.characterEndings.Add("dorian");
			text2 = Path.Combine("VoiceOver", "realize", "dorian");
		}
		else
		{
			Singleton<GameController>.Instance.characterEndings.Add(text);
		}
		DateADex.Instance.UnlockEnding(text, RelationshipStatus.Realized, false, false);
		Singleton<InkController>.Instance.story.variablesState["realized"] = Singleton<Save>.Instance.AvailableTotalRealizedDatables();
		Singleton<AudioManager>.Instance.PlayTrackAfterWait(text2, AUDIO_TYPE.SFX, false, false, 0.5f, true, 1f, 4f);
		RelationshipStatus relationshipStatus = Singleton<Save>.Instance.GetDateStatus(text);
		if (text == "celia_florence")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("celia");
		}
		else if (text == "skips")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("shadow");
		}
		else if (text == "clarence")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("dirk");
		}
		else if (text == "timmy")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("tim");
		}
		else if (text == "abel_dasha")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("abel");
		}
		else if (text == "artt_doug")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("artt");
		}
		else if (text == "eddie_volt")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("eddie");
		}
		else if (text == "dirk_harper")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("harper");
		}
		else if (text == "tina_tony")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("tina");
		}
		else if (text == "washford_drysdale")
		{
			relationshipStatus = Singleton<Save>.Instance.GetDateStatus("drysdale");
		}
		if (relationshipStatus != RelationshipStatus.Love && relationshipStatus != RelationshipStatus.Friend)
		{
			relationshipStatus = RelationshipStatus.Friend;
		}
		if (text == "shadow" || text == "skips")
		{
			MovingDateable.MoveDateable("MovingShadow", "gone", true);
		}
		if (text == "celia_florence")
		{
			string text3 = "MovingPolaroid";
			int num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text3, num.ToString() + "celia", true);
			string text4 = "MovingPolaroid";
			num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text4, num.ToString() + "florence", true);
			return;
		}
		if (text == "abel_dasha")
		{
			string text5 = "MovingPolaroid";
			int num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text5, num.ToString() + "abel", true);
			string text6 = "MovingPolaroid";
			num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text6, num.ToString() + "dasha", true);
			return;
		}
		if (text == "artt_doug")
		{
			string text7 = "MovingPolaroid";
			int num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text7, num.ToString() + "artt", true);
			string text8 = "MovingPolaroid";
			num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text8, num.ToString() + "doug", true);
			return;
		}
		if (text == "eddie_volt")
		{
			string text9 = "MovingPolaroid";
			int num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text9, num.ToString() + "eddie", true);
			return;
		}
		if (text == "dirk_harper")
		{
			string text10 = "MovingPolaroid";
			int num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text10, num.ToString() + "harper", true);
			if (Singleton<InkController>.Instance.GetVariable("harper_dirk") == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
			{
				string text11 = "MovingPolaroid";
				num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text11, num.ToString() + "dirk_clarence", true);
				return;
			}
			string text12 = "MovingPolaroid";
			num = (int)relationshipStatus;
			MovingDateable.MoveDateable(text12, num.ToString() + "dirk", true);
			return;
		}
		else
		{
			if (text == "tina_tony")
			{
				string text13 = "MovingPolaroid";
				int num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text13, num.ToString() + "tina", true);
				string text14 = "MovingPolaroid";
				num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text14, num.ToString() + "tony", true);
				return;
			}
			if (text == "washford_drysdale")
			{
				string text15 = "MovingPolaroid";
				int num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text15, num.ToString() + "drysdale", true);
				string text16 = "MovingPolaroid";
				num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text16, num.ToString() + "washford", true);
				return;
			}
			if (text == "skips")
			{
				string text17 = "MovingPolaroid";
				int num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text17, num.ToString() + "shadow", true);
				return;
			}
			if (text == "scandalabra")
			{
				int num;
				if (Singleton<InkController>.Instance.GetVariable("jon_dex") == "on")
				{
					string text18 = "MovingPolaroid";
					num = (int)relationshipStatus;
					MovingDateable.MoveDateable(text18, num.ToString() + "scandalabra_wick", true);
					return;
				}
				string text19 = "MovingPolaroid";
				num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text19, num.ToString() + text, true);
				return;
			}
			else if (text == "sinclaire")
			{
				int num;
				if ((bool)Singleton<InkController>.Instance.story.variablesState["martin_forever"])
				{
					string text20 = "MovingPolaroid";
					num = (int)relationshipStatus;
					MovingDateable.MoveDateable(text20, num.ToString() + "sinclaire_martin", true);
					return;
				}
				if ((bool)Singleton<InkController>.Instance.story.variablesState["sinclaire_turtle"])
				{
					string text21 = "MovingPolaroid";
					num = (int)relationshipStatus;
					MovingDateable.MoveDateable(text21, num.ToString() + "sinclaire_turtle", true);
					return;
				}
				string text22 = "MovingPolaroid";
				num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text22, num.ToString() + text, true);
				return;
			}
			else if (text == "tim" || text == "timmy")
			{
				int num;
				if ((bool)Singleton<InkController>.Instance.story.variablesState["tim_catboy_perm"])
				{
					string text23 = "MovingPolaroid";
					num = (int)relationshipStatus;
					MovingDateable.MoveDateable(text23, num.ToString() + "tim_timmy", true);
					return;
				}
				string text24 = "MovingPolaroid";
				num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text24, num.ToString() + "tim", true);
				return;
			}
			else
			{
				int num;
				if (!(text == "dirk") && !(text == "clarence"))
				{
					string text25 = "MovingPolaroid";
					num = (int)relationshipStatus;
					MovingDateable.MoveDateable(text25, num.ToString() + text, true);
					return;
				}
				if (Singleton<InkController>.Instance.GetVariable("harper_dirk") == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
				{
					string text26 = "MovingPolaroid";
					num = (int)relationshipStatus;
					MovingDateable.MoveDateable(text26, num.ToString() + "dirk_clarence", true);
					return;
				}
				string text27 = "MovingPolaroid";
				num = (int)relationshipStatus;
				MovingDateable.MoveDateable(text27, num.ToString() + text, true);
				return;
			}
		}
	}

	// Token: 0x0600029C RID: 668 RVA: 0x000105F0 File Offset: 0x0000E7F0
	[Command("UNDO_ENDING", GreaterThanOrEqual = 0)]
	public void RevertEnding(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (command.Count > 0)
		{
			text = command[0].Trim();
		}
		Singleton<Save>.Instance.SetDateStatus(text, RelationshipStatus.Single);
		DateADex.Instance.UpdateDateStatusInkVariables();
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0001063C File Offset: 0x0000E83C
	[Command("RANDOM", GreaterThanOrEqual = 0)]
	public void GenerateRandomNumbers(InkCommand command)
	{
		int num = int.Parse(command[0]);
		int num2 = int.Parse(command[1]);
		int num3 = Random.Range(num, num2 + 1);
		Singleton<InkController>.Instance.story.variablesState["randomNumber"] = num3;
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0001068C File Offset: 0x0000E88C
	[Command("SASSY_RANDOM", GreaterThanOrEqual = 0)]
	public void GenerateRandomNumbersForSassy(InkCommand command)
	{
		bool flag = (bool)Singleton<InkController>.Instance.story.variablesState["adriel_done"];
		bool flag2 = (bool)Singleton<InkController>.Instance.story.variablesState["amanda_done"];
		bool flag3 = (bool)Singleton<InkController>.Instance.story.variablesState["andrew_done"];
		bool flag4 = (bool)Singleton<InkController>.Instance.story.variablesState["ben_done"];
		bool flag5 = (bool)Singleton<InkController>.Instance.story.variablesState["cael_done"];
		bool flag6 = (bool)Singleton<InkController>.Instance.story.variablesState["debz_done"];
		bool flag7 = (bool)Singleton<InkController>.Instance.story.variablesState["devin_done"];
		bool flag8 = (bool)Singleton<InkController>.Instance.story.variablesState["dom_done"];
		bool flag9 = (bool)Singleton<InkController>.Instance.story.variablesState["elle_done"];
		bool flag10 = (bool)Singleton<InkController>.Instance.story.variablesState["evan_done"];
		bool flag11 = (bool)Singleton<InkController>.Instance.story.variablesState["garrett_done"];
		bool flag12 = (bool)Singleton<InkController>.Instance.story.variablesState["greg_done"];
		bool flag13 = (bool)Singleton<InkController>.Instance.story.variablesState["jamie_done"];
		bool flag14 = (bool)Singleton<InkController>.Instance.story.variablesState["jane_done"];
		bool flag15 = (bool)Singleton<InkController>.Instance.story.variablesState["sopheejay_done"];
		bool flag16 = (bool)Singleton<InkController>.Instance.story.variablesState["jayperior_done"];
		bool flag17 = (bool)Singleton<InkController>.Instance.story.variablesState["jonathan_done"];
		bool flag18 = (bool)Singleton<InkController>.Instance.story.variablesState["julia_done"];
		bool flag19 = (bool)Singleton<InkController>.Instance.story.variablesState["logan_done"];
		bool flag20 = (bool)Singleton<InkController>.Instance.story.variablesState["michael_done"];
		bool flag21 = (bool)Singleton<InkController>.Instance.story.variablesState["nick_done"];
		bool flag22 = (bool)Singleton<InkController>.Instance.story.variablesState["nij_done"];
		bool flag23 = (bool)Singleton<InkController>.Instance.story.variablesState["pejnt_done"];
		bool flag24 = (bool)Singleton<InkController>.Instance.story.variablesState["ray_done"];
		bool flag25 = (bool)Singleton<InkController>.Instance.story.variablesState["rebecca_done"];
		bool flag26 = (bool)Singleton<InkController>.Instance.story.variablesState["robbie_done"];
		bool flag27 = (bool)Singleton<InkController>.Instance.story.variablesState["samuel_done"];
		bool flag28 = (bool)Singleton<InkController>.Instance.story.variablesState["samantha_done"];
		bool flag29 = (bool)Singleton<InkController>.Instance.story.variablesState["souha_done"];
		bool flag30 = (bool)Singleton<InkController>.Instance.story.variablesState["tay_done"];
		bool flag31 = (bool)Singleton<InkController>.Instance.story.variablesState["thiago_done"];
		bool flag32 = (bool)Singleton<InkController>.Instance.story.variablesState["tj_done"];
		bool flag33 = (bool)Singleton<InkController>.Instance.story.variablesState["william_done"];
		List<int> list = new List<int>();
		if (!flag)
		{
			list.Add(0);
		}
		if (!flag2)
		{
			list.Add(1);
		}
		if (!flag3)
		{
			list.Add(2);
		}
		if (!flag4)
		{
			list.Add(3);
		}
		if (!flag5)
		{
			list.Add(4);
		}
		if (!flag6)
		{
			list.Add(5);
		}
		if (!flag7)
		{
			list.Add(6);
		}
		if (!flag8)
		{
			list.Add(7);
		}
		if (!flag9)
		{
			list.Add(8);
		}
		if (!flag10)
		{
			list.Add(9);
		}
		if (!flag11)
		{
			list.Add(10);
		}
		if (!flag12)
		{
			list.Add(11);
		}
		if (!flag13)
		{
			list.Add(12);
		}
		if (!flag14)
		{
			list.Add(13);
		}
		if (!flag15)
		{
			list.Add(14);
		}
		if (!flag16)
		{
			list.Add(15);
		}
		if (!flag17)
		{
			list.Add(16);
		}
		if (!flag18)
		{
			list.Add(17);
		}
		if (!flag19)
		{
			list.Add(18);
		}
		if (!flag20)
		{
			list.Add(19);
		}
		if (!flag21)
		{
			list.Add(20);
		}
		if (!flag22)
		{
			list.Add(21);
		}
		if (!flag23)
		{
			list.Add(22);
		}
		if (!flag24)
		{
			list.Add(23);
		}
		if (!flag25)
		{
			list.Add(24);
		}
		if (!flag26)
		{
			list.Add(25);
		}
		if (!flag27)
		{
			list.Add(26);
		}
		if (!flag28)
		{
			list.Add(27);
		}
		if (!flag29)
		{
			list.Add(28);
		}
		if (!flag30)
		{
			list.Add(29);
		}
		if (!flag31)
		{
			list.Add(30);
		}
		if (!flag32)
		{
			list.Add(31);
		}
		if (!flag33)
		{
			list.Add(32);
		}
		if (list.Count > 0)
		{
			int num = 0;
			int count = list.Count;
			int num2 = Random.Range(num, count);
			Singleton<InkController>.Instance.story.variablesState["randomNumber"] = list[num2];
			return;
		}
		Singleton<InkController>.Instance.story.variablesState["randomNumber"] = -1;
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00010CBC File Offset: 0x0000EEBC
	[Command("SKYLAR_RANDOM", GreaterThanOrEqual = 0)]
	public void GenerateRandomNumbersForSkylar(InkCommand command)
	{
		bool flag = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_airyn_clue"];
		bool flag2 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_ben_clue"];
		bool flag3 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_bobby_clue"];
		bool flag4 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_daemon_clue"];
		bool flag5 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_dolly_clue"];
		bool flag6 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_doug_clue"];
		bool flag7 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_farya_clue"];
		bool flag8 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_jerry_clue"];
		bool flag9 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_nightmare_clue"];
		bool flag10 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_reggie_clue"];
		bool flag11 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_river_clue"];
		bool flag12 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_sassy_clue"];
		bool flag13 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_shadow_clue"];
		bool flag14 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_textbox_clue"];
		bool flag15 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_trapdoor_clue"];
		bool flag16 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_willi_clue"];
		bool flag17 = (bool)Singleton<InkController>.Instance.story.variablesState["skylar_zoey_clue"];
		bool flag18 = (bool)Singleton<InkController>.Instance.story.variablesState["airyn_awakened"];
		bool flag19 = (bool)Singleton<InkController>.Instance.story.variablesState["benhwa_awakened"];
		bool flag20 = (bool)Singleton<InkController>.Instance.story.variablesState["bobby_awakened"];
		bool flag21 = (bool)Singleton<InkController>.Instance.story.variablesState["daemon_awakened"];
		bool flag22 = (bool)Singleton<InkController>.Instance.story.variablesState["dolly_awakened"];
		bool flag23 = (bool)Singleton<InkController>.Instance.story.variablesState["doug_awakened"];
		bool flag24 = (bool)Singleton<InkController>.Instance.story.variablesState["farya_awakened"];
		bool flag25 = (bool)Singleton<InkController>.Instance.story.variablesState["jerry_awakened"];
		bool flag26 = (bool)Singleton<InkController>.Instance.story.variablesState["nightmare_awakened"];
		bool flag27 = (bool)Singleton<InkController>.Instance.story.variablesState["reggie_awakened"];
		bool flag28 = (bool)Singleton<InkController>.Instance.story.variablesState["river_awakened"];
		bool flag29 = (bool)Singleton<InkController>.Instance.story.variablesState["sassy_awakened"];
		bool flag30 = (bool)Singleton<InkController>.Instance.story.variablesState["shadow_awakened"];
		bool flag31 = (bool)Singleton<InkController>.Instance.story.variablesState["tbc_awakened"];
		bool flag32 = (bool)Singleton<InkController>.Instance.story.variablesState["trap_door_opened"];
		bool flag33 = (bool)Singleton<InkController>.Instance.story.variablesState["willi_awakened"];
		bool flag34 = (bool)Singleton<InkController>.Instance.story.variablesState["zoey_awakened"];
		List<int> list = new List<int>();
		if (!flag && !flag18)
		{
			list.Add(0);
		}
		if (!flag2 && !flag19)
		{
			list.Add(1);
		}
		if (!flag3 && !flag20)
		{
			list.Add(2);
		}
		if (!flag4 && !flag21)
		{
			list.Add(3);
		}
		if (!flag5 && !flag22)
		{
			list.Add(4);
		}
		if (!flag6 && !flag23)
		{
			list.Add(5);
		}
		if (!flag7 && !flag24)
		{
			list.Add(6);
		}
		if (!flag8 && !flag25)
		{
			list.Add(7);
		}
		if (!flag9 && !flag26)
		{
			list.Add(8);
		}
		if (!flag10 && !flag27)
		{
			list.Add(9);
		}
		if (!flag11 && !flag28)
		{
			list.Add(10);
		}
		if (!flag12 && !flag29)
		{
			list.Add(11);
		}
		if (!flag13 && !flag30)
		{
			list.Add(12);
		}
		if (!flag14 && !flag31)
		{
			list.Add(13);
		}
		if (!flag15 && !flag32)
		{
			list.Add(14);
		}
		if (!flag16 && !flag33)
		{
			list.Add(15);
		}
		if (!flag17 && !flag34)
		{
			list.Add(16);
		}
		if (list.Count > 0)
		{
			int num = 0;
			int count = list.Count;
			int num2 = Random.Range(num, count);
			Singleton<InkController>.Instance.story.variablesState["randomNumber"] = list[num2];
			return;
		}
		Singleton<InkController>.Instance.story.variablesState["randomNumber"] = -1;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00011280 File Offset: 0x0000F480
	[Command("DEX", GreaterThanOrEqual = 1)]
	public void UnlockDexEntry(InkCommand command)
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		int num;
		if (command.Count > 1)
		{
			text = command[0].Trim();
			num = int.Parse(command[1].Trim());
		}
		else
		{
			num = int.Parse(command[0].Trim());
		}
		DateADex.Instance.UnlockDexEntry(text, num, false);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_date_a_dex_added, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00011318 File Offset: 0x0000F518
	[Command("REGGIE_STINGER", LessThanOrEqual = 1)]
	public void PlayReggieStinger(InkCommand command)
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.stinger_reggie, AUDIO_TYPE.SFX, true, false, 0f, false, 0.3f, null, false, SFX_SUBGROUP.STINGER, false);
		if (command.Count < 2)
		{
			Singleton<AudioManager>.Instance.ReggieMusic();
		}
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0001135E File Offset: 0x0000F55E
	[Command("NGP_UNLOCK")]
	public void UnlockNewGamePlus(InkCommand command)
	{
		Singleton<Save>.Instance.SetNewGamePlus(true);
		Save.AutoSaveGame();
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00011370 File Offset: 0x0000F570
	[Command("OPEN_LOCKET")]
	public void OpenLocket(InkCommand command)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < Singleton<Save>.Instance.AvailableTotalDatables(); i++)
		{
			RelationshipStatus dateStatus = Singleton<Save>.Instance.GetDateStatus(i);
			if (dateStatus == RelationshipStatus.Love || dateStatus == RelationshipStatus.Friend)
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			int num = Random.Range(0, list.Count);
			DateADexEntry entry = DateADex.Instance.GetEntry(num);
			int num2 = 0;
			if (Singleton<InkController>.Instance.story.variablesState["daemon_completed_playthroughs"] != null)
			{
				num2 = (int)Singleton<InkController>.Instance.story.variablesState["daemon_completed_playthroughs"];
			}
			Singleton<InkController>.Instance.story.variablesState["daemon_completed_playthroughs"] = num2 + 1;
			DateADex.Instance.OpenLocket(entry.internalName);
			return;
		}
		DateADex.Instance.OpenLocketWithoutAnyEntries();
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00011458 File Offset: 0x0000F658
	[Command("SPECS_PHONE", Exactly = 2)]
	public void IncreaseSpecWithPhone(InkCommand command)
	{
		SpecStatMain.ParseSpecsAttributes(command[0].Trim());
		int num = 0;
		int.TryParse(command[1].Trim(), out num);
		Singleton<PhoneManager>.Instance.OpenPhoneAppAsync(Object.FindObjectOfType<SpecStatMain>().transform.GetChild(0).gameObject);
		Singleton<SpecStatMain>.Instance.GiveStatPoint(command[0].Trim(), num);
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x000114C4 File Offset: 0x0000F6C4
	[Command("SPECS", Exactly = 2)]
	public void IncreaseSpec(InkCommand command)
	{
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.SFX, 0f);
		SpecStatMain.ParseSpecsAttributes(command[0].Trim());
		int num = 0;
		int.TryParse(command[1].Trim(), out num);
		Singleton<PhoneManager>.Instance.OpenPhoneAppAsyncForced(Object.FindObjectOfType<SpecStatMain>().transform.GetChild(0).gameObject);
		Singleton<SpecStatMain>.Instance.GiveStatPoint(command[0].Trim(), num);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00011540 File Offset: 0x0000F740
	[Command("SPECS_SET", Exactly = 2)]
	public void IncreaseSpecFinalValue(InkCommand command)
	{
		SpecStatMain.ParseSpecsAttributes(command[0].Trim());
		int num = 0;
		if (command[1].Contains("backup"))
		{
			num = (int)Singleton<InkController>.Instance.story.variablesState[command[1].Trim()];
		}
		else
		{
			int.TryParse(command[1].Trim(), out num);
		}
		Singleton<PhoneManager>.Instance.OpenPhoneAppAsyncForced(Object.FindObjectOfType<SpecStatMain>().transform.GetChild(0).gameObject);
		Singleton<SpecStatMain>.Instance.GiveStatPointFinalValue(command[0].Trim(), num);
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x000115E8 File Offset: 0x0000F7E8
	[Command("EMOTE", GreaterThanOrEqual = 2)]
	public void ShowEmote(InkCommand command)
	{
		string text = "";
		if (!this.TryGetInternalName(command[0], out text))
		{
			return;
		}
		EmoteHeight emoteHeight = EmoteHeight.bottom;
		string text2;
		if (command.Count > 2)
		{
			text2 = command[2];
			if (!(text2 == "bottom"))
			{
				if (!(text2 == "center"))
				{
					if (text2 == "top")
					{
						emoteHeight = EmoteHeight.top;
					}
				}
				else
				{
					emoteHeight = EmoteHeight.center;
				}
			}
			else
			{
				emoteHeight = EmoteHeight.bottom;
			}
		}
		text2 = command[1];
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
		if (num <= 2935168099U)
		{
			if (num <= 1833711264U)
			{
				if (num <= 1622628984U)
				{
					if (num != 366108702U)
					{
						if (num != 1622628984U)
						{
							return;
						}
						if (!(text2 == "bad"))
						{
							return;
						}
						Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.bad, emoteHeight);
						Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_bad, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
						return;
					}
					else
					{
						if (!(text2 == "sing"))
						{
							return;
						}
						Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.sing, emoteHeight);
						Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_sing, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
						return;
					}
				}
				else if (num != 1703759035U)
				{
					if (num != 1833711264U)
					{
						return;
					}
					if (!(text2 == "love1"))
					{
						return;
					}
					Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.love1, emoteHeight);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_love1, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
					return;
				}
				else
				{
					if (!(text2 == "sweat"))
					{
						return;
					}
					Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.sweat, emoteHeight);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_sweat, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
					return;
				}
			}
			else if (num <= 1884044121U)
			{
				if (num != 1867266502U)
				{
					if (num != 1884044121U)
					{
						return;
					}
					if (!(text2 == "love2"))
					{
						return;
					}
					Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.love2, emoteHeight);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_love1, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
					return;
				}
				else
				{
					if (!(text2 == "love3"))
					{
						return;
					}
					Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.love2, emoteHeight);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_love2, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
					return;
				}
			}
			else if (num != 2313861896U)
			{
				if (num != 2935168099U)
				{
					return;
				}
				if (!(text2 == "resolve"))
				{
					return;
				}
				Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.resolve, emoteHeight);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_resolve, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
				return;
			}
			else
			{
				if (!(text2 == "sleep"))
				{
					return;
				}
				Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.sleep, emoteHeight);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_sleep, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
				return;
			}
		}
		else if (num <= 3478679200U)
		{
			if (num <= 3180049141U)
			{
				if (num != 3037376254U)
				{
					if (num != 3180049141U)
					{
						return;
					}
					if (!(text2 == "shake"))
					{
						return;
					}
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_shake, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
					Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.shake, emoteHeight);
					return;
				}
				else
				{
					if (!(text2 == "angry"))
					{
						return;
					}
					Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.angry, emoteHeight);
					Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_angry, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
					return;
				}
			}
			else if (num != 3411225317U)
			{
				if (num != 3478679200U)
				{
					return;
				}
				if (!(text2 == "violent"))
				{
					return;
				}
				Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.violent, emoteHeight);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_violent, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
				return;
			}
			else
			{
				if (!(text2 == "stop"))
				{
					return;
				}
				Singleton<GameController>.Instance.HideEmotes(text);
				return;
			}
		}
		else if (num <= 3637782525U)
		{
			if (num != 3576140497U)
			{
				if (num != 3637782525U)
				{
					return;
				}
				if (!(text2 == "joy"))
				{
					return;
				}
				Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.joy, emoteHeight);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_joy, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
				return;
			}
			else
			{
				if (!(text2 == "love"))
				{
					return;
				}
				Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.love, emoteHeight);
				Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_love1, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
				return;
			}
		}
		else if (num != 4008950432U)
		{
			if (num != 4200608216U)
			{
				return;
			}
			if (!(text2 == "good"))
			{
				return;
			}
			Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.good, emoteHeight);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_good, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
			return;
		}
		else
		{
			if (!(text2 == "disgust"))
			{
				return;
			}
			Singleton<GameController>.Instance.ShowEmote(text, EmoteReaction.disgust, emoteHeight);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_disgust, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.EMOTE, false);
			return;
		}
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00011B72 File Offset: 0x0000FD72
	[Command("WORKSPACE", GreaterThanOrEqual = 1)]
	public void AddChatToWorkspace(InkCommand command)
	{
		Singleton<ChatMaster>.Instance.StartChat("wrkspace_chat." + command[0], ChatType.Wrkspce, false, true);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00011B94 File Offset: 0x0000FD94
	[Command("THISCORD", GreaterThanOrEqual = 1)]
	public void AddChatToThiscord(InkCommand command)
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_notification, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		Singleton<ChatMaster>.Instance.StartChat("thiscord_phone." + command[0], ChatType.Thiscord, false, true);
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00011BE8 File Offset: 0x0000FDE8
	[Command("NEW_MESSAGE_COMPUTER", GreaterThanOrEqual = 0)]
	public void ShowNotificationComputer(InkCommand command)
	{
		Singleton<PhoneManager>.Instance.SetNewMessageAlert(true);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_wkspace_messagereceived, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00011C28 File Offset: 0x0000FE28
	[Command("NEW_MESSAGE_PHONE", GreaterThanOrEqual = 0)]
	public void ShowNotificationPhone(InkCommand command)
	{
		Singleton<PhoneManager>.Instance.SetNewMessageAlert(false);
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_thiscord_notification, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00011C66 File Offset: 0x0000FE66
	[Command("NEWMESSAGE", Exactly = 1)]
	[Command("NEW_MESSAGE", Exactly = 1)]
	public void ShowNotification(InkCommand command)
	{
		if (command[0].ToUpperInvariant() == "COMPUTER")
		{
			this.ShowNotificationComputer(command);
			return;
		}
		this.ShowNotificationPhone(command);
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00011C90 File Offset: 0x0000FE90
	[Command("SETDAYPHASE", GreaterThanOrEqual = 1)]
	[Command("SET_DAY_PHASE", GreaterThanOrEqual = 1)]
	public void SetDayPhase(InkCommand command)
	{
		string text = command[0].ToLowerInvariant();
		if (text == "morning")
		{
			Singleton<DayNightCycle>.Instance.SetDateTime(Singleton<DayNightCycle>.Instance.PresentDayPresentTime, DayPhase.MORNING);
			return;
		}
		if (text == "noon")
		{
			Singleton<DayNightCycle>.Instance.SetDateTime(Singleton<DayNightCycle>.Instance.PresentDayPresentTime, DayPhase.NOON);
			return;
		}
		if (text == "afternoon")
		{
			Singleton<DayNightCycle>.Instance.SetDateTime(Singleton<DayNightCycle>.Instance.PresentDayPresentTime, DayPhase.AFTERNOON);
			return;
		}
		if (text == "evening")
		{
			Singleton<DayNightCycle>.Instance.SetDateTime(Singleton<DayNightCycle>.Instance.PresentDayPresentTime, DayPhase.EVENING);
			return;
		}
		if (text == "night")
		{
			Singleton<DayNightCycle>.Instance.SetDateTime(Singleton<DayNightCycle>.Instance.PresentDayPresentTime, DayPhase.NIGHT);
			return;
		}
		if (!(text == "midnight"))
		{
			return;
		}
		Singleton<DayNightCycle>.Instance.SetDateTime(Singleton<DayNightCycle>.Instance.PresentDayPresentTime, DayPhase.MIDNIGHT);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00011D7C File Offset: 0x0000FF7C
	[Command("KEITH_PICKUP", Exactly = 0)]
	public void KeithPickup(InkCommand command)
	{
		GameObject gameObject = GameObject.FindWithTag("Keith");
		if (gameObject != null)
		{
			GenericInteractable component = gameObject.GetComponent<GenericInteractable>();
			component.PlayMagicalSound();
			if (component.animator != null)
			{
				component.animator.enabled = true;
				component.animator.SetInteger("ending", 2);
			}
			component.MagicalInteract(true);
		}
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00011DDC File Offset: 0x0000FFDC
	[Command("KEITH_UNLOCK", Exactly = 0)]
	public void KeithUnlock(InkCommand command)
	{
		AtticDoorUnlocker atticDoorUnlocker = Object.FindObjectOfType<AtticDoorUnlocker>(true);
		if (atticDoorUnlocker != null)
		{
			atticDoorUnlocker.gameObject.SetActive(true);
			MovingDateable.MoveDateable("MovingPlants", "attic", true);
			Singleton<InkController>.Instance.story.variablesState["attic_open"] = true;
			atticDoorUnlocker.StartSequence();
		}
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00011E3A File Offset: 0x0001003A
	[Command("KEITH_UNLOCK_POSTPONE", Exactly = 0)]
	public void KeithUnlockPostponed(InkCommand command)
	{
		Singleton<GameController>.Instance.shouldTriggerKeithOpeningDoor = true;
		Singleton<InkController>.Instance.story.variablesState["attic_open"] = true;
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00011E66 File Offset: 0x00010066
	[Command("POLAROID", GreaterThanOrEqual = 0)]
	public void SetPolaroid(InkCommand command)
	{
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00011E68 File Offset: 0x00010068
	[Command("FTB", Exactly = 0)]
	public void FadeToBlack(InkCommand command)
	{
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00011E6A File Offset: 0x0001006A
	[Command("FTW", Exactly = 0)]
	public void FadeToWhite(InkCommand command)
	{
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00011E6C File Offset: 0x0001006C
	[Command("GAME_OVER", Exactly = 0)]
	public void GameOver(InkCommand command)
	{
		Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.GAME_OVER);
		Singleton<DayNightCycle>.Instance.GameOver();
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00011E88 File Offset: 0x00010088
	[Command("ROOMER", GreaterThanOrEqual = 1)]
	public void AddRoomer(InkCommand command)
	{
		if (command.Count < 2)
		{
			T17Debug.LogError("[DIALOGUETAGMANAGER] Not enough commands to add a Roomer");
			return;
		}
		string text = "";
		Singleton<Save>.Instance.TryGetInternalName(command[0], out text);
		if (Singleton<Save>.Instance.GetDateStatus("maggie_mglass") != RelationshipStatus.Unmet && (Singleton<Save>.Instance.GetDateStatus(text) == RelationshipStatus.Unmet || text == "skylar"))
		{
			Singleton<GameController>.Instance.talkingUI.AddRoomer();
			Singleton<AudioManager>.Instance.StopTrack(SFXBank.Instance.ui_roomer_added.name, 0f);
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_roomer_added, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		}
		Singleton<Save>.Instance.AddRoomerTip(command[0], command[1]);
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00011F59 File Offset: 0x00010159
	[Command("LOCATION", Exactly = 1)]
	public void ChangeLocation(InkCommand command)
	{
		Singleton<GameController>.Instance.talkingUI.ChangeCameraByRoom(command[0]);
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00011F74 File Offset: 0x00010174
	[Command("LOCATIONS", GreaterThanOrEqual = 1)]
	public void ChangeLocationNearest(InkCommand command)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < command.Count; i++)
		{
			list.Add(command[i]);
		}
		Singleton<GameController>.Instance.talkingUI.ChangeCameraByListedNearest(list);
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00011FB5 File Offset: 0x000101B5
	[Command("COLLECTABLE_LARGE_HOLD", Exactly = 2)]
	[Command("BACKGROUND_HOLD", Exactly = 2)]
	public void ShowLargeCollectable(InkCommand command)
	{
		DateADex.Instance.ShowLargeCollectable(command[0]);
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00011FC8 File Offset: 0x000101C8
	[Command("COLLECTABLE_LARGE_RELEASE", Exactly = 0)]
	[Command("BACKGROUND_RELEASE", Exactly = 0)]
	public void HideLargeCollectable(InkCommand command)
	{
		DateADex.Instance.HideLargeCollectable();
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00011FD4 File Offset: 0x000101D4
	[Command("ANIME", Exactly = 0)]
	public void SetAnimeModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingAnimeFigure", "visible", true);
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00011FE6 File Offset: 0x000101E6
	[Command("UNSKIPPABLE", Exactly = 0)]
	public void SetUnsippable(InkCommand command)
	{
		if (Singleton<Save>.Instance.GetVoiceOverVolume() > 0f)
		{
			Singleton<GameController>.Instance.isUnskippable = true;
		}
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00012004 File Offset: 0x00010204
	[Command("SUBTITLE", Exactly = 0)]
	public void SetSubtitle(InkCommand command)
	{
		if (Singleton<Save>.Instance.GetVoiceOverVolume() > 0f)
		{
			Singleton<GameController>.Instance.isUnskippable = true;
			Singleton<GameController>.Instance.isSubtitle = true;
			Singleton<GameController>.Instance.StartSongSubtitle();
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00012037 File Offset: 0x00010237
	[Command("TEDDY", Exactly = 1)]
	public void SetTeddyLocation(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingTeddyBear", command[0], true);
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0001204B File Offset: 0x0001024B
	[Command("CLOTHES", Exactly = 0)]
	public void SetClothesLocation(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingClothes", "visible", true);
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00012060 File Offset: 0x00010260
	[Command("BOBBY", Exactly = 1)]
	public void SetBobbyLocation(InkCommand command)
	{
		if (!(command[0] == "attic"))
		{
			MovingDateable.MoveDateable("MovingBobbyPin", command[0], true);
			return;
		}
		MovingDateable movingDateable = MovingDateable.GetMovingDateable("MovingSafe");
		bool flag = false;
		if (movingDateable != null)
		{
			foreach (MovingObject movingObject in movingDateable.Locations)
			{
				if (movingObject.Object.activeInHierarchy && movingObject.Key == "open")
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			MovingDateable.MoveDateable("MovingBobbyPin", "atticOpened", true);
			return;
		}
		MovingDateable.MoveDateable("MovingBobbyPin", "atticClosed", true);
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00012130 File Offset: 0x00010330
	[Command("BODHI", Exactly = 1)]
	public void SetBodhiLocation(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingTimeCapsule", command[0], true);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00012144 File Offset: 0x00010344
	[Command("VAUGHN", Exactly = 1)]
	public void SetVaughnLocation(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingRatTrap", command[0], true);
		GameObject gameObject = GameObject.FindWithTag("MovingRatTrap");
		if (gameObject != null)
		{
			gameObject.GetComponent<Rat>().MoveDateable(Singleton<InkController>.Instance.GetVariable("vaughn_rats"));
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00012194 File Offset: 0x00010394
	[Command("MATEO_QUEST", Exactly = 1)]
	public void SetMateoLocation(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingInanimals", command[0], true);
		if (Singleton<InkController>.Instance.story.variablesState["mateo_davi"].ToString() == "davi_found")
		{
			Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.INANIMAL_CONTROL);
		}
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x000121EA File Offset: 0x000103EA
	[Command("SHOW_LUCINDA_RUG", Exactly = 0)]
	public void SetLucindaModel(InkCommand command)
	{
		if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			MovingDateable.MoveDateable("MovingLucindaRug", "active", true);
		}
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00012208 File Offset: 0x00010408
	[Command("HIDE_LUCINDA_RUG", Exactly = 0)]
	public void HideLucindaModel(InkCommand command)
	{
		if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			MovingDateable.MoveDateable("MovingLucindaRug", "default", true);
		}
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00012226 File Offset: 0x00010426
	[Command("JERRY", Exactly = 1)]
	public void SetJerryModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingOfficeDrawer", command[0], true);
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0001223A File Offset: 0x0001043A
	[Command("ALARM", Exactly = 1)]
	public void SetSmokeAlarms(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingSmokeAlarms", command[0], true);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0001224E File Offset: 0x0001044E
	[Command("BURNED_WALL", Exactly = 0)]
	public void SetBurnedWall(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingBurnedWall", "burnt", true);
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00012260 File Offset: 0x00010460
	[Command("PLANTSPAWN", GreaterThanOrEqual = 0)]
	[Command("PLANT", GreaterThanOrEqual = 0)]
	public void SetPlantModel(InkCommand command)
	{
		MovingDateable.MoveDateableEnableAllKeys("MovingPlants");
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0001226C File Offset: 0x0001046C
	[Command("SCANDALABRA", GreaterThanOrEqual = 1)]
	public void SetScandalabraModel(InkCommand command)
	{
		if (command.Count == 1)
		{
			Candelabra.Instance.SetLevel(int.Parse(command[0]));
		}
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0001228D File Offset: 0x0001048D
	[Command("REBEL", Exactly = 1)]
	public void SetRubberDuckModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingRubberDuck", command[0], true);
	}

	// Token: 0x060002CB RID: 715 RVA: 0x000122A1 File Offset: 0x000104A1
	[Command("ART_ADDED", Exactly = 1)]
	public void SetArtPaitingModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingArt", command[0], true);
	}

	// Token: 0x060002CC RID: 716 RVA: 0x000122B5 File Offset: 0x000104B5
	[Command("ART_SCULPTURE", Exactly = 1)]
	public void SetArtSculptureModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingArtSculpture", command[0], true);
	}

	// Token: 0x060002CD RID: 717 RVA: 0x000122C9 File Offset: 0x000104C9
	[Command("JERRY_ART", Exactly = 0)]
	public void SetJerryArtModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingCarousel", "visible", true);
	}

	// Token: 0x060002CE RID: 718 RVA: 0x000122DB File Offset: 0x000104DB
	[Command("CAKE", Exactly = 0)]
	public void SetCakeModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingCake", "cake", true);
	}

	// Token: 0x060002CF RID: 719 RVA: 0x000122ED File Offset: 0x000104ED
	[Command("RONGO", Exactly = 0)]
	[Command("GEODES", Exactly = 0)]
	public void SetGeodeModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingGeode", "visible", true);
		Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.ADVENTURE_TIME);
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x0001230C File Offset: 0x0001050C
	[Command("CANDY_USED", Exactly = 0)]
	public void SetCandyUsed(InkCommand command)
	{
		Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.SWEET_TOOTH);
		DateADex.Instance.UpdateDateStatusInkVariables();
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00012325 File Offset: 0x00010525
	[Command("MEMORY", Exactly = 1)]
	public void SetKeepSakesModel(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingFrames", command[0], true);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00012339 File Offset: 0x00010539
	[Command("DRESSER", Exactly = 1)]
	public void SetDresserBehavior(InkCommand command)
	{
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0001233B File Offset: 0x0001053B
	[Command("IRON", Exactly = 0)]
	public void SetIronBoardDoorEnabled(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingIronBoard", "open", true);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00012350 File Offset: 0x00010550
	[Command("YACHT", Exactly = 0)]
	public void SetYachtEnabled(InkCommand command)
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.yachtDelivery, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
		MovingDateable.MoveDateable("MovingYacht", "visible", true);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00012393 File Offset: 0x00010593
	[Command("CLOSE_FRONT_DOOR", Exactly = 0)]
	public void CloseFrontDoor(InkCommand command)
	{
		DayNightCycle.PlayDayMusic(0f);
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0.5f);
		Singleton<GameController>.Instance.SetDoNotPostProcessOnReturn();
		FrontDoor.Instance.inNarratorConversation = false;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x000123C4 File Offset: 0x000105C4
	[Command("OPEN_FRONT_DOOR", Exactly = 0)]
	public void OpenFrontDoor(InkCommand command)
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			Singleton<Popup>.Instance.CreatePopup("Date Everything", "Behind this door lies content locked away for this silly little demo. Go date things!", true);
		}
		else
		{
			if (!Singleton<TutorialController>.Instance.CanGoToBed())
			{
				Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.SPEED_RUN);
			}
			foreach (AmbientSpace ambientSpace in Object.FindObjectsOfType<AmbientSpace>().ToList<AmbientSpace>())
			{
				ambientSpace.collider.enabled = false;
				ambientSpace.ResetTimed(15f);
			}
			Singleton<TutorialController>.Instance.car1GameObject.SetActive(false);
			Singleton<TutorialController>.Instance.car2GameObject.SetActive(false);
			BetterPlayerControl.Instance.isGameEndingOn = true;
			GameObject gameObject = GameObject.FindWithTag("FrontDoor");
			if (bool.Parse(Singleton<InkController>.Instance.GetVariable("hate_everything")) || Singleton<InkController>.Instance.GetVariable("david_franklin") == "love" || int.Parse(Singleton<InkController>.Instance.GetVariable("realized")) > 0)
			{
				this.MoveHelicopterFly(command);
			}
			gameObject.GetComponent<FrontDoor>().ConfigureOpenDoor();
		}
		FrontDoor.Instance.inNarratorConversation = false;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x000124FC File Offset: 0x000106FC
	[Command("SAFE_OPEN", Exactly = 0)]
	public void SetSafeDoorEnabled(InkCommand command)
	{
		MovingDateable movingDateable = MovingDateable.GetMovingDateable("MovingBobbyPin");
		bool flag = false;
		if (movingDateable != null)
		{
			foreach (MovingObject movingObject in movingDateable.Locations)
			{
				if (movingObject.Object.activeInHierarchy && movingObject.Key == "atticClosed")
				{
					flag = true;
				}
			}
		}
		MovingDateable.MoveDateable("MovingSafe", "open", true);
		if (flag)
		{
			MovingDateable.MoveDateable("MovingBobbyPin", "atticOpened", true);
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000125A4 File Offset: 0x000107A4
	[Command("GHOST_SEANCE", Exactly = 0)]
	public void SetGhostEnabled(InkCommand command)
	{
		GameObject gameObject = GameObject.FindWithTag("MovingGhost");
		if (gameObject != null)
		{
			gameObject.GetComponent<GhostController>().SetShownAtAllTimes(true);
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x000125D1 File Offset: 0x000107D1
	[Command("RECIPES_UNLOCKED", Exactly = 0)]
	public void SetDateADexRecipesEnabled(InkCommand command)
	{
		Singleton<Save>.Instance.SetInteractableState("DateADexRecipe", true);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000125E3 File Offset: 0x000107E3
	[Command("RECIPE_CHECK", Exactly = 0)]
	public void CheckRecipe(InkCommand command)
	{
	}

	// Token: 0x060002DB RID: 731 RVA: 0x000125E8 File Offset: 0x000107E8
	[Command("RECIPE_SHOW", GreaterThanOrEqual = 0)]
	public void ShowRecipe(InkCommand command)
	{
		string text = Singleton<GameController>.Instance.GetCurrentActiveDatable().InternalName();
		if (command.Count > 1)
		{
			text = command[0].Trim();
		}
		DateADex.Instance.RecipeCheck(text);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00012627 File Offset: 0x00010827
	[Command("RECIPE_HIDE", Exactly = 0)]
	public void HideRecipe(InkCommand command)
	{
		Singleton<RecipeAnim>.Instance.CollectionAnim.SetTrigger("RecipeExit");
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00012640 File Offset: 0x00010840
	[Command("RATS", Exactly = 1)]
	public void SetRatTrapModel(InkCommand command)
	{
		GameObject gameObject = GameObject.FindWithTag("MovingRatTrap");
		if (gameObject != null)
		{
			gameObject.GetComponent<Rat>().MoveDateable(command[0]);
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00012673 File Offset: 0x00010873
	[Command("ATTIC_BOXES", Exactly = 1)]
	public void SetAtticBoxesModelOnOff(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingBoxes", command[0], true);
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00012687 File Offset: 0x00010887
	[Command("BOXES", Exactly = 1)]
	public void SetAtticBoxesModelOnOffKeepsakes(InkCommand command)
	{
		Keepsakes.Instance.UpdateExamines();
		MovingDateable.MoveDateable("MovingKeepsakesAttic", command[0], true);
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x000126A5 File Offset: 0x000108A5
	[Command("BEDTIME_ANYTIME", Exactly = 0)]
	public void SetGoToBedAnytime(InkCommand command)
	{
		Singleton<Save>.Instance.SetTutorialThresholdState(Bed.BED_CAN_SLEEP_ANYTIME);
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x000126B6 File Offset: 0x000108B6
	[Command("TOGGLE_FPS", Exactly = 0)]
	public void SetToggleFps(InkCommand command)
	{
		Singleton<Save>.Instance.GetTutorialThresholdState(FPSCounter.FPS_COUNTER_INVISIBLE);
		Singleton<Save>.Instance.SetTutorialThresholdState(FPSCounter.FPS_COUNTER_INVISIBLE);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000126D7 File Offset: 0x000108D7
	[Command("REGGIE_APP_UNINSTALL", Exactly = 0)]
	public void SetUninstallReggieApp(InkCommand command)
	{
		Singleton<Save>.Instance.SetTutorialThresholdState(PhoneManager.REGGIE_APP_UNINSTALLED);
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000126E8 File Offset: 0x000108E8
	[Command("CROUCH", Exactly = 0)]
	public void SetCrouch(InkCommand command)
	{
		Singleton<Save>.Instance.SetTutorialThresholdState(PhoneManager.CROUCH_CONTROL_ENABLED);
		string actionText = ControlsUI.GetActionText(ReInput.players.GetPlayer(0), 33);
		Singleton<InkController>.Instance.TrySetVariable("squat_control", actionText);
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00012727 File Offset: 0x00010927
	[Command("DRONES", Exactly = 1)]
	public void MoveDrones(InkCommand command)
	{
		MovingDateable.MoveDateable("MovingDrones", "level" + command[0], true);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00012745 File Offset: 0x00010945
	[Command("uses_helicopter", Exactly = 0)]
	public void SwapHelicopterAnimator(InkCommand command)
	{
		Object.FindObjectOfType<FrontDoor>().SwapToHelicopterAnim();
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00012751 File Offset: 0x00010951
	[Command("helicopter", Exactly = 0)]
	public void MoveHelicopter(InkCommand command)
	{
		Singleton<TutorialController>.Instance.HideCars();
		MovingDateable.MoveDateable("MovingHelicopter", "landed", true);
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0001276D File Offset: 0x0001096D
	[Command("helicopter_fly", Exactly = 0)]
	public void MoveHelicopterFly(InkCommand command)
	{
		Singleton<TutorialController>.Instance.HideCars();
		this.SwapHelicopterAnimator(command);
		MovingDateable.MoveDateable("MovingHelicopter", "flying", true);
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00012790 File Offset: 0x00010990
	[Command("leave_house", Exactly = 0)]
	public void DaemonRemoveWall(InkCommand command)
	{
		GameObject gameObject = GameObject.FindWithTag("DaemonWall");
		Singleton<Save>.Instance.ResetDateableTalkedTo("daemon");
		Dresser.Instance.DisableCollision();
		gameObject.GetComponent<DresserWall>().MakeWallInvisible();
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x000127BF File Offset: 0x000109BF
	[Command("dateviator_done", Exactly = 0)]
	public void DateviatorDone(InkCommand command)
	{
		Singleton<Dateviators>.Instance.DequipOverride();
		Singleton<Save>.Instance.SetTutorialThresholdState(TutorialController.TUTORIAL_STATE_1000_DATEVIATORS_GONE);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000127DA File Offset: 0x000109DA
	[Command("start_dishy_battle", Exactly = 0)]
	public void StartDishyBattle(InkCommand command)
	{
		Singleton<BossBattleHealthBar>.Instance.StartBattle(true, false);
	}

	// Token: 0x060002EB RID: 747 RVA: 0x000127E8 File Offset: 0x000109E8
	[Command("start_chance_battle", Exactly = 0)]
	public void StartChanceBattle(InkCommand command)
	{
		Singleton<BossBattleHealthBar>.Instance.StartBattle(false, true);
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000127F6 File Offset: 0x000109F6
	[Command("end_dishy_battle", Exactly = 0)]
	public void EndDishyBattle(InkCommand command)
	{
		Singleton<BossBattleHealthBar>.Instance.EndBattle(true, false);
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00012804 File Offset: 0x00010A04
	[Command("end_chance_battle", Exactly = 0)]
	public void EndChanceBattle(InkCommand command)
	{
		Singleton<BossBattleHealthBar>.Instance.EndBattle(false, true);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00012812 File Offset: 0x00010A12
	[Command("player_health", Exactly = 0)]
	public void UpdatePlayerHealth(InkCommand command)
	{
		Singleton<BossBattleHealthBar>.Instance.UpdatePlayerHealth();
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0001281E File Offset: 0x00010A1E
	[Command("player_health_revive", Exactly = 0)]
	public void UpdatePlayerRevive(InkCommand command)
	{
		Singleton<BossBattleHealthBar>.Instance.RevivePlayer();
		Singleton<BossBattleHealthBar>.Instance.UpdatePlayerHealth();
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00012834 File Offset: 0x00010A34
	[Command("boss_health", Exactly = 0)]
	public void UpdateBossHealth(InkCommand command)
	{
		Singleton<BossBattleHealthBar>.Instance.UpdateDishyHealth();
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00012840 File Offset: 0x00010A40
	[Command("COFFEE_USE", Exactly = 0)]
	public void CoffeeDate(InkCommand command)
	{
		CoffeeMachine.Instance.TriggerDate();
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0001284C File Offset: 0x00010A4C
	[Command("turner", Exactly = 1)]
	public void MoveTurnerArt(InkCommand command)
	{
		MovingDateable.MoveDateable("TurnerArt", command[0], true);
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00012860 File Offset: 0x00010A60
	[Command("muromachi", Exactly = 1)]
	public void MoveMuromachiArt(InkCommand command)
	{
		MovingDateable.MoveDateable("MuromachiArt", command[0], true);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00012874 File Offset: 0x00010A74
	[Command("house", Exactly = 1)]
	public void MoveHouseArt(InkCommand command)
	{
		MovingDateable.MoveDateable("HouseArt", command[0], true);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00012888 File Offset: 0x00010A88
	[Command("banksy", Exactly = 1)]
	public void MoveBanksyArt(InkCommand command)
	{
		MovingDateable.MoveDateable("BanksyArt", command[0], true);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0001289C File Offset: 0x00010A9C
	[Command("DAEMON_GLITCH_ENABLED", Exactly = 0)]
	public void EnableDaemonGlitch()
	{
		Dresser.Instance.disabled = false;
		Singleton<InkController>.Instance.TrySetVariable("daemon_glitch_enabled", "true");
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x000128BD File Offset: 0x00010ABD
	[Command("DAEMON_GLITCH_DISABLED", Exactly = 0)]
	public void DisableDaemonGlitch()
	{
		Dresser.Instance.disabled = true;
		Singleton<InkController>.Instance.TrySetVariable("daemon_glitch_enabled", "false");
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x000128DE File Offset: 0x00010ADE
	[Command("CHANCE_CRIT", Exactly = 0)]
	public void ChanceCrit(InkCommand command)
	{
		MovingDateable.MoveDateable("d20", "love", true);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x000128F0 File Offset: 0x00010AF0
	[Command("CHANCE_FAIL", Exactly = 0)]
	public void ChanceFail(InkCommand command)
	{
		MovingDateable.MoveDateable("d20", "hate", true);
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00012902 File Offset: 0x00010B02
	[Command("CHANCE_NORMAL", Exactly = 0)]
	public void ChanceNormal(InkCommand command)
	{
		MovingDateable.MoveDateable("d20", "normal", true);
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00012914 File Offset: 0x00010B14
	[Command("FAST", Exactly = 0)]
	public void Fast(InkCommand command)
	{
		BetterPlayerControl.Instance.SpeedBoost();
	}

	// Token: 0x0400032E RID: 814
	private StageManager stageManagerRef;
}
