using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000030 RID: 48
[Serializable]
public class triggerzone
{
	// Token: 0x0400013B RID: 315
	public string Name;

	// Token: 0x0400013C RID: 316
	public CinemachineVirtualCamera Camera;

	// Token: 0x0400013D RID: 317
	public CinemachineVirtualCamera DialogueCamera;

	// Token: 0x0400013E RID: 318
	public Transform direction;

	// Token: 0x0400013F RID: 319
	public Vector3 Scale;

	// Token: 0x04000140 RID: 320
	public Vector3 Position;

	// Token: 0x04000141 RID: 321
	[HideInInspector]
	public Quaternion Rotation;
}
