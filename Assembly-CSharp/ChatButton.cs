using System;
using System.IO;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200014E RID: 334
public class ChatButton : MonoBehaviour
{
	// Token: 0x06000C61 RID: 3169 RVA: 0x00046DE4 File Offset: 0x00044FE4
	public void SetSelected(bool status)
	{
		if (this.SelectSymbol != null)
		{
			this.SelectSymbol.SetActive(status);
		}
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00046E00 File Offset: 0x00045000
	public void SetNewMessages(bool value)
	{
		this.HasMessages.SetActive(value);
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00046E10 File Offset: 0x00045010
	public Sprite GetCharacterIcon()
	{
		string text = this.CharacterName.text.Trim();
		if (text == "David Most")
		{
			return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "pfp_david"), false);
		}
		if (text == "foil" || text == "tinfoilhat")
		{
			return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "pfp_foil"), false);
		}
		if (text == "Val 9000")
		{
			return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "pfp_val"), false);
		}
		if (text == "Tom")
		{
			return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "pfp_tom"), false);
		}
		if (!(text == "Sam"))
		{
			return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "pfp_default"), false);
		}
		return Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "UI", "Thiscord", "pfp_sam"), false);
	}

	// Token: 0x04000B13 RID: 2835
	public Button button;

	// Token: 0x04000B14 RID: 2836
	public string NodePrefix;

	// Token: 0x04000B15 RID: 2837
	public TextMeshProUGUI CharacterName;

	// Token: 0x04000B16 RID: 2838
	public GameObject ActiveBacking;

	// Token: 0x04000B17 RID: 2839
	public GameObject HasMessages;

	// Token: 0x04000B18 RID: 2840
	public Image Icon;

	// Token: 0x04000B19 RID: 2841
	public GameObject SelectSymbol;
}
