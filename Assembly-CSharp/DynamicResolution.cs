using System;
using T17.Services;
using Team17.Scripts.Platforms.Enums;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200017A RID: 378
public class DynamicResolution : MonoBehaviour
{
	// Token: 0x06000D4A RID: 3402 RVA: 0x0004C134 File Offset: 0x0004A334
	private void Update()
	{
		if (DynamicResolution.SystemEnabled && DynamicResolution.CanUpdate)
		{
			this.GetFrameStats();
			double num = DynamicResolution.DesiredFrameTime - this.GPUFrameTime;
			DynamicResolution.TheHeadroom = num;
			if (num < 0.0)
			{
				this.BelowCount += 2U;
				if (this.BelowCount > 6U)
				{
					this.ScaleRaiseCounter = 0U;
					float num2 = (float)(num / DynamicResolution.DesiredFrameTime);
					DynamicResolution.CurrentScaleFactor = Mathf.Clamp01(DynamicResolution.CurrentScaleFactor + num2);
					this.SetNewScale();
					this.BelowCount = 0U;
					return;
				}
			}
			else if (this.GPUTimeDelta > num)
			{
				this.BelowCount += 1U;
				if (this.BelowCount > 6U)
				{
					this.BelowCount = 0U;
					this.ScaleRaiseCounter = 0U;
					float num3 = (float)(this.GPUTimeDelta / DynamicResolution.DesiredFrameTime);
					DynamicResolution.CurrentScaleFactor = Mathf.Clamp01(DynamicResolution.CurrentScaleFactor - num3);
					this.SetNewScale();
					return;
				}
			}
			else
			{
				this.BelowCount = 0U;
				if (this.GPUTimeDelta < 0.0)
				{
					this.ScaleRaiseCounter += 10U;
				}
				else
				{
					double num4 = DynamicResolution.DesiredFrameTime * 0.06;
					double desiredFrameTime = DynamicResolution.DesiredFrameTime;
					if (num > num4)
					{
						this.ScaleRaiseCounter += 3U;
					}
				}
				if (this.ScaleRaiseCounter >= 120U)
				{
					this.ScaleRaiseCounter = 0U;
					float num5 = (Mathf.Clamp((float)(num / DynamicResolution.DesiredFrameTime), 0.1f, 0.5f) - 0.1f) / 0.4f;
					float num6 = 0.06f * Mathf.Lerp(0.25f, 1f, num5);
					DynamicResolution.CurrentScaleFactor = Mathf.Clamp01(DynamicResolution.CurrentScaleFactor + num6);
					this.SetNewScale();
				}
			}
		}
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0004C2D2 File Offset: 0x0004A4D2
	private void SetNewScale()
	{
		float num = (DynamicResolution.TheFinalScaleFactor = Mathf.Lerp(0.5f, 1f, DynamicResolution.CurrentScaleFactor));
		ScalableBufferManager.ResizeBuffers(num, num);
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x0004C2F4 File Offset: 0x0004A4F4
	private static void ResetScale()
	{
		DynamicResolution.CurrentScaleFactor = 1f;
		ScalableBufferManager.ResizeBuffers(1f, 1f);
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x0004C310 File Offset: 0x0004A510
	private void GetFrameStats()
	{
		if (this.FrameCount < 1U)
		{
			this.FrameCount += 1U;
			return;
		}
		FrameTimingManager.CaptureFrameTimings();
		FrameTimingManager.GetLatestTimings(1U, this.FrameTimings);
		if ((long)this.FrameTimings.Length < 1L)
		{
			return;
		}
		if (this.FrameTimings[0].cpuTimeFrameComplete < this.FrameTimings[0].cpuTimePresentCalled)
		{
			return;
		}
		if (this.GPUFrameTime != 0.0)
		{
			this.GPUTimeDelta = this.FrameTimings[0].gpuFrameTime - this.GPUFrameTime;
		}
		this.GPUFrameTime = this.FrameTimings[0].gpuFrameTime;
		this.CPUFrameTime = this.FrameTimings[0].cpuFrameTime;
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x0004C3D7 File Offset: 0x0004A5D7
	public static void Enable()
	{
		if (DynamicResolution.PlatformSupported)
		{
			DynamicResolution.SystemEnabled = true;
		}
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x0004C3E6 File Offset: 0x0004A5E6
	public static void Disable()
	{
		if (DynamicResolution.PlatformSupported)
		{
			DynamicResolution.SystemEnabled = false;
			DynamicResolution.ResetScale();
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x0004C3FA File Offset: 0x0004A5FA
	public static bool IsSupportedOnPlatform()
	{
		return DynamicResolution.PlatformSupported;
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x0004C401 File Offset: 0x0004A601
	public static bool IsEnabled()
	{
		return DynamicResolution.SystemEnabled;
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x0004C408 File Offset: 0x0004A608
	public static double GetTargetFramerate()
	{
		return DynamicResolution.DesiredFrameRate;
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0004C40F File Offset: 0x0004A60F
	public static void SetTargetFramerate(double target)
	{
		DynamicResolution.DesiredFrameRate = target;
		DynamicResolution.DesiredFrameTime = 1000.0 / target;
		DynamicResolution.ResetScale();
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x0004C42C File Offset: 0x0004A62C
	private void Start()
	{
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Metal && (FrameTimingManager.GetCpuTimerFrequency() == 0UL || FrameTimingManager.GetGpuTimerFrequency() == 0UL))
		{
			DynamicResolution.PlatformSupported = false;
			DynamicResolution.SystemEnabled = false;
		}
		if (!Services.PlatformService.HasFlag(EPlatformFlags.ApplyGraphicalOptimisations))
		{
			DynamicResolution.PlatformSupported = false;
			DynamicResolution.SystemEnabled = false;
		}
		DynamicResolution.CanUpdate = true;
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0004C47B File Offset: 0x0004A67B
	private void OnDestroy()
	{
		if (DynamicResolution.SystemEnabled)
		{
			DynamicResolution.ResetScale();
		}
	}

	// Token: 0x04000C06 RID: 3078
	private static double DesiredFrameRate = 30.0;

	// Token: 0x04000C07 RID: 3079
	private static double DesiredFrameTime = 1000.0 / DynamicResolution.DesiredFrameRate;

	// Token: 0x04000C08 RID: 3080
	private const uint ScaleRaiseCounterLimit = 120U;

	// Token: 0x04000C09 RID: 3081
	private const uint ScaleRaiseCounterSmallIncrement = 3U;

	// Token: 0x04000C0A RID: 3082
	private const uint ScaleRaiseCounterBigIncrement = 10U;

	// Token: 0x04000C0B RID: 3083
	private const double HeadroomThreshold = 0.06;

	// Token: 0x04000C0C RID: 3084
	private const double DeltaThreshold = 0.14;

	// Token: 0x04000C0D RID: 3085
	private const float ScaleIncreaseBasis = 0.06f;

	// Token: 0x04000C0E RID: 3086
	private const float ScaleIncreaseSmallFactor = 0.25f;

	// Token: 0x04000C0F RID: 3087
	private const float ScaleIncreaseBigFactor = 1f;

	// Token: 0x04000C10 RID: 3088
	private const float ScaleHeadroomClampMin = 0.1f;

	// Token: 0x04000C11 RID: 3089
	private const float ScaleHeadroomClampMax = 0.5f;

	// Token: 0x04000C12 RID: 3090
	private const uint NumFrameTimings = 1U;

	// Token: 0x04000C13 RID: 3091
	private const float MinScaleFactor = 0.5f;

	// Token: 0x04000C14 RID: 3092
	private const float MaxScaleFactor = 1f;

	// Token: 0x04000C15 RID: 3093
	private uint FrameCount;

	// Token: 0x04000C16 RID: 3094
	private uint BelowCount;

	// Token: 0x04000C17 RID: 3095
	private FrameTiming[] FrameTimings = new FrameTiming[1];

	// Token: 0x04000C18 RID: 3096
	private double GPUFrameTime;

	// Token: 0x04000C19 RID: 3097
	private double CPUFrameTime;

	// Token: 0x04000C1A RID: 3098
	private double GPUTimeDelta;

	// Token: 0x04000C1B RID: 3099
	private uint ScaleRaiseCounter;

	// Token: 0x04000C1C RID: 3100
	private static float CurrentScaleFactor = 1f;

	// Token: 0x04000C1D RID: 3101
	private static double TheHeadroom = 0.0;

	// Token: 0x04000C1E RID: 3102
	private static float TheFinalScaleFactor = 0f;

	// Token: 0x04000C1F RID: 3103
	private static bool CanUpdate = false;

	// Token: 0x04000C20 RID: 3104
	private static bool SystemEnabled = true;

	// Token: 0x04000C21 RID: 3105
	private static bool PlatformSupported = true;
}
