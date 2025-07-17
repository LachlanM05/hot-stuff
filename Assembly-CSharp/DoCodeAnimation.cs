using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Team17.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001A RID: 26
public class DoCodeAnimation : MonoBehaviour
{
	// Token: 0x06000074 RID: 116 RVA: 0x000037D0 File Offset: 0x000019D0
	public bool IsAnimatingAtIndex(int index)
	{
		return index < this.animations.Count && this.animations[index].InProgress;
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000075 RID: 117 RVA: 0x000037F4 File Offset: 0x000019F4
	public bool IsAnimating
	{
		get
		{
			int i = 0;
			int count = this.animations.Count;
			while (i < count)
			{
				if (this.animations[i].InProgress)
				{
					return true;
				}
				i++;
			}
			return false;
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00003830 File Offset: 0x00001A30
	private void Start()
	{
		if (this.objectToAnimate == null)
		{
			this.objectToAnimate = base.gameObject;
		}
		for (int i = 0; i < this.animations.Count; i++)
		{
			if (this.animations[i].id <= 0)
			{
				DoCodeAnimation.AnimationVitals animationVitals = this.animations[i];
				animationVitals.InProgress = false;
				animationVitals.id = i + 1;
				this.animations[i] = animationVitals;
			}
			if (this.animations[i].triggerOnStart)
			{
				this.TriggerAnimation(i, false);
			}
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x000038C8 File Offset: 0x00001AC8
	private void OnEnable()
	{
		if (this.objectToAnimate == null)
		{
			this.objectToAnimate = base.gameObject;
		}
		if (this.stopAllCoroutinesOnEnable)
		{
			base.StopAllCoroutines();
		}
		for (int i = 0; i < this.animations.Count; i++)
		{
			if (this.animations[i].id <= 0)
			{
				DoCodeAnimation.AnimationVitals animationVitals = this.animations[i];
				animationVitals.InProgress = false;
				animationVitals.id = i + 1;
				this.animations[i] = animationVitals;
			}
			if (this.animations[i].triggerOnEnable)
			{
				this.TriggerAnimation(i, false);
			}
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000396E File Offset: 0x00001B6E
	public void Trigger()
	{
		this.TriggerAnimationAtIndex(0);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00003978 File Offset: 0x00001B78
	public void TriggerAll()
	{
		for (int i = 0; i < this.animations.Count; i++)
		{
			this.TriggerAnimation(i, false);
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x000039A3 File Offset: 0x00001BA3
	public void TriggerAnimationAtIndex(int index)
	{
		this.TriggerAnimation(index, false);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000039AD File Offset: 0x00001BAD
	public void TriggerAnimationAtIndexImmediatly(int index)
	{
		this.TriggerAnimation(index, true);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000039B8 File Offset: 0x00001BB8
	public void SetAllOnEnable()
	{
		for (int i = 0; i < this.animations.Count; i++)
		{
			DoCodeAnimation.AnimationVitals animationVitals = this.animations[i];
			animationVitals.triggerOnEnable = true;
			this.animations[i] = animationVitals;
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00003A00 File Offset: 0x00001C00
	public void SetAllFlagsOff()
	{
		for (int i = 0; i < this.animations.Count; i++)
		{
			DoCodeAnimation.AnimationVitals animationVitals = this.animations[i];
			animationVitals.triggerOnEnable = false;
			animationVitals.triggerOnStart = false;
			this.animations[i] = animationVitals;
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00003A50 File Offset: 0x00001C50
	private void TriggerAnimation(int animationIndex, bool now = false)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (animationIndex >= this.animations.Count)
		{
			return;
		}
		DoCodeAnimation.AnimationVitals animationVitals = this.animations[animationIndex];
		animationVitals.active = true;
		animationVitals.InProgress = true;
		this.animations[animationIndex] = animationVitals;
		switch (animationVitals.type)
		{
		case DoCodeAnimation.AnimationType.SlideInWaitForFrame:
			base.StartCoroutine(this.DelayThenMoveInAfterFrame(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.SlideOut:
			base.StartCoroutine(this.DelayThenMoveOut(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.PunchScale:
			base.StartCoroutine(this.DelayThenPunchScale(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.Scale:
			base.StartCoroutine(this.DelayThenScale(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.ShakeScale:
			base.StartCoroutine(this.DelayThenShakeScale(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.FadeIn:
			base.StartCoroutine(this.DelayThenFadeIn(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.FadeInCanvasGroup:
			base.StartCoroutine(this.DelayThenFadeInCanvasGroup(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.SlideIn:
			base.StartCoroutine(this.DelayThenMoveIn(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.FadeOut:
			base.StartCoroutine(this.DelayThenFadeOut(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.FadeOutCanvasGroup:
			base.StartCoroutine(this.DelayThenFadeOutCanvasGroup(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.ImageFillGrow:
			base.StartCoroutine(this.DelayThenFillImage(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.SlideInLocal:
			base.StartCoroutine(this.DelayThenMoveInLocal(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.SlideInWaitForFrameLocal:
			base.StartCoroutine(this.DelayThenMoveInAfterFrameLocal(animationVitals, now));
			break;
		case DoCodeAnimation.AnimationType.SlideOutLocal:
			base.StartCoroutine(this.DelayThenMoveOutLocal(animationVitals, now));
			break;
		}
		if (animationVitals.removeAfterActivated)
		{
			this.animations.RemoveAt(animationIndex);
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00003C00 File Offset: 0x00001E00
	private Vector3 GetStartVector(CompassDirections direction, float percentageStart = 0f)
	{
		Vector3 zero = Vector3.zero;
		switch (direction)
		{
		case CompassDirections.north:
			zero.y = this.objectToAnimate.transform.position.y / 100f * percentageStart;
			break;
		case CompassDirections.south:
			zero.y = -(this.objectToAnimate.transform.position.y / 100f * percentageStart);
			break;
		case CompassDirections.east:
			zero.x = this.objectToAnimate.transform.position.x / 100f * percentageStart;
			break;
		case CompassDirections.west:
			zero.x = -(this.objectToAnimate.transform.position.x / 100f * percentageStart);
			break;
		default:
			zero = new Vector3(this.objectToAnimate.transform.position.x / 100f * percentageStart, this.objectToAnimate.transform.position.y / 100f * percentageStart, this.objectToAnimate.transform.position.z / 100f * percentageStart);
			break;
		}
		return zero;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00003D34 File Offset: 0x00001F34
	private Vector3 GetStartVectorLocal(CompassDirections direction, float percentageStart = 0f)
	{
		Vector3 zero = Vector3.zero;
		switch (direction)
		{
		case CompassDirections.north:
			zero.y = this.objectToAnimate.transform.localPosition.y / 100f * percentageStart;
			break;
		case CompassDirections.south:
			zero.y = -(this.objectToAnimate.transform.localPosition.y / 100f * percentageStart);
			break;
		case CompassDirections.east:
			zero.x = this.objectToAnimate.transform.localPosition.x / 100f * percentageStart;
			break;
		case CompassDirections.west:
			zero.x = -(this.objectToAnimate.transform.localPosition.x / 100f * percentageStart);
			break;
		default:
			zero = new Vector3(this.objectToAnimate.transform.localPosition.x / 100f * percentageStart, this.objectToAnimate.transform.localPosition.y / 100f * percentageStart, this.objectToAnimate.transform.localPosition.z / 100f * percentageStart);
			break;
		}
		return zero;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00003E66 File Offset: 0x00002066
	private Vector3 GetScale(Vector3 scale, float percentage)
	{
		return new Vector3(scale.x / 100f * percentage, scale.y / 100f * percentage, scale.z / 100f * percentage);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00003E97 File Offset: 0x00002097
	private Vector3 SetPosition(Vector3 vectorAddition, Vector3 currentPosition)
	{
		return new Vector3(currentPosition.x + vectorAddition.x, currentPosition.y + vectorAddition.y, currentPosition.z + vectorAddition.z);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00003EC5 File Offset: 0x000020C5
	private IEnumerator DelayThenMoveInAfterFrame(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.SlideInWaitForFrame))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.SlideInWaitForFrame]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.SlideInWaitForFrame);
		}
		yield return new WaitForEndOfFrame();
		animation.endPosition = this.objectToAnimate.transform.position;
		this.objectToAnimate.transform.position = this.SetPosition(this.GetStartVector(animation.direction, animation.percentageStart), this.objectToAnimate.transform.position);
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = this.objectToAnimate.transform.DOMove(animation.endPosition, animation.speed, false).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.SlideInWaitForFrame, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.position));
		yield break;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00003EDB File Offset: 0x000020DB
	private IEnumerator DelayThenMoveIn(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.SlideIn))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.SlideIn]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.SlideIn);
		}
		if (animation.percentageStart <= animation.percentageEnd)
		{
			yield return new WaitForEndOfFrame();
		}
		animation.endPosition = this.objectToAnimate.transform.position;
		this.objectToAnimate.transform.position = this.SetPosition(this.GetStartVector(animation.direction, animation.percentageStart), this.objectToAnimate.transform.position);
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = this.objectToAnimate.transform.DOMove(animation.endPosition, animation.speed, false).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.SlideIn, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.position));
		yield break;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00003EF1 File Offset: 0x000020F1
	private IEnumerator DelayThenMoveInAfterFrameLocal(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.SlideInWaitForFrameLocal))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.SlideInWaitForFrameLocal]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.SlideInWaitForFrameLocal);
		}
		yield return new WaitForEndOfFrame();
		animation.endPosition = this.objectToAnimate.transform.localPosition;
		this.objectToAnimate.transform.localPosition = this.SetPosition(this.GetStartVectorLocal(animation.direction, animation.percentageStart), this.objectToAnimate.transform.localPosition);
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = this.objectToAnimate.transform.DOLocalMove(animation.endPosition, animation.speed, false).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.SlideInWaitForFrameLocal, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.localPosition));
		yield break;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00003F07 File Offset: 0x00002107
	private IEnumerator DelayThenMoveInLocal(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.SlideInLocal))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.SlideInLocal]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.SlideInLocal);
		}
		if (animation.percentageStart <= animation.percentageEnd)
		{
			yield return new WaitForEndOfFrame();
		}
		animation.endPosition = this.objectToAnimate.transform.localPosition;
		this.objectToAnimate.transform.localPosition = this.SetPosition(this.GetStartVectorLocal(animation.direction, animation.percentageStart), this.objectToAnimate.transform.localPosition);
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = this.objectToAnimate.transform.DOLocalMove(animation.endPosition, animation.speed, false).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.SlideInLocal, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.localPosition));
		yield break;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00003F1D File Offset: 0x0000211D
	private IEnumerator DelayThenMoveOut(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.SlideOut))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.SlideOut]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.SlideOut);
		}
		animation.endPosition = this.SetPosition(this.GetStartVector(animation.direction, animation.percentageStart), this.objectToAnimate.transform.position);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = this.objectToAnimate.transform.DOMove(animation.endPosition, animation.speed, false).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.SlideOut, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.position));
		yield break;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00003F33 File Offset: 0x00002133
	private IEnumerator DelayThenMoveOutLocal(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.SlideOutLocal))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.SlideOutLocal]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.SlideOutLocal);
		}
		animation.endPosition = this.SetPosition(this.GetStartVectorLocal(animation.direction, animation.percentageStart), this.objectToAnimate.transform.localPosition);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = this.objectToAnimate.transform.DOLocalMove(animation.endPosition, animation.speed, false).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.SlideOutLocal, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.localPosition));
		yield break;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00003F49 File Offset: 0x00002149
	private IEnumerator DelayThenPunchScale(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.PunchScale))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.PunchScale]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.PunchScale);
		}
		float _delay = animation.delay;
		if (now)
		{
			_delay = 0f;
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * _delay);
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.PunchScale))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.PunchScale]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.PunchScale);
		}
		Tweener tweener = this.objectToAnimate.transform.DOPunchScale(new Vector3(animation.amount, animation.amount, 0f), animation.speed, animation.vibrato, 0.3f);
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.PunchScale, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.localScale));
		yield break;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00003F66 File Offset: 0x00002166
	private IEnumerator DelayThenShakeScale(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.ShakeScale))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.ShakeScale]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.ShakeScale);
		}
		float _delay = animation.delay;
		if (now)
		{
			_delay = 0f;
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * _delay);
		Tweener tweener = this.objectToAnimate.transform.DOShakeScale(animation.speed, animation.amount, animation.vibrato, 90f, true, ShakeRandomnessMode.Full);
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.ShakeScale, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.localScale));
		yield break;
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00003F83 File Offset: 0x00002183
	private IEnumerator DelayThenScale(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.Scale))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.Scale]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.Scale);
		}
		Vector3 _originScale = this.objectToAnimate.transform.localScale;
		this.objectToAnimate.transform.localScale = this.GetScale(_originScale, animation.percentageStart);
		float _delay = animation.delay;
		if (now)
		{
			_delay = 0f;
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * _delay);
		Tweener tweener = this.objectToAnimate.transform.DOScale(this.GetScale(_originScale, animation.percentageEnd), animation.speed);
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.Scale, new DoCodeAnimation.ActiveAnimationInfo(tweener, this.objectToAnimate.transform.localScale));
		yield break;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00003FA0 File Offset: 0x000021A0
	private IEnumerator DelayThenFadeIn(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.FadeIn))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.FadeIn]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.FadeIn);
		}
		Image img = this.objectToAnimate.GetComponent<Image>();
		img.DOFade(0f, 0f);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = img.DOFade(1f, animation.speed).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.FadeIn, new DoCodeAnimation.ActiveAnimationInfo(tweener, Vector3.zero));
		yield break;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00003FB6 File Offset: 0x000021B6
	private IEnumerator DelayThenFadeOut(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.FadeOut))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.FadeOut]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.FadeOut);
		}
		Image img = this.objectToAnimate.GetComponent<Image>();
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = img.DOFade(0f, animation.speed).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.FadeOut, new DoCodeAnimation.ActiveAnimationInfo(tweener, new Vector3(img.color.a, 0f, 0f)));
		yield break;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00003FCC File Offset: 0x000021CC
	private IEnumerator DelayThenFadeInCanvasGroup(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.FadeInCanvasGroup))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.FadeInCanvasGroup]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.FadeInCanvasGroup);
		}
		CanvasGroup canvasGrp = this.objectToAnimate.GetComponent<CanvasGroup>();
		canvasGrp.DOFade(0f, 0f);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = canvasGrp.DOFade(1f, animation.speed).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.FadeInCanvasGroup, new DoCodeAnimation.ActiveAnimationInfo(tweener, Vector3.zero));
		yield break;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00003FE2 File Offset: 0x000021E2
	private IEnumerator DelayThenFadeOutCanvasGroup(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.FadeOutCanvasGroup))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.FadeOutCanvasGroup]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.FadeOutCanvasGroup);
		}
		CanvasGroup canvasGrp = this.objectToAnimate.GetComponent<CanvasGroup>();
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * animation.delay);
		Tweener tweener = canvasGrp.DOFade(0f, animation.speed).OnComplete(delegate
		{
			this.FinishedAnimationInstance(animation);
		});
		this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.FadeOutCanvasGroup, new DoCodeAnimation.ActiveAnimationInfo(tweener, new Vector3(canvasGrp.alpha, 0f, 0f)));
		yield break;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00003FF8 File Offset: 0x000021F8
	private IEnumerator DelayThenFillImage(DoCodeAnimation.AnimationVitals animation, bool now = false)
	{
		if (this.ActiveAnimations.ContainsKey(DoCodeAnimation.AnimationType.ImageFillGrow))
		{
			this.ActiveAnimations[DoCodeAnimation.AnimationType.ImageFillGrow]._tweener.Complete(true);
			this.ActiveAnimations.Remove(DoCodeAnimation.AnimationType.ImageFillGrow);
		}
		float _delay = animation.delay;
		if (now)
		{
			_delay = 0f;
		}
		Image _img = this.objectToAnimate.GetComponent<Image>();
		float _fill = _img.fillAmount;
		_img.fillAmount = 0f;
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(this.delayMultiplier * _delay);
		Tweener tweener = _img.DOFillAmount(_fill, animation.speed).SetEase(Ease.InOutCubic);
		if (!this.ActiveAnimations.Keys.Contains(DoCodeAnimation.AnimationType.ImageFillGrow))
		{
			this.ActiveAnimations.Add(DoCodeAnimation.AnimationType.ImageFillGrow, new DoCodeAnimation.ActiveAnimationInfo(tweener, Vector3.zero));
		}
		yield break;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00004018 File Offset: 0x00002218
	private void FinishedAnimationInstance(DoCodeAnimation.AnimationVitals animation)
	{
		animation.InProgress = false;
		if (animation.active)
		{
			animation.active = false;
		}
		int i = 0;
		int count = this.animations.Count;
		while (i < count)
		{
			if (animation.id == this.animations[i].id)
			{
				this.animations[i] = animation;
				break;
			}
			i++;
		}
		if (this.destroyOnFinish)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00004090 File Offset: 0x00002290
	public void ResetAnim(bool withCallbacks = false)
	{
		this.objectToAnimate.transform.DORewind(false);
		this.objectToAnimate.transform.DORestart(false);
		foreach (KeyValuePair<DoCodeAnimation.AnimationType, DoCodeAnimation.ActiveAnimationInfo> keyValuePair in this.ActiveAnimations)
		{
			DoCodeAnimation.ActiveAnimationInfo value = keyValuePair.Value;
			value._tweener.Complete(withCallbacks);
			value._tweener.Goto(0f, false);
			value._tweener.Rewind(false);
			switch (keyValuePair.Key)
			{
			case DoCodeAnimation.AnimationType.SlideInWaitForFrame:
			case DoCodeAnimation.AnimationType.SlideOut:
			case DoCodeAnimation.AnimationType.SlideIn:
				this.objectToAnimate.transform.position = value._startValue;
				continue;
			case DoCodeAnimation.AnimationType.PunchScale:
			case DoCodeAnimation.AnimationType.Scale:
			case DoCodeAnimation.AnimationType.ShakeScale:
				this.objectToAnimate.transform.localScale = value._startValue;
				continue;
			case DoCodeAnimation.AnimationType.SlideInLocal:
			case DoCodeAnimation.AnimationType.SlideInWaitForFrameLocal:
			case DoCodeAnimation.AnimationType.SlideOutLocal:
				this.objectToAnimate.transform.localPosition = value._startValue;
				continue;
			}
			T17Debug.LogError("[DoCodeAnimation] Reset anim called for an anim type that hasn't been implemented. Please implement reset for 'AnimationType." + keyValuePair.Key.ToString() + "'");
		}
	}

	// Token: 0x0400008C RID: 140
	[SerializeField]
	private GameObject objectToAnimate;

	// Token: 0x0400008D RID: 141
	public float delayMultiplier = 1f;

	// Token: 0x0400008E RID: 142
	[SerializeField]
	private List<DoCodeAnimation.AnimationVitals> animations;

	// Token: 0x0400008F RID: 143
	public bool destroyOnFinish;

	// Token: 0x04000090 RID: 144
	private Dictionary<DoCodeAnimation.AnimationType, DoCodeAnimation.ActiveAnimationInfo> ActiveAnimations = new Dictionary<DoCodeAnimation.AnimationType, DoCodeAnimation.ActiveAnimationInfo>();

	// Token: 0x04000091 RID: 145
	[SerializeField]
	private bool stopAllCoroutinesOnEnable;

	// Token: 0x0200027B RID: 635
	public enum AnimationType
	{
		// Token: 0x04000FB6 RID: 4022
		SlideInWaitForFrame,
		// Token: 0x04000FB7 RID: 4023
		SlideOut,
		// Token: 0x04000FB8 RID: 4024
		PunchScale,
		// Token: 0x04000FB9 RID: 4025
		Scale,
		// Token: 0x04000FBA RID: 4026
		ShakeScale,
		// Token: 0x04000FBB RID: 4027
		FadeIn,
		// Token: 0x04000FBC RID: 4028
		FadeInCanvasGroup,
		// Token: 0x04000FBD RID: 4029
		SlideIn,
		// Token: 0x04000FBE RID: 4030
		FadeOut,
		// Token: 0x04000FBF RID: 4031
		FadeOutCanvasGroup,
		// Token: 0x04000FC0 RID: 4032
		ImageFillGrow,
		// Token: 0x04000FC1 RID: 4033
		SlideInLocal,
		// Token: 0x04000FC2 RID: 4034
		SlideInWaitForFrameLocal,
		// Token: 0x04000FC3 RID: 4035
		SlideOutLocal
	}

	// Token: 0x0200027C RID: 636
	[Serializable]
	public struct AnimationVitals
	{
		// Token: 0x06001449 RID: 5193 RVA: 0x00061B14 File Offset: 0x0005FD14
		public AnimationVitals(DoCodeAnimation.AnimationType _type = DoCodeAnimation.AnimationType.SlideIn, float _speed = 0.5f, float _delay = 0.01f, float _percentageStart = 100f, float _percentageEnd = 100f, float _amount = 1f, int _vibrato = 1, bool _triggerOnStart = false, bool _triggerOnEnable = false, bool _removeAfterActivated = false)
		{
			this.type = _type;
			this.direction = CompassDirections.east;
			this.speed = _speed;
			this.delay = _delay;
			this.percentageStart = _percentageStart;
			this.percentageEnd = _percentageEnd;
			this.amount = _amount;
			this.vibrato = _vibrato;
			this.triggerOnStart = _triggerOnStart;
			this.triggerOnEnable = _triggerOnEnable;
			this.startPosition = Vector3.zero;
			this.endPosition = Vector3.zero;
			this.active = true;
			this.removeAfterActivated = _removeAfterActivated;
			this.InProgress = false;
			this.id = -1;
		}

		// Token: 0x04000FC4 RID: 4036
		public DoCodeAnimation.AnimationType type;

		// Token: 0x04000FC5 RID: 4037
		public CompassDirections direction;

		// Token: 0x04000FC6 RID: 4038
		public float speed;

		// Token: 0x04000FC7 RID: 4039
		public float delay;

		// Token: 0x04000FC8 RID: 4040
		public float percentageStart;

		// Token: 0x04000FC9 RID: 4041
		public float percentageEnd;

		// Token: 0x04000FCA RID: 4042
		public float amount;

		// Token: 0x04000FCB RID: 4043
		public int vibrato;

		// Token: 0x04000FCC RID: 4044
		public bool triggerOnStart;

		// Token: 0x04000FCD RID: 4045
		public bool triggerOnEnable;

		// Token: 0x04000FCE RID: 4046
		[HideInInspector]
		public Vector3 startPosition;

		// Token: 0x04000FCF RID: 4047
		[HideInInspector]
		public Vector3 endPosition;

		// Token: 0x04000FD0 RID: 4048
		public bool active;

		// Token: 0x04000FD1 RID: 4049
		public bool removeAfterActivated;

		// Token: 0x04000FD2 RID: 4050
		[HideInInspector]
		public bool InProgress;

		// Token: 0x04000FD3 RID: 4051
		[HideInInspector]
		public int id;
	}

	// Token: 0x0200027D RID: 637
	public struct ActiveAnimationInfo
	{
		// Token: 0x0600144A RID: 5194 RVA: 0x00061BA0 File Offset: 0x0005FDA0
		public ActiveAnimationInfo(Tweener tweener, Vector3 startValue)
		{
			this._tweener = tweener;
			this._startValue = startValue;
		}

		// Token: 0x04000FD4 RID: 4052
		public Tweener _tweener;

		// Token: 0x04000FD5 RID: 4053
		public Vector3 _startValue;
	}
}
