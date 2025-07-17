using System;
using Team17.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200015E RID: 350
public class SteamToggler : MonoBehaviour
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x0004A948 File Offset: 0x00048B48
	private void Start()
	{
		this.particleSystem = base.gameObject.GetComponent<ParticleSystem>();
		this.em = this.particleSystem.emission;
		this.controls = Object.FindObjectOfType<HvacControls>();
		if (this.controls != null)
		{
			this.SteamOnOff();
			this.controls.TemperatureUpdate.AddListener(new UnityAction(this.SteamOnOff));
			return;
		}
		T17Debug.LogError("No HvacControls script in scene to direct steam emission");
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0004A9C0 File Offset: 0x00048BC0
	private void SteamOnOff()
	{
		bool flag;
		if (this.controls.Temperature != 0)
		{
			flag = false;
			this.em.enabled = false;
		}
		else
		{
			flag = true;
			this.em.enabled = true;
		}
		if (this.interactable != null)
		{
			this.interactable.SetActive(flag);
			return;
		}
		T17Debug.LogError("[SteamToggler] '" + base.name + " missing interactable reference");
	}

	// Token: 0x04000B86 RID: 2950
	private ParticleSystem particleSystem;

	// Token: 0x04000B87 RID: 2951
	private ParticleSystem.EmissionModule em;

	// Token: 0x04000B88 RID: 2952
	private HvacControls controls;

	// Token: 0x04000B89 RID: 2953
	public GameObject interactable;
}
