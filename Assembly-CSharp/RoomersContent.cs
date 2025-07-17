using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using T17.Services;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class RoomersContent : MonoBehaviour
{
	// Token: 0x06000A4C RID: 2636 RVA: 0x0003B458 File Offset: 0x00039658
	private void OnEnable()
	{
		Save.onGameLoad -= this.SetupUsableRoomers;
		if (this._roomersToLoad == null)
		{
			this._roomersToLoad = new List<RoomerData>();
		}
		else
		{
			this._roomersToLoad.Clear();
		}
		if (Services.SaveGameService.IsBusy)
		{
			Save.onGameLoad += this.SetupUsableRoomers;
			return;
		}
		this.SetupUsableRoomers();
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0003B4BA File Offset: 0x000396BA
	private void OnDisable()
	{
		Save.onGameLoad -= this.SetupUsableRoomers;
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0003B4CD File Offset: 0x000396CD
	private void OnDestroy()
	{
		Save.onGameLoad -= this.SetupUsableRoomers;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x0003B4E0 File Offset: 0x000396E0
	private void SetupUsableRoomers()
	{
		this._usableRoomers = this.ParseRoomerInk();
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x0003B4F0 File Offset: 0x000396F0
	private List<RoomerData> ParseRoomerInk()
	{
		List<RoomerData> list = new List<RoomerData>();
		List<RoomerData> list2 = new List<RoomerData>();
		foreach (string text in Singleton<InkController>.Instance.GetStitchesAtLocation("roomers"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string text2 in Singleton<InkController>.Instance.story.TagsForContentAtPath(text))
			{
				string text3 = text2.Split(": ", StringSplitOptions.None)[0];
				string text4 = text2.Split(": ", StringSplitOptions.None)[1];
				try
				{
					dictionary.Add(text3, text4);
				}
				catch (Exception)
				{
				}
			}
			RoomerData roomerData = new RoomerData();
			roomerData.Title = dictionary["Title"];
			roomerData.DateableObjectName = text.Split("roomer_", StringSplitOptions.None)[1];
			for (int i = 0; i < 3; i++)
			{
				if (dictionary.ContainsKey("Clue" + (i + 1).ToString()))
				{
					if (Regex.IsMatch(dictionary["Clue" + (i + 1).ToString()], "(?i) *TAGS- *"))
					{
						string[] array = Regex.Split(dictionary["Clue" + (i + 1).ToString()], "(?i) *TAGS- *");
						string text5 = array[0];
						if (array.Length > 1)
						{
							string[] array2;
							if (Regex.IsMatch(array[1], " *, *"))
							{
								array2 = Regex.Split(array[1], " *, *");
							}
							else
							{
								array2 = new string[] { array[1] };
							}
							roomerData.Clues[i] = new RoomerData.Clue(text5, Array.Empty<string>())
							{
								InkFlags = array2
							};
						}
					}
					else
					{
						roomerData.Clues[i] = new RoomerData.Clue(dictionary["Clue" + (i + 1).ToString()], Array.Empty<string>());
					}
				}
			}
			if (!roomerData.GetIsAwakened())
			{
				list.Add(roomerData);
			}
			list2.Add(roomerData);
		}
		list2 = list2.OrderBy((RoomerData data) => data.Priority).Reverse<RoomerData>().ToList<RoomerData>();
		list = list.OrderBy((RoomerData data) => data.Priority).Reverse<RoomerData>().ToList<RoomerData>();
		this._allRoomers = list2;
		return list;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0003B7D8 File Offset: 0x000399D8
	public void ShowActiveRoomers()
	{
		this.ClearContent();
		this._usableRoomers = this.ParseRoomerInk();
		this._roomersToLoad.Clear();
		int num = 0;
		while (this._roomersToLoad.Count < 3 && this._usableRoomers.Count > num)
		{
			this._roomersToLoad.Add(this._usableRoomers[num]);
			num++;
		}
		foreach (RoomerData roomerData in this._usableRoomers)
		{
			if (!this._roomersToLoad.Contains(roomerData) && roomerData.HasActiveClue())
			{
				this._roomersToLoad.Add(roomerData);
			}
		}
		foreach (RoomerData roomerData2 in this._roomersToLoad)
		{
			Object.Instantiate<GameObject>(this.RoomerLogItemPrefab, this.Content.transform).GetComponent<RoomerLogItem>().Initialize(roomerData2);
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0003B8FC File Offset: 0x00039AFC
	public void ShowResolvedRoomers()
	{
		this.ClearContent();
		this._usableRoomers = this.ParseRoomerInk();
		this._roomersToLoad.Clear();
		foreach (RoomerData roomerData in this._allRoomers)
		{
			if (roomerData.GetIsAwakened())
			{
				this._roomersToLoad.Add(roomerData);
			}
		}
		foreach (RoomerData roomerData2 in this._roomersToLoad)
		{
			Object.Instantiate<GameObject>(this.RoomerLogItemPrefab, this.Content.transform).GetComponent<RoomerLogItem>().Initialize(roomerData2);
		}
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0003B9D8 File Offset: 0x00039BD8
	public void ClearContent()
	{
		RoomerLogItem[] componentsInChildren = this.Content.GetComponentsInChildren<RoomerLogItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	// Token: 0x04000971 RID: 2417
	private List<RoomerData> _usableRoomers;

	// Token: 0x04000972 RID: 2418
	private List<RoomerData> _allRoomers;

	// Token: 0x04000973 RID: 2419
	private List<RoomerData> _roomersToLoad;

	// Token: 0x04000974 RID: 2420
	private List<RoomerData> _roomersToDelete;

	// Token: 0x04000975 RID: 2421
	public GameObject RoomerLogItemPrefab;

	// Token: 0x04000976 RID: 2422
	public GameObject Content;
}
