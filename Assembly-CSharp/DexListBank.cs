using System;
using System.Collections.Generic;
using System.Linq;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;

// Token: 0x020000F7 RID: 247
public class DexListBank : BaseListBank
{
	// Token: 0x06000882 RID: 2178 RVA: 0x00032A14 File Offset: 0x00030C14
	public void Init(List<DateADexEntry> dexEntries)
	{
		this._contents = new List<DateADexEntry>(dexEntries);
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00032A22 File Offset: 0x00030C22
	public bool IsInitiated()
	{
		return this._contents != null;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00032A2D File Offset: 0x00030C2D
	public DexListBank.SortMethod GetSortMethod()
	{
		return this.currentSort;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00032A35 File Offset: 0x00030C35
	public override IListContent GetListContent(int index)
	{
		return this._contents[index];
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00032A43 File Offset: 0x00030C43
	public override int GetContentCount()
	{
		return this._contents.Count;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00032A50 File Offset: 0x00030C50
	public int GetListIndexForCharacterIndex(int characterIndex)
	{
		for (int i = 0; i < this._contents.Count; i++)
		{
			if (this._contents[i].CharIndex == characterIndex)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00032A8C File Offset: 0x00030C8C
	public void SetNotificationForCharacter(string internalName)
	{
		if (this._contents != null)
		{
			for (int i = 0; i < this._contents.Count; i++)
			{
				if (this._contents[i].internalName == internalName)
				{
					this._contents[i].notification = true;
					return;
				}
			}
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00032AE4 File Offset: 0x00030CE4
	public void SortBy(DexListBank.SortMethod sort)
	{
		if (this._contents != null)
		{
			switch (sort)
			{
			case DexListBank.SortMethod.New:
				this._contents = (from x in this._contents
					orderby x.CharIndex
					orderby x.notification descending
					orderby x.isAwakened descending
					select x).ToList<DateADexEntry>();
				return;
			case DexListBank.SortMethod.Status:
			{
				List<DateADexEntry> list = this._contents.OrderBy((DateADexEntry x) => x.CharIndex).ToList<DateADexEntry>();
				List<DateADexEntry> list2 = list.FindAll((DateADexEntry x) => Singleton<Save>.Instance.GetDateStatus(x.internalName) == RelationshipStatus.Love && Singleton<Save>.Instance.GetDateStatusRealized(x.internalName) != RelationshipStatus.Realized).ToList<DateADexEntry>();
				List<DateADexEntry> list3 = list.FindAll((DateADexEntry x) => Singleton<Save>.Instance.GetDateStatus(x.internalName) == RelationshipStatus.Friend && Singleton<Save>.Instance.GetDateStatusRealized(x.internalName) != RelationshipStatus.Realized).ToList<DateADexEntry>();
				List<DateADexEntry> list4 = list.FindAll((DateADexEntry x) => Singleton<Save>.Instance.GetDateStatus(x.internalName) == RelationshipStatus.Hate && Singleton<Save>.Instance.GetDateStatusRealized(x.internalName) != RelationshipStatus.Realized).ToList<DateADexEntry>();
				List<DateADexEntry> list5 = list.FindAll((DateADexEntry x) => Singleton<Save>.Instance.GetDateStatus(x.internalName) == RelationshipStatus.Single && Singleton<Save>.Instance.GetDateStatusRealized(x.internalName) != RelationshipStatus.Realized).ToList<DateADexEntry>();
				List<DateADexEntry> list6 = list.FindAll((DateADexEntry x) => Singleton<Save>.Instance.GetDateStatus(x.internalName) == RelationshipStatus.Unmet && Singleton<Save>.Instance.GetDateStatusRealized(x.internalName) != RelationshipStatus.Realized).ToList<DateADexEntry>();
				List<DateADexEntry> list7 = list.FindAll((DateADexEntry x) => Singleton<Save>.Instance.GetDateStatusRealized(x.internalName) == RelationshipStatus.Realized).ToList<DateADexEntry>();
				this._contents = list2;
				this._contents.AddRange(list3);
				this._contents.AddRange(list4);
				this._contents.AddRange(list5);
				this._contents.AddRange(list6);
				this._contents.AddRange(list7);
				return;
			}
			case DexListBank.SortMethod.Alphabetical:
				this._contents = this._contents.OrderBy((DateADexEntry x) => x.CharName).ToList<DateADexEntry>();
				return;
			}
			this._contents = this._contents.OrderBy((DateADexEntry x) => x.CharIndex).ToList<DateADexEntry>();
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00032D78 File Offset: 0x00030F78
	public DexListBank.SortMethod NextSort()
	{
		this.currentSort++;
		if (this.currentSort >= (DexListBank.SortMethod)Enum.GetNames(typeof(DexListBank.SortMethod)).Length)
		{
			this.currentSort = DexListBank.SortMethod.Index;
		}
		this.SortBy(this.currentSort);
		return this.currentSort;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00032DC5 File Offset: 0x00030FC5
	public void ResetSort()
	{
		this.currentSort = DexListBank.SortMethod.Index;
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00032DD0 File Offset: 0x00030FD0
	public override void OnScroll()
	{
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_scroll, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x040007D4 RID: 2004
	private List<DateADexEntry> _contents;

	// Token: 0x040007D5 RID: 2005
	private DexListBank.SortMethod currentSort;

	// Token: 0x02000313 RID: 787
	public enum SortMethod
	{
		// Token: 0x0400123C RID: 4668
		Index,
		// Token: 0x0400123D RID: 4669
		New,
		// Token: 0x0400123E RID: 4670
		Status,
		// Token: 0x0400123F RID: 4671
		Alphabetical
	}
}
