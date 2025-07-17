using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200025A RID: 602
	[CreateAssetMenu(fileName = "Input Field Validator", menuName = "Input Field Validator")]
	public class CalendarValidator : TMP_InputValidator
	{
		// Token: 0x060013A3 RID: 5027 RVA: 0x0005D780 File Offset: 0x0005B980
		public override char Validate(ref string text, ref int pos, char ch)
		{
			if (ch.Equals('/') && text.Length == 1)
			{
				text = "0" + text + "/";
				pos += 2;
				return '\0';
			}
			if (ch.Equals('/') && text.Length == 4)
			{
				string text2 = text.Substring(0, 3);
				text = string.Format("{0}0{1}/", text2, text[3]);
				pos += 2;
				return '\0';
			}
			if (char.IsNumber(ch) && text.Length < 10)
			{
				text = text.Insert(pos, ch.ToString());
				pos++;
				string text3 = text.Replace("/", "");
				text.StartsWith("0");
				int num = int.Parse(text3);
				if (num.ToString().Length > 8)
				{
					num = int.Parse(num.ToString().Substring(0, 8));
				}
				int length = text3.Length;
				if (length == 1)
				{
					text = string.Format("{0:#}", num).PadLeft(1, '0');
				}
				else if (length == 2)
				{
					text = string.Format("{0:##/}", num).PadLeft(3, '0');
				}
				else if (length == 3)
				{
					text = string.Format("{0:##/#}", num).PadLeft(4, '0');
				}
				else if (length == 4)
				{
					text = string.Format("{0:##/##/}", num).PadLeft(6, '0');
				}
				else if (length == 5)
				{
					text = string.Format("{0:##/##/#}", num).PadLeft(7, '0');
				}
				else if (length == 6)
				{
					text = string.Format("{0:##/##/##}", num).PadLeft(8, '0');
				}
				else if (length == 7)
				{
					text = string.Format("{0:##/##/###}", num).PadLeft(9, '0');
				}
				else if (length == 8)
				{
					text = string.Format("{0:##/##/####}", num).PadLeft(10, '0');
				}
				if (pos == 2 || pos == 5)
				{
					pos++;
				}
				return ch;
			}
			return '\0';
		}
	}
}
