using System;
using System.Collections.Generic;
using T17.Services;
using Team17.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x0200020A RID: 522
	public class ResolutionSettingsMenuSelector : DisplaySettingsMenuSelector
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x00056FBC File Offset: 0x000551BC
		protected override int _numberOfOptions
		{
			get
			{
				if (this._resolutionOptionList != null)
				{
					return this._resolutionOptionList.Count;
				}
				return 0;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x00056FD3 File Offset: 0x000551D3
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x00056FDA File Offset: 0x000551DA
		public static string ResolutionSettingsKey { get; private set; } = "resolution";

		// Token: 0x06001109 RID: 4361 RVA: 0x00056FE2 File Offset: 0x000551E2
		protected void Awake()
		{
			this.ValidateResolutions();
			ResolutionSettingsMenuSelector.ResolutionSettingsKey = this.SettingKey;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00056FF5 File Offset: 0x000551F5
		private void ValidateResolutions()
		{
			if (!this.initialised)
			{
				this.ParsePreferedResolutions();
				this.PopulateOptionList();
				this.CommittedSelectedIndex = this.SelectedIndex;
				this.initialised = true;
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0005701E File Offset: 0x0005521E
		protected new void Start()
		{
			this.NextOption.onClick.AddListener(new UnityAction(this.GoToNextOption));
			this.PreviousOption.onClick.AddListener(new UnityAction(this.GoToPreviousOption));
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0005705A File Offset: 0x0005525A
		protected void OnDestroy()
		{
			this.NextOption.onClick.RemoveListener(new UnityAction(this.GoToNextOption));
			this.PreviousOption.onClick.RemoveListener(new UnityAction(this.GoToPreviousOption));
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00057098 File Offset: 0x00055298
		private void ParsePreferedResolutions()
		{
			this._preferredResolutions = new Resolution[this.PreferredResolutions.Length];
			for (int i = 0; i < this.PreferredResolutions.Length; i++)
			{
				int num;
				int num2;
				ResolutionSettingsMenuSelector.ParseEntry(this.PreferredResolutions[i], out num, out num2);
				if (num == 0 || num2 == 0)
				{
					T17Debug.LogError(string.Format("Failed to parse preferred resolution. Index={0} value='{1}'", i, this.PreferredResolutions[i]));
				}
				this._preferredResolutions[i] = new Resolution
				{
					height = num2,
					width = num
				};
			}
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00057128 File Offset: 0x00055328
		private void PopulateOptionList()
		{
			this._resolutionOptionList = new List<Resolution>();
			foreach (Resolution resolution in Services.ResolutionProviderService.CurrentlySupportedResolutions)
			{
				this.AddResolutionToOptionsList(resolution);
			}
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00057184 File Offset: 0x00055384
		private void AddResolutionToOptionsList(Resolution resolution)
		{
			this._resolutionOptionList.Add(resolution);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00057194 File Offset: 0x00055394
		public bool ForceSettings(string resolutionString)
		{
			this.ValidateResolutions();
			int num = this._resolutionOptionList.FindIndex((Resolution x) => x.ToString() == resolutionString);
			if (num < 0)
			{
				int num2 = resolutionString.LastIndexOf('@');
				if (num2 > 0)
				{
					resolutionString = resolutionString.Substring(0, num2).TrimEnd();
					num = this._resolutionOptionList.FindIndex((Resolution x) => x.ToString().StartsWith(resolutionString));
				}
			}
			if (num >= 0)
			{
				this.SelectedIndex = num;
				this.CommittedSelectedIndex = this.SelectedIndex;
				this.SetSetting();
				return true;
			}
			return false;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00057234 File Offset: 0x00055434
		public bool GetSelectedValue(out Resolution resolution)
		{
			bool flag = false;
			if (this.SelectedIndex <= this._resolutionOptionList.Count)
			{
				resolution = this._resolutionOptionList[this.SelectedIndex];
				flag = true;
			}
			else
			{
				resolution = Screen.currentResolution;
				T17Debug.LogError(string.Format("[ResolutionSettingsMenuSelector]. failed to get the selected values. selectedIndex={0}. Reverting to screen.width and scree.height ", this.SelectedIndex));
			}
			return flag;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00057298 File Offset: 0x00055498
		public static bool ParseEntry(string resolutionText, out int width, out int height)
		{
			bool flag = false;
			string[] array = resolutionText.Split(new char[] { 'x', '@', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length >= 2)
			{
				flag = int.TryParse(array[0], out width);
				flag |= int.TryParse(array[1], out height);
			}
			else
			{
				width = 0;
				height = 0;
			}
			return flag;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x000572E5 File Offset: 0x000554E5
		public override void LoadSetting(int savedSettingIndex)
		{
			base.LoadSetting(savedSettingIndex);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x000572F0 File Offset: 0x000554F0
		public override void SetSetting()
		{
			Resolution resolution;
			if (this.GetSelectedValue(out resolution))
			{
				Services.GraphicsSettings.SetString(this.SettingKey, resolution.ToString());
				string text = string.Format("{0} X {1} @ {2}Hz", resolution.width, resolution.height, Convert.ToInt32(resolution.refreshRateRatio.value));
				this.SelectedOption.SetText(text, true);
				if (this.SettingCallback != null)
				{
					this.SettingCallback();
				}
			}
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00057380 File Offset: 0x00055580
		protected override void SetSelectedOptionDisplayText()
		{
			Resolution resolution;
			if (this.GetSelectedValue(out resolution))
			{
				string text = string.Format("{0} X {1} @ {2}Hz", resolution.width, resolution.height, Convert.ToInt32(resolution.refreshRateRatio.value));
				this.SelectedOption.SetText(text, true);
			}
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x000573E0 File Offset: 0x000555E0
		public override void GoToNextOption()
		{
			base.GoToPreviousOption();
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x000573E8 File Offset: 0x000555E8
		public override void GoToPreviousOption()
		{
			base.GoToNextOption();
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x000573F0 File Offset: 0x000555F0
		public int GetMinRefreshRate()
		{
			return this.MinRefreshRate;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x000573F8 File Offset: 0x000555F8
		public int GetMaxRefreshRate()
		{
			return this.MaxRefreshRate;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00057400 File Offset: 0x00055600
		public int GetNumberOfOptions()
		{
			return this._numberOfOptions;
		}

		// Token: 0x04000E2D RID: 3629
		[SerializeField]
		private string[] PreferredResolutions;

		// Token: 0x04000E2E RID: 3630
		[SerializeField]
		private int MinRefreshRate = 50;

		// Token: 0x04000E2F RID: 3631
		[SerializeField]
		private int MaxRefreshRate = int.MaxValue;

		// Token: 0x04000E30 RID: 3632
		private Resolution[] _preferredResolutions;

		// Token: 0x04000E31 RID: 3633
		private List<Resolution> _resolutionOptionList;

		// Token: 0x04000E32 RID: 3634
		private bool initialised;
	}
}
