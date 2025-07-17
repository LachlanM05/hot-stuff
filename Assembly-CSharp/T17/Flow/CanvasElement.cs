using System;
using Team17.Common;
using UnityEngine;

namespace T17.Flow
{
	// Token: 0x0200024D RID: 589
	public class CanvasElement : IdentElement
	{
		// Token: 0x06001333 RID: 4915 RVA: 0x0005C0C4 File Offset: 0x0005A2C4
		public override void StartElement()
		{
			if (this.valid)
			{
				this.elapsedTime = 0f;
				if (!this._parentGameObject.gameObject.activeSelf)
				{
					this._parentGameObject.gameObject.SetActive(true);
				}
				this._canvasGroup.alpha = this.alphaCurve.Evaluate(0f);
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0005C124 File Offset: 0x0005A324
		public override bool InitialiseElement()
		{
			this.valid = true;
			if (this._canvasGroup == null)
			{
				T17Debug.LogError("CanvasElement: No canvas group specified ");
				this.valid = false;
			}
			if (this._parentGameObject == null)
			{
				T17Debug.LogError("CanvasElement: No parent game object specified ");
				this.valid = false;
			}
			if (this.duration <= 0f)
			{
				T17Debug.LogError("CanvasElement: Must last longer than 0 seconds");
				this.valid = false;
			}
			if (this.valid)
			{
				if (this._parentGameObject.gameObject.activeSelf)
				{
					this._parentGameObject.gameObject.SetActive(false);
				}
				this._canvasGroup.alpha = 0f;
			}
			this.finished = !this.valid;
			return this.finished;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0005C1E4 File Offset: 0x0005A3E4
		public override void StopElement()
		{
			if (this.valid)
			{
				if (this._parentGameObject.gameObject.activeSelf)
				{
					this._parentGameObject.gameObject.SetActive(false);
				}
				this.finished = true;
			}
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0005C218 File Offset: 0x0005A418
		public override void UpdateElement(float deltaTime)
		{
			if (!this.finished && this.valid)
			{
				this.UpdateCanvas(Time.deltaTime);
			}
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0005C238 File Offset: 0x0005A438
		private void UpdateCanvas(float deltaTime)
		{
			this.elapsedTime = Mathf.Min(this.elapsedTime += deltaTime, this.duration);
			float num = this.elapsedTime / this.duration;
			this._canvasGroup.alpha = this.alphaCurve.Evaluate(num);
			if (this.elapsedTime == this.duration)
			{
				this.finished = true;
			}
		}

		// Token: 0x04000F07 RID: 3847
		[SerializeField]
		private CanvasGroup _canvasGroup;

		// Token: 0x04000F08 RID: 3848
		[SerializeField]
		private GameObject _parentGameObject;

		// Token: 0x04000F09 RID: 3849
		[SerializeField]
		private AnimationCurve alphaCurve = new AnimationCurve();

		// Token: 0x04000F0A RID: 3850
		[Tooltip("Time in seconds the element lasts")]
		[SerializeField]
		private float duration = 1f;

		// Token: 0x04000F0B RID: 3851
		private float elapsedTime;
	}
}
