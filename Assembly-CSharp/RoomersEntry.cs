using System;
using System.Collections.Generic;
using System.IO;
using AirFishLab.ScrollingList.ContentManagement;
using T17.Services;
using UnityEngine;

// Token: 0x020000CA RID: 202
[Serializable]
public class RoomersEntry : IListContent
{
	// Token: 0x060006B0 RID: 1712 RVA: 0x0002742C File Offset: 0x0002562C
	public void SetUpEntry(string charName, string questName, string questDescription, string roomName)
	{
		this.CharName = charName;
		this.QuestName = questName;
		this.MetStatus = Singleton<Save>.Instance.GetDateStatus(this.CharName) > RelationshipStatus.Unmet;
		this.internalCharName = Singleton<CharacterHelper>.Instance._characters.GetInternalName(charName.ToLowerInvariant());
		this.RoomName = roomName;
		this.QuestDescription = questDescription;
		this.IconImage = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", this.internalCharName.Replace("?", "").Replace("/", " ") + "_icon"), false);
		this.tips = new List<Save.RoomersTipStruct>();
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x000274E3 File Offset: 0x000256E3
	public void AddTip(Save.RoomersTipStruct tip)
	{
		this.tips.Add(tip);
	}

	// Token: 0x040005F2 RID: 1522
	public string CharName;

	// Token: 0x040005F3 RID: 1523
	public string QuestName;

	// Token: 0x040005F4 RID: 1524
	public string QuestDescription;

	// Token: 0x040005F5 RID: 1525
	public string RoomName;

	// Token: 0x040005F6 RID: 1526
	public bool MetStatus;

	// Token: 0x040005F7 RID: 1527
	public string internalCharName;

	// Token: 0x040005F8 RID: 1528
	public Sprite IconImage;

	// Token: 0x040005F9 RID: 1529
	public List<Save.RoomersTipStruct> tips;
}
