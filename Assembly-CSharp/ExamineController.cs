using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000067 RID: 103
public class ExamineController : Singleton<ExamineController>
{
	// Token: 0x0600036B RID: 875 RVA: 0x000162EC File Offset: 0x000144EC
	public void ShowExamine(string text)
	{
		if (this.isShown)
		{
			this.HideExamine();
			this.isShown = false;
			return;
		}
		this.ExamineGameObject.SetActive(true);
		this.ExamineText.SetText(text, true);
		base.StartCoroutine(this.WaitForEndOfFrame());
		this.isShown = true;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0001633C File Offset: 0x0001453C
	public void HideExamine()
	{
		this.isShown = false;
		if (this.ExamineGameObject == null)
		{
			return;
		}
		this.ExamineGameObject.SetActive(false);
		this.examineRect.anchoredPosition = new Vector2(0f, 225.5f);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0001637A File Offset: 0x0001457A
	private IEnumerator WaitForEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		LayoutRebuilder.MarkLayoutForRebuild(this.textRect);
		yield break;
	}

	// Token: 0x04000369 RID: 873
	public GameObject ExamineGameObject;

	// Token: 0x0400036A RID: 874
	public TextMeshProUGUI ExamineText;

	// Token: 0x0400036B RID: 875
	[SerializeField]
	private RectTransform textRect;

	// Token: 0x0400036C RID: 876
	[SerializeField]
	private RectTransform examineRect;

	// Token: 0x0400036D RID: 877
	public bool isShown;
}
