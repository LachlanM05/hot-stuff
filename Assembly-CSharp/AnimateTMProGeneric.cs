using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class AnimateTMProGeneric : AnimateTMProVertex
{
	// Token: 0x0600005E RID: 94 RVA: 0x0000349D File Offset: 0x0000169D
	public override Vector3 LetterTranslateFunction(float p, int index, int numCharacters)
	{
		return Vector3.LerpUnclamped(this.startOffset, this.endOffset, this.offsetCurve.Evaluate(p));
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000034BC File Offset: 0x000016BC
	public override Color32 LetterColorFunction(Color32 originalColor, float p, int index, int numCharacters)
	{
		return new Color32(originalColor.r, originalColor.g, originalColor.b, (byte)(this.alphaCurve.Evaluate(p) * 255f));
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000034E8 File Offset: 0x000016E8
	public override Vector3 LetterScaleFunction(float p, int index, int numCharacters)
	{
		return Vector3.LerpUnclamped(this.startScale, this.endScale, this.scaleCurve.Evaluate(p));
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00003507 File Offset: 0x00001707
	public override void OnAnimationComplete()
	{
	}

	// Token: 0x04000074 RID: 116
	[Header("Scale")]
	public Vector3 startScale;

	// Token: 0x04000075 RID: 117
	public Vector3 endScale;

	// Token: 0x04000076 RID: 118
	public AnimationCurve scaleCurve;

	// Token: 0x04000077 RID: 119
	[Header("Translate")]
	public Vector3 startOffset;

	// Token: 0x04000078 RID: 120
	public Vector3 endOffset;

	// Token: 0x04000079 RID: 121
	public AnimationCurve offsetCurve;

	// Token: 0x0400007A RID: 122
	[Header("Opacity")]
	public AnimationCurve alphaCurve;
}
