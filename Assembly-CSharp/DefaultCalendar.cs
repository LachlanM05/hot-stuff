using System;
using System.Globalization;
using TMPro;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class DefaultCalendar : MonoBehaviour
{
	// Token: 0x060008D0 RID: 2256 RVA: 0x000341BC File Offset: 0x000323BC
	private void Start()
	{
		if (CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern.ToLowerInvariant().StartsWith("m"))
		{
			this.inputField.SetTextWithoutNotify("MM/DD/YYYY");
		}
		else
		{
			this.inputField.SetTextWithoutNotify("DD/MM/YYYY");
		}
		this.inputField.textComponent.color = new Color(0.827451f, 0.827451f, 0.827451f);
	}

	// Token: 0x0400080C RID: 2060
	public TMP_InputField inputField;

	// Token: 0x0400080D RID: 2061
	private Color textColor;
}
