using System;
using TMPro;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class DexTitleFollow : MonoBehaviour
{
	// Token: 0x0600088E RID: 2190 RVA: 0x00032E0B File Offset: 0x0003100B
	private void Awake()
	{
		this.rect = base.GetComponent<RectTransform>();
		this.target = this.rect.anchoredPosition;
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00032E2C File Offset: 0x0003102C
	private void Update()
	{
		if (Vector2.Distance(this.rect.anchoredPosition, this.target) > 0.001f)
		{
			this.rect.anchoredPosition = Vector2.Lerp(this.rect.anchoredPosition, this.target, Time.deltaTime * this.speed);
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00032E83 File Offset: 0x00031083
	public void SetTarget(Vector2 newTarget)
	{
		this.target = newTarget;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00032E8C File Offset: 0x0003108C
	public void SetText(string numberIn, string nameIn, string objectIn)
	{
		this.titleNumber.text = "<size=75%>#<size=100%>" + numberIn;
		this.titleName.text = nameIn;
		this.titleObject.text = " the " + objectIn;
	}

	// Token: 0x040007D6 RID: 2006
	[SerializeField]
	private TextMeshProUGUI titleNumber;

	// Token: 0x040007D7 RID: 2007
	[SerializeField]
	private TextMeshProUGUI titleName;

	// Token: 0x040007D8 RID: 2008
	[SerializeField]
	private TextMeshProUGUI titleObject;

	// Token: 0x040007D9 RID: 2009
	private RectTransform rect;

	// Token: 0x040007DA RID: 2010
	private Vector2 target;

	// Token: 0x040007DB RID: 2011
	public float speed = 10f;
}
