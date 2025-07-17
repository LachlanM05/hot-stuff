using System;
using System.Collections.Generic;
using Team17.Common;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class OneTimeOnlyEnabler : MonoBehaviour
{
	// Token: 0x06000E8B RID: 3723 RVA: 0x0004FF80 File Offset: 0x0004E180
	private void Awake()
	{
		if (string.IsNullOrWhiteSpace(this.ID))
		{
			T17Debug.LogError("OneTimeOnlyEnabler needs an ID to work");
			return;
		}
		if (!OneTimeOnlyEnabler.s_PreviouslyEnabled.Contains(this.ID.ToLowerInvariant()))
		{
			OneTimeOnlyEnabler.s_PreviouslyEnabled.Add(this.ID.ToLowerInvariant());
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000CED RID: 3309
	private static List<string> s_PreviouslyEnabled = new List<string>(10);

	// Token: 0x04000CEE RID: 3310
	public string ID = "";
}
