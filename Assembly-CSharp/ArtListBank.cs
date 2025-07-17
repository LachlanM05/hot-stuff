using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class ArtListBank : BaseListBank
{
	// Token: 0x06000E18 RID: 3608 RVA: 0x0004E40A File Offset: 0x0004C60A
	public void Init(List<ArtEntry> artList)
	{
		this._contents = new List<ArtEntry>(artList);
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0004E418 File Offset: 0x0004C618
	public override IListContent GetListContent(int index)
	{
		return new ArtListBank.ArtContent
		{
			Value = this._contents[index]
		};
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x0004E431 File Offset: 0x0004C631
	public override int GetContentCount()
	{
		return this._contents.Count;
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x0004E440 File Offset: 0x0004C640
	public override void OnScroll()
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x04000C89 RID: 3209
	[SerializeField]
	private List<ArtEntry> _contents;

	// Token: 0x0200037F RID: 895
	public class ArtContent : IListContent
	{
		// Token: 0x040013C8 RID: 5064
		public ArtEntry Value;
	}
}
