using System;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000210 RID: 528
	[RequireComponent(typeof(ControllerGlyphComponent))]
	public class SpecsScreenControllerGlyphButton : MonoBehaviour
	{
		// Token: 0x06001140 RID: 4416 RVA: 0x00057A49 File Offset: 0x00055C49
		private void Awake()
		{
			this._specStatMain = base.GetComponentInParent<SpecStatMain>(true);
			this._controllerGlyphComponent = base.GetComponent<ControllerGlyphComponent>();
			this._controllerGlyphComponent.OnISOkToDisplayGlyth += this.AreSpecsButtonsSuppressed;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00057A7B File Offset: 0x00055C7B
		private void AreSpecsButtonsSuppressed(ControllerGlyphComponent.ResultEvent result)
		{
			if (!base.transform.parent.gameObject.activeInHierarchy)
			{
				result.result = false;
				return;
			}
			if (this._specStatMain.IsSuppressed())
			{
				result.result = false;
				return;
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00057AB1 File Offset: 0x00055CB1
		private void OnDestroy()
		{
			this._controllerGlyphComponent.OnISOkToDisplayGlyth -= this.AreSpecsButtonsSuppressed;
		}

		// Token: 0x04000E4A RID: 3658
		private ControllerGlyphComponent _controllerGlyphComponent;

		// Token: 0x04000E4B RID: 3659
		private SpecStatMain _specStatMain;
	}
}
