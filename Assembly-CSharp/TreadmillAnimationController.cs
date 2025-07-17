using System;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class TreadmillAnimationController : MonoBehaviour
{
	// Token: 0x06000D36 RID: 3382 RVA: 0x0004BC9C File Offset: 0x00049E9C
	private void Start()
	{
		this._scrollPropertyId = Shader.PropertyToID(this.MaterialScrollPropertyName);
		this._currentState = TreadmillAnimationController.BeltState.Off;
		this._currentScrollSpeed = 0f;
		this._currentScrollTime = 0f;
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0004BCCC File Offset: 0x00049ECC
	private void Update()
	{
		if (this._currentState != TreadmillAnimationController.BeltState.Off)
		{
			if (this._currentState == TreadmillAnimationController.BeltState.WarmUp)
			{
				this._currentScrollSpeed -= this.WarmupSpeed * Time.deltaTime;
				if (this._currentScrollSpeed <= this.ScrollSpeed)
				{
					this._currentScrollSpeed = this.ScrollSpeed;
					this._currentState = TreadmillAnimationController.BeltState.On;
				}
			}
			else if (this._currentState == TreadmillAnimationController.BeltState.CoolDown)
			{
				this._currentScrollSpeed += this.WarmupSpeed * Time.deltaTime;
				if (this._currentScrollSpeed >= 0f)
				{
					this._currentScrollSpeed = 0f;
					this._currentState = TreadmillAnimationController.BeltState.Off;
				}
			}
			this._currentScrollTime -= this._currentScrollSpeed * Time.deltaTime;
			if (this._currentScrollTime <= -1f)
			{
				this._currentScrollTime += 1f;
			}
			this.TreadmillBeltMesh.sharedMaterials[0].SetFloat(this._scrollPropertyId, this._currentScrollTime);
		}
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x0004BDC0 File Offset: 0x00049FC0
	public void OnTreadmillTurnOn()
	{
		if (this._currentState == TreadmillAnimationController.BeltState.Off || this._currentState == TreadmillAnimationController.BeltState.CoolDown)
		{
			this._currentState = TreadmillAnimationController.BeltState.WarmUp;
		}
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x0004BDDA File Offset: 0x00049FDA
	public void OnTreadmillLoop()
	{
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0004BDDC File Offset: 0x00049FDC
	public void OnTreadmillTurnOff()
	{
		if (this._currentState == TreadmillAnimationController.BeltState.On || this._currentState == TreadmillAnimationController.BeltState.WarmUp)
		{
			this._currentState = TreadmillAnimationController.BeltState.CoolDown;
		}
	}

	// Token: 0x04000BF4 RID: 3060
	[SerializeField]
	private SkinnedMeshRenderer TreadmillBeltMesh;

	// Token: 0x04000BF5 RID: 3061
	[SerializeField]
	private string MaterialScrollPropertyName = "_Scroll_Time";

	// Token: 0x04000BF6 RID: 3062
	[SerializeField]
	private float ScrollSpeed = -2f;

	// Token: 0x04000BF7 RID: 3063
	[SerializeField]
	private float WarmupSpeed = -0.1f;

	// Token: 0x04000BF8 RID: 3064
	private int _scrollPropertyId;

	// Token: 0x04000BF9 RID: 3065
	private float _currentScrollTime;

	// Token: 0x04000BFA RID: 3066
	private float _currentScrollSpeed;

	// Token: 0x04000BFB RID: 3067
	private TreadmillAnimationController.BeltState _currentState;

	// Token: 0x02000368 RID: 872
	private enum BeltState
	{
		// Token: 0x0400136B RID: 4971
		Off,
		// Token: 0x0400136C RID: 4972
		WarmUp,
		// Token: 0x0400136D RID: 4973
		On,
		// Token: 0x0400136E RID: 4974
		CoolDown
	}
}
