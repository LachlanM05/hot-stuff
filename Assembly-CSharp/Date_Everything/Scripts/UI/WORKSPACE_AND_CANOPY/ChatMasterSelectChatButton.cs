using System;
using UnityEngine;

namespace Date_Everything.Scripts.UI.WORKSPACE_AND_CANOPY
{
	// Token: 0x0200026C RID: 620
	public class ChatMasterSelectChatButton : MonoBehaviour
	{
		// Token: 0x06001414 RID: 5140 RVA: 0x00060971 File Offset: 0x0005EB71
		public void SelectChatButton()
		{
			if (Singleton<ChatMaster>.Instance != null)
			{
				Singleton<ChatMaster>.Instance.SelectCurrentActiveChatButton();
			}
		}
	}
}
