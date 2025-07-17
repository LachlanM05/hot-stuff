using System;
using T17.Services;

namespace Assets.Date_Everything.Scripts.SaveVersionAutoFix
{
	// Token: 0x0200025D RID: 605
	internal class SaveAutoFixManager
	{
		// Token: 0x060013AF RID: 5039 RVA: 0x0005DCFB File Offset: 0x0005BEFB
		public static void SetHotfixVariables()
		{
			Singleton<Save>.Instance.SetTutorialThresholdState(SaveAutoFixManager.HOTFIX_2_APPLIED);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0005DD0C File Offset: 0x0005BF0C
		public static void StartSaveFixes()
		{
			if (Services.GameSettings.GetInt(Save.SettingKeySkipTutorial, 0) == 1)
			{
				Singleton<InkController>.Instance.story.variablesState["can_skip_tutorial"] = true;
			}
			if (!Singleton<Save>.Instance.GetTutorialThresholdState(SaveAutoFixManager.HOTFIX_2_APPLIED))
			{
				Singleton<Save>.Instance.SetTutorialThresholdState(SaveAutoFixManager.HOTFIX_2_APPLIED);
				if (Singleton<Save>.Instance.GetDateStatus("bathsheba") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["bathsheba_ending"] = "friend";
				}
				if (Singleton<Save>.Instance.GetDateStatus("doug") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["doug_ending"] = "friend";
				}
				if (Singleton<Save>.Instance.GetDateStatus("friar") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["friar_ending"] = "friend";
				}
				if (Singleton<Save>.Instance.GetDateStatus("luke") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["luke_ending"] = "friend";
				}
				if (Singleton<Save>.Instance.GetDateStatus("mitchell") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["mitchell_ending"] = "friend";
				}
				if (Singleton<Save>.Instance.GetDateStatus("nightmare") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["nightmare_ending"] = "friend";
				}
				if (Singleton<Save>.Instance.GetDateStatus("scandalabra") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["scandalabra_ending"] = "friend";
				}
				if (Singleton<Save>.Instance.GetDateStatus("shadow") == RelationshipStatus.Friend)
				{
					Singleton<InkController>.Instance.story.variablesState["shadow_ending"] = "friend";
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				RelationshipStatus dateStatus = Singleton<Save>.Instance.GetDateStatus("skylar");
				if (dateStatus == RelationshipStatus.Friend || dateStatus == RelationshipStatus.Hate || dateStatus == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus2 = Singleton<Save>.Instance.GetDateStatus("phoenicia");
				if (dateStatus2 == RelationshipStatus.Friend || dateStatus2 == RelationshipStatus.Hate || dateStatus2 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus3 = Singleton<Save>.Instance.GetDateStatus("wallace");
				bool flag = (bool)Singleton<InkController>.Instance.story.variablesState["wallace_candy_used"];
				if (dateStatus3 == RelationshipStatus.Friend || dateStatus3 == RelationshipStatus.Hate || dateStatus3 == RelationshipStatus.Love || flag)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus4 = Singleton<Save>.Instance.GetDateStatus("florence");
				if (dateStatus4 == RelationshipStatus.Friend || dateStatus4 == RelationshipStatus.Hate || dateStatus4 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus5 = Singleton<Save>.Instance.GetDateStatus("celia");
				if (dateStatus5 == RelationshipStatus.Friend || dateStatus5 == RelationshipStatus.Hate || dateStatus5 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus6 = Singleton<Save>.Instance.GetDateStatus("stella");
				if (dateStatus6 == RelationshipStatus.Friend || dateStatus6 == RelationshipStatus.Hate || dateStatus6 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus7 = Singleton<Save>.Instance.GetDateStatus("dorian");
				if (dateStatus7 == RelationshipStatus.Friend || dateStatus7 == RelationshipStatus.Hate || dateStatus7 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus8 = Singleton<Save>.Instance.GetDateStatus("wyndolyn");
				if (dateStatus8 == RelationshipStatus.Friend || dateStatus8 == RelationshipStatus.Hate || dateStatus8 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus9 = Singleton<Save>.Instance.GetDateStatus("curtrod");
				if (dateStatus9 == RelationshipStatus.Friend || dateStatus9 == RelationshipStatus.Hate || dateStatus9 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus10 = Singleton<Save>.Instance.GetDateStatus("shelley");
				if (dateStatus10 == RelationshipStatus.Friend || dateStatus10 == RelationshipStatus.Hate || dateStatus10 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus11 = Singleton<Save>.Instance.GetDateStatus("abel");
				if (dateStatus11 == RelationshipStatus.Friend || dateStatus11 == RelationshipStatus.Hate || dateStatus11 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus12 = Singleton<Save>.Instance.GetDateStatus("chairemi");
				if (dateStatus12 == RelationshipStatus.Friend || dateStatus12 == RelationshipStatus.Hate || dateStatus12 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus13 = Singleton<Save>.Instance.GetDateStatus("lux");
				if (dateStatus13 == RelationshipStatus.Friend || dateStatus13 == RelationshipStatus.Hate || dateStatus13 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus14 = Singleton<Save>.Instance.GetDateStatus("hector");
				string text = (string)Singleton<InkController>.Instance.story.variablesState["hector_hate"];
				if (dateStatus14 == RelationshipStatus.Friend || dateStatus14 == RelationshipStatus.Hate || dateStatus14 == RelationshipStatus.Love || text == "candy_used")
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus15 = Singleton<Save>.Instance.GetDateStatus("prissy");
				if (dateStatus15 == RelationshipStatus.Friend || dateStatus15 == RelationshipStatus.Hate || dateStatus15 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus16 = Singleton<Save>.Instance.GetDateStatus("tim");
				if (dateStatus16 == RelationshipStatus.Friend || dateStatus16 == RelationshipStatus.Hate || dateStatus16 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus17 = Singleton<Save>.Instance.GetDateStatus("artt");
				if (dateStatus17 == RelationshipStatus.Friend || dateStatus17 == RelationshipStatus.Hate || dateStatus17 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus18 = Singleton<Save>.Instance.GetDateStatus("river");
				if (dateStatus18 == RelationshipStatus.Friend || dateStatus18 == RelationshipStatus.Hate || dateStatus18 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus19 = Singleton<Save>.Instance.GetDateStatus("eddie");
				if (dateStatus19 == RelationshipStatus.Friend || dateStatus19 == RelationshipStatus.Hate || dateStatus19 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus20 = Singleton<Save>.Instance.GetDateStatus("koa");
				if (dateStatus20 == RelationshipStatus.Friend || dateStatus20 == RelationshipStatus.Hate || dateStatus20 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus21 = Singleton<Save>.Instance.GetDateStatus("dolly");
				if (dateStatus21 == RelationshipStatus.Friend || dateStatus21 == RelationshipStatus.Hate || dateStatus21 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus22 = Singleton<Save>.Instance.GetDateStatus("dante");
				bool flag2 = (bool)Singleton<InkController>.Instance.story.variablesState["dante_redo"];
				if (dateStatus22 == RelationshipStatus.Friend || dateStatus22 == RelationshipStatus.Hate || dateStatus22 == RelationshipStatus.Love || flag2)
				{
					num += 5;
				}
				RelationshipStatus dateStatus23 = Singleton<Save>.Instance.GetDateStatus("telly");
				if (dateStatus23 == RelationshipStatus.Friend || dateStatus23 == RelationshipStatus.Hate || dateStatus23 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus24 = Singleton<Save>.Instance.GetDateStatus("connie");
				bool flag3 = (bool)Singleton<InkController>.Instance.story.variablesState["connie_candy_used"];
				if (dateStatus24 == RelationshipStatus.Friend || dateStatus24 == RelationshipStatus.Hate || dateStatus24 == RelationshipStatus.Love || flag3)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus25 = Singleton<Save>.Instance.GetDateStatus("keyes");
				if (dateStatus25 == RelationshipStatus.Friend || dateStatus25 == RelationshipStatus.Hate || dateStatus25 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus26 = Singleton<Save>.Instance.GetDateStatus("gaia");
				if (dateStatus26 == RelationshipStatus.Friend || dateStatus26 == RelationshipStatus.Hate || dateStatus26 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus27 = Singleton<Save>.Instance.GetDateStatus("jacques");
				if (dateStatus27 == RelationshipStatus.Friend || dateStatus27 == RelationshipStatus.Hate || dateStatus27 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus28 = Singleton<Save>.Instance.GetDateStatus("parker");
				if (dateStatus28 == RelationshipStatus.Friend || dateStatus28 == RelationshipStatus.Hate || dateStatus28 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus29 = Singleton<Save>.Instance.GetDateStatus("mateo");
				if (dateStatus29 == RelationshipStatus.Friend || dateStatus29 == RelationshipStatus.Hate || dateStatus29 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus30 = Singleton<Save>.Instance.GetDateStatus("tina");
				if (dateStatus30 == RelationshipStatus.Friend || dateStatus30 == RelationshipStatus.Hate || dateStatus30 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus31 = Singleton<Save>.Instance.GetDateStatus("beverly");
				bool flag4 = (bool)Singleton<InkController>.Instance.story.variablesState["beverly_candy"];
				if (dateStatus31 == RelationshipStatus.Friend || dateStatus31 == RelationshipStatus.Hate || dateStatus31 == RelationshipStatus.Love || flag4)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus32 = Singleton<Save>.Instance.GetDateStatus("mitchell");
				if (dateStatus32 == RelationshipStatus.Friend || dateStatus32 == RelationshipStatus.Hate || dateStatus32 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus33 = Singleton<Save>.Instance.GetDateStatus("cabrizzio");
				if (dateStatus33 == RelationshipStatus.Friend || dateStatus33 == RelationshipStatus.Hate || dateStatus33 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus34 = Singleton<Save>.Instance.GetDateStatus("sinclaire");
				if (dateStatus34 == RelationshipStatus.Friend || dateStatus34 == RelationshipStatus.Hate || dateStatus34 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus35 = Singleton<Save>.Instance.GetDateStatus("freddy");
				if (dateStatus35 == RelationshipStatus.Friend || dateStatus35 == RelationshipStatus.Hate || dateStatus35 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus36 = Singleton<Save>.Instance.GetDateStatus("stefan");
				if (dateStatus36 == RelationshipStatus.Friend || dateStatus36 == RelationshipStatus.Hate || dateStatus36 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus37 = Singleton<Save>.Instance.GetDateStatus("luke");
				if (dateStatus37 == RelationshipStatus.Friend || dateStatus37 == RelationshipStatus.Hate || dateStatus37 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus38 = Singleton<Save>.Instance.GetDateStatus("miranda");
				if (dateStatus38 == RelationshipStatus.Friend || dateStatus38 == RelationshipStatus.Hate || dateStatus38 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus39 = Singleton<Save>.Instance.GetDateStatus("dishy");
				bool flag5 = (bool)Singleton<InkController>.Instance.story.variablesState["dishy_sass_given"];
				if (dateStatus39 == RelationshipStatus.Friend || dateStatus39 == RelationshipStatus.Hate || dateStatus39 == RelationshipStatus.Love || flag5)
				{
					num += 5;
				}
				RelationshipStatus dateStatus40 = Singleton<Save>.Instance.GetDateStatus("daisuke");
				if (dateStatus40 == RelationshipStatus.Friend || dateStatus40 == RelationshipStatus.Hate || dateStatus40 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus41 = Singleton<Save>.Instance.GetDateStatus("friar");
				if (dateStatus41 == RelationshipStatus.Friend || dateStatus41 == RelationshipStatus.Hate || dateStatus41 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus42 = Singleton<Save>.Instance.GetDateStatus("kopi");
				bool flag6 = (bool)Singleton<InkController>.Instance.story.variablesState["kopi_candy"];
				if (dateStatus42 == RelationshipStatus.Friend || dateStatus42 == RelationshipStatus.Hate || dateStatus42 == RelationshipStatus.Love || flag6)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus43 = Singleton<Save>.Instance.GetDateStatus("cam");
				if (dateStatus43 == RelationshipStatus.Friend || dateStatus43 == RelationshipStatus.Hate || dateStatus43 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus44 = Singleton<Save>.Instance.GetDateStatus("ironaldini");
				if (dateStatus44 == RelationshipStatus.Friend || dateStatus44 == RelationshipStatus.Hate || dateStatus44 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus45 = Singleton<Save>.Instance.GetDateStatus("amir");
				if (dateStatus45 == RelationshipStatus.Friend || dateStatus45 == RelationshipStatus.Hate || dateStatus45 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus46 = Singleton<Save>.Instance.GetDateStatus("jeanloo");
				bool flag7 = (bool)Singleton<InkController>.Instance.story.variablesState["jeanloo_redo"];
				if (dateStatus46 == RelationshipStatus.Friend || dateStatus46 == RelationshipStatus.Hate || dateStatus46 == RelationshipStatus.Love || flag7)
				{
					num += 5;
				}
				RelationshipStatus dateStatus47 = Singleton<Save>.Instance.GetDateStatus("johnny");
				if (dateStatus47 == RelationshipStatus.Friend || dateStatus47 == RelationshipStatus.Hate || dateStatus47 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus48 = Singleton<Save>.Instance.GetDateStatus("bathsheba");
				if (dateStatus48 == RelationshipStatus.Friend || dateStatus48 == RelationshipStatus.Hate || dateStatus48 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus49 = Singleton<Save>.Instance.GetDateStatus("rebel");
				if (dateStatus49 == RelationshipStatus.Friend || dateStatus49 == RelationshipStatus.Hate || dateStatus49 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus50 = Singleton<Save>.Instance.GetDateStatus("barry");
				if (dateStatus50 == RelationshipStatus.Friend || dateStatus50 == RelationshipStatus.Hate || dateStatus50 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus51 = Singleton<Save>.Instance.GetDateStatus("tyrell");
				if (dateStatus51 == RelationshipStatus.Friend || dateStatus51 == RelationshipStatus.Hate || dateStatus51 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus52 = Singleton<Save>.Instance.GetDateStatus("farya");
				if (dateStatus52 == RelationshipStatus.Friend || dateStatus52 == RelationshipStatus.Hate || dateStatus52 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus53 = Singleton<Save>.Instance.GetDateStatus("dasha");
				if (dateStatus53 == RelationshipStatus.Friend || dateStatus53 == RelationshipStatus.Hate || dateStatus53 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus54 = Singleton<Save>.Instance.GetDateStatus("jerry");
				if (dateStatus54 == RelationshipStatus.Friend || dateStatus54 == RelationshipStatus.Hate || dateStatus54 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus55 = Singleton<Save>.Instance.GetDateStatus("penelope");
				if (dateStatus55 == RelationshipStatus.Friend || dateStatus55 == RelationshipStatus.Hate || dateStatus55 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus56 = Singleton<Save>.Instance.GetDateStatus("mac");
				if (dateStatus56 == RelationshipStatus.Friend || dateStatus56 == RelationshipStatus.Hate || dateStatus56 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus57 = Singleton<Save>.Instance.GetDateStatus("willi");
				if (dateStatus57 == RelationshipStatus.Friend || dateStatus57 == RelationshipStatus.Hate || dateStatus57 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus58 = Singleton<Save>.Instance.GetDateStatus("lyric");
				if (dateStatus58 == RelationshipStatus.Friend || dateStatus58 == RelationshipStatus.Hate || dateStatus58 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus59 = Singleton<Save>.Instance.GetDateStatus("rongomaiwhenua");
				if (dateStatus59 == RelationshipStatus.Friend || dateStatus59 == RelationshipStatus.Hate || dateStatus59 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus60 = Singleton<Save>.Instance.GetDateStatus("chance");
				if (dateStatus60 == RelationshipStatus.Friend || dateStatus60 == RelationshipStatus.Hate || dateStatus60 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus61 = Singleton<Save>.Instance.GetDateStatus("maggie");
				if (dateStatus61 == RelationshipStatus.Friend || dateStatus61 == RelationshipStatus.Hate || dateStatus61 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus62 = Singleton<Save>.Instance.GetDateStatus("winnifred");
				if (dateStatus62 == RelationshipStatus.Friend || dateStatus62 == RelationshipStatus.Hate || dateStatus62 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus63 = Singleton<Save>.Instance.GetDateStatus("rainey");
				if (dateStatus63 == RelationshipStatus.Friend || dateStatus63 == RelationshipStatus.Hate || dateStatus63 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus64 = Singleton<Save>.Instance.GetDateStatus("scandalabra");
				bool flag8 = (bool)Singleton<InkController>.Instance.story.variablesState["scandalabra_candy_used"];
				if (dateStatus64 == RelationshipStatus.Friend || dateStatus64 == RelationshipStatus.Hate || dateStatus64 == RelationshipStatus.Love || flag8)
				{
					num += 5;
				}
				RelationshipStatus dateStatus65 = Singleton<Save>.Instance.GetDateStatus("arma");
				bool flag9 = (bool)Singleton<InkController>.Instance.story.variablesState["arma_candy_used"];
				if (dateStatus65 == RelationshipStatus.Friend || dateStatus65 == RelationshipStatus.Hate || dateStatus65 == RelationshipStatus.Love || flag9)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus66 = Singleton<Save>.Instance.GetDateStatus("betty");
				if (dateStatus66 == RelationshipStatus.Friend || dateStatus66 == RelationshipStatus.Hate || dateStatus66 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus67 = Singleton<Save>.Instance.GetDateStatus("diana");
				if (dateStatus67 == RelationshipStatus.Friend || dateStatus67 == RelationshipStatus.Hate || dateStatus67 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus68 = Singleton<Save>.Instance.GetDateStatus("daemon");
				if (dateStatus68 == RelationshipStatus.Friend || dateStatus68 == RelationshipStatus.Hate || dateStatus68 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus69 = Singleton<Save>.Instance.GetDateStatus("teddy");
				bool flag10 = (bool)Singleton<InkController>.Instance.story.variablesState["teddy_candy"];
				if (dateStatus69 == RelationshipStatus.Friend || dateStatus69 == RelationshipStatus.Hate || dateStatus69 == RelationshipStatus.Love || flag10)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus70 = Singleton<Save>.Instance.GetDateStatus("hime");
				if (dateStatus70 == RelationshipStatus.Friend || dateStatus70 == RelationshipStatus.Hate || dateStatus70 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus71 = Singleton<Save>.Instance.GetDateStatus("benhwa");
				if (dateStatus71 == RelationshipStatus.Friend || dateStatus71 == RelationshipStatus.Hate || dateStatus71 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus72 = Singleton<Save>.Instance.GetDateStatus("hanks");
				if (dateStatus72 == RelationshipStatus.Friend || dateStatus72 == RelationshipStatus.Hate || dateStatus72 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus73 = Singleton<Save>.Instance.GetDateStatus("washford");
				if (dateStatus73 == RelationshipStatus.Friend || dateStatus73 == RelationshipStatus.Hate || dateStatus73 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus74 = Singleton<Save>.Instance.GetDateStatus("drysdale");
				if (dateStatus74 == RelationshipStatus.Friend || dateStatus74 == RelationshipStatus.Hate || dateStatus74 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus75 = Singleton<Save>.Instance.GetDateStatus("harper");
				if (dateStatus75 == RelationshipStatus.Friend || dateStatus75 == RelationshipStatus.Hate || dateStatus75 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus76 = Singleton<Save>.Instance.GetDateStatus("dirk");
				if (dateStatus76 == RelationshipStatus.Friend || dateStatus76 == RelationshipStatus.Hate || dateStatus76 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus77 = Singleton<Save>.Instance.GetDateStatus("tydus");
				if (dateStatus77 == RelationshipStatus.Friend || dateStatus77 == RelationshipStatus.Hate || dateStatus77 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus78 = Singleton<Save>.Instance.GetDateStatus("hoove");
				if (dateStatus78 == RelationshipStatus.Friend || dateStatus78 == RelationshipStatus.Hate || dateStatus78 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus79 = Singleton<Save>.Instance.GetDateStatus("bobby");
				if (dateStatus79 == RelationshipStatus.Friend || dateStatus79 == RelationshipStatus.Hate || dateStatus79 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus80 = Singleton<Save>.Instance.GetDateStatus("kristof");
				bool flag11 = (bool)Singleton<InkController>.Instance.story.variablesState["kristof_candy"];
				if (dateStatus80 == RelationshipStatus.Friend || dateStatus80 == RelationshipStatus.Hate || dateStatus80 == RelationshipStatus.Love || flag11)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus81 = Singleton<Save>.Instance.GetDateStatus("dunk");
				if (dateStatus81 == RelationshipStatus.Friend || dateStatus81 == RelationshipStatus.Hate || dateStatus81 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus82 = Singleton<Save>.Instance.GetDateStatus("fantina");
				bool flag12 = (bool)Singleton<InkController>.Instance.story.variablesState["fantina_startover"];
				if (dateStatus82 == RelationshipStatus.Friend || dateStatus82 == RelationshipStatus.Hate || dateStatus82 == RelationshipStatus.Love || flag12)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus83 = Singleton<Save>.Instance.GetDateStatus("stepford");
				if (dateStatus83 == RelationshipStatus.Friend || dateStatus83 == RelationshipStatus.Hate || dateStatus83 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus84 = Singleton<Save>.Instance.GetDateStatus("tony");
				if (dateStatus84 == RelationshipStatus.Friend || dateStatus84 == RelationshipStatus.Hate || dateStatus84 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus85 = Singleton<Save>.Instance.GetDateStatus("beau");
				if (dateStatus85 == RelationshipStatus.Friend || dateStatus85 == RelationshipStatus.Hate || dateStatus85 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus86 = Singleton<Save>.Instance.GetDateStatus("keith");
				if (dateStatus86 == RelationshipStatus.Friend || dateStatus86 == RelationshipStatus.Hate || dateStatus86 == RelationshipStatus.Love)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus87 = Singleton<Save>.Instance.GetDateStatus("bodhi");
				bool flag13 = (bool)Singleton<InkController>.Instance.story.variablesState["bodhi_candy"];
				if (dateStatus87 == RelationshipStatus.Friend || dateStatus87 == RelationshipStatus.Hate || dateStatus87 == RelationshipStatus.Love || flag13)
				{
					num += 5;
				}
				RelationshipStatus dateStatus88 = Singleton<Save>.Instance.GetDateStatus("vaughn");
				bool flag14 = (bool)Singleton<InkController>.Instance.story.variablesState["vaughn_candy"];
				if (dateStatus88 == RelationshipStatus.Friend || dateStatus88 == RelationshipStatus.Hate || dateStatus88 == RelationshipStatus.Love || flag14)
				{
					num += 5;
				}
				RelationshipStatus dateStatus89 = Singleton<Save>.Instance.GetDateStatus("sophia");
				if (dateStatus89 == RelationshipStatus.Friend || dateStatus89 == RelationshipStatus.Hate || dateStatus89 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus90 = Singleton<Save>.Instance.GetDateStatus("monique");
				if (dateStatus90 == RelationshipStatus.Friend || dateStatus90 == RelationshipStatus.Hate || dateStatus90 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus91 = Singleton<Save>.Instance.GetDateStatus("memoria");
				if (dateStatus91 == RelationshipStatus.Friend || dateStatus91 == RelationshipStatus.Hate || dateStatus91 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus92 = Singleton<Save>.Instance.GetDateStatus("holly");
				if (dateStatus92 == RelationshipStatus.Friend || dateStatus92 == RelationshipStatus.Hate || dateStatus92 == RelationshipStatus.Love)
				{
					num4 += 5;
				}
				RelationshipStatus dateStatus93 = Singleton<Save>.Instance.GetDateStatus("airyn");
				if (dateStatus93 == RelationshipStatus.Friend || dateStatus93 == RelationshipStatus.Hate || dateStatus93 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus94 = Singleton<Save>.Instance.GetDateStatus("tbc");
				if (dateStatus94 == RelationshipStatus.Friend || dateStatus94 == RelationshipStatus.Hate || dateStatus94 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus95 = Singleton<Save>.Instance.GetDateStatus("sassy");
				if (dateStatus95 == RelationshipStatus.Friend || dateStatus95 == RelationshipStatus.Hate || dateStatus95 == RelationshipStatus.Love)
				{
					num += 5;
				}
				RelationshipStatus dateStatus96 = Singleton<Save>.Instance.GetDateStatus("zoey");
				bool flag15 = (bool)Singleton<InkController>.Instance.story.variablesState["zoey_second_seance"];
				if (dateStatus96 == RelationshipStatus.Friend || dateStatus96 == RelationshipStatus.Hate || dateStatus96 == RelationshipStatus.Love || flag15)
				{
					num3 += 5;
				}
				RelationshipStatus dateStatus97 = Singleton<Save>.Instance.GetDateStatus("shadow");
				if (dateStatus97 == RelationshipStatus.Friend || dateStatus97 == RelationshipStatus.Hate || dateStatus97 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus98 = Singleton<Save>.Instance.GetDateStatus("doug");
				if (dateStatus98 == RelationshipStatus.Friend || dateStatus98 == RelationshipStatus.Hate || dateStatus98 == RelationshipStatus.Love)
				{
					num5 += 5;
				}
				RelationshipStatus dateStatus99 = Singleton<Save>.Instance.GetDateStatus("nightmare");
				if (dateStatus99 == RelationshipStatus.Friend || dateStatus99 == RelationshipStatus.Hate || dateStatus99 == RelationshipStatus.Love)
				{
					num2 += 5;
				}
				RelationshipStatus dateStatus100 = Singleton<Save>.Instance.GetDateStatus("reggie");
				if (dateStatus100 == RelationshipStatus.Friend || dateStatus100 == RelationshipStatus.Hate || dateStatus100 == RelationshipStatus.Love)
				{
					num += 5;
				}
				if (Singleton<DeluxeEditionController>.Instance.IS_DELUXE_EDITION)
				{
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					int num9 = 0;
					int num10 = 0;
					int num11 = (int)Singleton<InkController>.Instance.story.variablesState["armors"];
					bool flag16 = (bool)Singleton<InkController>.Instance.story.variablesState["shop_closed"];
					int num12 = (int)Singleton<InkController>.Instance.story.variablesState["armors"];
					if (flag16)
					{
						num6 = 1000;
						num7 = 1000;
						num8 = 1000;
						num9 = 1000;
						num10 = 1000;
					}
					else if (num11 > 0 && !flag16)
					{
						num6 = 10;
						num7 = 10;
						num8 = 10;
						num9 = 10;
						num10 = 10;
					}
					if (!flag16 && num12 > 0)
					{
						num6++;
						num7++;
						num8++;
						num9++;
						num10++;
					}
					num += num6;
					num2 += num7;
					num3 += num8;
					num4 += num9;
					num5 += num10;
					Singleton<InkController>.Instance.story.variablesState["sass_dlc"] = num6;
					Singleton<InkController>.Instance.story.variablesState["poise_dlc"] = num7;
					Singleton<InkController>.Instance.story.variablesState["empathy_dlc"] = num8;
					Singleton<InkController>.Instance.story.variablesState["charm_dlc"] = num9;
					Singleton<InkController>.Instance.story.variablesState["smarts_dlc"] = num10;
				}
				if (num > 100)
				{
					num = 100;
				}
				if (num2 > 100)
				{
					num2 = 100;
				}
				if (num3 > 100)
				{
					num3 = 100;
				}
				if (num4 > 100)
				{
					num4 = 100;
				}
				if (num5 > 100)
				{
					num5 = 100;
				}
				Singleton<InkController>.Instance.story.variablesState["sass"] = num;
				Singleton<InkController>.Instance.story.variablesState["poise"] = num2;
				Singleton<InkController>.Instance.story.variablesState["empathy"] = num3;
				Singleton<InkController>.Instance.story.variablesState["charm"] = num4;
				Singleton<InkController>.Instance.story.variablesState["smarts"] = num5;
				if ((string)Singleton<InkController>.Instance.story.variablesState["bobby_location"] == "attic")
				{
					MovingDateable movingDateable = MovingDateable.GetMovingDateable("MovingSafe");
					bool flag17 = false;
					if (movingDateable != null)
					{
						foreach (MovingObject movingObject in movingDateable.Locations)
						{
							if (movingObject.Object.activeInHierarchy && movingObject.Key == "open")
							{
								flag17 = true;
							}
						}
					}
					if (flag17)
					{
						MovingDateable.MoveDateable("MovingBobbyPin", "atticOpened", true);
					}
					else
					{
						MovingDateable.MoveDateable("MovingBobbyPin", "atticClosed", true);
					}
				}
				if ((int)Singleton<InkController>.Instance.story.variablesState["rebel_story"] > 3)
				{
					Singleton<InkController>.Instance.story.variablesState["rebel_story"] = 3;
				}
				if (Singleton<InkController>.Instance.story.variablesState["trap_door_opened"] == "true")
				{
					Singleton<InkController>.Instance.story.variablesState["trap_door_opened"] = true;
				}
				Singleton<InkController>.Instance.story.variablesState["realized"] = Singleton<Save>.Instance.AvailableTotalRealizedDatables();
				if (dateStatus64 == RelationshipStatus.Hate || dateStatus64 == RelationshipStatus.Friend || dateStatus64 == RelationshipStatus.Love)
				{
					Singleton<InkController>.Instance.story.variablesState["scandal_specs_given"] = true;
				}
			}
			if (!Singleton<Save>.Instance.GetTutorialThresholdState(SaveAutoFixManager.HOTFIX_2_1_APPLIED))
			{
				Singleton<Save>.Instance.SetTutorialThresholdState(SaveAutoFixManager.HOTFIX_2_1_APPLIED);
				RelationshipStatus dateStatus101 = Singleton<Save>.Instance.GetDateStatus("scandalabra");
				if (dateStatus101 == RelationshipStatus.Unmet || dateStatus101 == RelationshipStatus.Single)
				{
					Singleton<InkController>.Instance.story.variablesState["stayjon"] = false;
				}
			}
		}

		// Token: 0x04000F50 RID: 3920
		public static string HOTFIX_2_APPLIED = "Hotfix2Applied";

		// Token: 0x04000F51 RID: 3921
		public static string HOTFIX_2_1_APPLIED = "Hotfix2.1Applied";
	}
}
