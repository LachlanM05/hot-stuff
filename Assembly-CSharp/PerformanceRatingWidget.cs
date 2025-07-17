using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000156 RID: 342
public class PerformanceRatingWidget : MonoBehaviour
{
	// Token: 0x06000CC3 RID: 3267 RVA: 0x0004A377 File Offset: 0x00048577
	public void SetValue(float value)
	{
		this._value = Math.Clamp(value, 0f, (float)this._stars.Count);
		this.UpdateDisplay();
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x0004A39C File Offset: 0x0004859C
	private void OnValidate()
	{
		this.UpdateDisplay();
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x0004A3A4 File Offset: 0x000485A4
	private void UpdateDisplay()
	{
		for (int i = 0; i < this._stars.Count; i++)
		{
			this._stars[i].fillAmount = Mathf.Clamp01(this._value - (float)i);
		}
	}

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private List<Image> _stars;

	// Token: 0x04000B74 RID: 2932
	[Header("Display debug (will not affect ink values):")]
	[SerializeField]
	[Range(0f, 5f)]
	private float _value;
}
