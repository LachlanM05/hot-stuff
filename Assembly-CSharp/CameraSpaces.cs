using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class CameraSpaces : Singleton<CameraSpaces>
{
	// Token: 0x0600012A RID: 298 RVA: 0x00008E88 File Offset: 0x00007088
	public void Start()
	{
		this.isStarted = true;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00008E91 File Offset: 0x00007091
	private void LateUpdate()
	{
		this.RefreshCamRegions();
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00008E9C File Offset: 0x0000709C
	public void RefreshCamRegions()
	{
		if (this.isStarted)
		{
			if (this.player == null)
			{
				this.player = BetterPlayerControl.Instance.gameObject;
				return;
			}
			int num = 0;
			if (Singleton<InteractableManager>.Instance.GetActiveObj())
			{
				num = this.CameraLoc(Singleton<InteractableManager>.Instance.GetActiveObj().transform);
			}
			if (num <= 0)
			{
				num = this.CameraLoc(this.player.transform);
			}
			this.currentarea = num;
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00008F18 File Offset: 0x00007118
	public Transform CurrentDirTransform()
	{
		if (this.zones[this.currentarea].direction != null)
		{
			return this.zones[this.currentarea].direction;
		}
		return this.zones[this.currentarea].Camera.transform;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00008F78 File Offset: 0x00007178
	private int CameraLoc(Transform t)
	{
		int num = -1;
		foreach (triggerzone triggerzone in this.zones)
		{
			Bounds bounds = new Bounds(triggerzone.Position, triggerzone.Scale);
			if (bounds.Contains(t.position))
			{
				num = this.zones.IndexOf(triggerzone);
			}
			Collider collider;
			if (t.TryGetComponent<Collider>(out collider) && bounds.Contains(collider.ClosestPointOnBounds(triggerzone.Position)))
			{
				num = this.zones.IndexOf(triggerzone);
			}
		}
		if (num == -1)
		{
			return this.currentarea;
		}
		return num;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00009030 File Offset: 0x00007230
	public triggerzone PlayerZone()
	{
		if (this.player == null)
		{
			this.player = BetterPlayerControl.Instance.gameObject;
		}
		int num = this.CameraLoc(this.player.transform);
		return this.zones[num];
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000907C File Offset: 0x0000727C
	public triggerzone GetCameraByRoom(string room)
	{
		foreach (triggerzone triggerzone in this.zones)
		{
			if (triggerzone.Name.ToLowerInvariant() == room.ToLowerInvariant())
			{
				return triggerzone;
			}
		}
		return this.zones[0];
	}

	// Token: 0x06000131 RID: 305 RVA: 0x000090F4 File Offset: 0x000072F4
	private void OnValidate()
	{
		foreach (triggerzone triggerzone in this.zones)
		{
			if (triggerzone.Position == Vector3.zero)
			{
				triggerzone.Position = triggerzone.Camera.transform.position;
			}
			if (triggerzone.direction == null && triggerzone.Camera != null)
			{
				triggerzone.direction = triggerzone.Camera.transform;
			}
		}
	}

	// Token: 0x04000136 RID: 310
	public GameObject player;

	// Token: 0x04000137 RID: 311
	public List<triggerzone> zones;

	// Token: 0x04000138 RID: 312
	public int currentarea = -1;

	// Token: 0x04000139 RID: 313
	public int showonlybefore;

	// Token: 0x0400013A RID: 314
	public int showonlyafter;
}
