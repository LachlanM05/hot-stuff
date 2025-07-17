using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B5 RID: 437
public class SimpleUIParticle : MonoBehaviour
{
	// Token: 0x06000ED2 RID: 3794 RVA: 0x00050D6C File Offset: 0x0004EF6C
	private void Awake()
	{
		if (this.image == null && (this.image = base.GetComponent<Image>()) == null)
		{
			base.enabled = false;
			return;
		}
		this.ValidateMinMax(ref this.minSpawnDelay, ref this.maxSpawnDelay, 0f);
		this.ValidateMinMax(ref this.minLife, ref this.maxLife, 0.1f);
		this.ValidateMinMax(ref this.minScale, ref this.maxScale, 0f);
		this.ValidateMinMax(ref this.minRotation, ref this.maxRotation, 0f);
		this.colour = this.image.color;
		this.imageTransform = this.image.rectTransform;
		this.Reset();
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00050E2A File Offset: 0x0004F02A
	private void OnEnable()
	{
		if (this.image == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x00050E44 File Offset: 0x0004F044
	private void Update()
	{
		this.delaTime = Time.unscaledDeltaTime;
		if (this.delayToSpawn > 0f)
		{
			this.delayToSpawn -= this.delaTime;
			return;
		}
		if (this.time > this.lifeTime * 2f)
		{
			this.Reset();
			return;
		}
		this.time += this.delaTime;
		this.UpdateParticle();
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x00050EB4 File Offset: 0x0004F0B4
	private void Reset()
	{
		this.image.enabled = false;
		this.alphaValue = 0f;
		this.scaleValue = this.GetRandomValue(this.minScale, this.maxScale);
		this.delayToSpawn = this.GetRandomValue(this.minSpawnDelay, this.maxSpawnDelay);
		this.lifeTime = this.GetRandomValue(this.minLife, this.maxLife) / 2f;
		this.rotationSpeed = this.GetRandomValue(this.minRotation, this.maxRotation);
		if (this.bothDirections && this.GetRandomValue(0f, 1f) >= 0.5f)
		{
			this.rotationSpeed *= -1f;
		}
		this.time = 0f;
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x00050F7C File Offset: 0x0004F17C
	private void UpdateParticle()
	{
		if (!this.image.enabled)
		{
			this.image.enabled = true;
		}
		float num = Mathf.Clamp(this.time / this.lifeTime, 0f, 2f);
		if (num > 1f)
		{
			num = 2f - num;
		}
		this.alphaValue = Mathf.Min(num * 3f, 1f);
		Color color = this.colour;
		color.a *= this.alphaValue;
		this.image.color = color;
		this.scale.x = this.scaleValue * num;
		this.scale.y = this.scaleValue * num;
		this.imageTransform.localScale = this.scale;
		if (this.rotationSpeed != 0f)
		{
			Vector3 eulerAngles = this.imageTransform.eulerAngles;
			eulerAngles.z += this.rotationSpeed * this.delaTime;
			this.imageTransform.eulerAngles = eulerAngles;
		}
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x00051084 File Offset: 0x0004F284
	private void ValidateMinMax(ref float min, ref float max, float minValue = 0f)
	{
		if (max < minValue)
		{
			max = minValue;
		}
		if (min < minValue)
		{
			min = minValue;
		}
		if (min > max)
		{
			min = max;
		}
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x000510A0 File Offset: 0x0004F2A0
	private float GetRandomValue(float min, float max)
	{
		if (min == max)
		{
			return min;
		}
		return Random.Range(min, max);
	}

	// Token: 0x04000D13 RID: 3347
	public Image image;

	// Token: 0x04000D14 RID: 3348
	[Header("Spawn delay")]
	public float minSpawnDelay = 5f;

	// Token: 0x04000D15 RID: 3349
	public float maxSpawnDelay = 60f;

	// Token: 0x04000D16 RID: 3350
	[Header("Particle lifetime")]
	public float minLife = 0.25f;

	// Token: 0x04000D17 RID: 3351
	public float maxLife = 3f;

	// Token: 0x04000D18 RID: 3352
	[Header("Scale")]
	public float minScale = 0.5f;

	// Token: 0x04000D19 RID: 3353
	public float maxScale = 3f;

	// Token: 0x04000D1A RID: 3354
	[Header("Rotation deg per second")]
	public float minRotation = 10f;

	// Token: 0x04000D1B RID: 3355
	public float maxRotation = 100f;

	// Token: 0x04000D1C RID: 3356
	public bool bothDirections = true;

	// Token: 0x04000D1D RID: 3357
	private float alphaValue;

	// Token: 0x04000D1E RID: 3358
	private float scaleValue;

	// Token: 0x04000D1F RID: 3359
	private float delayToSpawn;

	// Token: 0x04000D20 RID: 3360
	private float lifeTime;

	// Token: 0x04000D21 RID: 3361
	private float rotationSpeed;

	// Token: 0x04000D22 RID: 3362
	private float time;

	// Token: 0x04000D23 RID: 3363
	private Color colour = Color.white;

	// Token: 0x04000D24 RID: 3364
	private Vector3 scale = Vector3.one;

	// Token: 0x04000D25 RID: 3365
	private RectTransform imageTransform;

	// Token: 0x04000D26 RID: 3366
	private float delaTime;
}
