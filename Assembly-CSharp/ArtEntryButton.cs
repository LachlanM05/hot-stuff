using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000104 RID: 260
public class ArtEntryButton : ListBox, IListContent, ISelectHandler, IEventSystemHandler
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00034237 File Offset: 0x00032437
	// (set) Token: 0x060008D3 RID: 2259 RVA: 0x0003423F File Offset: 0x0003243F
	public ArtEntry artEntry { get; set; }

	// Token: 0x060008D4 RID: 2260 RVA: 0x00034248 File Offset: 0x00032448
	protected override void UpdateDisplayContent(IListContent content)
	{
		ArtEntry value = ((ArtListBank.ArtContent)content).Value;
		this.artEntry = value;
		this.index = this.artEntry.number - 1;
		this.UpdateContent(value);
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00034284 File Offset: 0x00032484
	public override void OnBoxMoved(float positionRatio)
	{
		this.positionRatio = positionRatio;
		float num = this.fadeCurve.Evaluate(Mathf.Abs(positionRatio));
		if (this.fadeCanvasGroup != null)
		{
			this.fadeCanvasGroup.alpha = 1f - num;
			return;
		}
		foreach (ArtEntryButton.LerpImageColor lerpImageColor in this.fadeImages)
		{
			lerpImageColor.Image.color = Color.Lerp(lerpImageColor.Color1, lerpImageColor.Color2, num);
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00034328 File Offset: 0x00032528
	public void UpdateContent(ArtEntry Entry)
	{
		this.numberText.text = Entry.number.ToString();
		this.nameText.text = Entry.title;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00034354 File Offset: 0x00032554
	public void OnSelect(BaseEventData eventData)
	{
		if (this.leftControl != null && this.rightControl != null)
		{
			Selectable component = base.GetComponent<Selectable>();
			Navigation navigation = this.leftControl.navigation;
			navigation.selectOnUp = null;
			navigation.selectOnDown = null;
			navigation.selectOnLeft = null;
			navigation.selectOnRight = component;
			navigation.mode = Navigation.Mode.Explicit;
			this.leftControl.navigation = navigation;
			navigation = this.rightControl.navigation;
			navigation.selectOnUp = null;
			navigation.selectOnDown = null;
			navigation.selectOnLeft = component;
			navigation.selectOnRight = null;
			this.rightControl.navigation = navigation;
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x000343FF File Offset: 0x000325FF
	public void SetLeftRightControls(Selectable left, Selectable right)
	{
		this.leftControl = left;
		this.rightControl = right;
	}

	// Token: 0x0400080E RID: 2062
	public int index;

	// Token: 0x0400080F RID: 2063
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000810 RID: 2064
	[SerializeField]
	private TextMeshProUGUI numberText;

	// Token: 0x04000811 RID: 2065
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x04000812 RID: 2066
	[SerializeField]
	public Image newImage;

	// Token: 0x04000813 RID: 2067
	[SerializeField]
	private AnimationCurve fadeCurve;

	// Token: 0x04000814 RID: 2068
	[SerializeField]
	private CanvasGroup fadeCanvasGroup;

	// Token: 0x04000815 RID: 2069
	[SerializeField]
	private List<ArtEntryButton.LerpImageColor> fadeImages;

	// Token: 0x04000816 RID: 2070
	[SerializeField]
	private RectTransform offset;

	// Token: 0x04000817 RID: 2071
	private Selectable leftControl;

	// Token: 0x04000818 RID: 2072
	private Selectable rightControl;

	// Token: 0x0400081A RID: 2074
	private float positionRatio;

	// Token: 0x02000319 RID: 793
	[Serializable]
	public struct LerpImageColor
	{
		// Token: 0x0400125B RID: 4699
		public Image Image;

		// Token: 0x0400125C RID: 4700
		public Color Color1;

		// Token: 0x0400125D RID: 4701
		public Color Color2;
	}
}
