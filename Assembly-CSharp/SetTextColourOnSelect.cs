using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000134 RID: 308
public class SetTextColourOnSelect : MonoBehaviour
{
	// Token: 0x06000ABE RID: 2750 RVA: 0x0003DB84 File Offset: 0x0003BD84
	public void SetHoveredColour(bool isHovered)
	{
		this._isHovered = isHovered;
		if (this._isSelected && !this._isHovered)
		{
			this.SetSelectedColour(true);
			return;
		}
		this._text.color = (this._isHovered ? this._hoveredColour : this._defaultColour);
		if (this._image != null)
		{
			this._image.color = (this._isSelected ? this._selectedBackColour : this._defaultBackColour);
		}
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0003DC00 File Offset: 0x0003BE00
	public void SetSelectedColour(bool isSelected)
	{
		this._isSelected = isSelected;
		if (this._isHovered)
		{
			this._text.color = this._hoveredColour;
		}
		else
		{
			this._text.color = (isSelected ? this._selectedColour : this._defaultColour);
		}
		if (this._image != null)
		{
			this._image.color = (isSelected ? this._selectedBackColour : this._defaultBackColour);
		}
	}

	// Token: 0x040009C1 RID: 2497
	[SerializeField]
	private Color _selectedColour;

	// Token: 0x040009C2 RID: 2498
	[SerializeField]
	private Color _hoveredColour;

	// Token: 0x040009C3 RID: 2499
	[SerializeField]
	private Color _defaultColour;

	// Token: 0x040009C4 RID: 2500
	[SerializeField]
	private TextMeshProUGUI _text;

	// Token: 0x040009C5 RID: 2501
	[SerializeField]
	private Color _selectedBackColour;

	// Token: 0x040009C6 RID: 2502
	[SerializeField]
	private Color _defaultBackColour;

	// Token: 0x040009C7 RID: 2503
	[SerializeField]
	private Image _image;

	// Token: 0x040009C8 RID: 2504
	private bool _isSelected;

	// Token: 0x040009C9 RID: 2505
	private bool _isHovered;
}
