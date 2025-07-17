using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000075 RID: 117
public class Candelabra : MonoBehaviour, IReloadHandler
{
	// Token: 0x060003F2 RID: 1010 RVA: 0x00018A71 File Offset: 0x00016C71
	private void Awake()
	{
		Candelabra.Instance = this;
		UnityEvent dialogueExit = Singleton<GameController>.Instance.DialogueExit;
		if (dialogueExit == null)
		{
			return;
		}
		dialogueExit.AddListener(new UnityAction(this.CheckToDisableKissing));
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00018A99 File Offset: 0x00016C99
	public void SetLevel(int _level)
	{
		if (!this.genInt.animator)
		{
			return;
		}
		this.genInt.animator.SetInteger("Level", _level);
		this.level = _level;
		this.kissedAtLevel = false;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x00018AD4 File Offset: 0x00016CD4
	private void CheckToDisableKissing()
	{
		bool flag;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("maggie_quest2_solved"), out flag);
		if (flag)
		{
			this.level = 0;
		}
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x00018B04 File Offset: 0x00016D04
	public void Kiss()
	{
		switch (this.level)
		{
		case 1:
			this.genInt.SetCameraVars(GenericInteractable.AnimType.Both, BetterPlayerControl.PlayerState.CantControl);
			this.FirstKiss();
			return;
		case 2:
			this.genInt.SetCameraVars(GenericInteractable.AnimType.Both, BetterPlayerControl.PlayerState.CantControl);
			this.SecondKiss();
			return;
		case 3:
			this.genInt.SetCameraVars(GenericInteractable.AnimType.Both, BetterPlayerControl.PlayerState.CantControl);
			this.ThirdKiss();
			return;
		case 4:
			this.CompleteQuest();
			return;
		default:
			this.genInt.SetCameraVars(GenericInteractable.AnimType.Neither, BetterPlayerControl.PlayerState.CanControl);
			if (this.kissClips[0] != null)
			{
				this.genInt.standardSfx_activate = new List<AudioClip> { this.kissClips[0] };
			}
			return;
		}
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x00018BB0 File Offset: 0x00016DB0
	public void FirstKiss()
	{
		Singleton<InkController>.Instance.story.variablesState["candle_kiss"] = "1_kissed";
		if (this.kissClips[this.level] != null)
		{
			this.genInt.standardSfx_activate = new List<AudioClip> { this.kissClips[this.level] };
		}
		this.kissedAtLevel = true;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x00018C1C File Offset: 0x00016E1C
	public void SecondKiss()
	{
		Singleton<InkController>.Instance.story.variablesState["candle_kiss"] = "2_kissed";
		if (this.kissClips[this.level] != null)
		{
			this.genInt.standardSfx_activate = new List<AudioClip> { this.kissClips[this.level] };
		}
		this.kissedAtLevel = true;
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00018C88 File Offset: 0x00016E88
	public void ThirdKiss()
	{
		Singleton<InkController>.Instance.story.variablesState["candle_kiss"] = "3_kissed";
		if (this.kissClips[this.level] != null)
		{
			this.genInt.standardSfx_activate = new List<AudioClip> { this.kissClips[this.level] };
		}
		Singleton<AchievementController>.Instance.CheckPlaythrough(AchievementId.GRAPHIC_DEPICTIONS);
		this.kissedAtLevel = true;
		this.genInt.blockMagical = false;
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x00018D0B File Offset: 0x00016F0B
	public void CompleteQuest()
	{
		this.genInt.whichUsesCam = GenericInteractable.AnimType.Neither;
		this.genInt.blockMagical = false;
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00018D28 File Offset: 0x00016F28
	public void ToggleMagical()
	{
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("stageon"), out this.jonState);
		if (this.genInt.blockMagical)
		{
			return;
		}
		this.genInt.whichUsesCam = GenericInteractable.AnimType.Neither;
		if (this.magicalState)
		{
			if (!this.jonState)
			{
				if (Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Friend || Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Love)
				{
					Singleton<AudioManager>.Instance.PlayTrack(this.lovefriendOffLine, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
				}
			}
			else if (Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Friend || Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Love)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.jonlovefriendOffLine, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
			}
			for (int i = 0; i < 3; i++)
			{
				this.candleFlames[i].SetActive(false);
				this.genInt.state = false;
				this.genInt.PlayMagicalSound();
				this.magicalState = false;
			}
			return;
		}
		if (!this.jonState)
		{
			if (Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Friend || Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Love)
			{
				Singleton<AudioManager>.Instance.PlayTrack(this.lovefriendOnLine, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
			}
		}
		else if (Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Friend || Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Love)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.jonlovefriendOnLine, AUDIO_TYPE.SFX, false, false, 0f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, true);
		}
		for (int j = 0; j < 3; j++)
		{
			this.candleFlames[j].SetActive(true);
			this.genInt.state = true;
			this.genInt.PlayMagicalSound();
			this.magicalState = true;
		}
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00018F26 File Offset: 0x00017126
	public int Priority()
	{
		return 1000;
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00018F30 File Offset: 0x00017130
	public void LoadState()
	{
		string variable = Singleton<InkController>.Instance.GetVariable("candle_kiss");
		bool flag;
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("maggie_quest2_solved"), out flag);
		bool.TryParse(Singleton<InkController>.Instance.GetVariable("stageon"), out this.jonState);
		if (flag || this.jonState)
		{
			this.CompleteQuest();
			return;
		}
		if (variable != "off")
		{
			if (!(variable == "1_not_kissed"))
			{
				if (!(variable == "2_not_kissed"))
				{
					if (!(variable == "3_not_kissed"))
					{
						if (!(variable == "1_kissed"))
						{
							if (!(variable == "2_kissed"))
							{
								if (variable == "3_kissed")
								{
									this.level = 3;
									this.kissedAtLevel = true;
								}
							}
							else
							{
								this.level = 2;
								this.kissedAtLevel = true;
							}
						}
						else
						{
							this.level = 1;
							this.kissedAtLevel = true;
						}
					}
					else
					{
						this.level = 3;
					}
				}
				else
				{
					this.level = 2;
				}
			}
			else
			{
				this.level = 1;
			}
			this.genInt.animator.SetInteger("Level", this.level);
		}
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x00019055 File Offset: 0x00017255
	public void SaveState()
	{
	}

	// Token: 0x040003EC RID: 1004
	public int level;

	// Token: 0x040003ED RID: 1005
	public bool kissedAtLevel;

	// Token: 0x040003EE RID: 1006
	public static Candelabra Instance;

	// Token: 0x040003EF RID: 1007
	[SerializeField]
	private GenericInteractable genInt;

	// Token: 0x040003F0 RID: 1008
	[SerializeField]
	private AudioClip[] kissClips = new AudioClip[4];

	// Token: 0x040003F1 RID: 1009
	[SerializeField]
	private AudioClip lovefriendOnLine;

	// Token: 0x040003F2 RID: 1010
	[SerializeField]
	private AudioClip lovefriendOffLine;

	// Token: 0x040003F3 RID: 1011
	[SerializeField]
	private AudioClip jonlovefriendOnLine;

	// Token: 0x040003F4 RID: 1012
	[SerializeField]
	private AudioClip jonlovefriendOffLine;

	// Token: 0x040003F5 RID: 1013
	[SerializeField]
	[LabeledBool("Magical On", "Magical Off")]
	private bool magicalState;

	// Token: 0x040003F6 RID: 1014
	[SerializeField]
	[LabeledBool("Jon On", "Jon Off")]
	private bool jonState;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private GameObject[] candleFlames = new GameObject[3];
}
