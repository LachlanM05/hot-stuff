using System;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x020000A3 RID: 163
public class Minibar : MonoBehaviour
{
	// Token: 0x0600055C RID: 1372 RVA: 0x0001F52E File Offset: 0x0001D72E
	private void Awake()
	{
		this.pour1.Stop();
		this.pour2.Stop();
		this.pour3.Stop();
		this.drain.Stop();
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0001F55C File Offset: 0x0001D75C
	public void StartPour1()
	{
		this.pour1.Play();
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0001F569 File Offset: 0x0001D769
	public void StartPour2()
	{
		this.pour2.Play();
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0001F576 File Offset: 0x0001D776
	public void StopPour1()
	{
		this.pour1.Stop();
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0001F583 File Offset: 0x0001D783
	public void StopPour2()
	{
		this.pour2.Stop();
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0001F590 File Offset: 0x0001D790
	public void StartPour3()
	{
		this.pour3.Play();
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001F59D File Offset: 0x0001D79D
	public void StopPour3()
	{
		this.pour3.Stop();
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0001F5AA File Offset: 0x0001D7AA
	public void StartDrain()
	{
		this.drain.Play();
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001F5B7 File Offset: 0x0001D7B7
	public void StopDrain()
	{
		this.drain.Stop();
	}

	// Token: 0x0400053C RID: 1340
	[SerializeField]
	private VisualEffect pour1;

	// Token: 0x0400053D RID: 1341
	[SerializeField]
	private VisualEffect pour2;

	// Token: 0x0400053E RID: 1342
	[SerializeField]
	private VisualEffect pour3;

	// Token: 0x0400053F RID: 1343
	[SerializeField]
	private VisualEffect drain;
}
