using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class CullLights : MonoBehaviour
{
	// Token: 0x060005E6 RID: 1510 RVA: 0x000216FF File Offset: 0x0001F8FF
	private void Start()
	{
		this._lights = base.transform.parent.GetComponentsInChildren<Light>(true);
		this.SetLights(false);
		if (this.runtimeMesh != null)
		{
			base.GetComponent<MeshFilter>().mesh = this.runtimeMesh;
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002173E File Offset: 0x0001F93E
	private void OnBecameVisible()
	{
		this.SetLights(true);
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x00021747 File Offset: 0x0001F947
	private void OnBecameInvisible()
	{
		if (!this.ignoreLightsOff)
		{
			this.SetLights(false);
		}
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00021758 File Offset: 0x0001F958
	private void SetLights(bool state)
	{
		if (this._lights == null)
		{
			return;
		}
		Light[] lights = this._lights;
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].enabled = state;
		}
	}

	// Token: 0x040005AC RID: 1452
	private Light[] _lights;

	// Token: 0x040005AD RID: 1453
	[SerializeField]
	private Mesh runtimeMesh;

	// Token: 0x040005AE RID: 1454
	[SerializeField]
	private bool ignoreLightsOff;
}
