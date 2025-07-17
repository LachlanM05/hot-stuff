using System;
using UnityEngine;

// Token: 0x02000159 RID: 345
public static class VectorExtensions
{
	// Token: 0x06000CCC RID: 3276 RVA: 0x0004A4EE File Offset: 0x000486EE
	public static float SignedAngleTo(this Vector3 a, Vector3 b, Vector3 up)
	{
		return Mathf.Atan2(Vector3.Dot(up.normalized, Vector3.Cross(a, b)), Vector3.Dot(a, b)) * 57.29578f;
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x0004A515 File Offset: 0x00048715
	public static float SignedAngle(this Vector2 a)
	{
		return Mathf.Atan2(a.y, a.x) * 57.29578f;
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0004A530 File Offset: 0x00048730
	public static float SignedAngleTo(this Vector2 a, Vector2 b)
	{
		return Mathf.Atan2(a.x * b.y - a.y * b.x, a.x * b.x + a.y * b.y) * 57.29578f;
	}
}
