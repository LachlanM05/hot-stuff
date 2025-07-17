using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class ObjectExamine : MonoBehaviour
{
	// Token: 0x06000566 RID: 1382 RVA: 0x0001F5CC File Offset: 0x0001D7CC
	public bool ShowExamine()
	{
		string text = this.InkNode;
		if (!string.IsNullOrEmpty(text))
		{
			string currentFlowName = Singleton<InkController>.Instance.story.currentFlowName;
			if (text.StartsWith("Boxes_"))
			{
				int num = Singleton<Save>.Instance.AddBoxExamenData(this.InkNode);
				Singleton<AchievementController>.Instance.CheckBoxingDay();
				text = "Boxes_" + num.ToString();
			}
			Singleton<InkController>.Instance.story.SwitchFlow("examine_text." + text);
			Singleton<InkController>.Instance.story.ChoosePathString("examine_text." + text, true, Array.Empty<object>());
			if (Singleton<InkController>.Instance.story.canContinue)
			{
				Singleton<InkController>.Instance.ContinueStory();
				string text2 = "";
				RelationshipStatus relationshipStatus = RelationshipStatus.Unmet;
				foreach (string text3 in Singleton<InkController>.Instance.story.currentTags)
				{
					if (this.type == ObjectExamine.OverrideType.DoNotDisplay)
					{
						return false;
					}
					if (text3.StartsWith("character "))
					{
						string text4 = text3.Substring(text3.IndexOf(" ") + 1).Trim();
						if (text4.Contains("."))
						{
							text4 = text4.Substring(0, text4.IndexOf("."));
						}
						relationshipStatus = Singleton<Save>.Instance.GetDateStatus(text4);
						if (this.type == ObjectExamine.OverrideType.NullHate)
						{
							relationshipStatus = RelationshipStatus.Single;
						}
						else if (this.type == ObjectExamine.OverrideType.Loved)
						{
							relationshipStatus = RelationshipStatus.Love;
						}
					}
					else if (text3.StartsWith("null_hate_phrase") && (relationshipStatus == RelationshipStatus.Unmet || relationshipStatus == RelationshipStatus.Hate || relationshipStatus == RelationshipStatus.Single))
					{
						text2 = text3.Substring(text3.IndexOf(" ") + 1).Trim();
						if (text2 == "null_hate_phrase")
						{
							return false;
						}
						break;
					}
					else if (text3.StartsWith("love_friend_phrase") && (relationshipStatus == RelationshipStatus.Friend || relationshipStatus == RelationshipStatus.Love || relationshipStatus == RelationshipStatus.Realized))
					{
						text2 = text3.Substring(text3.IndexOf(" ") + 1).Trim();
						if (text2 == "love_friend_phrase")
						{
							return false;
						}
						break;
					}
				}
				string text5 = this.CleanUpText(text2);
				Singleton<InkController>.Instance.story.SwitchFlow(currentFlowName);
				Singleton<ExamineController>.Instance.ShowExamine(text5);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0001F850 File Offset: 0x0001DA50
	private string CleanUpText(string text)
	{
		if (text.StartsWith("true"))
		{
			text = text.Substring(4);
		}
		if (text.StartsWith("false"))
		{
			text = text.Substring(5);
		}
		return text;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0001F87F File Offset: 0x0001DA7F
	private void CheckToSwapInkNode()
	{
		string inkNode = this.InkNode;
	}

	// Token: 0x04000540 RID: 1344
	public string InkNode;

	// Token: 0x04000541 RID: 1345
	public ObjectExamine.OverrideType type;

	// Token: 0x020002D9 RID: 729
	public enum OverrideType
	{
		// Token: 0x04001149 RID: 4425
		None,
		// Token: 0x0400114A RID: 4426
		NullHate,
		// Token: 0x0400114B RID: 4427
		Loved,
		// Token: 0x0400114C RID: 4428
		DoNotDisplay
	}
}
