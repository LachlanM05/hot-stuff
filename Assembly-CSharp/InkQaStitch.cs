using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000100 RID: 256
[Serializable]
public class InkQaStitch
{
	// Token: 0x060008CB RID: 2251 RVA: 0x0003410F File Offset: 0x0003230F
	public string StitchTreated(string filename)
	{
		return filename + "." + this.Stitch.Replace("=", "");
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060008CC RID: 2252 RVA: 0x00034134 File Offset: 0x00032334
	public int PercentageDone
	{
		get
		{
			int num = 1;
			int num2 = 0;
			foreach (InkQaLine inkQaLine in this.LineList)
			{
				num++;
				if (inkQaLine.IsChecked)
				{
					num2++;
				}
			}
			return num2 * 100 / num;
		}
	}

	// Token: 0x04000806 RID: 2054
	[SerializeField]
	public string Stitch;

	// Token: 0x04000807 RID: 2055
	[SerializeField]
	public bool IsRegularFlow;

	// Token: 0x04000808 RID: 2056
	[SerializeField]
	public bool IsChecked;

	// Token: 0x04000809 RID: 2057
	[SerializeField]
	public List<InkQaLine> LineList = new List<InkQaLine>();
}
