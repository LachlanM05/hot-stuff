using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FF RID: 255
[Serializable]
public class InkQaFile
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0003405C File Offset: 0x0003225C
	public int PercentageDone
	{
		get
		{
			int num = 1;
			int num2 = 0;
			foreach (InkQaStitch inkQaStitch in this.StitchList)
			{
				foreach (InkQaLine inkQaLine in inkQaStitch.LineList)
				{
					num++;
					if (inkQaLine.IsChecked)
					{
						num2++;
					}
				}
			}
			return num2 * 100 / num;
		}
	}

	// Token: 0x04000803 RID: 2051
	[SerializeField]
	public string FileName;

	// Token: 0x04000804 RID: 2052
	[SerializeField]
	public string Character;

	// Token: 0x04000805 RID: 2053
	[SerializeField]
	public List<InkQaStitch> StitchList = new List<InkQaStitch>();
}
