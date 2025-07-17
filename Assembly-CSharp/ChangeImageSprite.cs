using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E6 RID: 230
public class ChangeImageSprite : MonoBehaviour
{
	// Token: 0x060007A2 RID: 1954 RVA: 0x0002AC4D File Offset: 0x00028E4D
	private void Start()
	{
		this.imageComp = base.GetComponent<Image>();
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0002AC5B File Offset: 0x00028E5B
	public void ResetSprite()
	{
		if (!this.imageComp)
		{
			this.Start();
			return;
		}
		this.imageComp.sprite = this.defaultSprite;
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0002AC82 File Offset: 0x00028E82
	public void ChangeSprite()
	{
		this.imageComp.sprite = this.changeToSprite;
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002AC95 File Offset: 0x00028E95
	public void ChangeSprite(float time)
	{
		base.Invoke("ChangeSprite", time);
	}

	// Token: 0x040006DB RID: 1755
	[SerializeField]
	private Sprite changeToSprite;

	// Token: 0x040006DC RID: 1756
	[SerializeField]
	private Sprite defaultSprite;

	// Token: 0x040006DD RID: 1757
	private Image imageComp;
}
