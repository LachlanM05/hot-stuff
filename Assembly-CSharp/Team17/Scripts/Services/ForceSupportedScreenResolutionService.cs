using System;
using Steamworks;
using T17.Services;
using UnityEngine;

namespace Team17.Scripts.Services
{
	// Token: 0x02000213 RID: 531
	public class ForceSupportedScreenResolutionService : MonoBehaviour
	{
		// Token: 0x0600114B RID: 4427 RVA: 0x00057B8E File Offset: 0x00055D8E
		private void Update()
		{
			if (ForceSupportedScreenResolutionService.ShouldForceToSupportedScreenResolution() && this.IsDirty())
			{
				this.ForceUpdateScreenResolution();
			}
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00057BA5 File Offset: 0x00055DA5
		private bool IsDirty()
		{
			return this._lastForcedWidth != Screen.width || this._lastForcedHeight != Screen.height;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00057BC6 File Offset: 0x00055DC6
		private bool IsSupported(int width, int height)
		{
			return Mathf.Approximately((float)width / (float)height, 1.7777778f);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00057BDC File Offset: 0x00055DDC
		private void ForceUpdateScreenResolution()
		{
			this._lastForcedWidth = Screen.width;
			this._lastForcedHeight = Screen.height;
			if (Services.ResolutionProviderService.IsResolutionSupported(Screen.currentResolution))
			{
				return;
			}
			SettingsMenu.ForceResolutionToSavedValue();
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00057C0B File Offset: 0x00055E0B
		private static bool ShouldForceToSupportedScreenResolution()
		{
			return !SteamUtils.IsSteamRunningOnSteamDeck();
		}

		// Token: 0x04000E4F RID: 3663
		private int _lastForcedWidth;

		// Token: 0x04000E50 RID: 3664
		private int _lastForcedHeight;
	}
}
