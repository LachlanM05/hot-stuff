using System;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x0200020B RID: 523
	[RequireComponent(typeof(ControllerGlyphComponent))]
	public class RoomersFilterNameUpdaterButtonComponent : MonoBehaviour
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x0005742F File Offset: 0x0005562F
		private void Awake()
		{
			this._controllerGlyphComponent = base.GetComponent<ControllerGlyphComponent>();
			this.UpdateGlyphText();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00057443 File Offset: 0x00055643
		private void Update()
		{
			if (this.RequiresUpdating())
			{
				this.UpdateGlyphText();
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00057454 File Offset: 0x00055654
		private void UpdateGlyphText()
		{
			if (Roomers.Instance != null)
			{
				this._glyphRoomersScreen = Roomers.Instance.CurrentScreen;
			}
			switch (this._glyphRoomersScreen)
			{
			case Roomers.Roomers_Screen.AllEntries:
				this._controllerGlyphComponent.SetText(this._glypTextWhenOnAllEntiresScreen);
				return;
			case Roomers.Roomers_Screen.ActiveEntries:
				this._controllerGlyphComponent.SetText(this._glypTextWhenOnActiveEntriesScreen);
				return;
			case Roomers.Roomers_Screen.CompletedEntries:
				this._controllerGlyphComponent.SetText(this._glyphTextWhenOnCompletedEntiresScreen);
				return;
			default:
				this._controllerGlyphComponent.SetText(string.Empty);
				return;
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x000574DF File Offset: 0x000556DF
		private bool RequiresUpdating()
		{
			return !(Roomers.Instance == null) && Roomers.Instance.CurrentScreen != this._glyphRoomersScreen;
		}

		// Token: 0x04000E34 RID: 3636
		private ControllerGlyphComponent _controllerGlyphComponent;

		// Token: 0x04000E35 RID: 3637
		private Roomers.Roomers_Screen _glyphRoomersScreen;

		// Token: 0x04000E36 RID: 3638
		[SerializeField]
		private string _glypTextWhenOnAllEntiresScreen = "{UIMenuExtraAction} Show Active Entries";

		// Token: 0x04000E37 RID: 3639
		[SerializeField]
		private string _glypTextWhenOnActiveEntriesScreen = "{UIMenuExtraAction} Show Completed Entries";

		// Token: 0x04000E38 RID: 3640
		[SerializeField]
		private string _glyphTextWhenOnCompletedEntiresScreen = "{UIMenuExtraAction} Show All Entries";
	}
}
