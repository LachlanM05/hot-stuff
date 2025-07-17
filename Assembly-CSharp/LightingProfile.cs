using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
[CreateAssetMenu(menuName = "ScriptableObjects/LightingProfile")]
[Serializable]
public class LightingProfile : ScriptableObject
{
	// Token: 0x06000028 RID: 40 RVA: 0x000029EC File Offset: 0x00000BEC
	public void ApplyLighting(GameObject SunLight)
	{
		RenderSettings.skybox = this.skyboxMaterial;
		RenderSettings.ambientSkyColor = this.skyColor;
		RenderSettings.ambientEquatorColor = this.equatorColor;
		RenderSettings.ambientGroundColor = this.groundColor;
		RenderSettings.fogColor = this.fogColor;
		RenderSettings.fogStartDistance = this.fogStart;
		RenderSettings.fogEndDistance = this.fogEnd;
		if (SunLight != null)
		{
			if (this.sunActive)
			{
				SunLight.SetActive(true);
				Quaternion rotation = SunLight.transform.rotation;
				rotation.y = this.sunRotationY;
				SunLight.transform.rotation = rotation;
				return;
			}
			SunLight.SetActive(false);
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002A8C File Offset: 0x00000C8C
	public void GetLighting()
	{
		this.skyboxMaterial = RenderSettings.skybox;
		this.skyColor = RenderSettings.ambientSkyColor;
		this.equatorColor = RenderSettings.ambientEquatorColor;
		this.groundColor = RenderSettings.ambientGroundColor;
		this.fogColor = RenderSettings.fogColor;
		this.fogStart = RenderSettings.fogStartDistance;
		this.fogEnd = RenderSettings.fogEndDistance;
	}

	// Token: 0x0400001F RID: 31
	public Material skyboxMaterial;

	// Token: 0x04000020 RID: 32
	[ColorUsage(false, true)]
	public Color skyColor;

	// Token: 0x04000021 RID: 33
	[ColorUsage(false, true)]
	public Color equatorColor;

	// Token: 0x04000022 RID: 34
	[ColorUsage(false, true)]
	public Color groundColor;

	// Token: 0x04000023 RID: 35
	public Color fogColor;

	// Token: 0x04000024 RID: 36
	public float fogStart;

	// Token: 0x04000025 RID: 37
	public float fogEnd;

	// Token: 0x04000026 RID: 38
	public float sunRotationY;

	// Token: 0x04000027 RID: 39
	public bool sunActive;
}
