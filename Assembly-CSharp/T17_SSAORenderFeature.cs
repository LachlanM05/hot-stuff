using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x02000190 RID: 400
public class T17_SSAORenderFeature : ScriptableRendererFeature
{
	// Token: 0x06000DD0 RID: 3536 RVA: 0x0004D694 File Offset: 0x0004B894
	public override void Create()
	{
		this.ssaoShader = Shader.Find("Team17Shaders/T17_SSAO_Volumetric");
		if (this.ssaoShader == null)
		{
			return;
		}
		this.ssaoMaterial = new Material(this.ssaoShader);
		this.ssaoPass = new T17_SSAORenderFeature.T17_SSAOPass(this.ssaoMaterial, this.settings);
		this.ssaoPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0004D6F8 File Offset: 0x0004B8F8
	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (this.ssaoMaterial != null && renderingData.cameraData.cameraType == CameraType.Game)
		{
			Camera camera = renderingData.cameraData.camera;
			if (camera != null)
			{
				camera.depthTextureMode |= DepthTextureMode.Depth;
			}
			renderer.EnqueuePass(this.ssaoPass);
		}
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x0004D750 File Offset: 0x0004B950
	protected override void Dispose(bool disposing)
	{
		if (this.ssaoPass != null)
		{
			this.ssaoPass.Dispose();
		}
		Object.Destroy(this.ssaoMaterial);
	}

	// Token: 0x04000C5A RID: 3162
	[HideInInspector]
	[SerializeField]
	private Shader ssaoShader;

	// Token: 0x04000C5B RID: 3163
	public T17_SSAORenderFeature.T17_SSAOSettings settings = new T17_SSAORenderFeature.T17_SSAOSettings();

	// Token: 0x04000C5C RID: 3164
	private T17_SSAORenderFeature.T17_SSAOPass ssaoPass;

	// Token: 0x04000C5D RID: 3165
	private Material ssaoMaterial;

	// Token: 0x04000C5E RID: 3166
	private const uint NOISE_GRID_SIZE = 4U;

	// Token: 0x0200037B RID: 891
	[Serializable]
	public class T17_SSAOSettings
	{
		// Token: 0x040013B2 RID: 5042
		[Range(0f, 10f)]
		[Tooltip("Amount of reduction on Depth Buffer before VAO Pass.")]
		public int m_Downsampling = 2;

		// Token: 0x040013B3 RID: 5043
		[Range(0.0001f, 0.02f)]
		[Tooltip("Strength of the volumetric obscurance effect.")]
		public float sphereSize = 0.0001f;

		// Token: 0x040013B4 RID: 5044
		[Range(0.0001f, 1f)]
		[Tooltip("Intensity of Occlusion.")]
		public float occlusionIntensity = 1f;

		// Token: 0x040013B5 RID: 5045
		[Range(1f, 20f)]
		[Tooltip("Distance Divisor.")]
		public float distanceDivisor = 10f;
	}

	// Token: 0x0200037C RID: 892
	private class T17_SSAOPass : ScriptableRenderPass
	{
		// Token: 0x06001804 RID: 6148 RVA: 0x0006D31C File Offset: 0x0006B51C
		public T17_SSAOPass(Material material, T17_SSAORenderFeature.T17_SSAOSettings SSAOSettings)
		{
			this.ssaoMaterial = material;
			base.renderPassEvent = base.renderPassEvent;
			this.ssaoSettings = SSAOSettings;
			this.ssaoSettings.m_Downsampling = Mathf.Clamp(this.ssaoSettings.m_Downsampling, 1, 6);
			this.ssaoLinearDepthTextureDescriptor = new RenderTextureDescriptor(Screen.width / this.ssaoSettings.m_Downsampling, Screen.height / this.ssaoSettings.m_Downsampling, RenderTextureFormat.RFloat);
			this.ssaoVAOTextureDescriptor = new RenderTextureDescriptor(Screen.width / this.ssaoSettings.m_Downsampling, Screen.height / this.ssaoSettings.m_Downsampling, RenderTextureFormat.RFloat);
			this.ssaoHBlurTextureDescriptor = new RenderTextureDescriptor(Screen.width / this.ssaoSettings.m_Downsampling, Screen.height / this.ssaoSettings.m_Downsampling, RenderTextureFormat.RFloat);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0006D40C File Offset: 0x0006B60C
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			this.ssaoLinearDepthTextureDescriptor.width = cameraTextureDescriptor.width / this.ssaoSettings.m_Downsampling;
			this.ssaoLinearDepthTextureDescriptor.height = cameraTextureDescriptor.height / this.ssaoSettings.m_Downsampling;
			this.ssaoVAOTextureDescriptor.width = cameraTextureDescriptor.width / this.ssaoSettings.m_Downsampling;
			this.ssaoVAOTextureDescriptor.height = cameraTextureDescriptor.height / this.ssaoSettings.m_Downsampling;
			this.ssaoHBlurTextureDescriptor.width = cameraTextureDescriptor.width / this.ssaoSettings.m_Downsampling;
			this.ssaoHBlurTextureDescriptor.height = cameraTextureDescriptor.height / this.ssaoSettings.m_Downsampling;
			RenderingUtils.ReAllocateIfNeeded(ref this.ssaoLinearDepthHandle, in this.ssaoLinearDepthTextureDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "");
			RenderingUtils.ReAllocateIfNeeded(ref this.ssaoVAOHandle, in this.ssaoVAOTextureDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "");
			RenderingUtils.ReAllocateIfNeeded(ref this.ssaoHBlurHandle, in this.ssaoHBlurTextureDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "");
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0006D530 File Offset: 0x0006B730
		private void UpdateSSAOSettings(ref RenderingData renderingData)
		{
			if (this.ssaoMaterial == null)
			{
				return;
			}
			if (this.oldSphereSize != this.ssaoSettings.sphereSize)
			{
				this.oldSphereSize = this.ssaoSettings.sphereSize;
				this.GenerateNoiseGrid(this.ssaoSettings.sphereSize);
			}
			this.ssaoMaterial.SetFloat("_OcclusionIntensity", this.ssaoSettings.occlusionIntensity);
			this.ssaoMaterial.SetFloat("_AOSphereSize", this.ssaoSettings.sphereSize);
			this.ssaoMaterial.SetFloat("_DistanceDivisor", this.ssaoSettings.distanceDivisor);
			this.ssaoMaterial.SetTexture("_ScaledCameraDepth", this.ssaoLinearDepthHandle);
			this.ssaoMaterial.SetVector("_NoiseScale", new Vector2((float)this.ssaoVAOTextureDescriptor.width, (float)this.ssaoVAOTextureDescriptor.height));
			Vector2 vector = new Vector2(1f / (float)(renderingData.cameraData.cameraTargetDescriptor.width / this.ssaoSettings.m_Downsampling), 1f / (float)(renderingData.cameraData.cameraTargetDescriptor.height / this.ssaoSettings.m_Downsampling));
			this.ssaoMaterial.SetVector("_TexelOffsetScale", vector);
			this.ssaoMaterial.SetVectorArray("_NoiseArray", this.noiseGrid);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0006D698 File Offset: 0x0006B898
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			RTHandle cameraDepthTargetHandle = renderingData.cameraData.renderer.cameraDepthTargetHandle;
			CommandBuffer commandBuffer = CommandBufferPool.Get("T17_SSAOVolumetric");
			this.UpdateSSAOSettings(ref renderingData);
			base.Blit(commandBuffer, cameraDepthTargetHandle, this.ssaoLinearDepthHandle, this.ssaoMaterial, 0);
			base.Blit(commandBuffer, this.ssaoLinearDepthHandle, this.ssaoVAOHandle, this.ssaoMaterial, 1);
			base.Blit(commandBuffer, this.ssaoVAOHandle, this.ssaoHBlurHandle, this.ssaoMaterial, 2);
			base.Blit(commandBuffer, ref renderingData, this.ssaoHBlurHandle, this.ssaoMaterial, 3);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0006D734 File Offset: 0x0006B934
		public void Dispose()
		{
			Object.Destroy(this.ssaoMaterial);
			if (this.ssaoLinearDepthHandle != null)
			{
				this.ssaoLinearDepthHandle.Release();
			}
			if (this.ssaoVAOHandle != null)
			{
				this.ssaoVAOHandle.Release();
			}
			if (this.ssaoHBlurHandle != null)
			{
				this.ssaoHBlurHandle.Release();
			}
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0006D788 File Offset: 0x0006B988
		private void GenerateNoiseGrid(float sphereSize)
		{
			float num = 0.3926991f;
			uint num2 = 0U;
			for (uint num3 = 0U; num3 < 4U; num3 += 1U)
			{
				for (uint num4 = 0U; num4 < 4U; num4 += 1U)
				{
					uint num5 = num3 + num4 * 4U;
					float num6 = num * num2;
					Vector4 vector;
					vector.x = Mathf.Cos(num6);
					vector.y = Mathf.Sin(num6);
					vector.z = 0f;
					vector.w = 0f;
					vector.Normalize();
					this.noiseGrid[(int)num5] = vector;
					num2 += 1U;
				}
			}
			for (uint num7 = 0U; num7 < 3U; num7 += 1U)
			{
				List<Vector4> list = new List<Vector4>(this.noiseGrid);
				for (uint num8 = 0U; num8 < 16U; num8 += 1U)
				{
					int num9 = Random.Range(0, list.Count - 1);
					this.noiseGrid[(int)num8] = list[num9];
					list.RemoveAt(num9);
				}
			}
		}

		// Token: 0x040013B6 RID: 5046
		private Material ssaoMaterial;

		// Token: 0x040013B7 RID: 5047
		private T17_SSAORenderFeature.T17_SSAOSettings ssaoSettings;

		// Token: 0x040013B8 RID: 5048
		private RenderTextureDescriptor ssaoLinearDepthTextureDescriptor;

		// Token: 0x040013B9 RID: 5049
		private RenderTextureDescriptor ssaoVAOTextureDescriptor;

		// Token: 0x040013BA RID: 5050
		private RenderTextureDescriptor ssaoHBlurTextureDescriptor;

		// Token: 0x040013BB RID: 5051
		private RTHandle ssaoLinearDepthHandle;

		// Token: 0x040013BC RID: 5052
		private RTHandle ssaoVAOHandle;

		// Token: 0x040013BD RID: 5053
		private RTHandle ssaoHBlurHandle;

		// Token: 0x040013BE RID: 5054
		private Vector4[] noiseGrid = new Vector4[16];

		// Token: 0x040013BF RID: 5055
		private float oldSphereSize = -1f;
	}
}
