using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010C RID: 268
[ExecuteInEditMode]
public class DialogBoxBehavior_OLD : MonoBehaviour
{
	// Token: 0x06000932 RID: 2354 RVA: 0x00035ACC File Offset: 0x00033CCC
	private void Start()
	{
		this.textAnimator = new AnimateTMProVertex[this.dialogText.Length];
		for (int i = 0; i < this.dialogText.Length; i++)
		{
			this.textAnimator[i] = this.dialogText[i].GetComponent<AnimateTMProVertex>();
		}
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00035B14 File Offset: 0x00033D14
	private void Update()
	{
		float num = this.boxHeight;
		if (this.boxMat.HasProperty("_HeightScale") && !Mathf.Approximately(num, this.boxMat.GetFloat("_HeightScale")))
		{
			this.boxMat.SetFloat("_HeightScale", num);
			this.boxMatFront.SetFloat("_HeightScale", num);
		}
		float num2 = 1f - this.shadowThickness;
		if (this.boxMat.HasProperty("_ShadowThickness") && !Mathf.Approximately(num2, this.boxMat.GetFloat("_ShadowThickness")))
		{
			this.boxMat.SetFloat("_ShadowThickness", num2);
			this.nameTagShadow.effectDistance = new Vector2(-17.1f * this.shadowThickness, -11.62f * this.shadowThickness);
		}
		if (this.boxMat.HasProperty("_FillColor") && !object.Equals(this.fillColor, this.boxMat.GetColor("_FillColor")))
		{
			this.boxMat.SetColor("_FillColor", this.fillColor);
		}
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00035C33 File Offset: 0x00033E33
	private IEnumerator UpdateDialogBoxHeight()
	{
		yield return new WaitForEndOfFrame();
		this.numLines = this.dialogText[0].textInfo.lineCount;
		this.anim.SetInteger("NumLines", this.numLines);
		this.cachedText = this.dialogText[0].text;
		yield break;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00035C44 File Offset: 0x00033E44
	private void UpdateDialogBoxMode(string modeIn)
	{
		if (modeIn == "player")
		{
			if (!this.anim.GetCurrentAnimatorStateInfo(1).IsName("Player Speaking"))
			{
				this.anim.SetTrigger("SpeakerIsPlayer");
				return;
			}
		}
		else if (!this.anim.GetCurrentAnimatorStateInfo(1).IsName("NPC Speaking"))
		{
			this.anim.SetTrigger("SpeakerIsNPC");
		}
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00035CB5 File Offset: 0x00033EB5
	public void ToggleOpen(bool flag)
	{
		if (flag)
		{
			this.anim.SetBool("Open", true);
			return;
		}
		this.anim.SetBool("Open", false);
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x00035CDD File Offset: 0x00033EDD
	public void SetText(string dialogTextIn)
	{
		this.dialogText[0].text = dialogTextIn;
		this.UpdateDialogBoxMode("player");
		base.StartCoroutine(this.UpdateDialogBoxHeight());
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00035D08 File Offset: 0x00033F08
	public void SetText(string nameIn, string dialogTextIn)
	{
		this.nameText.text = nameIn;
		this.dialogText[0].text = dialogTextIn;
		this.dialogText[0].transform.parent.gameObject.SetActive(false);
		this.dialogText[0].transform.parent.gameObject.SetActive(true);
		this.UpdateDialogBoxMode("NPC");
		base.StartCoroutine(this.UpdateDialogBoxHeight());
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00035D81 File Offset: 0x00033F81
	public void SetChoices()
	{
		this.dialogText[0].text = "";
		this.UpdateDialogBoxMode("player");
		this.anim.SetInteger("NumLines", 4);
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00035DB1 File Offset: 0x00033FB1
	public void SkipTextAnimation()
	{
		if (this.animatingText)
		{
			this.skippingText = true;
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00035DC4 File Offset: 0x00033FC4
	private void StartTextAnimation()
	{
		float num = (this.anim.GetCurrentAnimatorStateInfo(0).IsName("DialogBoxHeightIntro") ? 0.25f : 0f);
		for (int i = 0; i < this.textAnimator.Length; i++)
		{
			num += (float)this.dialogText[i].textInfo.characterCount * this.textAnimator[i].duration + (float)this.dialogText[i].textInfo.characterCount * this.textAnimator[i].letterDelay;
		}
	}

	// Token: 0x04000871 RID: 2161
	private int numLines;

	// Token: 0x04000872 RID: 2162
	[Header("Settings")]
	[SerializeField]
	private Material boxMat;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	private Material boxMatFront;

	// Token: 0x04000874 RID: 2164
	[SerializeField]
	private bool choicesClearDialog = true;

	// Token: 0x04000875 RID: 2165
	public bool matchHeightToText;

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	private float[] dialogBoxHeight;

	// Token: 0x04000877 RID: 2167
	[Header("UI Elements")]
	[Range(0f, 0.725f)]
	public float boxHeight;

	// Token: 0x04000878 RID: 2168
	[Range(0f, 1f)]
	public float shadowThickness;

	// Token: 0x04000879 RID: 2169
	public Color fillColor = new Color(0.2f, 0.2196078f, 0.345098f, 0.9019f);

	// Token: 0x0400087A RID: 2170
	[SerializeField]
	private Shadow nameTagShadow;

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	private Animator anim;

	// Token: 0x0400087C RID: 2172
	[Header("UI Content")]
	[SerializeField]
	private TextMeshProUGUI[] dialogText;

	// Token: 0x0400087D RID: 2173
	private AnimateTMProVertex[] textAnimator;

	// Token: 0x0400087E RID: 2174
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x0400087F RID: 2175
	private string cachedText;

	// Token: 0x04000880 RID: 2176
	private bool skippingText;

	// Token: 0x04000881 RID: 2177
	public bool animatingText;
}
