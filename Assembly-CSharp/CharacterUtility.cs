using System;
using System.IO;
using T17.Services;
using UnityEngine;

// Token: 0x020000C5 RID: 197
[CreateAssetMenu(fileName = "CharacterUtility", menuName = "ScriptableObjects/CharacterUtility", order = 12)]
[Serializable]
public class CharacterUtility : ScriptableObject
{
	// Token: 0x06000698 RID: 1688 RVA: 0x0002461D File Offset: 0x0002281D
	public CharacterUtility()
	{
		this.internalName = "";
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00024630 File Offset: 0x00022830
	public Sprite GetSpriteFromCustomExpression(string expression)
	{
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(expression, false);
		if (sprite == null)
		{
			return this.GetSpriteFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.neutral, false, false);
		}
		return sprite;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x00024660 File Offset: 0x00022860
	public static Sprite GetSpriteFileFromPoseExpression(E_General_Poses pose, E_Facial_Expressions expression, string name, bool isForDateADex)
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized(name) == RelationshipStatus.Realized)
		{
			if (pose == E_General_Poses.clarence)
			{
				pose = E_General_Poses.clarence_realized;
			}
			else if (pose == E_General_Poses.turtle)
			{
				pose = E_General_Poses.turtle;
			}
			else if (pose != E_General_Poses.wick && pose != E_General_Poses.timmy)
			{
				pose = E_General_Poses.realized;
			}
			if (name == "dirk" && Singleton<InkController>.Instance.GetVariable("harper_dirk") == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
			{
				pose = E_General_Poses.clarence_realized;
			}
		}
		if (isForDateADex)
		{
			if (name == "zoey" && Singleton<Save>.Instance.GetDateStatus(name) == RelationshipStatus.Unmet && !expression.ToString().EndsWith("_b"))
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Reactions", name, "zoey_white"), false);
			}
			if (name == "airyn" && Singleton<Save>.Instance.GetDateStatus(name) == RelationshipStatus.Unmet && !expression.ToString().EndsWith("_b"))
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Reactions", name, "airyn_white"), false);
			}
			if (name == "curtrod" || name == "curt" || name == "rod")
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
				{
					"Images",
					"Reactions",
					"curtrod",
					pose.ToString(),
					CharacterUtility.NamePoseExpressionToFilename("curtrod", pose, expression)
				}), false);
			}
			if (name == "eddie" || name == "eddievolt" || name == "volt")
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
				{
					"Images",
					"Reactions",
					"eddievolt",
					pose.ToString(),
					CharacterUtility.NamePoseExpressionToFilename("eddievolt", pose, expression)
				}), false);
			}
		}
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
		{
			"Images",
			"Reactions",
			name,
			pose.ToString(),
			CharacterUtility.NamePoseExpressionToFilename(name, pose, expression)
		}), false);
		if (sprite == null)
		{
			sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Reactions", name, CharacterUtility.NamePoseExpressionToFilename(name, pose, expression)), false);
		}
		return sprite;
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x000248F4 File Offset: 0x00022AF4
	public Sprite GetSpriteFileFromPoseExpression(E_General_Poses pose, E_Facial_Expressions expression, bool isForDateADex = false, bool returnNullIfDontExist = false)
	{
		if (Singleton<Save>.Instance.GetDateStatusRealized(this.internalName) == RelationshipStatus.Realized)
		{
			if (this.internalName == "dirk" && pose == E_General_Poses.clarence)
			{
				pose = E_General_Poses.clarence_realized;
			}
			else if (this.internalName == "sinclaire" && pose == E_General_Poses.turtle)
			{
				pose = E_General_Poses.turtle;
			}
			else if (pose != E_General_Poses.wick && pose != E_General_Poses.timmy)
			{
				pose = E_General_Poses.realized;
			}
			if (this.internalName == "dirk" && Singleton<InkController>.Instance.GetVariable("harper_dirk") == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
			{
				pose = E_General_Poses.clarence_realized;
			}
		}
		if (isForDateADex)
		{
			if (base.name == "dirk" && Singleton<InkController>.Instance.story.variablesState["harper_dirk"].ToString() == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
				{
					"Images",
					"Reactions",
					"clarence",
					pose.ToString(),
					CharacterUtility.NamePoseExpressionToFilename("clarence", pose, expression)
				}), false);
			}
			if (base.name == "stefan" && Singleton<Save>.Instance.GetDateStatusRealized(this.internalName) == RelationshipStatus.Realized && !expression.ToString().EndsWith("_b"))
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
				{
					"Images",
					"Reactions",
					base.name,
					pose.ToString(),
					CharacterUtility.NamePoseExpressionToFilename(base.name, pose, E_Facial_Expressions.dex)
				}), false);
			}
			if (base.name == "zoey" && Singleton<Save>.Instance.GetDateStatus(this.internalName) == RelationshipStatus.Unmet && !expression.ToString().EndsWith("_b"))
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Reactions", base.name, "zoey_white"), false);
			}
			if (base.name == "airyn" && Singleton<Save>.Instance.GetDateStatus(this.internalName) == RelationshipStatus.Unmet && !expression.ToString().EndsWith("_b"))
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Reactions", base.name, "airyn_white"), false);
			}
			if (base.name == "curtrod" || base.name == "curt" || base.name == "rod")
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
				{
					"Images",
					"Reactions",
					"curtrod",
					pose.ToString(),
					CharacterUtility.NamePoseExpressionToFilename("curtrod", pose, expression)
				}), false);
			}
			if (base.name == "eddie" || base.name == "eddievolt" || base.name == "volt")
			{
				return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
				{
					"Images",
					"Reactions",
					"eddievolt",
					pose.ToString(),
					CharacterUtility.NamePoseExpressionToFilename("eddievolt", pose, expression)
				}), false);
			}
		}
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine(new string[]
		{
			"Images",
			"Reactions",
			base.name,
			pose.ToString(),
			CharacterUtility.NamePoseExpressionToFilename(base.name, pose, expression)
		}), false);
		if (sprite == null && !returnNullIfDontExist)
		{
			sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Reactions", base.name, CharacterUtility.NamePoseExpressionToFilename(base.name, pose, expression)), false);
		}
		return sprite;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00024D47 File Offset: 0x00022F47
	public static void ReturnCharacterSprite(Sprite sprite)
	{
		Services.AssetProviderService.UnloadResourceAsset(sprite);
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00024D54 File Offset: 0x00022F54
	public Sprite GetSpriteFromPoseExpression(E_General_Poses pose, E_Facial_Expressions expression, bool returnNullIfDontExist = false, bool isForDateADex = false)
	{
		Sprite sprite = this.GetSpriteFileFromPoseExpression(pose, expression, isForDateADex, returnNullIfDontExist);
		if (sprite == null && returnNullIfDontExist)
		{
			return null;
		}
		if (sprite == null)
		{
			if (expression == E_Facial_Expressions.neutral && pose == E_General_Poses.neutral)
			{
				sprite = this.errorSprite;
			}
			else
			{
				sprite = this.GetSpriteFileFromPoseExpression(pose, E_Facial_Expressions.neutral, false, false);
				if (sprite == null)
				{
					sprite = this.GetSpriteFileFromPoseExpression(E_General_Poses.neutral, E_Facial_Expressions.neutral, false, false);
					if (sprite == null)
					{
						sprite = this.errorSprite;
					}
				}
			}
		}
		return sprite;
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00024DC2 File Offset: 0x00022FC2
	public Sprite GetSpriteFromCollectableNum(string name, int collectableNum)
	{
		return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Collectables", name, name + "_" + collectableNum.ToString()), false);
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00024DF1 File Offset: 0x00022FF1
	public static string NamePoseExpressionToFilename(string internalName, E_General_Poses pose, E_Facial_Expressions expression)
	{
		return internalName + "_" + CharacterUtility.ShortenedPose(pose) + CharacterUtility.ShortenedExpression(expression);
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00024E0C File Offset: 0x0002300C
	public static string ShortenedExpression(E_Facial_Expressions expression)
	{
		string text;
		switch (expression)
		{
		case E_Facial_Expressions.neutral:
		case E_Facial_Expressions.realized:
			text = "";
			break;
		case E_Facial_Expressions.neutral_b:
		case E_Facial_Expressions.realized_b:
			text = "_b";
			break;
		case E_Facial_Expressions.angry:
			text = "angry";
			break;
		case E_Facial_Expressions.angry_b:
			text = "angry_b";
			break;
		case E_Facial_Expressions.blush:
			text = "blush";
			break;
		case E_Facial_Expressions.blush_b:
			text = "blush_b";
			break;
		case E_Facial_Expressions.flirt:
			text = "flirt";
			break;
		case E_Facial_Expressions.flirt_b:
			text = "flirt_b";
			break;
		case E_Facial_Expressions.happy:
			text = "happy";
			break;
		case E_Facial_Expressions.happy_b:
			text = "happy_b";
			break;
		case E_Facial_Expressions.joy:
			text = "joy";
			break;
		case E_Facial_Expressions.joy_b:
			text = "joy_b";
			break;
		case E_Facial_Expressions.shock:
			text = "shock";
			break;
		case E_Facial_Expressions.shock_b:
			text = "shock_b";
			break;
		case E_Facial_Expressions.sad:
			text = "sad";
			break;
		case E_Facial_Expressions.sad_b:
			text = "sad_b";
			break;
		case E_Facial_Expressions.smirk:
			text = "smirk";
			break;
		case E_Facial_Expressions.smirk_b:
			text = "smirk_b";
			break;
		case E_Facial_Expressions.think:
		case E_Facial_Expressions.think_b:
			text = "think";
			break;
		case E_Facial_Expressions.tsundere:
		case E_Facial_Expressions.tsun:
			text = "tsun";
			break;
		case E_Facial_Expressions.tsundere_b:
		case E_Facial_Expressions.tsun_b:
			text = "tsun_b";
			break;
		case E_Facial_Expressions.custom_trap:
			text = "trap";
			break;
		case E_Facial_Expressions.custom_trap_b:
			text = "trap_b";
			break;
		case E_Facial_Expressions.custom_wink:
			text = "wink";
			break;
		case E_Facial_Expressions.custom_wink_b:
			text = "wink_b";
			break;
		case E_Facial_Expressions.omega:
			text = "omega";
			break;
		case E_Facial_Expressions.custom_omega:
			text = "omega";
			break;
		case E_Facial_Expressions.custom_omega_b:
			text = "omega_b";
			break;
		case E_Facial_Expressions.custom_human:
			text = "human";
			break;
		case E_Facial_Expressions.custom_human_b:
			text = "human_b";
			break;
		case E_Facial_Expressions.custom_back:
			text = "back";
			break;
		case E_Facial_Expressions.custom_capsule:
			text = "capsule";
			break;
		case E_Facial_Expressions.custom_capsule_b:
			text = "capsule_b";
			break;
		case E_Facial_Expressions.custom_casual:
			text = "casual";
			break;
		case E_Facial_Expressions.custom_casual_b:
			text = "casual_b";
			break;
		case E_Facial_Expressions.custom_cloak:
			text = "cloak";
			break;
		case E_Facial_Expressions.custom_cloak_b:
			text = "cloak_b";
			break;
		case E_Facial_Expressions.custom_front:
			text = "front";
			break;
		case E_Facial_Expressions.custom_front_b:
			text = "front_b";
			break;
		case E_Facial_Expressions.custom_full:
			text = "full";
			break;
		case E_Facial_Expressions.custom_full_b:
			text = "full_b";
			break;
		case E_Facial_Expressions.custom_helm:
			text = "helm";
			break;
		case E_Facial_Expressions.custom_helm_b:
			text = "helm_b";
			break;
		case E_Facial_Expressions.custom_helm1:
			text = "helm1";
			break;
		case E_Facial_Expressions.custom_helm1_b:
			text = "helm1_b";
			break;
		case E_Facial_Expressions.custom_helm2:
			text = "helm2";
			break;
		case E_Facial_Expressions.custom_helm2_b:
			text = "helm2_b";
			break;
		case E_Facial_Expressions.custom_helm3:
			text = "helm3";
			break;
		case E_Facial_Expressions.custom_helm3_b:
			text = "helm3_b";
			break;
		case E_Facial_Expressions.custom_helm4:
			text = "helm4";
			break;
		case E_Facial_Expressions.custom_helm4_b:
			text = "helm4_b";
			break;
		case E_Facial_Expressions.custom_helm5:
			text = "helm5";
			break;
		case E_Facial_Expressions.custom_helm5_b:
			text = "helm5_b";
			break;
		case E_Facial_Expressions.custom_helmangry:
			text = "helmangry";
			break;
		case E_Facial_Expressions.custom_helmhappy:
			text = "helmhappy";
			break;
		case E_Facial_Expressions.custom_helmsad:
			text = "helmsad";
			break;
		case E_Facial_Expressions.custom_sans:
			text = "sans";
			break;
		case E_Facial_Expressions.custom_sans_b:
			text = "sans_b";
			break;
		case E_Facial_Expressions.custom_volt:
			text = "volt";
			break;
		case E_Facial_Expressions.custom_volt_b:
			text = "volt_b";
			break;
		case E_Facial_Expressions.custom_mon:
			text = "mon";
			break;
		case E_Facial_Expressions.custom_mon_b:
			text = "mon_b";
			break;
		case E_Facial_Expressions.custom_tue:
			text = "tue";
			break;
		case E_Facial_Expressions.custom_tue_b:
			text = "tue_b";
			break;
		case E_Facial_Expressions.custom_wed:
			text = "wed";
			break;
		case E_Facial_Expressions.custom_wed_b:
			text = "wed_b";
			break;
		case E_Facial_Expressions.custom_thu:
			text = "thu";
			break;
		case E_Facial_Expressions.custom_thu_b:
			text = "thu_b";
			break;
		case E_Facial_Expressions.custom_fri:
			text = "fri";
			break;
		case E_Facial_Expressions.custom_fri_b:
			text = "fri_b";
			break;
		case E_Facial_Expressions.custom_sat:
			text = "sat";
			break;
		case E_Facial_Expressions.custom_sat_b:
			text = "sat_b";
			break;
		case E_Facial_Expressions.custom_blushlookaway:
			text = "blushlookaway";
			break;
		case E_Facial_Expressions.custom_blushlookaway_b:
			text = "blushlookaway_b";
			break;
		case E_Facial_Expressions.anger:
			text = "anger";
			break;
		case E_Facial_Expressions.anger_b:
			text = "anger_b";
			break;
		case E_Facial_Expressions.beast:
			text = "beast";
			break;
		case E_Facial_Expressions.beast_b:
			text = "beast_b";
			break;
		case E_Facial_Expressions.capsule:
			text = "capsule";
			break;
		case E_Facial_Expressions.capsule_b:
			text = "capsule_b";
			break;
		case E_Facial_Expressions.crossangry:
			text = "crossangry";
			break;
		case E_Facial_Expressions.crossangry_b:
			text = "crossangry_b";
			break;
		case E_Facial_Expressions.crossflirt:
			text = "crossflirt";
			break;
		case E_Facial_Expressions.crossflirt_b:
			text = "crossflirt_b";
			break;
		case E_Facial_Expressions.crosssad:
			text = "crosssad";
			break;
		case E_Facial_Expressions.crosssad_b:
			text = "crosssad_b";
			break;
		case E_Facial_Expressions.crosstsun:
			text = "crosstsun";
			break;
		case E_Facial_Expressions.crosstsun_b:
			text = "crosstsun_b";
			break;
		case E_Facial_Expressions.crosstsundere:
			text = "crosstsun";
			break;
		case E_Facial_Expressions.crosstsundere_b:
			text = "crosstsun_b";
			break;
		case E_Facial_Expressions.death:
			text = "death";
			break;
		case E_Facial_Expressions.death_b:
			text = "death_b";
			break;
		case E_Facial_Expressions.custom_death:
			text = "death";
			break;
		case E_Facial_Expressions.custom_death_b:
			text = "death_b";
			break;
		case E_Facial_Expressions.custom_rage:
			text = "rage";
			break;
		case E_Facial_Expressions.custom_rage_b:
			text = "rage_b";
			break;
		case E_Facial_Expressions.rage:
			text = "rage";
			break;
		case E_Facial_Expressions.rage_b:
			text = "rage_b";
			break;
		case E_Facial_Expressions.cry:
			text = "cry";
			break;
		case E_Facial_Expressions.cry_b:
			text = "cry_b";
			break;
		case E_Facial_Expressions.happycry:
			text = "happycry";
			break;
		case E_Facial_Expressions.happycry_b:
			text = "happycry_b";
			break;
		case E_Facial_Expressions.moon:
			text = "moon";
			break;
		case E_Facial_Expressions.moon_b:
			text = "moon_b";
			break;
		case E_Facial_Expressions.custom_moon:
			text = "moon";
			break;
		case E_Facial_Expressions.custom_pout:
			text = "pout";
			break;
		case E_Facial_Expressions.custom_pout_b:
			text = "pout_b";
			break;
		case E_Facial_Expressions.pout:
			text = "pout";
			break;
		case E_Facial_Expressions.pout_b:
			text = "pout_b";
			break;
		case E_Facial_Expressions.shout:
			text = "shout";
			break;
		case E_Facial_Expressions.shout_b:
			text = "shout_b";
			break;
		case E_Facial_Expressions.custom_laugh:
			text = "laugh";
			break;
		case E_Facial_Expressions.custom_laugh_b:
			text = "laugh_b";
			break;
		case E_Facial_Expressions.custom_rap1:
			text = "rap1";
			break;
		case E_Facial_Expressions.custom_rap2:
			text = "rap2";
			break;
		case E_Facial_Expressions.custom_rap3:
			text = "rap3";
			break;
		case E_Facial_Expressions.custom_heart:
			text = "heart";
			break;
		case E_Facial_Expressions.custom_glasses:
			text = "glasses";
			break;
		case E_Facial_Expressions.glasses:
			text = "glasses";
			break;
		case E_Facial_Expressions.custom_canister:
			text = "canister";
			break;
		case E_Facial_Expressions.custom_shoe1:
			text = "shoe1";
			break;
		case E_Facial_Expressions.custom_shoe2:
			text = "shoe2";
			break;
		case E_Facial_Expressions.custom_read:
			text = "read";
			break;
		case E_Facial_Expressions.custom_nude:
			text = "nude";
			break;
		case E_Facial_Expressions.custom_smile:
			text = "smile";
			break;
		case E_Facial_Expressions.custom_broken:
			text = "broken";
			break;
		case E_Facial_Expressions.sing:
			text = "sing";
			break;
		case E_Facial_Expressions.custom_ice:
			text = "ice";
			break;
		case E_Facial_Expressions.custom_spin:
			text = "spin";
			break;
		case E_Facial_Expressions.custom_mess:
			text = "mess";
			break;
		case E_Facial_Expressions.custom_puzzled_1:
			text = "puzzled1";
			break;
		case E_Facial_Expressions.custom_puzzled1:
			text = "puzzled1";
			break;
		case E_Facial_Expressions.custom_puzzled_2:
			text = "puzzled2";
			break;
		case E_Facial_Expressions.custom_puzzled2:
			text = "puzzled2";
			break;
		case E_Facial_Expressions.rock:
			text = "rock";
			break;
		case E_Facial_Expressions.custom_rock:
			text = "rock";
			break;
		case E_Facial_Expressions.custom_rockjoy:
			text = "rockjoy";
			break;
		case E_Facial_Expressions.custom_cluck:
			text = "cluck";
			break;
		case E_Facial_Expressions.custom_chirp:
			text = "chirp";
			break;
		case E_Facial_Expressions.custom_angrychirp:
			text = "angrychirp";
			break;
		case E_Facial_Expressions.custom_angry:
			text = "angry";
			break;
		case E_Facial_Expressions.arf:
			text = "arf";
			break;
		case E_Facial_Expressions.custom_arf:
			text = "arf";
			break;
		case E_Facial_Expressions.bark:
			text = "bark";
			break;
		case E_Facial_Expressions.custom_bark:
			text = "bark";
			break;
		case E_Facial_Expressions.concern:
			text = "concern";
			break;
		case E_Facial_Expressions.custom_concern:
			text = "concern";
			break;
		case E_Facial_Expressions.froth:
			text = "froth";
			break;
		case E_Facial_Expressions.custom_froth:
			text = "froth";
			break;
		case E_Facial_Expressions.growl:
			text = "growl";
			break;
		case E_Facial_Expressions.custom_growl:
			text = "growl";
			break;
		case E_Facial_Expressions.treat:
			text = "treat";
			break;
		case E_Facial_Expressions.custom_treat:
			text = "treat";
			break;
		case E_Facial_Expressions.custom_harper1:
			text = "harper1";
			break;
		case E_Facial_Expressions.custom_harper2:
			text = "harper2";
			break;
		case E_Facial_Expressions.custom_teddy1:
			text = "teddy1";
			break;
		case E_Facial_Expressions.custom_teddy2:
			text = "teddy2";
			break;
		case E_Facial_Expressions.custom_teddy3:
			text = "teddy3";
			break;
		case E_Facial_Expressions.custom_teddy4:
			text = "teddy4";
			break;
		case E_Facial_Expressions.custom_rap1_b:
			text = "rap1_b";
			break;
		case E_Facial_Expressions.custom_rap2_b:
			text = "rap2_b";
			break;
		case E_Facial_Expressions.custom_rap3_b:
			text = "rap3_b";
			break;
		case E_Facial_Expressions.custom_heart_b:
			text = "heart_b";
			break;
		case E_Facial_Expressions.custom_glasses_b:
			text = "glasses_b";
			break;
		case E_Facial_Expressions.custom_canister_b:
			text = "canister_b";
			break;
		case E_Facial_Expressions.custom_shoe1_b:
			text = "shoe1_b";
			break;
		case E_Facial_Expressions.custom_shoe2_b:
			text = "shoe2_b";
			break;
		case E_Facial_Expressions.custom_read_b:
			text = "read_b";
			break;
		case E_Facial_Expressions.custom_nude_b:
			text = "nude_b";
			break;
		case E_Facial_Expressions.custom_smile_b:
			text = "smile_b";
			break;
		case E_Facial_Expressions.custom_broken_b:
			text = "broken_b";
			break;
		case E_Facial_Expressions.sing_b:
			text = "sing_b";
			break;
		case E_Facial_Expressions.custom_ice_b:
			text = "ice_b";
			break;
		case E_Facial_Expressions.custom_spin_b:
			text = "spin_b";
			break;
		case E_Facial_Expressions.custom_mess_b:
			text = "mess_b";
			break;
		case E_Facial_Expressions.custom_puzzled_1_b:
			text = "puzzled1_b";
			break;
		case E_Facial_Expressions.custom_puzzled_2_b:
			text = "puzzled2_b";
			break;
		case E_Facial_Expressions.custom_rock_b:
			text = "rock_b";
			break;
		case E_Facial_Expressions.custom_rockjoy_b:
			text = "rockjoy_b";
			break;
		case E_Facial_Expressions.custom_cluck_b:
			text = "cluck_b";
			break;
		case E_Facial_Expressions.custom_chirp_b:
			text = "chirp_b";
			break;
		case E_Facial_Expressions.custom_angrychirp_b:
			text = "angrychirp_b";
			break;
		case E_Facial_Expressions.custom_angry_b:
			text = "angry_b";
			break;
		case E_Facial_Expressions.custom_arf_b:
			text = "arf_b";
			break;
		case E_Facial_Expressions.custom_bark_b:
			text = "bark_b";
			break;
		case E_Facial_Expressions.custom_concern_b:
			text = "concern_b";
			break;
		case E_Facial_Expressions.custom_froth_b:
			text = "froth_b";
			break;
		case E_Facial_Expressions.custom_growl_b:
			text = "growl_b";
			break;
		case E_Facial_Expressions.custom_treat_b:
			text = "treat_b";
			break;
		case E_Facial_Expressions.custom_harper1_b:
			text = "harper1_b";
			break;
		case E_Facial_Expressions.custom_harper2_b:
			text = "harper2_b";
			break;
		case E_Facial_Expressions.custom_teddy1_b:
			text = "teddy1_b";
			break;
		case E_Facial_Expressions.custom_teddy2_b:
			text = "teddy2_b";
			break;
		case E_Facial_Expressions.custom_teddy3_b:
			text = "teddy3_b";
			break;
		case E_Facial_Expressions.custom_teddy4_b:
			text = "teddy4_b";
			break;
		case E_Facial_Expressions.neutral_blink1:
			text = "blink1";
			break;
		case E_Facial_Expressions.neutral_blink2:
			text = "blink2";
			break;
		case E_Facial_Expressions.knop:
			text = "knop";
			break;
		case E_Facial_Expressions.helmet:
			text = "helmet";
			break;
		case E_Facial_Expressions.hurt:
			text = "hurt";
			break;
		case E_Facial_Expressions.pinch:
			text = "pinch";
			break;
		case E_Facial_Expressions.pinch_b:
			text = "pinch_b";
			break;
		case E_Facial_Expressions.wink:
			text = "wink";
			break;
		case E_Facial_Expressions.mid:
			text = "mid";
			break;
		case E_Facial_Expressions.custom_mid:
			text = "mid";
			break;
		case E_Facial_Expressions.wake:
			text = "wake";
			break;
		case E_Facial_Expressions.custom_wake:
			text = "wake";
			break;
		case E_Facial_Expressions.custom_yum:
			text = "yum";
			break;
		case E_Facial_Expressions.yum:
			text = "yum";
			break;
		case E_Facial_Expressions.spin:
			text = "spin";
			break;
		case E_Facial_Expressions.dex:
			text = "dex";
			break;
		default:
			text = "";
			break;
		}
		return text;
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00025A74 File Offset: 0x00023C74
	public static string ShortenedPose(E_General_Poses pose)
	{
		string text;
		switch (pose)
		{
		case E_General_Poses.neutral:
			text = "n";
			break;
		case E_General_Poses.friend:
			text = "f";
			break;
		case E_General_Poses.love:
			text = "l";
			break;
		case E_General_Poses.hate:
			text = "h";
			break;
		case E_General_Poses.realized:
			text = "r";
			break;
		case E_General_Poses.back:
			text = "back";
			break;
		case E_General_Poses.front:
			text = "front";
			break;
		case E_General_Poses.tiny:
			text = "tiny";
			break;
		case E_General_Poses.trap:
			text = "trap";
			break;
		case E_General_Poses.skips:
			text = "s";
			break;
		case E_General_Poses.tue:
			text = "tue";
			break;
		case E_General_Poses.wed:
			text = "wed";
			break;
		case E_General_Poses.thu:
			text = "thu";
			break;
		case E_General_Poses.fri:
			text = "fri";
			break;
		case E_General_Poses.sat:
			text = "sat";
			break;
		case E_General_Poses.tux:
			text = "tux";
			break;
		case E_General_Poses.full:
			text = "full";
			break;
		case E_General_Poses.wail:
			text = "wail";
			break;
		case E_General_Poses.dust:
			text = "d";
			break;
		case E_General_Poses.boss:
			text = "boss";
			break;
		case E_General_Poses.boss2:
			text = "boss2";
			break;
		case E_General_Poses.martin:
			text = "m";
			break;
		case E_General_Poses.turtle:
			text = "t";
			break;
		case E_General_Poses.sulk:
			text = "s";
			break;
		case E_General_Poses.feral:
			text = "feral";
			break;
		case E_General_Poses.nya:
			text = "nya";
			break;
		case E_General_Poses.timmy:
			text = "rt";
			break;
		case E_General_Poses.fire:
			text = "fire";
			break;
		case E_General_Poses.evil:
			text = "evil";
			break;
		case E_General_Poses.cloak:
			text = "c";
			break;
		case E_General_Poses.sly:
			text = "sly";
			break;
		case E_General_Poses.clarence:
			text = "n";
			break;
		case E_General_Poses.cluckles:
			text = "n";
			break;
		case E_General_Poses.wick:
			text = "w";
			break;
		case E_General_Poses.nude:
			text = "nude";
			break;
		case E_General_Poses.sleep:
			text = "sleep";
			break;
		case E_General_Poses.aha:
			text = "aha";
			break;
		case E_General_Poses.clarence_realized:
			text = "r";
			break;
		case E_General_Poses.lust:
			text = "lust";
			break;
		case E_General_Poses.capsule:
			text = "c";
			break;
		case E_General_Poses.love2:
			text = "l2";
			break;
		case E_General_Poses.clarence_friend:
			text = "f";
			break;
		case E_General_Poses.clarence_hate:
			text = "h";
			break;
		case E_General_Poses.clarence_love:
			text = "l";
			break;
		case E_General_Poses.clarence_love2:
			text = "l2";
			break;
		case E_General_Poses.folded:
			text = "folded";
			break;
		case E_General_Poses.gun:
			text = "gun";
			break;
		case E_General_Poses.selene:
			text = "selene";
			break;
		case E_General_Poses.secret:
			text = "secret";
			break;
		case E_General_Poses.annoy:
			text = "annoy";
			break;
		default:
			text = "n";
			break;
		}
		return text;
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00025D58 File Offset: 0x00023F58
	public E_General_Poses ShortenedPoseToEnum(string check)
	{
		if (check == "f")
		{
			return E_General_Poses.friend;
		}
		if (check == "l")
		{
			return E_General_Poses.love;
		}
		if (check == "h")
		{
			return E_General_Poses.hate;
		}
		if (!(check == "r"))
		{
			if (!(check == "n"))
			{
			}
			return E_General_Poses.neutral;
		}
		return E_General_Poses.realized;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00025DB4 File Offset: 0x00023FB4
	public E_Facial_Expressions ShortenedExpressionToEnum(string expression, bool isBlinking)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(expression);
		E_Facial_Expressions e_Facial_Expressions;
		if (num <= 2329051074U)
		{
			if (num <= 1406826723U)
			{
				if (num <= 581173629U)
				{
					if (num <= 229146611U)
					{
						if (num <= 134834832U)
						{
							if (num != 58733422U)
							{
								if (num != 94706083U)
								{
									if (num == 134834832U)
									{
										if (expression == "crosstsun_b")
										{
											return E_Facial_Expressions.crosstsundere_b;
										}
									}
								}
								else if (expression == "tsun")
								{
									e_Facial_Expressions = E_Facial_Expressions.tsundere;
									if (isBlinking)
									{
										return E_Facial_Expressions.tsundere_b;
									}
									return e_Facial_Expressions;
								}
							}
							else if (expression == "angrychirp")
							{
								return E_Facial_Expressions.custom_angrychirp;
							}
						}
						else if (num <= 152362934U)
						{
							if (num != 152360985U)
							{
								if (num == 152362934U)
								{
									if (expression == "wink")
									{
										return E_Facial_Expressions.custom_wink;
									}
								}
							}
							else if (expression == "spin")
							{
								return E_Facial_Expressions.custom_spin;
							}
						}
						else if (num != 170268548U)
						{
							if (num == 229146611U)
							{
								if (expression == "cloak")
								{
									return E_Facial_Expressions.custom_cloak;
								}
							}
						}
						else if (expression == "casual")
						{
							return E_Facial_Expressions.custom_casual;
						}
					}
					else if (num <= 366108702U)
					{
						if (num != 246031843U)
						{
							if (num != 299483063U)
							{
								if (num == 366108702U)
								{
									if (expression == "sing")
									{
										return E_Facial_Expressions.sing;
									}
								}
							}
							else if (expression == "wed")
							{
								return E_Facial_Expressions.custom_wed;
							}
						}
						else if (expression == "heart")
						{
							return E_Facial_Expressions.custom_heart;
						}
					}
					else if (num <= 493975929U)
					{
						if (num != 484751236U)
						{
							if (num == 493975929U)
							{
								if (expression == "helm")
								{
									return E_Facial_Expressions.custom_helm;
								}
							}
						}
						else if (expression == "crossangry")
						{
							return E_Facial_Expressions.crossangry;
						}
					}
					else if (num != 504067199U)
					{
						if (num == 581173629U)
						{
							if (expression == "moon_b")
							{
								return E_Facial_Expressions.moon_b;
							}
						}
					}
					else if (expression == "firelaugh_b")
					{
						return E_Facial_Expressions.custom_laugh_b;
					}
				}
				else if (num <= 1030696852U)
				{
					if (num <= 828922338U)
					{
						if (num != 588970055U)
						{
							if (num != 782019804U)
							{
								if (num == 828922338U)
								{
									if (expression == "firelaugh")
									{
										return E_Facial_Expressions.custom_laugh;
									}
								}
							}
							else if (expression == "flirt")
							{
								e_Facial_Expressions = E_Facial_Expressions.flirt;
								if (isBlinking)
								{
									return E_Facial_Expressions.flirt_b;
								}
								return e_Facial_Expressions;
							}
						}
						else if (expression == "treat")
						{
							return E_Facial_Expressions.custom_treat;
						}
					}
					else if (num <= 974867124U)
					{
						if (num != 898818735U)
						{
							if (num == 974867124U)
							{
								if (expression == "rock")
								{
									return E_Facial_Expressions.rock;
								}
							}
						}
						else if (expression == "happycry")
						{
							return E_Facial_Expressions.happycry;
						}
					}
					else if (num != 989916275U)
					{
						if (num == 1030696852U)
						{
							if (expression == "arf")
							{
								return E_Facial_Expressions.custom_arf;
							}
						}
					}
					else if (expression == "smirk")
					{
						e_Facial_Expressions = E_Facial_Expressions.smirk;
						if (isBlinking)
						{
							return E_Facial_Expressions.smirk_b;
						}
						return e_Facial_Expressions;
					}
				}
				else if (num <= 1095482827U)
				{
					if (num != 1055615879U)
					{
						if (num != 1095058742U)
						{
							if (num == 1095482827U)
							{
								if (expression == "knop")
								{
									return E_Facial_Expressions.knop;
								}
							}
						}
						else if (expression == "happycry_b")
						{
							return E_Facial_Expressions.happycry_b;
						}
					}
					else if (expression == "pinch")
					{
						return E_Facial_Expressions.pinch;
					}
				}
				else if (num <= 1385011439U)
				{
					if (num != 1235535326U)
					{
						if (num == 1385011439U)
						{
							if (expression == "rap1")
							{
								return E_Facial_Expressions.custom_rap1;
							}
						}
					}
					else if (expression == "pinch_b")
					{
						return E_Facial_Expressions.pinch_b;
					}
				}
				else if (num != 1401789058U)
				{
					if (num == 1406826723U)
					{
						if (expression == "bark")
						{
							return E_Facial_Expressions.custom_bark;
						}
					}
				}
				else if (expression == "rap2")
				{
					return E_Facial_Expressions.custom_rap2;
				}
			}
			else if (num <= 1881948671U)
			{
				if (num <= 1619038978U)
				{
					if (num <= 1449939112U)
					{
						if (num != 1418566677U)
						{
							if (num != 1426428559U)
							{
								if (num == 1449939112U)
								{
									if (expression == "blink1")
									{
										return E_Facial_Expressions.neutral_blink1;
									}
								}
							}
							else if (expression == "smile")
							{
								return E_Facial_Expressions.custom_smile;
							}
						}
						else if (expression == "rap3")
						{
							return E_Facial_Expressions.custom_rap3;
						}
					}
					else if (num <= 1538531746U)
					{
						if (num != 1500271969U)
						{
							if (num == 1538531746U)
							{
								if (expression == "back")
								{
									return E_Facial_Expressions.custom_back;
								}
							}
						}
						else if (expression == "blink2")
						{
							return E_Facial_Expressions.neutral_blink2;
						}
					}
					else if (num != 1616333674U)
					{
						if (num == 1619038978U)
						{
							if (expression == "trap")
							{
								return E_Facial_Expressions.custom_trap;
							}
						}
					}
					else if (expression == "cry_b")
					{
						return E_Facial_Expressions.cry_b;
					}
				}
				else if (num <= 1714644417U)
				{
					if (num != 1656618424U)
					{
						if (num != 1697565771U)
						{
							if (num == 1714644417U)
							{
								if (expression == "beast_b")
								{
									return E_Facial_Expressions.beast_b;
								}
							}
						}
						else if (expression == "helmsad")
						{
							return E_Facial_Expressions.custom_helmsad;
						}
					}
					else if (expression == "hurt")
					{
						return E_Facial_Expressions.hurt;
					}
				}
				else if (num <= 1819825498U)
				{
					if (num != 1790834346U)
					{
						if (num == 1819825498U)
						{
							if (expression == "yum")
							{
								return E_Facial_Expressions.yum;
							}
						}
					}
					else if (expression == "volt")
					{
						return E_Facial_Expressions.custom_volt;
					}
				}
				else if (num != 1848426333U)
				{
					if (num == 1881948671U)
					{
						if (expression == "harper2")
						{
							return E_Facial_Expressions.custom_harper2;
						}
					}
				}
				else if (expression == "helmhappy")
				{
					return E_Facial_Expressions.custom_helmhappy;
				}
			}
			else if (num <= 2008924546U)
			{
				if (num <= 1913005056U)
				{
					if (num != 1898726290U)
					{
						if (num != 1900915322U)
						{
							if (num == 1913005056U)
							{
								if (expression == "shoe2")
								{
									return E_Facial_Expressions.custom_shoe2;
								}
							}
						}
						else if (expression == "human")
						{
							return E_Facial_Expressions.custom_human;
						}
					}
					else if (expression == "harper1")
					{
						return E_Facial_Expressions.custom_harper1;
					}
				}
				else if (num <= 1963337913U)
				{
					if (num != 1927346304U)
					{
						if (num == 1963337913U)
						{
							if (expression == "shoe1")
							{
								return E_Facial_Expressions.custom_shoe1;
							}
						}
					}
					else if (expression == "ice")
					{
						return E_Facial_Expressions.custom_ice;
					}
				}
				else if (num != 1998127368U)
				{
					if (num == 2008924546U)
					{
						if (expression == "growl")
						{
							return E_Facial_Expressions.custom_growl;
						}
					}
				}
				else if (expression == "pout_b")
				{
					return E_Facial_Expressions.pout_b;
				}
			}
			else if (num <= 2125639436U)
			{
				if (num != 2020464800U)
				{
					if (num != 2093468251U)
					{
						if (num == 2125639436U)
						{
							if (expression == "rage")
							{
								return E_Facial_Expressions.rage;
							}
						}
					}
					else if (expression == "think")
					{
						return E_Facial_Expressions.think;
					}
				}
				else if (expression == "broken")
				{
					return E_Facial_Expressions.custom_broken;
				}
			}
			else if (num <= 2247813357U)
			{
				if (num != 2166136261U)
				{
					if (num == 2247813357U)
					{
						if (expression == "crosssad")
						{
							return E_Facial_Expressions.crosssad;
						}
					}
				}
				else if (expression != null)
				{
					if (expression.Length == 0)
					{
						e_Facial_Expressions = E_Facial_Expressions.neutral;
						if (isBlinking)
						{
							return E_Facial_Expressions.neutral_b;
						}
						return e_Facial_Expressions;
					}
				}
			}
			else if (num != 2319433256U)
			{
				if (num == 2329051074U)
				{
					if (expression == "rockjoy")
					{
						return E_Facial_Expressions.custom_rockjoy;
					}
				}
			}
			else if (expression == "omega")
			{
				return E_Facial_Expressions.custom_omega;
			}
		}
		else if (num <= 3173563725U)
		{
			if (num <= 2752420076U)
			{
				if (num <= 2617292509U)
				{
					if (num <= 2427567480U)
					{
						if (num != 2373614624U)
						{
							if (num != 2381086145U)
							{
								if (num == 2427567480U)
								{
									if (expression == "blushlookaway")
									{
										return E_Facial_Expressions.custom_blushlookaway;
									}
								}
							}
							else if (expression == "pout")
							{
								return E_Facial_Expressions.pout;
							}
						}
						else if (expression == "helmet")
						{
							return E_Facial_Expressions.helmet;
						}
					}
					else if (num <= 2552809861U)
					{
						if (num != 2484390670U)
						{
							if (num == 2552809861U)
							{
								if (expression == "tue")
								{
									return E_Facial_Expressions.custom_tue;
								}
							}
						}
						else if (expression == "crossflirt")
						{
							return E_Facial_Expressions.crossflirt;
						}
					}
					else if (num != 2575709521U)
					{
						if (num == 2617292509U)
						{
							if (expression == "mess")
							{
								return E_Facial_Expressions.custom_mess;
							}
						}
					}
					else if (expression == "happy")
					{
						e_Facial_Expressions = E_Facial_Expressions.happy;
						if (isBlinking)
						{
							return E_Facial_Expressions.happy_b;
						}
						return e_Facial_Expressions;
					}
				}
				else if (num <= 2686129127U)
				{
					if (num != 2637411955U)
					{
						if (num != 2685309491U)
						{
							if (num == 2686129127U)
							{
								if (expression == "nude")
								{
									return E_Facial_Expressions.custom_nude;
								}
							}
						}
						else if (expression == "crossflirt_b")
						{
							return E_Facial_Expressions.crossflirt_b;
						}
					}
					else if (expression == "concern")
					{
						return E_Facial_Expressions.custom_concern;
					}
				}
				else if (num <= 2711743576U)
				{
					if (num != 2688867395U)
					{
						if (num == 2711743576U)
						{
							if (expression == "helm1")
							{
								return E_Facial_Expressions.custom_helm1;
							}
						}
					}
					else if (expression == "cluck")
					{
						return E_Facial_Expressions.custom_cluck;
					}
				}
				else if (num != 2745298814U)
				{
					if (num == 2752420076U)
					{
						if (expression == "thu")
						{
							return E_Facial_Expressions.custom_thu;
						}
					}
				}
				else if (expression == "helm3")
				{
					return E_Facial_Expressions.custom_helm3;
				}
			}
			else if (num <= 2904790937U)
			{
				if (num <= 2795631671U)
				{
					if (num != 2762076433U)
					{
						if (num != 2778854052U)
						{
							if (num == 2795631671U)
							{
								if (expression == "helm4")
								{
									return E_Facial_Expressions.custom_helm4;
								}
							}
						}
						else if (expression == "helm5")
						{
							return E_Facial_Expressions.custom_helm5;
						}
					}
					else if (expression == "helm2")
					{
						return E_Facial_Expressions.custom_helm2;
					}
				}
				else if (num <= 2868343601U)
				{
					if (num != 2856220944U)
					{
						if (num == 2868343601U)
						{
							if (expression == "shock")
							{
								e_Facial_Expressions = E_Facial_Expressions.shock;
								if (isBlinking)
								{
									return E_Facial_Expressions.shock_b;
								}
								return e_Facial_Expressions;
							}
						}
					}
					else if (expression == "anger")
					{
						return E_Facial_Expressions.anger;
					}
				}
				else if (num != 2892060831U)
				{
					if (num == 2904790937U)
					{
						if (expression == "crossangry_b")
						{
							return E_Facial_Expressions.crossangry_b;
						}
					}
				}
				else if (expression == "rockjoy_b")
				{
					return E_Facial_Expressions.custom_rockjoy_b;
				}
			}
			else if (num <= 3031711993U)
			{
				if (num != 2922578957U)
				{
					if (num != 3004461074U)
					{
						if (num == 3031711993U)
						{
							if (expression == "crosstsun")
							{
								return E_Facial_Expressions.crosstsundere;
							}
						}
					}
					else if (expression == "sans")
					{
						return E_Facial_Expressions.custom_sans;
					}
				}
				else if (expression == "wake")
				{
					return E_Facial_Expressions.wake;
				}
			}
			else if (num <= 3089998242U)
			{
				if (num != 3037376254U)
				{
					if (num == 3089998242U)
					{
						if (expression == "shout")
						{
							return E_Facial_Expressions.shout;
						}
					}
				}
				else if (expression == "angry")
				{
					e_Facial_Expressions = E_Facial_Expressions.angry;
					if (isBlinking)
					{
						return E_Facial_Expressions.angry_b;
					}
					return e_Facial_Expressions;
				}
			}
			else if (num != 3101644410U)
			{
				if (num == 3173563725U)
				{
					if (expression == "death")
					{
						return E_Facial_Expressions.death;
					}
				}
			}
			else if (expression == "helmangry")
			{
				return E_Facial_Expressions.custom_helmangry;
			}
		}
		else if (num <= 3773841056U)
		{
			if (num <= 3547173497U)
			{
				if (num <= 3280944101U)
				{
					if (num != 3184975099U)
					{
						if (num != 3201752718U)
						{
							if (num == 3280944101U)
							{
								if (expression == "mid")
								{
									return E_Facial_Expressions.mid;
								}
							}
						}
						else if (expression == "puzzled1")
						{
							return E_Facial_Expressions.custom_puzzled_1;
						}
					}
					else if (expression == "puzzled2")
					{
						return E_Facial_Expressions.custom_puzzled_2;
					}
				}
				else if (num <= 3379211142U)
				{
					if (num != 3285700936U)
					{
						if (num == 3379211142U)
						{
							if (expression == "froth")
							{
								return E_Facial_Expressions.custom_froth;
							}
						}
					}
					else if (expression == "fri")
					{
						return E_Facial_Expressions.custom_fri;
					}
				}
				else if (num != 3470762949U)
				{
					if (num == 3547173497U)
					{
						if (expression == "blush")
						{
							e_Facial_Expressions = E_Facial_Expressions.blush;
							if (isBlinking)
							{
								return E_Facial_Expressions.blush_b;
							}
							return e_Facial_Expressions;
						}
					}
				}
				else if (expression == "read")
				{
					return E_Facial_Expressions.custom_read;
				}
			}
			else if (num <= 3630696144U)
			{
				if (num != 3592196823U)
				{
					if (num != 3620438587U)
					{
						if (num == 3630696144U)
						{
							if (expression == "canister")
							{
								return E_Facial_Expressions.custom_canister;
							}
						}
					}
					else if (expression == "glasses")
					{
						return E_Facial_Expressions.glasses;
					}
				}
				else if (expression == "sat")
				{
					return E_Facial_Expressions.custom_sat;
				}
			}
			else if (num <= 3680084270U)
			{
				if (num != 3637782525U)
				{
					if (num == 3680084270U)
					{
						if (expression == "dex")
						{
							return E_Facial_Expressions.dex;
						}
					}
				}
				else if (expression == "joy")
				{
					e_Facial_Expressions = E_Facial_Expressions.joy;
					if (isBlinking)
					{
						return E_Facial_Expressions.joy_b;
					}
					return e_Facial_Expressions;
				}
			}
			else if (num != 3767245148U)
			{
				if (num == 3773841056U)
				{
					if (expression == "teddy1")
					{
						return E_Facial_Expressions.custom_teddy1;
					}
				}
			}
			else if (expression == "beast")
			{
				return E_Facial_Expressions.beast;
			}
		}
		else if (num <= 3860638727U)
		{
			if (num <= 3807396294U)
			{
				if (num != 3782859736U)
				{
					if (num != 3794957372U)
					{
						if (num == 3807396294U)
						{
							if (expression == "teddy3")
							{
								return E_Facial_Expressions.custom_teddy3;
							}
						}
					}
					else if (expression == "crosssad_b")
					{
						return E_Facial_Expressions.crosssad_b;
					}
				}
				else if (expression == "front")
				{
					return E_Facial_Expressions.custom_front;
				}
			}
			else if (num <= 3854742175U)
			{
				if (num != 3824173913U)
				{
					if (num == 3854742175U)
					{
						if (expression == "chirp")
						{
							return E_Facial_Expressions.custom_chirp;
						}
					}
				}
				else if (expression == "teddy2")
				{
					return E_Facial_Expressions.custom_teddy2;
				}
			}
			else if (num != 3857729151U)
			{
				if (num == 3860638727U)
				{
					if (expression == "sad")
					{
						e_Facial_Expressions = E_Facial_Expressions.sad;
						if (isBlinking)
						{
							return E_Facial_Expressions.sad_b;
						}
						return e_Facial_Expressions;
					}
				}
			}
			else if (expression == "teddy4")
			{
				return E_Facial_Expressions.custom_teddy4;
			}
		}
		else if (num <= 4052004416U)
		{
			if (num != 3919479361U)
			{
				if (num != 4038796393U)
				{
					if (num == 4052004416U)
					{
						if (expression == "moon")
						{
							return E_Facial_Expressions.moon;
						}
					}
				}
				else if (expression == "rock_b")
				{
					return E_Facial_Expressions.custom_rock_b;
				}
			}
			else if (expression == "mon")
			{
				return E_Facial_Expressions.custom_mon;
			}
		}
		else if (num <= 4182042971U)
		{
			if (num != 4079998668U)
			{
				if (num == 4182042971U)
				{
					if (expression == "cry")
					{
						return E_Facial_Expressions.cry;
					}
				}
			}
			else if (expression == "capsule")
			{
				return E_Facial_Expressions.custom_capsule;
			}
		}
		else if (num != 4192455999U)
		{
			if (num == 4286165820U)
			{
				if (expression == "full")
				{
					return E_Facial_Expressions.custom_full;
				}
			}
		}
		else if (expression == "shout_b")
		{
			return E_Facial_Expressions.shout_b;
		}
		e_Facial_Expressions = E_Facial_Expressions.neutral;
		if (isBlinking)
		{
			e_Facial_Expressions = E_Facial_Expressions.neutral_b;
		}
		return e_Facial_Expressions;
	}

	// Token: 0x040005D7 RID: 1495
	[SerializeField]
	public string internalName;

	// Token: 0x040005D8 RID: 1496
	[SerializeField]
	public bool isCollectableCharacter;

	// Token: 0x040005D9 RID: 1497
	[SerializeField]
	[Tooltip("This does not need .ink")]
	public string inkName;

	// Token: 0x040005DA RID: 1498
	[SerializeField]
	public string parentFolderPath;

	// Token: 0x040005DB RID: 1499
	[SerializeField]
	public Sprite errorSprite;
}
