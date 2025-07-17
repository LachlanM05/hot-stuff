using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000025 RID: 37
public class AssetCleaner : MonoBehaviour, IReloadHandler
{
	// Token: 0x060000B5 RID: 181 RVA: 0x00004958 File Offset: 0x00002B58
	private void Start()
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		if (this.CleanMaterial == null || this.DirtyMaterial == null)
		{
			this.IsActive = false;
			Object.Destroy(this);
			return;
		}
		if (!this.LinkedInteractable)
		{
			this.LinkedInteractable = base.GetComponent<InteractableObj>();
		}
		if (this.LinkedInteractable == null)
		{
			return;
		}
		if (this.CleanMesh != null && this.DirtyMesh != null)
		{
			this._msh = this.LinkedInteractable.gameObject.GetComponent<MeshFilter>();
			if (this._msh == null)
			{
				return;
			}
			this._swapMesh = true;
			this.cleanMeshScale = base.transform.localScale;
			this.cleanMeshPosition = base.transform.localPosition;
			this.cleanMeshRotation = base.transform.localRotation;
			if (Vector3.Distance(this.messyMeshScale, new Vector3(0f, 0f, 0f)) < 0.1f)
			{
				this.messyMeshScale = this.cleanMeshScale;
			}
			if (Vector3.Distance(this.messyMeshPosition, new Vector3(0f, 0f, 0f)) < 0.1f)
			{
				this.messyMeshPosition = this.cleanMeshPosition;
			}
			if (this.messyMeshRotation.Equals(new Quaternion(0f, 0f, 0f, 0f)))
			{
				this.messyMeshRotation = this.cleanMeshRotation;
			}
		}
		this._rdr = this.LinkedInteractable.gameObject.GetComponent<Renderer>();
		if (this._rdr == null)
		{
			this._rdr = base.GetComponent<Renderer>();
			this._rdr == null;
		}
		if (this._rdr)
		{
			this.MakeDirty();
		}
		if (this.LinkedInteractable != null && this.LinkedInteractable.CleanEvent != null)
		{
			this.LinkedInteractable.CleanEvent.AddListener(new UnityAction(this.MakeClean));
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00004B60 File Offset: 0x00002D60
	public void MakeClean()
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		if (!base.TryGetComponent<Renderer>(out this._rdr))
		{
			return;
		}
		if (this.IsActive && this._rdr != null)
		{
			if (this._rdr != null)
			{
				Material[] materials = this._rdr.materials;
				materials[this.MeshIndex] = this.CleanMaterial;
				this._rdr.SetMaterials(materials.ToList<Material>());
			}
			if (this._swapMesh)
			{
				if (this._msh == null)
				{
					this._msh.mesh = this.CleanMesh;
				}
				base.transform.localScale = this.cleanMeshScale;
				base.transform.localRotation = this.cleanMeshRotation;
				base.transform.localPosition = this.cleanMeshPosition;
			}
			this._isClean = true;
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00004C3C File Offset: 0x00002E3C
	public void MakeDirty()
	{
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		if (this.IsActive)
		{
			if (this._rdr != null)
			{
				Material[] materials = this._rdr.materials;
				materials[this.MeshIndex] = this.DirtyMaterial;
				this._rdr.SetMaterials(materials.ToList<Material>());
			}
			if (this._swapMesh)
			{
				if (this._msh == null)
				{
					this._msh.mesh = this.DirtyMesh;
				}
				base.transform.localScale = this.messyMeshScale;
				base.transform.localRotation = this.messyMeshRotation;
				base.transform.localPosition = this.messyMeshPosition;
			}
			this._isClean = false;
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00004CF6 File Offset: 0x00002EF6
	public void ToggleClean()
	{
		if (this._isClean)
		{
			this.MakeDirty();
			return;
		}
		this.MakeClean();
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00004D0D File Offset: 0x00002F0D
	private void OnValidate()
	{
		if (!this.LinkedInteractable)
		{
			this.LinkedInteractable = base.GetComponent<InteractableObj>();
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00004D28 File Offset: 0x00002F28
	public void LoadState()
	{
		this.isLoadingIn = true;
		if (this.CleanMaterial == null || this.DirtyMaterial == null)
		{
			Object.Destroy(this);
			return;
		}
		if (DeluxeEditionController.IS_DEMO_EDITION)
		{
			return;
		}
		InteractableObj interactableObj = this.LinkedInteractable;
		if (interactableObj == null)
		{
			interactableObj = base.GetComponent<InteractableObj>();
		}
		if (interactableObj != null)
		{
			string text = interactableObj.inkFileName.Trim();
			if (text != null && text.Contains("."))
			{
				text = text.Substring(0, text.IndexOf("."));
			}
			string text2 = "";
			Singleton<Save>.Instance.TryGetInternalName(text, out text2);
			RelationshipStatus dateStatus = Singleton<Save>.Instance.GetDateStatus(text2);
			if (dateStatus == RelationshipStatus.Friend || dateStatus == RelationshipStatus.Love)
			{
				this.MakeClean();
			}
			else
			{
				this.MakeDirty();
			}
		}
		this.isLoadingIn = false;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00004DF3 File Offset: 0x00002FF3
	public void SaveState()
	{
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00004DF5 File Offset: 0x00002FF5
	public int Priority()
	{
		return 1100;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00004DFC File Offset: 0x00002FFC
	public bool isLoading()
	{
		return this.isLoadingIn;
	}

	// Token: 0x040000AD RID: 173
	public Material CleanMaterial;

	// Token: 0x040000AE RID: 174
	public Material DirtyMaterial;

	// Token: 0x040000AF RID: 175
	public Mesh CleanMesh;

	// Token: 0x040000B0 RID: 176
	public Mesh DirtyMesh;

	// Token: 0x040000B1 RID: 177
	public bool IsActive = true;

	// Token: 0x040000B2 RID: 178
	public InteractableObj LinkedInteractable;

	// Token: 0x040000B3 RID: 179
	public int MeshIndex;

	// Token: 0x040000B4 RID: 180
	private Renderer _rdr;

	// Token: 0x040000B5 RID: 181
	private MeshFilter _msh;

	// Token: 0x040000B6 RID: 182
	public bool _isClean;

	// Token: 0x040000B7 RID: 183
	private bool _swapMesh;

	// Token: 0x040000B8 RID: 184
	public Vector3 cleanMeshScale;

	// Token: 0x040000B9 RID: 185
	public Vector3 messyMeshScale;

	// Token: 0x040000BA RID: 186
	public Vector3 cleanMeshPosition;

	// Token: 0x040000BB RID: 187
	public Vector3 messyMeshPosition;

	// Token: 0x040000BC RID: 188
	public Quaternion cleanMeshRotation;

	// Token: 0x040000BD RID: 189
	public Quaternion messyMeshRotation;

	// Token: 0x040000BE RID: 190
	private bool isLoadingIn;
}
