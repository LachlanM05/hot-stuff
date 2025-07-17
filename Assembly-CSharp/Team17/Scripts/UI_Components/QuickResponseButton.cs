using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000207 RID: 519
	public class QuickResponseButton : MonoBehaviour
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0005676B File Offset: 0x0005496B
		public bool ForceOverrideAllUI
		{
			get
			{
				return this._forceOverrideAllUI;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00056773 File Offset: 0x00054973
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x0005677B File Offset: 0x0005497B
		public bool ForceUsePrimaryActionIfOnlySingleResponse
		{
			get
			{
				return this._forceUsePrimaryActionIfOnlySingleResponse;
			}
			set
			{
				this._forceUsePrimaryActionIfOnlySingleResponse = value;
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00056784 File Offset: 0x00054984
		private void OnEnable()
		{
			this._button = base.GetComponent<Button>();
			Singleton<QuickResponseService>.Instance.Register(this);
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0005679D File Offset: 0x0005499D
		private void OnDisable()
		{
			if (Singleton<QuickResponseService>.Instance != null)
			{
				Singleton<QuickResponseService>.Instance.Unregister(this);
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x000567B7 File Offset: 0x000549B7
		private void Update()
		{
			if (this.CanButtonBePressed() && Singleton<QuickResponseService>.Instance.IsQuickResponseButtonPressed(this))
			{
				Button.ButtonClickedEvent onClick = this._button.onClick;
				if (onClick == null)
				{
					return;
				}
				onClick.Invoke();
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x000567E3 File Offset: 0x000549E3
		private bool CanButtonBePressed()
		{
			return this._button != null && this._button.gameObject.activeInHierarchy && this._button.IsInteractable();
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00056814 File Offset: 0x00054A14
		public void SetIconText(string text)
		{
			if (this._iconText == null)
			{
				return;
			}
			ControllerGlyphComponent component = this._iconText.GetComponent<ControllerGlyphComponent>();
			if (component != null)
			{
				component.SetText(text);
				return;
			}
			this._iconText.text = text;
		}

		// Token: 0x04000E1F RID: 3615
		[SerializeField]
		private TMP_Text _iconText;

		// Token: 0x04000E20 RID: 3616
		[SerializeField]
		private bool _forceOverrideAllUI;

		// Token: 0x04000E21 RID: 3617
		[SerializeField]
		private bool _forceUsePrimaryActionIfOnlySingleResponse;

		// Token: 0x04000E22 RID: 3618
		private Button _button;
	}
}
