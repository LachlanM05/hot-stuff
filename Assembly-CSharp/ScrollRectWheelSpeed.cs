using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000131 RID: 305
public class ScrollRectWheelSpeed : MonoBehaviour
{
	// Token: 0x06000AB6 RID: 2742 RVA: 0x0003D8EB File Offset: 0x0003BAEB
	private void Start()
	{
		if (this.scrollRect == null)
		{
			return;
		}
		this.scrollRect.scrollSensitivity = this.wheelSpeed;
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x0003D90D File Offset: 0x0003BB0D
	private void OnValidate()
	{
		if (this.scrollRect == null)
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
		}
	}

	// Token: 0x040009B9 RID: 2489
	[SerializeField]
	private float wheelSpeed = 1f;

	// Token: 0x040009BA RID: 2490
	[SerializeField]
	private ScrollRect scrollRect;
}
