using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F6 RID: 246
public class DexEntryButton : ListBox
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000877 RID: 2167 RVA: 0x00032047 File Offset: 0x00030247
	// (set) Token: 0x06000878 RID: 2168 RVA: 0x0003204F File Offset: 0x0003024F
	public DateADexEntry DexEntry { get; private set; }

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x00032058 File Offset: 0x00030258
	// (set) Token: 0x0600087A RID: 2170 RVA: 0x00032060 File Offset: 0x00030260
	public bool MetEntry { get; private set; }

	// Token: 0x0600087B RID: 2171 RVA: 0x0003206C File Offset: 0x0003026C
	protected override void UpdateDisplayContent(IListContent content)
	{
		DateADexEntry dateADexEntry = (DateADexEntry)content;
		if (dateADexEntry.internalName == "tim")
		{
			if ((bool)Singleton<InkController>.Instance.story.variablesState["tim_catboy_perm"])
			{
				dateADexEntry.CharName = "Timmy";
			}
			else
			{
				dateADexEntry.CharName = "Timothy Timepiece";
			}
		}
		else if (dateADexEntry.internalName == "connie")
		{
			if (Singleton<InkController>.Instance.story.variablesState["connie_ending"].ToString() == "null")
			{
				dateADexEntry.CharName = "Luna";
				dateADexEntry.CharLikes = "Reloading, Looting, Corridors";
				dateADexEntry.CharDislikes = "Scroufs, Upholders, Selene";
			}
			else
			{
				dateADexEntry.CharName = "Connie Soul";
				dateADexEntry.CharLikes = "Interactive Fiction, Voice Acting, Day One Patches";
				dateADexEntry.CharDislikes = "Overwriting Saves Accidentally";
			}
		}
		else if (dateADexEntry.internalName == "scandalabra")
		{
			if (Singleton<InkController>.Instance.story.variablesState["jon_dex"].ToString() == "on")
			{
				dateADexEntry.CharName = "Jon";
				dateADexEntry.CharObj = "Wick";
				dateADexEntry.CharLikes = "Revenge";
				dateADexEntry.CharDislikes = "Magnifying Lenses";
			}
			else
			{
				dateADexEntry.CharName = "Scandalabra";
				dateADexEntry.CharObj = "Candelabra";
				dateADexEntry.CharLikes = "Scandal, Secrets, Polish";
				dateADexEntry.CharDislikes = "Rudeness, Low Speech, Dillydallying";
			}
		}
		else if (dateADexEntry.internalName == "dirk")
		{
			if (Singleton<InkController>.Instance.story.variablesState["harper_dirk"].ToString() == "healthy" && Singleton<InkController>.Instance.GetVariable("clarence_transform") != "dirk")
			{
				dateADexEntry.CharName = "Clarence Couture";
				dateADexEntry.CharObj = "Clean Clothes";
				dateADexEntry.CharLikes = "Fashion, Accessories, Bat-man";
			}
			else
			{
				dateADexEntry.CharName = "Dirk Deveraux";
				dateADexEntry.CharObj = "Dirty Laundry";
				dateADexEntry.CharLikes = "Security, Harper, Being told what to do";
			}
		}
		else if (dateADexEntry.internalName == "lucinda")
		{
			if (Singleton<InkController>.Instance.story.variablesState["lucinda_final"].ToString() == "null")
			{
				dateADexEntry.CharName = "Lucinda Lavish";
				dateADexEntry.CharLikes = "Luxury";
				dateADexEntry.CharDislikes = "Lollygagging";
			}
			else if (Singleton<InkController>.Instance.story.variablesState["lucinda_final"].ToString() == "lust")
			{
				dateADexEntry.CharName = "Lucinda Lust";
				dateADexEntry.CharLikes = "Licking";
				dateADexEntry.CharDislikes = "Lamewads";
			}
			else if (Singleton<InkController>.Instance.story.variablesState["lucinda_final"].ToString() == "loving")
			{
				dateADexEntry.CharName = "Lucinda Loving";
				dateADexEntry.CharLikes = "Listening";
				dateADexEntry.CharDislikes = "Labyrinths";
			}
			else if (Singleton<InkController>.Instance.story.variablesState["lucinda_final"].ToString() == "limitless")
			{
				dateADexEntry.CharName = "Lucinda Limitless";
				dateADexEntry.CharLikes = "Legends";
				dateADexEntry.CharDislikes = "Limits";
			}
			else if (Singleton<InkController>.Instance.story.variablesState["lucinda_final"].ToString() == "lucid")
			{
				dateADexEntry.CharName = "Lucinda Lucid";
				dateADexEntry.CharLikes = "Listing Likes";
				dateADexEntry.CharDislikes = "Listing Dislikes";
			}
			else
			{
				dateADexEntry.CharName = "Lucinda Loathsome";
				dateADexEntry.CharLikes = "Letting Go";
				dateADexEntry.CharDislikes = "You";
			}
		}
		this.DexEntry = dateADexEntry;
		this.MetEntry = Singleton<Save>.Instance.GetDateStatus(dateADexEntry.CharIndex) > RelationshipStatus.Unmet;
		if (this.MetEntry)
		{
			this.numberText.text = (dateADexEntry.CharIndex + 1).ToString();
			this.nameText.text = dateADexEntry.CharName.ToUpperInvariant() + " ";
			if (this.notificationIndecator)
			{
				this.notificationIndecator.SetActive(false);
				if (dateADexEntry.notification)
				{
					this.notificationIndecator.SetActive(true);
				}
			}
			try
			{
				switch (Singleton<Save>.Instance.GetDateStatus(dateADexEntry.CharIndex))
				{
				case RelationshipStatus.Hate:
					this.relationshipIndicator.gameObject.SetActive(true);
					this.relationshipIndicator.runtimeAnimatorController = this.hateAnimation;
					goto IL_0528;
				case RelationshipStatus.Love:
					this.relationshipIndicator.gameObject.SetActive(true);
					this.relationshipIndicator.runtimeAnimatorController = this.loveAnimation;
					goto IL_0528;
				case RelationshipStatus.Friend:
					this.relationshipIndicator.gameObject.SetActive(true);
					this.relationshipIndicator.runtimeAnimatorController = this.friendAnimation;
					goto IL_0528;
				}
				this.relationshipIndicator.gameObject.SetActive(false);
				IL_0528:;
			}
			catch (Exception)
			{
				this.relationshipIndicator.gameObject.SetActive(false);
			}
			try
			{
				if (Singleton<Save>.Instance.GetDateStatusRealized(dateADexEntry.CharIndex) == RelationshipStatus.Realized)
				{
					this.backgroundImage.sprite = this.realizedBackground;
				}
				else
				{
					this.backgroundImage.sprite = this.regularBackground;
				}
				return;
			}
			catch (Exception)
			{
				this.backgroundImage.sprite = this.regularBackground;
				return;
			}
		}
		this.numberText.text = (dateADexEntry.CharIndex + 1).ToString();
		this.nameText.text = "???";
		if (this.notificationIndecator)
		{
			this.notificationIndecator.SetActive(false);
		}
		this.relationshipIndicator.gameObject.SetActive(false);
		this.backgroundImage.sprite = this.regularBackground;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00032688 File Offset: 0x00030888
	public void UpdateCustomDisplayContent(RelationshipStatus status, string charNumber, string charName, string charObj)
	{
		this.numberText.text = charNumber;
		this.nameText.text = charName.ToUpperInvariant() + " ";
		try
		{
			switch (status)
			{
			case RelationshipStatus.Hate:
				this.relationshipIndicator.gameObject.SetActive(true);
				this.relationshipIndicator.runtimeAnimatorController = this.hateAnimation;
				goto IL_00C2;
			case RelationshipStatus.Love:
				this.relationshipIndicator.gameObject.SetActive(true);
				this.relationshipIndicator.runtimeAnimatorController = this.loveAnimation;
				goto IL_00C2;
			case RelationshipStatus.Friend:
				this.relationshipIndicator.gameObject.SetActive(true);
				this.relationshipIndicator.runtimeAnimatorController = this.friendAnimation;
				goto IL_00C2;
			}
			this.relationshipIndicator.gameObject.SetActive(false);
			IL_00C2:;
		}
		catch (Exception)
		{
			this.relationshipIndicator.gameObject.SetActive(false);
		}
		try
		{
			if (Singleton<Save>.Instance.GetDateStatusRealized(int.Parse(charNumber)) == RelationshipStatus.Realized)
			{
				this.backgroundImage.sprite = this.realizedBackground;
			}
			else
			{
				this.backgroundImage.sprite = this.regularBackground;
			}
		}
		catch (Exception)
		{
			this.backgroundImage.sprite = this.regularBackground;
		}
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x000327D8 File Offset: 0x000309D8
	public override void OnBoxMoved(float positionRatio)
	{
		this.positionRatio = positionRatio;
		float num = this.fadeCurve.Evaluate(Mathf.Abs(positionRatio));
		if (!Mathf.Approximately(num, 0f))
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		if (this.fadeCanvasGroup != null)
		{
			this.fadeCanvasGroup.alpha = 1f - num;
			return;
		}
		foreach (DexEntryButton.LerpImageColor lerpImageColor in this.fadeImages)
		{
			lerpImageColor.Image.color = Color.Lerp(lerpImageColor.Color1, lerpImageColor.Color2, num);
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00032894 File Offset: 0x00030A94
	public void SetupNavigation()
	{
		if (base.gameObject.GetComponent<Button>() != null)
		{
			Navigation navigation = base.gameObject.GetComponent<Button>().navigation;
			if (base.transform.GetSiblingIndex() == base.transform.parent.childCount - 1)
			{
				navigation.selectOnUp = null;
				Navigation navigation2 = DateADex.Instance.CollectableButton.navigation;
				navigation2.selectOnDown = base.gameObject.GetComponent<Button>();
				navigation2.mode = Navigation.Mode.Explicit;
				DateADex.Instance.CollectableButton.navigation = navigation2;
			}
			else
			{
				navigation.selectOnUp = base.transform.parent.GetChild(base.transform.GetSiblingIndex() + 1).GetComponent<Button>();
			}
			if (base.transform.GetSiblingIndex() == 0)
			{
				navigation.selectOnDown = DateADex.Instance.SortButton;
				Navigation navigation3 = DateADex.Instance.SortButton.navigation;
				navigation3.selectOnUp = base.gameObject.GetComponent<Button>();
				navigation3.mode = Navigation.Mode.Explicit;
				DateADex.Instance.SortButton.navigation = navigation3;
			}
			else
			{
				navigation.selectOnDown = base.transform.parent.GetChild(base.transform.GetSiblingIndex() - 1).GetComponent<Button>();
			}
			navigation.mode = Navigation.Mode.Explicit;
			base.gameObject.GetComponent<Button>().navigation = navigation;
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x000329F0 File Offset: 0x00030BF0
	public void SetupOnSelect(DateADex dateADex)
	{
		this.dateADexScreen = dateADex;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x000329F9 File Offset: 0x00030BF9
	public void OnSelect()
	{
		this.dateADexScreen.OnEntrySelected(base.GetComponent<Selectable>());
	}

	// Token: 0x040007C2 RID: 1986
	[SerializeField]
	private TextMeshProUGUI numberText;

	// Token: 0x040007C3 RID: 1987
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x040007C4 RID: 1988
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x040007C5 RID: 1989
	[SerializeField]
	private Image backgroundImage;

	// Token: 0x040007C6 RID: 1990
	[SerializeField]
	private Sprite regularBackground;

	// Token: 0x040007C7 RID: 1991
	[SerializeField]
	private Sprite realizedBackground;

	// Token: 0x040007C8 RID: 1992
	[SerializeField]
	private CanvasGroup fadeCanvasGroup;

	// Token: 0x040007C9 RID: 1993
	[SerializeField]
	private AnimationCurve fadeCurve;

	// Token: 0x040007CA RID: 1994
	[SerializeField]
	private Animator relationshipIndicator;

	// Token: 0x040007CB RID: 1995
	[SerializeField]
	private RuntimeAnimatorController loveAnimation;

	// Token: 0x040007CC RID: 1996
	[SerializeField]
	private RuntimeAnimatorController hateAnimation;

	// Token: 0x040007CD RID: 1997
	[SerializeField]
	private RuntimeAnimatorController friendAnimation;

	// Token: 0x040007CE RID: 1998
	[SerializeField]
	private List<DexEntryButton.LerpImageColor> fadeImages;

	// Token: 0x040007CF RID: 1999
	public GameObject notificationIndecator;

	// Token: 0x040007D2 RID: 2002
	private float positionRatio;

	// Token: 0x040007D3 RID: 2003
	private DateADex dateADexScreen;

	// Token: 0x02000312 RID: 786
	[Serializable]
	public struct LerpImageColor
	{
		// Token: 0x04001238 RID: 4664
		public Image Image;

		// Token: 0x04001239 RID: 4665
		public Color Color1;

		// Token: 0x0400123A RID: 4666
		public Color Color2;
	}
}
