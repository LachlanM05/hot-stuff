using System;
using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001B1 RID: 433
public class ScrollInView : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	// Token: 0x06000EA9 RID: 3753 RVA: 0x000503EC File Offset: 0x0004E5EC
	private void Awake()
	{
		this.ourScrollRect = base.GetComponentInParent<ScrollRect>(true);
		this.ourTransform = (RectTransform)base.transform;
		this.ourButton = base.GetComponent<Button>();
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x00050418 File Offset: 0x0004E618
	public void OnEnable()
	{
		this.SetUpTrigger();
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00050420 File Offset: 0x0004E620
	public void OnDisable()
	{
		this.RemoveTrigger();
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00050428 File Offset: 0x0004E628
	public void SetTriggerOnPress(bool enableTrigger)
	{
		if (this.triggerOnButtonPress != enableTrigger)
		{
			this.triggerOnButtonPress = enableTrigger;
			if (enableTrigger)
			{
				this.SetUpTrigger();
				return;
			}
			this.RemoveTrigger();
		}
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x0005044A File Offset: 0x0004E64A
	public void onClicked()
	{
		this.AttemptScrollToSelf();
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00050454 File Offset: 0x0004E654
	private void SetUpTrigger()
	{
		if (!this.triggerSet && this.triggerOnButtonPress && this.ourButton != null)
		{
			this.ourButton.onClick.AddListener(new UnityAction(this.onClicked));
			this.triggerSet = true;
		}
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x000504A2 File Offset: 0x0004E6A2
	private void RemoveTrigger()
	{
		if (this.triggerSet)
		{
			if (this.ourButton != null)
			{
				this.ourButton.onClick.RemoveListener(new UnityAction(this.onClicked));
			}
			this.triggerSet = false;
		}
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x000504DD File Offset: 0x0004E6DD
	public void OnSelect(BaseEventData eventData)
	{
		if (!ControllerMenuUI.WasMovedManually)
		{
			return;
		}
		this.AttemptScrollToSelf();
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x000504ED File Offset: 0x0004E6ED
	public void ManualScrollToSelf()
	{
		this.AttemptScrollToSelf();
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x000504F8 File Offset: 0x0004E6F8
	private void AttemptScrollToSelf()
	{
		if (this.countDownToActive == 0 && this.ourScrollRect != null)
		{
			this.targetNormalizedPosition = this.CalculateFocusedScrollPosition(this.ourTransform);
			this.oldNormalizedPosition = this.ourScrollRect.normalizedPosition;
			this.totalContents = this.ourScrollRect.content.childCount;
			this.interp = 0f;
			this.scrolling = this.MovingTowardPosition();
			return;
		}
		if (this.countDownToActive == 0 && base.GetComponentInParent<CircularScrollingList>())
		{
			base.GetComponentInParent<CircularScrollingList>().SelectContentID(base.GetComponent<ListBox>().ContentID, true);
		}
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x00050598 File Offset: 0x0004E798
	public void Update()
	{
		if (this.countDownToActive > 0)
		{
			this.countDownToActive--;
		}
		if (this.scrolling)
		{
			this.scrolling = this.MovingTowardPosition();
			bool flag = this.scrolling;
		}
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x000505CC File Offset: 0x0004E7CC
	private bool MovingTowardPosition()
	{
		if (this.totalContents != this.ourScrollRect.content.childCount)
		{
			return false;
		}
		if (EventSystem.current != null && (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetInstanceID() != base.gameObject.GetInstanceID()))
		{
			return false;
		}
		this.interp = Mathf.Clamp01(this.interp + 5f * Time.unscaledDeltaTime);
		this.ourScrollRect.normalizedPosition = Vector2.Lerp(this.oldNormalizedPosition, this.targetNormalizedPosition, this.interp);
		return this.interp != 1f;
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00050680 File Offset: 0x0004E880
	public Vector2 CalculateFocusedScrollPosition(Vector2 focusPoint)
	{
		Vector2 size = this.ourScrollRect.content.rect.size;
		Vector2 size2 = ((RectTransform)this.ourScrollRect.content.parent).rect.size;
		Vector2 vector = this.ourScrollRect.content.localScale;
		size.Scale(vector);
		focusPoint.Scale(vector);
		Vector2 normalizedPosition = this.ourScrollRect.normalizedPosition;
		if (this.ourScrollRect.horizontal && size.x > size2.x)
		{
			normalizedPosition.x = Mathf.Clamp01((focusPoint.x - size2.x * 0.5f) / (size.x - size2.x));
		}
		if (this.ourScrollRect.vertical && size.y > size2.y)
		{
			normalizedPosition.y = Mathf.Clamp01((focusPoint.y - size2.y * 0.5f) / (size.y - size2.y));
		}
		return normalizedPosition;
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00050790 File Offset: 0x0004E990
	public Vector2 CalculateFocusedScrollPosition(RectTransform item)
	{
		Vector2 vector = this.ourScrollRect.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));
		Vector2 size = this.ourScrollRect.content.rect.size;
		size.Scale(this.ourScrollRect.content.pivot);
		return this.CalculateFocusedScrollPosition(vector + size);
	}

	// Token: 0x04000CFA RID: 3322
	[SerializeField]
	private bool triggerOnButtonPress;

	// Token: 0x04000CFB RID: 3323
	private ScrollRect ourScrollRect;

	// Token: 0x04000CFC RID: 3324
	private RectTransform ourTransform;

	// Token: 0x04000CFD RID: 3325
	private Vector2 oldNormalizedPosition = Vector2.zero;

	// Token: 0x04000CFE RID: 3326
	private Vector2 targetNormalizedPosition = Vector2.zero;

	// Token: 0x04000CFF RID: 3327
	private bool scrolling;

	// Token: 0x04000D00 RID: 3328
	private const float speed = 5f;

	// Token: 0x04000D01 RID: 3329
	private float interp;

	// Token: 0x04000D02 RID: 3330
	private int totalContents;

	// Token: 0x04000D03 RID: 3331
	private int countDownToActive = 10;

	// Token: 0x04000D04 RID: 3332
	private Button ourButton;

	// Token: 0x04000D05 RID: 3333
	private bool triggerSet;
}
