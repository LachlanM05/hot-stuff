using System;
using Rewired;
using T17.Services;
using Team17.Services;
using TMPro;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class TextMeshPro_T17 : TextMeshProUGUI
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000EDE RID: 3806 RVA: 0x0005121C File Offset: 0x0004F41C
	// (set) Token: 0x06000EDF RID: 3807 RVA: 0x00051224 File Offset: 0x0004F424
	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			if (!Application.isPlaying)
			{
				base.text = value;
				return;
			}
			if (this.internalAction)
			{
				base.text = value;
				return;
			}
			this.sourceString = value;
			if (base.isActiveAndEnabled)
			{
				this.SetBaseTextToConvertedString();
				return;
			}
			base.text = this.sourceString;
		}
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00051272 File Offset: 0x0004F472
	protected override void Awake()
	{
		this._pendingSetText = false;
		this._pendingSetBaseText = false;
		base.Awake();
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00051288 File Offset: 0x0004F488
	protected override void OnEnable()
	{
		if (Application.isPlaying)
		{
			if (string.IsNullOrEmpty(this.sourceString))
			{
				this.sourceString = this.text;
			}
			if (!string.IsNullOrEmpty(this.sourceString))
			{
				this.SetTextToConvertedString();
			}
		}
		base.OnEnable();
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x000512C3 File Offset: 0x0004F4C3
	protected override void OnDisable()
	{
		if (Application.isPlaying)
		{
			this.RegisterForNotifications(false);
		}
		base.OnDisable();
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x000512D9 File Offset: 0x0004F4D9
	protected void Update()
	{
		if (this._pendingSetText)
		{
			this.SetTextToConvertedString();
		}
		if (this._pendingSetBaseText)
		{
			this.SetBaseTextToConvertedString();
		}
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x000512F8 File Offset: 0x0004F4F8
	private string ConvertString()
	{
		bool flag;
		string convertedString = this.GetConvertedString(this.sourceString, out flag);
		this.RegisterForNotifications(flag);
		return convertedString;
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x0005131C File Offset: 0x0004F51C
	private string GetConvertedString(string theText, out bool stringConverted)
	{
		IIconTextMarkupService iconTextMarkupService = Services.IconTextMarkupService;
		if (iconTextMarkupService != null)
		{
			return iconTextMarkupService.ReplaceMirandaMarkupWithTMPSpriteMarkup(theText, out stringConverted);
		}
		stringConverted = true;
		return theText;
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x00051340 File Offset: 0x0004F540
	private void RegisterForNotifications(bool register)
	{
		if (!ReInput.isReady)
		{
			return;
		}
		if (register && base.isActiveAndEnabled)
		{
			if (!this.resgisteredForChange)
			{
				ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.OnControllerChanged));
				Services.IconTextMarkupService.RefreshedIconsEvent += this.ForceRefreshText;
				this.resgisteredForChange = true;
				return;
			}
		}
		else if (this.resgisteredForChange)
		{
			Services.IconTextMarkupService.RefreshedIconsEvent -= this.ForceRefreshText;
			ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.OnControllerChanged));
			this.resgisteredForChange = false;
		}
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x000513D7 File Offset: 0x0004F5D7
	public void OnControllerChanged(Controller controller)
	{
		this.SetTextToConvertedString();
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x000513DF File Offset: 0x0004F5DF
	private void ForceRefreshText()
	{
		this.SetTextToConvertedString();
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x000513E8 File Offset: 0x0004F5E8
	private void SetTextToConvertedString()
	{
		this.internalAction = true;
		if (!this.HasTextMarkupServiceCachedContrllerMaps())
		{
			this._pendingSetText = true;
			return;
		}
		string text = this.ConvertString();
		this.text = string.Empty;
		this.text = text;
		this.internalAction = false;
		this._pendingSetText = false;
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x00051433 File Offset: 0x0004F633
	private void SetBaseTextToConvertedString()
	{
		if (!this.HasTextMarkupServiceCachedContrllerMaps())
		{
			this._pendingSetBaseText = true;
			return;
		}
		base.text = this.ConvertString();
		this._pendingSetBaseText = false;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x00051458 File Offset: 0x0004F658
	private bool HasTextMarkupServiceCachedContrllerMaps()
	{
		IIconTextMarkupService iconTextMarkupService = Services.IconTextMarkupService;
		return iconTextMarkupService != null && iconTextMarkupService.HasCachedControllerMaps;
	}

	// Token: 0x04000D27 RID: 3367
	private string sourceString = "";

	// Token: 0x04000D28 RID: 3368
	private bool resgisteredForChange;

	// Token: 0x04000D29 RID: 3369
	private bool internalAction;

	// Token: 0x04000D2A RID: 3370
	private bool _pendingSetText;

	// Token: 0x04000D2B RID: 3371
	private bool _pendingSetBaseText;
}
