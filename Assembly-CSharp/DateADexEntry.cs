using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList.ContentManagement;
using Date_Everything.Scripts.ScriptableObjects;
using UnityEngine;

// Token: 0x020000C8 RID: 200
[Serializable]
public class DateADexEntry : IListContent
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060006A8 RID: 1704 RVA: 0x000272DB File Offset: 0x000254DB
	public Sprite CharImage
	{
		get
		{
			return this.CharImageProvider.CharImage;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000272E8 File Offset: 0x000254E8
	public bool isAwakened
	{
		get
		{
			return Singleton<Save>.Instance.GetDateStatus(this.internalName) > RelationshipStatus.Unmet;
		}
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x000272FD File Offset: 0x000254FD
	public DateADexEntry()
	{
		this.Collectable_Names_Desc_Hint = new List<Tuple<string, string, string>>();
		this.DexComments = new List<string>();
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0002731C File Offset: 0x0002551C
	public int GetRecipe(SpecsAttributes specs)
	{
		if (this.Recipe == null || this.Recipe == "" || !this.Recipe.Contains(","))
		{
			return 0;
		}
		return int.Parse(this.Recipe.Split(',', StringSplitOptions.None)[(int)specs]);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0002736C File Offset: 0x0002556C
	public override string ToString()
	{
		return string.Concat(new string[] { this.CharName, " ", this.CharObj, " Likes: ", this.CharLikes, ", Dislikes ", this.CharDislikes });
	}

	// Token: 0x040005E2 RID: 1506
	public int CharIndex;

	// Token: 0x040005E3 RID: 1507
	public int NumberOfCollectables;

	// Token: 0x040005E4 RID: 1508
	public string internalName;

	// Token: 0x040005E5 RID: 1509
	public string CharName;

	// Token: 0x040005E6 RID: 1510
	public DataADexCharImageProvider CharImageProvider;

	// Token: 0x040005E7 RID: 1511
	public string CharObj;

	// Token: 0x040005E8 RID: 1512
	public string VoActor;

	// Token: 0x040005E9 RID: 1513
	public string SpecsAttribute;

	// Token: 0x040005EA RID: 1514
	public string CharLikes;

	// Token: 0x040005EB RID: 1515
	public string CharDislikes;

	// Token: 0x040005EC RID: 1516
	public string CharDYK;

	// Token: 0x040005ED RID: 1517
	public List<Tuple<string, string, string>> Collectable_Names_Desc_Hint;

	// Token: 0x040005EE RID: 1518
	public string Recipe;

	// Token: 0x040005EF RID: 1519
	public List<string> DexComments;

	// Token: 0x040005F0 RID: 1520
	public bool notification;
}
