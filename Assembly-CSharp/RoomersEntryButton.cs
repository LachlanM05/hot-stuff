using System;
using System.Collections.Generic;
using System.IO;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000FA RID: 250
public class RoomersEntryButton : ListBox, IListContent
{
	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000339AA File Offset: 0x00031BAA
	// (set) Token: 0x060008B5 RID: 2229 RVA: 0x000339B2 File Offset: 0x00031BB2
	public Save.RoomersStruct roomersEntry { get; private set; }

	// Token: 0x060008B6 RID: 2230 RVA: 0x000339BC File Offset: 0x00031BBC
	protected override void UpdateDisplayContent(IListContent content)
	{
		Save.RoomersStruct roomersStruct = (Save.RoomersStruct)content;
		this.roomersEntry = roomersStruct;
		this.nameText.text = roomersStruct.questName.ToUpperInvariant();
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", roomersStruct.character + "_icon"), false);
		if (sprite == null)
		{
			string text = "";
			if (Singleton<Save>.Instance.TryGetNameByInternalName(roomersStruct.character, out text))
			{
				sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", text + "_icon"), false);
			}
		}
		this.UnloadSprite();
		this.loadedSprite = sprite;
		this.iconImage.sprite = sprite;
		this.MetEntry = roomersStruct.isFound;
		if (this.MetEntry)
		{
			this.iconImage.gameObject.SetActive(true);
			this.iconImageNotFound.gameObject.SetActive(false);
			return;
		}
		this.iconImage.gameObject.SetActive(false);
		this.iconImageNotFound.gameObject.SetActive(true);
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00033AD4 File Offset: 0x00031CD4
	private void UnloadSprite()
	{
		if (this.loadedSprite != null)
		{
			if (this.iconImage != null)
			{
				this.iconImage.sprite = null;
			}
			Services.AssetProviderService.UnloadResourceAsset(this.loadedSprite);
			this.loadedSprite = null;
		}
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00033B20 File Offset: 0x00031D20
	private void OnDisable()
	{
		this.UnloadSprite();
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x00033B28 File Offset: 0x00031D28
	public override void OnBoxMoved(float positionRatio)
	{
		this.positionRatio = positionRatio;
		float num = this.fadeCurve.Evaluate(Mathf.Abs(positionRatio));
		foreach (RoomersEntryButton.LerpImageColor lerpImageColor in this.fadeImages)
		{
			lerpImageColor.Image.color = Color.Lerp(lerpImageColor.Color1, lerpImageColor.Color2, num);
		}
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00033BAC File Offset: 0x00031DAC
	public void UpdateContent(Save.RoomersStruct Entry)
	{
		this.nameText.text = Entry.questName.ToUpperInvariant();
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", Entry.character + "_icon"), false);
		this.UnloadSprite();
		this.loadedSprite = sprite;
		this.iconImage.sprite = sprite;
		this.MetEntry = Entry.isFound;
		this.roomersEntry = Entry;
		if (this.MetEntry)
		{
			this.iconImage.gameObject.SetActive(true);
			this.iconImageNotFound.gameObject.SetActive(false);
			return;
		}
		this.iconImage.gameObject.SetActive(false);
		this.iconImageNotFound.gameObject.SetActive(true);
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00033C73 File Offset: 0x00031E73
	public void UpdateOffset(int index)
	{
		this.offset.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(-540 + 6 * index));
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00033C8B File Offset: 0x00031E8B
	public void OnChangedToControllerInputTypeWhenSelected()
	{
		Roomers.Instance.SelectCurrentEntryButton();
	}

	// Token: 0x040007F1 RID: 2033
	public int index;

	// Token: 0x040007F2 RID: 2034
	[SerializeField]
	private Image iconImage;

	// Token: 0x040007F3 RID: 2035
	[SerializeField]
	private Image iconImageNotFound;

	// Token: 0x040007F4 RID: 2036
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x040007F5 RID: 2037
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x040007F6 RID: 2038
	[SerializeField]
	public Image newImage;

	// Token: 0x040007F7 RID: 2039
	[SerializeField]
	private AnimationCurve fadeCurve;

	// Token: 0x040007F8 RID: 2040
	[SerializeField]
	private List<RoomersEntryButton.LerpImageColor> fadeImages;

	// Token: 0x040007F9 RID: 2041
	[SerializeField]
	private RectTransform offset;

	// Token: 0x040007FB RID: 2043
	public bool MetEntry;

	// Token: 0x040007FC RID: 2044
	private float positionRatio;

	// Token: 0x040007FD RID: 2045
	private Sprite loadedSprite;

	// Token: 0x02000318 RID: 792
	[Serializable]
	public struct LerpImageColor
	{
		// Token: 0x04001258 RID: 4696
		public Image Image;

		// Token: 0x04001259 RID: 4697
		public Color Color1;

		// Token: 0x0400125A RID: 4698
		public Color Color2;
	}
}
