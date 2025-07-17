using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x0200012A RID: 298
public class RoomerGalleryButton : MonoBehaviour
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x0003B27C File Offset: 0x0003947C
	public void Setup(int _index, bool _found)
	{
		this.index = _index;
		this.found = _found;
		this.Deselect();
		this.button.onClick.AddListener(new UnityAction(this.SwitchToIndex));
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0003B2AE File Offset: 0x000394AE
	public void Select()
	{
		this.image.sprite = this.selectedSprite;
		RectTransform rectTransform = base.transform as RectTransform;
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70f);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70f);
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0003B2E4 File Offset: 0x000394E4
	public void Deselect()
	{
		this.image.sprite = (this.found ? this.notFoundSprite : this.isFoundSprite);
		RectTransform rectTransform = base.transform as RectTransform;
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50f);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50f);
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x0003B334 File Offset: 0x00039534
	private void SwitchToIndex()
	{
		Roomers.Instance.switchEntryIndex(this.index, false);
	}

	// Token: 0x04000963 RID: 2403
	[Header("Status")]
	public int index;

	// Token: 0x04000964 RID: 2404
	[FormerlySerializedAs("discovered")]
	public bool found;

	// Token: 0x04000965 RID: 2405
	[Header("References")]
	[SerializeField]
	private Image image;

	// Token: 0x04000966 RID: 2406
	[Header("Sprites")]
	[SerializeField]
	private Button button;

	// Token: 0x04000967 RID: 2407
	[SerializeField]
	private Sprite selectedSprite;

	// Token: 0x04000968 RID: 2408
	[FormerlySerializedAs("completedSprite")]
	[SerializeField]
	private Sprite isFoundSprite;

	// Token: 0x04000969 RID: 2409
	[FormerlySerializedAs("notCompletedSprite")]
	[SerializeField]
	private Sprite notFoundSprite;
}
