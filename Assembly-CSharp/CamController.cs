using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class CamController : MonoBehaviour
{
	// Token: 0x06000120 RID: 288 RVA: 0x00008C2A File Offset: 0x00006E2A
	private void Start()
	{
		this.virtualCameras = base.transform.parent.GetComponentsInChildren<CinemachineVirtualCamera>().ToList<CinemachineVirtualCamera>();
		this.cam = base.GetComponentInChildren<CinemachineVirtualCamera>();
		base.GetComponent<MeshRenderer>().enabled = false;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00008C60 File Offset: 0x00006E60
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in this.virtualCameras.Where((CinemachineVirtualCamera x) => x.gameObject != base.gameObject))
			{
				cinemachineVirtualCamera.Priority = 0;
			}
			this.cam.Priority = 100;
		}
	}

	// Token: 0x0400012F RID: 303
	public List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();

	// Token: 0x04000130 RID: 304
	private CinemachineVirtualCamera cam;
}
