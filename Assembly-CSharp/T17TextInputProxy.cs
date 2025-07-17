using System;
using T17.Services;
using Team17.Platform.OnScreenKeyboard;
using Team17.Platform.OnScreenKeyboard.TextMeshPro;
using UnityEngine.UI;

// Token: 0x020001B6 RID: 438
public class T17TextInputProxy : TextInputProxy
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00051138 File Offset: 0x0004F338
	public TextInputCompleteEvent TextInputCompleteEvent
	{
		get
		{
			return this.m_InputCompleteEvent;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00051140 File Offset: 0x0004F340
	protected override IOnScreenKeyboardService OnScreenKeyboardService
	{
		get
		{
			return Services.OnScreenKeyboard;
		}
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x00051148 File Offset: 0x0004F348
	protected new void Awake()
	{
		base.Awake();
		if (this.OnScreenKeyboardService == null || !this.OnScreenKeyboardService.IsSupported())
		{
			TMPInputFieldTextInputProvider tmpinputFieldTextInputProvider = base.TextInputProvider as TMPInputFieldTextInputProvider;
			if (base.OSKButton != null && tmpinputFieldTextInputProvider != null)
			{
				Navigation navigation = base.OSKButton.navigation;
				navigation.selectOnUp = tmpinputFieldTextInputProvider.InputField;
				navigation.selectOnDown = tmpinputFieldTextInputProvider.InputField;
				navigation.selectOnLeft = tmpinputFieldTextInputProvider.InputField;
				navigation.selectOnRight = tmpinputFieldTextInputProvider.InputField;
				base.OSKButton.navigation = navigation;
				base.OSKButton.interactable = false;
				base.OSKButton.enabled = false;
				Image component = base.GetComponent<Image>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
		}
	}
}
