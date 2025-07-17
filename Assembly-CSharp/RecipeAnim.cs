using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BE RID: 190
public class RecipeAnim : Singleton<RecipeAnim>
{
	// Token: 0x060005DA RID: 1498 RVA: 0x0002137C File Offset: 0x0001F57C
	public void AlertObservers(string alert)
	{
		if (alert.Equals("ReleaseEnded"))
		{
			this.ShouldHideOnRelease();
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00021391 File Offset: 0x0001F591
	public void ShouldHideOnRelease()
	{
		base.StopCoroutine(this.startanimCoroutine());
		this.Col.SetActive(false);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x000213AC File Offset: 0x0001F5AC
	public void startanim(int _smartsPoints, int _poisePoints, int _empathyPoints, int _charmPoints, int _sassPoints)
	{
		this.Col.SetActive(true);
		this.smartsPoints = _smartsPoints;
		this.poisePoints = _poisePoints;
		this.empathyPoints = _empathyPoints;
		this.charmPoints = _charmPoints;
		this.sassPoints = _sassPoints;
		base.StartCoroutine(this.startanimCoroutine());
		this.CollectionAnim.Play("RecipeEnter");
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00021407 File Offset: 0x0001F607
	private IEnumerator startanimCoroutine()
	{
		this.ResetRecipeBars();
		yield return null;
		int smartsLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("smarts", true);
		int poiseLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("poise", true);
		int empathyLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("empathy", true);
		int charmLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("charm", true);
		int sassLevel = Singleton<SpecStatMain>.Instance.GetStatLevel("sass", true);
		bool RecipeComplete = DateADex.Instance.CheckRecipeCompletion(smartsLevel, this.smartsPoints, poiseLevel, this.poisePoints, empathyLevel, this.empathyPoints, charmLevel, this.charmPoints, sassLevel, this.sassPoints);
		yield return null;
		this.SetRecipeBar(this.smartsPoints, smartsLevel, this.SmartsBarCurrent, "smarts", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(this.poisePoints, poiseLevel, this.PoiseBarCurrent, "poise", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(this.empathyPoints, empathyLevel, this.EmpathyBarCurrent, "empathy", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(this.charmPoints, charmLevel, this.CharmBarCurrent, "charm", false);
		yield return new WaitForSeconds(0.5f);
		this.SetRecipeBar(this.sassPoints, sassLevel, this.SassBarCurrent, "sass", RecipeComplete);
		yield break;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x00021416 File Offset: 0x0001F616
	private void EndAnimation()
	{
		base.StopCoroutine(this.startanimCoroutine());
		this.Col.SetActive(false);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00021430 File Offset: 0x0001F630
	private void SetRecipeBar(int points, int statLevel, GameObject barCurrent, string namedStat, bool RecipeComplete = false)
	{
		float num = (Mathf.Approximately((float)points, 0f) ? 0f : ((float)points / 100f));
		float num2 = (Mathf.Approximately((float)statLevel, 0f) ? 0f : ((float)statLevel / 100f));
		num = Mathf.Clamp(num, 0.03f, 1f);
		num2 = Mathf.Clamp(num2, 0.03f, 1f);
		RecipeBarColorTrigger component = barCurrent.GetComponent<RecipeBarColorTrigger>();
		Image component2 = barCurrent.GetComponent<Image>();
		component2.DOKill(false);
		component2.fillAmount = 0f;
		if (component != null)
		{
			component.Reset();
		}
		component2.DOFillAmount(num2, 2f).SetEase(Ease.InCubic);
		if (statLevel >= points)
		{
			if (component != null)
			{
				component.SetTargetValue(namedStat, num);
			}
			if (namedStat == "sass" && RecipeComplete)
			{
				if (component != null)
				{
					component.SetTargetSoundTrigger(namedStat, num2, false, true);
					return;
				}
			}
			else if (component != null)
			{
				component.SetTargetSoundTrigger(namedStat, num2, false, false);
				return;
			}
		}
		else if (component != null)
		{
			component.SetTargetSoundTrigger(namedStat, num2, true, false);
		}
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x00021524 File Offset: 0x0001F724
	private void ResetRecipeBars()
	{
		this.SetupRecipeBar(this.SmartsBar, this.SmartsPoints, this.SmartsBarCap, this.smartsPoints, this.SmartsBarCurrent);
		this.SetupRecipeBar(this.PoiseBar, this.PoisePoints, this.PoiseBarCap, this.poisePoints, this.PoiseBarCurrent);
		this.SetupRecipeBar(this.EmpathyBar, this.EmpathyPoints, this.EmpathyBarCap, this.empathyPoints, this.EmpathyBarCurrent);
		this.SetupRecipeBar(this.CharmBar, this.CharmPoints, this.CharmBarCap, this.charmPoints, this.CharmBarCurrent);
		this.SetupRecipeBar(this.SassBar, this.SassPoints, this.SassBarCap, this.sassPoints, this.SassBarCurrent);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x000215E8 File Offset: 0x0001F7E8
	private void SetupRecipeBar(GameObject barBG, GameObject percentageText, GameObject cap, int points, GameObject barCurrent)
	{
		float num = (Mathf.Approximately((float)points, 0f) ? 0f : ((float)points / 100f));
		num = Mathf.Clamp(num, 0.03f, 1f);
		RecipeBarColorTrigger component = barCurrent.GetComponent<RecipeBarColorTrigger>();
		Image component2 = barCurrent.GetComponent<Image>();
		RectTransform component3 = cap.GetComponent<RectTransform>();
		Vector2 anchorMax = component3.anchorMax;
		Vector2 anchorMin = component3.anchorMin;
		anchorMax.y = Mathf.Lerp(0.035f, 0.965f, num);
		anchorMin.y = anchorMax.y;
		component3.anchorMax = anchorMax;
		component3.anchorMin = anchorMin;
		component3.anchoredPosition = Vector2.zero;
		barBG.GetComponent<Image>().fillAmount = num;
		percentageText.GetComponent<TextMeshProUGUI>().SetText(string.Format("{0}", points), true);
		component2.DOKill(false);
		component2.fillAmount = 0f;
		if (component == null)
		{
			return;
		}
		component.Reset();
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x000216C9 File Offset: 0x0001F8C9
	public void stopanim()
	{
		base.StopCoroutine(this.startanimCoroutine());
		this.Col.SetActive(false);
	}

	// Token: 0x04000590 RID: 1424
	public const string k_AnimRegular = "RecipeEnter";

	// Token: 0x04000591 RID: 1425
	public GameObject Col;

	// Token: 0x04000592 RID: 1426
	public Animator CollectionAnim;

	// Token: 0x04000593 RID: 1427
	[Header("Recipe")]
	public GameObject SmartsBar;

	// Token: 0x04000594 RID: 1428
	public GameObject SmartsBarCurrent;

	// Token: 0x04000595 RID: 1429
	public GameObject SmartsBarCap;

	// Token: 0x04000596 RID: 1430
	public GameObject SmartsPoints;

	// Token: 0x04000597 RID: 1431
	public GameObject PoiseBar;

	// Token: 0x04000598 RID: 1432
	public GameObject PoiseBarCurrent;

	// Token: 0x04000599 RID: 1433
	public GameObject PoiseBarCap;

	// Token: 0x0400059A RID: 1434
	public GameObject PoisePoints;

	// Token: 0x0400059B RID: 1435
	public GameObject EmpathyBar;

	// Token: 0x0400059C RID: 1436
	public GameObject EmpathyBarCurrent;

	// Token: 0x0400059D RID: 1437
	public GameObject EmpathyBarCap;

	// Token: 0x0400059E RID: 1438
	public GameObject EmpathyPoints;

	// Token: 0x0400059F RID: 1439
	public GameObject CharmBar;

	// Token: 0x040005A0 RID: 1440
	public GameObject CharmBarCurrent;

	// Token: 0x040005A1 RID: 1441
	public GameObject CharmBarCap;

	// Token: 0x040005A2 RID: 1442
	public GameObject CharmPoints;

	// Token: 0x040005A3 RID: 1443
	public GameObject SassBar;

	// Token: 0x040005A4 RID: 1444
	public GameObject SassBarCurrent;

	// Token: 0x040005A5 RID: 1445
	public GameObject SassBarCap;

	// Token: 0x040005A6 RID: 1446
	public GameObject SassPoints;

	// Token: 0x040005A7 RID: 1447
	private int smartsPoints;

	// Token: 0x040005A8 RID: 1448
	private int poisePoints;

	// Token: 0x040005A9 RID: 1449
	private int empathyPoints;

	// Token: 0x040005AA RID: 1450
	private int charmPoints;

	// Token: 0x040005AB RID: 1451
	private int sassPoints;
}
