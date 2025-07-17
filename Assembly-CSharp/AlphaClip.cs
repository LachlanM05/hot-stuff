using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class AlphaClip : MonoBehaviour
{
	// Token: 0x06000043 RID: 67 RVA: 0x0000333C File Offset: 0x0000153C
	private void Start()
	{
		this.target_renderer.materials[this.m_index].SetFloat("_AlphaClip", 1f);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0000335F File Offset: 0x0000155F
	private void OnValidate()
	{
		this.target_renderer = base.GetComponent<Renderer>();
	}

	// Token: 0x04000067 RID: 103
	[SerializeField]
	private Renderer target_renderer;

	// Token: 0x04000068 RID: 104
	[SerializeField]
	private int m_index;
}
