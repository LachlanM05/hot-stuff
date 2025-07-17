using System;
using System.Collections.Generic;
using System.Linq;
using Team17.Common;
using UnityEngine;

namespace Team17.Scripts.UI_Components
{
	// Token: 0x02000209 RID: 521
	public class ResolutionProviderService : MonoBehaviour, IService
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00056C3C File Offset: 0x00054E3C
		public IReadOnlyCollection<Resolution> CurrentlySupportedResolutions
		{
			get
			{
				return this._currentlySupportedResolutions;
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00056C44 File Offset: 0x00054E44
		public void OnStartUp()
		{
			this.RefreshSupportedResolutions();
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00056C4C File Offset: 0x00054E4C
		public void OnCleanUp()
		{
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00056C4E File Offset: 0x00054E4E
		public void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00056C50 File Offset: 0x00054E50
		private static string CreateResolutionsStringForLogging(IReadOnlyCollection<Resolution> resolutions)
		{
			return string.Join(", ", resolutions.Select((Resolution x) => string.Format("{0}x{1} @ {2} Hz", x.width, x.height, Mathf.CeilToInt((float)x.refreshRateRatio.value))));
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00056C84 File Offset: 0x00054E84
		public Resolution FindClosestResolutionForCurrentScreen()
		{
			if (this.CurrentlySupportedResolutions.Any((Resolution x) => this.IsSameResolution(Screen.currentResolution, x)))
			{
				return Screen.currentResolution;
			}
			Resolution resolution2 = (from x in this.CurrentlySupportedResolutions
				where x.height <= Screen.height
				where x.width <= Screen.width
				select x into resolution
				orderby resolution.height descending, resolution.width descending
				select resolution).FirstOrDefault<Resolution>();
			if (!resolution2.Equals(default(Resolution)))
			{
				return resolution2;
			}
			return ResolutionProviderService.DefaultFallbackResolution;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00056D70 File Offset: 0x00054F70
		private void OnApplicationFocus(bool hasFocus)
		{
			try
			{
				if (hasFocus)
				{
					this.RefreshSupportedResolutions();
				}
			}
			catch (Exception ex)
			{
				T17Debug.LogError("ResolutionProviderService:OnApplicationFocus(" + hasFocus.ToString() + ") ERROR: " + ex.Message);
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00056DC0 File Offset: 0x00054FC0
		private void RefreshSupportedResolutions()
		{
			this._currentlySupportedResolutions.Clear();
			foreach (Resolution resolution in Screen.resolutions)
			{
				if (this.IsResolutionSupported(resolution))
				{
					this.AddUniqueResolution(resolution);
				}
			}
			if (this.CurrentlySupportedResolutions.Count < 1)
			{
				this.AddUniqueResolution(ResolutionProviderService.DefaultFallbackResolution);
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00056E20 File Offset: 0x00055020
		private void AddUniqueResolution(Resolution resolution)
		{
			if (!this.CurrentlySupportedResolutions.Any((Resolution x) => this.IsSameResolution(x, resolution)))
			{
				this._currentlySupportedResolutions.Add(resolution);
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00056E6C File Offset: 0x0005506C
		private bool IsSameResolution(Resolution resolutionA, Resolution resolutionB)
		{
			int num = Mathf.CeilToInt((float)resolutionA.refreshRateRatio.value);
			int num2 = Mathf.CeilToInt((float)resolutionB.refreshRateRatio.value);
			return resolutionA.width == resolutionB.width && resolutionA.height == resolutionB.height && num == num2;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00056ECC File Offset: 0x000550CC
		public bool IsResolutionSupported(Resolution resolution)
		{
			if (!this.IsRefreshRateSupported(resolution.refreshRateRatio))
			{
				return false;
			}
			float num = (float)resolution.width / (float)resolution.height;
			int i = 0;
			int num2 = ResolutionProviderService.SupportedResolutionRatios.Length;
			while (i < num2)
			{
				float num3 = ResolutionProviderService.SupportedResolutionRatios[i] - num;
				if ((double)num3 > -0.001 && (double)num3 < 0.001)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00056F37 File Offset: 0x00055137
		public bool IsRefreshRateSupported(RefreshRate refreshRate)
		{
			return Mathf.CeilToInt((float)refreshRate.value) >= 50;
		}

		// Token: 0x04000E29 RID: 3625
		private static readonly Resolution DefaultFallbackResolution = new Resolution
		{
			width = 1920,
			height = 1080
		};

		// Token: 0x04000E2A RID: 3626
		private static readonly float[] SupportedResolutionRatios = new float[] { 1.7777778f, 1.6f };

		// Token: 0x04000E2B RID: 3627
		private const int MinRefreshRateInHz = 50;

		// Token: 0x04000E2C RID: 3628
		private readonly List<Resolution> _currentlySupportedResolutions = new List<Resolution>();
	}
}
