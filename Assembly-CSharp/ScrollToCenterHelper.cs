using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000145 RID: 325
public static class ScrollToCenterHelper
{
	// Token: 0x06000BE9 RID: 3049 RVA: 0x00044E70 File Offset: 0x00043070
	public static void ScrollToCenter(this ScrollRect scrollRect, RectTransform target, MonoBehaviour monoBehaviour)
	{
		RectTransform rectTransform = ((scrollRect.viewport != null) ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>());
		Rect rect = rectTransform.rect;
		Bounds bounds = target.TransformBoundsTo(rectTransform);
		float num = rect.center.y - bounds.center.y;
		float num2 = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(1, num);
		if (ScrollToCenterHelper._coroutine != null)
		{
			monoBehaviour.StopCoroutine(ScrollToCenterHelper._coroutine);
		}
		ScrollToCenterHelper._coroutine = monoBehaviour.StartCoroutine(ScrollToCenterHelper.VerticalNormalizedPositionSmooth(scrollRect, Mathf.Clamp(num2, 0f, 1f)));
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00044F08 File Offset: 0x00043108
	private static Bounds TransformBoundsTo(this RectTransform source, Transform target)
	{
		Bounds bounds = default(Bounds);
		if (source != null)
		{
			source.GetWorldCorners(ScrollToCenterHelper._corners);
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			Matrix4x4 worldToLocalMatrix = target.worldToLocalMatrix;
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector3 = worldToLocalMatrix.MultiplyPoint3x4(ScrollToCenterHelper._corners[i]);
				vector = Vector3.Min(vector3, vector);
				vector2 = Vector3.Max(vector3, vector2);
			}
			bounds = new Bounds(vector, Vector3.zero);
			bounds.Encapsulate(vector2);
		}
		return bounds;
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00044FB0 File Offset: 0x000431B0
	private static float NormalizeScrollDistance(this ScrollRect scrollRect, int axis, float distance)
	{
		RectTransform viewport = scrollRect.viewport;
		RectTransform rectTransform = ((viewport != null) ? viewport : scrollRect.GetComponent<RectTransform>());
		Bounds bounds = new Bounds(rectTransform.rect.center, rectTransform.rect.size);
		RectTransform content = scrollRect.content;
		float num = ((content != null) ? content.TransformBoundsTo(rectTransform) : default(Bounds)).size[axis] - bounds.size[axis];
		return distance / num;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00045054 File Offset: 0x00043254
	private static IEnumerator VerticalNormalizedPositionSmooth(ScrollRect scrollRect, float position)
	{
		int maxTime = DateTime.Now.AddSeconds(2.0).Second;
		float num;
		float num2;
		do
		{
			scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, position, 0.25f);
			yield return ScrollToCenterHelper._waitForEndOfFrame;
			num = Mathf.Round(scrollRect.verticalNormalizedPosition * 1000f) * 0.001f;
			num2 = Mathf.Round(position * 1000f) * 0.001f;
		}
		while (num != num2 && maxTime > DateTime.Now.Second);
		scrollRect.verticalNormalizedPosition = position;
		yield break;
	}

	// Token: 0x04000AA2 RID: 2722
	private const int MaxCornersCount = 4;

	// Token: 0x04000AA3 RID: 2723
	private const float ScrollTimeStep = 0.25f;

	// Token: 0x04000AA4 RID: 2724
	private const int MaxScrollTimeSec = 2;

	// Token: 0x04000AA5 RID: 2725
	private static readonly Vector3[] _corners = new Vector3[4];

	// Token: 0x04000AA6 RID: 2726
	private static readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

	// Token: 0x04000AA7 RID: 2727
	private static Coroutine _coroutine;
}
