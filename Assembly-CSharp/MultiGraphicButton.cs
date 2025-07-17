using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AA RID: 426
public class MultiGraphicButton : Button
{
	// Token: 0x06000E7F RID: 3711 RVA: 0x0004FCD4 File Offset: 0x0004DED4
	protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
	{
		base.DoStateTransition(state, instant);
		int i = 0;
		int num = this.graphicalObjects.Length;
		while (i < num)
		{
			if (this.graphicalObjects[i] != null && this.graphicalObjects[i].graphicalObject != null)
			{
				Color color;
				switch (state)
				{
				case Selectable.SelectionState.Normal:
					color = this.graphicalObjects[i].normal;
					break;
				case Selectable.SelectionState.Highlighted:
					color = this.graphicalObjects[i].highlight;
					break;
				case Selectable.SelectionState.Pressed:
					color = this.graphicalObjects[i].pressed;
					break;
				case Selectable.SelectionState.Selected:
					color = this.graphicalObjects[i].selected;
					break;
				case Selectable.SelectionState.Disabled:
					color = this.graphicalObjects[i].disabled;
					break;
				default:
					color = Color.red;
					break;
				}
				this.graphicalObjects[i].graphicalObject.color = color;
			}
			i++;
		}
	}

	// Token: 0x04000CE5 RID: 3301
	[SerializeField]
	public MultiGraphicButton.GraphicItem[] graphicalObjects = new MultiGraphicButton.GraphicItem[0];

	// Token: 0x02000387 RID: 903
	[Serializable]
	public class GraphicItem
	{
		// Token: 0x040013E6 RID: 5094
		public Graphic graphicalObject;

		// Token: 0x040013E7 RID: 5095
		public Color normal = Color.white;

		// Token: 0x040013E8 RID: 5096
		public Color highlight = Color.yellow;

		// Token: 0x040013E9 RID: 5097
		public Color pressed = Color.green;

		// Token: 0x040013EA RID: 5098
		public Color selected = Color.yellow;

		// Token: 0x040013EB RID: 5099
		public Color disabled = Color.grey;
	}
}
