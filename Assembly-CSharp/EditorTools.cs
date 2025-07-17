using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class EditorTools : Singleton<EditorTools>
{
	// Token: 0x06000201 RID: 513 RVA: 0x0000C149 File Offset: 0x0000A349
	private void OnEnable()
	{
		Object.DontDestroyOnLoad(this);
		if (!Debug.isDebugBuild)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000C163 File Offset: 0x0000A363
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			if (!this.active)
			{
				this.active = true;
				CursorLocker.Unlock();
				return;
			}
			this.active = false;
			CursorLocker.Lock();
		}
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000C192 File Offset: 0x0000A392
	public void GoForwardDay()
	{
		Singleton<DayNightCycle>.Instance.ForceIncrementDay();
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000C19E File Offset: 0x0000A39E
	public void GoForwardTime()
	{
		Singleton<DayNightCycle>.Instance.ForceIncrementTime();
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000C1AA File Offset: 0x0000A3AA
	public void Sleep()
	{
		Object.FindObjectOfType<Bed>().Interact();
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000C1B6 File Offset: 0x0000A3B6
	public void Recharge()
	{
		Singleton<Dateviators>.Instance.ResetCharges();
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
	public void UnlockDateADex()
	{
		for (int i = 0; i < 102; i++)
		{
			Singleton<Save>.Instance.SetDateStatus(i, RelationshipStatus.Single);
		}
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000C1EC File Offset: 0x0000A3EC
	public void UnlockCollectables()
	{
		for (int i = 0; i < 102; i++)
		{
			Singleton<Save>.Instance.UnlockCollectable(DateADex.Instance.GetCharName(i), 1);
			Singleton<Save>.Instance.UnlockCollectable(DateADex.Instance.GetCharName(i), 2);
			Singleton<Save>.Instance.UnlockCollectable(DateADex.Instance.GetCharName(i), 3);
		}
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000C248 File Offset: 0x0000A448
	public void CollectableAnim()
	{
		DateADex.Instance.TestCollectable();
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000C254 File Offset: 0x0000A454
	public void LockAll()
	{
		for (int i = 0; i < 102; i++)
		{
			Singleton<Save>.Instance.SetDateStatus(i, RelationshipStatus.Unmet);
		}
		Singleton<Save>.Instance.ResetCollectables();
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000C284 File Offset: 0x0000A484
	public void UnlockTalkedTo()
	{
		Singleton<InteractableManager>.Instance.ResetTalkedTo();
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000C290 File Offset: 0x0000A490
	private void OnGUI()
	{
		if (!this.active)
		{
			return;
		}
		int num = 25;
		int num2 = 0;
		int num3 = num;
		int num4 = 120;
		foreach (command command in this.commands)
		{
			if (GUI.Button(new Rect(new Vector2((float)num2, (float)num3), Vector2.one * (float)num4), command.icon))
			{
				command.ueve.Invoke();
			}
			num2 += num4;
			if (num2 + command.icon.width >= Screen.width)
			{
				num2 = 0;
				num3 = num3 + command.icon.height + num;
			}
		}
	}

	// Token: 0x040002E5 RID: 741
	public List<command> commands;

	// Token: 0x040002E6 RID: 742
	public bool active;
}
