using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class LightFlickerEffect : MonoBehaviour
{
	// Token: 0x06000CDD RID: 3293 RVA: 0x0004A856 File Offset: 0x00048A56
	public void Reset()
	{
		this.smoothQueue.Clear();
		this.lastSum = 0f;
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0004A86E File Offset: 0x00048A6E
	private void Start()
	{
		this.smoothQueue = new Queue<float>(this.smoothing);
		if (this.light == null)
		{
			this.light = base.GetComponent<Light>();
		}
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0004A89C File Offset: 0x00048A9C
	private void Update()
	{
		if (this.light == null)
		{
			return;
		}
		while (this.smoothQueue.Count >= this.smoothing)
		{
			this.lastSum -= this.smoothQueue.Dequeue();
		}
		float num = Random.Range(this.minIntensity, this.maxIntensity);
		this.smoothQueue.Enqueue(num);
		this.lastSum += num;
		this.light.intensity = this.lastSum / (float)this.smoothQueue.Count;
	}

	// Token: 0x04000B80 RID: 2944
	[Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
	public Light light;

	// Token: 0x04000B81 RID: 2945
	[Tooltip("Minimum random light intensity")]
	public float minIntensity;

	// Token: 0x04000B82 RID: 2946
	[Tooltip("Maximum random light intensity")]
	public float maxIntensity = 1f;

	// Token: 0x04000B83 RID: 2947
	[Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
	[Range(1f, 50f)]
	public int smoothing = 5;

	// Token: 0x04000B84 RID: 2948
	private Queue<float> smoothQueue;

	// Token: 0x04000B85 RID: 2949
	private float lastSum;
}
