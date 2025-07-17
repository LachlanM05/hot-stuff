using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Scripts
{
	// Token: 0x020001E7 RID: 487
	[ExecuteInEditMode]
	public class UniqueId : MonoBehaviour
	{
		// Token: 0x0600105D RID: 4189 RVA: 0x00055AB0 File Offset: 0x00053CB0
		public int GetID()
		{
			return this.hashedID;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00055AB8 File Offset: 0x00053CB8
		private void Awake()
		{
			this.hashedID = this.uniqueId.GetHashCode();
		}

		// Token: 0x04000DEB RID: 3563
		private static readonly Dictionary<string, UniqueId> allGuids = new Dictionary<string, UniqueId>();

		// Token: 0x04000DEC RID: 3564
		[NotEditable]
		public string uniqueId = "";

		// Token: 0x04000DED RID: 3565
		[SerializeField]
		[NotEditable]
		private int hashedID;
	}
}
