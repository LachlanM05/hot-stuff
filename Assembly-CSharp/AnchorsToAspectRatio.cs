using System;
using UnityEngine;

// Token: 0x02000196 RID: 406
[ExecuteAlways]
public class AnchorsToAspectRatio : MonoBehaviour
{
	// Token: 0x06000DFC RID: 3580 RVA: 0x0004DDDD File Offset: 0x0004BFDD
	public void OnEnable()
	{
		this.AdjustAnchors();
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0004DDE8 File Offset: 0x0004BFE8
	public void AdjustAnchors()
	{
		if (this.aspectRatios == null || this.aspectRatios.Length == 0 || this.targetRect == null)
		{
			return;
		}
		int num = 0;
		float num2 = 10000f;
		float aspectRatio = this.GetAspectRatio();
		if (this.aspectRatios.Length > 1)
		{
			for (int i = this.aspectRatios.Length - 1; i >= 0; i--)
			{
				float num3;
				if (this.aspectRatios[i].resolutionType == AnchorsToAspectRatio.AspectRatioData.ResolutionType._Custom)
				{
					num3 = this.aspectRatios[i].aspectRatio;
				}
				else
				{
					num3 = AnchorsToAspectRatio.resolutions[(int)this.aspectRatios[i].resolutionType];
				}
				if (num3 != 0f)
				{
					float num4 = Mathf.Abs(aspectRatio - num3);
					if (num4 < num2)
					{
						num2 = num4;
						num = i;
					}
				}
			}
		}
		if (this.aspectRatios[num].anchorsToChange.Length != 0)
		{
			Vector2 anchorMin = this.targetRect.anchorMin;
			Vector2 anchorMax = this.targetRect.anchorMax;
			bool flag = false;
			bool flag2 = false;
			for (int j = this.aspectRatios[num].anchorsToChange.Length - 1; j >= 0; j--)
			{
				float newAnchorValue = this.aspectRatios[num].anchorsToChange[j].newAnchorValue;
				switch (this.aspectRatios[num].anchorsToChange[j].anchorToChange)
				{
				case AnchorsToAspectRatio.AspectRatioData.AdjustAnchor.Y_Min:
					anchorMin.y = newAnchorValue;
					flag = true;
					break;
				case AnchorsToAspectRatio.AspectRatioData.AdjustAnchor.Y_Max:
					anchorMax.y = newAnchorValue;
					flag2 = true;
					break;
				case AnchorsToAspectRatio.AspectRatioData.AdjustAnchor.X_Min:
					anchorMin.x = newAnchorValue;
					flag = true;
					break;
				case AnchorsToAspectRatio.AspectRatioData.AdjustAnchor.X_Max:
					anchorMax.x = newAnchorValue;
					flag2 = true;
					break;
				}
			}
			if (flag)
			{
				this.targetRect.anchorMin = anchorMin;
			}
			if (flag2)
			{
				this.targetRect.anchorMax = anchorMax;
			}
		}
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x0004DFB3 File Offset: 0x0004C1B3
	private float GetAspectRatio()
	{
		return (float)Screen.width / (float)Screen.height;
	}

	// Token: 0x04000C6F RID: 3183
	private static float[] resolutions = new float[] { 0f, 1.7777778f, 2.3888888f, 1.6f };

	// Token: 0x04000C70 RID: 3184
	[Tooltip("The RectTransform to adjust")]
	public RectTransform targetRect;

	// Token: 0x04000C71 RID: 3185
	[Tooltip("It will choose the closest to the current aspect ration")]
	public AnchorsToAspectRatio.AspectRatioData[] aspectRatios;

	// Token: 0x0200037E RID: 894
	[Serializable]
	public struct AspectRatioData
	{
		// Token: 0x040013C5 RID: 5061
		public AnchorsToAspectRatio.AspectRatioData.ResolutionType resolutionType;

		// Token: 0x040013C6 RID: 5062
		public float aspectRatio;

		// Token: 0x040013C7 RID: 5063
		public AnchorsToAspectRatio.AspectRatioData.AnchorChange[] anchorsToChange;

		// Token: 0x020003E6 RID: 998
		public enum AdjustAnchor
		{
			// Token: 0x0400154D RID: 5453
			Y_Min,
			// Token: 0x0400154E RID: 5454
			Y_Max,
			// Token: 0x0400154F RID: 5455
			X_Min,
			// Token: 0x04001550 RID: 5456
			X_Max
		}

		// Token: 0x020003E7 RID: 999
		public enum ResolutionType
		{
			// Token: 0x04001552 RID: 5458
			_Custom,
			// Token: 0x04001553 RID: 5459
			_1080p,
			// Token: 0x04001554 RID: 5460
			_UltraWide,
			// Token: 0x04001555 RID: 5461
			_SteamDeck
		}

		// Token: 0x020003E8 RID: 1000
		[Serializable]
		public struct AnchorChange
		{
			// Token: 0x04001556 RID: 5462
			public AnchorsToAspectRatio.AspectRatioData.AdjustAnchor anchorToChange;

			// Token: 0x04001557 RID: 5463
			public float newAnchorValue;
		}
	}
}
