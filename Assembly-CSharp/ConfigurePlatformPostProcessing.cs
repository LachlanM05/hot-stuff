using System;
using T17.Flow;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x02000006 RID: 6
public class ConfigurePlatformPostProcessing : MonoBehaviour
{
	// Token: 0x06000007 RID: 7 RVA: 0x000020B5 File Offset: 0x000002B5
	private bool IsEnabledOnPlatform()
	{
		return this.PC;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000020C0 File Offset: 0x000002C0
	private void ConfigurePostProcessing()
	{
		if (!this.IsEnabledOnPlatform())
		{
			Volume[] array = Object.FindObjectsOfType<Volume>(true);
			for (int i = array.Length - 1; i >= 0; i--)
			{
				Tonemapping tonemapping;
				if (array[i].profile.TryGet<Tonemapping>(out tonemapping))
				{
					tonemapping.active = false;
				}
				Vignette vignette;
				if (array[i].profile.TryGet<Vignette>(out vignette))
				{
					vignette.active = false;
				}
				DepthOfField depthOfField;
				if (array[i].profile.TryGet<DepthOfField>(out depthOfField))
				{
					depthOfField.active = false;
				}
			}
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002135 File Offset: 0x00000335
	private void OnEnable()
	{
		SceneTransitionManager.sceneActivated += this.OnSceneActviated;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002148 File Offset: 0x00000348
	private void OnDisable()
	{
		SceneTransitionManager.sceneActivated -= this.OnSceneActviated;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000215B File Offset: 0x0000035B
	private void OnSceneActviated(string sceneName)
	{
		this.ConfigurePostProcessing();
	}

	// Token: 0x04000006 RID: 6
	public bool PC = true;

	// Token: 0x04000007 RID: 7
	public bool PS5 = true;

	// Token: 0x04000008 RID: 8
	public bool XSX = true;

	// Token: 0x04000009 RID: 9
	public bool SWITCH = true;
}
