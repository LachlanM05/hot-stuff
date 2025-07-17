using System;
using UnityEngine;

namespace T17.Flow
{
	// Token: 0x0200024B RID: 587
	public class ActivateGameObjectElement : IdentElement
	{
		// Token: 0x06001327 RID: 4903 RVA: 0x0005BED2 File Offset: 0x0005A0D2
		public override void StartElement()
		{
			if (this.valid)
			{
				this.target.SetActive(this.activate);
				this.finished = true;
			}
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0005BEF4 File Offset: 0x0005A0F4
		public override bool InitialiseElement()
		{
			if (this.target != null)
			{
				this.valid = true;
				this.target.SetActive(!this.activate);
			}
			this.finished = !this.valid;
			return this.finished;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0005BF34 File Offset: 0x0005A134
		public override void StopElement()
		{
			this.finished = true;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0005BF3D File Offset: 0x0005A13D
		public override void UpdateElement(float deltaTime)
		{
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0005BF3F File Offset: 0x0005A13F
		public override bool IsSkipable(bool beingForced)
		{
			return false;
		}

		// Token: 0x04000F03 RID: 3843
		public GameObject target;

		// Token: 0x04000F04 RID: 3844
		public bool activate = true;
	}
}
