using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200013E RID: 318
public class Tab : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x06000B88 RID: 2952 RVA: 0x00042450 File Offset: 0x00040650
	private void OnEnable()
	{
		this._button = base.GetComponent<Button>();
		this._text = base.GetComponentInChildren<TextMeshProUGUI>();
		this._buttonBg = base.GetComponent<Image>();
		if (this.IsSelectedByDefault)
		{
			ControllerMenuUI.SetCurrentlySelected(this._button.gameObject, ControllerMenuUI.Direction.Down, false, false);
		}
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0004249C File Offset: 0x0004069C
	public void OnSelect(BaseEventData eventData)
	{
		this._text.color = this._button.colors.normalColor;
		this._buttonBg.color = this._button.colors.selectedColor;
		foreach (object obj in this.Content.transform)
		{
			Object.Destroy(((Transform)obj).gameObject);
		}
		foreach (CustomizationChoice customizationChoice in this.Choices)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.ChoiceButtonPrefab, this.Content.transform);
			gameObject.GetComponentsInChildren<Image>()[1].sprite = customizationChoice.Thumbnail;
			gameObject.GetComponent<ChoiceButton>().ChoiceObject = customizationChoice;
		}
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x00042590 File Offset: 0x00040790
	public void OnDeselect(BaseEventData eventData)
	{
		this._text.color = this._button.colors.selectedColor;
		this._buttonBg.color = this._button.colors.normalColor;
	}

	// Token: 0x04000A4B RID: 2635
	public bool IsSelectedByDefault;

	// Token: 0x04000A4C RID: 2636
	public GameObject Content;

	// Token: 0x04000A4D RID: 2637
	public CustomizationChoice[] Choices;

	// Token: 0x04000A4E RID: 2638
	public GameObject ChoiceButtonPrefab;

	// Token: 0x04000A4F RID: 2639
	private Button _button;

	// Token: 0x04000A50 RID: 2640
	private TextMeshProUGUI _text;

	// Token: 0x04000A51 RID: 2641
	private Image _buttonBg;
}
