using System;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000204 RID: 516
	public class LastActiveDateADexEntrySelectableProvider : BaseFindSelectableProvider
	{
		// Token: 0x060010D9 RID: 4313 RVA: 0x00056700 File Offset: 0x00054900
		public override Selectable FindSelectable()
		{
			return DateADex.Instance.GetLastSelectedEntry();
		}
	}
}
