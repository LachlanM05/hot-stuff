using System;
using System.Collections.Generic;
using Team17.Common;
using UnityEngine;

// Token: 0x020000AF RID: 175
[ExecuteAlways]
public class LightingScenarios : MonoBehaviour
{
	// Token: 0x0600058A RID: 1418 RVA: 0x0001FE23 File Offset: 0x0001E023
	private void Awake()
	{
		this.doorScript = this.doorToOpen.GetComponent<Door>();
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0001FE38 File Offset: 0x0001E038
	public void UpdateLighting(int timeOfDay, int previousTimeOfDay)
	{
		if (this.sunLight == null || this.reflectionProbe == null)
		{
			T17Debug.LogError("Set all references in lighting scenario metadata!");
			return;
		}
		if (this.LightProfiles.Count > timeOfDay)
		{
			foreach (LightingSetup lightingSetup in this.LightProfiles)
			{
				lightingSetup.MainLightGameObject.SetActive(false);
			}
			this.updateLighting(timeOfDay, previousTimeOfDay);
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0001FECC File Offset: 0x0001E0CC
	private void toggleLights(List<GameObject> lights, bool on)
	{
		for (int i = 0; i < lights.Count; i++)
		{
			Lights_Inter component = lights[i].GetComponent<Lights_Inter>();
			if (component.lightsOn != on)
			{
				component.loadingIn = true;
				component.Interact();
			}
		}
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0001FF10 File Offset: 0x0001E110
	private void updateLighting(int profile, int previousProfile)
	{
		if (this.LightProfiles.Count > profile)
		{
			LightingSetup lightingSetup = this.LightProfiles[profile];
			if (lightingSetup.OpenDoors)
			{
				this.doorScript.OpenDoor(base.transform.position, 0.5f);
			}
			if (lightingSetup.AdditionalLights != null)
			{
				lightingSetup.AdditionalLights.SetActive(true);
			}
			if (lightingSetup.MainLightGameObject != null)
			{
				lightingSetup.MainLightGameObject.SetActive(true);
			}
			if (lightingSetup.LightingProfile != null)
			{
				lightingSetup.LightingProfile.ApplyLighting(this.sunLight);
			}
			this.currentprofile = profile;
		}
		this.SyncLights(profile, previousProfile);
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0001FFC0 File Offset: 0x0001E1C0
	private void SyncLights(int profile, int previousProfile)
	{
		if (profile == 0)
		{
			int i = 0;
			int count = this.LightProfiles.Count;
			while (i < count)
			{
				int j = 0;
				int count2 = this.LightProfiles[i].InteractableLights.Count;
				while (j < count2)
				{
					Lights_Inter lights_Inter = this.LightProfiles[i].InteractableLights[j];
					if (lights_Inter != null && lights_Inter.lightsOn)
					{
						lights_Inter.Interact(false);
					}
					j++;
				}
				i++;
			}
			return;
		}
		if (profile == previousProfile)
		{
			return;
		}
		int k = 0;
		int count3 = this.LightProfiles[profile].InteractableLights.Count;
		while (k < count3)
		{
			Lights_Inter lights_Inter2 = this.LightProfiles[profile].InteractableLights[k];
			if (k < this.LightProfiles[profile - 1].InteractableLights.Count)
			{
				Lights_Inter lights_Inter3 = this.LightProfiles[previousProfile].InteractableLights[k];
				if (lights_Inter2 != null && lights_Inter3 != null && lights_Inter2.lightsOn != lights_Inter3.lightsOn)
				{
					lights_Inter2.Interact(false);
				}
			}
			k++;
		}
	}

	// Token: 0x0400054F RID: 1359
	public int currentprofile;

	// Token: 0x04000550 RID: 1360
	public List<LightingSetup> LightProfiles;

	// Token: 0x04000551 RID: 1361
	public GameObject sunLight;

	// Token: 0x04000552 RID: 1362
	public GameObject reflectionProbe;

	// Token: 0x04000553 RID: 1363
	public List<GameObject> lightSwitches;

	// Token: 0x04000554 RID: 1364
	public GameObject midnightLights;

	// Token: 0x04000555 RID: 1365
	public GameObject doorToOpen;

	// Token: 0x04000556 RID: 1366
	private Door doorScript;
}
