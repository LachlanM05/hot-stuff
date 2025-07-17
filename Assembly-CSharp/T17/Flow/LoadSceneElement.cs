using System;
using Team17.Common;
using UnityEngine;

namespace T17.Flow
{
	// Token: 0x0200024F RID: 591
	public class LoadSceneElement : IdentElement
	{
		// Token: 0x06001341 RID: 4929 RVA: 0x0005C2EF File Offset: 0x0005A4EF
		public override void StartElement()
		{
			if (this.valid)
			{
				if (!SceneTransitionManager.TransitionToScene(this.sceneToLoad))
				{
					T17Debug.LogError("Requested load scene (" + this.sceneToLoad + ") Failed");
				}
				this.finished = true;
			}
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0005C328 File Offset: 0x0005A528
		public override bool InitialiseElement()
		{
			this.skipable = false;
			this.valid = true;
			if (string.IsNullOrWhiteSpace(this.sceneToLoad))
			{
				T17Debug.LogError("LoadSceneElement: No scene specified ");
				this.valid = false;
			}
			this.finished = !this.valid;
			return this.finished;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0005C376 File Offset: 0x0005A576
		public override void StopElement()
		{
			if (this.valid)
			{
				this.finished = true;
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0005C387 File Offset: 0x0005A587
		public override void UpdateElement(float deltaTime)
		{
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0005C389 File Offset: 0x0005A589
		public override bool IsSkipable(bool beingForced)
		{
			return false;
		}

		// Token: 0x04000F0F RID: 3855
		[SerializeField]
		private string sceneToLoad = "";
	}
}
