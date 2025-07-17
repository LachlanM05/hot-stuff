using System;
using Rewired;
using T17.Services;
using Team17.Services;
using TMPro;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class ControlsUI : MonoBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x00002187 File Offset: 0x00000387
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000219C File Offset: 0x0000039C
	private void Update()
	{
		if (this.player == null)
		{
			this.player = ReInput.players.GetPlayer(0);
			return;
		}
		IIconTextMarkupService iconTextMarkupService = Services.IconTextMarkupService;
		bool flag = Services.InputService.IsLastActiveInputController();
		ControllerType controllerType = (flag ? ControllerType.Joystick : ControllerType.Keyboard);
		ControllerType? controllerType2 = (flag ? null : new ControllerType?(ControllerType.Mouse));
		if (this.InteractText)
		{
			this.InteractText.text = ControlsUI.GetActionText(this.player, 18, controllerType, controllerType2, iconTextMarkupService);
		}
		if (this.ExamenText)
		{
			this.ExamenText.text = ControlsUI.GetActionText(this.player, 12, controllerType, controllerType2, iconTextMarkupService);
		}
		this.DateviatorText.text = ControlsUI.GetActionText(this.player, 52, controllerType, controllerType2, iconTextMarkupService);
		if (this.AwakenText)
		{
			this.AwakenText.text = ControlsUI.GetActionText(this.player, 2, controllerType, controllerType2, iconTextMarkupService);
		}
		this.PauseText.text = ControlsUI.GetActionText(this.player, 5, controllerType, controllerType2, iconTextMarkupService);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000229B File Offset: 0x0000049B
	public static string TreatMouseMapping(string mapping)
	{
		return mapping.ToUpperInvariant().Replace("LEFT MOUSE BUTTON", "LMB").Replace("RIGHT MOUSE BUTTON", "RMB");
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000022C4 File Offset: 0x000004C4
	public static string GetActionText(Player player, int action)
	{
		IIconTextMarkupService iconTextMarkupService = Services.IconTextMarkupService;
		bool flag = Services.InputService.IsLastActiveInputController();
		ControllerType controllerType = (flag ? ControllerType.Joystick : ControllerType.Keyboard);
		ControllerType? controllerType2 = (flag ? null : new ControllerType?(ControllerType.Mouse));
		return ControlsUI.GetActionText(player, action, controllerType, controllerType2, iconTextMarkupService);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002308 File Offset: 0x00000508
	private static string GetActionText(Player player, int action, ControllerType currentControllerType, ControllerType? fallbackControllerType, IIconTextMarkupService iconTextMarkupService)
	{
		ActionElementMap actionElementMap = player.controllers.maps.GetFirstElementMapWithAction(currentControllerType, action, false);
		string text;
		if (actionElementMap != null)
		{
			text = actionElementMap.elementIdentifierName;
		}
		else if (fallbackControllerType != null)
		{
			actionElementMap = player.controllers.maps.GetFirstElementMapWithAction(fallbackControllerType.Value, action, false);
			if (actionElementMap != null)
			{
				text = actionElementMap.elementIdentifierName;
			}
			else
			{
				text = string.Empty;
			}
		}
		else
		{
			text = string.Empty;
		}
		return iconTextMarkupService.GetTMPSpriteTag(text, false);
	}

	// Token: 0x0400000A RID: 10
	public TextMeshProUGUI DateviatorText;

	// Token: 0x0400000B RID: 11
	public TextMeshProUGUI PauseText;

	// Token: 0x0400000C RID: 12
	public TextMeshProUGUI InteractText;

	// Token: 0x0400000D RID: 13
	public TextMeshProUGUI ExamenText;

	// Token: 0x0400000E RID: 14
	public TextMeshProUGUI AwakenText;

	// Token: 0x0400000F RID: 15
	private Player player;
}
