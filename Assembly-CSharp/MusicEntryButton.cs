using System;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000107 RID: 263
public class MusicEntryButton : ListBox, IListContent
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x000349C0 File Offset: 0x00032BC0
	// (set) Token: 0x060008F3 RID: 2291 RVA: 0x000349C8 File Offset: 0x00032BC8
	public MusicEntry musicEntry { get; set; }

	// Token: 0x060008F4 RID: 2292 RVA: 0x000349D4 File Offset: 0x00032BD4
	protected override void UpdateDisplayContent(IListContent content)
	{
		MusicEntry value = ((MusicListBank.MusicContent)content).Value;
		this.musicEntry = value;
		this.UpdateContent(value);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x000349FC File Offset: 0x00032BFC
	public override void OnBoxMoved(float positionRatio)
	{
		this.positionRatio = positionRatio;
		float num = this.fadeCurve.Evaluate(Mathf.Abs(positionRatio));
		if (this.fadeCanvasGroup != null)
		{
			this.fadeCanvasGroup.alpha = 1f - num;
			return;
		}
		foreach (MusicEntryButton.LerpImageColor lerpImageColor in this.fadeImages)
		{
			lerpImageColor.Image.color = Color.Lerp(lerpImageColor.Color1, lerpImageColor.Color2, num);
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00034AA0 File Offset: 0x00032CA0
	public void UpdateContent(MusicEntry Entry)
	{
		this.numberText.text = Entry.number.ToString();
		this.nameText.text = Entry.title;
		this.index = Entry.number - 1;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x00034AD7 File Offset: 0x00032CD7
	public void UpdateOffset(int index)
	{
	}

	// Token: 0x0400082F RID: 2095
	public int index;

	// Token: 0x04000830 RID: 2096
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000831 RID: 2097
	[SerializeField]
	private TextMeshProUGUI numberText;

	// Token: 0x04000832 RID: 2098
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x04000833 RID: 2099
	[SerializeField]
	private CanvasGroup fadeCanvasGroup;

	// Token: 0x04000834 RID: 2100
	[SerializeField]
	public Image newImage;

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private AnimationCurve fadeCurve;

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private List<MusicEntryButton.LerpImageColor> fadeImages;

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	private RectTransform offset;

	// Token: 0x04000839 RID: 2105
	private float positionRatio;

	// Token: 0x0200031C RID: 796
	[Serializable]
	public struct LerpImageColor
	{
		// Token: 0x04001263 RID: 4707
		public Image Image;

		// Token: 0x04001264 RID: 4708
		public Color Color1;

		// Token: 0x04001265 RID: 4709
		public Color Color2;
	}
}
