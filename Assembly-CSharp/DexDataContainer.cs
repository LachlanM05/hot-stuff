using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000F5 RID: 245
public class DexDataContainer : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	// Token: 0x06000874 RID: 2164 RVA: 0x00032004 File Offset: 0x00030204
	private void Awake()
	{
		this.rect = base.GetComponent<RectTransform>();
		this.entryNumber = (base.transform.GetSiblingIndex() + 1).ToString("00");
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0003203D File Offset: 0x0003023D
	public void OnSelect(BaseEventData eventData)
	{
	}

	// Token: 0x040007BD RID: 1981
	public string entryNumber;

	// Token: 0x040007BE RID: 1982
	public string entryName;

	// Token: 0x040007BF RID: 1983
	public string entryObject;

	// Token: 0x040007C0 RID: 1984
	public Sprite entryPortrait;

	// Token: 0x040007C1 RID: 1985
	public RectTransform rect;
}
