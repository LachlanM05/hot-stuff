using System;
using T17.Services;
using Team17.Scripts.UI_Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001A5 RID: 421
public class JoystickScroller : MonoBehaviour
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0004F3FE File Offset: 0x0004D5FE
	private bool IsJoystickScrollEnabled
	{
		get
		{
			return this._activatorWantsScrollEnabled || this._activationObjectsWantScrollEnabled;
		}
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x0004F410 File Offset: 0x0004D610
	private void Start()
	{
		this._joystickActivationProvider = base.GetComponent<IJoystickActivationProvider>();
		if (this._scrollRect == null)
		{
			base.GetComponentInChildren<ScrollRect>();
		}
		this._defaultScrollbarColour = this._scrollBar.colors.normalColor;
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x0004F458 File Offset: 0x0004D658
	private void Update()
	{
		float scrollInput = Services.UIInputService.GetScrollInput();
		if (Mathf.Abs(scrollInput) < this._deadZone)
		{
			this._isMoving = false;
			this.SetScrollbarColour();
			return;
		}
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			this._isMoving = false;
			this.SetScrollbarColour();
			return;
		}
		this.UpdateActivationObjectsScrollEnabled();
		this.UpdateActivatorScrollEnabled();
		if (!this.IsJoystickScrollEnabled)
		{
			this._isMoving = false;
			this.SetScrollbarColour();
			return;
		}
		int num = 1;
		if (scrollInput < 0f)
		{
			num = -1;
		}
		if (!this._isMoving)
		{
			this._isMoving = true;
			this.SetScrollbarColour();
		}
		if (this._scrollRect.vertical)
		{
			float y = this._scrollRect.content.sizeDelta.y;
			float num2 = this._scrollValue * Time.deltaTime * (float)num * y;
			this._scrollRect.verticalNormalizedPosition += num2 / y;
		}
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x0004F539 File Offset: 0x0004D739
	public void SetActivationObjects(GameObject[] activationObjects)
	{
		this._activationObjects = activationObjects;
		this.Reset();
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0004F548 File Offset: 0x0004D748
	private void Reset()
	{
		this._lastFocussedElement = null;
		this._activatorWantsScrollEnabled = false;
		this._activationObjectsWantScrollEnabled = false;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x0004F560 File Offset: 0x0004D760
	private void SetScrollbarColour()
	{
		ColorBlock colors = this._scrollBar.colors;
		colors.normalColor = (this._isMoving ? this._scrollBarInUseColour : this._defaultScrollbarColour);
		this._scrollBar.colors = colors;
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0004F5A2 File Offset: 0x0004D7A2
	private void UpdateActivatorScrollEnabled()
	{
		if (this._joystickActivationProvider == null)
		{
			this._activatorWantsScrollEnabled = false;
			return;
		}
		this._activatorWantsScrollEnabled = this._joystickActivationProvider.ShouldScrollerBeActive();
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x0004F5C8 File Offset: 0x0004D7C8
	private void UpdateActivationObjectsScrollEnabled()
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (currentSelectedGameObject.transform != this._lastFocussedElement)
		{
			this._activationObjectsWantScrollEnabled = false;
			for (int i = 0; i < this._activationObjects.Length; i++)
			{
				if (currentSelectedGameObject.transform.IsChildOf(this._activationObjects[i].transform))
				{
					this._activationObjectsWantScrollEnabled = true;
					break;
				}
			}
			this._lastFocussedElement = currentSelectedGameObject.transform;
		}
	}

	// Token: 0x04000CB9 RID: 3257
	[SerializeField]
	private GameObject[] _activationObjects;

	// Token: 0x04000CBA RID: 3258
	[SerializeField]
	private ScrollRect _scrollRect;

	// Token: 0x04000CBB RID: 3259
	[SerializeField]
	[Range(0f, 1f)]
	private float _scrollValue;

	// Token: 0x04000CBC RID: 3260
	[SerializeField]
	[Range(0f, 1f)]
	private float _deadZone;

	// Token: 0x04000CBD RID: 3261
	[SerializeField]
	private Scrollbar _scrollBar;

	// Token: 0x04000CBE RID: 3262
	[SerializeField]
	private Color _scrollBarInUseColour;

	// Token: 0x04000CBF RID: 3263
	private Color _defaultScrollbarColour;

	// Token: 0x04000CC0 RID: 3264
	private bool _activatorWantsScrollEnabled;

	// Token: 0x04000CC1 RID: 3265
	private bool _activationObjectsWantScrollEnabled;

	// Token: 0x04000CC2 RID: 3266
	private Transform _lastFocussedElement;

	// Token: 0x04000CC3 RID: 3267
	private bool _isMoving;

	// Token: 0x04000CC4 RID: 3268
	private IJoystickActivationProvider _joystickActivationProvider;
}
