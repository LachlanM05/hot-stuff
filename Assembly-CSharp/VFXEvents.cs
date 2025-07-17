using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

// Token: 0x0200015A RID: 346
public class VFXEvents : MonoBehaviour
{
	// Token: 0x06000CCF RID: 3279 RVA: 0x0004A580 File Offset: 0x00048780
	private void Awake()
	{
		VFXEvents.Instance = this;
		ScriptableRenderer renderer = (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(0);
		List<ScriptableRendererFeature> list = typeof(ScriptableRenderer).GetProperty("rendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(renderer) as List<ScriptableRendererFeature>;
		this.outlineFeature = list.Find((ScriptableRendererFeature x) => x.name == "Outline" && x.GetType() == typeof(RenderObjects)) as RenderObjects;
		this.ResetOutline();
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0004A5FC File Offset: 0x000487FC
	public void SpawnVisualEffect(Vector3 position, VFXEvents.VisualEffectType type = VFXEvents.VisualEffectType.NONE)
	{
		VisualEffect visualEffect = Object.Instantiate<VisualEffect>(this.VFXDictionary[type]);
		visualEffect.transform.position = position;
		visualEffect.gameObject.SetActive(true);
		visualEffect.Play();
		if (type == VFXEvents.VisualEffectType.GLEAM)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.ui_emote_good, AUDIO_TYPE.SFX, false, false, 0.1f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		if (type == VFXEvents.VisualEffectType.DUST)
		{
			Singleton<AudioManager>.Instance.PlayTrack(SFXBank.Instance.vfx_poof, AUDIO_TYPE.SFX, false, false, 0.1f, true, 1f, base.gameObject, false, SFX_SUBGROUP.FOLEY, false);
		}
		base.StartCoroutine(this.WaitForEnd(visualEffect));
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x0004A6A5 File Offset: 0x000488A5
	private IEnumerator WaitForEnd(VisualEffect effect)
	{
		yield return new WaitForSeconds(2f);
		yield return new WaitUntil(() => !effect.HasAnySystemAwake());
		Object.Destroy(effect.gameObject);
		yield break;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0004A6B4 File Offset: 0x000488B4
	public void UpdateOutline(float normalizedAmount)
	{
		this.outlineFeature.settings.overrideMaterial.SetFloat("_DATEVIATORS", 1f);
		this.outlineFeature.settings.overrideMaterial.SetFloat("_Dateviator_Strength", normalizedAmount);
		this.outlineFeature.settings.overrideMaterial.SetFloat("_Sheen_Strength", normalizedAmount);
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0004A716 File Offset: 0x00048916
	public void ResetOutline()
	{
		this.outlineFeature.settings.overrideMaterial.SetFloat("_Dateviator_Strength", 0f);
		this.outlineFeature.settings.overrideMaterial.SetFloat("_Sheen_Strength", 0f);
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0004A756 File Offset: 0x00048956
	private void OnDisable()
	{
		this.ResetOutline();
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x0004A760 File Offset: 0x00048960
	public void SetWind(float windAmount)
	{
		Material[] array = this.wind_mats;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat("_Wind_Strength", windAmount);
		}
	}

	// Token: 0x04000B79 RID: 2937
	public static VFXEvents Instance;

	// Token: 0x04000B7A RID: 2938
	public SerializableMap<VFXEvents.VisualEffectType, VisualEffect> VFXDictionary = new SerializableMap<VFXEvents.VisualEffectType, VisualEffect>();

	// Token: 0x04000B7B RID: 2939
	public Material[] wind_mats;

	// Token: 0x04000B7C RID: 2940
	private RenderObjects outlineFeature;

	// Token: 0x02000360 RID: 864
	public enum VisualEffectType
	{
		// Token: 0x0400134B RID: 4939
		NONE,
		// Token: 0x0400134C RID: 4940
		GLEAM,
		// Token: 0x0400134D RID: 4941
		EVENT,
		// Token: 0x0400134E RID: 4942
		DUST
	}
}
