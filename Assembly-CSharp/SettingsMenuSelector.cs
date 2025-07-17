using System;
using System.Collections.Generic;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000136 RID: 310
public class SettingsMenuSelector : MonoBehaviour
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0003EFD7 File Offset: 0x0003D1D7
	protected virtual int _numberOfOptions
	{
		get
		{
			return this.OptionList.Count;
		}
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0003EFE4 File Offset: 0x0003D1E4
	public virtual void LoadSetting(int savedSettingIndex)
	{
		this.SelectedIndex = savedSettingIndex;
		this.SetSelectedOptionDisplayText();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0003EFF3 File Offset: 0x0003D1F3
	public int GetSelectedIndex()
	{
		return this.SelectedIndex;
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0003EFFB File Offset: 0x0003D1FB
	public virtual void ForceSetting(int settingIndex)
	{
		this.SelectedIndex = settingIndex;
		this.SetSetting();
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0003F00C File Offset: 0x0003D20C
	public virtual void SetSetting()
	{
		if (this._numberOfOptions > this.SelectedIndex)
		{
			if (this.IsACloudSavableSetting)
			{
				Services.GameSettings.SetInt(this.SettingKey, this.SelectedIndex);
			}
			else
			{
				Services.GraphicsSettings.SetInt(this.SettingKey, this.SelectedIndex);
			}
			this.SetSelectedOptionDisplayText();
			if (this.SettingCallback != null)
			{
				this.SettingCallback();
			}
		}
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0003F078 File Offset: 0x0003D278
	protected void Start()
	{
		if (this.IsACloudSavableSetting)
		{
			Services.GameSettings.GetInt(this.SettingKey, 0);
		}
		else
		{
			Services.GraphicsSettings.GetInt(this.SettingKey, 0);
		}
		this.NextOption.onClick.AddListener(new UnityAction(this.GoToNextOption));
		this.PreviousOption.onClick.AddListener(new UnityAction(this.GoToPreviousOption));
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0003F0ED File Offset: 0x0003D2ED
	public virtual void GoToNextOption()
	{
		if (this.SelectedIndex + 1 == this._numberOfOptions)
		{
			this.SelectedIndex = 0;
		}
		else
		{
			this.SelectedIndex++;
		}
		this.SetSetting();
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0003F11C File Offset: 0x0003D31C
	public virtual void GoToPreviousOption()
	{
		if (this.SelectedIndex == 0)
		{
			this.SelectedIndex = this._numberOfOptions - 1;
		}
		else
		{
			this.SelectedIndex--;
		}
		this.SetSetting();
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0003F14A File Offset: 0x0003D34A
	protected virtual void SetSelectedOptionDisplayText()
	{
		this.SelectedOption.SetText(this.OptionList[this.SelectedIndex], true);
	}

	// Token: 0x040009FE RID: 2558
	public TextMeshProUGUI SelectedOption;

	// Token: 0x040009FF RID: 2559
	public Button NextOption;

	// Token: 0x04000A00 RID: 2560
	public Button PreviousOption;

	// Token: 0x04000A01 RID: 2561
	public List<string> OptionList;

	// Token: 0x04000A02 RID: 2562
	public string SettingKey;

	// Token: 0x04000A03 RID: 2563
	protected int SelectedIndex;

	// Token: 0x04000A04 RID: 2564
	public bool IsResolution;

	// Token: 0x04000A05 RID: 2565
	public UnityAction SettingCallback;

	// Token: 0x04000A06 RID: 2566
	public bool IsACloudSavableSetting = true;
}
