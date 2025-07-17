using System;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x0200001B RID: 27
public class EmoteStopper : MonoBehaviour
{
	// Token: 0x06000094 RID: 148 RVA: 0x0000421E File Offset: 0x0000241E
	private void Awake()
	{
		if (!this.visualEffect)
		{
			this.visualEffect = base.GetComponent<VisualEffect>();
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00004239 File Offset: 0x00002439
	private void OnDisable()
	{
		this.visualEffect.Stop();
		this.hasPlayed = false;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0000424D File Offset: 0x0000244D
	private void Update()
	{
		if (this.visualEffect.aliveParticleCount == 0 && this.hasPlayed)
		{
			this.DisableObject();
			this.hasPlayed = false;
			return;
		}
		if (this.visualEffect.aliveParticleCount > 0)
		{
			this.hasPlayed = true;
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00004287 File Offset: 0x00002487
	public void DisableObject()
	{
		this.visualEffect.Stop();
		base.gameObject.SetActive(false);
		this.hasPlayed = false;
	}

	// Token: 0x04000092 RID: 146
	[SerializeField]
	private VisualEffect visualEffect;

	// Token: 0x04000093 RID: 147
	private bool hasPlayed;
}
