using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Demos;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class ControlsGlyphs : MonoBehaviour
{
	// Token: 0x060007DA RID: 2010 RVA: 0x0002CA9C File Offset: 0x0002AC9C
	private void Awake()
	{
		if (ControlsGlyphs.Instance == null)
		{
			ControlsGlyphs.Instance = this;
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0002CAB1 File Offset: 0x0002ACB1
	private void OnDestroy()
	{
		if (this == ControlsGlyphs.Instance)
		{
			ControlsGlyphs.Instance = null;
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0002CAC4 File Offset: 0x0002ACC4
	public Sprite GetGlyph(Player p, int actionElementMapID, bool dark = true, bool forceMouseOverKeyboard = true)
	{
		ActionElementMap actionElementMap = ReInput.mapping.GetActionElementMap(actionElementMapID);
		return this.GetGlyph(p, actionElementMap, dark, forceMouseOverKeyboard);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0002CAE8 File Offset: 0x0002ACE8
	public Sprite GetGlyph(Player p, ActionElementMap actionElementMap, bool dark = true, bool forceMouseOverKeyboard = true)
	{
		Controller controller = p.controllers.GetLastActiveController();
		if (controller == null)
		{
			if (p.controllers.joystickCount > 0)
			{
				controller = p.controllers.Joysticks[0];
			}
			else
			{
				controller = p.controllers.Keyboard;
			}
		}
		Sprite sprite = null;
		if (controller.type == ControllerType.Keyboard && forceMouseOverKeyboard)
		{
			controller = ReInput.controllers.Mouse;
		}
		ControllerType type = controller.type;
		switch (type)
		{
		case ControllerType.Keyboard:
		{
			List<Sprite> list = new List<Sprite>();
			if (dark)
			{
				if (this.darkKeyboardGlyphs != null)
				{
					this.darkKeyboardGlyphs.GetGlyphs(actionElementMap, list);
					if (list.Count > 0)
					{
						sprite = list[0];
					}
				}
				return sprite;
			}
			if (this.lightKeyboardGlyphs != null)
			{
				this.lightKeyboardGlyphs.GetGlyphs(actionElementMap, list);
				if (list.Count > 0)
				{
					sprite = list[0];
				}
			}
			return sprite;
		}
		case ControllerType.Mouse:
			if (dark)
			{
				if (this.darkMouseGlyphs != null)
				{
					sprite = this.darkMouseGlyphs.GetGlyph(actionElementMap);
				}
				return sprite;
			}
			if (this.lightMouseGlyphs != null)
			{
				sprite = this.lightMouseGlyphs.GetGlyph(actionElementMap);
			}
			return sprite;
		case ControllerType.Joystick:
			if (this.controllerGlyphs != null)
			{
				sprite = this.controllerGlyphs.GetGlyph(actionElementMap);
			}
			return sprite;
		default:
			if (type != ControllerType.Custom)
			{
				return null;
			}
			return null;
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x0002CC38 File Offset: 0x0002AE38
	private void ShowGlyphHelp(Player p, Controller controller, InputAction action)
	{
		if (p == null || controller == null || action == null)
		{
			return;
		}
		ActionElementMap firstElementMapWithAction = p.controllers.maps.GetFirstElementMapWithAction(controller, action.id, true);
		if (firstElementMapWithAction == null)
		{
			return;
		}
		if (controller.type != ControllerType.Joystick)
		{
			return;
		}
		Sprite sprite = null;
		if (this.controllerGlyphs != null)
		{
			sprite = this.controllerGlyphs.GetGlyph((controller as Joystick).hardwareTypeGuid, firstElementMapWithAction.elementIdentifierId, firstElementMapWithAction.axisRange);
		}
		if (sprite == null)
		{
			return;
		}
		Rect rect = new Rect(0f, 30f, sprite.textureRect.width, sprite.textureRect.height);
		GUI.Label(new Rect(rect.x, rect.y + rect.height + 20f, rect.width, rect.height), action.descriptiveName);
		GUI.DrawTexture(rect, sprite.texture);
	}

	// Token: 0x04000705 RID: 1797
	public static ControlsGlyphs Instance;

	// Token: 0x04000706 RID: 1798
	[Header("Both Modes")]
	[SerializeField]
	private ControllerGlyphs controllerGlyphs;

	// Token: 0x04000707 RID: 1799
	[Header("Dark Mode")]
	[SerializeField]
	private KeyboardGlyphs darkKeyboardGlyphs;

	// Token: 0x04000708 RID: 1800
	[SerializeField]
	private MouseGlyphs darkMouseGlyphs;

	// Token: 0x04000709 RID: 1801
	[Header("Light Mode")]
	[SerializeField]
	private KeyboardGlyphs lightKeyboardGlyphs;

	// Token: 0x0400070A RID: 1802
	[SerializeField]
	private MouseGlyphs lightMouseGlyphs;
}
