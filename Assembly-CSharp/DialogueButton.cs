using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004B RID: 75
public class DialogueButton : MonoBehaviour
{
	// Token: 0x060001E9 RID: 489 RVA: 0x0000BB20 File Offset: 0x00009D20
	public void SetStatAnimationIconSprites(string stat)
	{
		this._stat = stat;
		string text = stat.ToLowerInvariant();
		if (text == "poise")
		{
			this.ChangeStatSprites(this.PoiseSprite);
			return;
		}
		if (text == "charm")
		{
			this.ChangeStatSprites(this.CharmSprite);
			return;
		}
		if (text == "smarts")
		{
			this.ChangeStatSprites(this.SmartsSprite);
			return;
		}
		if (text == "empathy")
		{
			this.ChangeStatSprites(this.EmpathySprite);
			return;
		}
		if (!(text == "sass"))
		{
			this.ChangeStatSprites(null);
			return;
		}
		this.ChangeStatSprites(this.SassSprite);
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000BBC6 File Offset: 0x00009DC6
	public void ShowStatAnimation()
	{
		if (this.triggerSpecAnimation)
		{
			this.SetStatAnimationIconSprites(this._stat);
			this.StatAnimationObj.SetActive(true);
			return;
		}
		this.StatAnimationObj.SetActive(false);
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000BBF5 File Offset: 0x00009DF5
	public void HideStatAnimation()
	{
		if (this.triggerSpecAnimation)
		{
			this.StatAnimationObj.SetActive(false);
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000BC0C File Offset: 0x00009E0C
	private void ChangeStatSprites(Sprite sprite)
	{
		if (sprite == null)
		{
			return;
		}
		for (int i = 0; i < this.StatAnimationObj.transform.childCount; i++)
		{
			this.StatAnimationObj.transform.GetChild(i).GetComponent<Image>().sprite = sprite;
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000BC5C File Offset: 0x00009E5C
	public void UpdateCandyIcon(string text)
	{
		if (text.Contains("(USE POISE CANDY)"))
		{
			this.UseCandyIcon.gameObject.SetActive(true);
			this.UseCandyIcon.sprite = this.PoiseSprite;
			return;
		}
		if (text.Contains("(USE SASS CANDY)"))
		{
			this.UseCandyIcon.gameObject.SetActive(true);
			this.UseCandyIcon.sprite = this.SassSprite;
			return;
		}
		if (text.Contains("(USE EMPATHY CANDY)"))
		{
			this.UseCandyIcon.gameObject.SetActive(true);
			this.UseCandyIcon.sprite = this.EmpathySprite;
			return;
		}
		if (text.Contains("(USE SMARTS CANDY)"))
		{
			this.UseCandyIcon.gameObject.SetActive(true);
			this.UseCandyIcon.sprite = this.SmartsSprite;
			return;
		}
		if (text.Contains("(USE CHARM CANDY)"))
		{
			this.UseCandyIcon.gameObject.SetActive(true);
			this.UseCandyIcon.sprite = this.CharmSprite;
			return;
		}
		this.UseCandyIcon.gameObject.SetActive(false);
	}

	// Token: 0x040002CF RID: 719
	private const int InvalidChoiceIndex = -1;

	// Token: 0x040002D0 RID: 720
	[SerializeField]
	private GameObject StatAnimationObj;

	// Token: 0x040002D1 RID: 721
	[SerializeField]
	private Image UseCandyIcon;

	// Token: 0x040002D2 RID: 722
	[SerializeField]
	private Sprite SmartsSprite;

	// Token: 0x040002D3 RID: 723
	[SerializeField]
	private Sprite PoiseSprite;

	// Token: 0x040002D4 RID: 724
	[SerializeField]
	private Sprite EmpathySprite;

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	private Sprite CharmSprite;

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	private Sprite SassSprite;

	// Token: 0x040002D7 RID: 727
	public bool triggerSpecAnimation;

	// Token: 0x040002D8 RID: 728
	private string _stat = "";
}
