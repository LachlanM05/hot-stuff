using System;
using Rewired;
using T17.Services;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class ControllerDisplayComponent : MonoBehaviour
{
	// Token: 0x06000E34 RID: 3636 RVA: 0x0004E9BD File Offset: 0x0004CBBD
	private void Start()
	{
		this.UpdateDisplay();
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x0004E9C5 File Offset: 0x0004CBC5
	private void OnEnable()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ControllerChanged));
		this.UpdateDisplay();
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0004E9EB File Offset: 0x0004CBEB
	private void OnDisable()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ControllerChanged));
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0004EA0C File Offset: 0x0004CC0C
	private void UpdateDisplay()
	{
		bool flag = !this.ShouldDisplay();
		if (this.elementToDisable != null)
		{
			this.elementToDisable.SetActive(flag);
		}
		if (this.componentToDisable != null)
		{
			this.componentToDisable.enabled = flag;
		}
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0004EA57 File Offset: 0x0004CC57
	private void ControllerChanged(Controller controller)
	{
		this.UpdateDisplay();
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x0004EA5F File Offset: 0x0004CC5F
	public bool IsSelectionBlocked()
	{
		return !this.neverBlockFocus && this.ShouldDisplay();
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0004EA74 File Offset: 0x0004CC74
	public bool ShouldDisplay()
	{
		bool flag = Services.InputService.IsLastActiveInputController();
		return (!flag && this.disableCondition == ControllerDisplayComponent.DisableCondiction.DisableOnKeyboard) || (flag && this.disableCondition == ControllerDisplayComponent.DisableCondiction.DisableOnController);
	}

	// Token: 0x04000C9A RID: 3226
	[SerializeField]
	private GameObject elementToDisable;

	// Token: 0x04000C9B RID: 3227
	[SerializeField]
	private MonoBehaviour componentToDisable;

	// Token: 0x04000C9C RID: 3228
	[SerializeField]
	private ControllerDisplayComponent.DisableCondiction disableCondition = ControllerDisplayComponent.DisableCondiction.DisableOnController;

	// Token: 0x04000C9D RID: 3229
	[SerializeField]
	private bool neverBlockFocus;

	// Token: 0x02000382 RID: 898
	private enum DisableCondiction
	{
		// Token: 0x040013D3 RID: 5075
		DisableOnKeyboard,
		// Token: 0x040013D4 RID: 5076
		DisableOnController
	}
}
