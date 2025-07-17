using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B2 RID: 434
public static class ScrollViewFocusFunctions
{
	// Token: 0x06000EB8 RID: 3768 RVA: 0x00050834 File Offset: 0x0004EA34
	public static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, Vector2 focusPoint)
	{
		Vector2 size = scrollView.content.rect.size;
		Vector2 size2 = ((RectTransform)scrollView.content.parent).rect.size;
		Vector2 vector = scrollView.content.localScale;
		size.Scale(vector);
		focusPoint.Scale(vector);
		Vector2 normalizedPosition = scrollView.normalizedPosition;
		if (scrollView.horizontal && size.x > size2.x)
		{
			normalizedPosition.x = Mathf.Clamp01((focusPoint.x - size2.x * 0.5f) / (size.x - size2.x));
		}
		if (scrollView.vertical && size.y > size2.y)
		{
			normalizedPosition.y = Mathf.Clamp01((focusPoint.y - size2.y * 0.5f) / (size.y - size2.y));
		}
		return normalizedPosition;
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x00050924 File Offset: 0x0004EB24
	public static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, RectTransform item)
	{
		Vector2 vector = scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));
		Vector2 size = scrollView.content.rect.size;
		size.Scale(scrollView.content.pivot);
		return scrollView.CalculateFocusedScrollPosition(vector + size);
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x00050993 File Offset: 0x0004EB93
	public static void FocusAtPoint(this ScrollRect scrollView, Vector2 focusPoint)
	{
		scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(focusPoint);
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x000509A2 File Offset: 0x0004EBA2
	public static void FocusOnItem(this ScrollRect scrollView, RectTransform item)
	{
		scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(item);
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x000509B1 File Offset: 0x0004EBB1
	private static IEnumerator LerpToScrollPositionCoroutine(this ScrollRect scrollView, Vector2 targetNormalizedPos, float speed)
	{
		Vector2 initialNormalizedPos = scrollView.normalizedPosition;
		for (float t = 0f; t < 1f; t += speed * Time.unscaledDeltaTime)
		{
			scrollView.normalizedPosition = Vector2.LerpUnclamped(initialNormalizedPos, targetNormalizedPos, 1f - (1f - t) * (1f - t));
			yield return null;
		}
		scrollView.normalizedPosition = targetNormalizedPos;
		yield break;
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x000509CE File Offset: 0x0004EBCE
	public static IEnumerator FocusAtPointCoroutine(this ScrollRect scrollView, Vector2 focusPoint, float speed)
	{
		yield return scrollView.LerpToScrollPositionCoroutine(scrollView.CalculateFocusedScrollPosition(focusPoint), speed);
		yield break;
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x000509EB File Offset: 0x0004EBEB
	public static IEnumerator FocusOnItemCoroutine(this ScrollRect scrollView, RectTransform item, float speed)
	{
		yield return scrollView.LerpToScrollPositionCoroutine(scrollView.CalculateFocusedScrollPosition(item), speed);
		yield break;
	}
}
