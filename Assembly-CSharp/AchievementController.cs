using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using T17.Services;
using Team17.Common;
using Team17.Platform.Achievements;
using Team17.Platform.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000011 RID: 17
public class AchievementController : Singleton<AchievementController>
{
	// Token: 0x0600002B RID: 43 RVA: 0x00002AEE File Offset: 0x00000CEE
	public override void AwakeSingleton()
	{
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002AF0 File Offset: 0x00000CF0
	private void Start()
	{
		this.CheckAwaken();
		Save.onGameLoad += this.OnGameLoaded;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002B0A File Offset: 0x00000D0A
	private void OnDestroy()
	{
		Save.onGameLoad -= this.OnGameLoaded;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002B1D File Offset: 0x00000D1D
	private void OnGameLoaded()
	{
		this.CheckAwaken();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002B28 File Offset: 0x00000D28
	public bool RequestAchievement(AchievementType type)
	{
		switch (type)
		{
		case AchievementType.AWAKEN_DATEABLE:
			return this.CheckAwaken();
		case AchievementType.REALIZE:
			return this.CheckRealize();
		case AchievementType.COLLECTABLE:
			return this.CheckCollectable();
		case AchievementType.PLAYTHROUGH:
			return this.CheckInkVariables();
		case AchievementType.ENDINGS:
			return this.CheckEndings();
		case AchievementType.LEAVE_HOUSE:
			return this.CheckLeaveHouse();
		case AchievementType.SPECS:
			return this.CheckSpecs();
		default:
			return false;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002B8C File Offset: 0x00000D8C
	public Achievement GetAchievement(AchievementId achievementId)
	{
		return this.AchievementList.Find((Achievement x) => x.achievementId == achievementId);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002BBD File Offset: 0x00000DBD
	private void GrantAchievement(Achievement achievement)
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		AchievementController.GrantAchievementOnPlatform(achievement);
		AchievementController.CheckPlatinumOnPlatform();
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002BD4 File Offset: 0x00000DD4
	private static void CheckPlatinumOnPlatform()
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		IUser user;
		if (!Services.UserService.TryGetPrimaryUser(out user))
		{
			T17Debug.LogError("[ACHIEVEMENT] Unable to check achievments because the primary user couldnt be found.");
			return;
		}
		RuntimeHelpers.InitializeArray(new AchievementId[42], fieldof(<PrivateImplementationDetails>.D90BFA0F57CBBD04892FAF39D126A4D506331CE0497F7672027BA9B82E06F1BD).FieldHandle);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002C18 File Offset: 0x00000E18
	private static void GrantAchievementOnPlatform(Achievement achievement)
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		IUser user;
		if (!Services.UserService.TryGetPrimaryUser(out user))
		{
			T17Debug.LogError(string.Format("[ACHIEVEMENT] Unable to unlock achievment '{0}' because the primary user couldnt be found.", achievement));
			return;
		}
		Services.PlatformAchievementsService.Unlock(user, achievement);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002C58 File Offset: 0x00000E58
	public void HideAchievementModal()
	{
		this.MockAchievementPanel.SetActive(false);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002C66 File Offset: 0x00000E66
	public bool CheckPlaythrough(AchievementId achievementId)
	{
		this.GrantAchievement(this.GetAchievement(achievementId));
		return true;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002C78 File Offset: 0x00000E78
	private bool CheckInkVariables()
	{
		if (Singleton<InkController>.Instance.GetVariable("do") == "103")
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.CONSUMMATE_PROFESSIONAL));
			return true;
		}
		bool flag = Singleton<InkController>.Instance.GetVariable("washford_drysdale").ToLowerInvariant() == "complete";
		bool flag2 = Singleton<InkController>.Instance.GetVariable("tina_tony").ToLowerInvariant() == "triangle";
		bool flag3 = Singleton<InkController>.Instance.GetVariable("cf_ending").ToLowerInvariant() == "friend" || Singleton<InkController>.Instance.GetVariable("cf_ending").ToLowerInvariant() == "love";
		bool flag4 = Singleton<InkController>.Instance.GetVariable("doug_artt").ToLowerInvariant() == "complete";
		bool flag5 = (bool)Singleton<InkController>.Instance.story.variablesState["abel_dasha_together"];
		if (flag && flag2 && flag3 && flag4 && flag5)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.MATCHMAKER));
			return true;
		}
		if ((bool)Singleton<InkController>.Instance.story.variablesState["sinclaire_turtle_realized"])
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.TERRAPIN_DROP));
			return true;
		}
		if ((bool)Singleton<InkController>.Instance.story.variablesState["wallace_true_friend"])
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.THESE_WALLS_COULD_TALK));
			return true;
		}
		if ((int)Singleton<InkController>.Instance.story.variablesState["dorians_met"] == 17)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.TOTAL_DORK));
			return true;
		}
		if (Singleton<InkController>.Instance.GetVariable("zoey_seance_ready").ToLowerInvariant() == "complete")
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.WIGHT_MAKES_RIGHT));
			return true;
		}
		return false;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002E49 File Offset: 0x00001049
	public bool CheckBoxingDay()
	{
		if (Singleton<Save>.Instance.GetBoxExamenData().Keys.Count >= 42)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.BOXING_DAY));
			return true;
		}
		return false;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002E74 File Offset: 0x00001074
	private bool CheckLeaveHouse()
	{
		bool flag = false;
		int num = Singleton<Save>.Instance.TotalRealizedDatables();
		RelationshipStatus dateStatusRealized = Singleton<Save>.Instance.GetDateStatusRealized("reggie");
		if (num != 0)
		{
			if (num != 1)
			{
			}
		}
		else
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.TOUCH_GRASS));
			flag = true;
		}
		int num2 = Singleton<Save>.Instance.TotalMetDatables();
		int statLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("smarts", true);
		int statLevel2 = Singleton<SpecStatMain>.Instance.GetStatLevel("poise", true);
		int statLevel3 = Singleton<SpecStatMain>.Instance.GetStatLevel("empathy", true);
		int statLevel4 = Singleton<SpecStatMain>.Instance.GetStatLevel("charm", true);
		int statLevel5 = Singleton<SpecStatMain>.Instance.GetStatLevel("sass", true);
		if (num2 >= 50 && (statLevel >= 100 || statLevel2 >= 100 || statLevel3 >= 100 || statLevel4 >= 100 || statLevel5 >= 100))
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.BOSS_FORM));
			flag = true;
		}
		if (Singleton<InkController>.Instance.story.variablesState["david_franklin"].ToString().ToLowerInvariant() == "love")
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.MOST_AND_LIESTE));
			flag = true;
		}
		return flag;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002F94 File Offset: 0x00001194
	private bool CheckRealize()
	{
		int num = Singleton<Save>.Instance.TotalRealizedDatables();
		if (num <= 50)
		{
			if (num == 1)
			{
				this.GrantAchievement(this.GetAchievement(AchievementId.JUST_BEGINNING_TO_REALIZE));
				return true;
			}
			if (num == 50)
			{
				this.GrantAchievement(this.GetAchievement(AchievementId.HALF_IN_HALF_OUT));
				return true;
			}
		}
		else
		{
			if (num == 90)
			{
				return true;
			}
			if (num == 100)
			{
				this.GrantAchievement(this.GetAchievement(AchievementId.EMPTY_NEST));
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002FFC File Offset: 0x000011FC
	private bool CheckAwaken()
	{
		int num = Singleton<Save>.Instance.TotalMetDatables();
		int num2 = AchievementController.TotalMetBaseDateables(num);
		if (num == 2)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.AWAKENED));
			return true;
		}
		if (num == 10)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.AWAKENINGS));
			return true;
		}
		if (num == 50)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.CROWD_CONTROL));
			return true;
		}
		if (num2 == 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.CAUGHT_THEM_ALL));
			return true;
		}
		return false;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0000306C File Offset: 0x0000126C
	private static int TotalMetBaseDateables(int numberOfDateablesAwakened)
	{
		bool flag = Singleton<Save>.Instance.GetDateStatus("lucinda") > RelationshipStatus.Unmet;
		bool flag2 = Singleton<Save>.Instance.GetDateStatus("mikey") > RelationshipStatus.Unmet;
		if (flag)
		{
			numberOfDateablesAwakened--;
		}
		if (flag2)
		{
			numberOfDateablesAwakened--;
		}
		return numberOfDateablesAwakened;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000030B0 File Offset: 0x000012B0
	private bool CheckEndings()
	{
		int num = Singleton<Save>.Instance.TotalFriendEndings();
		int num2 = Singleton<Save>.Instance.TotalLoveEndings();
		int num3 = Singleton<Save>.Instance.TotalHateEndings();
		RelationshipStatus dateStatus = Singleton<Save>.Instance.GetDateStatus("fantina");
		RelationshipStatus dateStatus2 = Singleton<Save>.Instance.GetDateStatus("sassy_chap");
		RelationshipStatus dateStatus3 = Singleton<Save>.Instance.GetDateStatus("skylar");
		bool flag = false;
		if (num >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.RELATE_EVERYTHING));
			flag = true;
		}
		if (num2 >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.MATE_EVERYTHING));
			flag = true;
		}
		if (num3 >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.HATE_EVERYTHING));
			flag = true;
		}
		if (dateStatus == RelationshipStatus.Hate)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.FROM_GREAT_HEIGHTS));
			flag = true;
		}
		if (dateStatus2 == RelationshipStatus.Hate)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.THANKS_FOR_YOUR_PURCHASE));
			flag = true;
		}
		if (dateStatus3 == RelationshipStatus.Love || dateStatus3 == RelationshipStatus.Friend)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.DO_NO_HARM));
			flag = true;
		}
		return flag;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x0000319C File Offset: 0x0000139C
	private bool CheckCollectable()
	{
		bool flag = false;
		int totalUnlockedCollectables = Singleton<Save>.Instance.GetTotalUnlockedCollectables(false);
		if (totalUnlockedCollectables >= 1 && !Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.FINDERS_KEEPERS));
			flag = true;
		}
		if (totalUnlockedCollectables >= 2 && Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.FINDERS_KEEPERS));
			flag = true;
		}
		if (totalUnlockedCollectables >= 50)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.LORE_THAN_YOU_BARGAINING_FOR));
			flag = true;
		}
		if (totalUnlockedCollectables >= 200)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.IT_LL_BE_WORTH_SOMETHING_SOMEDAY));
			flag = true;
		}
		if (totalUnlockedCollectables >= Singleton<Save>.Instance.GetTotalCollectables(false))
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.COLLECTION_COMPLETE));
			flag = true;
		}
		return flag;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003244 File Offset: 0x00001444
	private bool CheckSpecs()
	{
		int statLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("smarts", true);
		int statLevel2 = Singleton<SpecStatMain>.Instance.GetStatLevel("poise", true);
		int statLevel3 = Singleton<SpecStatMain>.Instance.GetStatLevel("empathy", true);
		int statLevel4 = Singleton<SpecStatMain>.Instance.GetStatLevel("charm", true);
		int statLevel5 = Singleton<SpecStatMain>.Instance.GetStatLevel("sass", true);
		bool flag = false;
		if (statLevel >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.SMARTY_PANTS));
			flag = true;
		}
		if (statLevel2 >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.HUMAN_CUCUMBER));
			flag = true;
		}
		if (statLevel3 >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.SEER_OF_SOULS));
			flag = true;
		}
		if (statLevel4 >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.PIZZAZZSTER));
			flag = true;
		}
		if (statLevel5 >= 100)
		{
			this.GrantAchievement(this.GetAchievement(AchievementId.SASSY_CHAP));
			flag = true;
		}
		return flag;
	}

	// Token: 0x04000061 RID: 97
	private IAchievementsService AchievementService;

	// Token: 0x04000062 RID: 98
	public List<Achievement> AchievementList;

	// Token: 0x04000063 RID: 99
	public GameObject MockAchievementPanel;

	// Token: 0x04000064 RID: 100
	public Image MockAchievementImage;

	// Token: 0x04000065 RID: 101
	public TextMeshProUGUI MockAchievementText;
}
