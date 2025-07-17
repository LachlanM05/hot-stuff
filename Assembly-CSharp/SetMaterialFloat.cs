using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
[ExecuteInEditMode]
public class SetMaterialFloat : MonoBehaviour
{
	// Token: 0x06000099 RID: 153 RVA: 0x000042B0 File Offset: 0x000024B0
	private void Update()
	{
		float num = this.value * this.sliderMax;
		if (this.mat.HasProperty(this.property) && !Mathf.Approximately(num, this.mat.GetFloat(this.property)))
		{
			this.mat.SetFloat(this.property, num);
		}
	}

	// Token: 0x04000094 RID: 148
	[SerializeField]
	private string property = "";

	// Token: 0x04000095 RID: 149
	[SerializeField]
	private Material mat;

	// Token: 0x04000096 RID: 150
	[Range(0f, 1f)]
	public float value;

	// Token: 0x04000097 RID: 151
	public float sliderMax = 1f;
}
