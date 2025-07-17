using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class FPSCounter : MonoBehaviour
{
	// Token: 0x06000CF2 RID: 3314 RVA: 0x0004ABE8 File Offset: 0x00048DE8
	private void Awake()
	{
		for (int i = 0; i < this._cacheNumbersAmount; i++)
		{
			this.CachedNumberStrings[i] = i.ToString();
		}
		this._frameRateSamples = new int[this._averageFromAmount];
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x0004AC2C File Offset: 0x00048E2C
	private void Update()
	{
		if (Singleton<Save>.Instance.GetTutorialThresholdState(FPSCounter.FPS_COUNTER_INVISIBLE))
		{
			Object.Destroy(base.gameObject);
		}
		int num = (int)Math.Round((double)(1f / Time.smoothDeltaTime));
		this._frameRateSamples[this._averageCounter] = num;
		float num2 = 0f;
		foreach (int num3 in this._frameRateSamples)
		{
			num2 += (float)num3;
		}
		this._currentAveraged = (int)Math.Round((double)(num2 / (float)this._averageFromAmount));
		this._averageCounter = (this._averageCounter + 1) % this._averageFromAmount;
		TMP_Text fpstext = this.FPSText;
		int currentAveraged = this._currentAveraged;
		string text;
		if (currentAveraged >= 0 && this.CachedNumberStrings.Count > currentAveraged && currentAveraged < this._cacheNumbersAmount)
		{
			text = this.CachedNumberStrings[currentAveraged];
		}
		else if (currentAveraged >= this._cacheNumbersAmount)
		{
			text = string.Format("> {0}", this._cacheNumbersAmount);
		}
		else if (currentAveraged < 0)
		{
			text = "< 0";
		}
		else
		{
			text = "?";
		}
		fpstext.text = text;
	}

	// Token: 0x04000BB6 RID: 2998
	public static string FPS_COUNTER_INVISIBLE = "FpsCounterInvisible";

	// Token: 0x04000BB7 RID: 2999
	public TextMeshProUGUI FPSText;

	// Token: 0x04000BB8 RID: 3000
	private Dictionary<int, string> CachedNumberStrings = new Dictionary<int, string>();

	// Token: 0x04000BB9 RID: 3001
	private int[] _frameRateSamples;

	// Token: 0x04000BBA RID: 3002
	private int _cacheNumbersAmount = 300;

	// Token: 0x04000BBB RID: 3003
	private int _averageFromAmount = 60;

	// Token: 0x04000BBC RID: 3004
	private int _averageCounter;

	// Token: 0x04000BBD RID: 3005
	private int _currentAveraged;
}
