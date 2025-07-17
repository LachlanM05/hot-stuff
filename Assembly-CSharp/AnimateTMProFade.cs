using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class AnimateTMProFade : AnimateTMProVertex
{
	// Token: 0x0600005B RID: 91 RVA: 0x00003467 File Offset: 0x00001667
	public override Color32 LetterColorFunction(Color32 originalColor, float p, int index, int numCharacters)
	{
		return new Color32(originalColor.r, originalColor.g, originalColor.b, (byte)(this.alphaCurve.Evaluate(p) * 255f));
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00003493 File Offset: 0x00001693
	public override void OnAnimationComplete()
	{
	}

	// Token: 0x04000073 RID: 115
	[Header("Opacity")]
	public AnimationCurve alphaCurve;
}
