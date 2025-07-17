using System;
using TMPro;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001FF RID: 511
	public class DebugUI : MonoBehaviour
	{
		// Token: 0x04000E12 RID: 3602
		[SerializeField]
		private TMP_Text m_UIInputText;

		// Token: 0x04000E13 RID: 3603
		[SerializeField]
		private TMP_Text m_LastInputTypeText;

		// Token: 0x04000E14 RID: 3604
		[SerializeField]
		private TMP_Text m_SuspendedGameText;

		// Token: 0x04000E15 RID: 3605
		[SerializeField]
		private TMP_Text m_CurrentlySelectedText;

		// Token: 0x04000E16 RID: 3606
		[SerializeField]
		private TMP_Text m_MapsText;

		// Token: 0x04000E17 RID: 3607
		[SerializeField]
		private TMP_Text m_InputStackText;

		// Token: 0x04000E18 RID: 3608
		[SerializeField]
		private TMP_Text m_UserInfoText;
	}
}
