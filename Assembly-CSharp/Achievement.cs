using System;
using Team17.Platform.Achievements;
using Team17.Platform.Achievements.Steam;
using UnityEngine;

// Token: 0x020000C3 RID: 195
[CreateAssetMenu(fileName = "Achievement", menuName = "ScriptableObjects/Achievement", order = 13)]
[Serializable]
public class Achievement : ScriptableObject, ISteamAchievementID, IAchievementID, ISteamProgressID, IProgressID
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000693 RID: 1683 RVA: 0x000245D2 File Offset: 0x000227D2
	string ISteamAchievementID.ID
	{
		get
		{
			return this.m_SteamID;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x000245DA File Offset: 0x000227DA
	string ISteamProgressID.ID
	{
		get
		{
			return this.m_SteamStatID;
		}
	}

	// Token: 0x040005CB RID: 1483
	[SerializeField]
	public AchievementId achievementId;

	// Token: 0x040005CC RID: 1484
	[SerializeField]
	public string title;

	// Token: 0x040005CD RID: 1485
	[SerializeField]
	public string displayText;

	// Token: 0x040005CE RID: 1486
	[SerializeField]
	public string mainInstruction;

	// Token: 0x040005CF RID: 1487
	[SerializeField]
	public string mainRequirement;

	// Token: 0x040005D0 RID: 1488
	[SerializeField]
	public Sprite sprite;

	// Token: 0x040005D1 RID: 1489
	[Header("Platform Data")]
	[Header("Steam")]
	[SerializeField]
	private string m_SteamID;

	// Token: 0x040005D2 RID: 1490
	[SerializeField]
	private string m_SteamStatID;

	// Token: 0x040005D3 RID: 1491
	[Header("Playstation")]
	[SerializeField]
	private int m_Playstation5ID;

	// Token: 0x040005D4 RID: 1492
	[Header("GameCore")]
	[SerializeField]
	private string m_GameCoreID;
}
