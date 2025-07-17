using System;
using UnityEngine;

namespace T17.Flow
{
	// Token: 0x0200024E RID: 590
	public abstract class IdentElement : MonoBehaviour
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x0005C2BF File Offset: 0x0005A4BF
		public virtual bool Finished
		{
			get
			{
				return this.finished;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600133A RID: 4922 RVA: 0x0005C2C7 File Offset: 0x0005A4C7
		public bool Valid
		{
			get
			{
				return this.valid;
			}
		}

		// Token: 0x0600133B RID: 4923
		public abstract bool InitialiseElement();

		// Token: 0x0600133C RID: 4924
		public abstract void StartElement();

		// Token: 0x0600133D RID: 4925
		public abstract void StopElement();

		// Token: 0x0600133E RID: 4926
		public abstract void UpdateElement(float deltaTime);

		// Token: 0x0600133F RID: 4927 RVA: 0x0005C2CF File Offset: 0x0005A4CF
		public virtual bool IsSkipable(bool beingForced)
		{
			return this.skipable || beingForced;
		}

		// Token: 0x04000F0C RID: 3852
		[SerializeField]
		public bool skipable = true;

		// Token: 0x04000F0D RID: 3853
		protected bool finished;

		// Token: 0x04000F0E RID: 3854
		protected bool valid = true;
	}
}
