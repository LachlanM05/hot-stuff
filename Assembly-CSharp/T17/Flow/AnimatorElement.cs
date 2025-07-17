using System;
using Team17.Common;
using UnityEngine;

namespace T17.Flow
{
	// Token: 0x0200024C RID: 588
	public class AnimatorElement : IdentElement
	{
		// Token: 0x0600132D RID: 4909 RVA: 0x0005BF54 File Offset: 0x0005A154
		public override void StartElement()
		{
			if (this.valid)
			{
				if (!this._Animator.gameObject.activeSelf)
				{
					this._Animator.gameObject.SetActive(true);
				}
				if (!this._Animator.enabled)
				{
					this._Animator.enabled = true;
				}
				if (!string.IsNullOrWhiteSpace(this._AnimationState))
				{
					this._Animator.Play(this._AnimationState, -1, 0f);
				}
				VoiceOverCharacter component = base.gameObject.GetComponent<VoiceOverCharacter>();
				if (component != null)
				{
					component.PlayVoiceOver(2f);
				}
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0005BFEC File Offset: 0x0005A1EC
		public override bool InitialiseElement()
		{
			this.valid = true;
			if (this._Animator == null)
			{
				T17Debug.LogError("AnimatorElement: No animator specified ");
				this.valid = false;
			}
			else
			{
				if (this._Animator.gameObject.activeSelf)
				{
					this._Animator.gameObject.SetActive(false);
				}
				if (this._Animator.enabled)
				{
					this._Animator.enabled = false;
				}
			}
			this.finished = !this.valid;
			return this.finished;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0005C072 File Offset: 0x0005A272
		public override void StopElement()
		{
			if (this.valid)
			{
				if (this._Animator.gameObject.activeSelf)
				{
					this._Animator.gameObject.SetActive(false);
				}
				this.finished = true;
			}
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0005C0A6 File Offset: 0x0005A2A6
		public override void UpdateElement(float deltaTime)
		{
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0005C0A8 File Offset: 0x0005A2A8
		public void AnimationReachedEnd()
		{
			this.finished = true;
		}

		// Token: 0x04000F05 RID: 3845
		[SerializeField]
		private Animator _Animator;

		// Token: 0x04000F06 RID: 3846
		[Tooltip("Set the anime state to start in or leave blank")]
		[SerializeField]
		private string _AnimationState = "";
	}
}
