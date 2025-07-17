using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x02000138 RID: 312
public class SpecGlossaryBlock : MonoBehaviour
{
	// Token: 0x06000B0E RID: 2830 RVA: 0x0003F624 File Offset: 0x0003D824
	public void Init(string name, string description)
	{
		this.NameFirstLetter.text = name.Substring(0, 1);
		this.NameRest.text = name.Substring(1).ToUpperInvariant();
		this.icon.sprite = this.iconAtlas.GetSprite(name.ToLowerInvariant().Trim() + "_icon");
		this.descriptionText.text = description;
	}

	// Token: 0x04000A0D RID: 2573
	[SerializeField]
	private TextMeshProUGUI NameFirstLetter;

	// Token: 0x04000A0E RID: 2574
	[SerializeField]
	private TextMeshProUGUI NameRest;

	// Token: 0x04000A0F RID: 2575
	[SerializeField]
	private SpriteAtlas iconAtlas;

	// Token: 0x04000A10 RID: 2576
	[SerializeField]
	private Image icon;

	// Token: 0x04000A11 RID: 2577
	[SerializeField]
	private TextMeshProUGUI descriptionText;
}
