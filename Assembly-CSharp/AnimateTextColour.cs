using System;
using TMPro;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class AnimateTextColour : MonoBehaviour
{
	// Token: 0x06000E01 RID: 3585 RVA: 0x0004DFE2 File Offset: 0x0004C1E2
	private void Awake()
	{
		if (this.textInstance == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x0004DFFC File Offset: 0x0004C1FC
	private void Update()
	{
		float num = Time.deltaTime * this.timeScale;
		if (num > 0f)
		{
			this.animTime = (this.animTime + num) % 1f;
			this.textInstance.color = Color.Lerp(this.colour1, this.colour2, this.curve.Evaluate(this.animTime));
		}
	}

	// Token: 0x04000C72 RID: 3186
	public AnimationCurve curve = new AnimationCurve();

	// Token: 0x04000C73 RID: 3187
	public TMP_Text textInstance;

	// Token: 0x04000C74 RID: 3188
	public Color colour1 = Color.white;

	// Token: 0x04000C75 RID: 3189
	public Color colour2 = Color.red;

	// Token: 0x04000C76 RID: 3190
	public float timeScale = 1f;

	// Token: 0x04000C77 RID: 3191
	private float animTime;
}
