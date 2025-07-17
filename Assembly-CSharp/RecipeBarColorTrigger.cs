using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000126 RID: 294
public class RecipeBarColorTrigger : MonoBehaviour
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x00039F39 File Offset: 0x00038139
	private void Start()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00039F47 File Offset: 0x00038147
	public void SetTargetValue(string _specs, float value)
	{
		this.targetValue = value;
		this.specs = _specs;
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.MonitorForColorChange());
		}
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00039F71 File Offset: 0x00038171
	public void SetTargetSoundTrigger(string _specs, float value, bool failed, bool andComplete)
	{
		this.targetValue = value;
		this.specs = _specs;
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.MonitorForSound(failed, andComplete));
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00039F9E File Offset: 0x0003819E
	private IEnumerator MonitorForColorChange()
	{
		yield return null;
		while (this.currentValue < this.targetValue - 0.05f)
		{
			this.currentValue = this.image.fillAmount;
			yield return null;
		}
		yield return null;
		this.colorChanger.ChangeColor();
		yield break;
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00039FAD File Offset: 0x000381AD
	private IEnumerator MonitorForSound(bool failed, bool andComplete)
	{
		yield return null;
		while (this.currentValue < this.targetValue - 0.05f)
		{
			this.currentValue = this.image.fillAmount;
			yield return null;
		}
		yield return null;
		this.colorChanger.PlaySound(this.specs, failed, true, andComplete);
		yield break;
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00039FCA File Offset: 0x000381CA
	public void Reset()
	{
		base.StopAllCoroutines();
		this.colorChanger.ResetColor();
		this.currentValue = 0f;
		this.targetValue = 1f;
	}

	// Token: 0x0400094A RID: 2378
	[SerializeField]
	private Image image;

	// Token: 0x0400094B RID: 2379
	[SerializeField]
	private ChangeTextColor colorChanger;

	// Token: 0x0400094C RID: 2380
	[SerializeField]
	private float targetValue;

	// Token: 0x0400094D RID: 2381
	[SerializeField]
	private float currentValue;

	// Token: 0x0400094E RID: 2382
	private string specs = "";
}
