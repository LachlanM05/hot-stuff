using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Team17.Scripts.Services
{
	// Token: 0x02000212 RID: 530
	public class EnsureIsBackupEventSystem : MonoBehaviour
	{
		// Token: 0x06001145 RID: 4421 RVA: 0x00057ADA File Offset: 0x00055CDA
		private void Awake()
		{
			this._backupEventSystem = base.GetComponent<EventSystem>();
			this.FindEventSystemsList();
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00057AEE File Offset: 0x00055CEE
		private void Update()
		{
			this.EnableBackupIfRequired();
			this.DisableBackupIfNoLongerRequired();
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00057AFC File Offset: 0x00055CFC
		private void DisableBackupIfNoLongerRequired()
		{
			if (this._eventSystems.Count > 1 && this._backupEventSystem.enabled)
			{
				this._backupEventSystem.enabled = false;
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00057B25 File Offset: 0x00055D25
		private void EnableBackupIfRequired()
		{
			if (this._eventSystems.Count == 0 && !this._backupEventSystem.enabled)
			{
				this._backupEventSystem.enabled = true;
			}
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00057B50 File Offset: 0x00055D50
		private void FindEventSystemsList()
		{
			FieldInfo field = typeof(EventSystem).GetField("m_EventSystems", BindingFlags.Static | BindingFlags.NonPublic);
			this._eventSystems = (List<EventSystem>)field.GetValue(null);
		}

		// Token: 0x04000E4D RID: 3661
		private List<EventSystem> _eventSystems;

		// Token: 0x04000E4E RID: 3662
		private EventSystem _backupEventSystem;
	}
}
