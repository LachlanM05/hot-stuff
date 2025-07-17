using System;
using TMPro;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class TextMeshProWaveEffect : MonoBehaviour
{
	// Token: 0x06000BC7 RID: 3015 RVA: 0x00044339 File Offset: 0x00042539
	private void Start()
	{
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x0004433C File Offset: 0x0004253C
	private void Update()
	{
		TextMeshProUGUI component = base.gameObject.GetComponent<TextMeshProUGUI>();
		if (component != null)
		{
			if (component.text.Contains("<wave>"))
			{
				this.originalText = component.text;
			}
			else if (!component.text.Contains("<voffset="))
			{
				this.originalText = null;
				return;
			}
			if (this.originalText != null)
			{
				if (!this.goingDown)
				{
					this.vOffset += Time.deltaTime * this.speed;
				}
				else
				{
					this.vOffset -= Time.deltaTime * this.speed;
				}
				if (this.vOffset > this.waveHeight)
				{
					this.goingDown = true;
				}
				else if (this.vOffset < -this.waveHeight)
				{
					this.goingDown = false;
				}
				component.SetText(this.originalText.Replace("<wave>", "<voffset=" + Math.Round((double)this.vOffset, 2).ToString().Replace(",", ".") + "em>").Replace("</wave>", "</voffset>"), true);
			}
		}
	}

	// Token: 0x04000A81 RID: 2689
	private float speed = 5f;

	// Token: 0x04000A82 RID: 2690
	private float waveHeight = 0.4f;

	// Token: 0x04000A83 RID: 2691
	private float vOffset;

	// Token: 0x04000A84 RID: 2692
	private bool goingDown;

	// Token: 0x04000A85 RID: 2693
	private string originalText;
}
