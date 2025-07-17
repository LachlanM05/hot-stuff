using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B1 RID: 177
[Serializable]
public class LightingSetup
{
	// Token: 0x0400055B RID: 1371
	public float Brightness;

	// Token: 0x0400055C RID: 1372
	public float XAngle;

	// Token: 0x0400055D RID: 1373
	public Color LightColor;

	// Token: 0x0400055E RID: 1374
	public Material Skybox;

	// Token: 0x0400055F RID: 1375
	public LightStates ToggleLights;

	// Token: 0x04000560 RID: 1376
	public GameObject AdditionalLights;

	// Token: 0x04000561 RID: 1377
	public bool OpenDoors;

	// Token: 0x04000562 RID: 1378
	public LightingProfile LightingProfile;

	// Token: 0x04000563 RID: 1379
	public GameObject MainLightGameObject;

	// Token: 0x04000564 RID: 1380
	public List<Lights_Inter> InteractableLights;
}
