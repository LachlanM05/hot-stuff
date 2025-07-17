using System;
using System.Collections;
using System.IO;
using T17.Services;
using Team17.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010B RID: 267
public class DialogBoxBehavior : MonoBehaviour
{
	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000922 RID: 2338 RVA: 0x000356BA File Offset: 0x000338BA
	public bool TextAnimating
	{
		get
		{
			return this.textAnimator.IsAnimating();
		}
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x000356C8 File Offset: 0x000338C8
	public void Awake()
	{
		if (this.narratorBackground != null)
		{
			this.narratorBackground.SetActive(false);
		}
		if (this.defaultBackground != null)
		{
			this.defaultBackground.SetActive(false);
		}
		this.nameText.text = "";
		this.dialogText.text = "";
		this.ClearDialogTextMesh();
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00035730 File Offset: 0x00033930
	private void ClearDialogTextMesh()
	{
		if (this.dialogText)
		{
			try
			{
				this.dialogText.ClearMesh();
			}
			catch (Exception ex)
			{
				T17Debug.LogError("Error clearing dialog text mesh: " + ex.Message);
			}
		}
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00035780 File Offset: 0x00033980
	public void UpdateDialog(string dialogTextIn)
	{
		this.UpdateDialog("", dialogTextIn);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00035790 File Offset: 0x00033990
	public void UpdateDialog(string nameIn, string dialogTextIn)
	{
		string text = nameIn;
		if (nameIn.ToUpperInvariant() == "NARRATOR")
		{
			this.nameText.color = Color.black;
			this.dialogText.color = Color.black;
			this.narratorBackground.SetActive(true);
			this.defaultBackground.SetActive(false);
			this.nameText.text = "";
			this.dialogText.text = "";
			this.ClearDialogTextMesh();
		}
		else
		{
			this.nameText.color = Color.white;
			this.dialogText.color = Color.white;
			this.narratorBackground.SetActive(false);
			this.defaultBackground.SetActive(true);
			string text2 = "";
			if (Singleton<Save>.Instance.TryGetInternalName(nameIn, out text2) && (!(text2 == "volt") || Singleton<Save>.Instance.GetDateStatus("eddie") == RelationshipStatus.Unmet) && (!text2.StartsWith("hank") || Singleton<Save>.Instance.GetDateStatus("hanks") == RelationshipStatus.Unmet) && DateADex.Instance.GetCharIndex(text2) >= 0 && Singleton<Save>.Instance.GetDateStatus(text2) == RelationshipStatus.Unmet)
			{
				nameIn = "???";
			}
			this.nameText.text = nameIn.ToUpperInvariant();
			this.dialogText.text = "";
			this.ClearDialogTextMesh();
		}
		if (text != this.previousSpeakers || !this.characterIcon.activeInHierarchy)
		{
			this.previousSpeakers = text;
			this.SetCharacterIcon(text);
			base.StartCoroutine(this.WriteNameText());
		}
		this.dialogText.text = dialogTextIn;
		base.StartCoroutine(this.WriteText(dialogTextIn));
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00035938 File Offset: 0x00033B38
	private void SetCharacterIcon(string characterName)
	{
		if (characterName == "narrator")
		{
			return;
		}
		characterName = DateADex.Instance.GetInternalName(characterName);
		this.UnloadCharacterIcon();
		this.characterIcon.SetActive(false);
		string text = Path.Combine("Images", "Icons", characterName.ToLowerInvariant() + "_icon");
		Sprite sprite = Services.AssetProviderService.LoadResourceAsset<Sprite>(text, false);
		if (sprite != null)
		{
			this.characterIcon.GetComponent<Image>().sprite = sprite;
			this.characterIcon.SetActive(true);
			this.loadedCharacterIcon = sprite;
			return;
		}
		this.characterIcon.SetActive(false);
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x000359D9 File Offset: 0x00033BD9
	private IEnumerator WriteText(string dialogTextIn)
	{
		this.ToggleNextArrow(false);
		this.dialogText.gameObject.SetActive(false);
		if (this.textTagsAnimator)
		{
			this.textTagsAnimator.Init(dialogTextIn);
		}
		if (!string.IsNullOrWhiteSpace(this.dialogText.text) && Services.GameSettings.GetInt("AnimateText", 1) == 1)
		{
			yield return new WaitForEndOfFrame();
			this.dialogText.gameObject.SetActive(true);
			yield return base.StartCoroutine(this.textAnimator.AnimateTextCoroutine());
		}
		this.ToggleNextArrow(true);
		if (this.textTagsAnimator)
		{
			this.textTagsAnimator.Play();
		}
		yield break;
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x000359EF File Offset: 0x00033BEF
	private IEnumerator WriteNameText()
	{
		yield return base.StartCoroutine(this.nameTextAnimator.AnimateTextCoroutine());
		yield break;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x000359FE File Offset: 0x00033BFE
	public void ToggleOpen(bool flag)
	{
		this.anim.SetBool("Open", flag);
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00035A11 File Offset: 0x00033C11
	public void ClearDialog(bool clearName = true)
	{
		this.dialogText.text = "";
		if (clearName)
		{
			this.nameText.text = "";
			this.UnloadCharacterIcon();
			this.characterIcon.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00035A4D File Offset: 0x00033C4D
	public void ClearDialogInvoked()
	{
		base.Invoke("ClearDialog", 1f);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00035A5F File Offset: 0x00033C5F
	public void OnChoicesDisplayed()
	{
		this.ToggleNextArrow(false);
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00035A68 File Offset: 0x00033C68
	public void SkipTextAnimation()
	{
		if (this.TextAnimating)
		{
			this.textAnimator.Skip();
		}
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00035A7D File Offset: 0x00033C7D
	public void ToggleNextArrow(bool flag)
	{
		this.nextArrow.SetActive(flag);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00035A8B File Offset: 0x00033C8B
	public void UnloadCharacterIcon()
	{
		if (this.loadedCharacterIcon != null)
		{
			this.characterIcon.GetComponent<Image>().sprite = null;
			Services.AssetProviderService.UnloadResourceAsset(this.loadedCharacterIcon);
			this.loadedCharacterIcon = null;
		}
	}

	// Token: 0x04000865 RID: 2149
	[Header("UI Elements")]
	[SerializeField]
	private TextMeshProUGUI dialogText;

	// Token: 0x04000866 RID: 2150
	[SerializeField]
	private DialogTextAnimator textAnimator;

	// Token: 0x04000867 RID: 2151
	[SerializeField]
	private AnimateTMProTextTags textTagsAnimator;

	// Token: 0x04000868 RID: 2152
	[SerializeField]
	private DialogTextAnimator nameTextAnimator;

	// Token: 0x04000869 RID: 2153
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x0400086A RID: 2154
	[SerializeField]
	private Animator anim;

	// Token: 0x0400086B RID: 2155
	[SerializeField]
	private GameObject nextArrow;

	// Token: 0x0400086C RID: 2156
	[SerializeField]
	private GameObject characterIcon;

	// Token: 0x0400086D RID: 2157
	[SerializeField]
	private GameObject narratorBackground;

	// Token: 0x0400086E RID: 2158
	[SerializeField]
	private GameObject defaultBackground;

	// Token: 0x0400086F RID: 2159
	[SerializeField]
	private string previousSpeakers;

	// Token: 0x04000870 RID: 2160
	private Sprite loadedCharacterIcon;
}
