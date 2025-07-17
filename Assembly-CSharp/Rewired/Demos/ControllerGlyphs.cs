using System;
using Rewired.Data.Mapping;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000265 RID: 613
	public class ControllerGlyphs : MonoBehaviour
	{
		// Token: 0x060013E2 RID: 5090 RVA: 0x0005F8C2 File Offset: 0x0005DAC2
		private void Awake()
		{
			ControllerGlyphs.Instance = this;
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0005F8CA File Offset: 0x0005DACA
		public Sprite GetGlyph(ActionElementMap actionElementMap)
		{
			if (actionElementMap == null)
			{
				throw new NullReferenceException("actionElementMap");
			}
			if (actionElementMap.controllerMap.controllerType != ControllerType.Mouse)
			{
				return null;
			}
			return this.GetGlyph(actionElementMap.controllerMap.hardwareGuid, actionElementMap.elementIdentifierId, actionElementMap.axisRange);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0005F908 File Offset: 0x0005DB08
		public Sprite GetGlyph(Guid joystickGuid, int elementIdentifierId, AxisRange axisRange)
		{
			if (ControllerGlyphs.Instance == null)
			{
				return null;
			}
			if (ControllerGlyphs.Instance.controllers == null)
			{
				return null;
			}
			for (int i = 0; i < ControllerGlyphs.Instance.controllers.Length; i++)
			{
				if (ControllerGlyphs.Instance.controllers[i] != null && !(ControllerGlyphs.Instance.controllers[i].joystick == null) && !(ControllerGlyphs.Instance.controllers[i].joystick.Guid != joystickGuid))
				{
					return ControllerGlyphs.Instance.controllers[i].GetGlyph(elementIdentifierId, axisRange);
				}
			}
			return null;
		}

		// Token: 0x04000F61 RID: 3937
		[SerializeField]
		private ControllerGlyphs.ControllerEntry[] controllers;

		// Token: 0x04000F62 RID: 3938
		private static ControllerGlyphs Instance;

		// Token: 0x020003D2 RID: 978
		[Serializable]
		private class ControllerEntry
		{
			// Token: 0x060018A5 RID: 6309 RVA: 0x00071520 File Offset: 0x0006F720
			public Sprite GetGlyph(int elementIdentifierId, AxisRange axisRange)
			{
				if (this.glyphs == null)
				{
					return null;
				}
				for (int i = 0; i < this.glyphs.Length; i++)
				{
					if (this.glyphs[i] != null && this.glyphs[i].elementIdentifierId == elementIdentifierId)
					{
						return this.glyphs[i].GetGlyph(axisRange);
					}
				}
				return null;
			}

			// Token: 0x04001511 RID: 5393
			public string name;

			// Token: 0x04001512 RID: 5394
			public HardwareJoystickMap joystick;

			// Token: 0x04001513 RID: 5395
			public ControllerGlyphs.GlyphEntry[] glyphs;
		}

		// Token: 0x020003D3 RID: 979
		[Serializable]
		private class GlyphEntry
		{
			// Token: 0x060018A7 RID: 6311 RVA: 0x0007157C File Offset: 0x0006F77C
			public Sprite GetGlyph(AxisRange axisRange)
			{
				switch (axisRange)
				{
				case AxisRange.Full:
					return this.glyph;
				case AxisRange.Positive:
					if (!(this.glyphPos != null))
					{
						return this.glyph;
					}
					return this.glyphPos;
				case AxisRange.Negative:
					if (!(this.glyphNeg != null))
					{
						return this.glyph;
					}
					return this.glyphNeg;
				default:
					return null;
				}
			}

			// Token: 0x04001514 RID: 5396
			public int elementIdentifierId;

			// Token: 0x04001515 RID: 5397
			public Sprite glyph;

			// Token: 0x04001516 RID: 5398
			public Sprite glyphPos;

			// Token: 0x04001517 RID: 5399
			public Sprite glyphNeg;
		}
	}
}
