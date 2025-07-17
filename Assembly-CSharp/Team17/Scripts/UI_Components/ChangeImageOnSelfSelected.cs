using System;
using UnityEngine;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001F7 RID: 503
	[RequireComponent(typeof(Image))]
	public class ChangeImageOnSelfSelected : MonoBehaviour
	{
		// Token: 0x0600109F RID: 4255 RVA: 0x00055F96 File Offset: 0x00054196
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this.SaveCurrentSetupAsDefaultSettings();
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00055FAC File Offset: 0x000541AC
		private bool IsSelected()
		{
			GameObject currentlySelected = Singleton<ControllerMenuUI>.Instance.currentlySelected;
			return !(currentlySelected == null) && (currentlySelected == base.gameObject || currentlySelected.transform.IsChildOf(base.gameObject.transform));
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00055FFA File Offset: 0x000541FA
		private void Update()
		{
			if (this.IsSelected())
			{
				this.Apply(this._selectedSprite, this._selectedColour);
				return;
			}
			this.Apply(this._defaultSprite, this._defaultColour);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00056029 File Offset: 0x00054229
		private void Apply(Sprite sprite, Color colour)
		{
			if (this._image.sprite != sprite)
			{
				this._image.sprite = sprite;
			}
			if (this._image.color != colour)
			{
				this._image.color = colour;
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00056069 File Offset: 0x00054269
		private void SaveCurrentSetupAsDefaultSettings()
		{
			if (this._savedDefaultSettings)
			{
				return;
			}
			this._savedDefaultSettings = true;
			this._defaultSprite = this._image.sprite;
			this._defaultColour = this._image.color;
		}

		// Token: 0x04000DFD RID: 3581
		[SerializeField]
		private Sprite _selectedSprite;

		// Token: 0x04000DFE RID: 3582
		[SerializeField]
		private Color _selectedColour;

		// Token: 0x04000DFF RID: 3583
		private Image _image;

		// Token: 0x04000E00 RID: 3584
		private bool _savedDefaultSettings;

		// Token: 0x04000E01 RID: 3585
		private Sprite _defaultSprite;

		// Token: 0x04000E02 RID: 3586
		private Color _defaultColour;
	}
}
