using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class DialogTextAnimator : AnimateTMProVertex
{
	// Token: 0x0600093D RID: 2365 RVA: 0x00035E80 File Offset: 0x00034080
	public override Vector3 LetterScaleFunction(float p, int index, int numCharacters)
	{
		return Vector3.LerpUnclamped(this.startScale, Vector3.one, this.scaleCurve.Evaluate(p));
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00035E9E File Offset: 0x0003409E
	public override Color32 LetterColorFunction(Color32 originalColor, float p, int index, int numCharacters)
	{
		return new Color32(originalColor.r, originalColor.g, originalColor.b, (byte)(this.alphaCurve.Evaluate(p) * 255f));
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00035ECA File Offset: 0x000340CA
	public override void OnAnimationComplete()
	{
		Singleton<AudioManager>.Instance.StopTrack("UI_Text_Writing", 0.1f);
	}

	// Token: 0x04000882 RID: 2178
	public Vector3 startScale;

	// Token: 0x04000883 RID: 2179
	public AnimationCurve scaleCurve;

	// Token: 0x04000884 RID: 2180
	public AnimationCurve alphaCurve;
}
