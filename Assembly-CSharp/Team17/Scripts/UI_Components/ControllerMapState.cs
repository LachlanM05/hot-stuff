using System;
using Rewired;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x020001FE RID: 510
	public class ControllerMapState
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00056406 File Offset: 0x00054606
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x0005640E File Offset: 0x0005460E
		public bool LastKnownState { get; private set; }

		// Token: 0x060010C4 RID: 4292 RVA: 0x00056417 File Offset: 0x00054617
		public ControllerMapState(ControllerMap map)
		{
			this.LastKnownState = map.enabled;
			this._mapId = map.id;
			this.Name = ControllerMapState.GetName(map);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00056444 File Offset: 0x00054644
		private static string GetName(ControllerMap map)
		{
			string text;
			switch (map.categoryId)
			{
			case 0:
				text = "Default";
				break;
			case 1:
				text = "Dialog";
				break;
			case 2:
				text = "CharacterController";
				break;
			case 3:
				text = "Engagement";
				break;
			case 4:
				text = "Debug";
				break;
			case 5:
				text = "UI";
				break;
			default:
				text = string.Format("no-mapped-name-{0}", map.categoryId);
				break;
			}
			string text2 = text;
			return map.controller.type.ToString() + "-" + text2;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000564E4 File Offset: 0x000546E4
		public bool IsLastKnownStateDirty()
		{
			ControllerMap controllerMap = ReInput.mapping.GetControllerMap(this._mapId);
			return controllerMap == null || this.LastKnownState != controllerMap.enabled;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00056518 File Offset: 0x00054718
		public void UpdateLastKnownState()
		{
			ControllerMap controllerMap = ReInput.mapping.GetControllerMap(this._mapId);
			if (controllerMap == null)
			{
				return;
			}
			this.LastKnownState = controllerMap.enabled;
		}

		// Token: 0x04000E0F RID: 3599
		public readonly string Name;

		// Token: 0x04000E10 RID: 3600
		private readonly int _mapId;
	}
}
