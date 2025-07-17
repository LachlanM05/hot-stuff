using System;
using TMPro;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class RoomerLogItem : MonoBehaviour
{
	// Token: 0x06000A47 RID: 2631 RVA: 0x0003B350 File Offset: 0x00039550
	public void Initialize(RoomerData data)
	{
		this.Data = data;
		this.TitleText.text = this.Data.Title;
		this.Clue1Text.text = this.Data.Clues[0].Description;
		this.Clue2Text.text = this.Data.Clues[1].Description;
		this.Clue3Text.text = this.Data.Clues[2].Description;
		this.Clue2CensorBar.SetActive(!this.Data.Clues[1].IsRevealed() && !this.Data.GetIsAwakened());
		this.Clue3CensorBar.SetActive(!this.Data.Clues[2].IsRevealed() && !this.Data.GetIsAwakened());
	}

	// Token: 0x0400096A RID: 2410
	public RoomerData Data;

	// Token: 0x0400096B RID: 2411
	public TextMeshProUGUI TitleText;

	// Token: 0x0400096C RID: 2412
	public TextMeshProUGUI Clue1Text;

	// Token: 0x0400096D RID: 2413
	public TextMeshProUGUI Clue2Text;

	// Token: 0x0400096E RID: 2414
	public TextMeshProUGUI Clue3Text;

	// Token: 0x0400096F RID: 2415
	public GameObject Clue2CensorBar;

	// Token: 0x04000970 RID: 2416
	public GameObject Clue3CensorBar;
}
