using System;
using T17.Services;
using Team17.Scripts.Platforms.Enums;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000C1 RID: 193
public class PlanarReflection : MonoBehaviour
{
	// Token: 0x060005EB RID: 1515 RVA: 0x00021794 File Offset: 0x0001F994
	private void Start()
	{
		QualitySettings.activeQualityLevelChanged += this.OnActiveQualityLevelChanged;
		this._planarPropertyId = Shader.PropertyToID("_PLANAR_REFLECTIONS");
		if (this.MirrorMeshRender != null)
		{
			this._planarMaterialIndex = -1;
			Material[] materials = this.MirrorMeshRender.materials;
			int num = 0;
			while (num < materials.Length && this._planarMaterialIndex == -1)
			{
				string[] propertyNames = materials[num].GetPropertyNames(MaterialPropertyType.Float);
				for (int i = 0; i < propertyNames.Length; i++)
				{
					if (propertyNames[i] == "_PLANAR_REFLECTIONS")
					{
						this._planarMaterialIndex = num;
						break;
					}
				}
				num++;
			}
		}
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0002182C File Offset: 0x0001FA2C
	private void OnEnable()
	{
		GraphicsQualityLevel currentGraphicsQualityLevel = Services.PlatformService.GetCurrentGraphicsQualityLevel();
		if (currentGraphicsQualityLevel >= GraphicsQualityLevel.High && this._mirrorCamGO == null)
		{
			this.InitializeMirror();
			RenderPipelineManager.beginCameraRendering += this.PreRender;
		}
		this.SetQuality(currentGraphicsQualityLevel);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x00021874 File Offset: 0x0001FA74
	private void OnDisable()
	{
		this.DestroyMirror();
		RenderPipelineManager.beginCameraRendering -= this.PreRender;
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0002188D File Offset: 0x0001FA8D
	private void OnDestroy()
	{
		QualitySettings.activeQualityLevelChanged -= this.OnActiveQualityLevelChanged;
		this.DestroyMirror();
		RenderPipelineManager.beginCameraRendering -= this.PreRender;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x000218B7 File Offset: 0x0001FAB7
	private void OnBecameVisible()
	{
		this.SetActiveMirror(true);
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x000218C0 File Offset: 0x0001FAC0
	private void OnBecameInvisible()
	{
		this.SetActiveMirror(false);
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x000218C9 File Offset: 0x0001FAC9
	private void PreRender(ScriptableRenderContext context, Camera cam)
	{
		if (cam == this._mirrorCam)
		{
			this.RenderMirror();
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x000218E0 File Offset: 0x0001FAE0
	private void InitializeMirror()
	{
		if (Camera.main == null)
		{
			return;
		}
		this._mainCam = Camera.main;
		if (this._mirrorCamGO == null)
		{
			this._mirrorCamGO = new GameObject(base.name + " [Camera]", new Type[]
			{
				typeof(Camera),
				typeof(Skybox)
			})
			{
				hideFlags = HideFlags.DontSave
			};
			this._mirrorCamGO.SetActive(false);
		}
		this._mirrorCam = this._mirrorCamGO.GetComponent<Camera>();
		this._mirrorCam.CopyFrom(this._mainCam);
		this._mirrorCam.cameraType = CameraType.Reflection;
		this._mirrorCam.usePhysicalProperties = false;
		this._mirrorCam.farClipPlane = this.farClipPlane;
		this._mirrorCam.depth = -99f;
		int num = LayerMask.NameToLayer("Mirror");
		if (num >= 0)
		{
			base.gameObject.layer = num;
			this._mirrorCam.cullingMask &= ~(1 << num);
		}
		this._mirrorCam.useOcclusionCulling = false;
		this.renderTarget.Release();
		this.renderTarget.width = (int)((float)this._mainCam.pixelWidth * this.reflectionQuality);
		this.renderTarget.height = (int)((float)this._mainCam.pixelHeight * this.reflectionQuality);
		this._mirrorCam.targetTexture = this.renderTarget;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00021A59 File Offset: 0x0001FC59
	private void SetActiveMirror(bool active)
	{
		this._isVisible = active;
		if (this._mirrorCamGO != null)
		{
			this._mirrorCamGO.SetActive(active);
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x00021A7C File Offset: 0x0001FC7C
	private void DestroyMirror()
	{
		if (this.renderTarget != null)
		{
			this.renderTarget.Release();
		}
		if (this._mirrorCamGO != null)
		{
			Object.Destroy(this._mirrorCamGO);
			this._mirrorCam = null;
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00021AB8 File Offset: 0x0001FCB8
	private void RenderMirror()
	{
		if (this._mirrorCamGO == null || this._mainCam == null)
		{
			this.InitializeMirror();
			if (this._mainCam == null || this._mirrorCam == null)
			{
				return;
			}
		}
		Vector3 forward = base.transform.forward;
		Vector3 vector = forward * Vector3.Dot(forward, this._mainCam.transform.position - base.transform.position);
		this._mirrorCam.transform.position = this._mainCam.transform.position - 2f * vector;
		Vector3 vector2 = Vector3.Reflect(this._mainCam.transform.forward, forward);
		Vector3 vector3 = Vector3.Reflect(this._mainCam.transform.up, forward);
		this._mirrorCam.transform.LookAt(this._mirrorCam.transform.position + vector2, vector3);
		this._mirrorCam.fieldOfView = this._mainCam.fieldOfView;
		Matrix4x4 worldToCameraMatrix = this._mirrorCam.worldToCameraMatrix;
		Vector3 vector4 = worldToCameraMatrix.MultiplyPoint(base.transform.position);
		Vector3 vector5 = worldToCameraMatrix.MultiplyVector(this.invertClip ? (-forward) : forward);
		Vector4 vector6 = new Vector4(vector5.x, vector5.y, vector5.z, -Vector3.Dot(vector4, vector5));
		this._mirrorCam.projectionMatrix = this._mirrorCam.CalculateObliqueMatrix(vector6);
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00021C54 File Offset: 0x0001FE54
	private void OnActiveQualityLevelChanged(int previousQuality, int currentQuality)
	{
		if (Services.PlatformService.GetCurrentGraphicsQualityLevel() >= GraphicsQualityLevel.High)
		{
			RenderPipelineManager.beginCameraRendering -= this.PreRender;
			RenderPipelineManager.beginCameraRendering += this.PreRender;
			this.InitializeMirror();
			this.SetActiveMirror(this._isVisible);
		}
		else
		{
			this.DestroyMirror();
			RenderPipelineManager.beginCameraRendering -= this.PreRender;
		}
		this.SetQuality((GraphicsQualityLevel)currentQuality);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00021CC4 File Offset: 0x0001FEC4
	private void SetQuality(GraphicsQualityLevel qualityLevel)
	{
		float num;
		switch (qualityLevel)
		{
		case GraphicsQualityLevel.VeryLow:
		case GraphicsQualityLevel.Low:
			MaterialQuality.Low.SetGlobalShaderKeywords();
			num = 0f;
			goto IL_004A;
		case GraphicsQualityLevel.Medium:
			MaterialQuality.Medium.SetGlobalShaderKeywords();
			num = 0f;
			goto IL_004A;
		}
		MaterialQuality.High.SetGlobalShaderKeywords();
		num = 1f;
		IL_004A:
		if (this.MirrorMeshRender != null && this._planarMaterialIndex >= 0)
		{
			this.MirrorMeshRender.materials[this._planarMaterialIndex].SetFloat(this._planarPropertyId, num);
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00021D50 File Offset: 0x0001FF50
	private void CalculateIfVisible()
	{
		Plane[] array = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		this._isVisible = GeometryUtility.TestPlanesAABB(array, this.MirrorMeshRender.bounds);
	}

	// Token: 0x040005AF RID: 1455
	private const string PLANAR_REFLECTION_PROPERTY_NAME = "_PLANAR_REFLECTIONS";

	// Token: 0x040005B0 RID: 1456
	[Range(0.01f, 1f)]
	public float reflectionQuality = 1f;

	// Token: 0x040005B1 RID: 1457
	public float farClipPlane = 1000f;

	// Token: 0x040005B2 RID: 1458
	public RenderTexture renderTarget;

	// Token: 0x040005B3 RID: 1459
	public bool invertClip = true;

	// Token: 0x040005B4 RID: 1460
	private Camera _mainCam;

	// Token: 0x040005B5 RID: 1461
	private GameObject _mirrorCamGO;

	// Token: 0x040005B6 RID: 1462
	private Camera _mirrorCam;

	// Token: 0x040005B7 RID: 1463
	[SerializeField]
	private MeshRenderer MirrorMeshRender;

	// Token: 0x040005B8 RID: 1464
	private int _planarPropertyId;

	// Token: 0x040005B9 RID: 1465
	private int _planarMaterialIndex = -1;

	// Token: 0x040005BA RID: 1466
	private bool _isVisible;
}
