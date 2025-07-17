using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class Tresta_Inter : Interactable
{
	// Token: 0x0600053A RID: 1338 RVA: 0x0001EC03 File Offset: 0x0001CE03
	public override string noderequired()
	{
		return "tresta_test";
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0001EC0A File Offset: 0x0001CE0A
	public override void Interact()
	{
		this.on = !this.on;
		if (!this.on)
		{
			this.ps.Stop();
			return;
		}
		this.ps.Play();
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0001EC3A File Offset: 0x0001CE3A
	public void Start()
	{
		this.startpos = base.transform.position;
		this.startrot = base.transform.eulerAngles;
		this.targetpos = this.startpos;
		this.targetrot = this.startrot;
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x0001EC78 File Offset: 0x0001CE78
	public void Update()
	{
		if (this.on)
		{
			this.targetpos = this.startpos + Vector3.up * Mathf.Sin(Time.time / 10f) / 4f;
			this.targetrot += Vector3.up * Time.deltaTime;
		}
		else
		{
			this.targetpos = this.startpos;
			this.targetrot = this.startrot;
		}
		this.visual.transform.position = Vector3.Lerp(this.visual.transform.position, this.targetpos, Time.deltaTime * 50f);
		this.visual.transform.eulerAngles = Vector3.Lerp(this.visual.transform.eulerAngles, this.targetrot, Time.deltaTime * 50f);
	}

	// Token: 0x04000522 RID: 1314
	private bool on;

	// Token: 0x04000523 RID: 1315
	public GameObject visual;

	// Token: 0x04000524 RID: 1316
	public ParticleSystem ps;

	// Token: 0x04000525 RID: 1317
	private Vector3 startpos;

	// Token: 0x04000526 RID: 1318
	private Vector3 startrot;

	// Token: 0x04000527 RID: 1319
	private Vector3 targetpos;

	// Token: 0x04000528 RID: 1320
	private Vector3 targetrot;
}
