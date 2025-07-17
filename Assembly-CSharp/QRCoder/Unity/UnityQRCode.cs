using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace QRCoder.Unity
{
	// Token: 0x02000259 RID: 601
	public class UnityQRCode : AbstractQRCode
	{
		// Token: 0x0600139C RID: 5020 RVA: 0x0005D5CD File Offset: 0x0005B7CD
		public UnityQRCode()
		{
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0005D5D5 File Offset: 0x0005B7D5
		public UnityQRCode(QRCodeData data)
			: base(data)
		{
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0005D5DE File Offset: 0x0005B7DE
		public Texture2D GetGraphic(int pixelsPerModule)
		{
			return this.GetGraphic(pixelsPerModule, Color.black, Color.white);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0005D5F1 File Offset: 0x0005B7F1
		public Texture2D GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex)
		{
			return this.GetGraphic(pixelsPerModule, UnityQRCode.HexToColor(darkColorHtmlHex), UnityQRCode.HexToColor(lightColorHtmlHex));
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0005D608 File Offset: 0x0005B808
		public static Color HexToColor(string hexColor)
		{
			hexColor = hexColor.Replace("0x", "").Replace("#", "").Trim();
			byte b = byte.MaxValue;
			byte b2 = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
			byte b3 = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
			byte b4 = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
			if (hexColor.Length == 8)
			{
				b = byte.Parse(hexColor.Substring(6, 2), NumberStyles.HexNumber);
			}
			return new Color32(b2, b3, b4, b);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0005D6A4 File Offset: 0x0005B8A4
		public Texture2D GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor)
		{
			int num = base.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
			Texture2D texture2D = new Texture2D(num, num, TextureFormat.ARGB32, false);
			Color[] brush = this.GetBrush(pixelsPerModule, pixelsPerModule, darkColor);
			Color[] brush2 = this.GetBrush(pixelsPerModule, pixelsPerModule, lightColor);
			for (int i = 0; i < num; i += pixelsPerModule)
			{
				for (int j = 0; j < num; j += pixelsPerModule)
				{
					if (base.QrCodeData.ModuleMatrix[(j + pixelsPerModule) / pixelsPerModule - 1][(i + pixelsPerModule) / pixelsPerModule - 1])
					{
						texture2D.SetPixels(i, j, pixelsPerModule, pixelsPerModule, brush);
					}
					else
					{
						texture2D.SetPixels(i, j, pixelsPerModule, pixelsPerModule, brush2);
					}
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0005D74C File Offset: 0x0005B94C
		internal Color[] GetBrush(int sizeX, int sizeY, Color defaultColor)
		{
			int num = sizeX * sizeY;
			List<Color> list = new List<Color>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(defaultColor);
			}
			return list.ToArray();
		}
	}
}
