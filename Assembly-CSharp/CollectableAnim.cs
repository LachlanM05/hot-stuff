using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003E RID: 62
public class CollectableAnim : Singleton<CollectableAnim>
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000144 RID: 324 RVA: 0x000094A2 File Offset: 0x000076A2
	// (set) Token: 0x06000145 RID: 325 RVA: 0x000094C4 File Offset: 0x000076C4
	private RectTransform Rect
	{
		get
		{
			if (this._rect == null)
			{
				this._rect = base.GetComponent<RectTransform>();
			}
			return this._rect;
		}
		set
		{
			this._rect = value;
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000094D0 File Offset: 0x000076D0
	public void StartHold(Sprite s, float anchoredXPosition)
	{
		this.SetAnchoredXPosition(anchoredXPosition);
		this.Col.SetActive(true);
		this.im.sprite = s;
		this.CollectionAnim.Play("UnlockCollectableHold");
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_collectable_appear, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00009534 File Offset: 0x00007734
	public void StartRelease()
	{
		this.Col.SetActive(true);
		this.CollectionAnim.Play("UnlockCollectableRelease");
		Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_collectable_leave, AUDIO_TYPE.SFX, false, false, 0f, false, 1f, null, false, SFX_SUBGROUP.UI, false);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00009583 File Offset: 0x00007783
	public void AlertObservers(string alert)
	{
		if (alert.Equals("ReleaseEnded"))
		{
			this.ShouldHideOnRelease();
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00009598 File Offset: 0x00007798
	public void ShouldHideOnRelease()
	{
		this.Col.SetActive(false);
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000095A6 File Offset: 0x000077A6
	public void startanim(Sprite s, float anchoredXPosition)
	{
		this.SetAnchoredXPosition(anchoredXPosition);
		this.Col.SetActive(true);
		this.im.sprite = s;
		this.CollectionAnim.Play("UnlockCollectable");
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000095D7 File Offset: 0x000077D7
	public void stopanim()
	{
		this.Col.SetActive(false);
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000095E8 File Offset: 0x000077E8
	private void SetAnchoredXPosition(float anchoredXPosition)
	{
		Vector2 anchoredPosition = this.Rect.anchoredPosition;
		anchoredPosition.x = anchoredXPosition;
		this.Rect.anchoredPosition = anchoredPosition;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00009615 File Offset: 0x00007815
	public void MoveAnchoredXPosition(float destinationAnchoredXPosition)
	{
		this._isMovingPosition = true;
		this._targetAnchoredXPosition = destinationAnchoredXPosition;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00009628 File Offset: 0x00007828
	public void Update()
	{
		if (this._isMovingPosition)
		{
			if (Mathf.Abs(this.Rect.anchoredPosition.x - this._targetAnchoredXPosition) > 0.1f)
			{
				Vector2 anchoredPosition = this.Rect.anchoredPosition;
				anchoredPosition.x = Mathf.Lerp(anchoredPosition.x, this._targetAnchoredXPosition, 6.66666f * Time.deltaTime);
				this.Rect.anchoredPosition = anchoredPosition;
				return;
			}
			this._isMovingPosition = false;
		}
	}

	// Token: 0x04000254 RID: 596
	private const float MOVE_POS_ANIM_SPEED = 6.66666f;

	// Token: 0x04000255 RID: 597
	public const string k_AnimRegular = "UnlockCollectable";

	// Token: 0x04000256 RID: 598
	public const string k_AnimHold = "UnlockCollectableHold";

	// Token: 0x04000257 RID: 599
	public const string k_AnimRelease = "UnlockCollectableRelease";

	// Token: 0x04000258 RID: 600
	public GameObject Col;

	// Token: 0x04000259 RID: 601
	public Image im;

	// Token: 0x0400025A RID: 602
	public Animator CollectionAnim;

	// Token: 0x0400025B RID: 603
	private bool _isMovingPosition;

	// Token: 0x0400025C RID: 604
	private float _targetAnchoredXPosition;

	// Token: 0x0400025D RID: 605
	private RectTransform _rect;
}
