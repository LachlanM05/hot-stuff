using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200011A RID: 282
public class NameEntrySounds : MonoBehaviour
{
	// Token: 0x06000991 RID: 2449 RVA: 0x00037267 File Offset: 0x00035467
	private void Start()
	{
		this.previousStringLength = this.inputField.text.Length;
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0003727F File Offset: 0x0003547F
	public void Initialize()
	{
		this.initialized = true;
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x00037288 File Offset: 0x00035488
	private void OnEnable()
	{
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.PlaySoundFor));
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x000372A6 File Offset: 0x000354A6
	private void OnDisable()
	{
		this.inputField.onValueChanged.RemoveListener(new UnityAction<string>(this.PlaySoundFor));
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x000372C4 File Offset: 0x000354C4
	public void PlaySoundFor(string entry)
	{
		if (!this.initialized || !ValidateQuestions.Instance.visible)
		{
			return;
		}
		if (entry.Length < this.previousStringLength)
		{
			this.previousStringLength = entry.Length;
			this.PlayBackSound();
			return;
		}
		char c = ((entry.Length > 0) ? entry.ToLowerInvariant().Last<char>() : 'a');
		if (this.TextEntrySounds.ContainsKey(c))
		{
			Singleton<AudioManager>.Instance.StopTrack("UI/Pen Input/" + this.TextEntrySounds[c], 0f);
			Singleton<AudioManager>.Instance.PlayTrack("UI/Pen Input/" + this.TextEntrySounds[c], AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.UI, null, false);
		}
		else
		{
			Singleton<AudioManager>.Instance.StopTrack("UI/Pen Input/PenInput_J", 0f);
			Singleton<AudioManager>.Instance.PlayTrack("UI/Pen Input/PenInput_J", AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.UI, null, false);
		}
		this.previousStringLength = entry.Length;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x000373D0 File Offset: 0x000355D0
	public void PlayBackSound()
	{
		if (!this.initialized)
		{
			return;
		}
		Singleton<AudioManager>.Instance.StopTrack("UI/Pen Input/UI_Backspace", 0f);
		Singleton<AudioManager>.Instance.PlayTrack("UI/Pen Input/UI_Backspace", AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, null, SFX_SUBGROUP.UI, null, false);
	}

	// Token: 0x040008D7 RID: 2263
	private Dictionary<char, string> TextEntrySounds = new Dictionary<char, string>
	{
		{ 'a', "PenInput_A" },
		{ 'b', "PenInput_B" },
		{ 'c', "PenInput_C" },
		{ 'd', "PenInput_D" },
		{ 'e', "PenInput_E" },
		{ 'f', "PenInput_F" },
		{ 'g', "PenInput_G" },
		{ 'h', "PenInput_H" },
		{ 'i', "PenInput_I" },
		{ 'j', "PenInput_J" },
		{ 'k', "PenInput_K" },
		{ 'l', "PenInput_L" },
		{ 'm', "PenInput_M" },
		{ 'n', "PenInput_N" },
		{ 'o', "PenInput_O" },
		{ 'p', "PenInput_P" },
		{ 'q', "PenInput_Q" },
		{ 'r', "PenInput_R" },
		{ 's', "PenInput_S" },
		{ 't', "PenInput_T" },
		{ 'u', "PenInput_U" },
		{ 'v', "PenInput_V" },
		{ 'w', "PenInput_W" },
		{ 'x', "PenInput_X" },
		{ 'y', "PenInput_Y" },
		{ 'z', "PenInput_Z" },
		{ '\b', "UI_Backspace" }
	};

	// Token: 0x040008D8 RID: 2264
	[SerializeField]
	private TMP_InputField inputField;

	// Token: 0x040008D9 RID: 2265
	private int previousStringLength;

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private bool initialized;
}
