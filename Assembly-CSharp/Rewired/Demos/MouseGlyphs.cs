using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000267 RID: 615
	public class MouseGlyphs : MonoBehaviour
	{
		// Token: 0x060013EA RID: 5098 RVA: 0x00060370 File Offset: 0x0005E570
		private void Awake()
		{
			this._glyphsDict = new Dictionary<int, MouseGlyphs.GlyphEntry>();
			foreach (MouseGlyphs.GlyphEntry glyphEntry in this.glyphs)
			{
				this._glyphsDict.Add(glyphEntry.elementIdentifierId, glyphEntry);
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x000603DC File Offset: 0x0005E5DC
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
			return this.GetGlyph(actionElementMap.elementIdentifierId, actionElementMap.axisRange);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00060410 File Offset: 0x0005E610
		public Sprite GetGlyph(int elementIdentifierId, AxisRange axisRange)
		{
			if (this.glyphs == null)
			{
				return null;
			}
			MouseGlyphs.GlyphEntry glyphEntry;
			if (this._glyphsDict.TryGetValue(elementIdentifierId, out glyphEntry))
			{
				return glyphEntry.GetGlyph(axisRange);
			}
			return null;
		}

		// Token: 0x04000F68 RID: 3944
		[SerializeField]
		private List<MouseGlyphs.GlyphEntry> glyphs = new List<MouseGlyphs.GlyphEntry>
		{
			new MouseGlyphs.GlyphEntry("Mouse Horizontal", 0),
			new MouseGlyphs.GlyphEntry("Mouse Vertical", 1),
			new MouseGlyphs.GlyphEntry("Mouse Wheel", 2),
			new MouseGlyphs.GlyphEntry("Left Mouse Button", 3),
			new MouseGlyphs.GlyphEntry("Right Mouse Button", 4),
			new MouseGlyphs.GlyphEntry("Mouse Button 3", 5),
			new MouseGlyphs.GlyphEntry("Mouse Button 4", 6),
			new MouseGlyphs.GlyphEntry("Mouse Button 5", 7),
			new MouseGlyphs.GlyphEntry("Mouse Button 6", 8),
			new MouseGlyphs.GlyphEntry("Mouse Button 7", 9),
			new MouseGlyphs.GlyphEntry("Mouse Wheel Horizontal", 10)
		};

		// Token: 0x04000F69 RID: 3945
		[NonSerialized]
		private Dictionary<int, MouseGlyphs.GlyphEntry> _glyphsDict;

		// Token: 0x020003D6 RID: 982
		[Serializable]
		private class GlyphEntry
		{
			// Token: 0x060018AD RID: 6317 RVA: 0x00071613 File Offset: 0x0006F813
			public GlyphEntry()
			{
			}

			// Token: 0x060018AE RID: 6318 RVA: 0x0007161B File Offset: 0x0006F81B
			public GlyphEntry(string name, int elementIdentifierId)
			{
				this.name = name;
				this.elementIdentifierId = elementIdentifierId;
			}

			// Token: 0x060018AF RID: 6319 RVA: 0x00071634 File Offset: 0x0006F834
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

			// Token: 0x0400151C RID: 5404
			public string name;

			// Token: 0x0400151D RID: 5405
			public int elementIdentifierId;

			// Token: 0x0400151E RID: 5406
			public Sprite glyph;

			// Token: 0x0400151F RID: 5407
			public Sprite glyphPos;

			// Token: 0x04001520 RID: 5408
			public Sprite glyphNeg;
		}
	}
}
