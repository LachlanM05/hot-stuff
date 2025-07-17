using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class UIRectFollowWorldElement : MonoBehaviour
{
	// Token: 0x06000BF2 RID: 3058 RVA: 0x000451A0 File Offset: 0x000433A0
	private void Start()
	{
		this.UICanvas = base.gameObject.GetComponentInParent<Canvas>();
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x000451B3 File Offset: 0x000433B3
	private void Update()
	{
		base.transform.position = this.worldToUISpace(this.UICanvas, this.worldObj.transform.position);
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x000451DC File Offset: 0x000433DC
	public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
	{
		Vector3 vector = Camera.main.WorldToScreenPoint(worldPos);
		Vector2 vector2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, vector, parentCanvas.worldCamera, out vector2);
		return parentCanvas.transform.TransformPoint(vector2);
	}

	// Token: 0x04000AAB RID: 2731
	public GameObject worldObj;

	// Token: 0x04000AAC RID: 2732
	private Canvas UICanvas;
}
