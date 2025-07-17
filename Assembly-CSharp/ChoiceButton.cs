using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000E9 RID: 233
public class ChoiceButton : MonoBehaviour
{
	// Token: 0x060007B0 RID: 1968 RVA: 0x0002B188 File Offset: 0x00029388
	private void Start()
	{
		this._button = base.GetComponent<Button>();
		this._button.onClick.AddListener(new UnityAction(this.SetChoice));
		if (this.ChoiceObject != null && this.ChoiceObject.Thumbnail != null)
		{
			this.Thumbnail.sprite = this.ChoiceObject.Thumbnail;
		}
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0002B1F4 File Offset: 0x000293F4
	private void SetChoice()
	{
	}

	// Token: 0x040006EA RID: 1770
	public CustomizationChoice ChoiceObject;

	// Token: 0x040006EB RID: 1771
	public Image Thumbnail;

	// Token: 0x040006EC RID: 1772
	private Button _button;
}
