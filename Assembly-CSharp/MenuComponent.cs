using System;
using T17.Services;
using Team17.Scripts.Services.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000116 RID: 278
public class MenuComponent : MonoBehaviour
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x0003703B File Offset: 0x0003523B
	public virtual bool NeedsInput
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000985 RID: 2437 RVA: 0x0003703E File Offset: 0x0003523E
	// (set) Token: 0x06000986 RID: 2438 RVA: 0x00037046 File Offset: 0x00035246
	public string MenuObjectName { get; private set; }

	// Token: 0x06000987 RID: 2439 RVA: 0x0003704F File Offset: 0x0003524F
	protected virtual void Awake()
	{
		this.MenuObjectName = base.name;
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00037060 File Offset: 0x00035260
	public void AutoSelect()
	{
		if (this.autoSelect != null)
		{
			ControllerMenuUI.SetCurrentlySelected(this.autoSelect, ControllerMenuUI.Direction.Down, false, false);
			if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != this.autoSelect && Singleton<ControllerMenuUI>.Instance != null)
			{
				Singleton<ControllerMenuUI>.Instance.HighlightAButton(true);
				return;
			}
		}
		else if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
		{
			foreach (Selectable selectable in base.GetComponentsInChildren<Selectable>())
			{
				if (selectable.gameObject.activeInHierarchy && selectable.IsInteractable())
				{
					this.autoSelect = selectable.gameObject;
					ControllerMenuUI.SetCurrentlySelected(selectable.gameObject, ControllerMenuUI.Direction.Down, false, false);
					return;
				}
			}
		}
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00037130 File Offset: 0x00035330
	private void OnEnable()
	{
		this.onShow.Invoke();
		if (this.NeedsInput)
		{
			this._inputModeHandle = Services.InputService.PushMode(this._inputModeToStack, this);
		}
		this.AutoSelect();
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00037162 File Offset: 0x00035362
	protected virtual void OnDisable()
	{
		this.onHide.Invoke();
		InputModeHandle inputModeHandle = this._inputModeHandle;
		if (inputModeHandle != null)
		{
			inputModeHandle.SafeDispose();
		}
		this._inputModeHandle = null;
	}

	// Token: 0x040008C6 RID: 2246
	public UnityEvent onShow;

	// Token: 0x040008C7 RID: 2247
	public UnityEvent onHide;

	// Token: 0x040008C9 RID: 2249
	public GameObject autoSelect;

	// Token: 0x040008CA RID: 2250
	public GameObject autoSelectFallback;

	// Token: 0x040008CB RID: 2251
	private InputModeHandle _inputModeHandle;

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	private IMirandaInputService.EInputMode _inputModeToStack = IMirandaInputService.EInputMode.UI;
}
