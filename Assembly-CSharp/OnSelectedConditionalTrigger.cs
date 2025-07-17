using System;
using T17.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001AE RID: 430
public class OnSelectedConditionalTrigger : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x06000E8E RID: 3726 RVA: 0x0004FFFF File Offset: 0x0004E1FF
	private void Awake()
	{
		this.lastInputController = this.WasLastInputAController();
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0005000D File Offset: 0x0004E20D
	public void OnSelect(BaseEventData eventData)
	{
		this.currentlySelected = true;
		this.InvokeSelectionEvents();
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x0005001C File Offset: 0x0004E21C
	public void OnDeselect(BaseEventData eventData)
	{
		this.currentlySelected = false;
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x00050025 File Offset: 0x0004E225
	public void Update()
	{
		if (this.WasLastInputAController() != this.lastInputController)
		{
			this.lastInputController = !this.lastInputController;
			if (this.currentlySelected)
			{
				this.InvokeSelectionEvents();
			}
		}
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x00050052 File Offset: 0x0004E252
	private void InvokeSelectionEvents()
	{
		this.ClearInvokeVars();
		if (this.lastInputController)
		{
			if (this.Controller != null)
			{
				this.Controller.Invoke();
				return;
			}
		}
		else if (this.MouseKeyboard != null)
		{
			this.MouseKeyboard.Invoke();
		}
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x00050089 File Offset: 0x0004E289
	private bool WasLastInputAController()
	{
		return Services.InputService.IsLastActiveInputController();
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x00050095 File Offset: 0x0004E295
	private void ClearInvokeVars()
	{
		this.hasChangedFocusThisInvoke = false;
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x000500A0 File Offset: 0x0004E2A0
	public void SetFocusToControl(GameObject control)
	{
		if (!this.hasChangedFocusThisInvoke && Singleton<ControllerMenuUI>.Instance != null && control != null && control.activeInHierarchy)
		{
			Selectable component = control.GetComponent<Selectable>();
			if (component != null && component.enabled && component.IsInteractable())
			{
				this.hasChangedFocusThisInvoke = true;
				ControllerMenuUI.SetCurrentlySelected(control, ControllerMenuUI.Direction.Down, false, false);
			}
		}
	}

	// Token: 0x04000CEF RID: 3311
	private bool lastInputController;

	// Token: 0x04000CF0 RID: 3312
	private bool currentlySelected;

	// Token: 0x04000CF1 RID: 3313
	[Header("Triggered when:\n1. The input method changes while control is selected\n2. The control gains focus")]
	[Header("Input methods")]
	public UnityEvent MouseKeyboard;

	// Token: 0x04000CF2 RID: 3314
	public UnityEvent Controller;

	// Token: 0x04000CF3 RID: 3315
	private bool hasChangedFocusThisInvoke;
}
