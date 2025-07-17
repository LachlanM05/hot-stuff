using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CowberryStudios.ProjectAI;
using Rewired;
using T17.Services;
using T17.UI;
using Team17.Common;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class SpecStatMain : Singleton<SpecStatMain>
{
	// Token: 0x06000B1C RID: 2844 RVA: 0x0003F9BC File Offset: 0x0003DBBC
	public void Start()
	{
		Save.onGameLoad += this.OnGameLoad;
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0003F9CF File Offset: 0x0003DBCF
	public void OnGameLoad()
	{
		this.loadedyet = true;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0003F9D8 File Offset: 0x0003DBD8
	public void Update()
	{
		if (this.visible)
		{
			this.ProcessInput();
		}
		if (this.suppressBackTimer != 0f && (this.suppressBackTimer -= Time.deltaTime) < 0f)
		{
			this.suppressBackTimer = 0f;
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0003FA28 File Offset: 0x0003DC28
	private void ProcessInput()
	{
		if (!this.IsSuppressed())
		{
			if (Services.UIInputService.IsUICancelDown())
			{
				this.BackPressed();
				return;
			}
			if (this.currentPage == SpecStatMain.SPECS_Page.Stats)
			{
				if (this.player.GetButtonDown(50))
				{
					this.ToggleTooltips();
					this.PlayControllerOnOffSound(this.showTooltips);
					return;
				}
				if (this.player.GetButtonDown(51))
				{
					this.ViewGlossary();
					this.PlayControllerOnOffSound(this.IsCurrentPage(SpecStatMain.SPECS_Page.Glossary));
				}
			}
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0003FA9C File Offset: 0x0003DC9C
	private void PlayControllerOnOffSound(bool on)
	{
		if (on)
		{
			if (this.ControllerOnSound != null)
			{
				this.ControllerOnSound.ForcePlayClicked();
				return;
			}
		}
		else if (this.ControllerOffSound != null)
		{
			this.ControllerOffSound.ForcePlayClicked();
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0003FAD4 File Offset: 0x0003DCD4
	public void Enabled()
	{
		this.visible = true;
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
		}
		this.ShowStats();
		PlayerPauser.Pause(this);
		base.StartCoroutine(this.SelectInitialButton());
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0003FB0F File Offset: 0x0003DD0F
	private IEnumerator SelectInitialButton()
	{
		IsSelectableRegistered buttonToSelect;
		if (this.keyButton.GetComponent<ControllerDisplayComponent>().IsSelectionBlocked())
		{
			buttonToSelect = this.autoSelectFallback.GetComponent<IsSelectableRegistered>();
		}
		else
		{
			buttonToSelect = this.keyButton;
		}
		yield return new WaitUntil(() => buttonToSelect.IsRegistered);
		ControllerMenuUI.SetCurrentlySelected(buttonToSelect.gameObject, ControllerMenuUI.Direction.Down, false, false);
		this.OnOpenedMenu();
		yield break;
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0003FB1E File Offset: 0x0003DD1E
	private void OnOpenedMenu()
	{
		if (this.CanShowTutorialMessages() && !this.HasSeenSpecsTutorialMessages())
		{
			this.ShowSpecsTutorialMessages();
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0003FB36 File Offset: 0x0003DD36
	private void SetSeenSpecsTutorialMessages()
	{
		Singleton<Save>.Instance.SetSeenSpecsTutorialMessages();
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0003FB42 File Offset: 0x0003DD42
	private void ShowSpecsTutorialMessages()
	{
		this.SetSeenSpecsTutorialMessages();
		this.ShowTutorialMessage1(delegate
		{
			this.ShowTutorialMessage2(delegate
			{
				this.ShowTutorialMessage3(null);
			});
		});
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0003FB5C File Offset: 0x0003DD5C
	private void ShowTutorialMessage1(Action onCompleted = null)
	{
		UIDialogManager.Instance.ShowOKDialog("SPECS", "Welcome to SPECS! Here you can track how your own personality changes as you learn from the objects around you.", onCompleted, true);
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0003FB75 File Offset: 0x0003DD75
	private void ShowTutorialMessage2(Action onCompleted = null)
	{
		UIDialogManager.Instance.ShowOKDialog("SPECS", "You receive +5 to one of your attributes after attaining a Love, Friend, or even Hate status with any dateable object for the first time!", onCompleted, true);
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0003FB8E File Offset: 0x0003DD8E
	private void ShowTutorialMessage3(Action onCompleted = null)
	{
		UIDialogManager.Instance.ShowOKDialog("SPECS", "You can even plan out which attribute you might get by checking which SPECS icon shows up on each dateable’s profile in the date-a-dex…", onCompleted, true);
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0003FBA7 File Offset: 0x0003DDA7
	private bool CanShowTutorialMessages()
	{
		return !(TalkingUI.Instance != null) || !TalkingUI.Instance.open;
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0003FBC5 File Offset: 0x0003DDC5
	private bool HasSeenSpecsTutorialMessages()
	{
		return Singleton<Save>.Instance.HasSeenSpecsTutorialMessages();
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x0003FBD4 File Offset: 0x0003DDD4
	public void Disabled()
	{
		this.visible = false;
		foreach (SpecStatMain.StatBlockRef statBlockRef in this.Active_Stat_Blocks)
		{
		}
		if (TalkingUI.Instance)
		{
			TalkingUI.Instance.UnpauseCoroutine("SpecStatMain");
		}
		PlayerPauser.Unpause();
		if (TalkingUI.Instance != null && TalkingUI.Instance.open && !Singleton<InkController>.Instance.forceGoHome)
		{
			Singleton<AudioManager>.Instance.StopSFXTrackGroup(SFX_SUBGROUP.STINGER, 0f);
			Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.DIALOGUE, 0f);
			Singleton<AudioManager>.Instance.ResumeTrackGroup(AUDIO_TYPE.MUSIC, false, 1f);
		}
		if (!Singleton<PhoneManager>.Instance.phoneOpened && Singleton<GameController>.Instance != null && !Singleton<GameController>.Instance.talkingUI.open)
		{
			CursorLocker.Lock();
		}
		if (!Singleton<GameController>.Instance.talkingUI.autoPlayNextVo && TalkingUI.Instance.open && !Singleton<InkController>.Instance.forceGoHome)
		{
			Singleton<GameController>.Instance.talkingUI.autoPlayNextVo = true;
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

	// Token: 0x06000B2C RID: 2860 RVA: 0x0003FD8C File Offset: 0x0003DF8C
	public void GiveStatPoint(string stat, int increment = 1)
	{
		int num = this.GetStatLevel(stat, false);
		if (num > 100)
		{
			num = 100;
		}
		this.SetStatPoint(stat, num, increment);
		Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.SPECS);
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0003FDC0 File Offset: 0x0003DFC0
	public void GiveStatPointFinalValue(string stat, int value = 1)
	{
		int num = value;
		if (num > 100)
		{
			num = 100;
		}
		this.SetStatPoint(stat, num, 0);
		Singleton<AchievementController>.Instance.RequestAchievement(AchievementType.SPECS);
	}

	// Token: 0x06000B2E RID: 2862
	public void SetStatPoint(string stat, int value, int increment)
	{
		value = 100;
		int num = this.GetStatLevel(stat, true) - increment;
		Singleton<InkController>.Instance.story.variablesState[stat.ToLowerInvariant().Trim()] = Mathf.Clamp(value, 0, this.Stat_Max);
		SpecStatBlock statBlockByName = this.GetStatBlockByName(stat);
		if (this.visible && statBlockByName != null)
		{
			statBlockByName.UpdateValue(num, value);
			base.StartCoroutine(this.WaitAndAnimateSPEC());
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0003FE63 File Offset: 0x0003E063
	public bool CheckStatLevel(string stat, int check)
	{
		return this.GetStatLevel(stat, true) >= check;
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0003FE74 File Offset: 0x0003E074
	public int GetStatLevel(string stat, bool includeDLC = true)
	{
		string text = stat.ToLowerInvariant().Trim();
		int num;
		if (text.Contains("_dlc"))
		{
			num = (int)Singleton<InkController>.Instance.story.variablesState[text];
		}
		else
		{
			num = (int)Singleton<InkController>.Instance.story.variablesState[text];
			int num2;
			if (includeDLC && Singleton<DeluxeEditionController>.Instance != null && Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION && this.TryGetDLCValueForStat(text, out num2))
			{
				num += num2;
			}
		}
		if (num > 100)
		{
			return 100;
		}
		return num;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0003FF08 File Offset: 0x0003E108
	private bool TryGetDLCValueForStat(string stat, out int value)
	{
		string text = stat + "_dlc";
		object obj = Singleton<InkController>.Instance.story.variablesState[text];
		if (obj == null)
		{
			value = 0;
			return false;
		}
		string text2 = obj.ToString();
		if (int.TryParse(text2, out value))
		{
			return true;
		}
		T17Debug.LogError(string.Concat(new string[] { "Failed to parse stat '", text, "' value : '", text2, "'" }));
		return false;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0003FF84 File Offset: 0x0003E184
	public string GetStatAdjective(string stat)
	{
		string text = Singleton<InkController>.Instance.GetStitchesAtLocation("specs").FirstOrDefault((string x) => x.EndsWith(stat));
		string[] array = Singleton<InkController>.Instance.story.TagsForContentAtPath(text)[2].Split(": ", StringSplitOptions.None)[1].Split(",", StringSplitOptions.None);
		int num = Mathf.Clamp(Mathf.FloorToInt((float)this.GetStatLevel(stat, true) / ((float)this.Stat_Max / (float)(array.Length - 1))), 0, array.Length - 1);
		string text2 = array[num];
		if (string.IsNullOrEmpty(text2))
		{
			return string.Empty;
		}
		return text2.Trim();
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00040038 File Offset: 0x0003E238
	private void ShowStats()
	{
		if (!this.loadedyet)
		{
			return;
		}
		this.HideAllTips();
		this.SuppressTime(2f);
		int num = this.Stat_Max;
		foreach (string text in Singleton<InkController>.Instance.GetStitchesAtLocation("specs"))
		{
			List<string> list = Singleton<InkController>.Instance.story.TagsForContentAtPath(text);
			string text2 = list[0].Split(": ", StringSplitOptions.None)[1];
			string text3 = list[1].Split(": ", StringSplitOptions.None)[1];
			string[] array = list[2].Split(": ", StringSplitOptions.None)[1].Split(",", StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Trim();
			}
			string[] array2 = list[3].Split(": ", StringSplitOptions.None)[1].Split(">>>", StringSplitOptions.None);
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = array2[j].Trim();
			}
			int statLevel = this.GetStatLevel(text2.ToLowerInvariant(), true);
			SpecStatBlock statBlockByName = this.GetStatBlockByName(text2);
			if (statBlockByName != null)
			{
				statBlockByName.Init(text2, array, array2);
				statBlockByName.UpdateValue(0, statLevel);
				base.StartCoroutine(this.WaitAndAnimateSPEC());
				if (statLevel < num)
				{
					num = statLevel;
				}
			}
			SpecGlossaryBlock glossaryBlockByName = this.GetGlossaryBlockByName(text2);
			if (glossaryBlockByName != null)
			{
				glossaryBlockByName.Init(text2, text3);
			}
		}
		this.UpdateStarGraphic();
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x000401F8 File Offset: 0x0003E3F8
	public void ViewStats()
	{
		this.GoToPage(SpecStatMain.SPECS_Page.Stats);
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x00040201 File Offset: 0x0003E401
	public void ViewGlossary()
	{
		this.GoToPage(SpecStatMain.SPECS_Page.Glossary);
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0004020C File Offset: 0x0003E40C
	private void GoToPage(SpecStatMain.SPECS_Page page)
	{
		if (this.currentPage != page)
		{
			this.SuppressTime(-1f);
			this.screenAnimator.SetTrigger("View" + page.ToString());
			this.currentPage = page;
			for (int i = this.disableInGlossary.Length - 1; i >= 0; i--)
			{
				if (this.disableInGlossary[i] != null)
				{
					this.disableInGlossary[i].gameObject.SetActive(this.currentPage == SpecStatMain.SPECS_Page.Stats);
				}
			}
		}
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x00040296 File Offset: 0x0003E496
	private bool IsCurrentPage(SpecStatMain.SPECS_Page page)
	{
		return this.currentPage == page;
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x000402A4 File Offset: 0x0003E4A4
	public void UpdateStarGraphic()
	{
		int num = this.Stat_Max;
		foreach (SpecStatMain.StatBlockRef statBlockRef in this.Active_Stat_Blocks)
		{
			int num2 = Mathf.FloorToInt((float)statBlockRef.StatBlock.currentValue);
			if (num2 < num)
			{
				num = num2;
			}
		}
		float num3 = (float)num / (float)this.Stat_Max;
		Mathf.Lerp(0.1f, 1f, num3);
		foreach (SpecStatMain.StatBlockRef statBlockRef2 in this.Active_Stat_Blocks)
		{
			statBlockRef2.StatBlock.OnUpdateStarCenter(num3);
		}
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x00040370 File Offset: 0x0003E570
	private SpecStatBlock GetStatBlockByName(string statName)
	{
		foreach (SpecStatMain.StatBlockRef statBlockRef in this.Active_Stat_Blocks)
		{
			if (statBlockRef.StatName.Trim().ToLowerInvariant() == statName.Trim().ToLowerInvariant())
			{
				return statBlockRef.StatBlock;
			}
		}
		return null;
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x000403EC File Offset: 0x0003E5EC
	private SpecGlossaryBlock GetGlossaryBlockByName(string statName)
	{
		foreach (SpecStatMain.StatBlockRef statBlockRef in this.Active_Stat_Blocks)
		{
			if (statBlockRef.StatName.Trim().ToLowerInvariant() == statName.Trim().ToLowerInvariant())
			{
				return statBlockRef.GlossaryBlock;
			}
		}
		return null;
	}

	// Token: 0x06000B3B RID: 2875 RVA: 0x00040468 File Offset: 0x0003E668
	public static SpecsAttributes ParseSpecsAttributes(string specsAttribute)
	{
		string text = specsAttribute.ToUpperInvariant();
		if (text == "SMARTS")
		{
			return SpecsAttributes.Smarts;
		}
		if (text == "POISE")
		{
			return SpecsAttributes.Poise;
		}
		if (text == "EMPATHY")
		{
			return SpecsAttributes.Empathy;
		}
		if (!(text == "CHARM"))
		{
			return SpecsAttributes.Sass;
		}
		return SpecsAttributes.Charm;
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x000404BB File Offset: 0x0003E6BB
	private IEnumerator WaitAndAnimateSPEC()
	{
		while (DateADex.Instance.resultSplashScreen.activeInHierarchy)
		{
			yield return new WaitForEndOfFrame();
		}
		this.glassesAnimitionComp.Trigger();
		this.starParticles.Play();
		yield break;
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x000404CA File Offset: 0x0003E6CA
	private void OnEnable()
	{
		this.SuppressTime(2f);
		PlayerPauser.Pause(this);
		base.StartCoroutine(this.SelectInitialButton());
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x000404EC File Offset: 0x0003E6EC
	private void OnDisable()
	{
		PlayerPauser.Unpause(this);
		if (Singleton<GameController>.Instance != null && Singleton<GameController>.Instance.talkingUI != null)
		{
			if (Singleton<PhoneManager>.Instance != null && !Singleton<PhoneManager>.Instance.phoneOpened && !Singleton<GameController>.Instance.talkingUI.open)
			{
				CursorLocker.Lock();
			}
			else if (Singleton<GameController>.Instance.talkingUI.open)
			{
				CursorLocker.Unlock();
			}
		}
		this.HideAllTips();
		Save.onGameLoad -= this.OnGameLoad;
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0004057C File Offset: 0x0003E77C
	public void ToggleTooltips()
	{
		this.showTooltips = !this.showTooltips;
		GameObject[] array = this.statTooltips;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(this.showTooltips);
		}
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x000405BB File Offset: 0x0003E7BB
	public void ShowTooltip(int _index)
	{
		if (_index >= 0 && this.statTooltips.Length > _index && this.statTooltips[_index] != null)
		{
			this.statTooltips[_index].SetActive(true);
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x000405EA File Offset: 0x0003E7EA
	public void HideTooltip(int _index)
	{
		if (_index >= 0 && this.statTooltips.Length > _index && this.statTooltips[_index] != null)
		{
			this.statTooltips[_index].SetActive(false);
		}
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x00040619 File Offset: 0x0003E819
	public void HideAllTips()
	{
		this.showTooltips = true;
		this.ToggleTooltips();
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00040628 File Offset: 0x0003E828
	public void BackPressed()
	{
		if (!this.IsSuppressed())
		{
			if (this.currentPage == SpecStatMain.SPECS_Page.Glossary)
			{
				this.ViewStats();
				return;
			}
			Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreenRotateSpecs();
		}
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x0004064C File Offset: 0x0003E84C
	private void SuppressTime(float time = -1f)
	{
		if (time == -1f)
		{
			time = 0.3f;
		}
		this.suppressBackTimer = time;
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00040664 File Offset: 0x0003E864
	public void PhoneCloseCheck()
	{
		if (Services.InputService.IsLastActiveInputController() || this.currentPage != SpecStatMain.SPECS_Page.Glossary)
		{
			Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreenRotateSpecs();
		}
		if (this.currentPage == SpecStatMain.SPECS_Page.Glossary)
		{
			this.ViewStats();
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x00040694 File Offset: 0x0003E894
	public bool IsSuppressed()
	{
		return this.suppressBackTimer != 0f;
	}

	// Token: 0x04000A2C RID: 2604
	private const int MaxStatValue = 100;

	// Token: 0x04000A2D RID: 2605
	public bool loadedyet;

	// Token: 0x04000A2E RID: 2606
	[SerializeField]
	private Animator screenAnimator;

	// Token: 0x04000A2F RID: 2607
	[SerializeField]
	private IsSelectableRegistered keyButton;

	// Token: 0x04000A30 RID: 2608
	[SerializeField]
	private IsSelectableRegistered autoSelectFallback;

	// Token: 0x04000A31 RID: 2609
	[SerializeField]
	private UIButtonSoundEvent ControllerOnSound;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private UIButtonSoundEvent ControllerOffSound;

	// Token: 0x04000A33 RID: 2611
	public List<SpecStatMain.StatBlockRef> Active_Stat_Blocks;

	// Token: 0x04000A34 RID: 2612
	private SpecStatMain.SPECS_Page currentPage;

	// Token: 0x04000A35 RID: 2613
	[SerializeField]
	private DoCodeAnimation glassesAnimitionComp;

	// Token: 0x04000A36 RID: 2614
	[SerializeField]
	private ParticleSystem starParticles;

	// Token: 0x04000A37 RID: 2615
	private bool showTooltips;

	// Token: 0x04000A38 RID: 2616
	[SerializeField]
	private GameObject[] statTooltips;

	// Token: 0x04000A39 RID: 2617
	private Player player;

	// Token: 0x04000A3A RID: 2618
	private int Stat_Max = 100;

	// Token: 0x04000A3B RID: 2619
	public bool visible;

	// Token: 0x04000A3C RID: 2620
	private float suppressBackTimer;

	// Token: 0x04000A3D RID: 2621
	private const float c_suppresTime = 0.3f;

	// Token: 0x04000A3E RID: 2622
	public GameObject[] disableInGlossary = new GameObject[0];

	// Token: 0x02000342 RID: 834
	[Serializable]
	public struct StatBlockRef
	{
		// Token: 0x040012E0 RID: 4832
		public string StatName;

		// Token: 0x040012E1 RID: 4833
		public SpecStatBlock StatBlock;

		// Token: 0x040012E2 RID: 4834
		public SpecGlossaryBlock GlossaryBlock;
	}

	// Token: 0x02000343 RID: 835
	private enum SPECS_Page
	{
		// Token: 0x040012E4 RID: 4836
		Stats,
		// Token: 0x040012E5 RID: 4837
		Glossary
	}
}
