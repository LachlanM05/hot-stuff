using System;
using T17.Services;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class CursorLocker : Singleton<CursorLocker>
{
	// Token: 0x06000150 RID: 336 RVA: 0x000096AB File Offset: 0x000078AB
	public override void AwakeSingleton()
	{
		this.cursorlocklevel = 0;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		this.currentVisibleState = false;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x000096C7 File Offset: 0x000078C7
	public static bool IsLocked()
	{
		return !(Singleton<CursorLocker>.Instance == null) && Singleton<CursorLocker>.Instance.cursorlocklevel != 0 && Singleton<CursorLocker>.Instance.cursorlocklevel > 0;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x000096F1 File Offset: 0x000078F1
	public static void Reset()
	{
		Singleton<CursorLocker>.Instance.cursorlocklevel = 0;
		CursorLocker.UpdateCursorLockState();
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00009703 File Offset: 0x00007903
	public static void Unlock()
	{
		if (Singleton<CursorLocker>.Instance != null)
		{
			Singleton<CursorLocker>.Instance.cursorlocklevel++;
			CursorLocker.UpdateCursorLockState();
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00009729 File Offset: 0x00007929
	public static void Lock()
	{
		if (Singleton<CursorLocker>.Instance != null)
		{
			Singleton<CursorLocker>.Instance.cursorlocklevel = Mathf.Max(Singleton<CursorLocker>.Instance.cursorlocklevel - 1, 0);
			CursorLocker.UpdateCursorLockState();
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00009759 File Offset: 0x00007959
	public static void ForceLock()
	{
		if (Singleton<CursorLocker>.Instance != null)
		{
			Singleton<CursorLocker>.Instance.cursorlocklevel = 0;
			CursorLocker.UpdateCursorLockState();
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009778 File Offset: 0x00007978
	public static void UpdateCursorLockState()
	{
		Singleton<CursorLocker>.Instance.UpdateCursorLockState_Impl();
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00009784 File Offset: 0x00007984
	private void UpdateCursorLockState_Impl()
	{
		bool flag = Singleton<CursorLocker>.Instance.keyboardAndMouseActive & (Singleton<CursorLocker>.Instance.cursorlocklevel > 0);
		if (flag != this.currentVisibleState || flag != Cursor.visible)
		{
			this.currentVisibleState = flag;
			Cursor.lockState = (this.currentVisibleState ? CursorLockMode.Confined : CursorLockMode.Locked);
			Cursor.visible = this.currentVisibleState;
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000097E0 File Offset: 0x000079E0
	public void Update()
	{
		if (Services.InputService != null)
		{
			bool flag = Services.InputService.WasLastControllerAPointer();
			if (this.keyboardAndMouseActive != flag)
			{
				this.keyboardAndMouseActive = flag;
				CursorLocker.UpdateCursorLockState();
			}
		}
	}

	// Token: 0x0400025E RID: 606
	public int cursorlocklevel;

	// Token: 0x0400025F RID: 607
	public bool keyboardAndMouseActive;

	// Token: 0x04000260 RID: 608
	public bool currentVisibleState;
}
