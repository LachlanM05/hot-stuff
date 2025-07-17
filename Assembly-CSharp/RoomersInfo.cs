using System;
using System.IO;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200016A RID: 362
public class RoomersInfo : MonoBehaviour
{
	// Token: 0x06000D07 RID: 3335 RVA: 0x0004B0FA File Offset: 0x000492FA
	private void Start()
	{
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x0004B0FC File Offset: 0x000492FC
	private void Update()
	{
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x0004B100 File Offset: 0x00049300
	public void ClearPanel()
	{
		this.RoomersTitle.text = "";
		this.RoomersDescription.text = "";
		this.CharacterName.text = "";
		this.RoomName.text = "";
		this.BottomIcon.sprite = null;
		this.BottomImage.SetActive(false);
		for (int i = this.TipContainer.transform.childCount - 1; i >= 0; i--)
		{
			Object.Destroy(this.TipContainer.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x0004B1A0 File Offset: 0x000493A0
	public void SetUpPanel(Save.RoomersStruct roomer)
	{
		this.RoomersTitle.text = roomer.questName.ToUpperInvariant();
		this.RoomersDescription.text = roomer.description;
		this.CharacterName.text = roomer.character;
		roomer.hasNew = false;
		DateADexEntry dateADexEntry = DateADex.Instance.GetEntry(roomer.character);
		if (roomer.character == "curt" && dateADexEntry == null)
		{
			this.CharacterName.text = "Curt & Rod";
			dateADexEntry = DateADex.Instance.GetEntry("curtrod");
		}
		this.RoomName.text = dateADexEntry.CharObj;
		this.BottomIcon.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", roomer.character + "_icon"), this.BottomIcon.sprite);
		if (this.BottomIcon.sprite == null)
		{
			string text = "";
			if (Singleton<Save>.Instance.TryGetNameByInternalName(roomer.character, out text))
			{
				this.BottomIcon.sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(Path.Combine("Images", "Icons", text + "_icon"), false);
			}
		}
		this.BottomImage.SetActive(roomer.isFound);
		for (int i = this.TipContainer.transform.childCount - 1; i >= 0; i--)
		{
			Object.Destroy(this.TipContainer.transform.GetChild(i).gameObject);
		}
		if (roomer.skylarTipIsFound && roomer.skylar != "")
		{
			this.AddTip("Skylar", roomer.skylar);
			return;
		}
		foreach (Save.RoomersTipStruct roomersTipStruct in roomer.tips)
		{
			if (roomersTipStruct.isFound && roomersTipStruct.tipInfo != "")
			{
				roomersTipStruct.tipNameAfterValidation = roomersTipStruct.tipName;
				roomersTipStruct.tipInfoAfterValidation = roomersTipStruct.tipInfo;
				this.AddTip(roomersTipStruct.tipNameAfterValidation, roomersTipStruct.tipInfoAfterValidation);
			}
		}
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x0004B3E8 File Offset: 0x000495E8
	public void AddTip(string title, string info)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.TipPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		gameObject.transform.parent = this.TipContainer.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.GetComponent<RoomersTip>().SetUpTip(title, info);
	}

	// Token: 0x04000BCA RID: 3018
	public TextMeshProUGUI RoomersTitle;

	// Token: 0x04000BCB RID: 3019
	public TextMeshProUGUI RoomersDescription;

	// Token: 0x04000BCC RID: 3020
	public GameObject TipContainer;

	// Token: 0x04000BCD RID: 3021
	public GameObject BottomImage;

	// Token: 0x04000BCE RID: 3022
	public Image BottomIcon;

	// Token: 0x04000BCF RID: 3023
	public TextMeshProUGUI CharacterName;

	// Token: 0x04000BD0 RID: 3024
	public TextMeshProUGUI RoomName;

	// Token: 0x04000BD1 RID: 3025
	public GameObject TipPrefab;
}
