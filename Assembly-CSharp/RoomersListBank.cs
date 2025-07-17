using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class RoomersListBank : BaseListBank
{
	// Token: 0x060008BE RID: 2238 RVA: 0x00033C9F File Offset: 0x00031E9F
	public void Init(List<RoomersEntryButton> roomersEntries)
	{
		this._contents = new List<RoomersEntryButton>(roomersEntries);
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00033CAD File Offset: 0x00031EAD
	public override IListContent GetListContent(int index)
	{
		return this._contents[index];
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00033CBB File Offset: 0x00031EBB
	public override int GetContentCount()
	{
		return this._contents.Count;
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00033CC8 File Offset: 0x00031EC8
	public override void OnScroll()
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x040007FE RID: 2046
	[SerializeField]
	private List<RoomersEntryButton> _contents;
}
