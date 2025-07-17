using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;

// Token: 0x020001AB RID: 427
public class MusicListBank : BaseListBank
{
	// Token: 0x06000E81 RID: 3713 RVA: 0x0004FDC2 File Offset: 0x0004DFC2
	public void Init(List<MusicEntry> soundtrack)
	{
		this._contents = new List<MusicEntry>(soundtrack);
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0004FDD0 File Offset: 0x0004DFD0
	public override IListContent GetListContent(int index)
	{
		return new MusicListBank.MusicContent
		{
			Value = this._contents[index]
		};
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x0004FDE9 File Offset: 0x0004DFE9
	public override int GetContentCount()
	{
		return this._contents.Count;
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
	public override void OnScroll()
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x04000CE6 RID: 3302
	private List<MusicEntry> _contents;

	// Token: 0x02000388 RID: 904
	public class MusicContent : IListContent
	{
		// Token: 0x040013EC RID: 5100
		public MusicEntry Value;
	}
}
