using System;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001F9 RID: 505
	public class CloseButtonGlyphComponent : MonoBehaviour
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x000560E0 File Offset: 0x000542E0
		private void Update()
		{
			bool flag = !Singleton<QuickResponseService>.Instance.IsQuickResponseEnabled();
			if (this._glyphObject.activeSelf != flag)
			{
				this._glyphObject.SetActive(flag);
			}
		}

		// Token: 0x04000E05 RID: 3589
		[SerializeField]
		private GameObject _glyphObject;
	}
}
