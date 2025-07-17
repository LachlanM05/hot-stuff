using System;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000202 RID: 514
	public class DisplaySettingsMenuSelector : SettingsMenuSelector
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060010CB RID: 4299 RVA: 0x0005655E File Offset: 0x0005475E
		public bool IsDirty
		{
			get
			{
				return this.SelectedIndex != this.CommittedSelectedIndex;
			}
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00056571 File Offset: 0x00054771
		public override void ForceSetting(int settingIndex)
		{
			base.ForceSetting(settingIndex);
			this.CommittedSelectedIndex = settingIndex;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00056581 File Offset: 0x00054781
		public override void GoToNextOption()
		{
			if (this.SelectedIndex + 1 == this._numberOfOptions)
			{
				this.SelectedIndex = 0;
			}
			else
			{
				this.SelectedIndex++;
			}
			this.SetSelectedOptionDisplayText();
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x000565B0 File Offset: 0x000547B0
		public override void GoToPreviousOption()
		{
			if (this.SelectedIndex == 0)
			{
				this.SelectedIndex = this._numberOfOptions - 1;
			}
			else
			{
				this.SelectedIndex--;
			}
			this.SetSelectedOptionDisplayText();
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000565DE File Offset: 0x000547DE
		public virtual void ApplyCurrentSetting()
		{
			if (this.IsDirty && this.SettingCallback != null)
			{
				this.SettingCallback();
			}
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x000565FB File Offset: 0x000547FB
		public virtual void CommitCurrentSetting()
		{
			if (this.IsDirty)
			{
				this.CommittedSelectedIndex = this.SelectedIndex;
				this.SetSetting();
			}
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00056617 File Offset: 0x00054817
		public virtual void RevertCurrentSettingToCommittedValue()
		{
			if (this.IsDirty)
			{
				this.SelectedIndex = this.CommittedSelectedIndex;
				this.SetSelectedOptionDisplayText();
				if (this.SettingCallback != null)
				{
					this.SettingCallback();
				}
			}
		}

		// Token: 0x04000E19 RID: 3609
		protected int CommittedSelectedIndex;
	}
}
