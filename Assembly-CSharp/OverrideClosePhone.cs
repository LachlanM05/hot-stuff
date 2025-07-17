using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AF RID: 431
public class OverrideClosePhone : MonoBehaviour
{
	// Token: 0x06000E97 RID: 3735 RVA: 0x0005010B File Offset: 0x0004E30B
	public bool InvokeClose()
	{
		if (this.CloseMethod != null)
		{
			this.CloseMethod.Invoke();
			return true;
		}
		return false;
	}

	// Token: 0x04000CF4 RID: 3316
	[SerializeField]
	private Button.ButtonClickedEvent CloseMethod;
}
