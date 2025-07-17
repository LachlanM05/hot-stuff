using System;
using UnityEngine;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000203 RID: 515
	public class DynamicNavigationOverride : MonoBehaviour
	{
		// Token: 0x060010D3 RID: 4307 RVA: 0x0005664E File Offset: 0x0005484E
		private void Awake()
		{
			this._selectable = base.GetComponent<Selectable>();
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0005665C File Offset: 0x0005485C
		public Selectable FindSelectableOnUp()
		{
			if (this._upOverride == null)
			{
				return this._selectable.FindSelectableOnUp();
			}
			return this._upOverride.FindSelectable();
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00056683 File Offset: 0x00054883
		public Selectable FindSelectableOnDown()
		{
			if (this._downOverride == null)
			{
				return this._selectable.FindSelectableOnDown();
			}
			return this._downOverride.FindSelectable();
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000566AA File Offset: 0x000548AA
		public Selectable FindSelectableOnLeft()
		{
			if (this._leftOverride == null)
			{
				return this._selectable.FindSelectableOnLeft();
			}
			return this._leftOverride.FindSelectable();
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000566D1 File Offset: 0x000548D1
		public Selectable FindSelectableOnRight()
		{
			if (this._rightOverride == null)
			{
				return this._selectable.FindSelectableOnRight();
			}
			return this._rightOverride.FindSelectable();
		}

		// Token: 0x04000E1A RID: 3610
		[SerializeField]
		private BaseFindSelectableProvider _upOverride;

		// Token: 0x04000E1B RID: 3611
		[SerializeField]
		private BaseFindSelectableProvider _downOverride;

		// Token: 0x04000E1C RID: 3612
		[SerializeField]
		private BaseFindSelectableProvider _leftOverride;

		// Token: 0x04000E1D RID: 3613
		[SerializeField]
		private BaseFindSelectableProvider _rightOverride;

		// Token: 0x04000E1E RID: 3614
		private Selectable _selectable;
	}
}
