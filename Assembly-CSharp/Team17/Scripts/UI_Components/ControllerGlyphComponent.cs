using System;
using Rewired;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001FB RID: 507
	public class ControllerGlyphComponent : MonoBehaviour
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060010B2 RID: 4274 RVA: 0x000561F4 File Offset: 0x000543F4
		// (remove) Token: 0x060010B3 RID: 4275 RVA: 0x0005622C File Offset: 0x0005442C
		public event ControllerGlyphComponent.ISOkToDisplayGlyth OnISOkToDisplayGlyth;

		// Token: 0x060010B4 RID: 4276 RVA: 0x00056261 File Offset: 0x00054461
		private void Awake()
		{
			this.layoutElement = base.GetComponent<LayoutElement>();
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0005626F File Offset: 0x0005446F
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.OnControllerChanged));
			this.isPermissionGranted = this.IsDisplayPermissionGranted();
			this.RefreshIconText();
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x000562A1 File Offset: 0x000544A1
		private void OnDisable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.OnControllerChanged));
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x000562C1 File Offset: 0x000544C1
		private void Update()
		{
			if (this.isControllerUsed && this.isPermissionGranted != this.IsDisplayPermissionGranted())
			{
				this.isPermissionGranted = !this.isPermissionGranted;
				this.RefreshIconText();
			}
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x000562F0 File Offset: 0x000544F0
		private bool IsDisplayPermissionGranted()
		{
			if (this.OnISOkToDisplayGlyth != null)
			{
				ControllerGlyphComponent.ResultEvent resultEvent = new ControllerGlyphComponent.ResultEvent();
				this.OnISOkToDisplayGlyth(resultEvent);
				return resultEvent.result;
			}
			return true;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0005631F File Offset: 0x0005451F
		private void OnControllerChanged(Controller controller)
		{
			this.RefreshIconText();
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00056327 File Offset: 0x00054527
		public void SetText(string text)
		{
			this._text = text;
			this.RefreshIconText();
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00056336 File Offset: 0x00054536
		private bool ShouldShowIcon()
		{
			if (Services.InputService.IsLastActiveInputController())
			{
				this.isControllerUsed = true;
				return true;
			}
			this.isControllerUsed = false;
			return false;
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00056358 File Offset: 0x00054558
		private void RefreshIconText()
		{
			if (!this.ShouldShowIcon() || !this.isPermissionGranted)
			{
				this._iconText.text = string.Empty;
				this.SetLayoutElementIgnore(true);
				return;
			}
			this._iconText.text = this._text;
			this.SetLayoutElementIgnore(false);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x000563A5 File Offset: 0x000545A5
		private void SetLayoutElementIgnore(bool ignore)
		{
			if (this.layoutElement != null)
			{
				this.layoutElement.ignoreLayout = ignore;
			}
		}

		// Token: 0x04000E0A RID: 3594
		[SerializeField]
		private string _text = string.Empty;

		// Token: 0x04000E0B RID: 3595
		[SerializeField]
		private TMP_Text _iconText;

		// Token: 0x04000E0C RID: 3596
		private bool isPermissionGranted = true;

		// Token: 0x04000E0D RID: 3597
		private bool isControllerUsed;

		// Token: 0x04000E0E RID: 3598
		private LayoutElement layoutElement;

		// Token: 0x020003B6 RID: 950
		public class ResultEvent
		{
			// Token: 0x040014C3 RID: 5315
			public bool result = true;
		}

		// Token: 0x020003B7 RID: 951
		// (Invoke) Token: 0x06001868 RID: 6248
		public delegate void ISOkToDisplayGlyth(ControllerGlyphComponent.ResultEvent result);
	}
}
