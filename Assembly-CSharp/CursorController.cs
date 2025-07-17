using System;
using T17.Services;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class CursorController : MonoBehaviour
{
	// Token: 0x06000E3C RID: 3644 RVA: 0x0004EAB7 File Offset: 0x0004CCB7
	public void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		this.currentVisibleState = false;
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0004EACC File Offset: 0x0004CCCC
	public void Update()
	{
		if (Services.InputService != null)
		{
			bool flag = !Services.InputService.IsLastActiveInputController();
			if (this.keyboardAndMouseActive != flag)
			{
				this.keyboardAndMouseActive = flag;
				Cursor.lockState = (this.keyboardAndMouseActive ? CursorLockMode.Confined : CursorLockMode.Locked);
				Cursor.visible = this.keyboardAndMouseActive;
			}
		}
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0004EB1A File Offset: 0x0004CD1A
	public void ForceHideCursor()
	{
		this.keyboardAndMouseActive = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = this.keyboardAndMouseActive;
	}

	// Token: 0x04000C9E RID: 3230
	public bool keyboardAndMouseActive;

	// Token: 0x04000C9F RID: 3231
	public bool currentVisibleState;
}
