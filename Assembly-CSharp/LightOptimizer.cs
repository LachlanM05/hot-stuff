using System;
using System.Collections.Generic;
using T17.Services;
using Team17.Scripts.Platforms.Enums;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class LightOptimizer : MonoBehaviour
{
	// Token: 0x06000D58 RID: 3416 RVA: 0x0004C504 File Offset: 0x0004A704
	private void Start()
	{
		if (Services.PlatformService.HasFlag(EPlatformFlags.ApplyGraphicalOptimisations))
		{
			Light[] array = Object.FindObjectsOfType<Light>(true);
			this.NearDistanceScaleSqr = this.NearDistanceScale * this.NearDistanceScale;
			this.FarDistanceScaleSqr = this.FarDistanceScale * this.FarDistanceScale;
			this.myMainCamera = Camera.main;
			this.myMainCamera.allowDynamicResolution = true;
			int num = 0;
			for (int i = array.Length - 1; i >= 0; i--)
			{
				Light light = array[i];
				if (light.type == LightType.Spot)
				{
					this.mySpotLights.Add(new LightOptimizer.LightWrapper(light));
				}
				else if (light.type == LightType.Point)
				{
					this.myPointLights.Add(new LightOptimizer.LightWrapper(light));
				}
				if (light.shadows == LightShadows.Soft)
				{
					light.shadows = LightShadows.Hard;
					num++;
				}
			}
			return;
		}
		base.enabled = false;
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x0004C5CC File Offset: 0x0004A7CC
	private void LateUpdate()
	{
		if (this.myMainCamera != null || (this.myMainCamera = Camera.main) != null)
		{
			if (!this.myMainCamera.allowDynamicResolution)
			{
				this.myMainCamera.allowDynamicResolution = true;
			}
			Vector3 position = this.myMainCamera.transform.position;
			for (int i = this.mySpotLights.Count - 1; i >= 0; i--)
			{
				LightOptimizer.LightWrapper lightWrapper = this.mySpotLights[i];
				float num = Math.Max(Math.Min(((position - lightWrapper.myPosition).sqrMagnitude - this.NearDistanceScaleSqr) / (this.FarDistanceScaleSqr - this.NearDistanceScaleSqr), 1f), 0f);
				if (num != lightWrapper.previousScale)
				{
					lightWrapper.previousScale = num;
					lightWrapper.myLight.range = Mathf.Lerp(lightWrapper.myOriginalRange * this.RangeScale, lightWrapper.myOriginalRange * this.maxRangeScale, num);
					lightWrapper.myLight.spotAngle = Mathf.Lerp(lightWrapper.myOriginalSpotSizeScale * this.SpotSizeScale, lightWrapper.myOriginalSpotSizeScale * this.maxSpotSizeScale, num);
				}
			}
			for (int j = this.myPointLights.Count - 1; j >= 0; j--)
			{
				LightOptimizer.LightWrapper lightWrapper2 = this.myPointLights[j];
				float num2 = Math.Max(Math.Min(((position - lightWrapper2.myPosition).sqrMagnitude - this.NearDistanceScaleSqr) / (this.FarDistanceScaleSqr - this.NearDistanceScaleSqr), 1f), 0f);
				if (num2 != lightWrapper2.previousScale)
				{
					lightWrapper2.previousScale = num2;
					lightWrapper2.myLight.range = Mathf.Lerp(lightWrapper2.myOriginalRange * this.RangeScale, lightWrapper2.myOriginalRange * this.maxRangeScale, num2);
				}
			}
		}
	}

	// Token: 0x04000C22 RID: 3106
	private float RangeScale = 1f;

	// Token: 0x04000C23 RID: 3107
	private float SpotSizeScale = 1f;

	// Token: 0x04000C24 RID: 3108
	private float maxRangeScale = 0.5f;

	// Token: 0x04000C25 RID: 3109
	private float maxSpotSizeScale = 0.5f;

	// Token: 0x04000C26 RID: 3110
	private float NearDistanceScale = 15f;

	// Token: 0x04000C27 RID: 3111
	private float FarDistanceScale = 50f;

	// Token: 0x04000C28 RID: 3112
	private float NearDistanceScaleSqr;

	// Token: 0x04000C29 RID: 3113
	private float FarDistanceScaleSqr;

	// Token: 0x04000C2A RID: 3114
	private List<LightOptimizer.LightWrapper> mySpotLights = new List<LightOptimizer.LightWrapper>();

	// Token: 0x04000C2B RID: 3115
	private List<LightOptimizer.LightWrapper> myPointLights = new List<LightOptimizer.LightWrapper>();

	// Token: 0x04000C2C RID: 3116
	private Camera myMainCamera;

	// Token: 0x0200036D RID: 877
	private struct LightWrapper
	{
		// Token: 0x060017ED RID: 6125 RVA: 0x0006CACE File Offset: 0x0006ACCE
		public LightWrapper(Light aLight)
		{
			this.myLight = aLight;
			this.myOriginalRange = aLight.range;
			this.myOriginalSpotSizeScale = aLight.spotAngle;
			this.myPosition = aLight.transform.position;
			this.previousScale = -1f;
		}

		// Token: 0x0400137E RID: 4990
		public Light myLight;

		// Token: 0x0400137F RID: 4991
		public Vector3 myPosition;

		// Token: 0x04001380 RID: 4992
		public float myOriginalRange;

		// Token: 0x04001381 RID: 4993
		public float myOriginalSpotSizeScale;

		// Token: 0x04001382 RID: 4994
		public float previousScale;
	}
}
